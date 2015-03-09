// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.StyleCop.CSharp
{
    using global::StyleCop;
    using global::StyleCop.CSharp;

    /// <summary>
    /// This StyleCop Rule makes sure that private instance fields are prefixed with an underscore.
    /// </summary>
    [SourceAnalyzer(typeof(CsParser))]
    public class NarvaloRules : SourceAnalyzer
    {
        public NarvaloRules()
            : base()
        {
        }

        public override void AnalyzeDocument(CodeDocument document)
        {
            var csdocument = (CsDocument)document;

            if (csdocument.RootElement != null && !csdocument.RootElement.Generated) {
                csdocument.WalkDocument(new CodeWalkerElementVisitor<object>(VisitElement_), null, null);
            }
        }

        bool VisitElement_(CsElement element, CsElement parentElement, object context)
        {
            // Flag a violation if the instance variables are not prefixed with an underscore.
            if (!element.Generated
                && element.ElementType == ElementType.Field
                && element.ActualAccess == AccessModifierType.Private
                ////&& element.ActualAccess != AccessModifierType.Public
                ////&& element.ActualAccess != AccessModifierType.Internal
                && element.Declaration.Name.ToCharArray()[0] != '_') {
                AddViolation(element, "PrivateFieldNamesMustBeginWithUnderscore");
            }

            return true;
        }
    }
}
