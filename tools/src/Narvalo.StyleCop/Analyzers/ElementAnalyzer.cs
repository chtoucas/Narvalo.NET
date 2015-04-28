// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Analyzers
{
    using System;
    using System.IO;

    using global::StyleCop;
    using global::StyleCop.CSharp;

    public sealed class ElementAnalyzer : CSharpAnalyzer
    {
        public ElementAnalyzer(SourceAnalyzer sourceAnalyzer) : base(sourceAnalyzer) { }

        protected override void AnalyzeDocumentCore(CsDocument document, bool userCode)
        {
            Param.AssertNotNull(document, "document");

            ProcessElement_(document.RootElement, nativeMethods: false, userCode: userCode);
        }

        private bool ProcessElement_(CsElement element, bool nativeMethods, bool userCode)
        {
            Param.AssertNotNull(element, "element");

            if (SourceAnalyzer.Cancel)
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
                            CheckFieldName_(element as Field);
                        }

                        break;

                    case ElementType.Method:
                        if (!nativeMethods
                            && !element.Declaration.Name.StartsWith("operator", StringComparison.OrdinalIgnoreCase)
                            && element.Declaration.Name != "foreach")
                        {
                            CheckMethodName_(element);
                        }

                        break;

                    case ElementType.Class:
                    case ElementType.Struct:
                    case ElementType.Interface:
                        if (!nativeMethods)
                        {
                            CheckClassName_(element, userCode);
                        }

                        break;

                    default:
                        break;
                }
            }

            if (!nativeMethods && (element.ElementType == ElementType.Class || element.ElementType == ElementType.Struct)
                && element.Declaration.Name.EndsWith("NativeMethods", StringComparison.OrdinalIgnoreCase))
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

                    if (!ProcessElement_(child, nativeMethods, userCode))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void CheckClassName_(CsElement element, bool userCode)
        {
            Param.AssertNotNull(element, "element");

            var className = element.Declaration.Name;

            // Skip debug view classes and AssemblyInfo class.
            if (className == "DebugView" || className == "AssemblyInfo")
            {
                return;
            }

            var name = AnalyzerUtility.TrimGenericInfoFromElementName(className);

            if (element.AccessModifier == AccessModifierType.Private)
            {
                if (!name.EndsWith("_", StringComparison.OrdinalIgnoreCase))
                {
                    SourceAnalyzer.AddViolation(
                        element,
                        element.LineNumber,
                        RuleName.PrivateClassNamesMustEndWithUnderscore,
                        element.FriendlyTypeText,
                        className);
                }
            }
            else if (userCode)
            {
                CsElement parentElement = element.FindParentElement();
                bool isRootClass = parentElement.ElementType != ElementType.Class;

                if (isRootClass)
                {
                    string fileName = Path.GetFileNameWithoutExtension(element.Document.SourceCode.Path);

                    if (fileName.EndsWith("$", StringComparison.OrdinalIgnoreCase))
                    {
                        if (name != fileName.TrimEnd('$') + "Extensions")
                        {
                            SourceAnalyzer.AddViolation(
                                element,
                                element.LineNumber,
                                RuleName.FileNamesMustMatchExtensionClassNamesFollowedByDollarSign,
                                element.FriendlyTypeText,
                                className);
                        }
                    }
                    else
                    {
                        string cleanfileName = AnalyzerUtility.TrimGenericInfoFromFilename(fileName);

                        if (name != cleanfileName)
                        {
                            SourceAnalyzer.AddViolation(
                                element,
                                element.LineNumber,
                                RuleName.FileNamesMustMatchTypeNames,
                                element.FriendlyTypeText,
                                className);
                        }
                    }
                }
            }

            foreach (var token in element.ElementTokens)
            {
                if (token.CsTokenType == CsTokenType.Abstract)
                {
                    if (name.EndsWith("Base", StringComparison.OrdinalIgnoreCase))
                    {
                        SourceAnalyzer.AddViolation(
                            element,
                            element.LineNumber,
                            RuleName.AbstractClassNamesMustNotEndWithBase,
                            element.FriendlyTypeText,
                            className);
                    }

                    break;
                }
            }
        }

        private void CheckMethodName_(CsElement element)
        {
            Param.AssertNotNull(element, "element");

            var methodName = element.Declaration.Name;

            // Skip Monad special methods.
            if (methodName == "η" || methodName == "μ")
            {
                return;
            }

            var name = AnalyzerUtility.TrimGenericInfoFromElementName(methodName);

            if (element.AccessModifier == AccessModifierType.Private
                && name.EndsWith("_", StringComparison.OrdinalIgnoreCase))
            {
                SourceAnalyzer.AddViolation(
                    element,
                    element.LineNumber,
                    RuleName.PrivateMethodNamesMustNotEndWithUnderscore,
                    element.FriendlyTypeText,
                    methodName);
            }

            if (
                element.AccessModifier == AccessModifierType.Internal
                && name.EndsWith("Internal", StringComparison.OrdinalIgnoreCase))
            {
                SourceAnalyzer.AddViolation(
                    element,
                    element.LineNumber,
                    RuleName.InternalMethodNamesMustNotEndWithInternal,
                    element.FriendlyTypeText,
                    methodName);
            }
        }

        private void CheckFieldName_(Field field)
        {
            Param.AssertNotNull(field, "field");

            if (field.AccessModifier != AccessModifierType.Private)
            {
                return;
            }

            var fieldName = field.Declaration.Name;

            if (field.Static)
            {
                if (!fieldName.StartsWith("s_", StringComparison.OrdinalIgnoreCase))
                {
                    SourceAnalyzer.AddViolation(
                        field,
                        field.LineNumber,
                        RuleName.PrivateStaticFieldNamesMustBeCorrectlyPrefixed,
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

                    SourceAnalyzer.AddViolation(
                        field,
                        field.LineNumber,
                        RuleName.PrivateConstNamesMustOnlyContainUppercaseLettersDigitsAndUnderscores,
                        field.FriendlyTypeText,
                        fieldName);
                    break;
                }
            }
            else
            {
                if (!fieldName.StartsWith("_", StringComparison.OrdinalIgnoreCase))
                {
                    SourceAnalyzer.AddViolation(
                        field,
                        field.LineNumber,
                        RuleName.PrivateFieldNamesMustBeginWithUnderscore,
                        field.FriendlyTypeText,
                        fieldName);
                }
            }
        }
    }
}
