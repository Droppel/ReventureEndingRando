mkdir -p build
rm -rf build/*
cp Client/bin/Debug/netstandard2.0/Archipelago.MultiClient.Net.dll build/Archipelago.MultiClient.Net.dll

# Build APWorld
mkdir -p tmp/reventure
cp -r World/* tmp/reventure/
rm -rf tmp/reventure/__pycache__
cd tmp
zip -r reventure.apworld reventure/*
cd ..
pwd
mv tmp/reventure.apworld build/reventure.apworld
rm -rf tmp/

# Build RegionsGenerator
# Linux
cd RegionsGenerator
cargo build --release
mv target/release/reventureregions ../build/regions_generator

# Windows
cargo build --release --target x86_64-pc-windows-gnu
mv target/x86_64-pc-windows-gnu/release/reventureregions.exe ../build/regions_generator.exe

# Build Client
cd ../Client
dotnet build
mv bin/Debug/netstandard2.0/ReventureEndingRando.dll ../build/ReventureEndingRando.dll