mkdir coverage\integration
OpenCover.Console.exe -target:"dotnet.exe" -targetargs:"test -f netcoreapp1.1 -c Release Tests/Amstramgram.Tests.Integration/Amstramgram.Tests.Integration.csproj" -hideskipped:File -output:coverage/integration/coverage.xml -oldStyle -filter:"+[Amstramgram*]* -[Amstramgram.Tests*]* -[Api]*Program -[Api]*Startup -[Data]*EntityFramework.Workaround.Program -[Data]*EntityFramework.Migrations* -[Data]*EntityFramework.Seed*" -searchdirs:"Tests/Amstramgram.Tests.Integration/bin/Release/netcoreapp1.1" -register:user
ReportGenerator.exe -reports:coverage/integration/coverage.xml -targetdir:coverage/integration -verbosity:Error
start .\coverage\integration\index.htm
