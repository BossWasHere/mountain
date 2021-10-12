@echo off

echo Cleaning output directories...

call :cleandir %~dp0..\Build\Debug
call :cleandir %~dp0..\Build\Release
call :cleandir %~dp0..\Build\generated

:: Uncomment if you want to delete original server jars on cleanup
::call :cleandir %~dp0..\Build\Cache

echo Cleanup complete!
exit /b 0

:cleandir

echo [Clean %*]

if not exist ("%*") exit /b 0

del /q "%*\*"
FOR /D %%p IN ("%*\*.*") DO (
    rmdir "%%p" /s /q
)

exit /b 0
