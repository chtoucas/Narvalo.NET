namespace Narvalo.GhostScript.Options
{
    public class PdfArgs : GhostScriptArgs<PdfDevice>
    {
        public PdfArgs(string inputFile)
            : base(inputFile, new PdfDevice())
        {
        }
    }
}
