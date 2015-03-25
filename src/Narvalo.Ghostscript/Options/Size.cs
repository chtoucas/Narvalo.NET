// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System;

    public struct Size : IEquatable<Size>
    {
        private readonly int _width;
        private readonly int _height;

        public Size(int dpi)
        {
            _width = dpi;
            _height = dpi;
        }

        public Size(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public int Width { get { return _width; } }

        public int Height { get { return _height; } }

        public bool IsSquare { get { return _width == _height; } }

        public static bool operator ==(Size left, Size right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Size left, Size right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Size other)
        {
            return _width == other._width
                && _height == other._height;
        }

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
            hash = (23 * hash) + Width.GetHashCode();
            hash = (23 * hash) + Height.GetHashCode();
            return hash;
        }
    }
}
