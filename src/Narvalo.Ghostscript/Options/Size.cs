// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System;

    public struct Size : IEquatable<Size>
    {
        public Size(int dpi)
        {
            Width = dpi;
            Height = dpi;
        }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        public bool IsSquare => Width == Height;

        public static bool operator ==(Size left, Size right) => left.Equals(right);

        public static bool operator !=(Size left, Size right) => !left.Equals(right);

        public bool Equals(Size other) => Width == other.Width && Height == other.Height;

        public override bool Equals(object obj)
        {
            if (!(obj is Size))
            {
                return false;
            }

            return Equals((Size)obj);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = (31 * hash) + Width.GetHashCode();
            hash = (31 * hash) + Height.GetHashCode();
            return hash;
        }
    }
}
