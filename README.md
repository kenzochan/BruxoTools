To run, type "dotnet run" in the cmd

To deploy:
 Compress-Archive -Path ./publish/* -DestinationPath deploy.zip
az webapp deploy --resource-group BruxoTools --name BruxoTools --src-path deploy.zip
*Remember to delete the deploy's zip