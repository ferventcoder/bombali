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
"%DIR%lib\tools\Nant\nant.exe" %1 /f:.\BuildScripts.Custom\__master.build -D:build.config.settings=%build.config.settings%

if %ERRORLEVEL% NEQ 0 goto errors

goto finish

:usage 
echo.
echo Usage: build.bat
echo Ensure you've set project name (%PROJECT_NAME%), your repository path (%REPO_PATH%), and company name (%COMPANY_NAME%).
echo.
goto finish

:errors
::ECHO %ERRORLEVEL%
EXIT /B %ERRORLEVEL%

:finish