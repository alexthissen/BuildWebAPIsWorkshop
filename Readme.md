# Workshop Building Web APIs using ASP.NET Core 2.1 
## Robust and production ready 

Welcome to the workshop "Building Web APIs using ASP.NET Core 2.1". 

In this workshop you will learn about creating production-ready Web APIs with
This workshop is a living workshop: evolving, maturing and improving. 
Currently ASP.NET Core 2.1 is in Preview 2. This might work, and they might break or change.

The general idea is that you will be able to observe building a ASP.NET Core Web API 
from "File, New Project" to include more and more features that will help running it reliably in production.
The topics that are addressed are:

- Service contract integration testing
- Creating client SDKs
- Health checks
- Instrumentation and telemetry for monitoring
- Versioning
- OpenAPI documentation and Swagger tooling
- Resiliency of external resources (using HttpClientFactory, Refit and Polly)
- Security: User Secrets, HTTPS and HSTS

To get the best experience from the workshop it is recommended that you prepare the following:
- Laptop for browsing around in the code
- Azure subscription to create and use App Services and Application Insights

### Preparing for workshop

Even though you can certainly participate in this workshop without a laptop, 
it will be cool if you can install your development machine ahead of time.

For hosting your API in Microsoft Azure you will need an Azure subscription. 
You can create a trial account if you do not have a subscription available.
Visit https://azure.microsoft.com/free/ to get started with a trial

### Installation list
- Visual Studio 15.6.6 or later. Try Visual Studio 15.7.0 Preview 3.0 if you are adventurous.
  - https://www.visualstudio.com/downloads/
  Visual Studio Code (if you are not on Windows)
  - https://code.visualstudio.com/download
- .NET Core SDK 2.1 Preview 2 (or later)
  - https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.1.0-preview2-download.md
- AutoRest for generation of REST API clients
  - https://github.com/Azure/autorest#installing-autorest
- Postman to issue REST calls (optional)
  - https://www.getpostman.com/apps
- Git for Windows and a GUI client
  - https://git-scm.com/download/win
  - https://git-scm.com/download/gui/windows

### Clone the repository

The repository is located on GitHub at https://github.com/alexthissen/BuildWebAPIsWorkshop.
The clone URL is https://github.com/alexthissen/BuildWebAPIsWorkshop.git.

Your options:
- Use Visual Studio 2017
- Git UI tooling of your choice.
- Git Command Line Interface (CLI)

Start a command console and execute Git CLI commands:
```
cd c:\Sources
git clone https://github.com/alexthissen/BuildWebAPIsWorkshop.git BuildWebAPIsWorkshop
```

You can switch to specific point in the version history. 
These points show individual features being implemented as increments. 
You can follow along on your laptop and inspect at a later moment in time as well.

Some useful Git CLI commands:
```
# List available tags
git tag -l

# Checkout specific tag
git checkout tags/{tagname}

# Return to current version
git checkout master
```