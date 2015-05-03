﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.T4
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TextTemplating;

    /// <summary>
    /// Provides a base class for Monad templates.
    /// </summary>
    public abstract class _MonadTemplate : VSTemplate
    {
        /// <summary>
        /// A value indicating whether the monad is null-able.
        /// </summary>
        private bool _isNullable = true;

        /// <summary>
        /// A value indicating whether we prefer to use the LINQ dialect.
        /// </summary>
        private bool _preferLinqDialect = true;

        /// <summary>
        /// The generic constraints on the underlying type T.
        /// </summary>
        private string _typeConstraints = String.Empty;

        /// <summary>
        /// The name of the Plus method.
        /// </summary>
        private string _plusName = "Plus";

        /// <summary>
        /// The name of the Return method.
        /// </summary>
        private string _returnName = "Return";

        /// <summary>
        /// The name of the Zero property.
        /// </summary>
        private string _zeroName = "Zero";

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
        /// <value><see langword="true"/> if the monad is a MonadPlus; otherwise <see langword="false"/>.</value>
        protected bool HasPlus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the monad is a MonadZero. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if the monad is a MonadZero; otherwise <see langword="false"/>.</value>
        protected bool HasZero { get; set; }

        /// <summary>
        /// Gets a value indicating whether the monad has a Filter method. Default to true.
        /// </summary>
        /// <value><see langword="true"/> if the monad has a Filter method; otherwise <see langword="false"/>.</value>
        protected bool HasFilter { get { return HasZero; } }

        /// <summary>
        /// Gets a value indicating whether the monad has Join and GroupJoin methods. Default to true.
        /// </summary>
        /// <value><see langword="true"/> if the monad has Join and GroupJoin methods; otherwise <see langword="false"/>.</value>
        protected bool HasJoin { get { return HasZero; } }

        /// <summary>
        /// Gets a value indicating whether the monad has a Then method. Default to true.
        /// </summary>
        /// <value><see langword="true"/> if the monad has a Then method; otherwise <see langword="false"/>.</value>
        protected bool HasThen { get { return HasZero; } }

        /// <summary>
        /// Gets a value indicating whether the monad has a Sum method. Default to true.
        /// </summary>
        /// <value><see langword="true"/> if the monad has a Sum method; otherwise <see langword="false"/>.</value>
        protected bool HasSum { get { return HasZero && HasPlus; } }

        /// <summary>
        /// Gets or sets a value indicating whether we prefer to use the LINQ dialect. Default to true.
        /// </summary>
        /// <remarks>Among other things, this property changes the names of the Map and Filter methods.</remarks>
        /// <value><see langword="true"/> if we prefer to use the LINQ dialect; otherwise <see langword="false"/>.</value>
        protected bool PreferLinqDialect
        {
            get { return _preferLinqDialect; }
            set { _preferLinqDialect = value; }
        }

        #endregion

        #region Constraints.

        /// <summary>
        /// Gets or sets a value indicating whether the monad is null-able. Default to true.
        /// </summary>
        /// <value><see langword="true"/> if the monad is null-able; otherwise <see langword="false"/>.</value>
        protected bool IsNullable { get { return _isNullable; } set { _isNullable = value; } }

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
        /// <value><see langword="true"/> if the underlying type T has a generic type constraint;
        /// otherwise <see langword="false"/>.</value>
        protected bool HasTypeConstraints { get; private set; }

        #endregion

        #region Method and Property Names.

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
                    throw new ArgumentException("The name of the Zero property can not be null or blank.", "value");
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
                    throw new ArgumentException("The name of the Plus method can not be null or blank.", "value");
                }

                _plusName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Return method. Default to "Return".
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
                    throw new ArgumentException("The name of the Return method can not be null or blank.", "value");
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
        protected string MapName
        {
            get { return PreferLinqDialect ? "Select" : "Map"; }
        }

        #endregion

        #region Primary Postconditions.

        /// <summary>
        /// Gets or sets a value indicating whether Monad.Bind() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Bind() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool BindEnsuresSome { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Monad.η() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.η() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool ReturnEnsuresSome { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Monad.μ() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.μ() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool MultiplicationEnsuresSome { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Monad.δ() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.δ() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool DuplicateEnsuresSome { get; set; }

        #endregion

        #region Postconditions.

        /// <summary>
        /// Gets a value indicating whether Monad.Bind() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Bind() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostBindEnsuresSome { get { return IsNullable && BindEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Return() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Return() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostReturnEnsuresSome { get { return IsNullable && ReturnEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Multiplication() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Multiplication() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostMultiplicationEnsuresSome { get { return IsNullable && MultiplicationEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Duplicate() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Duplicate() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostDuplicateEnsuresSome { get { return IsNullable && DuplicateEnsuresSome; } }

        /// <summary>
        /// Gets or sets a value indicating whether Monad.Unit ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Unit ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostUnitEnsuresSome { get { return PostReturnEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Map() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Map() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostMapEnsuresSome { get { return PostBindEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Filter() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Filter() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostFilterEnsuresSome { get { return PostBindEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Coalesce() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Coalesce() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostCoalesceEnsuresSome { get { return PostBindEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Then() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Then() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostThenEnsuresSome { get { return PostCoalesceEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.Join() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Join() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostJoinEnsuresSome { get { return PostMapEnsuresSome; } }

        /// <summary>
        /// Gets a value indicating whether Monad.GroupJoin() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.GroupJoin() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostGroupJoinEnsuresSome { get { return PostMapEnsuresSome; } }

        #endregion

        /// <inheritdoc cref="TextTransformation.TransformText" />
        public override string TransformText()
        {
            WriteHeader();

            WriteContent();

            return GenerationEnvironment.ToString();
        }

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

        protected void InitializeTypeConstraints(params string[] constraints)
        {
            TypeConstraints = String.Join(", ", constraints.Where(_ => !String.IsNullOrWhiteSpace(_)));
        }

        #endregion

        #region Writers into the generated output.

        protected virtual void WriteContent() { }

        protected void WriteNotNull(string name)
        {
            if (IsNullable)
            {
                WriteLine(@"Require.NotNull({0}, ""{0}"");", name);
            }
            else
            {
                // Ensures that the next line in the output is correctly indented.
                WriteLine("/* T4: C# indent */");
            }
        }

        protected void WriteNotNull(bool force = false)
        {
            if (force || IsNullable)
            {
                WriteLine(@"Require.Object(@this);");
            }
            else
            {
                // Ensures that the next line in the output is correctly indented.
                WriteLine("/* T4: C# indent */");
            }
        }

        protected void WriteTypeConstraints(params string[] typeNames)
        {
            if (!HasTypeConstraints)
            {
                // Ensures that the next line in the output is correctly indented.
                WriteLine("/* T4: C# indent */");

                return;
            }

            foreach (var typeName in typeNames)
            {
                PushIndent("            ");
                WriteLine("where {0} : {1}", typeName, TypeConstraints);
                PopIndent();
            }
        }

        #endregion
    }
}
