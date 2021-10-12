#!/bin/bash

solution_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )
build_dir="$solution_dir/../Build"

version=$( head -n 1 $solution_dir/../MinecraftVersion.txt )
server_jar="$build_dir/Cache/MinecraftServer-$version.jar"

if [ ! -f "$server_jar" ]; then
    echo "$server_jar does not exist - aborting"
    
    exit 1
fi

if [ ! command -v java &> /dev/null ]; then
    echo "java could not be found"
    
    exit 1
fi

java -cp "$server_jar" net.minecraft.data.Main --all --output "$build_dir/generated"

echo "Done!"
