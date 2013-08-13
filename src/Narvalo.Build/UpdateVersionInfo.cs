namespace Narvalo.Build
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class UpdateVersionInfo : Task
    {
        string _branch;
        short _major;
        short _minor;
        short _build;
        short _revision;

        [Required]
        public string VersionFile { get; set; }

        [Output]
        public string Branch { get; set; }

        [Output]
        public string Revision { get; set; }

        [Output]
        public string Version { get; set; }

        public override bool Execute()
        {
            if (VersionFile.Length == 0) {
                Log.LogMessage(MessageImportance.High, "You must supply a version file.");
                return false;
            }

            XDocument xdoc;

            try {
                using (StreamReader reader = new StreamReader(VersionFile)) {
                    xdoc = XDocument.Load(reader);
                }

                var root = xdoc.Root;

                _branch = root.Element("Branch").Value;

                _major = Int16.Parse(root.Element("Major").Value);
                _minor = Int16.Parse(root.Element("Minor").Value);
                _build = Int16.Parse(root.Element("Build").Value);
                _revision = Int16.Parse(root.Element("Revision").Value);

                _revision++;

                root.Element("Revision").Value = _revision.ToString(CultureInfo.InvariantCulture);
                root.Element("Timestamp").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                Log.LogMessage(MessageImportance.Normal, "Updating VersionInfo: " + VersionFile);

                using (StreamWriter writer = new StreamWriter(VersionFile)) {
                    xdoc.Save(writer);
                }
            }
            catch (IOException ex) {
                Log.LogErrorFromException(ex);
            }

            Branch = _branch;
            Revision = _revision.ToString(CultureInfo.InvariantCulture);
            Version = String.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", _major, _minor, _build);

            return !Log.HasLoggedErrors;
        }
    }
}
