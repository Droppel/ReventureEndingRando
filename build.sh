mkdir build
rm -rf build/*
cp Client/bin/Debug/netstandard2.0/Archipelago.MultiClient.Net.dll build/Archipelago.MultiClient.Net.dll

# Build APWorld
zip -r reventure.apworld World/*
mv reventure.apworld build/reventure.apworld

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