param($installPath, $toolsPath, $package, $project)

# Remove the App_Start file for other languages
if ($project.Type -eq "C#") {
	$project.ProjectItems.Item("AttributeRouting.vb").Delete();
} elseif ($project.Type -eq "VB.NET")  {
	$project.ProjectItems.Item("AttributeRouting.cs").Delete();
}