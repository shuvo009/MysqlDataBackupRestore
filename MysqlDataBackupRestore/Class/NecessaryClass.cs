using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MysqlDataBackupRestore.Interfaces;
using ProcestaNecessaryFunction;
using MysqlDataBackupRestore.Class;
namespace MysqlDataBackupRestore.Class
{
    class NecessaryClass : NecessaryElementInterface
    {
        //Time Reaming Calculation
        public string TimeRemaing(string backupTime)
        {
            DateTime backupDate = Convert.ToDateTime(backupTime); //Date Formate 3/30/2011 4:40:00 PM 
            DateTime backupDateAndTime = new DateTime(backupDate.Year, backupDate.Month, backupDate.Day, backupDate.Hour, backupDate.Minute, backupDate.Second);
            DateTime currentDateAndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan remainTime = backupDateAndTime.Subtract(currentDateAndTime);
            return remainTime.ToString();
        }
        //BackUp Time Maker
        public DateTime TimeAdder(DateTime currentBackupTime, int value, int options)
        {
            switch (options)
            {                                                                // 1-->Hourly
                case 1 :                                                     // 2-->Daily
                    return currentBackupTime.AddHours(value);                // 3-->Weekly
                case 2:                                                      // 4-->Monthly
                    return currentBackupTime.AddDays(value);          
                case 3:
                    return currentBackupTime.AddDays(value);
                case 4:
                    return currentBackupTime.AddMonths(value);
                default:
                    return currentBackupTime.AddDays(value);
            }
        }
        //Auto backup scheduler 
        public void AutobackupSchedular()
        {
            if (Properties.Settings.Default.AutoBackupKind.Equals(1))
            {
                switch (Properties.Settings.Default.AutoBackupSchedular)
                {
                    case "Every Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 1, 1);
                        break;
                    case "After 2 Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 2, 1);
                        break;
                    case "After 3 Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 3, 1);
                        break;
                    case "After 4 Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 4, 1);
                        break;
                    case "After 5 Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 5, 1);
                        break;
                    case "After 6 Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 6, 1);
                        break;
                    case "After 7 Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 7, 1);
                        break;
                    case "After 8 Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 8, 1);
                        break;
                    case "After 9 Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 9, 1);
                        break;
                    case "After 10 Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 10, 1);
                        break;
                    case "After 11 Hour":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 11, 1);
                        break;
                    default:
                        Properties.Settings.Default.AutoBackupEnable = false;
                        break;
                }
            }
            else if (Properties.Settings.Default.AutoBackupKind.Equals(2))
            {
                switch (Properties.Settings.Default.AutoBackupSchedular)
                {
                    case "Every Day":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 1, 2);
                        break;
                    case "After 2 Day":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 2, 2);
                        break;
                    case "After 3 Day":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 3, 2);
                        break;
                    case "After 4 Day":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 4, 2);
                        break;
                    case "After 5 Day":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 5, 2);
                        break;
                    case "After 6 Day":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 6, 2);
                        break;
                    default:
                        Properties.Settings.Default.AutoBackupEnable = false;
                        break;
                }
            }
            else if (Properties.Settings.Default.AutoBackupKind.Equals(3))
            {
                switch (Properties.Settings.Default.AutoBackupSchedular)
                {
                    case "Evert Week":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 7, 3);
                        break;
                    case "After 2 Week":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 14, 3);
                        break;
                    case "After 3 Week":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 21, 3);
                        break;
                    case "After 4 Week":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 28, 3);
                        break;
                    case "After 5 Week":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 35, 3);
                        break;
                    default:
                        Properties.Settings.Default.AutoBackupEnable = false;
                        break;
                }
            }
            else if (Properties.Settings.Default.AutoBackupKind.Equals(4))
            {
                switch (Properties.Settings.Default.AutoBackupSchedular)
                {
                    case "Every Month":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 1, 4);
                        break;
                    case "After 2 Month":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 2, 4);
                        break;
                    case "After 3 Month":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 3, 4);
                        break;
                    case "After 4 Month":
                        Properties.Settings.Default.AutoBackupTime = this.TimeAdder(Properties.Settings.Default.AutoBackupTime, 4, 4);
                        break;
                    default:
                        break;
                }
            }
            Properties.Settings.Default.Save();
        }
     
        
        //Backup Timer maker
        //public string TimeMaker(Telerik.Windows.Controls.RadDatePicker radDatePicker,Telerik.Windows.Controls.RadTimePicker radTimePicker)
        //{
        //    NecessaryFunction necessaryElement = new NecessaryFunction();
        //}
        
    }
}
