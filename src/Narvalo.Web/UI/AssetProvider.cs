// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Web.Properties;

    /// <summary>
    /// Provides a base implementation for the asset provider model.
    /// </summary>
    public abstract partial class AssetProvider : ProviderBase
    {
        internal const string DefaultDefaultName = "AssetProvider";

        private string _defaultDescription;
        private string _defaultName;

        protected AssetProvider() { }

        /// <summary>
        /// Gets or sets a default description suitable for display in administrative
        /// tools or other user interfaces (UIs).
        /// </summary>
        /// <value>A default description suitable for display in administrative tools
        /// or other UIs.</value>
        /// <seealso cref="ProviderBase.Description"/>.
        protected string DefaultDescription
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return String.IsNullOrWhiteSpace(_defaultDescription)
                    ? Strings.AssetProvider_Description
                    : _defaultDescription;
            }

            set
            {
                _defaultDescription = value;
            }
        }

        /// <summary>
        /// Gets or sets the default name used to refer to the provider during configuration.
        /// </summary>
        /// <value>The default name used to refer to the provider during configuration.</value>
        /// <seealso cref="ProviderBase.Name"/>.
        protected string DefaultName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return String.IsNullOrWhiteSpace(_defaultName) ? DefaultDefaultName : _defaultName;
            }

            set
            {
                _defaultName = value;
            }
        }

        public abstract Uri GetFontUri(string relativePath);

        public abstract Uri GetImageUri(string relativePath);

        public abstract Uri GetScriptUri(string relativePath);

        public abstract Uri GetStyleUri(string relativePath);

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing 
        /// the provider-specific attributes specified in the configuration for this provider.</param>
        public sealed override void Initialize(string name, NameValueCollection config)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                name = DefaultName;
            }

            if (config == null)
            {
                config = new NameValueCollection(1);
                config.Add("description", DefaultDescription);
            }
            else if (String.IsNullOrWhiteSpace(config["description"]))
            {
                config.Set("description", DefaultDescription);
            }

            base.Initialize(name, config);

            InitializeCustom(config);

            // Sanity checks.
            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!String.IsNullOrEmpty(key))
                {
                    throw new ProviderException(
                        Format.Resource(Strings.AssetProvider_UnknownConfigurationKey_Format, key));
                }
            }

            // REVIEW: Name is initialized by base.Initialize() but Name 
            // could also be overridden by a derived class.
            //Contract.Assume(Name != null);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "config",
            Justification = "[Intentionally] Internal parameter validation in debug builds only.")]
        internal static void InitializeCustomCore([ValidatedNotNull]NameValueCollection config)
        {
            Check.NotNull(config, "The base class 'AssetProvider' guarantees that 'config' is never null.");
        }

        protected virtual void InitializeCustom(NameValueCollection config)
        {
            // Intentionally left blank.
        }
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Web.UI
{
    using System;
    using System.Configuration.Provider;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(AssetProviderContract))]
    public abstract partial class AssetProvider : ProviderBase { }

    [ContractClassFor(typeof(AssetProvider))]
    internal abstract class AssetProviderContract : AssetProvider
    {
        public override Uri GetFontUri(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetImageUri(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetScriptUri(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetStyleUri(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }
    }
}

#endif
