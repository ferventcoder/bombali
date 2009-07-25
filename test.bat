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

if %ERRORLEVEL% NEQ 0 goto errors

"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\analyzers\_test.build %1 -D:build.config.settings=%build.config.settings%

if %ERRORLEVEL% NEQ 0 goto errors

"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\analyzers\_test.build open_results -D:build.config.settings=%build.config.settings%

if %ERRORLEVEL% NEQ 0 goto errors

goto finish

:usage
echo.
echo Usage: test.bat
echo Usage: test.bat all - to run all tests
echo.
goto finish

:errors
EXIT /B %ERRORLEVEL%

:finish