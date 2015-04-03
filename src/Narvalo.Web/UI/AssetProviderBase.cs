// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Diagnostics.Contracts;

    using Narvalo.Web.Properties;

    /// <summary>
    /// Provides a base implementation for the asset provider model.
    /// </summary>
    public abstract partial class AssetProviderBase : ProviderBase
    {
        private string _defaultDescription;
        private string _defaultName;

        protected AssetProviderBase() { }

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
                    ? Strings_Web.AssetProviderBase_Description
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

                return String.IsNullOrWhiteSpace(_defaultName) ? "AssetProvider" : _defaultName;
            }

            set
            {
                _defaultName = value;
            }
        }

        public abstract Uri GetFont(string relativePath);

        public abstract Uri GetImage(string relativePath);

        public abstract Uri GetScript(string relativePath);

        public abstract Uri GetStyle(string relativePath);

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing 
        /// the provider-specific attributes specified in the configuration for this provider.</param>
        public override void Initialize(string name, NameValueCollection config)
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
            else if (String.IsNullOrEmpty(config["description"]))
            {
                config.Set("description", DefaultDescription);
            }

            base.Initialize(name, config);

            // This is ensured by base.Initialize().
            Contract.Assume(Name != null);

            Contract.Assert(config != null);

            InitializeCustom(config);

            // Sanity checks.
            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!String.IsNullOrEmpty(key))
                {
                    throw new ProviderException(
                        Format.Resource(Strings_Web.AssetProviderBase_UnknownConfigurationKey_Format, key));
                }
            }
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

    [ContractClass(typeof(AssetProviderBaseContract))]
    public abstract partial class AssetProviderBase : ProviderBase { }

    [ContractClassFor(typeof(AssetProviderBase))]
    internal abstract class AssetProviderBaseContract : AssetProviderBase
    {
        public override Uri GetFont(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetImage(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetScript(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetStyle(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }
    }
}

#endif
