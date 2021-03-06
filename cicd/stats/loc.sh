#!/bin/sh -e

cd "$(dirname $0)/../../"

cloc \
    --exclude-dir=.bin,.obj,node_modules,vendor,wwwroot,Data,Migrations \
    --not-match-d="^\..*\$" \
    --force-lang=Razor,razor . \
    --include-lang=C#,Razor
