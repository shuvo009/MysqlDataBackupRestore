using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MysqlDataBackupRestore.Interfaces
{
    interface NecessaryElementInterface
    {
        string TimeRemaing(string backupTime);
        DateTime TimeAdder(DateTime currentBackupTime, int value, int options);
    }
}
