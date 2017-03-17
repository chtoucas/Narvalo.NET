// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.T4
{
    using System;
#if TYPE_CONSTRAINTS
    using System.Linq;
#endif

    using Microsoft.VisualStudio.TextTemplating;

    /// <summary>
    /// Provides a base class for Monad templates.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>It is your duty to ensure that Zero and Unit are not null.</item>
    /// <item>We expect "η" &amp; "δ" to never return null. Am I right?</item>
    /// <item>If the monad does have a zero, we do expect "Bind" &amp; "μ"
    /// to never return null but the zero if needed.</item>
    /// <item>If the monad does not have a zero, we do not have any expectation
    /// on the return value of "Bind" &amp; "μ".</item>
    /// <item>If you override any extension method, they must respect the same
    /// contracts.</item>
    /// </list>
    /// </remarks>
    public abstract class _MonadTemplate : VSTemplate
    {
        /// <summary>
        /// The generic constraints on the underlying type T.
        /// </summary>
        private string _typeConstraints = String.Empty;

        private string _internalNamespace;
        private string _linqNamespace;

        /// <summary>
        /// The name of the Eta method.
        /// </summary>
        private string _etaName = "η";

        /// <summary>
        /// The name of the Plus method.
        /// </summary>
        private string _plusName = "Plus";

        /// <summary>
        /// The name of the Return method.
        /// </summary>
        private string _returnName = "Of";

        /// <summary>
        /// The name of the Zero property.
        /// </summary>
        private string _zeroName = "Zero";

        private string _extensionsClsSuffix = String.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="_MonadTemplate"/> class.
        /// </summary>
        protected _MonadTemplate() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="_MonadTemplate"/> class.
        /// </summary>
        /// <param name="parent">The parent text transformation.</param>
        protected _MonadTemplate(TextTransformation parent) : base(parent) { }

        // NB: Automatically set to true when there is more than one generic parameter.
        protected bool DisableReturn { get; set; } = false;

        protected bool DisableFlatten { get; set; } = false;

        protected bool IsDelegate { get; set; } = false;

        protected bool EmitLinq { get; set; } = true;

        protected string InternalNamespace
        {
            get
            {
                if (_internalNamespace == null)
                {
                    _internalNamespace = Namespace + ".Internal";
                }

                return _internalNamespace;
            }

            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The Internal namespace can not be null or blank.");
                }

                _internalNamespace = value;
            }
        }

        protected string LinqNamespace
        {
            get
            {
                if (_linqNamespace == null)
                {
                    _linqNamespace = Namespace + ".Linq";
                }

                return _linqNamespace;
            }

            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The LINQ namespace can not be null or blank.");
                }

                _linqNamespace = value;
            }
        }

        #region Monad characteristics

        /// <summary>
        /// Gets or sets a value indicating whether the monad is a MonadPlus. Default to false.
        /// </summary>
        /// <value>true if the monad is a MonadPlus; otherwise false.</value>
        protected bool HasPlus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the monad is a MonadZero. Default to false.
        /// </summary>
        /// <value>true if the monad is a MonadZero; otherwise false.</value>
        protected bool HasZero { get; set; }

        /// <summary>
        /// Gets a value indicating whether the monad has a Filter method. Default to true.
        /// </summary>
        /// <value>true if the monad has a Filter method; otherwise false.</value>
        protected bool HasFilter => HasZero;

        /// <summary>
        /// Gets a value indicating whether the monad has Join and GroupJoin methods. Default to true.
        /// </summary>
        /// <value>true if the monad has Join and GroupJoin methods; otherwise false.</value>
        protected bool HasJoin => HasFilter;

        /// <summary>
        /// Gets a value indicating whether the monad has a Sum method. Default to true.
        /// </summary>
        /// <value>true if the monad has a Sum method; otherwise false.</value>
        protected bool HasSum => HasZero && HasPlus;

        /// <summary>
        /// Gets or sets a value indicating whether we prefer to use the LINQ dialect. Default to true.
        /// </summary>
        /// <remarks>Among other things, this property changes the names of the Map and Filter methods
        /// to the preferred .NET equivalents: Select and Where.</remarks>
        /// <value>true if we prefer to use the LINQ dialect; otherwise false.</value>
        protected bool PreferLinqDialect { get; set; } = true;

        #endregion

        #region Constraints

        /// <summary>
        /// Gets or sets a value indicating whether the monad is null-able. Default to true.
        /// </summary>
        /// <value>true if the monad is null-able; otherwise false.</value>
        protected bool IsNullable { get; set; } = true;

        protected string ClassTypeDecl => IsNullable ? "class" : "struct";

        protected string HelpersTypeDecl { get; private set; } = "static partial class";

        protected string ExtensionsClsName => Name + _extensionsClsSuffix;

#if TYPE_CONSTRAINTS

        /// <summary>
        /// Gets the generic constraints on the underlying type T. Default to String.Empty.
        /// </summary>
        /// <value>The generic constraints on the underlying type T.</value>
        protected string TypeConstraints
        {
            get
            {
                if (!HasTypeConstraints)
                {
                    throw new InvalidOperationException("The underlying type does not enforce any type constraint.");
                }

                return _typeConstraints;
            }

            private set
            {
                _typeConstraints = value;
                HasTypeConstraints = !String.IsNullOrWhiteSpace(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the underlying type T has a generic type constraint. Default to false.
        /// </summary>
        /// <value>true if the underlying type T has a generic type constraint;
        /// otherwise false.</value>
        protected bool HasTypeConstraints { get; private set; }

#endif

        #endregion

        #region Generic parameters

        protected string MainGeneric { get; private set; } = "T";

        protected string[] RightGenerics { get; private set; } = new String[0];

        // String that contains all generic type decls, eg "T, T2, T3"
        protected string GenericsDecl { get; private set; } = "T";

        protected bool HasRightGenerics { get; private set; } = false;

        // String that only contains the "rigth" generic type decls, eg "T2, T3"
        protected string RTDecl { get; private set; } = String.Empty;

        #endregion

        #region Method and Property Names

        /// <summary>
        /// Gets or sets the name of the Eta method. Default to "η".
        /// </summary>
        /// <value>The name of the Eta method.</value>
        protected string EtaName
        {
            get
            {
                return _etaName;
            }

            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The name of the Eta method can not be null or blank.", nameof(value));
                }

                _etaName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Zero property. Default to "Zero".
        /// </summary>
        /// <value>The name of the Zero property.</value>
        protected string ZeroName
        {
            get
            {
                if (!HasZero)
                {
                    throw new InvalidOperationException("The monad is not a MonadZero.");
                }

                return _zeroName;
            }

            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The name of the Zero property can not be null or blank.", nameof(value));
                }

                _zeroName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Plus method. Default to "Plus".
        /// </summary>
        /// <value>The name of the Plus method.</value>
        protected string PlusName
        {
            get
            {
                if (!HasPlus)
                {
                    throw new InvalidOperationException("The monad is not a MonadPlus.");
                }

                return _plusName;
            }

            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The name of the Plus method can not be null or blank.", nameof(value));
                }

                _plusName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Return method. Default to "Of".
        /// </summary>
        /// <value>The name of the Return method.</value>
        protected string ReturnName
        {
            get
            {
                return _returnName;
            }

            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The name of the Return method can not be null or blank.", nameof(value));
                }

                _returnName = value;
            }
        }

        /// <summary>
        /// Gets the name of the Filter method.
        /// </summary>
        /// <value>The name of the Filter method.</value>
        protected string FilterName
        {
            get
            {
                if (!HasZero)
                {
                    throw new InvalidOperationException("The monad does not have a Zero.");
                }

                return PreferLinqDialect ? "Where" : "Filter";
            }
        }

        /// <summary>
        /// Gets the name of the Map method.
        /// </summary>
        /// <value>The name of the Map method.</value>
        protected string MapName => PreferLinqDialect ? "Select" : "Map";

        #endregion

        #region Postconditions

        /// <summary>
        /// Gets a value indicating whether Monad.Bind() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>If the monad does have a zero, we expect "Bind"
        /// to never return null but the zero if needed.</remarks>
        /// <value>true if Monad.Bind() ensures a non-null return value;
        /// otherwise false.</value>
        protected bool PostBindEnsuresSome => IsNullable && HasZero;

        /// <summary>
        /// Gets a value indicating whether Monad.μ() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>If the monad does have a zero, we expect "μ"
        /// to never return null but the zero if needed.</remarks>
        /// <value>true if Monad.μ() ensures a non-null return value;
        /// otherwise false.</value>
        protected bool PostMultiplicationEnsuresSome => IsNullable && HasZero;

        /// <summary>
        /// Gets a value indicating whether Monad.Map() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value>true if Monad.Map() ensures a non-null return value;
        /// otherwise false.</value>
        protected bool PostMapEnsuresSome => PostBindEnsuresSome;

        /// <summary>
        /// Gets a value indicating whether Monad.Filter() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value>true if Monad.Filter() ensures a non-null return value;
        /// otherwise false.</value>
        protected bool PostFilterEnsuresSome => PostBindEnsuresSome;

        /// <summary>
        /// Gets a value indicating whether Monad.Then() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value>true if Monad.Then() ensures a non-null return value;
        /// otherwise false.</value>
        protected bool PostThenEnsuresSome => PostBindEnsuresSome;

        /// <summary>
        /// Gets a value indicating whether Monad.Join() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value>true if Monad.Join() ensures a non-null return value;
        /// otherwise false.</value>
        protected bool PostJoinEnsuresSome => PostMapEnsuresSome;

        /// <summary>
        /// Gets a value indicating whether Monad.GroupJoin() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value>true if Monad.GroupJoin() ensures a non-null return value;
        /// otherwise false.</value>
        protected bool PostGroupJoinEnsuresSome => PostMapEnsuresSome;

        #endregion

        public override string TransformText()
        {
            WriteHeader();

            WriteContent();

            return GenerationEnvironment.ToString();
        }

        #region Initalizers

        // Only for the Narvalo.Fx project.
        protected void InitializeNamespacesForNarvaloFx()
        {
            InternalNamespace = "Narvalo.Internal";
            LinqNamespace = "Narvalo.Linq";
        }

        protected void InitializeHelpers(bool asStruct, string suffix = "Extensions")
        {
            HelpersTypeDecl = asStruct ? "partial struct" : "partial class";
            _extensionsClsSuffix = suffix;
        }

        protected void InitializeGenericParameters(string mainGeneric, params string[] rightGenerics)
        {
            if (rightGenerics == null) { throw new ArgumentNullException(nameof(rightGenerics)); }

            MainGeneric = mainGeneric;

            if (rightGenerics.Length > 0)
            {
                DisableReturn = true;
                RightGenerics = rightGenerics;

                HasRightGenerics = true;
                RTDecl = ", " + String.Join(", ", rightGenerics);
                GenericsDecl = mainGeneric + RTDecl;
            }
            else
            {
                GenericsDecl = mainGeneric;
            }
        }

        protected void InitializeDelegate()
        {
            IsDelegate = true;
            DisableReturn = true;
            DisableFlatten = true;
        }

        /// <summary>
        /// Initializes a MonadZero.
        /// </summary>
        protected void InitializeZero()
        {
            HasZero = true;
        }

        /// <summary>
        /// Initializes a MonadPlus.
        /// </summary>
        protected void InitializePlus()
        {
            HasZero = true;
            HasPlus = true;
        }

        /// <summary>
        /// Initializes a MonadOr.
        /// </summary>
        protected void InitializeOr()
        {
            HasZero = true;
            ZeroName = "None";

            HasPlus = true;
            PlusName = "OrElse";
        }

#if TYPE_CONSTRAINTS

        protected void InitializeTypeConstraints(params string[] constraints)
        {
            TypeConstraints = String.Join(", ", constraints.Where(_ => !String.IsNullOrWhiteSpace(_)));
        }

#endif

        #endregion

        #region Writers into the generated output

        protected virtual void WriteContent() { }

        protected void WriteNotNull(string name)
        {
            if (IsNullable)
            {
                WriteLine(@"Require.NotNull({0}, nameof({0}));", name);
            }
            else
            {
                // Ensures that the next line in the output is correctly indented.
                WriteLine("/* T4: NotNull({0}) */", name);
            }
        }

        protected void WriteNotNull(bool force = false)
        {
            if (force || IsNullable)
            {
                WriteLine(@"Require.NotNull(@this, nameof(@this));");
            }
            else
            {
                // Ensures that the next line in the output is correctly indented.
                WriteLine("/* T4: NotNull(@this) */");
            }
        }

#if TYPE_CONSTRAINTS

        protected void WriteCustomTypeConstraint(string typeName, params string[] typeConstraints)
        {
            var constraints = String.Join(",", typeConstraints);

            PushIndent("            ");
            if (HasTypeConstraints)
            {
                WriteLine("where {0} : {1}, {2}", typeName, TypeConstraints, constraints);
            }
            else
            {
                WriteLine("where {0} : {1}", typeName, constraints);
            }
            PopIndent();
        }

        protected void WriteTypeConstraints(params string[] typeNames)
        {
            if (!HasTypeConstraints)
            {
                // Ensures that the next line in the output is correctly indented.
                WriteLine("/* T4: type constraint */");

                return;
            }

            foreach (var typeName in typeNames)
            {
                PushIndent("            ");
                WriteLine("where {0} : {1}", typeName, TypeConstraints);
                PopIndent();
            }
        }

#endif

        protected void WriteFactory(string name)
        {
            if (HasRightGenerics)
            {
                if (IsDelegate)
                {
                    // To output Monad.Of<TResult, TError>, use:
                    Write(@"{0}.{1}<{2}{3}>", Name, ReturnName, name, RTDecl);
                }
                else
                {
                    // Internally we prefer Result<TResult, TError>.η
                    Write(@"{0}<{2}{3}>.{1}", Name, EtaName, name, RTDecl);
                }
            }
            else
            {
                if (IsDelegate)
                {
                    // To output Monad.Of<TResult, TError>, use:
                    Write(@"{0}.{1}", Name, ReturnName);
                }
                else
                {
                    // Internally we prefer Maybe<TResult>.η
                    Write(@"{0}<{2}>.{1}", Name, EtaName, name);
                }
            }
        }

        #endregion
    }
}
