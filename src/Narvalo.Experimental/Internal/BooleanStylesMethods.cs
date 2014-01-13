namespace Narvalo.Internal
{
    /// <summary>
    /// Méthodes d'extension pour BooleanStyles.
    /// </summary>
    static class BooleanStylesMethods
    {
        public static bool Contains(this BooleanStyles style, BooleanStyles value)
        {
            return (style & value) == value;
        }
    }
}
