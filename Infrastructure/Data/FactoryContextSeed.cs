using System.Reflection;
using System.Text.Json;
using Domain.Entities;
using Domain.Entities.BaseEntities;
using Domain.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class FactoryContextSeed
{
    private readonly FactoryContext _context;
    private readonly ILoggerFactory _loggerFactory;

    public FactoryContextSeed(FactoryContext context, ILoggerFactory loggerFactory)
    {
        _context = context;
        _loggerFactory = loggerFactory;
    }

    public bool Seed()
    {
        try
        {
            var logger = _loggerFactory.CreateLogger<FactoryContext>();
            if ( SeedData<Category>(@"/Data/SeedData/Categories.json")) logger.LogInformation("Categories data has been added successfully.");
            if ( SeedData<Product>(@"/Data/SeedData/Products.json")) logger.LogInformation("Products data has been added successfully.");
            if ( SeedData<Warehouse>(@"/Data/SeedData/Warehouses.json")) logger.LogInformation("Warehouses data has been added successfully.");
            if ( SeedData<Vehicle>(@"/Data/SeedData/Vehicles.json")) logger.LogInformation("Vehicles data has been added successfully.");
            if ( SeedData<VehiclesPart>(@"/Data/SeedData/VehiclesParts.json")) logger.LogInformation("VehiclesParts data has been added successfully.");
            if ( SeedData<DeliveryMethod>(@"/Data/SeedData/DeliveryMethods.json")) logger.LogInformation("DeliveryMethods data has been added successfully.");
            return true;
        }
        catch (Exception ex)
        {
            var logger = _loggerFactory.CreateLogger<FactoryContext>();
            logger.LogError(ex.Message);
        }
        return false;
    }


    public bool SeedData<TEntity>(string fileName) where TEntity : class
    {
        try
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + fileName;
             using var json = File.OpenRead(path);

            if (!_context.Set<TEntity>().Any())
            {
                var jsonData = JsonSerializer.Deserialize<List<TEntity>>(json);
                if (jsonData != null)
                {
                    foreach (var item in jsonData)  _context.Set<TEntity>().Add(item);

                     _context.SaveChanges();
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            var logger = _loggerFactory.CreateLogger<FactoryContext>();
            logger.LogError(ex.Message);
        }
        return false;
    }
}