namespace Narvalo
{
    using System;

    [Flags]
    public enum EnumStyles
    {
        None = 0,
        Literal = 1 << 0,
        Integer = 1 << 1,
        AllowLeadingWhite = 1 << 2,
        AllowTrailingWhite = 1 << 3,

        Any = Literal | Integer | AllowWhiteSpaces,
        AllowWhiteSpaces = AllowLeadingWhite | AllowTrailingWhite,
    }
}
