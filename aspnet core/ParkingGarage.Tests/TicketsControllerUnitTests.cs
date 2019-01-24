using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using ParkingGarage.Common;
using ParkingGarage.Controllers;
using ParkingGarage.Extensions;
using ParkingGarage.Models;
using ParkingGarage.Repositories;
using ParkingGarage.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParkingGarage.Tests {
    public class TicketsControllerUnitTests {

        private LocationSettings LocationSettings 
            => new LocationSettings() {
                BaseRate = "3.00",
                Occupancy = "50",
                RateDurations = "60,180,360,1440",
                RateIncreasePercent = "1.50",
            };

        [Fact]
        public async Task Ticket_Controller_GetAll_Should_Retrieve_Tickets() {
            var ticketServiceMock = new Mock<ITicketService>(MockBehavior.Strict);
            var locationService = new LocationService(Options.Create(LocationSettings));

            var ticketDocument = File.ReadAllText(GetFilePath("TicketDocument"));
            var ticketBase = JsonConvert.DeserializeObject<Ticket>(ticketDocument);

            var ticket1 = new Ticket(ticketBase);
            var ticket2 = new Ticket(ticketBase);

            ticket1.Id = "One";
            ticket1.TimeIssued = DateTime.Now;

            ticket2.Id = "Two";
            ticket2.TimeIssued = DateTime.Now;

            var tickets = (new List<Ticket>() { ticket1, ticket2 }).Cast<Ticket>();

            ticketServiceMock.Setup(x => x.GetAll()).Returns(Task.FromResult(tickets));
            //ticketServiceMock.Setup(x => x.GetAsync(It.IsAny<string>(), true))
            //    .Returns<string, bool>((id, isActive) => Task.FromResult(
            //        tickets.Where(t => t.Id == id)
            //        .FirstOrDefault()));
            //ticketServiceMock.Setup(x => x.GetExistingAsync(It.IsAny<string>()))
            //    .Returns<string>(license => Task.FromResult(
            //        tickets.Where(t => t.LicensePlate == license)
            //        .FirstOrDefault()));

            var controller = new TicketsController(locationService, ticketServiceMock.Object);

            var result = await controller.Index();

            var okResult = result.Should().BeOfType<List<Ticket>>().Subject;

            okResult.ToList().Count().Should().Be(2);
        }

        [Fact]
        public async Task Ticket_Controller_Should_Get_Specific() {
            var locationServiceMock = new Mock<ILocationService>(MockBehavior.Loose);
            var ticketServiceMock = new Mock<ITicketService>(MockBehavior.Strict);

            var ticketDocument = File.ReadAllText(GetFilePath("TicketDocument"));
            var ticketBase = JsonConvert.DeserializeObject<Ticket>(ticketDocument);

            var ticket1 = new Ticket(ticketBase);
            var ticket2 = new Ticket(ticketBase);

            ticket1.Id = "One";
            ticket1.TimeIssued = DateTime.Now;

            ticket2.Id = "Two";
            ticket2.TimeIssued = DateTime.Now;

            var tickets = (new List<Ticket>() { ticket1, ticket2 }).Cast<Ticket>();

            ticketServiceMock.Setup(x => x.GetAsync(It.IsAny<string>(), true))
                .Returns<string, bool>((id, isActive) => Task.FromResult(
                    tickets.Where(t => t.Id == id)
                    .FirstOrDefault()));

            var controller = new TicketsController(locationServiceMock.Object, ticketServiceMock.Object);

            var result = await controller.Get("One");

            var okResult = result.Should().BeOfType<Ticket>().Subject;

            okResult.Should().BeEquivalentTo<Ticket>(ticket1);
        }

        [Fact]
        public async Task Ticket_Controller_Post_With_Occupancy_Should_Create_Ticket() {

            var ticketServiceMock = new Mock<ITicketService>(MockBehavior.Strict);

            var locationService = new LocationService(Options.Create(LocationSettings));

            var ticketsContainer = new List<Ticket>();

            var ticketDocument = File.ReadAllText(GetFilePath("TicketDocument"));
            var ticketBase = JsonConvert.DeserializeObject<Ticket>(ticketDocument);

            var ticket1 = new Ticket(ticketBase);
            var ticket2 = new Ticket(ticketBase);

            ticket1.Id = "One";
            ticket1.TimeIssued = DateTime.Now;

            ticket2.Id = "Two";
            ticket2.TimeIssued = DateTime.Now;

            var tickets = (new List<Ticket>() { ticket1, ticket2 }).Cast<Ticket>();

            ticketServiceMock.Setup(x => x.GetExistingAsync(It.IsAny<string>()))
                .Returns<string>(license => Task.FromResult(
                    tickets.Where(t => t.LicensePlate == license)
                    .FirstOrDefault()));

            ticketServiceMock.Setup(x => x.Create(It.IsAny<Ticket>()))
                .Returns<Ticket>(ticket => {
                    ticketsContainer.Add(ticket);

                    var doc = new Document();
                    var castTicket = JsonConvert.SerializeObject(ticket);
                    
                    doc.LoadFrom(new JsonTextReader(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(castTicket)))));

                    return Task.FromResult(doc);
                });

            var controller = new TicketsController(locationService, ticketServiceMock.Object);

            var licensePlate = "123 456";
            var result = await controller.Post(licensePlate);

            var okResult = result.Should().BeOfType<Ticket>().Subject;

            ticketsContainer.Should().HaveCount(1);
        }

        [Fact]
        public async Task Ticket_Controller_Post_With_Already_Registered_Vehicle_Should_Not_Create_New_Ticket() {

            var cosmosDBRepositoryMock = new Mock<ICosmosDBRepository<Ticket>>(MockBehavior.Strict);
            
            var locationService = new LocationService(Options.Create(LocationSettings));
            var ticketService = new TicketService(cosmosDBRepositoryMock.Object, locationService);

            var ticketsContainer = new List<Ticket>();

            cosmosDBRepositoryMock.Setup(x => x.CreateItemAsync(It.IsAny<Ticket>()))
                .Returns<Ticket>(ticket => {
                    var doc = new Document();
                    var castTicket = JsonConvert.SerializeObject(ticket);

                    doc.LoadFrom(new JsonTextReader(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(castTicket)))));

                    ticketsContainer.Add(doc.ToObject<Ticket>());
                    return Task.FromResult(doc);
                });

            cosmosDBRepositoryMock.Setup(x => x.GetItemsAsync(It.IsAny<Expression<Func<Ticket, bool>>>()))
                .Returns((Expression<Func<Ticket, bool>> predicate) => {
                    return Task.FromResult(
                        ticketsContainer
                            .AsQueryable()
                            .Where(predicate)
                            .ToList()
                            .Cast<Ticket>());
                });

            var controller = new TicketsController(locationService, ticketService);

            var licensePlate = "123 456";
            var result = await controller.Post(licensePlate);

            var okResult = result.Should().BeOfType<Ticket>().Subject;

            ticketsContainer.Should().HaveCount(1);

            await controller.Post(licensePlate);

            ticketsContainer.Should().HaveCount(1);
        }

        [Fact]
        public async Task Ticket_Controller_Post_With_Different_Vehicle_Should_Create_New_Ticket() {

            var cosmosDBRepositoryMock = new Mock<ICosmosDBRepository<Ticket>>(MockBehavior.Strict);

            var locationService = new LocationService(Options.Create(LocationSettings));
            var ticketService = new TicketService(cosmosDBRepositoryMock.Object, locationService);

            var ticketsContainer = new List<Ticket>();

            cosmosDBRepositoryMock.Setup(x => x.CreateItemAsync(It.IsAny<Ticket>()))
                .Returns<Ticket>(ticket => {
                    var doc = new Document();
                    var castTicket = JsonConvert.SerializeObject(ticket);

                    doc.LoadFrom(new JsonTextReader(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(castTicket)))));

                    ticketsContainer.Add(doc.ToObject<Ticket>());
                    return Task.FromResult(doc);
                });

            cosmosDBRepositoryMock.Setup(x => x.GetItemsAsync(It.IsAny<Expression<Func<Ticket, bool>>>()))
                .Returns((Expression<Func<Ticket, bool>> predicate) => {
                    return Task.FromResult(
                        ticketsContainer
                            .AsQueryable()
                            .Where(predicate)
                            .ToList()
                            .Cast<Ticket>());
                });

            var controller = new TicketsController(locationService, ticketService);

            var licensePlate = "123 456";
            var result = await controller.Post(licensePlate);

            var okResult = result.Should().BeOfType<Ticket>().Subject;

            ticketsContainer.Should().HaveCount(1);

            licensePlate = "654 321";
            await controller.Post(licensePlate);

            ticketsContainer.Should().HaveCount(2);
        }

        [Fact]
        public async Task Ticket_Controller_Full_Lot_Should_Not_Create_New_Ticket() {

            var cosmosDBRepositoryMock = new Mock<ICosmosDBRepository<Ticket>>(MockBehavior.Strict);

            var locationSettings = new LocationSettings() {
                BaseRate = "3.00",
                Occupancy = "0",
                RateDurations = "60,180,360,1440",
                RateIncreasePercent = "1.50",
            };

            var locationService = new LocationService(Options.Create(locationSettings));
            var ticketService = new TicketService(cosmosDBRepositoryMock.Object, locationService);

            var ticketsContainer = new List<Ticket>();

            cosmosDBRepositoryMock.Setup(x => x.GetItemsAsync(It.IsAny<Expression<Func<Ticket, bool>>>()))
                .Returns((Expression<Func<Ticket, bool>> predicate) => {
                    return Task.FromResult(
                        ticketsContainer
                            .AsQueryable()
                            .Where(predicate)
                            .ToList()
                            .Cast<Ticket>());
                });

            var controller = new TicketsController(locationService, ticketService);

            var licensePlate = "123 456";
            var result = await controller.Post(licensePlate);

            ticketsContainer.Should().HaveCount(0);
            cosmosDBRepositoryMock.Verify(x => x.CreateItemAsync(It.IsAny<Ticket>()), Times.Never);
        }
        
        private static string GetFilePath(string fileName) {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"../../../{fileName}.json");
        }
    }
}
