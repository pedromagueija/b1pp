Framework 4.6

Properties {   
    $base_dir = Split-Path $psake.build_script_file	
    
    $nunit = "packages\NUnit.ConsoleRunner.3.6.0\tools\nunit3-console.exe"
    $tests_regx = "Tests.Unit.dll"
    
    $solution = "$base_dir\B1PP.sln"
    $configuration = "Release"
    $referencesDocumentation = "None"
    $platform = "Any CPU"
    
    $out_dir = "$base_dir\out"
}

FormatTaskName (("-"*25) + " [{0}] " + ("-"*25))

Task Default -Depends Build

Task Build -Depends Clean, Compile, Test, Package

Task Clean {   
    Write-Host "Cleaning..." -ForegroundColor Green
    
    Create-Dir $out_dir
    
    Exec { 
        msbuild $solution /m /t:Clean /p:Configuration=$configuration /p:Platform=$platform /v:quiet
    }
}

Task Compile -Depends Clean {	
    Exec {
        Write-Host "Building configuration:" $configuration $platform -ForegroundColor Green  
        msbuild $solution /m /t:Build /p:Configuration=$configuration /p:Platform=$platform /v:quiet /p:OutDir=$out_dir 
    }
}

Task Test -Depends Compile {
    if($nunit -ne "") {
        $assemblies = (Get-ChildItem "$out_dir" -Recurse -Include $tests_regx)
        Run-Tests $assemblies
    }
    else {
        Write-Host "There are no automated tests to run" -ForegroundColor Yellow
    }
}

Task Package -Depends Test {
    Exec {
        nuget pack -sym .\Framework\Framework.csproj -OutputDirectory ".\nupkg" -Prop "Configuration=$configuration;Platform=$platform;OutDir=$out_dir"
    }
}

#-- Helper functions

# Turns a string into a filename (all lowercase, replaces ' ' with '-'
function global:To-FileName($value) {
    return $value.Replace(" ", "-").ToLower();
}


function global:Create-Dir($directory)
{
    if (Test-Path $directory) 
    {	
        rd $directory -rec -force | out-null
    }
    
    mkdir $directory | out-null
}

function global:Run-Tests ($test_assembly)
{
    Write-Host "Running tests..." -ForegroundColor Green
    exec { & $nunit $test_assembly --work=$out_dir --noresult }
}