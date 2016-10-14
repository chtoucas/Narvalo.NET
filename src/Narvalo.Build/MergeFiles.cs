// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build
{
    using System.IO;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Narvalo.Build.Internal;
    using Narvalo.Build.Properties;

    /// <summary>
    /// MSBuild task to merge files.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <ItemGroup>
    ///   <StyleFiles Include="$(SourceWebPhysicalPath)\Styles\*.css" />
    /// </ItemGroup>
    /// <Target Name="AfterBuild">
    ///   <MergeFiles Files="@(StyleFiles)" OutFile="$(SourceWebPhysicalPath)\Styles\site.css" />
    /// </Target>
    /// ]]>
    /// </code>
    /// </example>
    /**
     * <content markup="commonmark">
     * <![CDATA[
     * It is possible to do this in pure MSBuild: [MSDN](http://blogs.msdn.com/b/msbuild/archive/2005/09/29/475046.aspx).
     *
     * The first step is to create an ItemGroup
     * with the files you want to concatenate:
     * ```xml
     * <ItemGroup>
     *   <InFiles Include="1.in;2.in"/>
     * </ItemGroup>
     * ```
     * Then create a target to do the concatenation:
     * ```xml
     * <Target Name="ConcatenateFiles">
     *   <ReadLinesFromFile File="%(InFiles.Identity)">
     *     <Output TaskParameter="Lines" ItemName="lines"/>
     *   </ReadLinesFromFile>
     *   <WriteLinesToFile File="test.out" Lines="@(Lines)" Overwrite="false" />
     * </Target>
     * ```
     * The first task, ReadLinesFromFiles, goes through each file in the InFiles list
     * and adds their lines to a new list called Lines. To ensure that each file is done
     * in a loop (batched), the .Identity item metadata is tacked on. The lines that
     * are read in are added to a new property called Lines, which is created by using
     * the Output element. Then WriteLinesToFile takes the generated list and spits
     * them out to a file.
     * ]]>
     * </content>
     */
    public sealed class MergeFiles : Task
    {
        /// <summary>
        /// Gets or sets the list of files to merge.
        /// </summary>
        /// <value>The list of files to merge.</value>
        [Required]
        public ITaskItem[] Files { get; set; }

        /// <summary>
        /// Gets or sets the path of the merged file.
        /// </summary>
        /// <value>The path of the merged file.</value>
        [Required]
        public string OutFile { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            if (Files.Length == 0)
            {
                Log.LogMessage(MessageImportance.High, Strings.MergeFiles_EmptyListOfFiles);
                return false;
            }

            try
            {
                using (var writer = new StreamWriter(OutFile))
                {
                    foreach (ITaskItem file in Files)
                    {
                        if (!File.Exists(file.ItemSpec))
                        {
                            Log.LogError(Format.Resource(Strings.FileNotFound_Format, file.ItemSpec));
                            break;
                        }

                        using (var reader = new StreamReader(file.ItemSpec))
                        {
                            writer.Write(reader.ReadToEnd());
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Log.LogErrorFromException(ex, showStackTrace: false);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
