# ParkingGarage

##steps to run:
- This application uses Azure CosmosDB as it's datastore using local Emulation.
Download Link (Windows): https://aka.ms/cosmosdb-emulator
Documentation: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
- If using Postman to test endpoints, they are configured in HTTPS. Thus SSL certificate verification should be disabled when running locally


##Todos:
- unit testing
- integration testing
- finishing endpoints
- Change returns to leverage view instead of raw data models
- Consider swagger as API documenting layer.

##Other considerations
- Have a registered user and scope tickets to them
- Configuring CosmosDB registry to be more generic
- Consider implementing Mediator pattern to dynamically register assemblies