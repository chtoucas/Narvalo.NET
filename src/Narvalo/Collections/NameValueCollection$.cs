namespace Narvalo.Collections
{
    using System;
    using System.Collections.Specialized;
    using Narvalo;
    using Narvalo.Fx;

    public static class NameValueCollectionExtensions
    {
        public static Maybe<string> MayGetValue(this NameValueCollection nvc, string param)
        {
            Requires.Object(nvc);

            return nvc.MayGetValues(param)
                // On ne s'attend à ne récupérer qu'une seule valeur.
                .Filter(@_ => @_.Length == 1)
                // Si la condition précédente est satisfaite, on sélectionne l'unique élément du tableau.
                .Map(@_ => @_[0]);
        }

        public static Maybe<string[]> MayGetValues(this NameValueCollection nvc, string param)
        {
            Requires.Object(nvc);

            return Maybe.Create(nvc.GetValues(param));
        }

        public static Maybe<T[]> MayParseAnyValue<T>(
            this NameValueCollection nvc,
            string param,
            MayFunc<string, T> parser)
        {
            Requires.Object(nvc);

            return nvc.MayGetValues(param).Bind(@_ => Maybe.Collect(@_, parser));
        }

        public static Maybe<T> MayParseValue<T>(
            this NameValueCollection nvc,
            string param,
            MayFunc<string, T> parser)
        {
            Requires.Object(nvc);
            Requires.NotNull(parser, "parser");

            return nvc.MayGetValue(param).Bind(_ => parser(_));
        }

        public static Maybe<T[]> MayParseValues<T>(
            this NameValueCollection nvc,
            string param,
            MayFunc<string, T> parser)
        {
            Requires.Object(nvc);

            return nvc.MayGetValues(param).Bind(@_ => Maybe.Map(@_, parser));
        }
    }
}
