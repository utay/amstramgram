FROM microsoft/aspnetcore-build:2.0

COPY . /app

RUN cd /app/Api && dotnet publish -c Release -o published

CMD ["/app/run.sh"]
