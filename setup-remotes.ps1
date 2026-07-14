# Run once after cloning to configure all remotes
git remote add Github git@github.com:michaelmadell/MEFWUpdate_CSHX.git
git remote add Bitbucket git@bitbucket.org:ahkengteam/mefwupdate_cshx.git
git remote add all git@bitbucket.org:ahkengteam/mefwupdate_cshx.git
git remote set-url --add --push all git@bitbucket.org:ahkengteam/mefwupdate_cshx.git
git remote set-url --add --push all git@github.com:michaelmadell/MEFWUpdate_CSHX.git
Write-Host "Remotes configured:"
git remote -v