### Infiltrated build system
Another example that shocked the world is the [SolarWinds hack](https://www.techtarget.com/whatis/feature/SolarWinds-hack-explained-Everything-you-need-to-know). In this hack, the attackers inserted a backdoor into the SolarWinds Orion software. This backdoor was then distributed to all customers of SolarWinds. This backdoor was then used to infiltrate the networks of the customers of SolarWinds.

In the SolarWinds case, attackers got access to the build system. They inserted malware into the build environment that was used to build the SolarWinds Orion software. This malware was then used to insert a backdoor into the SolarWinds Orion software. This backdoor was then distributed to all customers of SolarWinds.

All the other attacks that are described in this workshop are about your dependencies and how you can protect yourself from them. A supply chain attack on your build environment is a different kind of attack. You're not in control of the packages that are cached locally on your build server. If a hacker gets access to your build server, they can add malicious code to your local cache. If NuGet decides to use the local cache of the build server to restore the packages, the malicious code will be used in your software.

### Mitigation
To protect your software from the serious risks of an infiltrated build system, it's essential to take proactive measures. Start by securing your build environment. Make sure that only authorized personnel have access to the build environment. Make sure that the build environment is up-to-date with the latest security patches. Make sure that the build environment is monitored for any suspicious activity.

From a .NET perspective, there are mainly two things that you can do to make that the build result on your local machine is the same as the build result on the build server ([reproducible builds](https://reproducible-builds.org/)).
The first thing that I would recommend is the use of a *package lock file*. With `PackageReference`, NuGet always tries to produce the same closure of package dependencies if the package reference list has not changed. However, there are a few scenarios where it may not be able to do so. Cases where the package graph may change include:
- **Package deletion**, nuget.org doesn't support package deletion, but not all package sources have this constraint.
- **Floating versions**, where the version range is not pinned to a specific version.
- **Package content mismatch**, the same version of a package may have different content on different package sources.

Starting from `Nuget.exe` 4.9 and .NET SDK 2.1.500, you can use a lock file. 
You must enable the lock file by adding the following line to your project file:
```XML
<PropertyGroup>
    <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
</PropertyGroup>
```

**Excercise**: Add the lock file to the project file and run the `dotnet restore` command. Check the output of the command and see that a lock file is generated.

This will generate a lock file with all your resolved dependencies in it. You must commit/check-in this lock file to your source control so that it is always available for `restore`.NuGet does a quick check to see if there were any changes in the package dependencies as mentioned in the project file (or dependent projects’ files) and if there were no changes, it just restores the packages mentioned in the lock file without re-evaluating the full package graph. NuGet detects a change in the defined dependencies as enumerated in the project file(s), it re-evaluates the package graph and updates the lock file to reflect the new package graph applicable for the project. This is the default mode and is useful when you are in a development environment actively modifying your package and project dependencies.

Reading this, I hear you think. If the dependency graph is re-evaluated when there are changes in the project file, how does this protect me from an infiltrated build system? Well, by default it doesn't. But you can tell the dotnet restore command to run in a `locked-mode`. You can run the `dotnet restore` command with the `--locked-mode` flag. This will make sure that the lock file is used to restore the packages and that the lock file is not updated. This way you can make sure that the packages that are restored are the same as the packages that are restored on your local machine.

**Excercise**: Change a version number of any used package in your `.csproj` file. Run the `dotnet restore` command but with the flag `--locked-mode`. This build will fail if you did the change correctly.

You can imagine that if a hacker gets access to your build environment, they will try to change the versions / packages that you use to insert their own malicious package. This `--locked-mode` will prevent the build from succeeding and fail your build.

Beside the `--locked-mode` flag you can also use the following configuration in your project file:
```XML
<PropertyGroup>
   <!-- assuming you have the MSBuild property (ContinuousIntegrationBuild) set to true -->
   <RestoreLockedMode Condition=<"'$(ContinuousIntegrationBuild)' == 'true'">true</RestoreLockedMode>
</PropertyGroup>
```

The second thing that you can do is setup your .NET build to be a [reproducible build / determenistic](https://reproducible-builds.org/). A reproducible build is a process where you compile the same source code with the same tools and settings, and it always produces the exact same output (like a binary or executable file). In simpler terms, it's like following a recipe to bake a cake: if you use the same ingredients and steps every time, you'll end up with the same cake each time. The key idea is that anyone who has the source code can rebuild the software and get the same result as the original, ensuring nothing has been altered or tampered with along the way. This helps verify that the software you have is trustworthy and hasn't been modified maliciously.

Reproducible builds are enabled by default in .NET SDK projects, but there is an extra property, `ContinuousIntegrationBuild` (also used in last example), that you can set to `true`. This forces the build server to normalize stored file paths.

To set the [`ContinuousIntegrationBuild`](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#continuousintegrationbuild) property to true, you can use the following setting in your project file:
```XML
<PropertyGroup Condition="'$(GITHUB_ACTIONS)' == 'true'">
  <ContinuousIntegrationBuild>true</ContinuousIntegrationBuild>
</PropertyGroup>
```

Exercise: Add the `ContinuousIntegrationBuild` property to the project file. 

To make your build even more reliable, [there a even more project properties](https://github.com/dotnet/reproducible-builds/blob/main/Documentation/Reproducible-MSBuild/Techniques/toc.md) that you can set. These properties are used to configure [MSBUILD](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props?view=aspnetcore-9.0).

The settings below can be configured in two ways. You can add them to every csproj file in your project or you can create a [file called `Directory.Build.props`](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-by-directory?view=vs-2022). This file enables you to customize all project files in your directory or sub-directories of the location where you place the file.

**Excercise**: Add the settings below to your solution and see the effect of each setting on your build behaviour.

- `AssemblySearchPaths` - Restrict the reference assembly search path to avoid machine-specific dependencies.
```XML
<!--
    Restrict the reference assembly search path to avoid machine-specific dependencies. See
    https://github.com/dotnet/msbuild/blob/3deec2fe6b7cdc7c3f2458d23d5451893872c031/documentation/wiki/ResolveAssemblyReference.md?plain=1#L148
  -->
<<PropertyGroup>
    <AssemblySearchPaths>
      {CandidateAssemblyFiles};
      {HintPathFromItem};
      {TargetFrameworkDirectory};
      {RawFileName}
    </AssemblySearchPaths>
  </PropertyGroup>
```

- `NetCoreTargetingPackRoot` - Disable msbuild's lookup of dotnetcore through default install locations. Instead, resolve from nuget feed.
```XML
<PropertyGroup>
   <NetCoreTargetingPackRoot>[UNDEFINED]</NetCoreTargetingPackRoot>
</PropertyGroup>
```

- `DisableImplicitNuGetFallbackFolder` and `DisableImplicitLibraryPacksFolder` - Disable the extra implicit nuget package caches provided by dotnetsdk.
```XML
<PropertyGroup>
   <DisableImplicitNuGetFallbackFolder>true</DisableImplicitNuGetFallbackFolder>
   <DisableImplicitLibraryPacksFolder>true</DisableImplicitLibraryPacksFolder>
</PropertyGroup>
```

All these settings are good to understand and now what they do. To make this easier for you, Microsoft created a [NuGet package 'ReproducibleBuilds'](https://github.com/dotnet/reproducible-builds/tree/main) that does all these settings for you.

This is the end of the workshop!