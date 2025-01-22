#### Dependency confusion
Imagine that you work at a company that uses both a public and a private Nuget feed to retrieve the packages that are used in your software projects. You have a package that is called MyCompany.Common. You have a build pipeline that builds the package and pushes it to the private Nuget feed. Via some way of research / reverse engineering, a hacker found the name of your private image used internally (MyCompany.Common). The hacker then creates a package with the same name, version and adds some malware/backdoor to the source code and uploads it to the public Nuget feed.

Because of the way a nuget restore works (it uses the first source that responds the quickest), the malicious package from the public Nuget feed can be installed instead of the package from the private Nuget feed. This is called dependency confusion. The hacker is able to inject malicious code into your software without you knowing it. This is a big risk for your software supply chain.

For example, you are using the package MyCompany.Common with version `1.0.0` and you reference it in your project file like this <PackageReference Include="MyCompany.Common" Version="1.0.*" />. You have a connection to your own private feed and the public Nuget feed. The hacker uploads a package with the name MyCompany.Common with version 1.0.1 to the public Nuget feed. When you run a restore, NuGet will restore the package of the hacker because of the resolution rules. You can **unintentionally** introduce a new way for your software to be vulnerable for a supply chain attack. It is good to understand how [Nuget semantic versioning works](https://learn.microsoft.com/en-us/nuget/concepts/package-versioning?tabs=semver20sort#references-in-project-files-packagereference) and what the risks are of the way you reference your packages in your project file.

##### Mitigations
**NOTE**: before you perform the next exercise make sure that you run the command `dotnet nuget locals all --clear`. This will clear you local cached packages, so that you can see the effect of the mitigations that you are going to do. If you don't run this command, NuGet will use the cached packages and you will not see the effect of the mitigations.

By default, having both a public and a private feed is a risk. I would recommend to **only use a private feed**. This way you (your company) has control over the packages that are available to your software projects. You can configure Nuget to only use the private feed. Configuring NuGet to use a certain package source is done via the [`nuget.config` file](https://learn.microsoft.com/en-us/nuget/reference/nuget-config-file).

**Exercise**: Create a `nuget.config` file in the root of the repository and configure NuGet to use the public NuGet feed.

If for some reason you use / require a public feed next to your private feed, there are a few things that you can do to mitigate the risk of dependency confusion. The first thing that you can (if you publish a package yourself) is [claim the prefix](https://learn.microsoft.com/en-us/nuget/nuget-org/id-prefix-reservation) of your packages. This means that you claim the prefix of your packages on the public Nuget feed. This way, the public Nuget feed will not allow anyone to upload a package with the same prefix as your packages. This will prevent the hacker from uploading a package with the same name as your package. In the example of `MyCompany.Common`, you would claim the prefix `MyCompany`.

We are **not going** to do this in this workshop, but it is good to know that this is a possibility.

The second thing that you can do is use the `nuget.config` file to configure the package sources that you want to use. You can add a `packageSourceMapping` element to the `nuget.config` file. This element allows you to map a package source to a specific feed. This way you can configure NuGet to only use the private feed for the package `MyCompany.Common`.

**Exercise**: Add the package source mapping to the `nuget.config` file and configure NuGet to pull the required packages from the configured feed. 

A third thing that you can do is to **configure trusted signers**. This way you can configure NuGet to only accept packages that are signed by a trusted signer. This way you can make sure that the packages that are being restored are coming from a trusted source. This is a good way to mitigate the risk of dependency confusion.

**Exercise**: Configure trusted signers in the `nuget.config` file.

To protect your software from the serious risks of dependency confusion, it's essential to take proactive measures. Start by configuring Nuget to only use a private feed, ensuring control over the packages integrated into your projects. If you must use a public feed, mitigate risks by claiming your package prefix, setting up package source mapping, and configuring trusted signers. Taking these steps will significantly reduce the likelihood of malicious code infiltrating your software supply chain. Act now by reviewing and updating your Nuget configurations to safeguard your projects against these potential vulnerabilities.

Go to the next attack: [05. typosquatting attack](./05-typosquatting.md)
