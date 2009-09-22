@echo off

SET file.settings="..\..\Settings\${environment}.settings"

SET DIR=%~d0%~p0%

"%DIR%deployment\nant.exe" /f:"%DIR%scripts\db.deploy" -D:file.settings=%file.settings%
::"%DIR%deployment\nant.exe" /f:"%DIR%scripts\db.deploy" -D:dirs.db=%dirs.db% -D:database.server=%database.server% -D:database.name=%database.name%