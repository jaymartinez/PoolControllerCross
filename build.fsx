#r "packages/FAKE/tools/FakeLib.dll" // 1
open Fake 
open EnvironmentHelper
open ProcessTestRunner
open System
open Fake.IO

let buildDir = "BuildOutput"
let iOSAdHocObjDir = "PoolControllerCross.iOS/obj/iPhone/"
let iOSAdHocBinDir = "PoolControllerCross.iOS/bin/iPhone/"
//let testDir = ""
//let nunitRunnerPath = "./packages/NUnit.ConsoleRunner/tools/nunit3-console.exe"
let windowsDeploy = buildDir + "/UWP"

let appVersion = "2.0.0.0"

let getAndroidSdkPath = 
    let sdkPath = environVar("ANDROID_HOME")
    if sdkPath = "" || sdkPath = null then "C:\\Program Files (x86)\\Android\\android-sdk"
    else sdkPath

// Restores packages at the specified [path (a literal path or GLOB expression)]
let RestorePackages path =
    !! path
        |> Seq.iter (RestorePackage id)

//Set NodeReuse to false so MSBuild.exe will close after each build
MSBuildDefaults <- { MSBuildDefaults with NodeReuse = false} 

Target "Clean" (fun _ ->
    CleanDirs [buildDir; iOSAdHocObjDir + "Ad-HocTesting"; iOSAdHocObjDir + "Ad-Hoc"; iOSAdHocBinDir + "Ad-HocTesting"; iOSAdHocBinDir + "Ad-Hoc"]
)

//Restore solution packages
Target "RestoreSolutionPackages" (fun _ ->
     "./PoolControllerCross.sln"
     |> RestoreMSSolutionPackages (fun p ->
         { p with
             Sources = p.Sources
             OutputPath = "./packages"
             Retries = 4 })
)

//Restore project packages
Target "RestoreProjectPackages" (fun _ -> 
    RestorePackages "PoolControllerCross.Test/packages.config"
    RestorePackages "./PoolController*/packages.config"
 )

Target "Droid" (fun _ ->
    let releaseDir = "PoolController.Droid/bin/Release"
    
    !! "PoolController.Droid/PoolController.Droid.csproj" 
        |> MSBuild "" "SignAndroidPackage" 
        [ 
            ("Configuration", "Release"); 
            ("Platform", "AnyCPU"); 
            ("AndroidSdkDirectory", getAndroidSdkPath) 
        ] 
    |> Log "---Build-Droid build output---"
    
    //Copy APK to build directory
    CopyFile buildDir (releaseDir + "/PoolController.Droid.apk")
)

Target "iOSStore" (fun _ ->
    let copyDir = buildDir + "/iOSStore"
    CreateDir copyDir//Create the iOSStore directory

    !! "PoolControllerCross.sln" 
        |> MSBuild "" "Build" 
        [ 
            ("Configuration", "Ad-Hoc"); 
            ("Platform", "iPhone");  
            ("ServerAddress", "192.168.0.46");
            ("ServerUser", "Jay Martinez");
            ("ServerPassword", "Olive0il"); 
        ] 
    |> Log "---Build-iOS Store build output---"
    
    //Copy IPA to build directory
    CopyFile copyDir ("PoolControllerCross.iOS/bin/iPhone/Ad-Hoc/PoolController.ipa")
)

Target "iOSTesting" (fun _ ->
    let copyDir = buildDir + "/iOSTesting"
    CreateDir copyDir //Create the iOSTesting directory

    !! "PoolControllerCross.sln" 
        |> MSBuild "" "Build" 
        [ 
            ("Configuration", "Ad-HocTesting"); 
            ("Platform", "iPhone");  
            ("ServerAddress", "192.168.0.46");
            ("ServerUser", "Jay Martinez");
            ("ServerPassword", "Olive0il"); 
        ] 
    |> Log "---Build-iOS Store build output---"
    
    //Copy IPA to build directorybuildDir + "/UWP
    CopyFile copyDir ("PoolControllerCross.iOS/bin/iPhone/Ad-HocTesting/PoolController.ipa")
)

Target "UWPx64" (fun _ ->

    !! "PoolControllerCross.sln" 
        |> MSBuild "" "Publish" 
        [ 
            ("Configuration", "Release"); 
            ("Platform", "x64");  
            ("AppxPackageDir", "../../BuildOutput/UWP/pool-controller") //Assign where to place app package
        ] 
    |> Log "---Build-UWPx64 build output---"
)

Target "UWPx86" (fun _ ->

    !! "PoolControllerCross.sln" 
        |> MSBuild "" "Publish" 
        [ 
            ("Configuration", "Release"); 
            ("Platform", "x86");  
            ("AppxPackageDir", "../../BuildOutput/UWP/pool-controller") //Assign where to place app package
        ] 
    |> Log "---Build-UWPx86 build output---"
)

Target "CleanSolution" (fun _ ->
    trace "\n>>> Cleaning Solution <<<"
    !! "./*.sln"
        |> MSBuild "" "Clean"
        [
            ("Configuration", "Release");
            ("Platform", "Any CPU");
        ]
        |> Log " >> Clean Solution: "
)

// Run this target before kicking off the unit tests
Target "BuildTest" (fun _ ->
    trace "\n>>> Building test project <<<"
    !! (testDir + "*.csproj")
        |> MSBuildDebug "" "Build"
        |> Log " >> TestBuild-Output: "
)

//TODO - Integrate into build. 
Target "RunTests" (fun _ ->
    trace "\n>>> Running Unit Tests <<<"
    [nunitRunnerPath, "--testlist=./PoolControllerCross.Test/bin/Debug/PoolControllerCross.Test.dll --work=./PoolControllerCross.Test/TestResults"]
        |> RunConsoleTests (fun p -> {p with TimeOut = TimeSpan.FromMinutes 1.; ErrorLevel = TestRunnerErrorLevel.Error })
)

Target "ZipWindows" (fun _ ->
    trace "\n>>> Zipping Windows Artifacts <<<"

    let appxDir = windowsDeploy + "/pool-controller/PoolControllerCross.UWP_" + appVersion + "_x64_Test"
    let appx32bitDir = windowsDeploy + "/pool-controller/PoolControllerCross.UWP_" + appVersion + "_x86_Test"
    let zipFileName = buildDir + "/PoolController_" + appVersion + ".zip"

    // Copy the readme over
    //CopyFile appxDir ("README.txt")
    //CopyFile appx32bitDir ("README.txt")

    !! (windowsDeploy + "/**")
        -- "*.zip"
        |> Zip buildDir (zipFileName)
)

"Clean"
    ==> "RestoreSolutionPackages"
    ==> "RestoreProjectPackages"
    //==> "CleanSolution"
    //==> "BuildTest"
    //==> "RunTests"
    //==> "Droid"
    //==> "iOSTesting"
    //==> "iOSStore"
    ==> "UWPx64"
    ==> "UWPx86"
    ==> "ZipWindows"

RunTargetOrDefault "ZipWindows"