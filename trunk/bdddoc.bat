@echo off

::Project UppercuT - http://uppercut.googlecode.com

if '%2' NEQ '' goto usage
if '%3' NEQ '' goto usage
if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage

SET APP_BDDDOC="..\lib\references\bdddoc\bdddoc.console.exe"
SET TEST_ASSEMBLY_NAME="bombali.tests.dll"

SET DIR=%~d0%~p0%

SET build.config.settings="%DIR%Settings\UppercuT.config"
"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\_compile.build -D:build.config.settings=%build.config.settings%
"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\analyzers\_test.build %1 -D:build.config.settings=%build.config.settings%
"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts.Custom\_bdddoc.build  -D:build.config.settings=%build.config.settings% -D:app.bdddoc=%APP_BDDDOC% -D:test_assembly=%TEST_ASSEMBLY_NAME%
"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts.Custom\_bdddoc.build open_results