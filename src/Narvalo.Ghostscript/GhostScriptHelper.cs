// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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
            Require.NotNullOrEmpty(inputFile, nameof(inputFile));

            if (!File.Exists(inputFile))
            {
                throw new FileNotFoundException("XXX");
            }

            Bitmap retval;
            string tmpFile = String.Empty;

            try
            {
                tmpFile = Path.GetTempFileName();

                var args = new ImageArgs(inputFile, ImageFormat.Png24) {
                    Display = new Display { PageSizeMode = PageSizeMode.CropBox, },
                    OutputFile = tmpFile,
                };

                GhostScriptApiFactory.CreateApi().Execute<ImageDevice>(args);

                using (var stream = new FileStream(tmpFile, FileMode.Open, FileAccess.Read))
                {
                    retval = new Bitmap(stream);
                }
            }
            finally
            {
                if (!String.IsNullOrEmpty(tmpFile))
                {
                    File.Delete(tmpFile);
                }
            }

            return retval;
        }

        public static string ExtractText(string inputFile)
        {
            Require.NotNullOrEmpty(inputFile, nameof(inputFile));

            if (!File.Exists(inputFile))
            {
                throw new FileNotFoundException();
            }

            string retval;
            string tmpFile = String.Empty;

            try
            {
                tmpFile = Path.GetTempFileName();

                var args = new TextArgs(inputFile) {
                    OutputFile = tmpFile,
                };

                GhostScriptApiFactory.CreateApi().Execute<TextDevice>(args);

                using (var reader = new StreamReader(tmpFile))
                {
                    retval = reader.ReadToEnd();
                }
            }
            finally
            {
                if (!String.IsNullOrEmpty(tmpFile))
                {
                    File.Delete(tmpFile);
                }
            }

            return retval;
        }
    }
}
