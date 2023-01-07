using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.Specifications.ProductSpecifications;
using Domain.Specifications.VehicleSpecifications;

namespace Infrastructure.Services;

public class VehicleService : IVehicleService
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetObjectFromServicesSpecification> CreateVehicle(Vehicle createVehicleParams,
        CancellationToken cancellationToken)
    {
        var returnObject = new GetObjectFromServicesSpecification();

        var specParams = new GetVehicleSpecificationParams();

        specParams.Search = createVehicleParams.Name;

        var spec = new GetVehiclesSpecification(specParams);

        var vehicleExist = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec, cancellationToken);

        if (vehicleExist != null)
        {
            returnObject.StatusCode = 409;

            returnObject.Message = "The vehicle name is already exist.";
            return returnObject;
        }

        await _unitOfWork.Repository<Vehicle>().InsertAsync(createVehicleParams, cancellationToken);

        var result = await _unitOfWork.Complete(cancellationToken);

        if (result <= 0)
        {
            returnObject.StatusCode = 400;

            returnObject.Message = "Bad request.";

            return returnObject;
        }

        returnObject.StatusCode = 201;

        returnObject.Message = "Vehicle has been created successfully.";

        returnObject.ResultObject = createVehicleParams;

        return returnObject;
    }

    public async Task<GetObjectFromServicesSpecification> UpdateVehicle(Vehicle updateVehicleParams,
        CancellationToken cancellationToken)
    {
        var returnObject = new GetObjectFromServicesSpecification();

        var specParams = new GetVehicleSpecificationParams();

        var currentTimestamp = DateTimeOffset.Now;

        if (updateVehicleParams.UpdatedDate != null) currentTimestamp = (DateTimeOffset)updateVehicleParams.UpdatedDate;

        specParams.Search = updateVehicleParams.Name;

        var spec = new GetVehiclesSpecification(specParams);

        var vehicleExist = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec, cancellationToken);

        if (vehicleExist != null && vehicleExist.Id != updateVehicleParams.Id)
        {
            returnObject.StatusCode = 409;

            returnObject.Message = "The vehicle name is already exist.";

            return returnObject;
        }

        var result = await _unitOfWork.Complete(cancellationToken);

        if (result <= 0 && updateVehicleParams.UpdatedDate == currentTimestamp)
        {
            returnObject.StatusCode = 304;

            returnObject.Message = "Not Modified.";

            return returnObject;
        }

        if (result <= 0)
        {
            returnObject.StatusCode = 400;

            returnObject.Message = "Bad request.";

            return returnObject;
        }

        returnObject.StatusCode = 204;

        returnObject.Message = "Vehicle has been updated successfully.";

        returnObject.ResultObject = updateVehicleParams;

        return returnObject;
    }

    public async Task<GetObjectFromServicesSpecification> AddOrUpdateVehiclesParts(
        AddOrUpdateVehiclesPartsSpecificationParams updateVehiclesPartParams,
        CancellationToken cancellationToken = default)
    {
        var returnObject = new GetObjectFromServicesSpecification();

        // Check if not found Vehicle
        var vehicleSpec = new GetVehiclesWithPartsSpecification(updateVehiclesPartParams.VehicleId);

        var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(vehicleSpec, cancellationToken);

        if (vehicle == null)
        {
            returnObject.StatusCode = 404;

            returnObject.Message = "The VehicleId not found.";

            return returnObject;
        }

        // Check if not found Product
        var productSpec = new GetProductsWithCategoriesSpecification(updateVehiclesPartParams.ProductId);

        var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(productSpec, cancellationToken);

        if (product == null)
        {
            returnObject.StatusCode = 404;

            returnObject.Message = "The ProductId not found.";

            return returnObject;
        }

        // Check the category of product
        var parentPath = product.Category.ParentPath;

        var result = 0;

        var parentPathId = product.Category.ParentPath != null
            ? int.Parse(parentPath.Split("/")[0])
            : product.CategoryId;

        var allVehiclesParts = vehicle.VehiclesParts.Where(x =>  
            (!string.IsNullOrEmpty(x.Product.Category.ParentPath)  ? 
                int.Parse(x.Product.Category.ParentPath!.Split("/")[0]) == parentPathId : 
                x.Product.CategoryId == parentPathId)
            ).ToList();

        if (allVehiclesParts.Any())
        {

            var vehicleParts = await _unitOfWork.Repository<VehiclesPart>().SearchAsync(x =>
                x.VehicleId == updateVehiclesPartParams.VehicleId &&
                x.ProductId == allVehiclesParts[0].ProductId, cancellationToken);

            // Update the VehiclesParts with same category. For instance if we receive a new chassis Id we have to realize that the old chassis Id need to change
            if (vehicleParts != null)
            {
                vehicleParts[0].ProductId = updateVehiclesPartParams.ProductId;

                result = await _unitOfWork.Complete(cancellationToken);

                var currentTimestamp = DateTimeOffset.Now;

                if (vehicleParts[0].UpdatedDate != null) currentTimestamp = (DateTimeOffset)vehicleParts[0].UpdatedDate;

                if (result <= 0 && (vehicleParts[0].UpdatedDate == currentTimestamp || vehicleParts[0].UpdatedDate == null))
                {
                    returnObject.StatusCode = 304;

                    returnObject.Message = "Not Modified.";

                    return returnObject;
                }

                returnObject.StatusCode = 204;

                returnObject.Message = "VehiclesParts has been updated successfully.";

                return returnObject;
            }
        }

        // Add VehiclesPart
        var vehiclePart = new VehiclesPart
        {
            ProductId = updateVehiclesPartParams.ProductId,

            VehicleId = updateVehiclesPartParams.VehicleId
        };

        await _unitOfWork.Repository<VehiclesPart>().InsertAsync(vehiclePart, cancellationToken);

        result = await _unitOfWork.Complete(cancellationToken);

        if (result <= 0)
        {
            returnObject.StatusCode = 400;

            returnObject.Message = "Bad request.";

            return returnObject;
        }

        returnObject.StatusCode = 201;

        returnObject.Message = "VehiclesPart has been created successfully.";

        returnObject.ResultObject = vehicle;

        return returnObject;
    }

}