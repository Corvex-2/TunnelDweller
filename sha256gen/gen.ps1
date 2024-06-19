param (
    [Parameter(Mandatory=$true)][string]$fileName
 )



$sha256 = [System.Security.Cryptography.SHA256]::Create()
$fileBytes = [System.IO.File]::ReadAllBytes($fileName)
$hashBytes = $sha256.ComputeHash($fileBytes)
$base64String = [Convert]::ToBase64String($hashBytes)
Write-Host ($fileName + ": " + $base64String);
