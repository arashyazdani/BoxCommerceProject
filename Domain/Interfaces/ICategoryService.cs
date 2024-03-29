﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications;
using Domain.Specifications.CategorySpecifications;

namespace Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<GetObjectFromServicesSpecification> CreateCategory(Category createCategoryParams, CancellationToken cancellationToken = default(CancellationToken));
        Task<GetObjectFromServicesSpecification> UpdateCategory(Category updateCategoryParams, CancellationToken cancellationToken = default(CancellationToken));

    }
}
