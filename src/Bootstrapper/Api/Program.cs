

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;

builder.Services
        .AddCarterWithAssemblies(catalogAssembly, basketAssembly);

builder.Services.AddMediatWithAssemblies(catalogAssembly, basketAssembly);



builder.Services
        .AddCatalogModule(builder.Configuration)
        .AddBasketModule(builder.Configuration)
        .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });


app.UseCatalogModule()
   .UseBasketModule()
   .UseOrderingModule();


app.Run();
