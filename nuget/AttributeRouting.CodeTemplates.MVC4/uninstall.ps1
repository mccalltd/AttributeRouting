param($installPath, $toolsPath, $package, $project)

# Remove the templates
$project.ProjectItems.Item("CodeTemplates").ProjectItems.Item("AddController").Delete();
