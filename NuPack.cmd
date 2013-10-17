@echo off

.\tools\NuGet.exe pack src\Narvalo.Build\Narvalo.Build.csproj -Build -Prop Configuration=Release -OutputDirectory _build -Exclude AuthorNotes.md
.\tools\NuGet.exe pack src\Narvalo\Narvalo.csproj -Build -Prop Configuration=Release -OutputDirectory _build
@rem -Symbols
.\tools\NuGet.exe pack src\Narvalo.Web\Narvalo.Web.csproj -Build -Prop Configuration=Release -OutputDirectory _build
@rem -Symbols
