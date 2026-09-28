FROM otel/opentelemetry-collector-contrib:0.161.0
COPY src/Dfe.EarlyYearsQualification.Web/otel-config.yaml /etc/otelcol-contrib/config.yaml