using Catalog.Products.Events;

namespace Catalog.Products.Models
{
    public class Product: Aggregate<Guid>
    {
        public string Name { get; private set; } = default!;
        public List<string> Category {  get; private set; }= new List<string>();
        public string Description {  get; private set; } = default!; 
        public string ImageFile { get; private set; } = default!;
        public decimal Price {  get; private set; } 

        public static Product Create(Guid id,string name,List<string>category,string description,string imageFile,decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
            var product=new Product()
            {
                Id = id,
                Name = name,
                Category = category,
                Description = description,
                ImageFile = imageFile,
                Price = price

            };
            product.AddDomainEvent(new ProductCreatedEvent(product));
            return product;
        }
        public void Update(string name, List<string> category, string description, string imageFile, decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            this.Name = name;
            this.Category = category;
            this.Description = description;
            this.ImageFile = imageFile;
           
            if (this.Price != price)
            {
                this.Price = price;
                this.AddDomainEvent(new ProductPriceChangedEvent(this));

            }

        }

    }
}
