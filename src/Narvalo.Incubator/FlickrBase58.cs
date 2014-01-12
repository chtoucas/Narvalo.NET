namespace Narvalo
 {
     using System;
     using Narvalo.Diagnostics;

     /// <summary>
     /// Base58 inspired by Flickr's service of url shortening.
     /// Base58 is what you get after taking Base62 [a-zA-Z0-9] and removing any character
     /// that may induce to error when introduced by hand: 0 (zero), O (uppercase 'o'), 
     /// I (uppercase 'i'), and l (lowercase 'L'). This concept was introduced to the general 
     /// public by Flickr, which uses the following String:
     /// </summary>
     public static class FlickrBase58
     {
         private const int FlickrBase58AlphabetLength = 58;

         private const long FlickrBase58MaxLength = 11;

         private static readonly char[] FlickrBase58Alphabet = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z', 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 
            'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 
            'W', 'X', 'Y', 'Z',
        };

         public static string Encode(long value)
         {
             Requires.GreaterThanOrEqualTo(value, 0, "value");

             string result = String.Empty;

             while (value > 0) {
                 long r = value % FlickrBase58AlphabetLength;
                 result = FlickrBase58Alphabet[r] + result;
                 value /= FlickrBase58AlphabetLength;
             }

             return result;
         }

         public static long Decode(string value)
         {
             Requires.NotNull(value, "value");
             Requires.LessThanOrEqualTo(value.Length, FlickrBase58MaxLength, "value");

             long result = 0;
             long multiplier  = 1;

             for (int i = value.Length - 1; i >= 0; i--) {
                 int index = Array.IndexOf(FlickrBase58Alphabet, value[i]);
                 if (index == -1) {
                     throw new ArgumentException("Illegal character " + value[i] + " at " + i);
                 }
                 checked {
                     result += multiplier * index;
                     if (i != 0) {
                         multiplier *= FlickrBase58AlphabetLength;
                     }
                 }
             }

             return result;
         }
     }
 }
