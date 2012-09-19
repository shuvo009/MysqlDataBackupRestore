using System;
using System.Windows;
using System.Windows.Input;
using ProcestaRegeditClasss;
using Telerik.Windows.Controls;
using AlbatrossVaribales;
using MysqlDataBackupRestore.Class;
namespace MysqlDataBackupRestore
{
	/// <summary>
	/// Interaction logic for Setting.xaml
	/// </summary>
	public partial class Setting : Window
	{
		public Setting()
		{
			this.InitializeComponent();
            AllItemHidden();
            GeneralItemSettingAndCLear();
            //this.PanelGeneral.Visibility = Visibility.Visible;
        }

        #region Tree View

        private void SettingTreeViewSelect(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem Item = SettingTreeView.SelectedContainer;
            if (Item.Name.Equals(TreeViewGeneral.Name))
            {
                AllItemHidden();
                GeneralItemSettingAndCLear();
                this.PanelGeneral.Visibility = Visibility.Visible;
            }
            else if (Item.Name.Equals(TreeViewGeneralAutoBackup.Name))
            {
                AllItemHidden();
                AutobackupItemSettingAndLoad();
                this.PanelAutoBackup.Visibility = Visibility.Visible;
            }
            else if(Item.Name.Equals(TreeViewDatabase.Name))
            {
                AllItemHidden();
                DatabaseSettingIteSettingAndSetting();
                this.PanelDatabase.Visibility = Visibility.Visible;
            }
            else if (Item.Name.Equals(TreeViewEmail.Name))
            {
                AllItemHidden();
                EmailItemClearAndSetting();
                this.PanelEmail.Visibility = Visibility.Visible;
            }
            else if (Item.Name.Equals(TreeViewSkyDrive.Name))
            {
                AllItemHidden();
                SkyDriveItemClearAndLoad();
                this.PanelSkyDrive.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Button Event

        #region Panel Database
        //Database setting update
        private void DatabaseButtonUpdateClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DatabaseTextBoxUserName.Text!=string.Empty && this.DatabasePasswordBoxPassword.Password!=string.Empty && this.DatabaseTextBoxHostIP.Text!=string.Empty && this.DatabaseTextBoxPortNumber.Text!=string.Empty && this.DatabaseTextBoxUserName.Text!=string.Empty)
            {
                Properties.Settings.Default.DatabaseUsername = this.DatabaseTextBoxUserName.Text;
                Properties.Settings.Default.DatabasePassword = this.DatabasePasswordBoxPassword.Password;
                Properties.Settings.Default.DatabaseHostIP = this.DatabaseTextBoxHostIP.Text;
                Properties.Settings.Default.DatabasePortNumber = this.DatabaseTextBoxPortNumber.Text;
                Properties.Settings.Default.DatabaseDatabaseName = this.DatabaseTextBoxDataBaseName.Text;
                Properties.Settings.Default.Save();
                Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0,5],VariableClass.ERROR_MESSAGES[0,0],MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
            {
                Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0,4],VariableClass.ERROR_MESSAGES[0,0],MessageBoxButton.OK,MessageBoxImage.Stop);
            }
        }
        #endregion

        #region Panel General
        //General Brows Button click Event
        private void GeneralButtonBrowsCLick(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog genearlFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            genearlFolderBrowser.Description = "Select a folder to save your backup file";
            genearlFolderBrowser.RootFolder = System.Environment.SpecialFolder.Desktop;
            System.Windows.Forms.DialogResult generalDialogResult=genearlFolderBrowser.ShowDialog();
            if (generalDialogResult.Equals(System.Windows.Forms.DialogResult.OK))
            {
                this.generalLabelSavePath.Text = genearlFolderBrowser.SelectedPath;
                Properties.Settings.Default.LastSavePath = genearlFolderBrowser.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region Panel AutoBackup
        //Auto Back up Update Click
        private void AutoBackupButtonUpdateClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.AutoBackupButtonUpdate.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            ProcestaNecessaryFunction.NecessaryFunction necessaryFunctionElement = new ProcestaNecessaryFunction.NecessaryFunction();
            bool timeEmptyError=true;
            if (this.AutoBackupSettingRadioButtonHourly.IsChecked.Equals(true))
            {
                Properties.Settings.Default.AutoBackupKind = 1;
                Properties.Settings.Default.AutoBackupSchedular = this.AutobackUpComboBoxHourly.Text;
                Properties.Settings.Default.AutoBackupTime = DateTime.Now;
                Properties.Settings.Default.Save();
            }
            else if (this.AutoBackupSettingRadioButtonDaily.IsChecked.Equals(true))
            {
                if (!this.AutobackUpTimeDaily.SelectedTime.Equals(string.Empty))
                {
                    Properties.Settings.Default.AutoBackupKind = 2;
                    Properties.Settings.Default.AutoBackupSchedular = this.AutobackUpComboBoxDaily.Text;
                    Properties.Settings.Default.AutoBackupTime = Convert.ToDateTime(string.Format("{0} {1}",DateTime.Now.ToShortDateString() , necessaryFunctionElement.TwelveHourClock(this.AutobackUpTimeDaily.SelectedTime.ToString())));
                    Properties.Settings.Default.Save();
                }
                else
                {
                    timeEmptyError = false;
                }
            }
            else if (this.AutoBackupSettingRadioButtonWeekly.IsChecked.Equals(true))
            {
                if (!this.AutoBackupTimeWeekly.SelectedTime.Equals(string.Empty))
                {
                    Properties.Settings.Default.AutoBackupKind = 3;
                    Properties.Settings.Default.AutoBackupSchedular = this.AutobackUpComboBoxWeekly.Text;
                    Properties.Settings.Default.AutoBackupTime = Convert.ToDateTime(string.Format("{0} {1}",DateTime.Now.ToShortDateString() , necessaryFunctionElement.TwelveHourClock(this.AutoBackupTimeWeekly.SelectedTime.ToString())));
                    Properties.Settings.Default.Save();
                }
                else
                {
                    timeEmptyError = false;
                }
            }
            else if (this.AutoBackupSettingRadioButtonMonthely.IsChecked.Equals(true))
            {
                if (!this.AutoBackupTimeBoxMonthely.SelectedTime.Equals(string.Empty))
                {
                    Properties.Settings.Default.AutoBackupKind = 4;
                    Properties.Settings.Default.AutoBackupSchedular = this.AutoBackupComboBoxMonthly.Text;
                    Properties.Settings.Default.AutoBackupTime = Convert.ToDateTime(string.Format("{0} {1}",DateTime.Now.ToShortDateString() , necessaryFunctionElement.TwelveHourClock(this.AutoBackupTimeBoxMonthely.SelectedTime.ToString())));
                    Properties.Settings.Default.Save();
                }
                else
                {
                    timeEmptyError = false;
                }
            }
            new NecessaryClass().AutobackupSchedular();
            if (timeEmptyError)
            {
                Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0, 5], VariableClass.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0,4],VariableClass.ERROR_MESSAGES[0,0],MessageBoxButton.OK,MessageBoxImage.Stop);
            }
            this.AutoBackupButtonUpdate.IsEnabled =true;
            this.Cursor = Cursors.Arrow;
        }
        #endregion

        #region Panel Email
        //Email Setting update Click
        private void EmailButtonUpdateClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.EmailComboBoxTo.Text != string.Empty && this.EmailComboBoxTo.Text!=string.Empty)
            {
                Properties.Settings.Default.SendMailToAddress = this.EmailTextBoxTo.Text + this.EmailComboBoxTo.Text;
                Properties.Settings.Default.Save();
                Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0, 5], VariableClass.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0,4],VariableClass.ERROR_MESSAGES[0,0],MessageBoxButton.OK,MessageBoxImage.Stop);
            }
        }
        #endregion

        #region Panel Sky Drive
        private void SkyDriveButtonUpdateClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.SkyDriveTextBoxEmail.Text!=string.Empty && this.SkyDrivePasswordBoxPassword.Password!=string.Empty)
            {
                Properties.Settings.Default.SkyDiveEmailID = this.SkyDriveTextBoxEmail.Text;
                Properties.Settings.Default.SkyDrivePassword = this.SkyDrivePasswordBoxPassword.Password;
                Properties.Settings.Default.Save();
                Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0, 5], VariableClass.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Microsoft.Windows.Controls.MessageBox.Show(VariableClass.ERROR_MESSAGES[0,4],VariableClass.ERROR_MESSAGES[0,0],MessageBoxButton.OK,MessageBoxImage.Stop);
            }
        }
        #endregion

        #endregion

        #region CheckBox Events

        #region General Check Box
        //At Start Up Checked
        private void GeneralCheckBoxStartAtStartUpChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.GeneralCheckBoxStartAtStartUp.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            RegeditWriteRead registryWrite = new RegeditWriteRead();
            registryWrite.RegistryWriter(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Albatross", string.Format("{0} -s",System.Reflection.Assembly.GetExecutingAssembly().Location), Microsoft.Win32.RegistryValueKind.String);
            Properties.Settings.Default.StartupEnable = true;
            Properties.Settings.Default.Save();
            this.GeneralCheckBoxStartAtStartUp.IsEnabled = true;
            this.Cursor = Cursors.Arrow;
        }
        //At Start up Unchecked
        private void GeneralCheckBoxStartAtStartUpUnChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.GeneralCheckBoxStartAtStartUp.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            RegeditWriteRead registryDaelete = new RegeditWriteRead();
            registryDaelete.RegistryKeyDelete(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", "Albatross");
            Properties.Settings.Default.StartupEnable = false;
            Properties.Settings.Default.Save();
            this.GeneralCheckBoxStartAtStartUp.IsEnabled = true;
            this.Cursor = Cursors.Arrow;
        }
        //Save Local Computer Checked
        private void GeneralCheckBoxSaveLockComputerChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.generalLabelSavePath.Text.Equals(string.Empty))
            {
                Properties.Settings.Default.SaveLocalComputer = true;
                Properties.Settings.Default.Save();
                this.GeneralButtonBrows.IsEnabled = true;
            }
        }
        //Save Local Computer UnChecked
        private void GeneralCheckBoxSaveLockComputerUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings.Default.SaveLocalComputer = false;
            Properties.Settings.Default.Save();
            this.GeneralButtonBrows.IsEnabled = false;
        }
        //Attached with E-mail Checked
        private void GeneralCheckBoxAttachWithMailChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings.Default.AttachEmail = true;
            Properties.Settings.Default.Save();
        }
        //Attached with E-mail Unchecked
        private void GeneralCheckBoxAttachWithMailUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings.Default.AttachEmail = false;
            Properties.Settings.Default.Save();
        }
        //Sky Drive unchecked
        private void GeneralCheckBoxSkyDiveUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings.Default.SkyDrive = false;
            Properties.Settings.Default.Save();
        }
        //Sky Drive Checked
        private void GeneralCheckBoxSkyDiveChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings.Default.SkyDrive = true;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region AutoBackUp
            //Auto Backup On OFF Checked
            private void AutoBackupCheckBoxONOFFChecked(object sender, System.Windows.RoutedEventArgs e)
            {
                this.AutoBackupPanelSetting.IsEnabled = true;
                Properties.Settings.Default.AutoBackupEnable = true;
                Properties.Settings.Default.Save();
            }
            //Auto Backup On OFF Unchecked 
            private void AutoBackupCheckBoxONOFFUnChecked(object sender, System.Windows.RoutedEventArgs e)
            {
                this.AutoBackupPanelSetting.IsEnabled = false;
                Properties.Settings.Default.AutoBackupEnable = false;
                Properties.Settings.Default.Save();
            }
            #endregion

        #endregion

        #region ReadioButton Event

            #region Panel AutoBackUp
            // Hourly Checked
            private void AutoBackupSettingRadioButtonHourlyChecked(object sender, System.Windows.RoutedEventArgs e)
            {
                this.AutoBackupPanelHourly.Visibility = Visibility.Visible;
            }
            //Hourly UnChecked
            private void AutoBackupSettingRadioButtonHourlyUnChecked(object sender, System.Windows.RoutedEventArgs e)
            {
                this.AutoBackupPanelHourly.Visibility = Visibility.Hidden;
            }
            //Daily Checked
            private void AutoBackupSettingRadioButtonDailyChecked(object sender, System.Windows.RoutedEventArgs e)
            {
                this.AutobackUpPanelDaily.Visibility = Visibility.Visible;
            }
            //Daily UnChecked
            private void AutoBackupSettingRadioButtonDailyUnchecked(object sender, System.Windows.RoutedEventArgs e)
            {
                this.AutobackUpPanelDaily.Visibility = Visibility.Hidden;
            }
            //Weekly Checked
            private void AutoBackupSettingRadioButtonWeeklyChecked(object sender, System.Windows.RoutedEventArgs e)
            {
                this.AutobackupPanelWeekly.Visibility = Visibility.Visible;
            }
            //Weekly UnChecked
            private void AutoBackupSettingRadioButtonWeeklyUnChecked(object sender, System.Windows.RoutedEventArgs e)
            {
                this.AutobackupPanelWeekly.Visibility = Visibility.Hidden;
            }
            //Monthly checked
            private void AutoBackupSettingRadioButtonMonthelyChecked(object sender, System.Windows.RoutedEventArgs e)
            {
                this.AutoBackupPanelMonthly.Visibility = Visibility.Visible;
            }
            //Monthly UnChecked
            private void AutoBackupSettingRadioButtonMonthelyUnChecked(object sender, System.Windows.RoutedEventArgs e)
            {
                this.AutoBackupPanelMonthly.Visibility = Visibility.Hidden;
            }
            #endregion

        #endregion

        #region Private Function

            #region ItemHidden
            private void AllItemHidden()
        {
            this.PanelGeneral.Visibility = Visibility.Hidden;
            this.PanelDatabase.Visibility=Visibility.Hidden;
            this.PanelAutoBackup.Visibility = Visibility.Hidden;
            this.PanelEmail.Visibility = Visibility.Hidden;
            this.PanelSkyDrive.Visibility = Visibility.Hidden;
        }
         #endregion

         #region ItemClear or Load Setting
         //General Item Clear and Setting
         private void GeneralItemSettingAndCLear()
        {
           this.GeneralCheckBoxStartAtStartUp.IsChecked = Properties.Settings.Default.StartupEnable;
           this.GeneralCheckBoxSaveLockComputer_.IsChecked = Properties.Settings.Default.SaveLocalComputer;
           this.GeneralCheckBoxAttachWithMail.IsChecked = Properties.Settings.Default.AttachEmail;
           this.GeneralCheckBoxSkyDive.IsChecked = Properties.Settings.Default.SkyDrive;
           this.generalLabelSavePath.Text = Properties.Settings.Default.LastSavePath;
        }
         //AutoBackup Item Clear and Setting
         private void AutobackupItemSettingAndLoad()
        {
            this.AutoBackupCheckBoxONOFF.IsChecked = Properties.Settings.Default.AutoBackupEnable;
            this.AutoBackupLabelStartAt.Text = Properties.Settings.Default.AutoBackupTime.ToString();

            switch (Properties.Settings.Default.AutoBackupKind)
            {
                case 1:
                    this.AutoBackupSettingRadioButtonHourly.IsChecked = true;
                    this.AutobackUpComboBoxHourly.Text = Properties.Settings.Default.AutoBackupSchedular;
                    break;
                case 2:
                    this.AutoBackupSettingRadioButtonDaily.IsChecked = true;
                    this.AutobackUpComboBoxDaily.Text = Properties.Settings.Default.AutoBackupSchedular;
                    break;
                case 3:
                    this.AutoBackupSettingRadioButtonWeekly.IsChecked = true;
                    this.AutobackUpComboBoxWeekly.Text = Properties.Settings.Default.AutoBackupSchedular;
                    break;
                case 4:
                    this.AutoBackupSettingRadioButtonMonthely.IsChecked = true;
                    this.AutoBackupComboBoxMonthly.Text = Properties.Settings.Default.AutoBackupSchedular;
                    break;
                default:
                    break;
            }
        }
        //Database Setting Item Clear and setting
         private void DatabaseSettingIteSettingAndSetting()
         {
             this.DatabaseTextBoxUserName.Text = Properties.Settings.Default.DatabaseUsername;
             this.DatabasePasswordBoxPassword.Password = string.Empty;
             this.DatabaseTextBoxHostIP.Text = Properties.Settings.Default.DatabaseHostIP;
             this.DatabaseTextBoxPortNumber.Text = Properties.Settings.Default.DatabasePortNumber;
             this.DatabaseTextBoxDataBaseName.Text = Properties.Settings.Default.DatabaseDatabaseName;
         }
         //E-Mail Item Clear Load Setting
         private void EmailItemClearAndSetting()
         {
             String[] email= Properties.Settings.Default.SendMailToAddress.Split('@');
             this.EmailTextBoxTo.Text = email[0];
             try
             {
                 this.EmailComboBoxTo.Text = String.Format("@{0}", email[1]);
             }
             catch(IndexOutOfRangeException)
             {

                 this.EmailComboBoxTo.Text = string.Empty;
             }
            
         }
         //Sky Drive Item Clear and Load Setting
         private void SkyDriveItemClearAndLoad()
         {
             this.SkyDriveTextBoxEmail.Text = Properties.Settings.Default.SkyDiveEmailID;
             this.SkyDrivePasswordBoxPassword.Password = string.Empty;
         }

         
         #endregion

        #endregion
    }
}