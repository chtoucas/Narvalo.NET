namespace Narvalo
{
    using System;

    [Flags]
    [Serializable]
    public enum BooleanStyles
    {
        None = 0,
        Literal = 1 << 0,
        Integer = 1 << 1,
        EmptyIsFalse = 1 << 2,
        HtmlInput = 1 << 3,

        Default = Literal | Integer | EmptyIsFalse,
        Any = Literal | Integer | EmptyIsFalse | HtmlInput,
    }
}