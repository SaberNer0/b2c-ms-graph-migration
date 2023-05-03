## Prerequisites

* [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) for debugging or file editing
* [.NET Core SDK](https://dotnet.microsoft.com/) 3.1+
* [Azure AD B2C tenant](https://docs.microsoft.com/azure/active-directory-b2c/tutorial-create-tenant) with one or more user accounts in the directory
* [Management app registered](https://docs.microsoft.com/azure/active-directory-b2c/microsoft-graph-get-started) in your B2C tenant

## Setup

1. Clone the repo or download and extract the [ZIP archive](https://github.com/Azure-Samples/ms-identity-dotnetcore-b2c-account-management/archive/master.zip)
2. Modify `./src/appsettings.json` with values appropriate for your environment:
    - Azure AD B2C **tenant ID**
    - Registered application's **Application (client) ID**
    - Registered application's **Client secret**
3. Build the application with `dotnet build`:

    ```console
    azureuser@machine:~/ms-identity-dotnetcore-b2c-account-management$ cd src
    azureuser@machine:~/ms-identity-dotnetcore-b2c-account-management/src$ dotnet build
    Microsoft (R) Build Engine version 16.4.0+e901037fe for .NET Core
    Copyright (C) Microsoft Corporation. All rights reserved.

      Restore completed in 431.62 ms for /home/azureuser/ms-identity-dotnetcore-b2c-account-management/src/b2c-ms-graph.csproj.
      b2c-ms-graph -> /home/azureuser/ms-identity-dotnetcore-b2c-account-management/src/bin/Debug/netcoreapp3.0/b2c-ms-graph.dll

    Build succeeded.
        0 Warning(s)
        0 Error(s)

    Time Elapsed 00:00:02.62
    ```
4. Add 2 custom attributes to your B2C instance in order to run the sample operation with custom attributes involved.
   Attributes to add:
    - Role (string)
    - StudioKey (int)
