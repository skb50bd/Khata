using System;

using Domain;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Product>(p =>
            {
                p.HasData(
                    new Product
                    {
                        Id = 1,
                        Name = "Nokia 1100",
                        Description = "Great Budget Phone"
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Moto 360",
                        Description = "Killer Phone"
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Nokia X6",
                        Description = "Show you what we can do."
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Moto Z Play",
                        Description = "Awesome Battery Life"
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Moto X Play",
                        Description = "Thought you'd never have something like it?"
                    },
                    new Product
                    {
                        Id = 6,
                        Name = "Nokia 3310",
                        Description = "You're gonna miss it when its gone."
                    },
                    new Product
                    {
                        Id = 7,
                        Name = "Samsung Galaxy S9+",
                        Description = "Feel the power in you palm."
                    },
                    new Product
                    {
                        Id = 8,
                        Name = "OnePlus 6T",
                        Description = "Cool, innit?"
                    },
                    new Product
                    {
                        Id = 9,
                        Name = "OnePlus 3T",
                        Description = "Cause OnePlus 3S would be dumb."
                    },
                    new Product
                    {
                        Id = 10,
                        Name = "OnePlus 5",
                        Description = "Ya didnot expect that, did ya?"
                    },
                    new Product
                    {
                        Id = 11,
                        Name = "Moto Z3 Force",
                        Description = "May the force be with you."
                    },
                    new Product
                    {
                        Id = 12,
                        Name = "Okapia Life",
                        Description = "Feel the Life"
                    },
                    new Product
                    {
                        Id = 13,
                        Name = "Sony Xperia Z",
                        Description = "You know it's good, cause it's waterproof!"
                    },
                    new Product
                    {
                        Id = 14,
                        Name = "Sony Xperia Z2",
                        Description = "When being water resistant is not enough ;)"
                    },
                    new Product
                    {
                        Id = 15,
                        Name = "Sony Xperia Z5 Premium",
                        Description = "Hello! Is it me you're looking for?"
                    },
                    new Product
                    {
                        Id = 16,
                        Name = "Sony Xperia Sola",
                        Description = "Innovation at your fingertips."
                    },
                    new Product
                    {
                        Id = 17,
                        Name = "Sony Xperia Ray",
                        Description = "Keep the handy qwerty pad!"
                    },
                    new Product
                    {
                        Id = 18,
                        Name = "Google Pixel",
                        Description = "Made by Google!"
                    },
                    new Product
                    {
                        Id = 19,
                        Name = "Google Pixel 2",
                        Description = "Convinced by the camera!"
                    },
                    new Product
                    {
                        Id = 20,
                        Name = "Google Pixel 3",
                        Description = "Unstoppable growth!"
                    },
                    new Product
                    {
                        Id = 21,
                        Name = "Apple iPhone X",
                        Description = "It's apple!"
                    },
                    new Product
                    {
                        Id = 22,
                        Name = "Apple iPhone X Max",
                        Description = "Cause we need more money!"
                    }
                );
                p.OwnsOne(pp => pp.Price).HasData(new
                {
                    ProductId = 1,
                    Retail = 2000M,
                    Bulk = 1850M,
                    Margin = 1600M,
                    Purchase = 1500M
                },
                    new
                    {
                        ProductId = 2,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                new
                {
                    ProductId = 3,
                    Retail = 2000M,
                    Bulk = 1850M,
                    Margin = 1600M,
                    Purchase = 1500M
                },
                    new
                    {
                        ProductId = 4,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 5,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 6,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 7,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 8,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 9,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 10,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 11,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 12,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 13,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 14,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 15,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    }, new
                    {
                        ProductId = 16,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 17,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 18,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    }, new
                    {
                        ProductId = 19,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    }, new
                    {
                        ProductId = 20,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 21,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    },
                    new
                    {
                        ProductId = 22,
                        Retail = 2000M,
                        Bulk = 1850M,
                        Margin = 1600M,
                        Purchase = 1500M
                    });

                p.OwnsOne(pp => pp.Inventory).HasData(new
                {
                    ProductId = 1,
                    Stock = 5M,
                    Warehouse = 0M,
                    AlertAt = 10M
                },
                    new
                    {
                        ProductId = 2,
                        Stock = 15M,
                        Warehouse = 10M,
                        AlertAt = 10M
                    },
                    new
                    {
                        ProductId = 3,
                        Stock = 55M,
                        Warehouse = 550M,
                        AlertAt = 10000M
                    },
                    new
                    {
                        ProductId = 4,
                        Stock = 0M,
                        Warehouse = 0M,
                        AlertAt = 10M
                    },
                    new
                    {
                        ProductId = 5,
                        Stock = 18M,
                        Warehouse = 40M,
                        AlertAt = 10M
                    },
                    new
                    {
                        ProductId = 6,
                        Stock = 0M,
                        Warehouse = 5M,
                        AlertAt = 5M
                    },
                    new
                    {
                        ProductId = 7,
                        Stock = 15M,
                        Warehouse = 550M,
                        AlertAt = 1000M
                    },
                    new
                    {
                        ProductId = 8,
                        Stock = 20M,
                        Warehouse = 5M,
                        AlertAt = 18M
                    },
                    new
                    {
                        ProductId = 9,
                        Stock = 3M,
                        Warehouse = 3M,
                        AlertAt = 3M
                    },
                    new
                    {
                        ProductId = 10,
                        Stock = 6M,
                        Warehouse = 7M,
                        AlertAt = 80M
                    },
                    new
                    {
                        ProductId = 11,
                        Stock = 6M,
                        Warehouse = 0M,
                        AlertAt = 100M
                    }, new
                    {
                        ProductId = 12,
                        Stock = 0M,
                        Warehouse = 0M,
                        AlertAt = 100M
                    }, new
                    {
                        ProductId = 13,
                        Stock = 6M,
                        Warehouse = 0M,
                        AlertAt = 10M
                    },
                    new
                    {
                        ProductId = 14,
                        Stock = 5M,
                        Warehouse = 0M,
                        AlertAt = 10M
                    },
                    new
                    {
                        ProductId = 15,
                        Stock = 5M,
                        Warehouse = 20M,
                        AlertAt = 10M
                    }, new
                    {
                        ProductId = 16,
                        Stock = 16M,
                        Warehouse = 16M,
                        AlertAt = 9M
                    },
                    new
                    {
                        ProductId = 17,
                        Stock = 51M,
                        Warehouse = 1011M,
                        AlertAt = 101M
                    },
                    new
                    {
                        ProductId = 18,
                        Stock = 51M,
                        Warehouse = 50M,
                        AlertAt = 100M
                    },
                    new
                    {
                        ProductId = 19,
                        Stock = 61M,
                        Warehouse = 11M,
                        AlertAt = 80M
                    }, new
                    {
                        ProductId = 20,
                        Stock = 15M,
                        Warehouse = 10M,
                        AlertAt = 30M
                    },
                    new
                    {
                        ProductId = 21,
                        Stock = 50M,
                        Warehouse = 00M,
                        AlertAt = 100M
                    }, new
                    {
                        ProductId = 22,
                        Stock = 3M,
                        Warehouse = 3M,
                        AlertAt = 10M
                    });

                p.OwnsOne(pp => pp.Metadata).HasData(new
                {
                    Id = 1,
                    ProductId = 1,
                    Creator = "admin",
                    Modifier = "admin",
                    CreationTime = DateTimeOffset.Now,
                    ModificationTime = DateTimeOffset.Now
                },
                    new
                    {
                        Id = 2,
                        ProductId = 2,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 3,
                        ProductId = 3,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 4,
                        ProductId = 4,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 5,
                        ProductId = 5,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 6,
                        ProductId = 6,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 7,
                        ProductId = 7,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 8,
                        ProductId = 8,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 9,
                        ProductId = 9,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 10,
                        ProductId = 10,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    }, new
                    {
                        Id = 11,
                        ProductId = 11,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 12,
                        ProductId = 12,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    }, new
                    {
                        Id = 13,
                        ProductId = 13,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    }, new
                    {
                        Id = 14,
                        ProductId = 14,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 15,
                        ProductId = 15,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 16,
                        ProductId = 16,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 17,
                        ProductId = 17,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 18,
                        ProductId = 18,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 19,
                        ProductId = 19,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 20,
                        ProductId = 20,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    },
                    new
                    {
                        Id = 21,
                        ProductId = 21,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    }, new
                    {
                        Id = 22,
                        ProductId = 22,
                        Creator = "admin",
                        Modifier = "admin",
                        CreationTime = DateTimeOffset.Now,
                        ModificationTime = DateTimeOffset.Now
                    });
            });
        }
    }
}