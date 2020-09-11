$env:Path += ";C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin"
MSBuild iTunesAssistant.sln -t:Rebuild -p:Configuration=Release
xcopy /I /Y .\iTunesAssistant\bin\Release\netcoreapp3.1 ..\..\Tools\iTunesAssistant