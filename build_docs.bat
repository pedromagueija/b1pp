rem docfx metadata /Documentation/docfx.json
rem docfx build /Documentation/docfx.json

rem xcopy /Documentation/_site docs/

dotnet ".\tools\Wyam-v2.2.4\Wyam.dll" build -p -w