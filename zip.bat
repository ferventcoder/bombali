@echo off

::Project UppercuT - http://uppercut.googlecode.com
::No edits to this file are required - http://uppercut.pbwiki.com

SET DIR=%~d0%~p0%

call "%DIR%build.bat"

SET build.config.settings="%DIR%Settings\UppercuT.config"
"%DIR%lib\tools\Nant\nant.exe" %1 /f:.\BuildScripts\_zip.build -D:build.config.settings=%build.config.settings%

:: DEPRECATED
::"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\_zip.build -D:project.name=%PROJECT_NAME%
:: Use this option if you explicitly want to set the items in _zip.build

::%DIR%lib\tools\Nant\nant.exe /f:.\BuildScripts\_update_assemblies.build