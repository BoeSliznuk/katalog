namespace katalog.Models
{
    public class Order
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public Cart Cart { get; set; }
    }
}
