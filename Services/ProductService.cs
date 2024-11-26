using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using THAMCOMVC.ViewModels;


namespace THAMCOMVC.Services;

public class ProductService{

    public static List<ProductViewModel> GetProducts(){
        return new List<ProductViewModel>
    {
        new ProductViewModel
        {
            Id = 1,
            ProductName = "Laptop",
            ImagePath = "https://unsplash.com/photos/8IKf54pc3qk",
            ProductDescription = "A high-performance laptop for all your computing needs."
        },
        new ProductViewModel
        {
            Id = 2,
            ProductName = "Smartphone",
            ImagePath = "https://unsplash.com/photos/_-JR5TxKNSo",
            ProductDescription = "A sleek smartphone with the latest features."
        },
        new ProductViewModel
        {
            Id = 3,
            ProductName = "Headphones",
            ImagePath = "https://unsplash.com/photos/pElSkGRA2NU",
            ProductDescription = "Noise-cancelling headphones for an immersive experience."
        },
        new ProductViewModel
        {
            Id = 4,
            ProductName = "Shoes",
            ImagePath = "https://unsplash.com/photos/1b8L1fG2AgE",
            ProductDescription = "Comfortable and stylish shoes for any occasion."
        },
        new ProductViewModel
        {
            Id = 5,
            ProductName = "Handbag",
            ImagePath = "https://unsplash.com/photos/MxuQfD8zjtQ",
            ProductDescription = "A trendy handbag to complete your look."
        },
        new ProductViewModel
        {
            Id = 6,
            ProductName = "Watch",
            ImagePath = "https://unsplash.com/photos/Q0-fOL2nqZc",
            ProductDescription = "A classic watch to keep you punctual and stylish."
        },
        new ProductViewModel
        {
            Id = 7,
            ProductName = "Couch",
            ImagePath = "https://pixabay.com/photos/couch-living-room-furniture-1835923/",
            ProductDescription = "A comfortable couch to elevate your living room."
        },
        new ProductViewModel
        {
            Id = 8,
            ProductName = "Lamp",
            ImagePath = "https://pixabay.com/photos/lighting-bedroom-bedside-1867764/",
            ProductDescription = "A modern lamp to brighten your space."
        },
        new ProductViewModel
        {
            Id = 9,
            ProductName = "Bookshelf",
            ImagePath = "https://pixabay.com/photos/books-library-bookstore-1281581/",
            ProductDescription = "A sturdy bookshelf for your collection."
        },
        new ProductViewModel { Id = 10, ProductName = "Noise Cancelling Headphones", ProductDescription = "High-quality headphones with active noise cancellation.", ImagePath = "https://via.placeholder.com/200x200?text=Headphones" },
        new ProductViewModel { Id = 11, ProductName = "Smartphone", ProductDescription = "Latest generation smartphone with OLED display.", ImagePath = "https://via.placeholder.com/200x200?text=Smartphone" },
        new ProductViewModel { Id = 12, ProductName = "Gaming Laptop", ProductDescription = "Powerful laptop for gaming and productivity.", ImagePath = "https://via.placeholder.com/200x200?text=Gaming+Laptop" },
         new ProductViewModel { Id = 13, ProductName = "Fitness Tracker", ProductDescription = "Track your fitness activities and monitor your health.", ImagePath = "https://via.placeholder.com/200x200?text=Fitness+Tracker" },
        new ProductViewModel { Id = 14, ProductName = "Bluetooth Speaker", ProductDescription = "Portable speaker with rich sound quality.", ImagePath = "https://via.placeholder.com/200x200?text=Bluetooth+Speaker" },
        new ProductViewModel { Id = 15, ProductName = "4K TV", ProductDescription = "Ultra HD TV with stunning picture clarity.", ImagePath = "https://via.placeholder.com/200x200?text=4K+TV" },
        new ProductViewModel { Id = 16, ProductName = "Wireless Mouse", ProductDescription = "Ergonomic wireless mouse with fast connectivity.", ImagePath = "https://via.placeholder.com/200x200?text=Wireless+Mouse" },
        new ProductViewModel { Id = 17, ProductName = "Mechanical Keyboard", ProductDescription = "Durable keyboard with tactile switches.", ImagePath = "https://via.placeholder.com/200x200?text=Keyboard" },
        new ProductViewModel { Id = 18, ProductName = "Camera", ProductDescription = "High-definition camera for photography enthusiasts.", ImagePath = "https://via.placeholder.com/200x200?text=Camera" },
        new ProductViewModel { Id = 19, ProductName = "Smartwatch", ProductDescription = "Smartwatch with advanced health monitoring.", ImagePath = "https://via.placeholder.com/200x200?text=Smartwatch" },
        new ProductViewModel { Id = 20, ProductName = "VR Headset", ProductDescription = "Virtual reality headset for immersive experiences.", ImagePath = "https://via.placeholder.com/200x200?text=VR+Headset" },
        new ProductViewModel { Id = 21, ProductName = "Action Camera", ProductDescription = "Waterproof action camera for adventure filming.", ImagePath = "https://via.placeholder.com/200x200?text=Action+Camera" },
        new ProductViewModel { Id = 22, ProductName = "Electric Kettle", ProductDescription = "Fast-boiling electric kettle with auto shut-off.", ImagePath = "https://via.placeholder.com/200x200?text=Electric+Kettle" },
        new ProductViewModel { Id = 23, ProductName = "Backpack", ProductDescription = "Stylish and spacious backpack for travel and work.", ImagePath = "https://via.placeholder.com/200x200?text=Backpack" },
        new ProductViewModel { Id = 24, ProductName = "Smart Light Bulbs", ProductDescription = "Wi-Fi-enabled light bulbs for home automation.", ImagePath = "https://via.placeholder.com/200x200?text=Smart+Bulbs" },

            };
    }
}