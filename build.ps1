Add-Type -AssemblyName System.Windows.Forms

$FileBrowser = New-Object System.Windows.Forms.OpenFileDialog -Property @{ InitialDirectory = $PSScriptRoot }
$FileBrowser.Filter = "Visual C# Project File|*.csproj"
$FileBrowser.Title = "Select Injector Project"

$injectorCsproj = $null
$updaterCsproj = $null

if([System.IO.File]::Exists($PSScriptRoot + "\TunnelDweller.Injector\TunnelDweller.Injector.csproj") -and [System.IO.File]::Exists($PSScriptRoot + "\TunnelDweller.Updater\TunnelDweller.Updater.csproj"))
{
    Write-Host "Automatically detected required csproj files!"

    $buildDir = $PSScriptRoot + "\psbuild64\"

    $injectorCsproj = $PSScriptRoot + "\TunnelDweller.Injector\TunnelDweller.Injector.csproj";
    $updaterCsproj = $PSScriptRoot + "\TunnelDweller.Updater\TunnelDweller.Updater.csproj"
    
    dotnet publish $injectorCsproj -o $buildDir -r win-x64 -p:PublishSingleFile=$true --sc $false
    dotnet publish $updaterCsproj -o $buildDir -r win-x64 -p:PublishSingleFile=$true --sc $false
}
else
{

$res = $FileBrowser.ShowDialog()
if($res -eq [System.Windows.Forms.DialogResult]::OK)
{
    $injectorCsproj = $FileBrowser.FileName
}
else
{
    Write-Host "No Injector Csproj selected! Injector will be skipped."
}


$FileBrowser.FileName = ""
$FileBrowser.InitialDirectory = $PSScriptRoot
$res = $FileBrowser.ShowDialog()
if($res -eq [System.Windows.Forms.DialogResult]::OK)
{
    $updaterCsproj = $FileBrowser.FileName
}
else
{
    Write-Host "No Updater Csproj selected! Updater will be skipped."
}

$buildDir = $PSScriptRoot + "\psbuild64\"

Write-Host "Publishing Injector and Updater!"

$buildCount = 0;

if($injectorCsproj -isnot $null)
{
    dotnet publish $injectorCsproj -o $buildDir -r win-x64 -p:PublishSingleFile=$true --sc $false
    $buildCount = $buildCount + 1;
}
else
{
    Write-Host "Injector got skipped."
}

if($updaterCsproj -isnot $null)
{
    dotnet publish $updaterCsproj -o $buildDir -r win-x64 -p:PublishSingleFile=$true --sc $false
    $buildCount = $buildCount + 1;
}
else
{
    Write-Host "Updater got skipped."
}
}