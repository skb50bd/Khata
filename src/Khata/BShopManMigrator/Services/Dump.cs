using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Data.Core;
using Domain;

using MongoDB.Bson;
using MongoDB.Driver;

using Newtonsoft.Json;

namespace ImportData.Services
{
    public static class Dump
    {
        #region Data Properties
        static string[] Shops = new string[0];
        static readonly Dictionary<string, Outlet> Outlets =
            new Dictionary<string, Outlet>();
        static readonly Dictionary<string, Customer> Customers =
            new Dictionary<string, Customer>();
        static readonly Dictionary<string, Supplier> Suppliers =
            new Dictionary<string, Supplier>();
        static readonly Dictionary<string, Expense> Expenses =
            new Dictionary<string, Expense>();
        static readonly Dictionary<string, Employee> Employees =
            new Dictionary<string, Employee>();
        static readonly Dictionary<string, DebtPayment> DebtPayments =
            new Dictionary<string, DebtPayment>();
        static readonly Dictionary<string, SupplierPayment> SupplierPayments =
            new Dictionary<string, SupplierPayment>();
        static readonly Dictionary<string, Purchase> Purchases =
            new Dictionary<string, Purchase>();
        static readonly Dictionary<string, Dictionary<string, Sale>> Sales =
            new Dictionary<string, Dictionary<string, Sale>>();
        static readonly Dictionary<string, Dictionary<string, Product>> Products =
            new Dictionary<string, Dictionary<string, Product>>();
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
        static Dictionary<string, Sale> AllSales
        {
            get
            {
                var sales = new Dictionary<string, Sale>();
                foreach (var key in Sales.Keys)
                {
                    foreach ((var id, var sale) in Sales[key])
                    {
                        sales[id] = sale;
                    }
                }
                return sales;
            }
        } 
        #endregion

        static decimal CashBalance { get; set; }

        static readonly JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

        static readonly IMongoDatabase Mongo = new MongoClient().GetDatabase("BShopManDb");
        static readonly string workingDir = @"D:\Dump";

        public static async Task InsertAllAsync(IUnitOfWork db)
        {
            DumpShops();
            foreach(var o in Outlets.Values)
            {
                db.Outlets.Add(o);
            }
            db.Complete();

            var ans = "";
            #region Customers
            DumpCustomers();
            Console.Write("Customers? (enter N to cancel): ");
            ans = Console.ReadLine();
            if (ans.ToUpperInvariant() != "N")
            {
                foreach (var c in Customers.Values.ToList())
                {
                    db.Customers.Add(c);
                }
            }
            #endregion
            db.Complete();

            #region Suppliers
            DumpSuppliers();
            Console.Write("Suppliers? (enter N to cancel): ");
            ans = Console.ReadLine();
            if (ans.ToUpperInvariant() != "N")
            {
                foreach (var c in Suppliers.Values.ToList())
                {
                    db.Suppliers.Add(c);
                }
            }
            #endregion
            db.Complete();

            #region Employees
            DumpEmployees();
            Console.Write("Employees? (enter N to cancel): ");
            ans = Console.ReadLine();
            if (ans.ToUpperInvariant() != "N")
            {
                foreach (var c in Employees.Values.ToList())
                {
                    db.Employees.Add(c);
                }
            }
            #endregion

            #region Expenses
            DumpExpenses();
            Console.Write("Expenses? (enter N to cancel): ");
            ans = Console.ReadLine();
            if (ans.ToUpperInvariant() != "N")
            {
                foreach (var c in Expenses.Values)
                {
                    db.Expenses.Add(c);
                }
                db.Complete();
                foreach (var item in Expenses.Values)
                {
                    var withdrawal = new Withdrawal(item as IWithdrawal)
                    {
                        Metadata = item.Metadata
                    };
                    db.Withdrawals.Add(withdrawal);
                }
                db.Complete();
            }
            #endregion

            #region Create Cash Customers and Suppliers
            var customer = new Customer
            {
                FirstName = "Cash",
                LastName = "Customer",
                Address = "",
                Phone = "00000000000",
                Email = "someone@example.com",
                CompanyName = "N/A",
                Metadata = Metadata.CreatedNew("auto")
            };

            var supplier = new Supplier
            {
                FirstName = "Cash",
                LastName = "Supplier",
                Address = "",
                Phone = "00000000000",
                Email = "someone@example.com",
                CompanyName = "N/A",
                Metadata = Metadata.CreatedNew("auto")
            };
            #endregion

            #region SupplierPayments
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
                    db.SupplierPayments.Add(sp);
                }
                db.Complete();
                foreach (var item in SupplierPayments.Values)
                {
                    var withdrawal = new Withdrawal(item as IWithdrawal)
                    {
                        Metadata = item.Metadata
                    };
                    db.Withdrawals.Add(withdrawal);
                }
                db.Complete();

            }
            #endregion

            #region Products
            DumpProducts();
            Console.Write("Products? (enter N to cancel): ");
            ans = Console.ReadLine();
            if (ans.ToUpperInvariant() != "N")
            {
                foreach (var p in 
                    AllProducts.Values
                        .OrderBy(p => p.Name))
                {
                    db.Products.Add(p);
                db.Complete();
                }
            }
            #endregion


            #region Purchase
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
                db.Complete();
                foreach (var item in Purchases.Values)
                {
                    var withdrawal = new Withdrawal(item as IWithdrawal)
                    {
                        Metadata = item.Metadata
                    };
                    db.Withdrawals.Add(withdrawal);
                }
                db.Complete();
            }
            #endregion

            #region Sales
            DumpSales();
            Console.Write("Sales? (enter N to cancel): ");
            ans = Console.ReadLine();
            if (ans.ToUpperInvariant() != "N")
            {
                var added = 0;
                foreach (var sale in AllSales.Values)
                {
                    Console.WriteLine($"CustomerId: {sale.CustomerId}");
                    var customerExists = await db.Customers.Exists(sale.CustomerId);
                    Console.WriteLine($"Customer Exists: {customerExists}");
                    if (!customerExists)
                    {
                        sale.CustomerId = (await db.Customers.Get(
                            c => c.FullName.ToLowerInvariant().Contains("cash"),
                            c => c.Id, 1, 0)).FirstOrDefault().Id;
                        sale.Invoice.CustomerId = sale.CustomerId;
                    }

                    db.Sales.Add(sale);
                    Console.WriteLine($"{added++} sales added");
                }
                db.Complete();
                foreach (var item in AllSales.Values)
                {
                    var deposit = new Deposit(item as IDeposit)
                    {
                        Metadata = item.Metadata
                    };
                    db.Deposits.Add(deposit);
                }
                db.Complete();
            }
            #endregion

            #region DebtPayments
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

                    db.DebtPayments.Add(dp);
                }
                db.Complete();
                foreach (var item in DebtPayments.Values)
                {
                    var deposit = new Deposit(item as IDeposit)
                    {
                        Metadata = item.Metadata
                    };
                    db.Deposits.Add(deposit);
                }
                db.Complete();
            }
            #endregion

            #region Cash
            GetCashData();
            Console.Write("Cash? (enter N to cancel): ");
            ans = Console.ReadLine();
            if (ans.ToUpperInvariant() != "N")
            {
                var cr = await db.CashRegister.Get();
                cr.Balance = CashBalance;
            }
            else
            {
                var cr = await db.CashRegister.Get();
                cr.Balance = 0M;
            }
            #endregion

            db.Complete();

            Console.WriteLine("Everything Added...");
        }

        private static void DumpShops()
        {
            var shopsCollection = Mongo.GetCollection<BsonDocument>("Shop").Find(new BsonDocument()).ToList();
            foreach (var s in shopsCollection)
            {
                var title = s["shopName"].ToString().Trim();
                var slogan = s["tagline"].ToString().Trim();
                var address = s["address"].ToString().Trim();
                var phone = string.Join(", ", s["contactNumbers"].AsBsonArray.Select(b => b.ToString())).Trim();
                var email = string.Join(", ", s["emailAddresses"].AsBsonArray.Select(b => b.ToString())).Trim();

                Outlets[s["_id"].ToString()] = new Outlet
                {
                    Title = title,
                    Slogan = slogan,
                    Address = address,
                    Phone = phone,
                    Email = email,
                    Metadata = GetMetadata(s["meta"].AsBsonDocument)
                };
            }
            var json = JsonConvert.SerializeObject(Outlets, Formatting.Indented);
            System.IO.File.WriteAllText(workingDir + @"\Outlets.json", json);

            Shops = shopsCollection.Select(s => s["_id"].ToString()).ToArray();
            Console.WriteLine($"{Shops.Count()} Shops Found");
            foreach (var shop in Shops)
                System.IO.Directory.CreateDirectory(workingDir + @"\" + shop);
        }

        private static void GetCashData()
        {
            var cashCollection = Mongo.GetCollection<BsonDocument>("Cash").Find(new BsonDocument()).ToList();
            CashBalance = cashCollection.ToArray().FirstOrDefault()["current"].ToDecimal();
            Console.WriteLine($"Current Balance {CashBalance}");
        }

        private static void DumpCustomers()
        {
            var customersCollection = Mongo.GetCollection<BsonDocument>("Customer").Find(new BsonDocument()).ToList();
            foreach (var c in customersCollection)
            {
                var nameTokens = c["fullName"].ToString().Split();
                var firstName = nameTokens.FirstOrDefault();
                var lastName = string.Join(' ', nameTokens.TakeLast(nameTokens.Count() - 1)).Trim();
                if (firstName.Length == 0) firstName = "_____";
                if (lastName.Length == 0) lastName = "_____";
                var email = c["emailAddress"].ToString().Trim();
                var phone = c["phone"].ToString().Trim();
                var address = c["address"].ToString().Trim();
                var companyName = c["companyName"].ToString().Trim();
                var note = c["note"].ToString().Trim();
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
                var firstName = nameTokens.FirstOrDefault();
                var lastName = string.Join(' ', nameTokens.TakeLast(nameTokens.Count() - 1)).Trim();
                if (firstName.Length == 0) firstName = "_____";
                if (lastName.Length == 0) lastName = "_____";
                var email = s["emailAddress"].ToString().Trim();
                var phone = s["phone"].ToString().Trim();
                var address = s["address"].ToString().Trim();
                var companyName = s["companyName"].ToString().Trim();
                var note = s["note"].ToString().Trim();
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
                var firstName = nameTokens.FirstOrDefault();
                var lastName = string.Join(' ', nameTokens.TakeLast(nameTokens.Count() - 1)).Trim();
                var email = e["emailAddress"].ToString().Trim();
                var phone = e["phone"].ToString().Trim();
                var address = e["address"].ToString().Trim();
                var designation = e["designation"].ToString().Trim();
                var balance = e["currentBalance"].ToDecimal();
                var nid = e["nationalIdN"].ToString().Trim();
                var salary = e["monthlySalary"].ToDecimal();
                var note = e["note"].ToString().Trim();
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
                var note = e["note"].ToString().Trim();
                var amount = e["totalAmount"].ToDecimal();
                foreach (var cartItem in e["cart"].AsBsonArray)
                {
                    if (!string.IsNullOrWhiteSpace(note)) note += ". \n";
                    note += $"{ cartItem["productName"].ToString()}- " +
                        $"{ cartItem["unitPurchasePrice"].ToDecimal() * cartItem["quantity"].ToDecimal()}";
                }
                note = note.Trim();
                Expenses[e["_id"].ToString()] = new Expense
                {
                    Name        = "Expense",
                    Amount      = amount,
                    Description = note,
                    IsRemoved   = false,
                    Metadata    = GetMetadata(e["meta"].AsBsonDocument)
                };
            }
            var json = JsonConvert.SerializeObject(Expenses, Formatting.Indented);
            System.IO.File.WriteAllText(workingDir + @"\Expenses.json", json);
        }

        private static void DumpDebtPayments()
        {
            var debtPaymentsCollection = 
                Mongo.GetCollection<BsonDocument>("DebtCollection")
                    .Find(new BsonDocument()).ToList();
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
            //var json = JsonConvert.SerializeObject(DebtPayments, Formatting.Indented, jss);
            //System.IO.File.WriteAllText(workingDir + @"\DebtPayments.json", json);
        }

        private static void DumpSupplierPayments()
        {
            var supplierPaymentsCollection = 
                Mongo.GetCollection<BsonDocument>("Repayment")
                    .Find(new BsonDocument()).ToList();
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
                var name = d["productName"].ToString().Trim();
                var shopId = d["shopId"].ToString();
                var unit = d["units"][0]["unitName"].ToString().Trim();
                var shopStock = d["shopStock"].ToDecimal();
                var warehouseStock = d["godownStock"].ToDecimal();
                var alertStock = d["alertStock"].ToDecimal();
                var purchasePrice = d["purchasePrice"].ToDecimal();
                var retailPrice = d["retailPrice"].ToDecimal();
                var bulkPrice = d["wholeSalePrice"].ToDecimal();
                var specification = d["specification"].ToString().Trim();
                var category = d["category"].ToString().Trim();
                var manufacturer = d["manufacturer"].ToString().Trim();
                var note = d["notes"].ToString().Trim();

                Products[d["shopId"].ToString()][d["_id"].ToString()] = new Product
                {
                    Name = RemoveBsonNull(name),
                    OutletId = Outlets[shopId].Id,
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
                        Margin = purchasePrice * 1.01M
                    },
                    IsRemoved = false,
                    Description = (RemoveBsonNull(note)
                            + $"\nManufacturer: {manufacturer}."
                            + $"\nCategory: {category}."
                            + $"\nSpecification: {specification}.").Trim(),
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
                    //Console.WriteLine("WARNING- SUPPLIER NOT FOUND");
                    //Console.ReadKey();
                }
                var amount = p["totalAmount"].ToDecimal();
                var less = p["less"].ToDecimal();
                var paid = p["paid"].ToDecimal();
                var note = RemoveBsonNull(p["note"].ToString()).Trim();

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

            var salesCollection = 
                Mongo.GetCollection<BsonDocument>("Sale")
                    .Find(new BsonDocument()).ToList();
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
                var note = RemoveBsonNull(s["notes"].ToString()).Trim();
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
                        var up = li["unitPrice"].ToDecimal();
                        var upp = li["unitPurchasePrice"].ToDecimal();
                        cart.Add(new SaleLineItem(prod, q, q * up, q * upp));
                    }
                }

                var newItem = new Sale
                {
                    CustomerId = customer?.Id ?? 0,
                    SaleDate = dealtime,
                    Type = saleType,
                    OutletId = Outlets[shopId].Id,
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

                var dp = new DebtPayment
                {
                    CustomerId = customer?.Id ?? 0,
                    DebtBefore = newItem.Payment.Paid - newItem.Payment.Total,
                    Amount = newItem.Payment.Paid - newItem.Payment.Total,
                    Description = note,
                    Metadata = GetMetadata(s["meta"].AsBsonDocument)
                };

                var customerInvoice = new CustomerInvoice
                {
                    Date = DateTime.Now,
                    OutletId = Outlets[shopId].Id,
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
                    CustomerId = newItem.CustomerId,
                    Metadata = newItem.Metadata
                };
                newItem.Invoice = customerInvoice;
                dp.Invoice = customerInvoice;

                if(dp.Amount > 0)
                {
                    newItem.Payment.Paid -= dp.Amount;
                    dp.Invoice = customerInvoice;
                    DebtPayments[s["_id"].ToString()] = dp;
                }
                if (newItem.Cart.Any())
                {
                    newItem.Invoice = customerInvoice;
                    Sales[shopId][s["_id"].ToString()] = newItem;
                }
            }

            foreach (var shop in Shops)
            {
                var json = JsonConvert.SerializeObject(Sales[shop], Formatting.Indented, jss);
                System.IO.File.WriteAllText(workingDir + @"\" + shop + @"\Sales.json", json);
            }
        }

        private static string RemoveBsonNull(string str)
            => str == "BsonNull" ? "" : str.Trim();

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

