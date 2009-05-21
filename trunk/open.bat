@echo off

::Project UppercuT - http://uppercut.googlecode.com
::No edits to this file are required - http://uppercut.pbwiki.com

SET DIR=%~d0%~p0%

SET build.config.settings="%DIR%Settings\UppercuT.config"
"%DIR%lib\tools\Nant\nant.exe" %1 /f:.\BuildScripts\_open.build -D:build.config.settings=%build.config.settings%

:: DEPRECATED
:: call "%DIR%Settings\build.settings.bat"

::Visual Studio 2005 (will open correctly in VS2008 if the solutions format is VS2008)
::start /b "%PROGRAMFILES%\Microsoft Visual Studio 8\Common7\IDE\devenv.exe" %PROJECT_NAME%.sln /Edit
::Visual Studio 2008 - you only need to choose this if you don't have VS2005 installed (although I don't and the script above still works fine)
::start /b "%PROGRAMFILES%\Microsoft Visual Studio 9\Common7\IDE\devenv.exe" %PROJECT_NAME%.sln /Edit