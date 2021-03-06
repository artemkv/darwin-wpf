param ([string]$scriptFolder, [string]$targetFolder)

If (-not $scriptFolder.EndsWith("\\")) {
    $scriptFolder = $scriptFolder + "\\"
}
If (-not $targetFolder.EndsWith("\\")) {
    $targetFolder = $targetFolder + "\\"
}

[string]$CreateScriptFileName = $scriptFolder + 'createdbcomplete.sql'

[string]$path
$path = $scriptFolder + '01 CreateScript\\*.sql'
Get-ChildItem $path | ForEach-Object { Get-Content $_.FullName | Add-Content $CreateScriptFileName }
$path = $scriptFolder + '03 UDF\\*.sql'
Get-ChildItem $path | ForEach-Object { Get-Content $_.FullName | Add-Content $CreateScriptFileName }
$path = $scriptFolder + '04 SP\\*.sql'
Get-ChildItem $path | ForEach-Object { Get-Content $_.FullName | Add-Content $CreateScriptFileName }
$path = $scriptFolder + '80 DemoData\\*.sql'
Get-ChildItem $path | ForEach-Object { Get-Content $_.FullName | Add-Content $CreateScriptFileName }

Copy-Item -Path $CreateScriptFileName -Destination $targetFolder