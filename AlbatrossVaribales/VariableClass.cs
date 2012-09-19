using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlbatrossVaribales
{
    public class VariableClass
    {
        public static bool START_UP_FLAGE = false;
        public static bool CLOSE_ON_BUTTON = true;
        public static string FILE_ENCRIPT_DISCRIPT_PASSWORD = "!QASW@#&89u5OPHT)z(3";
        public static string MYSQL_DATABASE_NAME = "petunia_nashir_electronices";
        public static String[,] ERROR_MESSAGES = new String[2, 10]
        {
            {"Albatross 1.0","I Am Here","Albatross Started","Are You Want to Edit","Fields are empty","Update Successfully","Database Backup Error !","Backup Finished","Please Select a File","Not a Albatross Backup File"},
            //    00            01              02                  03                      04                  05                  06                      07                  08                      09
            {"Restore Successfully","Database Restore Error !","Albatross","","","","","","",""}
            //10                            11                      12
        };
    }
}
