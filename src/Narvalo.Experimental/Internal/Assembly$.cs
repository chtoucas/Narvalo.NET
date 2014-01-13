namespace Narvalo.Internal
{
    using System.Reflection;
    using Narvalo;

	/// <summary>
	/// Extension methods for Assembly class
	/// </summary>
	static class AssemblyExtensions
	{
		/// <summary>
		/// Returns the short name of an assembly in a way that is safe in medium trust
		/// </summary>
		/// <param name="assembly">The assembly to return the name for</param>
		/// <returns>The assembly short name</returns>
        public static string GetSafeName(this Assembly assembly)
		{
            Requires.NotNull(assembly, "assembly");

			return new AssemblyName(assembly.FullName).Name;
		}
	}
}
