namespace Narvalo.Fx
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /*!
     * Connection to Linq
     * ------------------
     * 
     * To support Linq we only need to create the appropriate methods and the C# compiler will work its 
     * magic. Actually, this is something that we have almost already done. Indeed, this is just a matter of using
     * the right terminology :
     * + Select is the Linq name for the Map method from monads,
     * + SelectMany is the Linq name for the Bind method from monads,
     * + ...
     * We provide the correct aliases inside the Narvalo.Linq namespace.
     * 
     * Nevertheless, since this might look too unusual we also explicitely implement the `IEnumerable<T>` interface.
     */

    public partial class Maybe<T>
    {
        /// <summary />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (IsSome) {
                return new List<T> { _value }.GetEnumerator();
            }
            else {
                return Enumerable.Empty<T>().GetEnumerator();
            }
        }

        /// <summary />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
    }
}
