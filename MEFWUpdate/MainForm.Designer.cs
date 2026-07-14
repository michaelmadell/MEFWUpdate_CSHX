namespace MEFWUpdate
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) components.Dispose();
                Cleanup();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._pnlConsent = new System.Windows.Forms.Panel();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnAgree = new System.Windows.Forms.Button();
            this._lblQuestion = new System.Windows.Forms.Label();
            this._grpWarn = new System.Windows.Forms.GroupBox();
            this._lblWarnText = new System.Windows.Forms.Label();
            this._lblConsentTitle = new System.Windows.Forms.Label();
            this._pnlUpdate = new System.Windows.Forms.Panel();
            this._btnClose = new System.Windows.Forms.Button();
            this._pbProgress = new System.Windows.Forms.ProgressBar();
            this._lblTimeout = new System.Windows.Forms.Label();
            this._lblStatus = new System.Windows.Forms.Label();
            this._rtbConsole = new System.Windows.Forms.RichTextBox();
            this._lblUpdateSub = new System.Windows.Forms.Label();
            this._lblUpdateTitle = new System.Windows.Forms.Label();
            this._ticker = new System.Windows.Forms.Timer(this.components);
            this._pnlConsent.SuspendLayout();
            this._grpWarn.SuspendLayout();
            this._pnlUpdate.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlConsent
            // 
            this._pnlConsent.Controls.Add(this._btnCancel);
            this._pnlConsent.Controls.Add(this._btnAgree);
            this._pnlConsent.Controls.Add(this._lblQuestion);
            this._pnlConsent.Controls.Add(this._grpWarn);
            this._pnlConsent.Controls.Add(this._lblConsentTitle);
            this._pnlConsent.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlConsent.Location = new System.Drawing.Point(0, 0);
            this._pnlConsent.Name = "_pnlConsent";
            this._pnlConsent.Size = new System.Drawing.Size(616, 497);
            this._pnlConsent.TabIndex = 1;
            // 
            // _btnCancel
            // 
            this._btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._btnCancel.Location = new System.Drawing.Point(366, 322);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(120, 30);
            this._btnCancel.TabIndex = 0;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // _btnAgree
            // 
            this._btnAgree.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._btnAgree.Location = new System.Drawing.Point(90, 322);
            this._btnAgree.Name = "_btnAgree";
            this._btnAgree.Size = new System.Drawing.Size(260, 30);
            this._btnAgree.TabIndex = 1;
            this._btnAgree.Text = "I Understand - Proceed with Update";
            this._btnAgree.Click += new System.EventHandler(this.OnAgreeClick);
            // 
            // _lblQuestion
            // 
            this._lblQuestion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblQuestion.Location = new System.Drawing.Point(20, 290);
            this._lblQuestion.Name = "_lblQuestion";
            this._lblQuestion.Size = new System.Drawing.Size(572, 20);
            this._lblQuestion.TabIndex = 2;
            this._lblQuestion.Text = "Do you understand the risks and wish to proceed with the firmware update?";
            this._lblQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _grpWarn
            // 
            this._grpWarn.Controls.Add(this._lblWarnText);
            this._grpWarn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._grpWarn.Location = new System.Drawing.Point(20, 56);
            this._grpWarn.Name = "_grpWarn";
            this._grpWarn.Size = new System.Drawing.Size(572, 220);
            this._grpWarn.TabIndex = 3;
            this._grpWarn.TabStop = false;
            this._grpWarn.Text = "IMPORTANT - Read before proceeding";
            // 
            // _lblWarnText
            // 
            this._lblWarnText.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblWarnText.ForeColor = System.Drawing.Color.DarkRed;
            this._lblWarnText.Location = new System.Drawing.Point(12, 24);
            this._lblWarnText.Name = "_lblWarnText";
            this._lblWarnText.Size = new System.Drawing.Size(548, 185);
            this._lblWarnText.TabIndex = 0;
            this._lblWarnText.Text = resources.GetString("_lblWarnText.Text");
            // 
            // _lblConsentTitle
            // 
            this._lblConsentTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this._lblConsentTitle.Location = new System.Drawing.Point(12, 16);
            this._lblConsentTitle.Name = "_lblConsentTitle";
            this._lblConsentTitle.Size = new System.Drawing.Size(580, 28);
            this._lblConsentTitle.TabIndex = 4;
            this._lblConsentTitle.Text = "Intel ME Firmware Update";
            this._lblConsentTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _pnlUpdate
            // 
            this._pnlUpdate.Controls.Add(this._btnClose);
            this._pnlUpdate.Controls.Add(this._pbProgress);
            this._pnlUpdate.Controls.Add(this._lblTimeout);
            this._pnlUpdate.Controls.Add(this._lblStatus);
            this._pnlUpdate.Controls.Add(this._rtbConsole);
            this._pnlUpdate.Controls.Add(this._lblUpdateSub);
            this._pnlUpdate.Controls.Add(this._lblUpdateTitle);
            this._pnlUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlUpdate.Location = new System.Drawing.Point(0, 0);
            this._pnlUpdate.Name = "_pnlUpdate";
            this._pnlUpdate.Size = new System.Drawing.Size(616, 497);
            this._pnlUpdate.TabIndex = 0;
            this._pnlUpdate.Visible = false;
            // 
            // _btnClose
            // 
            this._btnClose.Enabled = false;
            this._btnClose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._btnClose.Location = new System.Drawing.Point(516, 457);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(90, 28);
            this._btnClose.TabIndex = 0;
            this._btnClose.Text = "Close";
            this._btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // _pbProgress
            // 
            this._pbProgress.Location = new System.Drawing.Point(10, 433);
            this._pbProgress.MarqueeAnimationSpeed = 25;
            this._pbProgress.Name = "_pbProgress";
            this._pbProgress.Size = new System.Drawing.Size(596, 18);
            this._pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this._pbProgress.TabIndex = 1;
            // 
            // _lblTimeout
            // 
            this._lblTimeout.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblTimeout.ForeColor = System.Drawing.Color.DarkOrange;
            this._lblTimeout.Location = new System.Drawing.Point(458, 407);
            this._lblTimeout.Name = "_lblTimeout";
            this._lblTimeout.Size = new System.Drawing.Size(146, 18);
            this._lblTimeout.TabIndex = 2;
            this._lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _lblStatus
            // 
            this._lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblStatus.Location = new System.Drawing.Point(6, 406);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size(400, 18);
            this._lblStatus.TabIndex = 3;
            this._lblStatus.Text = "Initializing...";
            // 
            // _rtbConsole
            // 
            this._rtbConsole.BackColor = System.Drawing.Color.Black;
            this._rtbConsole.Font = new System.Drawing.Font("Courier New", 9F);
            this._rtbConsole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this._rtbConsole.Location = new System.Drawing.Point(10, 80);
            this._rtbConsole.Name = "_rtbConsole";
            this._rtbConsole.ReadOnly = true;
            this._rtbConsole.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this._rtbConsole.Size = new System.Drawing.Size(596, 324);
            this._rtbConsole.TabIndex = 4;
            this._rtbConsole.Text = "";
            this._rtbConsole.WordWrap = false;
            // 
            // _lblUpdateSub
            // 
            this._lblUpdateSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._lblUpdateSub.ForeColor = System.Drawing.Color.Tomato;
            this._lblUpdateSub.Location = new System.Drawing.Point(10, 44);
            this._lblUpdateSub.Name = "_lblUpdateSub";
            this._lblUpdateSub.Size = new System.Drawing.Size(596, 18);
            this._lblUpdateSub.TabIndex = 5;
            this._lblUpdateSub.Text = "WARNING: Do NOT close, restart, or power off the system until complete.";
            // 
            // _lblUpdateTitle
            // 
            this._lblUpdateTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._lblUpdateTitle.Location = new System.Drawing.Point(10, 10);
            this._lblUpdateTitle.Name = "_lblUpdateTitle";
            this._lblUpdateTitle.Size = new System.Drawing.Size(596, 22);
            this._lblUpdateTitle.TabIndex = 6;
            this._lblUpdateTitle.Text = "Intel ME Firmware Update - In Progress";
            // 
            // _ticker
            // 
            this._ticker.Interval = 1000;
            this._ticker.Tick += new System.EventHandler(this.OnTick);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(616, 497);
            this.Controls.Add(this._pnlUpdate);
            this.Controls.Add(this._pnlConsent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = System.Drawing.SystemIcons.Shield;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(632, 507);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Intel ME Firmware Update";
            this._pnlConsent.ResumeLayout(false);
            this._grpWarn.ResumeLayout(false);
            this._pnlUpdate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel     _pnlConsent;
        private System.Windows.Forms.Label     _lblConsentTitle;
        private System.Windows.Forms.GroupBox  _grpWarn;
        private System.Windows.Forms.Label     _lblWarnText;
        private System.Windows.Forms.Label     _lblQuestion;
        private System.Windows.Forms.Button    _btnAgree;
        private System.Windows.Forms.Button    _btnCancel;
        private System.Windows.Forms.Panel     _pnlUpdate;
        private System.Windows.Forms.Label     _lblUpdateTitle;
        private System.Windows.Forms.Label     _lblUpdateSub;
        private System.Windows.Forms.RichTextBox _rtbConsole;
        private System.Windows.Forms.Label     _lblStatus;
        private System.Windows.Forms.Label     _lblTimeout;
        private System.Windows.Forms.ProgressBar _pbProgress;
        private System.Windows.Forms.Button    _btnClose;
        private System.Windows.Forms.Timer     _ticker;
    }
}
