@echo off

.\tools\NuGet.exe pack src\Narvalo.Build\Narvalo.Build.csproj -Build -Prop Configuration=Release -OutputDirectory _build -Exclude AuthorNotes.md -Symbols
.\tools\NuGet.exe pack src\Narvalo\Narvalo.csproj -Build -Prop Configuration=Release -OutputDirectory _build
.\tools\NuGet.exe pack src\Narvalo.Web\Narvalo.Web.csproj -Build -Prop Configuration=Release -OutputDirectory _build
