$versionFile = "..\product_version.props"

[xml]$versionFileXml = Get-Content $versionFile
$version = New-Object System.Version($versionFileXml.Project.PropertyGroup.VersionPrefix)
$suffix = $versionFileXml.Project.PropertyGroup.VersionSuffix
if ($suffix) {
	$version = "$version-$suffix" }

write-host "tagging commit '$version'.."

git tag v$version