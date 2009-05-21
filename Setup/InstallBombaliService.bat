@echo off
echo Installing Bombali Monitoring Service....
%windir%\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe ..\bombali.host.exe

if %ERRORLEVEL% NEQ 0 goto errors

goto complete 

:complete
echo Install Complete.

echo Starting Bombali Monitoring Service.......
net start "Bombali Monitoring Service"

goto finish

:errors
goto finish

:finish
pause