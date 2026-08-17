FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /
EXPOSE 8080 

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /
COPY src/ src/
RUN dotnet publish "src/Dfe.EarlyYearsQualification.Web/Dfe.EarlyYearsQualification.Web.csproj" -c Release -o /out/publish
RUN rm -rf /src

FROM base AS final
WORKDIR /src
RUN useradd -m dotnet
COPY --from=build /out/publish .
RUN chown dotnet:dotnet /src .
USER dotnet

EXPOSE 4318
ENV OTEL_SERVICE_NAME=early-years-qualification 
ENV OTEL_EXPORTER_OTLP_ENDPOINT=http://otel-collector:4318 
ENV OTEL_EXPORTER_OTLP_PROTOCOL=http/protobuf 
ENV OTEL_TRACES_EXPORTER=otlp
ENV OTEL_RESOURCE_ATTRIBUTES=deployment.environment=staging,service.version=1.0.0,service.namespace=dfe 
ENV OTEL_TRACES_SAMPLER=parentbased_always_on 
ENV OTEL_METRICS_EXPORTER=none 
ENV OTEL_LOGS_EXPORTER=none 
ENV DOTNET_EnableDiagnostics=1 
ENTRYPOINT ["/src/splunk-launch.sh","dotnet", "Dfe.EarlyYearsQualification.Web.dll"]