
REM @echo OFF

if exist "%ProgramFiles(x86)%\MSBuild\14.0\bin" set MSBUILDDIR=%ProgramFiles(x86)%\MSBuild\14.0\bin

echo %MSBUILDDIR%

pushd .
del sdk\SparkSDKTests\app.config
copy c:\app.config sdk\SparkSDKTests\app.config
REM .\bin\nuget.exe update -self
bin\nuget.exe restore sdk\solutions\WinSDK4Desktop\WinSDK4Desktop.sln
"%MSBUILDDIR%\msbuild.exe" sdk\solutions\WinSDK4Desktop\WinSDK4Desktop.sln /t:Rebuild /p:Configuration="Debug" /p:Platform="x86"
"%MSBUILDDIR%\msbuild.exe" sdk\solutions\WinSDK4Desktop\WinSDK4Desktop.sln /t:Rebuild /p:Configuration="Release" /p:Platform="x86"
popd

call bin\mstest.bat