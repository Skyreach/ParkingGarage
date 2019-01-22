using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace ParkingGarage.Extensions
{
    public static class TicketExtensions
    {
        public static T ToObject<T>(this Document document) {
            return JsonConvert.DeserializeObject<T>(document.ToString());

        }
    }
}
