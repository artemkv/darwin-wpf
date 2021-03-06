param ([string]$scriptFolder, [string]$targetFolder)

If (-not $scriptFolder.EndsWith("\\")) {
    $scriptFolder = $scriptFolder + "\\"
}
If (-not $targetFolder.EndsWith("\\")) {
    $targetFolder = $targetFolder + "\\"
}

[string]$CreateScriptFileName = $scriptFolder + 'createdbcomplete.sql'
[string]$CreateScriptTargetFileName = $targetFolder + 'createdbcomplete.sql'

If (Test-Path -Path $CreateScriptFileName -PathType Any) {
    Remove-Item $CreateScriptFileName
}

If (Test-Path -Path $CreateScriptTargetFileName -PathType Any) {
    Remove-Item $CreateScriptTargetFileName
}