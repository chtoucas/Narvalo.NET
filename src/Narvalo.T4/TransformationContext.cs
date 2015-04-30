// <copyright file="TransformationContext.cs" company="T4 Toolbox Team">
//  Copyright © T4 Toolbox Team. All Rights Reserved.
// </copyright>

namespace T4Toolbox
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    using EnvDTE;
    using Microsoft.VisualStudio.TextTemplating;

    /// <summary>
    /// Provides context information about template transformation environment.
    /// </summary>
    /// <remarks>
    /// This class is static by design. It is declared as abstract in order to allow 
    /// <see cref="TransformationContextProcessor"/> to generate a descendant class 
    /// with <see cref="Transformation"/> property strongly-typed as GeneratedTextTransformation.
    /// </remarks>
    public abstract class TransformationContext
    {
        /// <summary>
        /// Stores the top-level Visual Studio automation object.
        /// </summary>
        private static DTE dte;

        /// <summary>
        /// Stores names of output files and their content until the end of the transformation,
        /// when we can be certain that all generated output has been collected and a meaningful
        /// check can be performed to make sure that files that haven't changed are not checked
        /// out unnecessarily.
        /// </summary>
        //private static OutputManager outputManager;

        /// <summary>
        /// Visual Studio <see cref="Project"/> to which the template file belongs.
        /// </summary>
        private static Project project;

        /// <summary>
        /// Visual Studio <see cref="ProjectItem"/> representing the template file.
        /// </summary>
        private static ProjectItem projectItem;

        /// <summary>
        /// Currently running, generated <see cref="TextTransformation"/> object.
        /// </summary>
        private static TextTransformation transformation;

        /// <summary>
        /// Gets or sets the top-level Visual Studio automation object.
        /// </summary>
        /// <value>
        /// A <see cref="DTE"/> object.
        /// </value>
        /// <exception cref="TransformationException">
        /// When Visual Studio automation object is not available.
        /// </exception>
        /// <remarks>
        /// <see cref="TransformationContext"/> assumes that it is running inside of
        /// Visual Studio T4 host and will automaticaly find the main automation object.
        /// However, when running inside of the comman line T4 host (TextTransform.exe),
        /// Visual Studio is not available. Code generators that require Visual Studio
        /// automation in the command line host can launch it explicitly and assign this
        /// property to enable normal behavior of the <see cref="TransformationContext"/>.
        /// </remarks>
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DTE", Justification = "Property name matches the type name")]
        public static DTE DTE
        {
            get
            {
                if (TransformationContext.dte == null)
                {
                    IServiceProvider hostServiceProvider = (IServiceProvider)TransformationContext.Host;
                    if (hostServiceProvider == null)
                    {
                        throw new TransformationException("Host property returned unexpected value (null)");
                    }

                    TransformationContext.dte = (DTE)hostServiceProvider.GetService(typeof(DTE));
                    if (TransformationContext.dte == null)
                    {
                        throw new TransformationException("Unable to retrieve DTE");
                    }
                }

                return TransformationContext.dte;
            }

            set
            {
                TransformationContext.dte = value;
            }
        }

        /// <summary>
        /// Gets <see cref="ITextTemplatingEngineHost"/> which is running the 
        /// <see cref="Transformation"/>.
        /// </summary>
        /// <value>
        /// An <see cref="ITextTemplatingEngineHost"/> instance.
        /// </value>
        /// <exception cref="TransformationException">
        /// When <see cref="TransformationContext"/> has not been properly initialized;
        /// or when currently running <see cref="TextTransformation"/> is not host-specific.
        /// </exception>
        public static ITextTemplatingEngineHost Host
        {
            get
            {
                if (HostProperty == null)
                {
                    throw new TransformationException(
                        "Unable to access templating engine host. " +
                        "Please make sure your template includes hostspecific=\"True\" " +
                        "parameter in the <#@ template #> directive.");
                }

                return (ITextTemplatingEngineHost)HostProperty.GetValue(Transformation, null);
            }
        }

        /// <summary>
        /// Gets the Visual Studio <see cref="Project"/> to which the template file belongs.
        /// </summary>
        /// <value>
        /// A <see cref="Project"/> object.
        /// </value>
        [CLSCompliant(false)]
        public static Project Project
        {
            get
            {
                if (TransformationContext.project == null)
                {
                    TransformationContext.project = TransformationContext.ProjectItem.ContainingProject;
                }

                return TransformationContext.project;
            }
        }

        /// <summary>
        /// Gets the Visual Studio <see cref="ProjectItem"/> representing the template file.
        /// </summary>
        /// <value>
        /// A <see cref="ProjectItem"/> object.
        /// </value>
        [CLSCompliant(false)]
        public static ProjectItem ProjectItem
        {
            get
            {
                if (projectItem == null)
                {
                    projectItem = FindProjectItem(Host.TemplateFile);
                }

                return projectItem;
            }
        }

        /// <summary>
        /// Gets the default namespace specified in the options of the current project.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> with default namespace of the project the template belongs to.
        /// </value>
        public static string RootNamespace
        {
            get
            {
                foreach (Property property in TransformationContext.Project.Properties)
                {
                    if (property.Name == "RootNamespace")
                    {
                        return (string)property.Value;
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the currently running, generated <see cref="TextTransformation"/> object.
        /// </summary>
        /// <value>
        /// A <see cref="TextTransformation"/> object.
        /// </value>
        /// <exception cref="TransformationException">
        /// When <see cref="TransformationContext"/> has not been properly initialized.
        /// </exception>
        public static TextTransformation Transformation
        {
            get
            {
                if (transformation == null)
                {
                    throw new TransformationException(
                        "Transformation context was not properly initialized. " +
                        "Please make sure your template uses the following directive: " +
                        "<#@ include file=\"T4Toolbox.tt\" #>.");
                }

                return transformation;
            }
        }

        /// <summary>
        /// Gets <see cref="PropertyInfo"/> object that represents
        /// Host property of the GeneratedTextTransformation.
        /// </summary>
        private static PropertyInfo HostProperty
        {
            get
            {
                Type transformationType = Transformation.GetType();
                return transformationType.GetProperty("Host");
            }
        }

        /// <summary>
        /// Returns <see cref="ProjectItem"/> for the specified file.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <returns>
        /// Visual Studio <see cref="ProjectItem"/> object.
        /// </returns>
        /// <remarks>
        /// This method is used by templates to access CodeModel for generating
        /// output using C# or Visual Basic source code as a model.
        /// </remarks>
        [CLSCompliant(false)]
        public static ProjectItem FindProjectItem(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            return DTE.Solution.FindProjectItem(fileName);
        }
    }
}
