
@echo OFF

if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin" set MSBUILDDIR=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin
echo %MSBUILDDIR%

pushd .
del sdk\SparkSDKTests\app.config
copy c:\app.config sdk\SparkSDKTests\app.config
REM .\bin\nuget.exe update -self
bin\nuget.exe restore sdk\solutions\WinSDK4Desktop\WinSDK4Desktop.sln -NonInteractive

echo copy scf libraries

set SDKNuGetPackage=Cisco.Spark.WindowsSDK.1.4.0-EFT01
echo SDKNuGetPackage is %SDKNuGetPackage%
copy /y spark-client-framework\scfLibrary\Release\*.dll sdk\solutions\WinSDK4Desktop\packages\\%SDKNuGetPackage%\\
if not %errorlevel% == 0 ( 
	echo update scf libraries failed.
	goto EXIT 
)

"%MSBUILDDIR%\msbuild.exe" sdk\solutions\WinSDK4Desktop\WinSDK4Desktop.sln /t:Rebuild /p:Configuration="Debug" /p:Platform="x86"
if not %errorlevel% == 0 ( 
	echo build debug version failed!
	goto EXIT 
)

"%MSBUILDDIR%\msbuild.exe" sdk\solutions\WinSDK4Desktop\WinSDK4Desktop.sln /t:Rebuild /p:Configuration="Release" /p:Platform="x86"
if not %errorlevel% == 0 (
	echo build release version failed!
	goto EXIT
)

popd

call bin\mstest.bat

call bin\packageNuGet.bat

:EXIT
echo error level: %errorlevel% 
EXIT /B %errorlevel%