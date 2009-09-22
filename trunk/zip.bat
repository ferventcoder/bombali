@echo off

::Project UppercuT - http://uppercut.googlecode.com
::No edits to this file are required - http://uppercut.pbwiki.com

SET DIR=%~d0%~p0%

call "%DIR%build.bat"

if %ERRORLEVEL% NEQ 0 goto errors

SET build.config.settings="%DIR%settings\UppercuT.config"
"%DIR%lib\Nant\nant.exe" %1 /f:.\build\zip.build -D:build.config.settings=%build.config.settings%

if %ERRORLEVEL% NEQ 0 goto errors

goto finish

:errors
EXIT /B %ERRORLEVEL%

:finish