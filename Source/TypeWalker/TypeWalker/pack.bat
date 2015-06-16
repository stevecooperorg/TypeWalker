del *.nupkg
del Build /S/Q
xcopy NuGet Build\ /E
dir bin\debug
copy bin\Debug\TypeWalker.exe Build\lib\Net40\
copy bin\Debug\TypeWalker.pdb Build\lib\Net40\
REM copy bin\Debug\* Build\lib\Net40\
nuget pack Build/TypeWalker.nuspec
