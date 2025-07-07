namespace Catalog.Data.Seed
{
    public static class InitialData
    {
        public static IEnumerable<Product> Products => new List<Product>
        {
        Product.Create(
            id:   Guid.NewGuid(),
            name: "Wireless Mouse",
            category:   new List<string> { "Electronics", "Accessories" },
            description: "Ergonomic wireless mouse with adjustable DPI settings.",
            imageFile: "mouse.jpg",
            price:29.99m
        ),
        Product.Create(
            id: Guid.NewGuid(),
            name: "Bluetooth Headphones",
            category: new List<string> { "Electronics", "Audio" },
            description: "Noise-canceling over-ear Bluetooth headphones with long battery life.",
            imageFile: "headphones.jpg",
            price: 89.99m
        ),
        Product.Create(
            id: Guid.NewGuid(),
            name: "Gaming Keyboard",
            category: new List<string> { "Electronics", "Gaming" },
            description: "Mechanical keyboard with RGB lighting and programmable keys.",
            imageFile: "keyboard.jpg",
            price: 59.99m
        ),
         Product.Create(
            id: Guid.NewGuid(),
            name: "4K Monitor",
            category: new List<string> { "Electronics", "Display" },
            description: "27-inch 4K UHD monitor with ultra-thin bezels.",
            imageFile: "monitor.jpg",
            price: 279.99m
        ),
         Product.Create(
            id: Guid.NewGuid(),
            name: "USB-C Hub",
            category: new List<string> { "Accessories", "Tech" },
            description: "Multi-port USB-C hub with HDMI, USB-A, and SD card reader.",
            imageFile: "hub.jpg",
            price: 39.99m
        )
    };
    }
}

