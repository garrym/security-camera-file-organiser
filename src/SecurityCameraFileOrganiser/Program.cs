using System;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace SecurityCameraFileOrganiser
{
    class Program
    {
        static void Main()
        {
            var directory = ConfigurationManager.AppSettings["ImageDirectory"];
            var fileNames = Directory.GetFiles(directory);
            foreach (var fileName in fileNames)
            {
                if (DateTime.TryParseExact(fileName, "\\MDAlar\\m_yyyyMMdd-HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    Console.WriteLine("Found a date!");
                }

                //MDAlarm_20170831-203351
                //2017-08-31

            }
        }
    }
}
