param location string = 'westeurope'
param rgName string = 'myResourceGroup'
param appServicePlanName string = 'myAppServicePlan'
param webAppName string = 'myWebApp'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: rgName
  location: location
}

resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
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

resource webApp 'Microsoft.Web/sites@2021-02-01' = {
  name: webAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
  }
}
