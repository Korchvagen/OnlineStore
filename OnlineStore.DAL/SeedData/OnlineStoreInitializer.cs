using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace OnlineStore.DAL.SeedData
{
    public class OnlineStoreInitializer
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new OnlineStoreContext(serviceProvider.GetRequiredService<DbContextOptions<OnlineStoreContext>>()))
            {
                if (!context.Accounts.Any())
                {
                    await context.Accounts.AddAsync(
                        new Accounts
                        {
                            Role = Enum.UserRole.Admin,
                            Login = "Korchvagen007@gmail.com",
                            Password = "ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f",
                            Status = Enum.AccountStatus.Activated
                        }
                    );
                    await context.SaveChangesAsync();

                    await context.Cart.AddAsync(
                        new Cart
                        {
                            Account = context.Accounts.ToArray()[0]
                        }
                    );
                    await context.SaveChangesAsync();
                }

                var directory = Directory.GetCurrentDirectory();

                if (!context.Banner.Any())
                {
                    await context.Banner.AddRangeAsync(
                        new Banner
                        {
                            Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Banners\\banner-1.jpg"),
                            FileName = "banner-1.png",
                            Link = "/Products/ProductPage/3"
                        },
                        new Banner
                        {
                            Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Banners\\banner-2.jpg"),
                            FileName = "banner-2.png",
                            Link = "/Products/ProductPage/1"
                        },
                        new Banner
                        {
                            Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Banners\\banner-3.jpg"),
                            FileName = "banner-3.png",
                            Link = "/Products/Index"
                        },
                        new Banner
                        {
                            Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Banners\\banner-4.jpg"),
                            FileName = "banner-4.png",
                            Link = "/Products/ProductPage/7"
                        },
                        new Banner
                        {
                            Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Banners\\banner-5.jpg"),
                            FileName = "banner-5.png",
                            Link = "/Products/ProductPage/8"
                        }
                    );
                    await context.SaveChangesAsync();
                }

                if (context.Product.Any())
                {
                    return;
                }

                await context.Product.AddRangeAsync(
                    new Product
                    {
                        Name = "iPhone 14 Pro Max",
                        Category = Enum.ProductCategory.Smartphone,
                        Price = 3900,
                        Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Products\\1.png"),
                        FileName = "1.png"
                    },
                    new Product
                    {
                        Name = "iPhone 14 Pro Max",
                        Category = Enum.ProductCategory.Smartphone,
                        Price = 3900,
                        Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Products\\2.png"),
                        FileName = "2.png"
                    },
                    new Product
                    {
                        Name = "iPhone 14",
                        Category = Enum.ProductCategory.Smartphone,
                        Price = 2500,
                        Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Products\\3.png"),
                        FileName = "3.png"
                    },
                    new Product
                    {
                        Name = "iPhone 14 Pro",
                        Category = Enum.ProductCategory.Smartphone,
                        Price = 3800,
                        Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Products\\4.png"),
                        FileName = "4.png"
                    },
                    new Product
                    {
                        Name = "iPhone 13 Pro Max",
                        Category = Enum.ProductCategory.Smartphone,
                        Price = 3650,
                        Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Products\\5.png"),
                        FileName = "5.png"
                    },
                    new Product
                    {
                        Name = "iPhone 13 Pro",
                        Category = Enum.ProductCategory.Smartphone,
                        Price = 2900,
                        Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Products\\6.png"),
                        FileName = "6.png"
                    },
                    new Product
                    {
                        Name = "MacBook Pro",
                        Category = Enum.ProductCategory.Laptop,
                        Price = 8999,
                        Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Products\\7.png"),
                        FileName = "7.png"
                    },
                    new Product
                    {
                        Name = "Apple iMac",
                        Category = Enum.ProductCategory.Monoblock,
                        Price = 6800,
                        Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Products\\8.png"),
                        FileName = "8.png"
                    },
                    new Product
                    {
                        Name = "Lenovo V14",
                        Category = Enum.ProductCategory.Laptop,
                        Price = 5500,
                        Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Products\\9.png"),
                        FileName = "9.png"
                    },
                    new Product
                    {
                        Name = "Lenovo Ideacentre 5i",
                        Category = Enum.ProductCategory.Monoblock,
                        Price = 4199,
                        Image = ConvertImage(directory + ".DAL\\SeedData\\SeedImages\\Products\\10.png"),
                        FileName = "10.png"
                    }
                );
                await context.SaveChangesAsync();

                await context.ProductInfo.AddRangeAsync(
                    new ProductInfo
                    {
                        Amount = 10,
                        CreationDate = DateTime.Parse("2022-9-10"),
                        LifeTime = 5,
                        Material = "Metal and Glass",
                        Color = "Deep Purple",
                        Memory = "128 GB",
                        Rating = 8.8,
                        Product = context.Product.ToArray()[0]
                    },
                    new ProductInfo
                    {
                        Amount = 12,
                        CreationDate = DateTime.Parse("2022-9-15"),
                        LifeTime = 5,
                        Material = "Metal and Glass",
                        Color = "Gold",
                        Memory = "256 GB",
                        Rating = 8.1,
                        Product = context.Product.ToArray()[1]
                    },
                    new ProductInfo
                    {
                        Amount = 15,
                        CreationDate = DateTime.Parse("2022-9-06"),
                        LifeTime = 5,
                        Material = "Metal and Glass",
                        Color = "Alpine Green",
                        Memory = "128 GB",
                        Rating = 7.4,
                        Product = context.Product.ToArray()[2]
                    },
                    new ProductInfo
                    {
                        Amount = 8,
                        CreationDate = DateTime.Parse("2022-9-09"),
                        LifeTime = 5,
                        Material = "Metal and Glass",
                        Color = "Space Black",
                        Memory = "1 TB",
                        Rating = 7.9,
                        Product = context.Product.ToArray()[3]
                    },
                    new ProductInfo
                    {
                        Amount = 25,
                        CreationDate = DateTime.Parse("2021-10-25"),
                        LifeTime = 5,
                        Material = "Metal and Glass",
                        Color = "Blue Deals",
                        Memory = "512 GB",
                        Rating = 8.2,
                        Product = context.Product.ToArray()[4]
                    },
                    new ProductInfo
                    {
                        Amount = 22,
                        CreationDate = DateTime.Parse("2021-9-21"),
                        LifeTime = 5,
                        Material = "Metal and Glass",
                        Color = "Silver",
                        Memory = "512 GB",
                        Rating = 7.2,
                        Product = context.Product.ToArray()[5]
                    },
                    new ProductInfo
                    {
                        Amount = 6,
                        CreationDate = DateTime.Parse("2021-12-04"),
                        LifeTime = 6,
                        Material = "Aluminum",
                        Color = "Space Gray",
                        Memory = "1 TB",
                        Rating = 8.4,
                        Product = context.Product.ToArray()[6]
                    },
                    new ProductInfo
                    {
                        Amount = 4,
                        CreationDate = DateTime.Parse("2020-5-02"),
                        LifeTime = 6,
                        Material = "Metal",
                        Color = "Silver",
                        Memory = "2 TB",
                        Rating = 6.8,
                        Product = context.Product.ToArray()[7]
                    },
                    new ProductInfo
                    {
                        Amount = 7,
                        CreationDate = DateTime.Parse("2022-6-29"),
                        LifeTime = 7,
                        Material = "Plastic",
                        Color = "Black",
                        Memory = "2 TB",
                        Rating = 7.7,
                        Product = context.Product.ToArray()[8]
                    },
                    new ProductInfo
                    {
                        Amount = 3,
                        CreationDate = DateTime.Parse("2022-4-11"),
                        LifeTime = 7,
                        Material = "Metal",
                        Color = "Black",
                        Memory = "1 TB",
                        Rating = 6.5,
                        Product = context.Product.ToArray()[9]
                    }
                );
                await context.SaveChangesAsync();
            }
        }

        private static byte[] ConvertImage(string url)
        {
            Image image = Image.FromFile(url);
            MemoryStream memoryStream = new MemoryStream();
            var extension = url.Split('.').Last();

            if (extension == "png")
            {
                image.Save(memoryStream, ImageFormat.Png);
            }
            else
            {
                image.Save(memoryStream, ImageFormat.Jpeg);
            }

            byte[] b = memoryStream.ToArray();

            return b;
        }
    }
}
