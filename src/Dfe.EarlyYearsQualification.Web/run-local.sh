#!/usr/bin/env bash

# 1. Locate the .csproj file automatically
CSPROJ_PATH="Dfe.EarlyYearsQualification.Web.csproj"

if [[ -z "$CSPROJ_PATH" ]]; then
  echo "Error: Could not find a .csproj file!" >&2
  exit 1
fi

echo "Exporting user secrets for $CSPROJ_PATH into .env..."

# 2. Extract secrets and convert keys (Nested:Key -> Nested__Key) into .env
dotnet user-secrets list --project "$CSPROJ_PATH" | \
  sed 's/ = /=/' | \
  sed 's/:/__/g' > .env

# 3. Launch Docker Compose
echo "Starting Docker Compose..."
docker-compose -f docker-compose-o11y.yml up --build