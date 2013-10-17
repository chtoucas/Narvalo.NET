@echo off

.nuget\NuGet.exe pack src\Narvalo\Narvalo.csproj -Build -Prop Configuration=Release -OutputDirectory _build -Exclude AuthorNotes.txt -Symbols
.nuget\NuGet.exe pack src\Narvalo.Web\Narvalo.Web.csproj -Build -Prop Configuration=Release -OutputDirectory _build -Symbols
.nuget\NuGet.exe pack src\Narvalo.Build\Narvalo.Build.csproj -Build -Prop Configuration=Release -OutputDirectory _build -Exclude AuthorNotes.txt -Symbols
