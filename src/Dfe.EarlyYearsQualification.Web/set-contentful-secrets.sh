dotnet user-secrets init --id "Dfe.EarlyYearsQualification.Web"

read -p "Enter Delivery API key: " deliveryApiKey

read -p "Enter Preview API Key: " previewApiKey

read -p "Enter Space ID: " spaceID

dotnet user-secrets set "ContentfulOptions:DeliveryApiKey" "$deliveryApiKey"
dotnet user-secrets set "ContentfulOptions:PreviewApiKey" "$previewApiKey"
dotnet user-secrets set "ContentfulOptions:SpaceId" "$spaceID"