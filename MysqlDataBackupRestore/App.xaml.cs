using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Samples;
using AlbatrossVaribales;
using System.Windows.Controls.Primitives;
using MysqlDataBackup;
namespace MysqlDataBackupRestore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //start up Action
        private void AutoStartupCherk(object sender, StartupEventArgs e)
        {
            try
            {
                if (e.Args.Length > 0 && e.Args[0].Equals("-s"))
                {
                    VariableClass.START_UP_FLAGE = true;
                    return;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
    }
}
