// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Web.WebPages;
    using System.Web.WebPages.Instrumentation;

    // A mix of codes borrowed from Nocco and RazorEngine.
    // Cf.
    // https://github.com/Antaris/RazorEngine/blob/master/src/Core/RazorEngine.Core/Templating/TemplateBase.cs

    public abstract class TemplateBase
    {
        protected TemplateBase()
        {
            Buffer = new StringBuilder();
        }

        public string Title { get; set; }
        public IEnumerable<HtmlBlock> Blocks { get; set; }

        public StringBuilder Buffer { get; set; }

        public abstract void Execute();

        public virtual void Write(object value)
        {
            WriteLiteral(value);
        }

        public virtual void WriteLiteral(object value)
        {
            Buffer.Append(value);
        }

        public virtual void WriteAttribute(
            string name,
            PositionTagged<string> prefix,
            PositionTagged<string> suffix,
            params AttributeValue[] values)
        {
            bool first = true;
            bool wroteSomething = false;
            if (values.Length == 0) {
                // Explicitly empty attribute, so write the prefix and suffix
                WritePositionTaggedLiteral(prefix);
                WritePositionTaggedLiteral(suffix);
            }
            else {
                for (int i = 0; i < values.Length; i++) {
                    AttributeValue attrVal = values[i];
                    PositionTagged<object> val = attrVal.Value;
                    PositionTagged<string> next = i == values.Length - 1 ?
                        suffix : // End of the list, grab the suffix
                        values[i + 1].Prefix; // Still in the list, grab the next prefix

                    bool? boolVal = null;
                    if (val.Value is bool) {
                        boolVal = (bool)val.Value;
                    }

                    if (val.Value != null && (boolVal == null || boolVal.Value)) {
                        string valStr = val.Value as string;
                        if (valStr == null) {
                            valStr = val.Value.ToString();
                        }
                        if (boolVal != null) {
                            Debug.Assert(boolVal.Value);
                            valStr = name;
                        }

                        if (first) {
                            WritePositionTaggedLiteral(prefix);
                            first = false;
                        }
                        else {
                            WritePositionTaggedLiteral(attrVal.Prefix);
                        }

                        // Calculate length of the source span by the position of the next value (or suffix)
                        int sourceLength = next.Position - attrVal.Value.Position;

                        if (attrVal.Literal) {
                            WriteLiteral(valStr);
                        }
                        else {
                            Write(valStr); // Write value
                        }
                        wroteSomething = true;
                    }
                }
                if (wroteSomething)
                    WritePositionTaggedLiteral(suffix);
            }
        }

        void WritePositionTaggedLiteral(string value, int position)
        {
            WriteLiteral(value);
        }

        void WritePositionTaggedLiteral(PositionTagged<string> value)
        {
            WritePositionTaggedLiteral(value.Value, value.Position);
        }
    }
}
