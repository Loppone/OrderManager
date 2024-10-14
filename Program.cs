using AddressService;
using GenericService;
using Microsoft.EntityFrameworkCore;
using OrderManagerApi;
using OrderService;
using OrderService.Models.Business;
using ProductService;
using UserService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Automapper
builder.Services.AddAutoMapper(typeof(GenericMappingProfile<,>));
builder.Services.AddAutoMapper(typeof(OrderMappingDtoProfile));
builder.Services.AddAutoMapper(typeof(OrderMappingBusinessProfile));
builder.Services.AddAutoMapper(typeof(ProductMappingDtoProfile));
builder.Services.AddAutoMapper(typeof(ProductMappingBusinessProfile));
builder.Services.AddAutoMapper(typeof(UserMappingDtoProfile));
builder.Services.AddAutoMapper(typeof(UserMappingBusinessProfile));
builder.Services.AddAutoMapper(typeof(AddressMappingDtoProfile));
builder.Services.AddAutoMapper(typeof(AddressMappingBusinessProfile));

// DbContext
var connectionString = builder.Configuration.GetConnectionString("OrderManagerConnectionString");

builder.Services.AddDbContext<OrderService.Data.OrderDbContext>(opt =>
        opt.UseSqlServer(connectionString));

builder.Services.AddDbContext<ProductService.Data.ProductDbContext>(opt =>
        opt.UseSqlServer(connectionString));

builder.Services.AddDbContext<UserService.Data.UserDbContext>(opt =>
        opt.UseSqlServer(connectionString));

builder.Services.AddDbContext<AddressService.Data.AddressDbContext>(opt =>
        opt.UseSqlServer(connectionString));

// DI
builder.Services.AddScoped<IGenericService<Order>, GenericService<Order, OrderService.Models.Data.Order>>();
builder.Services.AddScoped<IRepository<OrderService.Models.Data.Order>, Repository<OrderService.Models.Data.Order, OrderService.Data.OrderDbContext>>();
builder.Services.AddScoped<IGenericService<OrderProduct>, GenericService<OrderProduct, OrderService.Models.Data.OrderProduct>>();
builder.Services.AddScoped<IRepository<OrderService.Models.Data.OrderProduct>, Repository<OrderService.Models.Data.OrderProduct, OrderService.Data.OrderDbContext>>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, CustomOrderService>();

builder.Services.AddScoped<IRepository<ProductService.Models.Data.Product>, Repository<ProductService.Models.Data.Product, ProductService.Data.ProductDbContext>>();
builder.Services.AddScoped<IGenericService<ProductService.Models.Business.Product>, GenericService<ProductService.Models.Business.Product, ProductService.Models.Data.Product>>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, CustomProductService>();

builder.Services.AddScoped<IRepository<UserService.Models.Data.User>, Repository<UserService.Models.Data.User, UserService.Data.UserDbContext>>();
builder.Services.AddScoped<IGenericService<UserService.Models.Business.User>, GenericService<UserService.Models.Business.User, UserService.Models.Data.User>>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, CustomUserService>();

builder.Services.AddScoped<IRepository<AddressService.Models.Data.Address>, Repository<AddressService.Models.Data.Address, AddressService.Data.AddressDbContext>>();
builder.Services.AddScoped<IGenericService<AddressService.Models.Business.Address>, GenericService<AddressService.Models.Business.Address, AddressService.Models.Data.Address>>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, CustomAddressService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DummyDb.Init();

app.Run();
