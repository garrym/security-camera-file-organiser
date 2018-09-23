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


            var filePaths = Directory.GetFiles(directory);
            foreach (var filePath in filePaths)
            {
                var fileInfo = new FileInfo(filePath);
                var datePart = fileInfo.Name.Replace("MDAlarm_", "").Replace(".jpg", "");

                if (DateTime.TryParseExact(datePart, "yyyyMMdd-HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    var dateDirectory = $"{date.Year:0000}-{date.Month:00}-{date.Day:00}";

                    var fullDateDirectory = Path.Combine(directory, dateDirectory);
                    if (!Directory.Exists(fullDateDirectory))
                        Directory.CreateDirectory(fullDateDirectory);

                    var newPath = Path.Combine(fullDateDirectory, fileInfo.Name);

                    File.Move(filePath, newPath);
                }
            }
        }
    }
}
