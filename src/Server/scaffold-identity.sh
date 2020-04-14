#!/bin/bash

templates="Account.Login;Account.Register;Account.Manage.Index"

dotnet tool install -g dotnet-aspnet-codegenerator \
    || dotnet tool update -g dotnet-aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

rm Directory.Build.props
rm -rf Areas Pages/_ViewImports.cshtml

dotnet build
dotnet aspnet-codegenerator \
    --no-build \
    identity \
    --listFiles
dotnet aspnet-codegenerator \
    --no-build \
    identity \
    -u AppUser \
    -dc AppDb \
    -fi "$templates" \
    --force \
    --useSqLite

git checkout -- \
    appsettings.json \
    DevOpsLab.Server.csproj \
    Directory.Build.props

rm -rf \
    bin \
    obj \
    ScaffoldingReadMe.txt \
    Areas/Identity/IdentityHostingStartup.cs \
    Areas/Identity/Data

find Areas/Identity -type f -name "*.cs" \
    | xargs sed -i 's/using DevOpsLab.Server.Areas.Identity.Data;/using DevOpsLab.Server.Db;\nusing DevOpsLab.Server.Models;/g'

find Pages Areas/Identity -type f -name "*.cshtml" \
    | xargs sed -i 's/@using DevOpsLab.Server.Areas.Identity.Data/@using DevOpsLab.Server.Db\n@using DevOpsLab.Server.Models/g'

dotnet build
