namespace Narvalo.Web.UI.Assets.AssetBundle
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Hosting;
    using Microsoft.Ajax.Utilities;

    /// <summary>
    /// Summary description for JavaScriptBundleHandler
    /// </summary>
    public class AssetBundleHandler : IHttpHandler
    {
        private static readonly Regex Version
            = new Regex("(v_[0-9]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly StringDictionary AllowedFileTypes = new StringDictionary() {
		    {".js", "text/javascript"},
		    {".css", "text/css"}
	    };

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string extension = Path.GetExtension(context.Request.PhysicalPath).ToLowerInvariant();

            if (!AllowedFileTypes.ContainsKey(extension)) {
                throw new HttpException(403, "File type not supported");
            }

            string path = Version.Replace(context.Request.PhysicalPath, string.Empty);
            FileInfo[] files = BundleHelper.FindFiles(path);
            string content = ReadContent(files);
            string result = Minify(content, extension);
            context.Response.Write(result);

            SetHeaders(context, files, AllowedFileTypes[extension]);
        }

        private string ReadContent(FileInfo[] files)
        {
            StringBuilder sb = new StringBuilder(1024);
            Array.ForEach(files, f => sb.AppendLine(File.ReadAllText(f.FullName)));
            return sb.ToString();
        }

        private static string Minify(string content, string extension)
        {
            if (extension == ".js") {
                CodeSettings settings = new CodeSettings();
                settings.MinifyCode = true;
                settings.LocalRenaming = LocalRenaming.CrunchAll;
                settings.RemoveFunctionExpressionNames = true;
                settings.EvalTreatment = EvalTreatment.MakeAllSafe;

                Minifier cruncher = new Minifier();
                return cruncher.MinifyJavaScript(content, settings);
            }
            else if (extension == ".css") {
                Minifier cruncher = new Minifier();
                return cruncher.MinifyStyleSheet(content);
            }

            return content;
        }

        private void SetHeaders(HttpContext context, FileInfo[] files, string mimeType)
        {
            DateTime lastModified = files.Max(f => f.LastWriteTime);

            context.Response.AddFileDependencies(files.Select(f => f.FullName).ToArray());
            context.Response.ContentType = mimeType;

            context.Response.Cache.SetLastModified(lastModified);
            context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
            context.Response.Cache.SetOmitVaryStar(true);
            context.Response.Cache.SetValidUntilExpires(true);
        }
    }

    public static class BundleHelper
    {
        internal static FileInfo[] FindFiles(string absoluteFileName)
        {
            // Is file
            if (File.Exists(absoluteFileName)) {
                return new[] { new FileInfo(absoluteFileName) };
            }

            // Is directory
            string dir = absoluteFileName.Replace(Path.GetExtension(absoluteFileName), string.Empty);
            return new DirectoryInfo(dir).GetFiles("*" + Path.GetExtension(absoluteFileName));
        }

        public static string InsertFile(string relativePath)
        {
            if (HttpContext.Current.Cache[relativePath] == null) {
                if (!relativePath.StartsWith("/", StringComparison.Ordinal) && !relativePath.StartsWith("~", StringComparison.Ordinal))
                    relativePath = "/" + relativePath;

                string absolutePath = HostingEnvironment.MapPath(relativePath);
                FileInfo[] files = FindFiles(absolutePath);
                DateTime lastModified = files.Max(f => f.LastWriteTime);

                int index = relativePath.LastIndexOf('/') + 1;
                string newPath = relativePath.Insert(index, "v_" + lastModified.Ticks + "/");
                string dependency = files.Length > 1 ? files[0].DirectoryName : files[0].FullName;
                HttpContext.Current.Cache.Insert(relativePath, newPath, new CacheDependency(dependency));
            }

            return (string)HttpContext.Current.Cache[relativePath];
        }
    }
}
