using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MEFWUpdate
{
    public partial class MainForm : Form
    {
        // ── Win32 P/Invoke ────────────────────────────────────────────────────
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool ShutdownBlockReasonCreate(IntPtr hWnd, string reason);

        [DllImport("user32.dll")]
        private static extern bool ShutdownBlockReasonDestroy(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private const int  WM_CLOSE           = 0x0010;
        private const int  WM_SYSCOMMAND      = 0x0112;
        private const int  WM_QUERYENDSESSION = 0x0011;
        private const int  WM_ENDSESSION      = 0x0016;
        private const uint SC_CLOSE           = 0xF060;
        private const uint MF_BYCOMMAND       = 0x00000000;
        private const uint MF_GRAYED          = 0x00000001;
        private const uint MF_ENABLED         = 0x00000000;
        private const int  TIMEOUT_MINUTES    = 15;

        // ── State ─────────────────────────────────────────────────────────────
        private bool     _updating;
        private bool     _timedOut;
        private DateTime _startTime;
        private int      _lastExitCode;
        private string   _lastError;
        private string   _tempDir;
        private Process  _proc;

        private readonly ConcurrentQueue<(string text, Color color)> _outputQueue
            = new ConcurrentQueue<(string text, Color color)>();

        // ── Constructor ───────────────────────────────────────────────────────
        public MainForm()
        {
            InitializeComponent();

            // Drain the output queue on a 50 ms UI timer so DataReceived floods
            // cannot starve the message pump (which would freeze the countdown
            // timer and progress bar).
            var displayTimer = new System.Windows.Forms.Timer(components) { Interval = 50 };
            displayTimer.Tick += FlushOutputQueue;
            displayTimer.Start();
        }

        // ── WndProc: block close while update runs ────────────────────────────
        protected override void WndProc(ref Message m)
        {
            if (_updating && !_timedOut)
            {
                switch (m.Msg)
                {
                    case WM_CLOSE:
                        ShowBlockWarning();
                        return;

                    case WM_SYSCOMMAND:
                        if (((int)m.WParam & 0xFFF0) == (int)SC_CLOSE)
                        {
                            ShowBlockWarning();
                            return;
                        }
                        break;

                    case WM_QUERYENDSESSION:
                        m.Result = IntPtr.Zero;
                        return;

                    case WM_ENDSESSION:
                        return;
                }
            }
            base.WndProc(ref m);
        }

        // ── Event handlers ────────────────────────────────────────────────────
        private void OnAgreeClick(object sender, EventArgs e)
        {
            _pnlConsent.Visible = false;
            _pnlUpdate.Visible  = true;
            StartUpdate();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Cleanup();
            Application.Exit();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (!_updating) return;

            var remaining = TimeSpan.FromMinutes(TIMEOUT_MINUTES) - (DateTime.Now - _startTime);

            if (remaining.TotalSeconds <= 0)
            {
                _updating = false;
                _timedOut = true;
                _ticker.Stop();
                ShutdownBlockReasonDestroy(Handle);
                SetCloseGrayed(false);
                _btnClose.Enabled     = true;
                _lblTimeout.Text      = "TIMEOUT";
                _lblTimeout.ForeColor = Color.Red;
                AppendText(
                    "\r\n[TIMEOUT] 15-minute safety limit reached.\r\n" +
                    "         The update may still be running. Close at your own risk.\r\n",
                    Color.Red);
                _lblStatus.Text      = "Safety timeout reached";
                _lblStatus.ForeColor = Color.Red;
            }
            else
            {
                _lblTimeout.Text = string.Format("Timeout: {0:mm\\:ss}", remaining);
            }
        }

        // ── Update orchestration ──────────────────────────────────────────────
        private void StartUpdate()
        {
            Print("Intel ME Firmware Update\r\n",         Color.Cyan);
            Print("--------------------------------------------\r\n", Color.Gray);
            Print("Extracting embedded update tools...\r\n",  Color.Yellow);

            if (!ExtractResources())
            {
                _lblStatus.Text      = "Failed to extract embedded resources.";
                _lblStatus.ForeColor = Color.Red;
                _pbProgress.Style    = ProgressBarStyle.Continuous;
                ActivateCloseButton();
                return;
            }

            Print("Tools extracted.\r\n",                Color.FromArgb(0, 160, 0));
            Print("Launching FWUpdLcl64.exe...\r\n\r\n",  Color.Yellow);

            _updating  = true;
            _timedOut  = false;
            _startTime = DateTime.Now;

            ShutdownBlockReasonCreate(Handle,
                "Intel ME Firmware Update in progress - shutting down now may damage the system.");

            SetCloseGrayed(true);
            _ticker.Start();

            _lblStatus.Text      = "Firmware update in progress - DO NOT CLOSE THIS WINDOW";
            _lblStatus.ForeColor = Color.DarkRed;

            new Thread(UpdateThread) { IsBackground = true, Name = "FWUpdateThread" }.Start();
        }

        private bool ExtractResources()
        {
            try
            {
                _tempDir = Path.Combine(
                    Path.GetTempPath(),
                    "MEFWUpdate_" + Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(_tempDir);

                if (!ExtractResource("FWUpdLcl64.exe", Path.Combine(_tempDir, "FWUpdLcl64.exe")))
                {
                    AppendText(
                        "ERROR: Cannot extract FWUpdLcl64.exe from resources.\r\n" +
                        "       Ensure the file was present during build.\r\n", Color.Red);
                    return false;
                }
                if (!ExtractResource("fw_update.bin", Path.Combine(_tempDir, "fw_update.bin")))
                {
                    AppendText(
                        "ERROR: Cannot extract fw_update.bin from resources.\r\n" +
                        "       Ensure the file was present during build.\r\n", Color.Red);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                AppendText("ERROR: " + ex.Message + "\r\n", Color.Red);
                return false;
            }
        }

        private bool ExtractResource(string logicalName, string destPath)
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var src = asm.GetManifestResourceStream(logicalName))
            {
                if (src == null) return false;
                using (var dst = File.Create(destPath))
                    src.CopyTo(dst);
            }
            return true;
        }

        // ── Background thread ─────────────────────────────────────────────────
        private void UpdateThread()
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName               = Path.Combine(_tempDir, "FWUpdLcl64.exe"),
                    Arguments              = string.Format(
                        "-F \"{0}\" -ALLOWSV",
                        Path.Combine(_tempDir, "fw_update.bin")),
                    WorkingDirectory       = _tempDir,
                    UseShellExecute        = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError  = true,
                    RedirectStandardInput  = true,
                    CreateNoWindow         = true,
                    WindowStyle            = ProcessWindowStyle.Hidden
                };

                _proc = new Process { StartInfo = psi };
                _proc.OutputDataReceived += (s, e) =>
                {
                    if (e.Data != null) AppendText(e.Data + "\r\n", Color.FromArgb(180, 255, 180));
                };
                _proc.ErrorDataReceived += (s, e) =>
                {
                    if (e.Data != null) AppendText(e.Data + "\r\n", Color.FromArgb(255, 170, 80));
                };

                _proc.Start();
                _proc.StandardInput.Close();
                _proc.BeginOutputReadLine();
                _proc.BeginErrorReadLine();
                _proc.WaitForExit();

                _lastExitCode = _proc.ExitCode;
                Invoke(new MethodInvoker(OnDone));
            }
            catch (Exception ex)
            {
                _lastError = ex.Message;
                Invoke(new MethodInvoker(OnFailed));
            }
        }

        // ── Completion handlers (UI thread) ───────────────────────────────────
        private void OnDone()
        {
            _updating = false;
            _ticker.Stop();
            ShutdownBlockReasonDestroy(Handle);

            _pbProgress.Style = ProgressBarStyle.Continuous;
            _pbProgress.Value = 100;

            if (_lastExitCode == 0)
            {
                AppendText("\r\n[SUCCESS] Firmware update completed successfully.\r\n",
                    Color.FromArgb(0, 180, 0));
                _lblStatus.Text      = "Update successful - please restart the system";
                _lblStatus.ForeColor = Color.DarkGreen;
                _lblTimeout.Text     = "Done";
                Text = "Intel ME Firmware Update - SUCCESS";
            }
            else
            {
                AppendText(string.Format(
                    "\r\n[FAILED] Firmware update failed. Exit code: {0}\r\n", _lastExitCode),
                    Color.Red);
                _lblStatus.Text      = "Update FAILED - see console output";
                _lblStatus.ForeColor = Color.Red;
                _lblTimeout.Text     = "Done";
                Text = "Intel ME Firmware Update - FAILED";
            }

            ActivateCloseButton();
            Cleanup();

            if (_lastExitCode == 0)
                MessageBox.Show(
                    "Firmware update completed successfully.\r\n\r\n" +
                    "Please restart the system to finalize the update.",
                    "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(
                    string.Format(
                        "Firmware update failed.\r\nExit code: {0}\r\n\r\n" +
                        "Review the console output for details.", _lastExitCode),
                    "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void OnFailed()
        {
            _updating = false;
            _ticker.Stop();
            ShutdownBlockReasonDestroy(Handle);

            _pbProgress.Style    = ProgressBarStyle.Continuous;
            _pbProgress.Value    = 0;
            _lblStatus.Text      = "Error: " + _lastError;
            _lblStatus.ForeColor = Color.Red;
            AppendText("\r\n[ERROR] " + _lastError + "\r\n", Color.Red);

            ActivateCloseButton();
            Cleanup();
        }

        // ── Console output ────────────────────────────────────────────────────
        // Thread-safe: just enqueue. FlushOutputQueue drains on the UI thread
        // every 50 ms, preventing DataReceived floods from starving the pump.
        private void AppendText(string text, Color color) => _outputQueue.Enqueue((text, color));
        private void Print(string text, Color color)      => _outputQueue.Enqueue((text, color));

        private void FlushOutputQueue(object sender, EventArgs e)
        {
            if (_outputQueue.IsEmpty) return;
            _rtbConsole.SuspendLayout();
            while (_outputQueue.TryDequeue(out var item))
            {
                _rtbConsole.SelectionStart  = _rtbConsole.TextLength;
                _rtbConsole.SelectionLength = 0;
                _rtbConsole.SelectionColor  = item.color;
                _rtbConsole.AppendText(item.text);
            }
            _rtbConsole.ScrollToCaret();
            _rtbConsole.ResumeLayout();
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private void SetCloseGrayed(bool grayed)
        {
            var hSys = GetSystemMenu(Handle, false);
            if (hSys != IntPtr.Zero)
                EnableMenuItem(hSys, SC_CLOSE,
                    MF_BYCOMMAND | (grayed ? MF_GRAYED : MF_ENABLED));
        }

        private void ActivateCloseButton()
        {
            SetCloseGrayed(false);
            _btnClose.Enabled = true;
        }

        private void ShowBlockWarning()
        {
            MessageBox.Show(
                "Intel ME Firmware Update is currently in progress.\r\n\r\n" +
                "Closing this application now could corrupt the Management\r\n" +
                "Engine firmware and may render the system unbootable.\r\n\r\n" +
                "Please wait for the update to complete.",
                "Cannot Close - Update In Progress",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void Cleanup()
        {
            try
            {
                if (_tempDir != null && Directory.Exists(_tempDir))
                    Directory.Delete(_tempDir, true);
            }
            catch { }
            _tempDir = null;
        }
    }
}
