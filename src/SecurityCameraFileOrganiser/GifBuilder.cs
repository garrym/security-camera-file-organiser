using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SecurityCameraFileOrganiser
{
    public class GifBuilder
    {
        public void Build(IList<string> imagePaths, string outputPath)
        {
            if (File.Exists(outputPath))
                return;

            var gifBitmapEncoder = new GifBitmapEncoder();

            var bitmaps = imagePaths.Select(x => new Bitmap(x));
            foreach (var bitmap in bitmaps)
            {
                var gdiBitmap = bitmap.GetHbitmap();
                var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(gdiBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                gifBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                DeleteObject(gdiBitmap);
            }

            WriteWithLoop(gifBitmapEncoder, outputPath);
        }

        private void WriteWithLoop(GifBitmapEncoder gifBitmapEncoder, string outputPath)
        {
            // https://stackoverflow.com/a/48395131
            using (var ms = new MemoryStream())
            {
                gifBitmapEncoder.Save(ms);
                var fileBytes = ms.ToArray();
                // This is the NETSCAPE2.0 Application Extension.
                var applicationExtension = new byte[] { 33, 255, 11, 78, 69, 84, 83, 67, 65, 80, 69, 50, 46, 48, 3, 1, 0, 0, 0 };
                var newBytes = new List<byte>();
                newBytes.AddRange(fileBytes.Take(13));
                newBytes.AddRange(applicationExtension);
                newBytes.AddRange(fileBytes.Skip(13));
                File.WriteAllBytes(outputPath, newBytes.ToArray());
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
    }
}
