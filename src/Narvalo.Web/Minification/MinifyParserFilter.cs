namespace Narvalo.Web.Minification
{
    using System;
    using System.Collections;
    using System.Web.UI;
    using Narvalo.Fx;
    using Narvalo.Web.Internal;
    using Narvalo.Web.UI;

    /// <summary>
    /// Permet de supprimer pendant la phase de compilation d'une page, un certain nombre 
    /// de caractères inutiles, en l'occurence les espaces blancs.
    /// WARNING: Pour éviter les mauvaises surprises, le filtre <code>MinifyLevel.Simple</code>
    /// est actif par défaut. Ceci dit, on ne prend pas de risque, ainsi le filtre 
    /// <code>MinifyLevel.Advanced</code> n'est utilisée que 
    /// si il est explicitement demandé. Il est aussi possible de désactiver complètement
    /// ce filtre en utilisant le mode <code>MinifyLevel.None</code>.
    /// </summary>
    /// <remarks>
    /// Le nom de la classe peut être trompeur. On ne se propose en aucun cas de "minifier"
    /// le HTML. De la manière dont on procède, on n'a accès qu'à certains fragments du 
    /// contenu sans la moindre information contextuelle. On ne peut donc pas prendre
    /// de mesures trop extrèmes.
    /// </remarks>
    public class MinifyParserFilter : UnrestrictedParserFilterBase
    {
        const string LevelDirectiveName = "Minify";

        MinifyLevel _minifyLevel = MinifyLevel.None;

        protected bool IsEnabled
        {
            get
            {
                return _minifyLevel != MinifyLevel.None;
            }
        }

        public override void ParseComplete(ControlBuilder rootBuilder)
        {
            Requires.NotNull(rootBuilder, "rootBuilder");

            if (IsEnabled) {
                RecursivelyMinifyControl_(rootBuilder);
            }

            base.ParseComplete(rootBuilder);
        }

        public override void PreprocessDirective(string directiveName, IDictionary attrs)
        {
            Requires.NotNull(attrs, "attributes");

            if (attrs.Contains(LevelDirectiveName)) {
                MayParse.ToEnum<MinifyLevel>((string)attrs[LevelDirectiveName])
                    .WhenSome(_ => _minifyLevel = _);

                attrs.Remove(LevelDirectiveName);
            }

            base.PreprocessDirective(directiveName, attrs);
        }

        #region Membres privés

        void RecursivelyMinifyControl_(ControlBuilder cb)
        {
            ArrayList childBuilders = cb.GetChildBuilders();

            for (int i = 0; i < childBuilders.Count; i++) {
                string literal = childBuilders[i] as string;
                if (String.IsNullOrEmpty(literal)) {
                    continue;
                }

                childBuilders[i] = Minifier.RemoveWhiteSpaces(literal, _minifyLevel);
            }

            foreach (object childBuilder in childBuilders) {
                var child = childBuilder as ControlBuilder;
                if (child != null) {
                    RecursivelyMinifyControl_(child);
                }
            }

            foreach (TemplatePropertyEntry entry in cb.GetTemplatePropertyEntries()) {
                RecursivelyMinifyControl_(entry.Builder);
            }

            foreach (ComplexPropertyEntry entry in cb.GetComplexPropertyEntries()) {
                RecursivelyMinifyControl_(entry.Builder);
            }

            ControlBuilder defaultPropertyBuilder = cb.GetDefaultPropertyBuilder();
            if (defaultPropertyBuilder != null) {
                RecursivelyMinifyControl_(defaultPropertyBuilder);
            }
        }

        #endregion

        //private bool IsDebuggingEnabled() {
        //    if (HttpContext.Current != null)
        //        return HttpContext.Current.IsDebuggingEnabled;

        //    return ((CompilationSection)WebConfigurationManager.GetSection("system.web/compilation", VirtualPath)).Debug;
        //}
    }
}
