dotnet publish Flow.Launcher.Plugin.Kaomoji -c Release -r win-x64 --no-self-contained
Compress-Archive -LiteralPath Flow.Launcher.Plugin.Kaomoji/bin/Release/win-x64/publish -DestinationPath Flow.Launcher.Plugin.Kaomoji/bin/Kaomoji.zip -Force