﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace SecurityCameraFileOrganiser
{
    class Program
    {
        private const char DirectorySeparatorCharacter = ';';

        static void Main()
        {
            foreach (var imageDirectory in GetImageDirectories())
            {
                foreach (var filePath in Directory.GetFiles(imageDirectory))
                {
                    ProcessFile(filePath);
                }
            }
        }

        private static IList<string> GetImageDirectories()
        {
            var configurationPaths = ConfigurationManager.AppSettings["ImageDirectories"];
            if (string.IsNullOrWhiteSpace(configurationPaths))
            {
                Console.WriteLine("No image directories specified.  You must add them as an appsetting named 'ImageDirectories'.");
                Console.WriteLine($"Separate multiple directories with '{DirectorySeparatorCharacter}'");
                Environment.Exit(1);
            }

            return configurationPaths.Split(DirectorySeparatorCharacter);
        }

        private static void ProcessFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var datePart = fileInfo.Name.Replace("MDAlarm_", "").Replace(".jpg", "");

            if (DateTime.TryParseExact(datePart, "yyyyMMdd-HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                var dateDirectory = $"{date.Year:0000}-{date.Month:00}-{date.Day:00}";

                var fullDateDirectory = Path.Combine(fileInfo.DirectoryName, dateDirectory);
                if (!Directory.Exists(fullDateDirectory))
                    Directory.CreateDirectory(fullDateDirectory);

                var newPath = Path.Combine(fullDateDirectory, fileInfo.Name);
                if (!File.Exists(newPath))
                    File.Move(filePath, newPath);
            }
        }
    }
}
