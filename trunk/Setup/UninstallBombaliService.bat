@echo off

echo Stopping Bombali Monitoring Service.......
net stop "Bombali Monitoring Service"

echo Uninstalling  ...Bombali Monitoring Service
%windir%\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe /uninstall ..\bombali.host.exe

echo Uninstall Complete.
pause