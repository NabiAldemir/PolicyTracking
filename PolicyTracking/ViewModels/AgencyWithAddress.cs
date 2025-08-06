using EntityLayer.Concrete;

namespace PolicyTrackingWebUI.ViewModels
{
    public class AgencyWithAddress
    {
        public string AgencyName { get; set; }

        public Address? AddressValues { get; set; }
    }

    public class AddressValues
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Neighbourhood { get; set; }
        public string? Street { get; set; }
        public string? DoorNumber { get; set; }
        public string? PostalCode { get; set; }
    }
}
