﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public interface ISpecification<T> 
    {
        Expression<Func<T, bool>> Filter { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, Object>> OrderBy { get; }
        Expression<Func<T, Object>> GroupBy { get; }
        Expression<Func<T, Object>> OrderByDescending { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
