@echo off

echo Copying to output directory...

if "%1"=="Release" (
    set BuildMode=Release
) else (
    set BuildMode=Debug
)

for %%k in (exe dll pdb config) do (

    robocopy "%TargetDir:~0,-1%" "%SolutionDir%Build\%BuildMode%" *.%%k /XO /NJH /NJS /NDL /NC /NP

)

echo Copy complete!
exit /b 0
