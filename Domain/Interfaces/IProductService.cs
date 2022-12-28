using Domain.Entities;
using Domain.Specifications.ProductSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Specifications;

namespace Domain.Interfaces
{
    public interface IProductService
    {
        Task<GetObjectFromServicesSpecification> CreateProduct(Product createProductParams);
        Task<GetObjectFromServicesSpecification> UpdateProduct(Product updateProductParams);
    }
}
