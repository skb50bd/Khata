using System.Linq;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Driver;

using static System.Console;

namespace ImportData.Services
{
    public class Import
    {
        protected readonly IMongoDatabase _mongo;
        protected readonly ILogger<Import> _logger;
        protected readonly IUnitOfWork _db;
        public Import(
            IUnitOfWork db,
            ILogger<Import> logger)
        {
            _mongo = new MongoClient().GetDatabase("BShopManDb");
            _db = db;
            _logger = logger;
        }
        protected static string RemoveBsonNull(string str) => str == "BsonNull" ? "" : str;

        public async Task Info()
        {
            var customers = _mongo.GetCollection<BsonDocument>("Customer")
                                .Find(new BsonDocument()).ToList();
            long count = customers.Count();
            WriteLine($"{count} Importable Customers Found");
            WriteLine($"{ await _db.Customers.Count() } Customers exist on current database");

            WriteLine($"Remove Existing Customers? (Y/n)");
            var ans = ReadLine();
            if (ans.ToLowerInvariant() != "n")
            {
                foreach (var item in await _db.Customers.GetAll())
                    await _db.Customers.Delete(item.Id);
                await _db.CompleteAsync();
            }
            WriteLine($"Are your sure to import all {customers.Count} items? (Y/n)");
            ans = ReadLine();
            if (ans.ToLowerInvariant() != "n")
            {
                foreach (var item in customers)
                {
                    Customer convert(BsonDocument d)
                    {
                        var nameTokens = d["fullName"].ToString().Split();
                        var firstName = nameTokens.First();
                        var lastName = string.Join(' ', nameTokens.TakeLast(nameTokens.Count() - 1));
                        var email = d["emailAddress"].ToString();
                        var phone = d["phone"].ToString();
                        var address = d["companyName"].ToString();
                        var companyName = d["address"].ToString();
                        var note = d["note"].ToString();
                        return new Customer
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            Address = RemoveBsonNull(address),
                            CompanyName = RemoveBsonNull(companyName),
                            Phone = RemoveBsonNull(phone),
                            Email = RemoveBsonNull(email),
                            IsRemoved = !d["isActive"].ToBoolean(),
                            Note = RemoveBsonNull(note),
                            Debt = d["debt"].ToDecimal(),
                            Metadata = Metadata.CreatedNew("admin")

                        };
                    }
                    var newItem = convert(item);
                    _db.Customers.Add(newItem);
                }
            }
            await _db.CompleteAsync();

            var products = _mongo.GetCollection<BsonDocument>("Product")
                                .Find(new BsonDocument()).ToList();
            count = customers.Count();
            WriteLine($"{count} Importable Products Found");
            WriteLine($"{ await _db.Products.Count() } Products exist on current database");

            WriteLine($"Remove Existing Products? (Y/n)");
            ans = ReadLine();
            if (ans.ToLowerInvariant() != "n")
            {
                foreach (var item in await _db.Products.GetAll())
                    await _db.Products.Delete(item.Id);
                await _db.CompleteAsync();
            }
            WriteLine($"Are your sure to import all {products.Count} items? (Y/n)");
            ans = ReadLine();
            if (ans.ToLowerInvariant() != "n")
            {
                foreach (var item in products)
                {
                    Product convert(BsonDocument d)
                    {
                        var name = d["productName"].ToString();
                        var unit = d["units"][0]["unitName"].ToString();
                        var shopStock = d["shopStock"].ToDecimal();
                        var warehouseStock = d["godownStock"].ToDecimal();
                        var alertStock = d["alertStock"].ToDecimal();
                        var purchasePrice = d["purchasePrice"].ToDecimal();
                        var retailPrice = d["retailPrice"].ToDecimal();
                        var bulkPrice = d["wholeSalePrice"].ToDecimal();
                        var specification = d["specification"].ToString();
                        var category = d["category"].ToString();
                        var manufacturer = d["manufacturer"].ToString();
                        var note = d["notes"].ToString();
                        return new Product
                        {
                            Name = RemoveBsonNull(name),
                            Unit = RemoveBsonNull(unit),
                            Inventory = new Inventory
                            {
                                Stock = shopStock,
                                Warehouse = warehouseStock,
                                AlertAt = alertStock
                            },
                            Price = new Pricing
                            {
                                Bulk = bulkPrice,
                                Retail = retailPrice,
                                Purchase = purchasePrice,
                                Margin = purchasePrice * 1.1M
                            },
                            IsRemoved = false,
                            Description = RemoveBsonNull(note)
                                    + $"\nManufacturer: {manufacturer}"
                                    + $"\nCategory: {category}"
                                    + $"\nSpecification: {specification}",
                            Metadata = Metadata.CreatedNew("admin")

                        };
                    }
                    var newItem = convert(item);
                    _db.Products.Add(newItem);
                }
            }
            await _db.CompleteAsync();
        }
    }
}
