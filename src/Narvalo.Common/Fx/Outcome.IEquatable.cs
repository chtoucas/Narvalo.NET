namespace Narvalo.Fx
{
    public partial struct Outcome
    {
        /// <summary />
        public static bool operator ==(Outcome left, Outcome right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Outcome left, Outcome right)
        {
            return !left.Equals(right);
        }

        /// <summary />
        public bool Equals(Outcome other)
        {
            return Successful || (Unsuccessful && _exception.Equals(other._exception));
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (!(obj is Outcome)) {
                return false;
            }

            return Equals((Outcome)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            return Successful ? 0 : _exception.GetHashCode();
        }
    }
}
