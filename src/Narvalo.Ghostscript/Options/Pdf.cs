// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public sealed class Pdf
    {
        public Pdf() : base() { }

        public int? FirstPage { get; set; }

        public int? LastPage { get; set; }

        public bool NoUserInit { get; set; } = false;

        public string Password { get; set; }

        public bool RenderTTNotDef { get; set; } = false;

        public bool ShowAcroForm { get; set; } = false;

        public bool ShowAnnotations { get; set; } = true;

        public void AddTo(ICollection<string> args)
        {
            Require.NotNull(args, nameof(args));

            if (!String.IsNullOrEmpty(Password))
            {
                args.Add("-sPDFPassword=" + Password);
            }

            if (FirstPage.HasValue)
            {
                args.Add("-dFirstPage=" + FirstPage.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (LastPage.HasValue)
            {
                args.Add("-dLastPage=" + LastPage.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (!ShowAnnotations)
            {
                args.Add("-dShowAnnots=false");
            }

            if (ShowAcroForm)
            {
                args.Add("-dShowAcroForm");
            }

            if (NoUserInit)
            {
                args.Add("-dNoUserUnit");
            }

            if (RenderTTNotDef)
            {
                args.Add("-dRENDERTTNOTDEF");
            }
        }
    }
}
