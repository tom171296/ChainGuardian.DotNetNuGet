### Restoring packages
Great now that we have tackled the **known** vulnerabilities in our software, let's have a look at the **hidden dangers** of the software supply chain. Before we dive into the dangers, it is good to understand how the NuGet package restore works.

Using third party packages requires you to restore them via NuGet. All project dependencies that are listed in either a project file or a packages.config file are restored.

Package restore first installs all the direct dependencies. These are the dependencies that are directly referenced between the <PackageReference> tags or <package> tags. After the direct dependencies all transitive packages are installed. By default, NuGet will first scan the local global packages or HTTP cache folders. If the required package isn't in the local folders, NuGet tries to download it.

What makes this behavior dangerous is that **NuGet ignores the order of package sources configured**. It first looks in the local NuGet cache, it the package is not present, it fires a request to each package source configured. It uses the package source that responds the quickest. This means that **by default, you don't have any control from what source you are restoring your packages**. If a restore fails, NuGet doesn't indicate the failure until after it checks all sources. NuGet reports a failure ony for the last source in the list. The error implies that the package wasn't present on any of the resources.

![Nuget Supply Chain Security](/assets/images/nuget-restore.drawio.png)

Package resolution for transitive dependencies follows a set of rules. To understand certain risks and how to mitigate them, it is important to understand how Nuget resolves transitive dependencies. Before your read further, I recommend you to read the [official documentation](https://learn.microsoft.com/en-us/nuget/concepts/dependency-resolution) on how Nuget resolves transative dependencies.

Now we are going to have a look at two different types of supply chain attacks that can happen when your packages are being restored.

Look at two different types of supply chain attacks that can happen when your packages are being restored.
- [dependency confusion](./04-dependency-confusion.md)
- [typosquatting](./05-typosquatting.md)