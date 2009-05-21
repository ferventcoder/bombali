@echo off

::Project UppercuT - http://uppercut.googlecode.com
::No edits to this file are required - http://uppercut.pbwiki.com

if '%2' NEQ '' goto usage
if '%3' NEQ '' goto usage
if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage

SET DIR=%~d0%~p0%

SET build.config.settings="%DIR%Settings\UppercuT.config"
"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\_compile.build -D:build.config.settings=%build.config.settings%

if '%TESTING_FRAMEWORK%' == '"mbunit"' goto mbunit
if '%TESTING_FRAMEWORK%' == '"nunit"' goto nunit

:mbunit
"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\Analyzers\_MbUnit.build %1 -D:build.config.settings=%build.config.settings%
::"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\Analyzers\_MbUnit.build open_results -D:build.config.settings=%build.config.settings%

goto bdddoc

:bdddoc

SET APP_BDDDOC="..\lib\references\bdddoc\bdddoc.console.exe"
SET TEST_ASSEMBLY_NAME="bombali.tests.dll"

%DIR%lib\tools\Nant\nant.exe /f:.\BuildScripts.Custom\_bdddoc.build -D:app.bdddoc=%APP_BDDDOC% -D:test_assembly=%TEST_ASSEMBLY_NAME%
%DIR%lib\tools\Nant\nant.exe /f:.\BuildScripts.Custom\_bdddoc.build open_results

goto finish

:nunit
"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\Analyzers\_nunit.build %1 -D:build.config.settings=%build.config.settings%
"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\Analyzers\_nunit.build open_results -D:build.config.settings=%build.config.settings%

goto finish

:usage
echo.
echo Usage: test.bat
echo Usage: test.bat run_all_tests to run all tests
echo.
goto finish

:finish