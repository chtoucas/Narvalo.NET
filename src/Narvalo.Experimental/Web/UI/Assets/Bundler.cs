namespace Narvalo.Web.UI.Assets.AssetBundle
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// Html.AssetBundler()
    ///     .Css()
    ///     .Add("style.css")
    ///     .Render();
    /// </example>
    public static class Bundler
    {
        public static void Bundle(IJavaScriptBundle jsBundle)
        {
        }

        public static void Bundle(ICssBundle cssBundle)
        {
        }

        public static IJavaScriptBundle Js()
        {
            throw new NotImplementedException();
        }

        public static ICssBundle Css()
        {
            throw new NotImplementedException();
        }
    }

    public interface IJavaScriptBundle
    {
        void AddRemote(string path, string remoteUri);
        void AddRemote(string path, Uri remoteUri);
        void Add(string path);
        void Render();
    }

    public interface ICssBundle
    {
        void AddRemote(string path, string remoteUri);
        void AddRemote(string path, Uri remoteUri);
        void Add(string path);
        void Render();
    }
}
