namespace DOMAIN.Entities.PLECSYS_Records.Items
{
    public class ItemApiResponse
    {
        public List<Item>? Data { get; set; }

        public bool Success { get; set; }

        public required string Message { get; set; }
    }

    public class Item
    {
        public int Item_id { get; set; }

        public required string InternalCode { get; set; }

        public decimal Price { get; set; }

        public required string Description { get; set; }
    }
}
