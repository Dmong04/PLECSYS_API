namespace APPLICATION.Use_cases.SaleOrderDetails_case
{
    public class SaleOrderDetailsRequest
    {
        public required int Product_id { get; set; }
        public required int Quantity { get; set; }
    }
}
