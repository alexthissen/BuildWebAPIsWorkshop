# Workshop Building Web APIs using ASP.NET Core 2.1 
## Robust and production ready 

Welcome to the workshop "Building Web APIs using ASP.NET Core 2.1". 

In this workshop you will learn about creating production 

To get the best experience with the hands-on-labs it is recommended that you prepare the following:
- Laptop for following along with the code
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
- Git CLI

Start a command console and execute Git CLI commands
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