namespace Narvalo.GhostScript.Options
{
    public class TextArgs : GhostScriptArgs<TextDevice>
    {
        public TextArgs(string inputFile)
            : base(inputFile, new TextDevice())
        {
        }
    }
}
