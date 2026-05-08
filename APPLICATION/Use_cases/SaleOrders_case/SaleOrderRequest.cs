using APPLICATION.Use_cases.SaleOrderDetails_case;
using DOMAIN.Entities.GPS;

namespace APPLICATION.Use_cases.SaleOrders_case
{
    public class SaleOrdersRequest
    {
        public required int Supplier_id { get; set; }

        public required string User_email { get; set; }

        public required int Company_id { get; set; }

        public required GPSNode GPS { get; set; }

        public required List<SaleOrderDetailsRequest> SaleOrders { get; set; }
    }
}
