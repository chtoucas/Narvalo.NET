namespace Narvalo.GhostScript.Options
{
    public class PrinterArgs : GhostScriptArgs<PrinterDevice>
    {
        public PrinterArgs(string inputFile)
            : base(inputFile, new PrinterDevice())
        {
        }
    }
}
