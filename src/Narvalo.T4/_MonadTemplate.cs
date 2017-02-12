// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.T4
{
    using System;
    using System.Linq;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="_MonadTemplate"/> class.
        /// </summary>
        protected _MonadTemplate() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="_MonadTemplate"/> class.
        /// </summary>
        /// <param name="parent">The parent text transformation.</param>
        protected _MonadTemplate(TextTransformation parent) : base(parent) { }

        #region Monad characteristics

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
        protected bool HasFilter => HasZero;

        /// <summary>
        /// Gets a value indicating whether the monad has Join and GroupJoin methods. Default to true.
        /// </summary>
        /// <value><see langword="true"/> if the monad has Join and GroupJoin methods; otherwise <see langword="false"/>.</value>
        protected bool HasJoin => HasFilter;

        /// <summary>
        /// Gets a value indicating whether the monad has a Sum method. Default to true.
        /// </summary>
        /// <value><see langword="true"/> if the monad has a Sum method; otherwise <see langword="false"/>.</value>
        protected bool HasSum => HasZero && HasPlus;

        /// <summary>
        /// Gets or sets a value indicating whether we prefer to use the LINQ dialect. Default to true.
        /// </summary>
        /// <remarks>Among other things, this property changes the names of the Map and Filter methods
        /// to the preferred .NET equivalents: Select and Where.</remarks>
        /// <value><see langword="true"/> if we prefer to use the LINQ dialect; otherwise <see langword="false"/>.</value>
        protected bool PreferLinqDialect { get; set; } = true;

        #endregion

        #region Constraints

        /// <summary>
        /// Gets or sets a value indicating whether the monad is null-able. Default to true.
        /// </summary>
        /// <value><see langword="true"/> if the monad is null-able; otherwise <see langword="false"/>.</value>
        protected bool IsNullable { get; set; } = true;

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

        #region Method and Property Names

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
        /// <value><see langword="true"/> if Monad.Bind() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostBindEnsuresSome => IsNullable && HasZero;

        /// <summary>
        /// Gets a value indicating whether Monad.μ() ensures a non-null return value. Default to false.
        /// </summary>
        /// <remarks>If the monad does have a zero, we expect "μ"
        /// to never return null but the zero if needed.</remarks>
        /// <value><see langword="true"/> if Monad.μ() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostMultiplicationEnsuresSome =>  IsNullable && HasZero;

        /// <summary>
        /// Gets a value indicating whether Monad.Map() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Map() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostMapEnsuresSome => PostBindEnsuresSome;

        /// <summary>
        /// Gets a value indicating whether Monad.Filter() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Filter() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostFilterEnsuresSome => PostBindEnsuresSome;

        /// <summary>
        /// Gets a value indicating whether Monad.Then() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Then() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostThenEnsuresSome => PostBindEnsuresSome;

        /// <summary>
        /// Gets a value indicating whether Monad.Join() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.Join() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostJoinEnsuresSome => PostMapEnsuresSome;

        /// <summary>
        /// Gets a value indicating whether Monad.GroupJoin() ensures a non-null return value. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if Monad.GroupJoin() ensures a non-null return value;
        /// otherwise <see langword="false"/>.</value>
        protected bool PostGroupJoinEnsuresSome => PostMapEnsuresSome;

        #endregion

        /// <inheritdoc cref="TextTransformation.TransformText" />
        public override string TransformText()
        {
            WriteHeader();

            WriteContent();

            return GenerationEnvironment.ToString();
        }

        #region Initalizers

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

        protected void InitializeTypeConstraints(params string[] constraints)
        {
            TypeConstraints = String.Join(", ", constraints.Where(_ => !String.IsNullOrWhiteSpace(_)));
        }

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
                WriteLine("/* T4: C# indent */");
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
