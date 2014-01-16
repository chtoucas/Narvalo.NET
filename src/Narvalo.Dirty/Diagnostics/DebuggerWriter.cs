namespace Narvalo.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;

    // TODO: Surcharger les autres méthodes de TextWriter ?
    public class DebuggerWriter : TextWriter
    {
        static readonly UnicodeEncoding Encoding_
           = new UnicodeEncoding(false /* bigEndian */, false /* byteOrderMark */);

        readonly int _level;
        readonly string _category;

        public DebuggerWriter()
            : this(0, Debugger.DefaultCategory, CultureInfo.CurrentCulture) { }

        public DebuggerWriter(int level, string category)
            : this(level, category, CultureInfo.CurrentCulture) { }

        public DebuggerWriter(int level, string category, IFormatProvider formatProvider)
            : base(formatProvider)
        {
            _level = level;
            _category = category;
        }

        public string Category { get { return _category; } }

        public override Encoding Encoding { get { return Encoding_; } }

        public int Level { get { return _level; } }

        public override void Write(string value)
        {
            Debugger.Log(_level, _category, value);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Debugger.Log(_level, _category, new String(buffer, index, count));
        }
    }
}
