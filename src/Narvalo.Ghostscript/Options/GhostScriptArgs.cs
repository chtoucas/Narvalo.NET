namespace Narvalo.GhostScript.Options
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Diagnostics;

    public class GhostScriptArgs<T> where T : Device
    {
        #region Fields

        private readonly T _device;
        private readonly string _inputFile;

        private Interactions _interactions = Interactions.Default;
        private Miscs _miscs =  Miscs.Default;
        private string _outputFile;
        private Pdf _pdf;

        #endregion

        #region Ctor

        public GhostScriptArgs(string inputFile, T device)
        {
            Requires.NotNullOrEmpty(inputFile, "inputFile");
            Requires.NotNull(device, "device");

            _inputFile = inputFile;
            _device = device;
        }

        #endregion

        #region Properties

        public bool CanOutput { get { return !String.IsNullOrEmpty(OutputFile); } }

        public T Device { get { return _device; } }

        public string InputFile { get { return _inputFile; } }

        public Interactions Interactions { get { return _interactions; } set { _interactions = value; } }

        public Miscs Miscs { get { return _miscs; } set { _miscs = value; } }

        // TODO: Ce paramètre ne devrait-il pas être obligatoire ?
        public string OutputFile { get { return _outputFile; } set { _outputFile = value; } }

        public Pdf Pdf { get { return _pdf; } set { _pdf = value; } }

        #endregion

        #region Public methods

        public string[] ToParamArray()
        {
            var args = new List<string>();

            Interactions.AddTo(args);
            Miscs.AddTo(args);

            // Input/Output
            args.Add(InputFile);
            if (CanOutput) {
                args.Add("-sOutputFile=" + OutputFile);
            }

            Device.AddTo(args);
            if (Device.CanDisplay) {
                Device.AddDisplayTo(args);
            }

            if (Pdf != null) {
                Pdf.AddTo(args);
            }

            return args.ToArray();
        }

        public override string ToString()
        {
            return String.Join(" ", ToParamArray());
        }

        #endregion

        #region Helpers

        //private static string GetIccParameter(Icc arg) {
        //    switch (arg) {
        //        case Icc.DefaultGrayProfile:
        //            return "-sDefaultGrayProfile";
        //        case Icc.DefaultRgbProfile:
        //            return "-sDefaultRGBProfile";
        //        case Icc.DefaultCmykProfile:
        //            return "-sDefaultCMYKProfile";
        //        case Icc.DeviceNProfile:
        //            return "-sDeviceNProfile";
        //        case Icc.ProofProfile:
        //            return "-sProofProfile";
        //        case Icc.DeviceLinkProfile:
        //            return "-sDeviceLinkProfile";
        //        case Icc.NamedProfile:
        //            return "-sNamedProfile";
        //        case Icc.OutputIccProfile:
        //            return "-sOutputICCProfile";
        //        case Icc.RenderIntent:
        //            return "-sRenderIntent";
        //        case Icc.GraphicIccProfile:
        //            return "-sGraphicICCProfile";
        //        case Icc.GraphicIntent:
        //            return "-sGraphicIntent";
        //        case Icc.ImageIccProfile:
        //            return "-sImageICCProfile";
        //        case Icc.ImageIntent:
        //            return "-sImageIntent";
        //        case Icc.TextIccProfile:
        //            return "-sTextICCProfile";
        //        case Icc.TextIntent:
        //            return "-sTextIntent";
        //        case Icc.OverrideIcc:
        //            return "-dOverrideICC";
        //        case Icc.OverrideRI:
        //            return "-dOverrideRI";
        //        case Icc.SourceObjectIcc:
        //            return "-sSourceObjectICC";
        //        case Icc.DeviceGrayToK:
        //            return "-dDeviceGrayToK";
        //        case Icc.IccProfilesDirectory:
        //            return "-sICCProfilesDir";
        //        default:
        //            throw new ArgumentException("Invalid argument.", "arg");
        //    }
        //}

        //private static string GetFontsParameter(Fonts arg) {
        //    switch (arg) {
        //        case Fonts.DiskFonts:
        //            return "-dDISKFONTS";
        //        case Fonts.LocalFonts:
        //            return "-dLOCALFONTS";
        //        case Fonts.NoCCFonts:
        //            return "-dNOCCFONTS";
        //        case Fonts.NoFontMap:
        //            return "-dNOFONTMAP";
        //        case Fonts.NoFontPath:
        //            return "-dNOFONTPATH";
        //        case Fonts.NoPlatformFonts:
        //            return "-dNOPLATFONTS";
        //        case Fonts.NoNativeFontMap:
        //            return "-dNONATIVEFONTMAP";
        //        case Fonts.FontMap:
        //            return "-sFONTMAP";
        //        case Fonts.FontPath:
        //            return "-sFONTPATH";
        //        case Fonts.SubstituteFont:
        //            return "-sSUBSTFONT";
        //        case Fonts.OldCffParser:
        //            return "-dOLDCFF";
        //        default:
        //            throw new ArgumentException("Invalid argument.", "arg");
        //    }
        //}

        //private static string GetRenderingParameter(Rendering arg) {
        //    switch (arg) {
        //        case Rendering.ColorScreen:
        //            return "-dCOLORSCREEN";
        //        case Rendering.DitherPpi:
        //            return "-dDITHERPPI";
        //        case Rendering.Interpolate:
        //            return "-dDOINTERPOLATE";
        //        case Rendering.NoInterpolate:
        //            return "-dNOINTERPOLATE";
        //        case Rendering.TextAlphaBits:
        //            return "-dTextAlphaBits";
        //        case Rendering.GraphicsAlphaBits:
        //            return "-dGraphicsAlphaBits";
        //        case Rendering.AlignToPixels:
        //            return "-dAlignToPixels";
        //        case Rendering.GridToFitTT:
        //            return "-dGridFitTT";
        //        case Rendering.UseCieColor:
        //            return "-dUseCIEColor";
        //        case Rendering.NoCie:
        //            return "-dNOCIE";
        //        case Rendering.NoSubstituteDeviceColors:
        //            return "-dNOSUBSTDEVICECOLORS";
        //        case Rendering.NoPsicc:
        //            return "-dNOPSICC";
        //        case Rendering.NoTransparency:
        //            return "-dNOTRANSPARENCY";
        //        case Rendering.NoTN5044:
        //            return "-dNO_TN5044";
        //        case Rendering.DoPS:
        //            return "-dDOPS";
        //        default:
        //            throw new ArgumentException("Invalid argument.", "arg");
        //    }
        //}

        //private static string GetResourceParameter(Resource arg) {
        //    switch (arg) {
        //        case Resource.GenericResourceDirectory:
        //            return "-sGenericResourceDir";
        //        case Resource.FontResourceDirectory:
        //            return "-sFontResourceDir";
        //        default:
        //            throw new ArgumentException("Invalid argument.", "arg");
        //    }
        //}

        #endregion
    }
}

