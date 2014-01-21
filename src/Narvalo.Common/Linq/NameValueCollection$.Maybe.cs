namespace Narvalo.Linq
{
    using System;
    using System.Collections.Specialized;
    using Narvalo;
    using Narvalo.Fx;

    public static partial class NameValueCollectionExtensions
    {
        public static Maybe<string> MayGetValue(this NameValueCollection @this, string name)
        {
            Require.Object(@this);

            return @this.MayGetValues(name)
                .Filter(@_ => @_.Length == 1)   // On ne s'attend à récupérer qu'une seule valeur.
                .Map(@_ => @_[0]);              // Si la condition précédente est satisfaite, on sélectionne l'unique élément du tableau.
        }

        public static Maybe<string[]> MayGetValues(this NameValueCollection @this, string name)
        {
            Require.Object(@this);

            return Maybe.Create(@this.GetValues(name));
        }

        public static Maybe<T[]> MayParseAnyValue<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);

            return @this.MayGetValues(name).Bind(@_ => Maybe.Collect(@_, parserM));
        }

        public static Maybe<T> MayParseValue<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);
            Require.NotNull(parserM, "parserM");

            return @this.MayGetValue(name).Bind(_ => parserM.Invoke(_));
        }

        public static Maybe<T[]> MayParseValues<T>(
            this NameValueCollection @this,
            string name,
            Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);

            return @this.MayGetValues(name).Bind(@_ => Maybe.Map(@_, parserM));
        }
    }
}
