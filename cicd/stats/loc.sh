#!/bin/bash

cd $(dirname $0)
cd ../../

cloc --exclude-dir=node_modules,vendor,bin,obj,Migrations --not-match-d="^\..*\$" --force-lang=Razor,razor .

