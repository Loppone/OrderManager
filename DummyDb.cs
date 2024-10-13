using AddressService.Models.Data;
using Microsoft.EntityFrameworkCore;
using OrderService.Models.Data;
using ProductService.Models.Data;
using UserService.Models.Data;

namespace OrderManagerApi
{
    public class DummyDb
    {
        public static void Init()
        {
            var ctx = new DummyDbContext();

            var mustCreateDb = false;
            if (mustCreateDb)
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();

                using (var tr = ctx.Database.BeginTransaction())
                {
                    var users = new List<User>()
                {
                    new User() { Id = 1, Name = "Max", Email = "test@gmail.com", PhoneNumber = "3391234567" }
                };

                    ctx.Users.AddRange(users);

                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Users ON");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Users OFF");


                    var addresses = new List<Address>()
                {
                    new Address { Id = 1, UserId = 1, Street = "Via degli orti", StreetNumber = "11", City = "Mandello del Lario", ZipCode = "23826" },
                    new Address { Id = 2, UserId = 1, Street = "Via delle erbe", StreetNumber = "13", City = "Mandello del Lario", ZipCode = "23826" }
                };

                    ctx.Addresses.AddRange(addresses);

                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Addresses ON");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Addresses OFF");


                    var categories = new List<Category>()
                {
                    new Category() { Id = 1, Name = "Attrezzi" },
                    new Category() { Id = 2, Name = "Abbigliamento" },
                };

                    ctx.Categories.AddRange(categories);

                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Categories ON");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Categories OFF");

                    var products = new List<Product>()
                {
                    new Product() { Id = 1, CategoryId = 1, Name = "Racchetta RF 01", Price = 280 },
                    new Product() { Id = 2, CategoryId = 2, Name = "T-Shirt RF", Price = 50 },
                    new Product() { Id = 3, CategoryId = 2, Name = "Borsone RF Collection", Price = 240 },
                };

                    ctx.Products.AddRange(products);

                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Products ON");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Products OFF");

                    var orders = new List<Order>()
                {
                    new Order() { Id = 1, UserId = 1, OrderDate = DateTime.Now },
                    new Order() { Id = 2, UserId = 2, OrderDate = DateTime.Now }
                };

                    ctx.Orders.AddRange(orders);

                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Orders ON");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Orders OFF");

                    var ordersProducts = new List<OrderProduct>()
                {
                    new OrderProduct() { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2 },
                    new OrderProduct() { Id = 2, OrderId = 1, ProductId = 2, Quantity = 1 },
                    new OrderProduct() { Id = 3, OrderId = 2, ProductId = 3, Quantity = 1 },
                };

                    ctx.OrderProducts.AddRange(ordersProducts);

                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.OrdersProducts ON");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.OrdersProducts OFF");

                    tr.Commit();
                }
            }
        }

        public class DummyDbContext : DbContext
        {
            public DbSet<Address> Addresses { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderProduct> OrderProducts { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<User> Users { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PhotoSi;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}
