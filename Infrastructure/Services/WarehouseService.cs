﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.Specifications.WarehouseSpecifications;

namespace Infrastructure.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarehouseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetObjectFromServicesSpecification> CreateWarehouse(Warehouse createWarehouseParams, CancellationToken cancellationToken = default(CancellationToken))
        {
            var returnObject = new GetObjectFromServicesSpecification();

            var specParams = new GetWarehouseSpecificationParams();

            specParams.Search = createWarehouseParams.Name;

            var spec = new GetWarehousesSpecification(specParams);

            var warehouseExist = await _unitOfWork.Repository<Warehouse>().GetEntityWithSpec(spec, cancellationToken);

            if (warehouseExist != null)
            {
                returnObject.StatusCode = 409;

                returnObject.Message = "The warehouse name is already exist.";
                return returnObject;
            }

            await _unitOfWork.Repository<Warehouse>().InsertAsync(createWarehouseParams, cancellationToken);

            var result = await _unitOfWork.Complete(cancellationToken);

            if (result <= 0)
            {
                returnObject.StatusCode = 400;

                returnObject.Message = "Bad request.";

                return returnObject;
            }

            returnObject.StatusCode = 201;

            returnObject.Message = "Warehouse has been created successfully.";

            returnObject.ResultObject = createWarehouseParams;

            return returnObject;
        }

        public async Task<GetObjectFromServicesSpecification> UpdateWarehouse(Warehouse updateWarehouseParams, CancellationToken cancellationToken = default(CancellationToken))
        {
            var returnObject = new GetObjectFromServicesSpecification();

            var specParams = new GetWarehouseSpecificationParams();
            var currentTimestamp = DateTimeOffset.Now;
            if (updateWarehouseParams.UpdatedDate != null) currentTimestamp = (DateTimeOffset)updateWarehouseParams.UpdatedDate;
            specParams.Search = updateWarehouseParams.Name;

            var spec = new GetWarehousesSpecification(specParams);

            var warehouseExist = await _unitOfWork.Repository<Warehouse>().GetEntityWithSpec(spec, cancellationToken);

            if (warehouseExist != null && warehouseExist.Id != updateWarehouseParams.Id)
            {
                returnObject.StatusCode = 409;

                returnObject.Message = "The warehouse name is already exist.";

                return returnObject;
            }
            var result = await _unitOfWork.Complete(cancellationToken);

            if (result <= 0 && updateWarehouseParams.UpdatedDate == currentTimestamp)
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

            returnObject.Message = "Warehouse has been updated successfully.";

            returnObject.ResultObject = updateWarehouseParams;

            return returnObject;
        }
    }
}
