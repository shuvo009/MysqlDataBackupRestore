using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Samples;
using AlbatrossVaribales;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using MysqlDataBackup;
using System.ComponentModel;
using MysqlDataBackupRestore;
using ProcestaEncryptAndDescript.FileOperation;
using CompressAndDeCompress;
using InternetAccessories;
using System.IO;
using MysqlDataBackupRestore.Class;
namespace MysqlDataBackupRestore
{
    public partial class MainWindow : Window
    {
        #region BackgroundWorker
        BackgroundWorker databaseBackupWorker = new BackgroundWorker();
        BackgroundWorker databaseRestoreWorker = new BackgroundWorker();
        #endregion

        #region Private Varibale

        DispatcherTimer startUpChecker = new DispatcherTimer();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            startUpChecker.Tick += new EventHandler(StatUpCheckerTrick);
            startUpChecker.Interval = new TimeSpan(0, 0, 1);
            startUpChecker.Start();
            ShowAutoBackupOnOff();
            databaseBackupWorker.WorkerReportsProgress = true;
            this.databaseRestoreWorker.WorkerReportsProgress=true;
            databaseBackupWorker.DoWork += new DoWorkEventHandler(DatabaseBackupDoWorker);
            databaseBackupWorker.ProgressChanged += new ProgressChangedEventHandler(DatabaseBackupProcessChanged);
            databaseBackupWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DatabaseBackupRunWorkCompleatd);
            databaseRestoreWorker.DoWork += new DoWorkEventHandler(DatabaseRestoreDowWorker);
            databaseRestoreWorker.ProgressChanged += new ProgressChangedEventHandler(DatabaseRestoreProcessChanged);
            databaseRestoreWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DatabaseRestoreRunWorkCompleated);
        }

        #region Windows Event
        //Window Resize Event
        private void WindnowSizeChangeNotifyIcon(object sender, System.EventArgs e)
        {
                if (this.WindowState.Equals(WindowState.Minimized))
                {
                    WindowMinimizeTry(VariableClass.ERROR_MESSAGES[0,1]);
                    this.ShowInTaskbar = false;
                }
                else if (this.WindowState.Equals(WindowState.Normal))
                {
                    this.ShowInTaskbar = true;
                }
                else
                {
                    return;
                }
        }
        //Window Load Event
        private void AlbatrossMainWindoLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VariableClass.START_UP_FLAGE)
            {
                this.WindowState = WindowState.Minimized;
                WindowMinimizeTry(VariableClass.ERROR_MESSAGES[0, 2]);
                this.ShowInTaskbar = false;
            }
        }
        #endregion

        #region Albatross Notify Icon
        //Double Click On Notify Icon
        private void AlbatrossDoubleClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.WindowState.Equals(WindowState.Minimized))
            {
                this.WindowState = WindowState.Normal;
                this.ShowInTaskbar = true;
            }
            else
            {
                WindowMinimizeTry(VariableClass.ERROR_MESSAGES[0,1]);
                this.WindowState = WindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }
        //Menu item Exit Click
        private void MenuItemAlbatrossExit(object sender, System.Windows.RoutedEventArgs e)
        {
            VariableClass.CLOSE_ON_BUTTON = false;
            Application.Current.Shutdown();
        }
        //Menu item Open Click 
        private void MenuItemAlbatrossOpen(object sender, System.Windows.RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.ShowInTaskbar = true;
        }
        #endregion

        #region Button Event
        //Backup Click
        private void TabAutoBackupClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.HideAllPanel();
            this.PanelAutoBackUp.Visibility = Visibility.Visible;
        }
        //Restore Click
        private void ButtonRestoreClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.HideAllPanel();
            this.PanelRestore.Visibility = Visibility.Visible;
        }  
        // Setting Button Click
        private void ButtonSettingClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ButtonSetting.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            new Setting().ShowDialog();
            this.ButtonSetting.IsEnabled = true;
            this.Cursor = Cursors.Arrow;
        }

        #region Panel AutoBackup
        //Backup start Click
        private void AutoBackUpButtonStartClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.AutoBackUpButtonStart.IsEnabled = false;
            this.DatabaseBackupProgressBar.Value = 1;
            this.databaseBackupWorker.RunWorkerAsync();
        }
        #endregion

        #region Panel Restore
        //Brows Click
        private void RestoreButtonBrowsClick(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog fileOpenDialogBox = new OpenFileDialog();
            fileOpenDialogBox.Filter = "Albatross Backup File (*.gz)|*.gz";
            fileOpenDialogBox.FilterIndex = 2;
            fileOpenDialogBox.RestoreDirectory = true;
            if ((bool)fileOpenDialogBox.ShowDialog())
            {
                FileInfo selectFileInformation = new FileInfo(fileOpenDialogBox.FileName);
                this.RestoreLabelFilePath.Text = selectFileInformation.FullName;
                this.RestoreLabelFileName.Text = selectFileInformation.Name;
                this.RestoreLabelCreateDate.Text = selectFileInformation.CreationTime.ToString();
                this.RestoreLabelReadOnly_.Text = selectFileInformation.IsReadOnly.ToString();
            } 
        } 
        //Restore Click
        private void RestoreButtonStartCLick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (this.RestoreLabelFilePath.Text.Equals(string.Empty))
                {
                    this.RestoreButtonBrowsClick(sender, e);
                }
                this.RestoreButtonStart.IsEnabled = false;
                String[] databaseRestoreWorkerArg = new String[2];
                databaseRestoreWorkerArg[0] = new FileInfo(this.RestoreLabelFilePath.Text).Name;
                databaseRestoreWorkerArg[1] = this.RestoreLabelFilePath.Text;
                this.restoreProgressBar.Value = 1;
                databaseRestoreWorker.RunWorkerAsync(databaseRestoreWorkerArg);
            }
            catch
            {
                this.RestoreButtonStart.IsEnabled = true;
            }
        } 
        #endregion
        #endregion

        #region Background Worker
        #region Backup
        //Backup DoWork
        private void DatabaseBackupDoWorker(object sender, DoWorkEventArgs e)
        {
            string copyFileName = string.Empty;
            databaseBackupWorker.ReportProgress(0);
            string BackupFileName = new MDBackupRestoreNecessaryElementClass().BackupFileNameMaker(VariableClass.MYSQL_DATABASE_NAME);
            string encryptFileName= string.Format("AB{0}",BackupFileName);
            string compreshedFileName =string.Format("{0}.gz",encryptFileName);
            copyFileName = compreshedFileName;
            databaseBackupWorker.ReportProgress(10);
            MysqlBackupVariablesClass.MYSQL_CONNECTION_STRING[0] = Properties.Settings.Default.DatabaseHostIP;
            MysqlBackupVariablesClass.MYSQL_CONNECTION_STRING[1] = Properties.Settings.Default.DatabasePortNumber;
            MysqlBackupVariablesClass.MYSQL_CONNECTION_STRING[2] = Properties.Settings.Default.DatabaseDatabaseName;
            MysqlBackupVariablesClass.MYSQL_CONNECTION_STRING[3] = Properties.Settings.Default.DatabaseUsername;
            MysqlBackupVariablesClass.MYSQL_CONNECTION_STRING[4] = Properties.Settings.Default.DatabasePassword;
            new MysqlDataBackupRestoreClass().DataBaseBackup(VariableClass.MYSQL_DATABASE_NAME, System.IO.Path.Combine(new ProcestaNecessaryFunction.NecessaryFunction().GetTempFolder(VariableClass.ERROR_MESSAGES[1,2]),BackupFileName));
            encryptFileName = System.IO.Path.Combine(new ProcestaNecessaryFunction.NecessaryFunction().GetTempFolder(VariableClass.ERROR_MESSAGES[1, 2]), encryptFileName);
            compreshedFileName = System.IO.Path.Combine(new ProcestaNecessaryFunction.NecessaryFunction().GetTempFolder(VariableClass.ERROR_MESSAGES[1, 2]), compreshedFileName);
            BackupFileName = System.IO.Path.Combine(new ProcestaNecessaryFunction.NecessaryFunction().GetTempFolder(VariableClass.ERROR_MESSAGES[1, 2]), BackupFileName);
            databaseBackupWorker.ReportProgress(20);
            FileOperations.EncryptFile(BackupFileName, encryptFileName);
            databaseBackupWorker.ReportProgress(30);
            File.Delete(BackupFileName);
            databaseBackupWorker.ReportProgress(40);
            new GZFormate().CompressToGZip(encryptFileName);
            databaseBackupWorker.ReportProgress(50);
            File.Delete(encryptFileName);
            databaseBackupWorker.ReportProgress(60);
            try
            {
                if (Properties.Settings.Default.SaveLocalComputer)
                {
                    File.Copy(compreshedFileName, System.IO.Path.Combine(Properties.Settings.Default.LastSavePath, copyFileName));
                }
                if (Properties.Settings.Default.AttachEmail)
                {
                    string eMailBody = string.Format("Automatically Send By {0} At {1} From {2}", VariableClass.ERROR_MESSAGES[0, 0], DateTime.Now, Properties.Settings.Default.SendMailFromAddress);
                    new ProcestaSendMail(Properties.Settings.Default.SendMailFromAddress, Properties.Settings.Default.SendMailPassword, Properties.Settings.Default.SmtpServer, Properties.Settings.Default.SendMailPortNumber, Properties.Settings.Default.SendMailToAddress, "Petunia Database", eMailBody, compreshedFileName);
                }
                if (Properties.Settings.Default.SkyDrive)
                {
                    new UploadSkyDrive(Properties.Settings.Default.SkyDiveEmailID, Properties.Settings.Default.SkyDrivePassword, string.Format("{0} Backup", VariableClass.ERROR_MESSAGES[0, 0]), compreshedFileName);
                }
                databaseBackupWorker.ReportProgress(70);
            }
            catch
            {
                e.Cancel=true;
            }
            finally
            {
                File.Delete(compreshedFileName);
                databaseBackupWorker.ReportProgress(70);
            }
        }
        //Process Changed
        private void DatabaseBackupProcessChanged(object sender,ProgressChangedEventArgs e)
        {
                this.DatabaseBackupProgressBar.Value = e.ProgressPercentage;
        }
        //Run Work Complete
        private void DatabaseBackupRunWorkCompleatd(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0,6],VariableClass.ERROR_MESSAGES[0,0],MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                //Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0, 7], VariableClass.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.AutoBackUpButtonStart.IsEnabled = true;
            this.startUpChecker.IsEnabled = true;
        }
        #endregion

        #region Restore
        //Backup DoWork
        private void DatabaseRestoreDowWorker(object sender, DoWorkEventArgs e)
        { 
            String[] Fileinfo=new String[2];
            Fileinfo=(String[])e.Argument;
                try
                {
                    databaseRestoreWorker.ReportProgress(10);
                    File.Copy(Fileinfo[1], System.IO.Path.Combine(new ProcestaNecessaryFunction.NecessaryFunction().GetTempFolder(VariableClass.ERROR_MESSAGES[1, 2]), Fileinfo[0]), true);
                    databaseRestoreWorker.ReportProgress(20);
                    new GZFormate().DeCompressToGZip(Fileinfo[0]);
                    databaseRestoreWorker.ReportProgress(30);
                    File.Delete(Fileinfo[0]);
                    databaseRestoreWorker.ReportProgress(40);
                    string sqlFileName = Fileinfo[0].Substring(0, Fileinfo[0].Length - 2);
                    FileOperations.DecryptFile(sqlFileName, System.IO.Path.Combine(new ProcestaNecessaryFunction.NecessaryFunction().GetTempFolder(VariableClass.ERROR_MESSAGES[1, 2]), "restoreAble.sql"));
                    databaseRestoreWorker.ReportProgress(50);
                    File.Delete(sqlFileName);
                    databaseRestoreWorker.ReportProgress(60);
                    new MysqlDataBackupRestoreClass().DataBaseRestore(System.IO.Path.Combine(new ProcestaNecessaryFunction.NecessaryFunction().GetTempFolder(VariableClass.ERROR_MESSAGES[1, 2]), "restoreAble.sql"));
                    databaseRestoreWorker.ReportProgress(70);
                    File.Delete(System.IO.Path.Combine(new ProcestaNecessaryFunction.NecessaryFunction().GetTempFolder(VariableClass.ERROR_MESSAGES[1, 2]), "restoreAble.sql"));
                    databaseRestoreWorker.ReportProgress(80);
                }
                catch
                {
                    e.Cancel = true;
                }
        }
        //Process Changed
        private void DatabaseRestoreProcessChanged(object sender, ProgressChangedEventArgs e)
        {
            this.restoreProgressBar.Value = e.ProgressPercentage;
        }
        //Run Work Complete
        private void DatabaseRestoreRunWorkCompleated(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[1,1], VariableClass.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[1, 0], VariableClass.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.RestoreButtonStart.IsEnabled = true;
            
        }
        #endregion
        #endregion

        #region Timer Event
        private void StatUpCheckerTrick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.AutoBackupEnable)
            {
                this.LabelAutoBackUpTimeRemaining.Text = new MysqlDataBackupRestore.Class.NecessaryClass().TimeRemaing(Properties.Settings.Default.AutoBackupTime.ToString());
                if ((this.LabelAutoBackUpTimeRemaining.Text.IndexOf('-').Equals(0)) || this.LabelAutoBackUpTimeRemaining.Text.Equals(string.Empty))
                {
                    Properties.Settings.Default.AutoBackupSchedular = "Every Day";
                    Properties.Settings.Default.AutoBackupKind = 2;
                    Properties.Settings.Default.AutoBackupTime = DateTime.Now;
                    Properties.Settings.Default.SaveLocalComputer = true;
                    string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AlbatrossBackup");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    Properties.Settings.Default.LastSavePath = folderPath;
                    Properties.Settings.Default.Save();
                    new NecessaryClass().AutobackupSchedular();
                }
                if (this.LabelAutoBackUpTimeRemaining.Text.Equals("0.00:00:00") | this.LabelAutoBackUpTimeRemaining.Text.Equals("00:00:00"))
                {
                    this.startUpChecker.IsEnabled = false;
                    this.AutoBackUpButtonStart.IsEnabled = false;
                    this.DatabaseBackupProgressBar.Value = 1;
                    this.databaseBackupWorker.RunWorkerAsync();
                    new NecessaryClass().AutobackupSchedular();
                }
            }
            else
            {
                this.LabelAutoBackUpTimeRemaining.Text = string.Empty;
            }
            ShowAutoBackupOnOff();
        }
        #endregion

        #region Private Function
        //HIde All Panel
        private void HideAllPanel()
        {
            this.PanelAutoBackUp.Visibility = Visibility.Hidden;
            this.PanelRestore.Visibility = Visibility.Hidden;
        }
        // Window Minimize Try 
        private void WindowMinimizeTry(string info)
        {
            Samples.FancyBalloon NotifyIconShow = new FancyBalloon();
            NotifyIconShow.BalloonText = VariableClass.ERROR_MESSAGES[0, 0];
            NotifyIconShow.LabelMessages.Text = info;
            AlbatrossNoitify.ShowCustomBalloon(NotifyIconShow, PopupAnimation.Fade, 2000);
        }
        //Show AutoBackup On Or OFF
        private void ShowAutoBackupOnOff()
        {
            if (Properties.Settings.Default.AutoBackupEnable)
            {
                this.LabelAutoBackUpOnOff.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF37FF00");
                this.LabelAutoBackUpOnOff.Text = "ON";
            }
            else
            {
                this.LabelAutoBackUpOnOff.Foreground = Brushes.Red;
                this.LabelAutoBackUpOnOff.Text = "OFF";
            }
        }
        //On Closing
        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    if (VariableClass.CLOSE_ON_BUTTON)
        //    {
        //        MessageBoxResult messBoxResult = new MessageBoxResult();
        //        messBoxResult = Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0, 3], VariableClass.ERROR_MESSAGES[0, 0], MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (messBoxResult.Equals(MessageBoxResult.Yes))
        //        {
        //            Application.Current.Shutdown();
        //        }
        //        else
        //        {
        //            e.Cancel = true;
        //        }
        //    }
        //}
        #endregion
           
    }
}
