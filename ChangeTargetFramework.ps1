$oldVersion = "v4.7.2"
$newVersion = "net472"

$projects = Get-ChildItem *.csproj -Recurse
foreach ($project in $projects) {
    [xml] $xdoc = get-content $project

    if ($xdoc.Project.PropertyGroup[0].TargetFrameworkVersion) {
        Write-Host "$($project.Name) $($xdoc.Project.PropertyGroup[0].TargetFrameworkVersion)=> $oldVersion"
        $xdoc.Project.PropertyGroup[0].TargetFrameworkVersion = $oldVersion
        $xdoc.Save($project)
    }
    else {
        if ($xdoc.Project.PropertyGroup[0].TargetFramework) {
            Write-Host "$($project.Name) $($xdoc.Project.PropertyGroup[0].TargetFramework)=> $newVersion"
            $xdoc.Project.PropertyGroup[0].TargetFramework = $newVersion
            $xdoc.Save($project)
        }
    }
}