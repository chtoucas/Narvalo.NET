namespace Narvalo.Internal
{
    using System;
    using Narvalo.Fx;
    using Xunit;

    public static class MayParseHelperFacts
    {
        public static class Parse
        {
            //[Fact]
            //public static void ReturnsNone_ForNullString()
            //{
            //    // Act
            //    Maybe<string> result = MayParse_(null);
            //    // Assert
            //    Assert.False(result.IsSome);
            //}

            //[Fact]
            //public static void ReturnsNone_WhenFailed()
            //{
            //    // Act
            //    Maybe<string> result = MayParse_(String.Empty);
            //    // Assert
            //    Assert.False(result.IsSome);
            //}

            //[Fact]
            //public static void ReturnsSome_WhenSucceed()
            //{
            //    // Arrange
            //    string value = "Une chaîne quelconque";
            //    // Act
            //    Maybe<string> result = MayParse_(value);
            //    // Assert
            //    Assert.True(result.IsSome);
            //    Assert.Equal(value, result.Value);
            //}

            //#region Membres privés

            //static Maybe<string> MayParse_(string value)
            //{
            //    return MayParse.MayParseCore<string>(
            //        value,
            //        (string val, out string result) => {
            //            if (value.Length > 0) {
            //                result = value;
            //                return true;
            //            }
            //            else {
            //                result = null;
            //                return false;
            //            }
            //        }
            //    );
            //}

            //#endregion
        }
    }
}
