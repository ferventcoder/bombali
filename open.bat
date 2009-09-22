@echo off

::Project UppercuT - http://uppercut.googlecode.com
::No edits to this file are required - http://uppercut.pbwiki.com

SET DIR=%~d0%~p0%

SET build.config.settings="%DIR%settings\UppercuT.config"
"%DIR%lib\Nant\nant.exe" %1 /f:.\build\open.build -D:build.config.settings=%build.config.settings%