using GenericService;
using Microsoft.EntityFrameworkCore;
using OrderManagerApi;
using OrderService;
//using OrderService.Data;
//using OrderService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Automapper
builder.Services.AddAutoMapper(typeof(GenericMappingProfile<,>)); // Usa il profilo generico
builder.Services.AddAutoMapper(typeof(OrderMappingProfile));

// DbContext
var orderConnectionString = builder.Configuration.GetConnectionString("OrderConnectionString");

builder
    .Services
    .AddDbContext<OrderService.Data.OrderDbContext>(opt =>
        {
            //opt.UseLazyLoadingProxies();
            opt.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnectionString"));
        });

// DI
builder.Services.AddScoped<IRepository<OrderService.Data.Order>, Repository<OrderService.Data.Order, OrderService.Data.OrderDbContext>>();
builder.Services.AddScoped<IGenericService<OrderService.Models.Order>, GenericService<OrderService.Models.Order, OrderService.Data.Order>>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, CustomOrderService>();


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
