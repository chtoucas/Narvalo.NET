# On télécharge la dernière version de NuGet.exe

$wc = New-Object System.Net.WebClient
$wc.DownloadFile("http://www.nuget.org/nuget.exe", $(Get-Location).Path + "\tools\nuget.exe")