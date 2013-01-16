param($installPath, $toolsPath, $package, $project)

$codeTemplatesFolder = $project.ProjectItems.Item("CodeTemplates");

# Remove the templates for other languages
if ($project.Type -eq "C#") {
	$codeTemplatesFolder.ProjectItems.Item("AddController-VisualBasic").Delete();
	$codeTemplatesFolder.ProjectItems.Item("AddController-CSharp").Name = "AddController";
} elseif ($project.Type -eq "VB.NET")  {
	$codeTemplatesFolder.ProjectItems.Item("AddController-CSharp").Delete();
	$codeTemplatesFolder.ProjectItems.Item("AddController-VisualBasic").Name = "AddController";
}

# ASP.NET MVC uses a custom Text Template Host to scaffold views and controllers so clear the built in TextTemplatingFileGenerator from the Custom Tool property.
foreach($ttFile in $codeTemplatesFolder.ProjectItems.Item("AddController").ProjectItems){
	$ttFile.Properties.Item("CustomTool").Value = "";
}

foreach($ttLanguageFolder in $codeTemplatesFolder.ProjectItems.Item("AddView").ProjectItems){
	foreach($ttFile in $ttLanguageFolder.ProjectItems){
		$ttFile.Properties.Item("CustomTool").Value = "";
	}
}

