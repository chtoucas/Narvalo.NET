// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <remarks>
    /// To force a specific paper size and ignore the paper size specified in the document,
    /// select a paper size, and also include the FixedMedia.
    /// </remarks>
    public class Display
    {
        private PaperSize _defaultPaperSize = PaperSize.None;
        private bool? _fixedMedia;
        private bool? _fixedResolution;
        private Size? _mediaSize;
        private bool? _orient1;
        private Size? _pageSize;
        private PageSizeMode _pageSizeMode = PageSizeMode.None;
        private PaperSize _paperSize = PaperSize.None;
        private bool _pdfFitPage = false;
        private bool? _printed;
        private Size? _resolution;

        public Display() : base() { }

        /// <summary>
        /// This value will be used to replace the device default papersize ONLY if the default papersize 
        /// for the device is 'letter' or 'a4' serving to insulate users of A4 or 8.5x11 from particular 
        /// device defaults (the collection of contributed drivers in Ghostscript vary as to the default size).
        /// </summary>
        public PaperSize DefaultPaperSize { get { return _defaultPaperSize; } set { _defaultPaperSize = value; } }

        /// <summary>
        /// Causes the media size to be fixed after initialization, forcing pages of other sizes or 
        /// orientations to be clipped. This may be useful when printing documents on a printer that 
        /// can handle their requested paper size but whose default is some other size. 
        /// </summary>
        public bool? FixedMedia { get { return _fixedMedia; } set { _fixedMedia = value; } }

        /// <summary>
        /// Causes the media resolution to be fixed similarly.
        /// </summary>
        public bool? FixedResolution { get { return _fixedResolution; } set { _fixedResolution = value; } }

        /// <summary>
        /// Determines whether the file should be displayed or printed using the "screen" or 
        /// "printer" options for annotations and images. With -dPrinted, the output will use 
        /// the file's "print" options; with -dPrinted=false, the output will use the file's "screen" 
        /// options. If neither of these is specified, the output will use the screen options for any 
        /// output device that doesn't have an OutputFile parameter, and the printer options for devices 
        /// that do have this parameter.
        /// </summary>
        public bool? Printed { get { return _printed; } set { _printed = value; } }

        public bool? Orient1 { get { return _orient1; } set { _orient1 = value; } }

        /// <summary>
        /// Specifying the device width and height in pixels for the benefit of devices such as X11 windows 
        /// and VESA displays that require (or allow) you to specify width and height. 
        /// </summary>
        public Size? MediaSize { get { return _mediaSize; } set { _mediaSize = value; } }

        public Size? PageSize { get { return _pageSize; } set { _pageSize = value; } }

        public PageSizeMode PageSizeMode { get { return _pageSizeMode; } set { _pageSizeMode = value; } }

        public PaperSize PaperSize { get { return _paperSize; } set { _paperSize = value; } }

        /// <summary>
        /// Rather than selecting a PageSize given by the PDF MediaBox, TrimBox (see -dUseTrimBox)
        /// or CropBox (see -dUseCropBox), the PDF file will be scaled to fit the current device
        /// page size (usually the default page size).
        /// This is useful to avoid clipping information on a PDF document when sending to a printer
        /// that may have unprintable areas at the edge of the media larger than allowed for in the document.
        /// This is also useful for creating fixed size images of PDF files that may have a variety
        /// of page sizes, for example thumbnail images.
        /// </summary>
        public bool PdfFitPage { get { return _pdfFitPage; } set { _pdfFitPage = value; } }

        public Size? Resolution { get { return _resolution; } set { _resolution = value; } }

        public void AddTo(ICollection<string> args)
        {
            Require.NotNull(args, "args");

            if (Orient1.HasValue)
            {
                if (Orient1.Value)
                {
                    args.Add("-dORIENT1=true");
                }
                else
                {
                    args.Add("-dORIENT1=false");
                }
            }

            if (Printed.HasValue)
            {
                if (Printed.Value)
                {
                    args.Add("-dPrinted");
                }
                else
                {
                    args.Add("-dPrinted=false");
                }
            }

            // Screen
            if (MediaSize.HasValue)
            {
                // NB: GhostScript automatically sets FixedMedia
                //args.Add("-dDEVICEWIDTH=" + MediaSize.Value.Width.ToString(CultureInfo.InvariantCulture));
                //args.Add("-dDEVICEHEIGHT=" + MediaSize.Value.Height.ToString(CultureInfo.InvariantCulture));
                args.Add("-g" + MediaSize.Value.Width.ToString(CultureInfo.InvariantCulture)
                    + "x" + MediaSize.Value.Height.ToString(CultureInfo.InvariantCulture));
            }
            else if (FixedMedia.HasValue && FixedMedia.Value)
            {
                args.Add("-dFIXEDMEDIA");
            }

            // Printer
            if (Resolution.HasValue)
            {
                // NB: GhostScript automatically sets FixedResolution
                if (Resolution.Value.IsSquare)
                {
                    args.Add("-r" + Resolution.Value.Height.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    //args.Add("-dDEVICEXRESOLUTION=" + Resolution.Value.Width.ToString(CultureInfo.InvariantCulture));
                    //args.Add("-dDEVICEYRESOLUTION=" + Resolution.Value.Height.ToString(CultureInfo.InvariantCulture));
                    args.Add("-r" + Resolution.Value.Width.ToString(CultureInfo.InvariantCulture)
                        + "x" + Resolution.Value.Height.ToString(CultureInfo.InvariantCulture));
                }
            }
            else if (FixedResolution.HasValue && FixedResolution.Value)
            {
                args.Add("-dFIXEDRESOLUTION");
            }

            switch (PageSizeMode)
            {
                case PageSizeMode.CropBox:
                    args.Add("-dUseCropBox");
                    break;
                case PageSizeMode.TrimBox:
                    args.Add("-dUseTrimBox");
                    break;
                case PageSizeMode.None:
                    if (PaperSize != PaperSize.None)
                    {
                        args.Add("-sPAPERSIZE=" + GetPaperSizeName(PaperSize));
                    }
                    else if (PageSize.HasValue)
                    {
                        args.Add("-dDEVICEWIDTHPOINTS=" + PageSize.Value.Width.ToString(CultureInfo.InvariantCulture));
                        args.Add("-dDEVICEHEIGHTPOINTS=" + PageSize.Value.Height.ToString(CultureInfo.InvariantCulture));
                    }
                    break;
                default:
                    throw new NotSupportedException("Unsupported page size.");
            }
        }

        #region Utilitaires

        public static string GetPaperSizeName(PaperSize paperSize)
        {
            switch (paperSize)
            {
                // US
                case PaperSize.ArchE:
                    return "archE";
                case PaperSize.ArchD:
                    return "archD";
                case PaperSize.ArchC:
                    return "archC";
                case PaperSize.ArchB:
                    return "archB";
                case PaperSize.ArchA:
                    return "archA";
                case PaperSize.Ledger:
                    return "ledger";
                case PaperSize.LedgerPortrait:
                    return "11x17";
                case PaperSize.Legal:
                    return "legal";
                case PaperSize.Letter:
                    return "letter";
                case PaperSize.LetterSmall:
                    return "lettersmall";

                // ISO
                case PaperSize.A0:
                    return "a0";
                case PaperSize.A1:
                    return "a1";
                case PaperSize.A2:
                    return "a2";
                case PaperSize.A3:
                    return "a3";
                case PaperSize.A4:
                    return "a4";
                case PaperSize.A5:
                    return "a5";
                case PaperSize.A6:
                    return "a6";
                case PaperSize.A7:
                    return "a7";
                case PaperSize.A8:
                    return "a8";
                case PaperSize.A9:
                    return "a9";
                case PaperSize.A10:
                    return "a10";
                case PaperSize.B0:
                    return "isob0";
                case PaperSize.B1:
                    return "isob1";
                case PaperSize.B2:
                    return "isob2";
                case PaperSize.B3:
                    return "isob3";
                case PaperSize.B4:
                    return "isob4";
                case PaperSize.B5:
                    return "isob5";
                case PaperSize.B6:
                    return "isob6";
                case PaperSize.C0:
                    return "c0";
                case PaperSize.C1:
                    return "c1";
                case PaperSize.C2:
                    return "c2";
                case PaperSize.C3:
                    return "c3";
                case PaperSize.C4:
                    return "c4";
                case PaperSize.C5:
                    return "c5";
                case PaperSize.C6:
                    return "c6";

                // Other
                case PaperSize.Foolscap:
                    return "fse";
                case PaperSize.Hagaki:
                    return "hagaki";
                case PaperSize.HalfLetter:
                    return "halfletter";

                case PaperSize.None:
                default:
                    throw new NotSupportedException("Unsupported paper size.");
            }
        }

        #endregion
    }
}
