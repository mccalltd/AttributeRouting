param($installPath, $toolsPath, $package, $project)

# Remove the templates for other languages
if ($project.Type -eq "C#") {	
	$path = [System.IO.Path]
	$projectDirectory = $path::GetDirectoryName($project.FullName)
	$codeTemplatesDirectory = $projectDirectory + "\CodeTemplates"

	# delete vb dir
	Remove-Item -Path ($codeTemplatesDirectory + "\VisualBasic") -Recurse

	# move the csharp stuff into proper position
	Move-Item -Path ($codeTemplatesDirectory + "\CSharp\AddController") -Destination ($codeTemplatesDirectory + "\AddController")
	
	# delete csharp dir
	Remove-Item -Path ($codeTemplatesDirectory + "\CSharp") -Recurse
}

$codeTemplatesFolder = $project.ProjectItems.Item("CodeTemplates");

# ASP.NET MVC uses a custom Text Template Host to scaffold views and controllers so clear the built in TextTemplatingFileGenerator from the Custom Tool property.
foreach($ttFile in $codeTemplatesFolder.ProjectItems.Item("AddController").ProjectItems){
	$ttFile.Properties.Item("CustomTool").Value = "";
}

foreach($ttLanguageFolder in $codeTemplatesFolder.ProjectItems.Item("AddView").ProjectItems){
	foreach($ttFile in $ttLanguageFolder.ProjectItems){
		$ttFile.Properties.Item("CustomTool").Value = "";
	}
}

