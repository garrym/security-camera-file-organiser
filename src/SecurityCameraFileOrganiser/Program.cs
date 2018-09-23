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
                var fileInfo = new FileInfo(fileName);

                var datePart = fileInfo.Name.Replace("MDAlarm_", "").Replace(".jpg", "");

                if (DateTime.TryParseExact(datePart, "yyyyMMdd-HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    Console.WriteLine("Found a date!");
                }
            }
        }
    }
}
