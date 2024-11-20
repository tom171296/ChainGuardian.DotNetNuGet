#### Typosquatting
A different way to attack your software supply chain with the focus on your dependencies is by using a typosquatting attack. Typosquatting is a form of cybersquatting that relies on mistakes such as typographical errors made by users when inputting a website address into a web browser. Should a user accidentally enter an incorrect website address, they may be led to an alternative website that could contain malware, phishing scams, or other malicious content.

In .NET we have the command dotnet add package <package-name>. This command will add a package to your project. The package name that you provide is used to download the package from the Nuget feed. If you make a typo in the package name, Nuget will try to download the package with the typo in the name. This is where the risk of typosquatting comes in. A hacker can upload a package with a name that is very similar to a popular package. If you make a typo in the package name, Nuget will download the package from the hacker instead of the package from the original author. Looking at the MyCompany.Common example, the hacker can upload a package with the name MyCompany.Commonn to the public Nuget feed. If you make a typo in the package name, Nuget will download the package from the hacker.

##### Mitigations
Some of the mitigations that you did for the dependency confusion attack can also be used for the typosquatting attack.
- Use a private feed, this way you have control over the packages that are available to your software projects.
- Configure trusted signers. This way you can configure Nuget to only accept packages that are signed by a certain trusted signer.

By implementing these proactive measures, you can significantly reduce the risk of typosquatting attacks on your software supply chain. Safeguard your projects against potential vulnerabilities by reviewing and updating your Nuget configurations to protect your software from malicious code infiltration.

Go to the next attack: [06. infiltrated build system](./06-infiltrated-build-system.md)