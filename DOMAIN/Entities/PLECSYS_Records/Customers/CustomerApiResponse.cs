namespace DOMAIN.Entities.PLECSYS_Records.Customers
{
    public class CustomerApiResponse
    {
        public List<Customer>? Data { get; set; }

        public bool Success { get; set; }

        public required string Message { get; set; }
    }

    public class Customer
    {
        public int Company_id { get; set; }

        public int CompanyXcliente_id { get; set; }

        public required string Company_legal_name { get; set; }
    }
}
