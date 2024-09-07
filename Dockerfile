FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
ADD ./out .

STOPSIGNAL SIGQUIT
CMD [ "dotnet", "AppEntryPoint.dll" ]
