namespace Narvalo.Collections
{
    using System;
    using System.Collections.Specialized;
    using Narvalo;
    using Narvalo.Fx;

    public static class NameValueCollectionExtensions
    {
        public static Maybe<string> MayGetValue(this NameValueCollection @this, string name)
        {
            Requires.Object(@this);

            return @this.MayGetValues(name)
                // On ne s'attend à ne récupérer qu'une seule valeur.
                .Filter(@_ => @_.Length == 1)
                // Si la condition précédente est satisfaite, on sélectionne l'unique élément du tableau.
                .Map(@_ => @_[0]);
        }

        public static Maybe<string[]> MayGetValues(this NameValueCollection @this, string name)
        {
            Requires.Object(@this);

            return Maybe.Create(@this.GetValues(name));
        }

        public static Maybe<T[]> MayParseAnyValue<T>(
            this NameValueCollection @this,
            string name,
            MayFunc<string, T> parser)
        {
            Requires.Object(@this);

            return @this.MayGetValues(name).Bind(@_ => Maybe.Collect(@_, parser));
        }

        public static Maybe<T> MayParseValue<T>(
            this NameValueCollection @this,
            string name,
            MayFunc<string, T> parser)
        {
            Requires.Object(@this);
            Requires.NotNull(parser, "parser");

            return @this.MayGetValue(name).Bind(_ => parser(_));
        }

        public static Maybe<T[]> MayParseValues<T>(
            this NameValueCollection @this,
            string name,
            MayFunc<string, T> parser)
        {
            Requires.Object(@this);

            return @this.MayGetValues(name).Bind(@_ => Maybe.Map(@_, parser));
        }
    }
}
