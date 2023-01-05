using Bogus;
using Domain.Entities;
using Domain.Specifications.VehicleSpecifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.FakeData
{
    public static class FakeVehicles<T> where T : class
    {
        private static int _id = 1;
        private static int _vehiclePartsNum = 3;
        private static Faker _faker = new Faker();

        public static T FakeVehicleData(int? parentId, T obj)
        {
            dynamic returnData = obj;

            if (typeof(T) != typeof(CreateVehicleParams)) returnData.Id = 1;
            returnData.Priority = 1;
            returnData.Name = _faker.Company.CompanyName();
            returnData.Enabled = true;
            returnData.Details = _faker.Commerce.ProductDescription();
            if (parentId == -1) returnData.UpdatedDate = DateTimeOffset.Now;

            return returnData;
        }

        public static Vehicle FakeVehicleWithPartsData()
        {
            var fakeVehicle = FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle());

            _vehiclePartsNum = 3;

            List<VehiclesPart> vehiclesParts = FakeVehiclePartList.Generate(3);
            
            var fakeProductsList = FakeProducts<IList<Product>>.FakeProductList.Generate(3);

            fakeVehicle.VehiclesParts = vehiclesParts;

            foreach (var product in fakeProductsList)
            {
                product.VehiclesParts = vehiclesParts;
            }

            _id = 1;

            foreach (var vehiclesPart in vehiclesParts)
            {
                vehiclesPart.VehicleId = 1;
                vehiclesPart.Product = fakeProductsList.FirstOrDefault(c => c.Id == _id)!;
                vehiclesPart.Vehicle = fakeVehicle;
                _id++;
            }
            return fakeVehicle;

        }

        public static List<Vehicle> FakeVehicleWithPartsDataList()
        {
            
            var fakeProductsList = FakeProducts<IList<Product>>.FakeProductList.Generate(3);

            var fakeVehiclesList = FakeVehicleList.Generate(3);


            List<VehiclesPart> vehiclesParts = FakeVehiclePartList.Generate(9);

            foreach (var vehicle in fakeVehiclesList)
            {
                vehicle.VehiclesParts = vehiclesParts;
            }

            foreach (var product in fakeProductsList)
            {
                product.VehiclesParts = vehiclesParts;
            }

            _id = 1;

            _vehiclePartsNum = 1;

            foreach (var vehiclesPart in vehiclesParts)
            {
                vehiclesPart.VehicleId = _vehiclePartsNum;
                vehiclesPart.ProductId = _id;

                vehiclesPart.Product = fakeProductsList.FirstOrDefault(c => c.Id == _id)!;

                vehiclesPart.Vehicle = fakeVehiclesList.FirstOrDefault(c => c.Id == _vehiclePartsNum)!;

                if (_id % 3 == 0)
                {
                    _vehiclePartsNum++;
                    _id = 0;
                }
                _id++;
            }
            return fakeVehiclesList;
        }

        public static Faker<Vehicle> FakeVehicleList { get; } =
            new Faker<Vehicle>()
                .RuleFor(p => p.Id, f => _id++)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Details, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Priority, f => f.Commerce.Random.Int(1, _id))
                .RuleFor(p => p.Enabled, f => true)
                .RuleFor(p=>p.VehiclesParts,f=> FakeVehiclePartList!.Generate(_vehiclePartsNum));


        public static Faker<VehiclesPart> FakeVehiclePartList { get; } =
            new Faker<VehiclesPart>()
                .RuleFor(p => p.VehicleId, f => _id)
                .RuleFor(p => p.ProductId, f => _id);
    }
}
