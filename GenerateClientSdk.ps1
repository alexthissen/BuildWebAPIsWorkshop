# TODO: Parametrize URL and name 
iwr https://localhost:44383/swagger/v1/swagger.json -o BuildWebAPIs.v1.json
autorest --input-file=BuildWebAPIs.v1.json --csharp --output-folder=GenealogyWebAPI.ClientSdk --namespace=GenealogyWebAPI.ClientSdk

dotnet new sln -n GenealogyClientSdk --force
dotnet new classlib -o GenealogyWebAPI.ClientSdk --force
dotnet sln GenealogyClientSdk.sln add GenealogyWebAPI.ClientSdk
cd GenealogyWebAPI.ClientSdk
del class1.cs

dotnet add package Microsoft.AspNetCore --version 2.1-preview2-final
dotnet add package Microsoft.Rest.ClientRuntime --version 2.3.11
dotnet add package Newtonsoft.Json --version 11.0.2

dotnet restore
dotnet build
dotnet pack --include-symbols