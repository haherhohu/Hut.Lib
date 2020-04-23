 # packaging sample script with nuget.package 
 # if using, change version (1.0.0)
 
 dotnet pack
 dotnet nuget push /bin/Debug/Hut.Lib.1.0.0.nupkg -k oy2mwz4koqdyge3pg5fqagjdmzpvcxgezm43smmtn2dj7y -s https://api.nuget.org/v3/index.json