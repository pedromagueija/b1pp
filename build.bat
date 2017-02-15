powershell -Command "& { Import-Module .\tools\psake\psake.psm1; Invoke-psake .\build.ps1 -framework '4.5.1' }"
pause