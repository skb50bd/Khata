using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

using MongoDB.Bson;
using MongoDB.Driver;

using Newtonsoft.Json;

namespace ImportData.Services
{
    public static class Dump
    {
        static string[] Shops = new string[0];
        static Dictionary<string, Customer> Customers = new Dictionary<string, Customer>();
        static Dictionary<string, Supplier> Suppliers = new Dictionary<string, Supplier>();
        static Dictionary<string, Expense> Expenses = new Dictionary<string, Expense>();
        static Dictionary<string, Employee> Employees = new Dictionary<string, Employee>();
        static Dictionary<string, DebtPayment> DebtPayments = new Dictionary<string, DebtPayment>();
        static Dictionary<string, SupplierPayment> SupplierPayments = new Dictionary<string, SupplierPayment>();
        static Dictionary<string, Purchase> Purchases = new Dictionary<string, Purchase>();
        static Dictionary<string, Dictionary<string, Sale>> Sales = new Dictionary<string, Dictionary<string, Sale>>();
        static Dictionary<string, Dictionary<string, Product>> Products = new Dictionary<string, Dictionary<string, Product>>();
        static Dictionary<string, Product> AllProducts
        {
            get
            {
                var prods = new Dictionary<string, Product>();
                foreach (var key in Products.Keys)
                {
                    foreach ((var id, var prod) in Products[key])
                    {
                        prods[id] = prod;
                    }
                }
                return prods;
            }
        }
        static JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

        static IMongoDatabase Mongo = new MongoClient().GetDatabase("BShopManDb");
        static string workingDir = @"D:\Dump";

        public static void CreateDump()
        {

        }


        public static void AddItems<T>(IList<T> items, Action<T> add, IUnitOfWork db)
        {
            var count = items.Count();
            var failCount = 0;
            for (var i = 0; i < count; i++)
            {
                Console.WriteLine($"Adding { i + 1 } of { count }");
                add(items[i]);
                try
                {
                    //db.Complete();
                }
                catch (Exception e)
                {
                    failCount++;
                    Console.WriteLine(e.Message);
                    string json = JsonConvert.SerializeObject(items[i], Formatting.Indented, jss);
                    Console.WriteLine("Failed to Write the Object" + json);
                    System.IO.File.AppendAllText(workingDir + @"\" + typeof(T).Name + "Errors.json", json);
                }
            }

            Console.WriteLine($"Failed to add {failCount} items");
            Console.ReadLine();
        }

        public static async Task InsertAllAsync(IUnitOfWork db)
        {
            DiscoverShops();
            Console.Write($"{Shops.Count()} Shops found.\nWhich one to load? (0 - {Shops.Count() - 1})");
            var shop = Console.ReadLine();
            if (int.TryParse(shop, out int index) && index < Shops.Count())
            {
                var ans = "";

                DumpCustomers();
                Console.Write("Customers? (enter N to cancel): ");
                ans = Console.ReadLine();
                if (ans.ToUpperInvariant() != "N")
                {
                    AddItems(Customers.Values.ToList(), db.Customers.Add, db);
                }

                DumpSuppliers();
                Console.Write("Suppliers? (enter N to cancel): ");
                ans = Console.ReadLine();
                if (ans.ToUpperInvariant() != "N")
                {
                    AddItems(Suppliers.Values.ToList(), db.Suppliers.Add, db);
                }

                DumpEmployees();
                Console.Write("Employees? (enter N to cancel): ");
                ans = Console.ReadLine();
                if (ans.ToUpperInvariant() != "N")
                {
                    AddItems(Employees.Values.ToList(), db.Employees.Add, db);
                }

                DumpExpenses();
                Console.Write("Expenses? (enter N to cancel): ");
                ans = Console.ReadLine();
                if (ans.ToUpperInvariant() != "N")
                {
                    AddItems(Expenses.Values.ToList(), db.Expenses.Add, db);
                }

                db.Complete();
                var customer = new Customer
                {
                    FirstName = "Cash",
                    LastName = "Customer",
                    Metadata = Metadata.CreatedNew("auto")
                };

                var supplier = new Supplier
                {
                    FirstName = "Cash",
                    LastName = "Supplier",
                    Metadata = Metadata.CreatedNew("auto")
                };

                DumpDebtPayments();
                Console.Write("Debt Payments? (enter N to cancel): ");
                ans = Console.ReadLine();
                if (ans.ToUpperInvariant() != "N")
                {
                    foreach (var dp in DebtPayments.Values)
                    {
                        if (dp.Customer == null)
                            dp.Customer = customer;
                        if (dp.Invoice.Customer == null)
                            dp.Invoice.Customer = customer;
                    }
                    AddItems(DebtPayments.Values.ToList(), db.DebtPayments.Add, db);
                }

                DumpSupplierPayments();
                Console.Write("Supplier Payments? (enter N to cancel): ");
                ans = Console.ReadLine();
                if (ans.ToUpperInvariant() != "N")
                {
                    foreach (var sp in SupplierPayments.Values)
                    {
                        if (sp.Supplier == null)
                            sp.Supplier = supplier;
                        if (sp.Vouchar.Supplier == null)
                            sp.Vouchar.Supplier = supplier;
                    }
                    AddItems(SupplierPayments.Values.ToList(), db.SupplierPayments.Add, db);
                }

                DumpProducts();
                var shopId = Shops[index];
                Console.Write("Products? (enter N to cancel): ");
                ans = Console.ReadLine();
                if (ans.ToUpperInvariant() != "N")
                {
                    AddItems(AllProducts.Values.ToList(), db.Products.Add, db);
                }
                db.Complete();

                DumpPurchases();
                Console.Write("Purchase? (enter N to cancel): ");
                ans = Console.ReadLine();
                if (ans.ToUpperInvariant() != "N")
                {
                    foreach (var purchase in Purchases.Values)
                    {
                        Console.WriteLine($"SupplierId: {purchase.SupplierId}");
                        var supplierExists = await db.Suppliers.Exists(purchase.SupplierId);
                        Console.WriteLine($"Supplier Exists: {supplierExists}");
                        if (!supplierExists)
                        {
                            purchase.Supplier = supplier;
                            purchase.Vouchar.Supplier = supplier;
                        }
                        db.Purchases.Add(purchase);
                    }
                }
                db.Complete();

                DumpSales();
                Console.Write("Sales? (enter N to cancel): ");
                ans = Console.ReadLine();
                if (ans.ToUpperInvariant() != "N")
                {
                    var added = 0;
                    //AddItems(Sales[shopId].Values.ToList(), db.Sales.Add, db);
                    foreach (var sale in Sales[shopId].Values)
                    {
                        Console.WriteLine($"CustomerId: {sale.CustomerId}");
                        var customerExists = await db.Customers.Exists(sale.CustomerId);
                        Console.WriteLine($"Customer Exists: {customerExists}");
                        if (!customerExists)
                        {
                            sale.CustomerId = (await db.Customers.Get(
                                c => c.FullName.ToLowerInvariant().Contains("cash"),
                                c => c.Id, 1, 0)).First().Id;
                            sale.Invoice.CustomerId = sale.CustomerId;
                        }

                        db.Sales.Add(sale);
                        Console.WriteLine($"{added} customers added");
                    }
                }
                db.Complete();

                Console.WriteLine("Everything Added...");
            }
        }

        private static void DiscoverShops()
        {
            var shopsCollection = Mongo.GetCollection<BsonDocument>("Shop").Find(new BsonDocument()).ToList();
            Shops = shopsCollection.Select(s => s["_id"].ToString()).ToArray();
            Console.WriteLine($"{Shops.Count()} Shops Found");
            foreach (var shop in Shops)
                System.IO.Directory.CreateDirectory(workingDir + @"\" + shop);
        }

        private static void DumpCustomers()
        {
            var customersCollection = Mongo.GetCollection<BsonDocument>("Customer").Find(new BsonDocument()).ToList();
            foreach (var c in customersCollection)
            {
                var nameTokens = c["fullName"].ToString().Split();
                var firstName = nameTokens.First();
                var lastName = string.Join(' ', nameTokens.TakeLast(nameTokens.Count() - 1));
                var email = c["emailAddress"].ToString();
                var phone = c["phone"].ToString();
                var address = c["address"].ToString();
                var companyName = c["companyName"].ToString();
                var note = c["note"].ToString();
                Customers[c["_id"].ToString()] = new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Address = RemoveBsonNull(address),
                    CompanyName = RemoveBsonNull(companyName),
                    Phone = RemoveBsonNull(phone),
                    Email = RemoveBsonNull(email),
                    IsRemoved = !c["isActive"].ToBoolean(),
                    Note = RemoveBsonNull(note),
                    Debt = c["debt"].ToDecimal(),
                    Metadata = GetMetadata(c["meta"].AsBsonDocument)
                };
            }
            var json = JsonConvert.SerializeObject(Customers, Formatting.Indented);
            System.IO.File.WriteAllText(workingDir + @"\Customers.json", json);
        }

        private static void DumpSuppliers()
        {
            var customersCollection = Mongo.GetCollection<BsonDocument>("Supplier").Find(new BsonDocument()).ToList();
            foreach (var s in customersCollection)
            {
                var nameTokens = s["fullName"].ToString().Split();
                var firstName = nameTokens.First();
                var lastName = string.Join(' ', nameTokens.TakeLast(nameTokens.Count() - 1));
                var email = s["emailAddress"].ToString();
                var phone = s["phone"].ToString();
                var address = s["address"].ToString();
                var companyName = s["companyName"].ToString();
                var note = s["note"].ToString();
                Suppliers[s["_id"].ToString()] = new Supplier
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Address = RemoveBsonNull(address),
                    CompanyName = RemoveBsonNull(companyName),
                    Phone = RemoveBsonNull(phone),
                    Email = RemoveBsonNull(email),
                    IsRemoved = !s["isActive"].ToBoolean(),
                    Note = RemoveBsonNull(note),
                    Payable = s["payable"].ToDecimal(),
                    Metadata = GetMetadata(s["meta"].AsBsonDocument)
                };
            }
            var json = JsonConvert.SerializeObject(Suppliers, Formatting.Indented);
            System.IO.File.WriteAllText(workingDir + @"\Suppliers.json", json);
        }

        private static void DumpEmployees()
        {
            var employeesCollection = Mongo.GetCollection<BsonDocument>("Employee").Find(new BsonDocument()).ToList();
            foreach (var e in employeesCollection)
            {
                var nameTokens = e["fullName"].ToString().Split();
                var firstName = nameTokens.First();
                var lastName = string.Join(' ', nameTokens.TakeLast(nameTokens.Count() - 1));
                var email = e["emailAddress"].ToString();
                var phone = e["phone"].ToString();
                var address = e["address"].ToString();
                var designation = e["designation"].ToString();
                var balance = e["currentBalance"].ToDecimal();
                var nid = e["nationalIdN"].ToString();
                var salary = e["monthlySalary"].ToDecimal();
                var note = e["note"].ToString();
                Employees[e["_id"].ToString()] = new Employee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Address = RemoveBsonNull(address),
                    Phone = RemoveBsonNull(phone),
                    Email = RemoveBsonNull(email),
                    IsRemoved = !e["isActive"].ToBoolean(),
                    Note = RemoveBsonNull(note),
                    Balance = balance,
                    Designation = RemoveBsonNull(designation),
                    Salary = salary,
                    NIdNumber = RemoveBsonNull(nid),
                    Metadata = GetMetadata(e["meta"].AsBsonDocument)
                };
            }
            var json = JsonConvert.SerializeObject(Employees, Formatting.Indented);
            System.IO.File.WriteAllText(workingDir + @"\Employees.json", json);
        }

        private static void DumpExpenses()
        {
            var expensesCollection = Mongo.GetCollection<BsonDocument>("Expense").Find(new BsonDocument()).ToList();
            foreach (var e in expensesCollection)
            {
                var note = e["note"].ToString();
                var amount = e["totalAmount"].ToDecimal();
                foreach (var cartItem in e["cart"].AsBsonArray)
                {
                    note += $". { cartItem["productName"].ToString()}- " +
                        $"{ cartItem["unitPurchasePrice"].ToDecimal() * cartItem["quantity"].ToDecimal()}";
                }
                Expenses[e["_id"].ToString()] = new Expense
                {
                    Name = "Expense",
                    Amount = amount,
                    Description = note,
                    IsRemoved = false,
                    Metadata = GetMetadata(e["meta"].AsBsonDocument)
                };
            }
            var json = JsonConvert.SerializeObject(Expenses, Formatting.Indented);
            System.IO.File.WriteAllText(workingDir + @"\Expenses.json", json);
        }

        private static void DumpDebtPayments()
        {
            var debtPaymentsCollection = Mongo.GetCollection<BsonDocument>("DebtCollection").Find(new BsonDocument()).ToList();
            foreach (var e in debtPaymentsCollection)
            {
                var amount = e["amount"].ToDecimal();
                var customerId = e["customerId"].ToString();
                Customer customer = Customers.Keys.Contains(customerId)
                    ? Customers[customerId]
                    : null;
                var newItem = new DebtPayment
                {
                    CustomerId = customer?.Id ?? 0,
                    DebtBefore = amount,
                    Amount = amount,
                    Description = "",
                    IsRemoved = false,
                    Metadata = GetMetadata(e["meta"].AsBsonDocument)
                };

                newItem.Invoice = new CustomerInvoice
                {
                    Date = DateTime.Now,
                    PreviousDue = newItem.Amount,
                    PaymentPaid = newItem.Amount,
                    DebtPayment = newItem,
                    CustomerId = customer?.Id ?? 0,
                    PaymentDiscountCash = 0M,
                    Metadata = newItem.Metadata
                };
                DebtPayments[e["_id"].ToString()] = newItem;
            }
            var json = JsonConvert.SerializeObject(DebtPayments, Formatting.Indented, jss);
            System.IO.File.WriteAllText(workingDir + @"\DebtPayments.json", json);
        }

        private static void DumpSupplierPayments()
        {
            var supplierPaymentsCollection = Mongo.GetCollection<BsonDocument>("Repayment").Find(new BsonDocument()).ToList();
            foreach (var e in supplierPaymentsCollection)
            {
                var supplierId = e["supplierId"].ToString();
                Supplier supplier = Customers.Keys.Contains(supplierId)
                    ? Suppliers[supplierId]
                    : null;
                var amount = e["amount"].ToDecimal();
                var newItem = new SupplierPayment
                {
                    SupplierId = supplier?.Id ?? 0,
                    PayableBefore = amount,
                    Amount = amount,
                    Description = "",
                    IsRemoved = false,
                    Metadata = GetMetadata(e["meta"].AsBsonDocument)
                };

                newItem.Vouchar = new Vouchar
                {
                    Date = DateTime.Now,
                    PreviousDue = newItem.Amount,
                    PaymentPaid = newItem.Amount,
                    SupplierPayment = newItem,
                    SupplierId = newItem.SupplierId,
                    Metadata = newItem.Metadata
                };
                SupplierPayments[e["_id"].ToString()] = newItem;
            }
            var json = JsonConvert.SerializeObject(SupplierPayments, Formatting.Indented, jss);
            System.IO.File.WriteAllText(workingDir + @"\SupplierPayments.json", json);
        }

        private static void DumpProducts()
        {
            // Initialize ShopsSpace
            foreach (var shop in Shops)
            {
                Products[shop] = new Dictionary<string, Product>();
            }

            var productsCollection = Mongo.GetCollection<BsonDocument>("Product").Find(new BsonDocument()).ToList();
            foreach (var d in productsCollection)
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

                Products[d["shopId"].ToString()][d["_id"].ToString()] = new Product
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
                    Metadata = GetMetadata(d["meta"].AsBsonDocument)

                };
            }
            foreach (var shop in Products.Keys)
            {
                var json = JsonConvert.SerializeObject(Products[shop], Formatting.Indented, jss);
                System.IO.File.WriteAllText(workingDir + @"\" + shop + @"\Products.json", json);
            }

        }

        private static void DumpPurchases()
        {
            var purchasesCollection =
                Mongo.GetCollection<BsonDocument>("Purchase")
                    .Find(new BsonDocument())
                    .ToList();
            foreach (var p in purchasesCollection)
            {
                var supplierId = p["supplierId"].ToString();
                Supplier supplier = Suppliers.Keys.Contains(supplierId)
                    ? Suppliers[supplierId]
                    : null;
                if (supplier is null)
                {
                    Console.WriteLine("WARNING- SUPPLIER NOT FOUND");
                    Console.ReadKey();
                }
                var amount = p["totalAmount"].ToDecimal();
                var less = p["less"].ToDecimal();
                var paid = p["paid"].ToDecimal();
                var note = RemoveBsonNull(p["note"].ToString());

                var dealtime = DateTime.SpecifyKind(p["dealTime"].ToUniversalTime(), DateTimeKind.Utc);

                var cart = new List<PurchaseLineItem>();
                foreach (BsonDocument li in p["cart"].AsBsonArray)
                {
                    var productId = li["productId"].ToString();
                    if (AllProducts.Keys.Contains(productId))
                    {
                        var prod = AllProducts[productId];
                        if (prod.Id > 0)
                        {
                            var q = li["quantity"].ToDecimal();
                            var upp = li["unitPurchasePrice"].ToDecimal();
                            cart.Add(new PurchaseLineItem(prod, q, q * upp));
                        }
                        else
                        {
                            Console.WriteLine("WARNING- PRODUCT NOT FOUND");
                            Console.ReadKey();
                        }
                    }
                }

                var newItem = new Purchase
                {
                    SupplierId = supplier?.Id ?? 0,
                    PurchaseDate = dealtime,
                    Payment = new PaymentInfo
                    {
                        SubTotal = amount,
                        DiscountCash = less,
                        Paid = paid
                    },
                    Cart = cart,
                    Description = note,
                    Metadata = GetMetadata(p["meta"].AsBsonDocument)
                };

                var vouchar = new Vouchar
                {
                    Date = DateTime.Now,
                    PreviousDue = 0,
                    PaymentSubtotal = newItem.Payment.SubTotal,
                    PaymentPaid = newItem.Amount,
                    Cart = newItem.Cart.Select(
                        pli => new InvoiceLineItem
                        {
                            Name = pli.Name,
                            Quantity = pli.Quantity,
                            UnitPrice = pli.UnitPurchasePrice,
                            NetPrice = pli.NetPurchasePrice
                        }).ToList(),
                    Purchase = newItem,
                    SupplierId = newItem.SupplierId,
                    Metadata = newItem.Metadata
                };
                newItem.Vouchar = vouchar;
                Purchases[p["_id"].ToString()] = newItem;
            }

            var json = JsonConvert.SerializeObject(Purchases, Formatting.Indented, jss);
            System.IO.File.WriteAllText(workingDir + @"\Purchases.json", json);
        }

        private static void DumpSales()
        {
            // Initialize ShopsSpace
            foreach (var shop in Shops)
            {
                Sales[shop] = new Dictionary<string, Sale>();
            }

            var salesCollection = Mongo.GetCollection<BsonDocument>("Sale").Find(new BsonDocument()).ToList();
            foreach (var s in salesCollection)
            {
                var customerId = s["customerId"].ToString();
                Customer customer = Customers.Keys.Contains(customerId)
                    ? Customers[customerId]
                    : null;

                var shopId = s["shopId"].ToString();

                var amount = s["totalAmount"].ToDecimal();
                var less = s["less"].ToDecimal();
                var paid = s["paid"].ToDecimal();
                var note = RemoveBsonNull(s["notes"].ToString());
                var dealtime = DateTime.SpecifyKind(s["dealTime"].ToUniversalTime(), DateTimeKind.Utc);
                var saleType = (SaleType)s["saleType"].ToInt32();

                var cart = new List<SaleLineItem>();
                foreach (BsonDocument li in s["cart"].AsBsonArray)
                {
                    var productId = li["productId"].ToString();
                    if (Products[shopId].Keys.Contains(productId))
                    {
                        var prod = Products[shopId][productId];
                        var q = li["quantity"].ToDecimal();
                        var upp = li["unitPurchasePrice"].ToDecimal();
                        cart.Add(new SaleLineItem(prod, q, q * upp));
                    }
                }

                var newItem = new Sale
                {
                    CustomerId = customer?.Id ?? 0,
                    SaleDate = dealtime,
                    Type = saleType,
                    Payment = new PaymentInfo
                    {
                        SubTotal = amount,
                        DiscountCash = less,
                        Paid = paid
                    },
                    Cart = cart,
                    Description = note,
                    Metadata = GetMetadata(s["meta"].AsBsonDocument)
                };

                var customerInvoice = new CustomerInvoice
                {
                    Date = DateTime.Now,
                    PreviousDue = 0,
                    PaymentSubtotal = newItem.Payment.SubTotal,
                    PaymentPaid = newItem.Amount,
                    Cart = newItem.Cart.Select(
                        pli => new InvoiceLineItem
                        {
                            Name = pli.Name,
                            Quantity = pli.Quantity,
                            UnitPrice = pli.UnitPrice,
                            NetPrice = pli.NetPrice
                        }).ToList(),
                    Sale = newItem,
                    CustomerId = newItem.CustomerId,
                    Metadata = newItem.Metadata
                };
                newItem.Invoice = customerInvoice;
                Sales[shopId][s["_id"].ToString()] = newItem;
            }

            foreach (var shop in Shops)
            {
                var json = JsonConvert.SerializeObject(Sales[shop], Formatting.Indented, jss);
                System.IO.File.WriteAllText(workingDir + @"\" + shop + @"\Sales.json", json);
            }
        }

        private static string RemoveBsonNull(string str) => str == "BsonNull" ? "" : str;

        private static Metadata GetMetadata(BsonDocument d)
        {
            var created = DateTime.SpecifyKind(
                d["created"].ToUniversalTime(),
                DateTimeKind.Utc);
            var modified = DateTime.SpecifyKind(
                d["modified"].ToUniversalTime(),
                DateTimeKind.Utc);

            return new Metadata
            {
                Creator = d["creator"].ToString(),
                Modifier = d["modifier"].ToString(),
                CreationTime = created,
                ModificationTime = modified
            };
        }
    }
}

