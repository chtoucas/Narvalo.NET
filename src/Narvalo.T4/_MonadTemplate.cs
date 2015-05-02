// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.T4
{
    using System;

    using Microsoft.VisualStudio.TextTemplating;

    /// <summary>
    /// Provides a base class for Monad templates hosted inside Visual Studio.
    /// </summary>
    public abstract class _MonadTemplate : VSTemplate
    {
        private string _advancedNamespace;

        private bool _isNullable = true;
        private bool _preferLinqDialect = true;

        private bool _hasUnderlyingTypeConstraint;
        private string _underlyingTypeConstraint = String.Empty;

        private string _plusName = "Plus";
        private string _returnName = "Return";
        private string _zeroName = "Zero";

        private string _unitFullName = "Narvalo.Fx.Unit";

        /// <summary>
        /// Initializes a new instance of the <see cref="_MonadTemplate"/> class.
        /// </summary>
        protected _MonadTemplate() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="_MonadTemplate"/> class.
        /// </summary>
        /// <param name="parent">The parent text transformation.</param>
        protected _MonadTemplate(TextTransformation parent) : base(parent) { }

        #region Monad characteristics.

        /// <summary>
        /// Gets or sets a value indicating whether the monad is a MonadPlus. Default to false.
        /// </summary>
        protected bool HasPlus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the monad is a MonadZero. Default to false.
        /// </summary>
        protected bool HasZero { get; set; }

        /// <summary>
        /// Gets a value indicating whether the monad has a Filter method. Default to true.
        /// </summary>
        protected bool HasFilter { get { return HasZero; } }

        /// <summary>
        /// Gets a value indicating whether the monad has Join and GroupJoin methods. Default to true.
        /// </summary>
        protected bool HasJoin { get { return HasZero; } }

        /// <summary>
        /// Gets a value indicating whether the monad has a Then method. Default to true.
        /// </summary>
        protected bool HasThen { get { return HasZero; } }

        /// <summary>
        /// Gets a value indicating whether the monad has a Sum method. Default to true.
        /// </summary>
        protected bool HasSum { get { return HasZero && HasPlus; } }

        /// <summary>
        /// Gets or sets a value indicating whether we prefer to use the LINQ dialect. Default to true.
        /// </summary>
        /// <remarks>Among other things, this property changes the names of the Map and Filter methods.</remarks>
        protected bool PreferLinqDialect
        {
            get { return _preferLinqDialect; }
            set { _preferLinqDialect = value; }
        }

        #endregion

        #region Type constraints.

        /// <summary>
        /// Gets or sets a value indicating whether the monad is nullable. Default to true.
        /// </summary>
        protected bool IsNullable { get { return _isNullable; } set { _isNullable = value; } }

        /// <summary>
        /// Gets or sets a generic constraint on the underlying type T. Default to String.Empty.
        /// </summary>
        protected string UnderlyingTypeConstraint
        {
            get
            {
                return _underlyingTypeConstraint;
            }

            set
            {
                _underlyingTypeConstraint = value;
                _hasUnderlyingTypeConstraint = !String.IsNullOrWhiteSpace(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the underlying type T satisfies a generic type constraint. Default to false.
        /// </summary>
        protected bool HasUnderlyingTypeConstraint { get { return _hasUnderlyingTypeConstraint; } }

        #endregion

        #region Names

        /// <summary>
        /// Gets or sets the name of the 'Advanced' namespace.
        /// </summary>
        /// <remarks>If no namespace was specified, it will be deduced from Namespace.</remarks>
        protected string AdvancedNamespace
        {
            get
            {
                if (_advancedNamespace == null)
                {
                    _advancedNamespace = Namespace + ".Advanced";
                }

                return _advancedNamespace;
            }

            set
            {
                if (_advancedNamespace != null)
                {
                    throw new InvalidOperationException("You can set the name of the 'Advanced' namespace only once.");
                }

                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The 'Advanced' namespace can not be null or blank.", "value");
                }

                _advancedNamespace = value;
            }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the Unit property. Default to "Narvalo.Fx.Unit".
        /// </summary>
        protected string UnitFullName
        {
            get
            {
                return _unitFullName;
            }

            set
            {
                if (_unitFullName != null)
                {
                    throw new InvalidOperationException("You can set the name of the Unit full name only once.");
                }

                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The Unit full name can not be null or blank.", "value");
                }

                _unitFullName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Zero property. Default to "Zero".
        /// </summary>
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
                    throw new ArgumentException("The name of the Zero property can not be null or blank.", "value");
                }

                _zeroName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Plus method. Default to "Plus".
        /// </summary>
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
                    throw new ArgumentException("The name of the Plus method can not be null or blank.", "value");
                }

                _plusName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Return method. Default to "Return".
        /// </summary>
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
                    throw new ArgumentException("The name of the Return method can not be null or blank.", "value");
                }

                _returnName = value;
            }
        }

        /// <summary>
        /// Gets the name of the Filter method.
        /// </summary>
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
        protected string MapName
        {
            get { return PreferLinqDialect ? "Select" : "Map"; }
        }

        #endregion

        #region Postconditions.

        /// <summary>
        /// Gets or sets a value indicating whether Monad.Bind() ensures a non-null return value. Default to false.
        /// </summary>
        protected bool BindEnsuresSome { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Monad.η() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>Monad.Return() being just the public alias of Monad.η(), this also applies
        /// to the Monad.Return() method.</remarks>
        protected bool UnitEnsuresSome { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Monad.μ() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>Monad.Flatten() being just the public alias of Monad.μ(), this also applies
        /// to the Monad.Flatten() method.</remarks>
        protected bool MultiplicationEnsuresSome { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Monad.When() ensures a non-null return value. Always true.
        /// </summary>
        /// <remarks>Whatever happen, Monad.When() always return a non-null value.</remarks>
        protected bool WhenEnsuresSome { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Map() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>Monad.Map() uses Monad.Bind().</remarks>
        protected bool MapEnsuresSome { get { return BindEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Filter() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>Monad.Filter() uses Monad.Bind().</remarks>
        protected bool FilterEnsuresSome { get { return BindEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Coalesce() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>Monad.Coalesce() uses Monad.Bind().</remarks>
        protected bool CoalesceEnsuresSome { get { return BindEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Then() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>Monad.Then() uses Monad.Coalesce().</remarks>
        protected bool ThenEnsuresSome { get { return CoalesceEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Join() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>Monad.Join() uses Monad.Map().</remarks>
        protected bool JoinEnsuresSome { get { return MapEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.GroupJoin() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>Monad.GroupJoin() uses Monad.Map().</remarks>
        protected bool GroupJoinEnsuresSome { get { return MapEnsuresSome; } }

        #endregion

        /// <inheritdoc cref="TextTransformation.TransformText" />
        public override string TransformText()
        {
            WriteHeader();

            WriteContent();

            return GenerationEnvironment.ToString();
        }

        protected virtual void WriteContent() { }

        #region Initalizers.

        /// <summary>
        /// Initializes a MonadZero.
        /// </summary>
        protected void InitializeZero()
        {
            // ZeroName use the default value.
            HasZero = true;
        }

        /// <summary>
        /// Initializes a MonadPlus.
        /// </summary>
        protected void InitializePlus()
        {
            // ZeroName use the default value.
            HasZero = true;

            // PlusName use the default value.
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

        #endregion
    }
}
