# On télécharge la dernière version de NuGet.exe.

Write-Host "Downloading last version of NuGet.exe"
$wc = New-Object System.Net.WebClient
$wc.DownloadFile("http://www.nuget.org/nuget.exe", $(Get-Location).Path + "\.nuget\nuget.exe")