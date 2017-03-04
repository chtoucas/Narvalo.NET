// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System.Diagnostics.CodeAnalysis;

    public static class OpenGraphType
    {
        public const string Article = "article";
        public const string Book = "book";
        public const string Profile = "profile";
        public const string WebSite = "website";

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
            Justification = "[Intentionally] The nested class only contains constants.")]
        public static class Music
        {
            public const string Album = "music.album";
            public const string Playlist = "music.playlist";
            public const string RadioStation = "music.radio_station";
            public const string Song = "music.song";
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", 
            Justification = "[Intentionally] The nested class only contains constants.")]
        public static class Video
        {
            public const string Episode = "video.episode";
            public const string Movie = "video.movie";
            public const string Other = "video.other";
            public const string TvShow = "video.tv_show";
        }
    }
}
