using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class OrderDTO
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShipToAddress { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string? PictureUrl { get; set; }
    }
}
