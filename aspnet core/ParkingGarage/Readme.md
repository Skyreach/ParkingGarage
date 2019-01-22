# ParkingGarage

##steps to run:
- This application uses noSQL as it's datastore using local Emulation.
Download Link (Windows): https://aka.ms/cosmosdb-emulator
Documentation: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator

- Once Azure Cosmos DB Emulator is running, run the data explorer to retrieve URI, Primary Key, Primary Connection String
- Update appsettings.json values 
    "Endpoint": retrieved URI,
    "Key": retrieved Key,
    "PrimaryConnectionString": retrieved Primary Connection String,

- Build and Run project, there is a default view that loads, I've done nothing with it so it's safe to ignore
    
- If using Postman to test endpoints, they are configured in HTTPS. Thus SSL certificate verification should be disabled when running locally

#endpoints
- GET: /ticket, retrieves all tickets for all registered
- POST: /ticket, [FromBody] licensePlate as string (e.g. 'AAAA 123', has a default "No Vehicle Association"), will create a new ticket or retrieve existing, if there is a collision on license plate
- GET: /ticket/{ticketId} retrieves ticket information
- POST: /payments/{ticketId}, [FromBody] credit card info

Alternatively, unit tests can also be run to see app with pre-determined values

##Todos:
- Introduce user workflow for registering and vehicle associations
- Change returns to leverage view instead of raw data models
- Update PaymentsController Unit Tests to be more dependant on mocks
- Implement DataService Layer for cleaner local state management

##Other considerations
- Build a frontend, lol
- Have a registered user and scope tickets to them, beyond just license plate info
- Configuring CosmosDB registry to be more generic
- Consider swagger as API documenting layer
- Consider implementing Mediator pattern to dynamically register assemblies
- Containerization, Travis CI, etc...