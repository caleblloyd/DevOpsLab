#!/bin/bash

templates="Account.Login;Account.Register"

dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

rm Directory.Build.props
dotnet build
dotnet aspnet-codegenerator identity \
    -u ApplicationUser \
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
    | xargs sed -i 's/using DevOpsLab.Server.Areas.Identity.Data;/using DevOpsLab.Server.Db;\nusing DevOpsLab.Shared.Models;/g'

find Pages Areas/Identity -type f -name "*.cshtml" \
    | xargs sed -i 's/@using DevOpsLab.Server.Areas.Identity.Data/@using DevOpsLab.Server.Db\n@using DevOpsLab.Shared.Models/g'

dotnet build
