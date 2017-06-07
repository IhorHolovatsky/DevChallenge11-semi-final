@echo off

echo Installing IIS 7.5, it will take a while...
CMD /C START /w PKGMGR.EXE /l:log.etw /iu:IIS-WebServerRole
echo Done!


echo Disabling firewall
netsh advfirewall set allprofiles state off
echo Done!