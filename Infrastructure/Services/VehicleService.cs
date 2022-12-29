using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications;
using Domain.Specifications.VehicleSpecifications;

namespace Infrastructure.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetObjectFromServicesSpecification> CreateVehicle(Vehicle createVehicleParams)
        {
            var returnObject = new GetObjectFromServicesSpecification();

            var specParams = new GetVehicleSpecificationParams();

            specParams.Search = createVehicleParams.Name;

            var spec = new GetVehiclesSpecification(specParams);

            var vehicleExist = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            if (vehicleExist != null)
            {
                returnObject.StatusCode = 409;

                returnObject.Message = "The vehicle name is already exist.";
                return returnObject;
            }

            await _unitOfWork.Repository<Vehicle>().InsertAsync(createVehicleParams);

            var result = await _unitOfWork.Complete();

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

        public async Task<GetObjectFromServicesSpecification> UpdateVehicle(Vehicle updateVehicleParams)
        {
            var returnObject = new GetObjectFromServicesSpecification();

            var specParams = new GetVehicleSpecificationParams();
            var currentTimestamp = DateTimeOffset.Now;
            if (updateVehicleParams.UpdatedDate != null) currentTimestamp = (DateTimeOffset)updateVehicleParams.UpdatedDate;
            specParams.Search = updateVehicleParams.Name;

            var spec = new GetVehiclesSpecification(specParams);

            var vehicleExist = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            if (vehicleExist != null && vehicleExist.Id != updateVehicleParams.Id)
            {
                returnObject.StatusCode = 409;

                returnObject.Message = "The vehicle name is already exist.";

                return returnObject;
            }
            var result = await _unitOfWork.Complete();

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
    }
}
