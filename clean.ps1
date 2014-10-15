# On supprime tous les répertoires bin et obj dans les sources.
Get-ChildItem .\src -include bin,obj -Recurse | ForEach ($_) { Remove-Item $_.FullName -Force -Recurse }
Get-ChildItem .\test -include bin,obj -Recurse | ForEach ($_) { Remove-Item $_.FullName -Force -Recurse }
