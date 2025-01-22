# Vulnerabilities

If you include software of which you don't know the origin, you are exposed to the risk of including malicious code in your software. There can be vulnerabilities in the package that could be exploited and be used as a backdoor to harm your environment.

Looking at recent history, there a examples of malicious code having a big impact on the world of software development. One of the most famous examples was the finding of a vulnerability in Log4J, an open-source logging library that is widely used by apps and services across the internet. Exploiting this vulnerability, attackers could break into systems, steal passwords and logins, extract data and infect networks.

The `dotnet list` command has an option that you can list all vulnerable packages.

Run `dotnet list package --vulnerable`, this will output all vulnerabilities that are present in the project. The output will say that there are no vulnerabilities present in the project. This is because the list command by default doesn't look at all transitive packages like you saw before.

To get all vulnerabilities, including your transitive packages, run `dotnet list package --include-transitive --vulnerable`. This result will say that there is a vulnerability in the transitive package `System.Text.Json` version that is used.

The output of these commands tell you a few different things:

- In what package (top level or transitive) the vulnerability is present
- What the impact/severity of the vulnerability is
- A link to the CVE (Common Vulnerabilities and Exposures) database where you can find more information about the vulnerability

Open one of the links from the vulnerabilities that are present in your project. This will give you more information about the vulnerability and how to mitigate it. **Do not patch this vulnerability yet**, we will do this later in the workshop.

Starting from .NET 8 (SDK 8.0.100) the `restore` command can audit([Auditing package dependencies for security vulnerabilities | Microsoft Learn](https://learn.microsoft.com/en-us/NuGet/concepts/auditing-packages)) all your 3rd party packages. The restore command does a vulnerability check on all the packages that are being restored. If you only use a private feed, like Azure DevOps artifacts, the restore command is not able to find any vulnerabilities because azure devops by default doesn't have any CVE database or information. 

With the introduction of .NET 9, the private artifact store issue is fixed. There is a new property that you can add to the `NuGet.config` file that allows you to set an audit source. By setting this property, NuGet gets its [vulnerabilityInfo resource](https://learn.microsoft.com/en-us/NuGet/api/vulnerability-info) from a different source than the configured NuGet feeds.

Run the command `dotnet restore` in the root of the repository. This will restore all the packages and do a vulnerability check on all the packages that are being restored. **Check the output of the restore command.** This will output a list of all the vulnerabilities that are present in the packages that are being restored as warnings in the console.

The console outputs only the direct dependencies that have vulnerabilities. In the .NET 8.0.100 SDK the `restore` command defaulted all settings that are required to do a vulnerability check:

- `NuGetAudit` is set to `true`
- `NuGetAuditMode` is set to `direct`
- `NuGetAuditLevel` is set to `low`

*In .NET 9.0.100 SDK value of `NuGetAuditMode` is changed to `all`

Add the following line to the `ChainGuardian.csproj` file:

```xml
<PropertyGroup>
    <NuGetAudit>true</NuGetAudit>
    <NuGetAuditMode>direct</NuGetAuditMode>
    <NuGetAuditLevel>low</NuGetAuditLevel>
</PropertyGroup>
```

See all the different options that are possible in the [Microsoft documentation](https://learn.microsoft.com/en-us/NuGet/concepts/auditing-packages#configuring-NuGet-audit) and **make sure that the restore command will output all the vulnerabilities that are present in the packages that are being restored.**

Now you have a situation where the restore command will output all the vulnerabilities that are present in the packages that are being restored. This is a good first step in securing your software supply chain. However, this is not enough. You need to make sure that you are aware of the vulnerabilities that are present in your software and that you are able to patch them.

.NET has a build in feature that can help you with that. You can make your build fail if there are vulnerabilities present in your software. To make the actual build fail when a vulnerability is found, **add the following line to your project file**:

```xml
<WarningsAsErrors>NU1901,NU1903,NU1903,NU1904</WarningsAsErrors>
```

Run the `dotnet restore` or `dotnet build` command and see that you are no longer able to create a successful build. **Now update the packages so that the build succeeds.**

Go to the next step: [03. license trap](./03-license-trap.md.md)