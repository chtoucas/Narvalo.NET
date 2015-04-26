namespace Narvalo.StyleCop.CSharp
{
    using System;

    using global::StyleCop;
    using global::StyleCop.CSharp;

    [SourceAnalyzer(typeof(CsParser))]
    public sealed class CustomRules : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument document)
        {
            Param.RequireNotNull(document, "document");

            var csdocument = (CsDocument)document;

            if (csdocument.RootElement != null && !csdocument.RootElement.Generated)
            {
                // csdocument.WalkDocument(new CodeWalkerElementVisitor<object>(ProcessElement_), null, null);
                ProcessElement(csdocument.RootElement, nativeMethods: false);
            }
        }

        private bool ProcessElement(CsElement element, bool nativeMethods)
        {
            Param.AssertNotNull(element, "element");

            if (Cancel)
            {
                return false;
            }

            if (!element.Generated && element.Declaration != null && element.Declaration.Name != null)
            {
                switch (element.ElementType)
                {
                    case ElementType.Field:
                        if (!nativeMethods)
                        {
                            CheckField_(element as Field);
                        }

                        break;

                    case ElementType.Method:
                        if (!nativeMethods
                            && !element.Declaration.Name.StartsWith("operator", StringComparison.Ordinal)
                            && element.Declaration.Name != "foreach")
                        {
                            CheckMethod_(element);
                        }

                        break;

                    case ElementType.Class:
                        if (!nativeMethods)
                        {
                            CheckClass_(element);
                        }

                        break;

                    default:
                        break;
                }
            }

            if (!nativeMethods && (element.ElementType == ElementType.Class || element.ElementType == ElementType.Struct)
                && element.Declaration.Name.EndsWith("NativeMethods", StringComparison.Ordinal))
            {
                nativeMethods = true;
            }

            bool doneAccessor = false;
            foreach (CsElement child in element.ChildElements)
            {
                if ((element.ElementType == ElementType.Indexer && !doneAccessor) || element.ElementType != ElementType.Indexer)
                {
                    if (child.ElementType == ElementType.Accessor)
                    {
                        doneAccessor = true;
                    }

                    if (!ProcessElement(child, nativeMethods))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void CheckClass_(CsElement element)
        {
            Param.AssertNotNull(element, "element");

            var className = element.Declaration.Name;

            // By-pass debug view classes.
            if (className == "DebugView")
            {
                return;
            }

            if (
                element.AccessModifier == AccessModifierType.Private
                && !className.EndsWith("_", StringComparison.OrdinalIgnoreCase))
            {
                AddViolation(
                    element,
                    element.LineNumber,
                    Rules.PrivateNestedClassesMustEndWithUnderscore,
                    element.FriendlyTypeText,
                    className);
            }
        }

        private void CheckMethod_(CsElement element)
        {
            Param.AssertNotNull(element, "element");

            var methodName = element.Declaration.Name;

            if (methodName == "η"
                || methodName == "μ")
            {
                // By-pass Monad special methods.
                return;
            }

            // Remove the generic part in the name.
            var indexOfBracket = methodName.IndexOf('<');
            var cleanName = indexOfBracket != -1
                ? methodName.Substring(0, indexOfBracket)
                : methodName;

            if (element.AccessModifier == AccessModifierType.Private
                && !cleanName.EndsWith("_", StringComparison.OrdinalIgnoreCase))
            {
                AddViolation(
                    element,
                    element.LineNumber,
                    Rules.PrivateMethodsMustEndWithUnderscore,
                    element.FriendlyTypeText,
                    methodName);
            }

            if (
                element.AccessModifier == AccessModifierType.Internal
                && cleanName.EndsWith("Internal", StringComparison.Ordinal))
            {
                AddViolation(
                    element,
                    element.LineNumber,
                    Rules.InternalMethodsMustNotEndWithInternal,
                    element.FriendlyTypeText,
                    methodName);
            }
        }

        private void CheckField_(Field field)
        {
            Param.AssertNotNull(field, "field");

            if (field.AccessModifier != AccessModifierType.Private)
            {
                return;
            }

            var fieldName = field.Declaration.Name;

            if (field.Static)
            {
                if (!fieldName.StartsWith("s_", StringComparison.Ordinal))
                {
                    AddViolation(
                        field,
                        field.LineNumber,
                        Rules.PrivateStaticFieldsMustBeCorrectlyPrefixed,
                        field.FriendlyTypeText,
                        fieldName);
                }
            }
            else if (field.Const)
            {
                foreach (var c in fieldName)
                {
                    if (c == '_' || Char.IsNumber(c) || Char.IsUpper(c))
                    {
                        continue;
                    }

                    AddViolation(
                        field,
                        field.LineNumber,
                        Rules.PrivateConstsMustOnlyContainUppercaseLettersDigitsAndUnderscores,
                        field.FriendlyTypeText,
                        fieldName);
                    break;
                }
            }
            else
            {
                if (!fieldName.StartsWith("_", StringComparison.OrdinalIgnoreCase))
                {
                    AddViolation(
                        field,
                        field.LineNumber,
                        Rules.PrivateFieldsMustBeginWithUnderscore,
                        field.FriendlyTypeText,
                        fieldName);
                }
            }
        }
    }
}
