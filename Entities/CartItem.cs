namespace BackEndForFashion.Entities
{
    //Luu cac muc torng gio hang
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        //quan he trung gian n-n cua cart va product
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
