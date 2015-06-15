del *.nupkg
del Build /S/Q
xcopy NuGet Build\ /E
xcopy bin\Debug Build\lib\Net40\
nuget pack Build/TypeWalker.nuspec