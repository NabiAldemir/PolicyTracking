namespace PolicyTracking.ViewModels
{
    public class CustomerWithAddress
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List <CustomerAdressDto> Addresses { get; set; }
    }
    public class CustomerAdressDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neighbourhood { get; set; }
        public string Street { get; set; }
        public string DoorNumber { get; set; }
        public string PostalCode { get; set; }
    }
}
