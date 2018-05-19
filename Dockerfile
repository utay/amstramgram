FROM microsoft/aspnetcore-build:2.0

RUN apt-get update
RUN apt-get -y install nodejs

COPY . /app

RUN cd /app/Front && npm install && npm run build
RUN cd /app/Api && dotnet publish -c Release -o published

CMD ["/app/run.sh"]
