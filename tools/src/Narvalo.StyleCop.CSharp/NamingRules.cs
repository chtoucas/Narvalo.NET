// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.StyleCop.CSharp
{
    using System;
    using System.Collections.Generic;

    using global::StyleCop;
    using global::StyleCop.CSharp;

    /// <summary>
    /// Checks the names of code elements.
    /// </summary>
    [SourceAnalyzer(typeof(CsParser))]
    public sealed class NamingRules : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument document)
        {
            Param.RequireNotNull(document, "document");

            var csdocument = (CsDocument)document;

            if (csdocument.RootElement != null && !csdocument.RootElement.Generated)
            {
                csdocument.WalkDocument(new CodeWalkerElementVisitor<object>(ProcessElement_), null, null);
            }
        }

        /// <summary>
        /// Processes one element and its children.
        /// </summary>
        /// <param name="element">The element to process.</param>
        /// <returns>Returns false if the analyzer should quit.</returns>
        private bool ProcessElement(CsElement element)
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
                    case ElementType.Method:
                        if (!element.Declaration.Name.StartsWith("operator", StringComparison.Ordinal) && element.Declaration.Name != "foreach")
                        {
                            //CheckCase(element, element.Declaration.Name, element.LineNumber, true);
                        }

                        break;

                    case ElementType.Field:
                        //CheckFieldUnderscores(element);
                        //CheckFieldPrefix(element as Field);

                        break;

                    default:
                        break;
                }
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

                    if (!ProcessElement(child))
                    {
                        return false;
                    }
                }
            }

            //if (!nativeMethods)
            //{
            //    ProcessStatementContainer(element, validPrefixes);
            //}

            return true;
        }

        /// <summary>
        /// Processes the given statement container.
        /// </summary>
        /// <param name="element">
        /// The statement container element to process.
        /// </param>
        /// <param name="validPrefixes">
        /// The list of acceptable Hungarian-type prefixes.
        /// </param>
        private void ProcessStatementContainer(CsElement element, Dictionary<string, string> validPrefixes)
        {
            Param.AssertNotNull(element, "element");
            Param.AssertNotNull(validPrefixes, "validPrefixes");

            // Check the statement container's variables.
            if (element.Variables != null)
            {
                foreach (Variable variable in element.Variables)
                {
                    if (!variable.Generated)
                    {
                        //CheckMethodVariablePrefix(variable, element, validPrefixes);
                        //CheckUnderscores(element, element.Variables);
                    }
                }
            }

            // Check each of the statements under this container.
            foreach (Statement statement in element.ChildStatements)
            {
                ProcessStatement(statement, element, validPrefixes);
            }
        }

        /// <summary>
        /// Processes the given statement.
        /// </summary>
        /// <param name="statement">
        /// The statement to process.
        /// </param>
        /// <param name="element">
        /// The parent element.
        /// </param>
        /// <param name="validPrefixes">
        /// The list of acceptable Hungarian-type prefixes.
        /// </param>
        private void ProcessStatement(Statement statement, CsElement element, Dictionary<string, string> validPrefixes)
        {
            Param.AssertNotNull(statement, "statement");
            Param.AssertNotNull(element, "element");
            Param.AssertNotNull(validPrefixes, "validPrefixes");

            // Check the statement's variables.
            if (statement.Variables != null)
            {
                foreach (Variable variable in statement.Variables)
                {
                    //CheckMethodVariablePrefix(variable, element, validPrefixes);
                    //CheckUnderscores(element, statement.Variables);
                }
            }

            // Check the expressions under this statement.
            //foreach (Expression expression in statement.ChildExpressions)
            //{
            //    ProcessExpression(expression, element, validPrefixes);
            //}

            // Check each of the statements under this statement.
            foreach (Statement childStatement in statement.ChildStatements)
            {
                this.ProcessStatement(childStatement, element, validPrefixes);
            }
        }

        private bool ProcessElement_(CsElement element, CsElement parentElement, object context)
        {
            if (!element.Generated
                && element.ElementType == ElementType.Field
                && element.ActualAccess == AccessModifierType.Private
                && element.Declaration.Name.ToCharArray()[0] != '_')
            {
                AddViolation(element, "PrivateFieldNameMustBeginWithUnderscore");
            }
            //if (!element.Generated
            //    && element.ElementType == ElementType.Method
            //    && element.ActualAccess == AccessModifierType.Private
            //    && !element.Declaration.Name.EndsWith("_"))
            //{
            //    AddViolation(element, "PrivateMethodNamesMustEndWithUnderscore");
            //}

            return true;
        }

        /// <summary>
        /// Checks the case of the first character in the given word.
        /// </summary>
        /// <param name="element">
        /// The element that the word appears in.
        /// </param>
        /// <param name="name">
        /// The word to check.
        /// </param>
        /// <param name="line">
        /// The line that the word appears on.
        /// </param>
        /// <param name="upper">
        /// True if the character should be upper, false if it should be lower.
        /// </param>
        private void CheckCase(CsElement element, string name, int line, bool upper)
        {
            Param.AssertNotNull(element, "element");
            Param.AssertValidString(name, "name");
            Param.AssertGreaterThanZero(line, "line");
            Param.Ignore(upper);

            if (name.Length >= 1)
            {
                char firstLetter = name[0];

                // If the first character is not a letter, then it does not make any sense to check for upper or lower case.
                if (char.IsLetter(firstLetter))
                {
                    if (upper)
                    {
                        if (Char.IsLower(firstLetter))
                        {
                            // We check for IsLower and not for !isUpper. This is for cultures that don't have Upper or Lower case
                            // letters like Chinese.
                            //AddViolation(element, line, Rules.ElementMustBeginWithUpperCaseLetter, element.FriendlyTypeText, name);
                        }
                    }
                    else
                    {
                        if (Char.IsUpper(firstLetter))
                        {
                            //this.AddViolation(element, line, Rules.ElementMustBeginWithLowerCaseLetter, element.FriendlyTypeText, name);
                        }
                    }
                }
            }
        }
    }
}
