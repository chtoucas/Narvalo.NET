namespace Narvalo.GhostScript
{
    using System;
    using System.Drawing;
    using System.IO;
    using Narvalo.GhostScript.Options;

    public static class GhostScriptHelper
    {
        public static Bitmap ExtractCover(string inputFile)
        {
            Requires.NotNullOrEmpty(inputFile, "inputFile");

            if (!File.Exists(inputFile)) {
                throw new FileNotFoundException();
            }

            Bitmap result;
            string tmpFile = String.Empty;

            try {
                tmpFile = Path.GetTempFileName();

                var args = new ImageArgs(inputFile, ImageFormat.Png24) {
                    Display = new Display { PageSizeMode = PageSizeMode.CropBox, },
                    OutputFile = tmpFile,
                };

                GhostScriptApiFactory.CreateApi().Execute<ImageDevice>(args);

                using (var stream = new FileStream(tmpFile, FileMode.Open, FileAccess.Read)) {
                    result = new Bitmap(stream);
                }
            }
            finally {
                if (!String.IsNullOrEmpty(tmpFile)) {
                    File.Delete(tmpFile);
                }
            }

            return result;
        }

        public static string ExtractText(string inputFile)
        {
            Requires.NotNullOrEmpty(inputFile, "inputFile");

            if (!File.Exists(inputFile)) {
                throw new FileNotFoundException();
            }

            string result;
            string tmpFile = String.Empty;

            try {
                tmpFile = Path.GetTempFileName();

                var args = new TextArgs(inputFile) {
                    OutputFile = tmpFile,
                };

                GhostScriptApiFactory.CreateApi().Execute<TextDevice>(args);

                using (var reader = new StreamReader(tmpFile)) {
                    result = reader.ReadToEnd();
                }
            }
            finally {
                if (!String.IsNullOrEmpty(tmpFile)) {
                    File.Delete(tmpFile);
                }
            }

            return result;
        }
    }
}
