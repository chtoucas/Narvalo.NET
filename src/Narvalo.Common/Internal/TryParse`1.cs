namespace Narvalo.Internal
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "La méthode retourne un booléen pour indiquer le succès ou l'échec.")]
    delegate bool TryParse<TResult>(string value, out TResult result);
}
