using FluentAssertions;
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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParkingGarage.Tests {
    public class PaymentsControllerUnitTests {

        private LocationSettings LocationSettings
            => new LocationSettings() {
                BaseRate = "3.00",
                Occupancy = "50",
                RateDurations = "60,180,360,1440",
                RateIncreasePercent = "1.50",
            };

        [Fact]
        public async Task Payments_Controller_Should_Be_Able_To_Resolve_Ticket() {

            var cosmosDBRepositoryMock = new Mock<ICosmosDBRepository<Ticket>>(MockBehavior.Strict);

            var locationService = new LocationService(Options.Create(LocationSettings));
            var ticketService = new TicketService(cosmosDBRepositoryMock.Object, locationService);
            var paymentsService = new PaymentService(ticketService);

            var ticketsContainer = new List<Ticket>();

            cosmosDBRepositoryMock.Setup(x => x.CreateItemAsync(It.IsAny<Ticket>()))
                .Returns<Ticket>(ticketResult => {
                    var doc = new Document();
                    var castTicket = JsonConvert.SerializeObject(ticketResult);

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

            cosmosDBRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<string>()))
                .Returns<string>(id => {
                    return Task.FromResult(
                        ticketsContainer
                            .Where(t => t.Id == id)
                            .FirstOrDefault()
                        );
                });

            cosmosDBRepositoryMock.Setup(x => x.UpdateItemAsync(It.IsAny<string>(), It.IsAny<Ticket>()))
                .Returns<string, Ticket>((id, t) => {
                    var doc = new Document();
                    var castTicket = JsonConvert.SerializeObject(t);

                    doc.LoadFrom(new JsonTextReader(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(castTicket)))));

                    var ticketToUpdate = ticketsContainer.Where(x => x.Id == id).FirstOrDefault();
                    if (ticketToUpdate != null) {
                        ticketToUpdate = t;
                    }
                    return Task.FromResult(doc);
                });

            // Todo: mock instead of calling controller directly
            var ticketController = new TicketsController(locationService, ticketService);

            var licensePlate = "123 456";
            var ticket = await ticketController.Post(licensePlate);

            var controller = new TicketPaymentsController(paymentsService, ticketService);

            var cardNumber = "1234 5678 0000 0000";
            var result = await controller.Post(ticket.Id, cardNumber);

            ticketsContainer.Should().HaveCount(1);

            result.Should().BeEquivalentTo($"Ticket {ticket.Id} has been paid with credit card {cardNumber} for amount $3.00") ;
        }

        //todo: test variable rates
    }
}
