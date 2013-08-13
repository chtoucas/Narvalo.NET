﻿namespace Narvalo.Build
{
    using System.IO;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <example>
    /// <UsingTask TaskName="Narvalo.Build.MergeFiles"
    ///   AssemblyFile="$(ProjectDir)..\Narvalo.Build\bin\$(ConfigurationName)\Narvalo.Build.dll"/>
    ///
    /// <ItemGroup>
    ///   <StyleFiles Include="$(SourceWebPhysicalPath)\Styles\*.css" />
    /// </ItemGroup>
    ///
    /// <Target Name="AfterBuild">
    ///   <MergeFiles Files="@(StyleFiles)" OutFile="$(SourceWebPhysicalPath)\Styles\site.css" />
    /// </Target>
    /// </example>
    /// <remarks>
    /// Il est possible de réaliser la même opération avec les outils de base de MSBuild.
    /// <see cref="http://blogs.msdn.com/b/msbuild/archive/2005/09/29/475046.aspx"/>
    /// Dan provided an elegant answer. The first step is to create an ItemGroup
    /// with the files you want to concatenate:
    /// <code>
    /// <ItemGroup>
    ///   <InFiles Include="1.in;2.in"/>
    /// </ItemGroup>
    /// </code>
    /// Then create a target to do the concatenation:
    /// <code>
    /// <Target Name="ConcatenateFiles">
    ///   <ReadLinesFromFile File="%(InFiles.Identity)">
    ///     <Output TaskParameter="Lines" ItemName="lines"/>
    ///   </ReadLinesFromFile>
    ///   <WriteLinesToFile File="test.out" Lines="@(Lines)" Overwrite="false" />
    /// </Target>
    /// </code>
    /// The first task, ReadLinesFromFiles, goes through each file in the InFiles list
    /// and adds their lines to a new list called Lines. To ensure that each file is done
    /// in a loop (batched), the .Identity item metadata is tacked on. The lines that
    /// are read in are added to a new property called Lines, which is created by using
    /// the Output element. Then WriteLinesToFile takes the generated list and spits
    /// them out to a file.
    /// </remarks>
    public class MergeFiles : Task
    {
        [Required]
        public ITaskItem[] Files { get; set; }

        [Required]
        public string OutFile { get; set; }

        public override bool Execute()
        {
            if (Files.Length == 0) {
                Log.LogMessage(MessageImportance.High,
                    "You supplied an empty list of files to be merged.");
                return true;
            }

            try {
                using (StreamWriter writer = new StreamWriter(OutFile)) {
                    foreach (ITaskItem file in Files) {
                        if (!File.Exists(file.ItemSpec)) {
                            Log.LogMessage(MessageImportance.High,
                                "The file " + file.ItemSpec + " does not exist.");
                            continue;
                        }

                        using (StreamReader reader = new StreamReader(file.ItemSpec)) {
                            writer.Write(reader.ReadToEnd());
                        }
                    }
                }
            }
            catch (IOException ex) {
                Log.LogErrorFromException(ex);

                throw;
            }

            return !Log.HasLoggedErrors;
        }
    }
}
