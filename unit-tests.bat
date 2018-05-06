mkdir coverage\unit
OpenCover.Console.exe -target:"dotnet.exe" -targetargs:"test -f netcoreapp1.1 -c Release Tests/Amstramgram.Tests.Unit/Amstramgram.Tests.Unit.csproj" -hideskipped:File -output:coverage/unit/coverage.xml -oldStyle -filter:"+[Amstramgram*]* -[Amstramgram.Tests*]* -[Api]*Program -[Api]*Startup -[Data]*EntityFramework.Workaround.Program -[Data]*EntityFramework.Migrations* -[Data]*EntityFramework.Seed*" -searchdirs:"Tests/Amstramgram.Tests.Unit/bin/Release/netcoreapp1.1" -register:user
ReportGenerator.exe -reports:coverage/unit/coverage.xml -targetdir:coverage/unit -verbosity:Error
start .\coverage\unit\index.htm
