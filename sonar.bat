SonarQube.Scanner.MSBuild.exe begin /k:"b1pp" /n:"B1++" /v:"1.0"
MSBuild.exe /t:Rebuild
SonarQube.Scanner.MSBuild.exe end

MSBuild.SonarQube.Runner.exe begin /k:"b1pp" /n:"B1++" /v:"1.0" /d:sonar.cs.nunit.reportsPaths="%CD%\NUnitResults.xml"
MSBuild.exe /t:Rebuild
packages\NUnit.ConsoleRunner.3.6.1\tools\nunit3-console.exe --result="%CD%\NUnitResults.xml" "%CD%\Tests.Unit\bin\Debug\Tests.Unit.dll"
MSBuild.SonarQube.Runner.exe end

MSBuild.SonarQube.Runner.exe begin /k:"b1pp" /n:"B1++" /v:"1.0" /d:sonar.cs.opencover.reportsPaths="%CD%\opencover.xml"
MSBuild.exe /t:Rebuild
packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -output:"%CD%\opencover.xml" -register:user -target:"%CD%\packages\NUnit.ConsoleRunner.3.6.1\tools\nunit3-console.exe" -targetargs:"%CD%\Tests.Unit\bin\Debug\Tests.Unit.dll --noresult"
MSBuild.SonarQube.Runner.exe end