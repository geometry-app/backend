FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
ADD ./out .

STOPSIGNAL SIGQUIT
CMD [ "dotnet", "AppEntryPoint.dll" ]
