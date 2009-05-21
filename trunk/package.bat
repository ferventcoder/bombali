@echo off

::Project UppercuT - http://uppercut.googlecode.com
::No edits to this file are required - http://uppercut.pbwiki.com

SET DIR=%~d0%~p0%

SET build.config.settings="%DIR%Settings\UppercuT.config"
"%DIR%lib\tools\Nant\nant.exe" %1 /f:.\BuildScripts\_package.build -D:build.config.settings=%build.config.settings%

:: DEPRECATED
:: call "%DIR%Settings\build.settings.bat"

::"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\_package.build -D:dirs.db.project=%DB_PROJECT_FOLDER% -D:dirs.report.project=%REPORT_PROJECT_FOLDER%

:: Use this option if you explicitly want to set the items in the _package.build

::"%DIR%lib\tools\Nant\nant.exe" /f:.\BuildScripts\_package.build