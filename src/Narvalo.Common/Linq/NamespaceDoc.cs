// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Runtime.CompilerServices;

    /*!
     * Aliases
     * -------
     * 
     * Linq            | Monad
     * ----------------+----------------
     * Where (*)       | Filter
     * Select (*)      | Map
     * SelectMany (*)  | Bind
     * 
     * Linq            | List           |
     * ----------------+----------------+----------------------------------------------
     * Aggregate       | FoldLeft       | Fold ou Recuce (sans valeur initiale)
     *                 | FoldRight      | FoldBack ou RecuceBack (sans valeur initiale)
     *                 | Cons           | Prepend
     *                 | Scons          | Append
     * 
     * 
     * References
     * ----------
     * 
     * + Query Expression Pattern / C# 5.0 Language Specification. Section 7.16.3
     *   http://www.microsoft.com/en-us/download/details.aspx?id=7029
     * + The .NET Standard Query Operators
     *   http://msdn.microsoft.com/en-us/library/bb394939.aspx
     * + Classification of Standard Query Operators by Manner of Execution
     *   http://msdn.microsoft.com/en-us/library/bb882641.aspx
     * + http://en.wikipedia.org/wiki/Language_Integrated_Query
     */

    [CompilerGeneratedAttribute]
    class NamespaceDoc
    {
    }
}
