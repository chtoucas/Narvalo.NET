namespace Narvalo.Oueb
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Web.WebPages;
    using System.Web.WebPages.Instrumentation;

    public abstract class TemplateBase
    {
        protected TemplateBase()
        {
            Buffer = new StringBuilder();
        }

        // Properties available from within the template
        public string Title { get; set; }
        public string PathToCss { get; set; }
        public string PathToJs { get; set; }
        public Func<string, string> GetSourcePath { get; set; }
        public List<Section> Sections { get; set; }
        public List<string> Sources { get; set; }
        public StringBuilder Buffer { get; set; }

        // This `Execute` function will be defined in the inheriting template
        // class. It generates the HTML by calling `Write` and `WriteLiteral`.
        public abstract void Execute();

        public virtual void Write(object value)
        {
            WriteLiteral(value);
        }

        public virtual void WriteLiteral(object value)
        {
            Buffer.Append(value);
        }

        public virtual void WriteAttribute(string name, PositionTagged<string> prefix,
                                           PositionTagged<string> suffix, params AttributeValue[] values)
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
