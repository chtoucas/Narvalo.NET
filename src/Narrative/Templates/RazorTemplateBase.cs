// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narrative.Templates
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Web.WebPages;
    using System.Web.WebPages.Instrumentation;

    // A mix of codes borrowed from Nocco and RazorEngine.
    // Cf.
    // https://github.com/Antaris/RazorEngine/blob/master/src/Core/RazorEngine.Core/Templating/TemplateBase.cs

    public abstract class RazorTemplateBase
    {
        readonly StringBuilder _buffer;

        protected RazorTemplateBase()
        {
            _buffer = new StringBuilder();
        }

        public string Title { get; set; }

        public IEnumerable<RazorTemplateBlock> Blocks { get; set; }

        public StringBuilder Buffer { get { return _buffer; } }

        public abstract void Execute();

        public virtual void Write(object value)
        {
            WriteLiteral(value);
        }

        public virtual void WriteLiteral(object value)
        {
            _buffer.Append(value);
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
                WritePositionTagged_(prefix);
                WritePositionTagged_(suffix);
            }
            else {
                for (int i = 0; i < values.Length; i++) {
                    AttributeValue attrVal = values[i];
                    PositionTagged<object> val = attrVal.Value;

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
                            WritePositionTagged_(prefix);
                            first = false;
                        }
                        else {
                            WritePositionTagged_(attrVal.Prefix);
                        }

                        if (attrVal.Literal) {
                            WriteLiteral(valStr);
                        }
                        else {
                            Write(valStr);
                        }

                        wroteSomething = true;
                    }
                }

                if (wroteSomething) {
                    WritePositionTagged_(suffix);
                }
            }
        }

        void WritePositionTagged_(PositionTagged<string> value)
        {
            WriteLiteral(value.Value);
        }
    }
}
