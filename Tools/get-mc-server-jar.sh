#!/bin/bash

solution_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )
cache_dir="$solution_dir/../Build/Cache"

version=$( head -n 1 $solution_dir/../MinecraftVersion.txt )
outJar="$cache_dir/MinecraftServer-$version.jar"

if [ -f "$outJar" ]; then
    echo "$outJar already exists - aborting"
    
    exit 0
fi

mkdir -p "$cache_dir"

echo "Searching for .minecraft/versions/$version/$version.json ..."

if [[ "$OSTYPE" == "cygwin" || "$OSTYPE" == "msys" ]]; then
    MCDIR=$( cygpath -u $APPDATA\\.minecraft )
    
elif [[ "$OSTYPE" == "darwin"* ]]; then
    MCDIR="~/Library/Application Support/minecraft"
    
else
    MCDIR="~/.minecraft"
    
fi

MCJSON="$MCDIR/versions/$version/$version.json"

if [ -f "$MCJSON" ]; then
    echo "Found version $version"
    MCURL=$(grep -o -P '(?<=")[[:alnum:]:\/.]+server.jar(?=")' "$MCJSON")
    
    if [[ $MCURL == https* ]]; then
        echo "Downloading server from $MCURL"
        curl -o "$outJar" "$MCURL"
        echo "Done!"
    else
        echo "Error: could not find server download URL string in version manifest"
    fi
else
    echo "Error: could not find version $version"
    echo "Please run this version from your Minecraft launcher first"
    
    exit 1
fi
