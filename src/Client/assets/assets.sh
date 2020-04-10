#!/bin/sh -e

cd $(dirname $0)

yarn install

# clear vendor directory
rm -rf ../wwwroot/vendor
mkdir -p ../wwwroot/vendor

# clarity
mkdir ../wwwroot/vendor/clarity
cp node_modules/@clr/ui/clr-ui*.css* \
    node_modules/@clr/icons/clr-icons*.css* \
    node_modules/@clr/icons/clr-icons*.js* \
    ../wwwroot/vendor/clarity

# codemirror
cp -r node_modules/codemirror/lib ../wwwroot/vendor/codemirror
./node_modules/uglify-js/bin/uglifyjs -o ../wwwroot/vendor/codemirror/codemirror.min.js \
    node_modules/codemirror/lib/codemirror.js \
    node_modules/codemirror/mode/nginx/nginx.js

# jquery
cp -r node_modules/jquery/dist ../wwwroot/vendor/jquery

# webcomponents
mkdir ../wwwroot/vendor/webcomponents
cp node_modules/@webcomponents/custom-elements/custom-elements*.js* \
    ../wwwroot/vendor/webcomponents

# force watcher reload
touch ../DevOpsLab.Client.csproj