
REM @echo OFF

if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin" set MSBUILDDIR=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin
echo %MSBUILDDIR%

pushd .
del sdk\SparkSDKTests\app.config
copy c:\app.config sdk\SparkSDKTests\app.config
REM .\bin\nuget.exe update -self
bin\nuget.exe restore sdk\solutions\WinSDK4Desktop\WinSDK4Desktop.sln -NonInteractive

echo "copy scf libraries"
xcopy spark-client-framework\scfLibrary\Release\*.dll sdk\solutions\WinSDK4Desktop\packages\Cisco.Spark.WindowsSDK.1.4.0-EFT01\

"%MSBUILDDIR%\msbuild.exe" sdk\solutions\WinSDK4Desktop\WinSDK4Desktop.sln /t:Rebuild /p:Configuration="Debug" /p:Platform="x86"
"%MSBUILDDIR%\msbuild.exe" sdk\solutions\WinSDK4Desktop\WinSDK4Desktop.sln /t:Rebuild /p:Configuration="Release" /p:Platform="x86"
popd

call bin\mstest.bat

call bin\packageNuGet.bat