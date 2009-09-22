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

SET build.config.settings="%DIR%settings\UppercuT.config"
"%DIR%lib\Nant\nant.exe" /f:.\build\compile.step -D:build.config.settings=%build.config.settings%

if %ERRORLEVEL% NEQ 0 goto errors

"%DIR%lib\Nant\nant.exe" /f:.\build\analyzers\test.step %1 -D:build.config.settings=%build.config.settings%
"%DIR%lib\Nant\nant.exe" /f:.\build\analyzers\test.step open_results -D:build.config.settings=%build.config.settings%

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