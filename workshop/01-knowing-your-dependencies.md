### Knowing what is in you software
Open the file `./src/ChainGuardian/ChainGuardian.csproj` in your code editor. This file contains the project top level dependencies. These dependencies are the NuGet packages that your application relies on. To list all these packages, run the following command in the terminal:

```powershell
dotnet list package
```

These dependencies are just your top-level dependencies. Your supply chain consist of much more packages than just the packages that are defined in your project file. To get a full overview of all the packages that are used in your application, run the `dotnet list package` command with the `--include-transitive` flag:

```powershell
dotnet list package --include-transitive
```

See the entire [`dotnet list` command](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-list-package) for all the other possibilities.

#### SBOM
If your software is running in production the list package command is never ran for that software. To be able to monitor and document all the dependencies that you have in your running software, you can create a **bill of material** file. This file contains a nested description of software artifact components and metadata. This information can also include licensing information, persistent references, and other auxiliary information. This file can than be used to monitor all dependencies and licenses for your software.

To get a software bill of material, there are several tools that you can use. For this workshop we will use the [dotnet CycloneDX tool](https://github.com/CycloneDX/cyclonedx-dotnet). This tool supports the CycloneDX format. There are multiple SBOM formats, but the two most populair ones are CycloneDX and SPDX. These formats contain overlapping information. However, the two formats have tradionally two different use cases within the software development lifecycle. The SPDX was primarily designed as a way to manage open-source software liceness and share information about the packages. CycloneDX allows users to create SBOMs that provide detailed information about all components like dependencies. [To see a full comparison, I would recommend this blog.](https://scribesecurity.com/blog/spdx-vs-cyclonedx-sbom-formats-compared/)

Install the CycloneDX tool:

```powershell
dotnet tool install --global CycloneDX
```

Generate the SBOM file for the Chainguardian solution:

```powershell
dotnet-cyclonedx ./ChainGuardian.DotNetNuGet.sln
```

This will generate a file called `bom.xml` in the root of the repository. Open this file and inspect the content of it.

Go to the next step: [02. vulnerabilities.md](./02-vulnerabilities.md)