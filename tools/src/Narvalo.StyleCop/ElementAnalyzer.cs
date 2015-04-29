// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.IO;

    using StyleCop;
    using StyleCop.CSharp;

    public sealed class ElementAnalyzer : CSharpAnalyzer
    {
        public ElementAnalyzer(SourceAnalyzer sourceAnalyzer) : base(sourceAnalyzer) { }

        public override void Analyze(CsDocument document, bool userCode)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            VisitElement(document.RootElement, nativeMethods: false, userCode: userCode);
        }

        private static bool IgnoreClass(string className)
        {
            // Skip debug view classes and AssemblyInfo class.
            return className == "DebugView"
                || className == "AssemblyInfo";
        }

        private static bool IgnoreMethod(string methodName)
        {
            Param.AssertNotNull(methodName, "methodName");

            // Skip monad special methods.
            return methodName == "foreach"
                || methodName == "η"
                || methodName == "μ"
                || methodName.StartsWith("operator", StringComparison.OrdinalIgnoreCase);
        }

        private bool VisitElement(CsElement element, bool nativeMethods, bool userCode)
        {
            Param.AssertNotNull(element, "element");

            if (SourceAnalyzer.Cancel)
            {
                return false;
            }

            if (!nativeMethods
                && !element.Generated
                && element.Declaration != null
                && element.Declaration.Name != null)
            {
                CheckElement(element, userCode);
            }

            if (!nativeMethods
                && (element.ElementType == ElementType.Class || element.ElementType == ElementType.Struct)
                && element.Declaration.Name.EndsWith("NativeMethods", StringComparison.OrdinalIgnoreCase))
            {
                nativeMethods = true;
            }

            bool doneAccessor = false;
            foreach (CsElement child in element.ChildElements)
            {
                if ((element.ElementType == ElementType.Indexer && !doneAccessor)
                    || element.ElementType != ElementType.Indexer)
                {
                    if (child.ElementType == ElementType.Accessor)
                    {
                        doneAccessor = true;
                    }

                    if (!VisitElement(child, nativeMethods, userCode))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void CheckElement(CsElement element, bool userCode)
        {
            Param.AssertNotNull(element, "element");

            switch (element.ElementType)
            {
                case ElementType.Field:
                    CheckFieldName(element as Field);
                    break;

                case ElementType.Method:
                    CheckMethodName(element);
                    break;

                case ElementType.Class:
                case ElementType.Struct:
                    CheckClassName(element, userCode);
                    break;

                default:
                    break;
            }
        }

        #region Class Names

        private void CheckClassName(CsElement element, bool userCode)
        {
            Param.AssertNotNull(element, "element");

            var className = element.Declaration.Name;

            if (IgnoreClass(className)) { return; }

            var baseName = AnalyzerUtility.TrimGenericInfoFromElementName(className);

            CheckPrivateClassName(element, className, baseName);
            CheckFileNameMatchesClassName(element, className, baseName, userCode);
            CheckAbstractClassName(element, className, baseName);
        }

        private void CheckPrivateClassName(CsElement element, string className, string baseName)
        {
            Param.AssertNotNull(element, "element");
            Param.AssertNotNull(baseName, "baseName");

            if (element.AccessModifier != AccessModifierType.Private) { return; }

            if (!baseName.EndsWith("_", StringComparison.OrdinalIgnoreCase))
            {
                SourceAnalyzer.AddViolation(
                    element,
                    element.LineNumber,
                    RuleName.PrivateClassNamesMustEndWithUnderscore,
                    element.FriendlyTypeText,
                    className);
            }
        }

        private void CheckFileNameMatchesClassName(CsElement element, string className, string baseName, bool userCode)
        {
            Param.AssertNotNull(element, "element");

            // Skip non-user code.
            if (!userCode) { return; }

            // Skip private classes.
            if (element.AccessModifier == AccessModifierType.Private) { return; }

            // Skip nested classes.
            CsElement parentElement = element.FindParentElement();
            if (parentElement != null && parentElement.ElementType == ElementType.Class) { return; }

            string fileName = Path.GetFileNameWithoutExtension(element.Document.SourceCode.Path);

            if (fileName.EndsWith("$", StringComparison.OrdinalIgnoreCase))
            {
                string expectedName = fileName.TrimEnd('$') + "Extensions";

                if (baseName != expectedName)
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
                string baseFileName = AnalyzerUtility.TrimGenericInfoFromFileName(fileName);

                if (baseName != baseFileName)
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

        private void CheckAbstractClassName(CsElement element, string className, string baseName)
        {
            Param.AssertNotNull(element, "element");
            Param.AssertNotNull(baseName, "baseName");

            foreach (var token in element.ElementTokens)
            {
                if (token.CsTokenType == CsTokenType.Abstract)
                {
                    if (baseName.EndsWith("Base", StringComparison.OrdinalIgnoreCase))
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

        #endregion

        #region Method Names

        private void CheckMethodName(CsElement element)
        {
            Param.AssertNotNull(element, "element");

            var methodName = element.Declaration.Name;

            if (IgnoreMethod(methodName)) { return; }

            var baseName = AnalyzerUtility.TrimGenericInfoFromElementName(methodName);

            CheckPrivateMethodName(element, methodName, baseName);
            CheckInternalMethodName(element, methodName, baseName);
        }

        private void CheckPrivateMethodName(CsElement element, string methodName, string baseName)
        {
            Param.AssertNotNull(element, "element");
            Param.AssertNotNull(baseName, "baseName");

            if (element.AccessModifier != AccessModifierType.Private) { return; }

            if (baseName.EndsWith("_", StringComparison.OrdinalIgnoreCase))
            {
                SourceAnalyzer.AddViolation(
                    element,
                    element.LineNumber,
                    RuleName.PrivateMethodNamesMustNotEndWithUnderscore,
                    element.FriendlyTypeText,
                    methodName);
            }
        }

        private void CheckInternalMethodName(CsElement element, string methodName, string baseName)
        {
            Param.AssertNotNull(element, "element");
            Param.AssertNotNull(baseName, "baseName");

            if (element.AccessModifier != AccessModifierType.Internal) { return; }

            if (baseName.EndsWith("Internal", StringComparison.OrdinalIgnoreCase))
            {
                SourceAnalyzer.AddViolation(
                    element,
                    element.LineNumber,
                    RuleName.InternalMethodNamesMustNotEndWithInternal,
                    element.FriendlyTypeText,
                    methodName);
            }
        }

        #endregion

        #region Field Names

        private void CheckFieldName(Field field)
        {
            CheckPrivateFieldName(field);
        }

        private void CheckPrivateFieldName(Field field)
        {
            Param.AssertNotNull(field, "field");

            if (field.AccessModifier != AccessModifierType.Private) { return; }

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

        #endregion
    }
}
