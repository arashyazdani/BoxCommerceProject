using Domain.Entities;
using Domain.Specifications.ProductSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductService
    {
        Task<GetObjectFromProductService> CreateProduct(Product createProductParams);
        Task<GetObjectFromProductService> UpdateProduct(Product updateProductParams);
    }
}
