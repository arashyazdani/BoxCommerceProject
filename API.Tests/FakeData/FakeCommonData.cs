using Domain.Specifications;
using Domain.Specifications.CategorySpecifications;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.FakeData
{
    public static class FakeCommonData<T> where T : class
    {
        public static GetObjectFromServicesSpecification FakeServiceObject(int statusCode, string message, dynamic? resultObject)
        {
            var returnObject = new GetObjectFromServicesSpecification
            {
                StatusCode = statusCode,
                Message = message,
                ResultObject = resultObject
            };

            return returnObject;
        }

        public static ExpandoObject CreateFormatException()
        {

            dynamic specParams = new ExpandoObject();

            specParams.PageIndex = int.Parse("No Integer");

            return specParams;

        }
    }
}
