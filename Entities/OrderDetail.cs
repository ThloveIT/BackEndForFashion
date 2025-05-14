namespace BackEndForFashion.Entities
{
    //chi tiet don hang(cac san pham trong don hang)
    public class OrderDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        //so luong san pham trong don hang
        public int Quantity { get; set; }
        // gia don vi cua san pham
        public decimal UnitPrice { get; set; }
    }
}
