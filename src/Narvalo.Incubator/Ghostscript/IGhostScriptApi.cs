namespace Narvalo.GhostScript
{
    using Narvalo.GhostScript.Options;

    public interface IGhostScriptApi
    {
        void Execute<T>(GhostScriptArgs<T> args) where T : Device;
        void Execute(string[] args);
    }
}
