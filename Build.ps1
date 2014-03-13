param([string] $Configuration = "Debug")

""
"Updating Nuget packages..."
""

.\.nuget\Nuget.exe install ".nuget\packages.config" -SolutionDirectory ".\"

""
"Building with $Configuration configuration"
""

msbuild /p:Configuration=$Configuration