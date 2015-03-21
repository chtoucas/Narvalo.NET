// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public static class OptionsExtensions
    {
        private static readonly IEnumerable<Interactions> InteractionsList
            = Enum.GetValues(typeof(Interactions)).Cast<Interactions>();

        private static readonly IEnumerable<Miscs> MiscsList
            = Enum.GetValues(typeof(Miscs)).Cast<Miscs>();

        public static void AddTo(this Eps eps, ICollection<string> args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("interactions");
            }

            args.Add(GetParameter(eps));
        }

        public static void AddTo(this Interactions interactions, ICollection<string> args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("interactions");
            }

            foreach (var value in InteractionsList)
            {
                if ((interactions & value) == value)
                {
                    args.Add(GetParameter(value));
                }
            }
        }

        public static void AddTo(this Miscs miscs, ICollection<string> args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("miscs");
            }

            foreach (var value in MiscsList)
            {
                if ((miscs & value) == value)
                {
                    args.Add(GetParameter(value));
                }
            }
        }

        private static string GetParameter(Eps eps)
        {
            switch (eps)
            {
                case Eps.EpsCrop:
                    return "-dEPSCrop";
                case Eps.EppsFitPage:
                    return "-dEPSFitPage";
                case Eps.NoEps:
                    return "-dNOEPS";
                case Eps.None:
                default:
                    throw new InvalidEnumArgumentException("Unsupported eps.");
            }
        }

        private static string GetParameter(Interactions interactions)
        {
            switch (interactions)
            {
                case Interactions.Batch:
                    return "-dBATCH";
                case Interactions.NoPagePrompt:
                    return "-dNOPAGEPROMPT";
                case Interactions.NoPause:
                    return "-dNOPAUSE";
                case Interactions.NoPrompt:
                    return "-dNOPROMPT";
                case Interactions.Quiet:
                    return "-dQUIET";
                case Interactions.QuietStartup:
                    return "-q";
                case Interactions.ShortErrors:
                    return "-dSHORTERRORS";

                // case Interaction.StandardOutput:
                //    return "-sstdout";
                case Interactions.TtyPause:
                    return "-dTTYPAUSE";
                case Interactions.None:
                default:
                    throw new InvalidEnumArgumentException("Unsupported interactions.");
            }
        }

        private static string GetParameter(Miscs miscs)
        {
            switch (miscs)
            {
                case Miscs.DelayBind:
                    return "-dDELAYBIND";
                case Miscs.PdfMarks:
                    return "-dDOPDFMARKS";
                case Miscs.JobServer:
                    return "-dJOBSERVER";
                case Miscs.NoBind:
                    return "-dNOBIND";
                case Miscs.NoCache:
                    return "-dNOCACHE";
                case Miscs.NoGC:
                    return "-dNOGC";
                case Miscs.NoOuterSave:
                    return "-dNOOUTERSAVE";
                case Miscs.NoSafer:
                    return "-dNOSAFER";
                case Miscs.Safer:
                    return "-dSAFER";
                case Miscs.Strict:
                    return "-dSTRICT";
                case Miscs.SystemDictionaryWritable:
                    return "-dWRITESYSTEMDICT";
                case Miscs.None:
                default:
                    throw new InvalidEnumArgumentException("Unsupported miscs.");
            }
        }
    }
}
