param location string = resourceGroup().location
param appServicePlanName string
param webAppName string

resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: appServicePlanName
  location: location
  properties: {
    perSiteScaling: false
    reserved: false
  }
  sku: {
    name: 'B1' // Basic tier
    tier: 'Basic'
    size: 'B1'
    capacity: 1
  }
}

resource webApp 'Microsoft.Web/sites@2024-04-01' = {
  name: webAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
  }
}

output webAppUrl string = webApp.properties.defaultHostName
