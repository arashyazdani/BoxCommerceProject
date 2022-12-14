using API.Errors;
using AutoMapper;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using System.Net.Http;
using API.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Domain.Specifications;
using System.Net.Sockets;
using Stripe;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace API.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ApiResponse> _apiResponse;

        public CategoriesControllerTests()
        {
            _apiResponse = new Mock<ApiResponse>();
            _mapper = new Mock<IMapper>();
            _unitOfWork = new Mock<IUnitOfWork>();

        }

        [Fact]
        public async Task Get_OK_ObjectResult_CategoryById()
        {
            //Arrange
            Category newCategory = CreateTestCategory();

            var spec = new GetCategoriesWithParentsSpecification(1);
            //_unitOfWork.Setup(x => x.Repository<Category>().GetEntityWithSpec(spec)).ReturnsAsync(newCategory)
            //    .Verifiable();
            _unitOfWork.Setup(x => x.Repository<Category>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Category>>()))
                .ReturnsAsync(newCategory)
                .Verifiable();

            //Act

            var result2 = await _unitOfWork.Object.Repository<Category>().GetEntityWithSpec(spec);

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object);
            
            // Act

            var result = await controller.GetCategoryById(1);

            // Assert

            result.Value?.Id.ShouldBe(newCategory.Id);
            result.Value?.Name.ShouldBeEquivalentTo(newCategory.Name);
            result.Value?.Name.ShouldBeEquivalentTo("newCategory.Name");
            result.Result.ShouldBeOfType<OkObjectResult>();
            
        }

        [Fact]
        public async Task Get_Category_By_Id_Should_Not_Be_Null()
        {
            //Arrange
            Category newCategory = CreateTestCategory();

            var spec = new GetCategoriesWithParentsSpecification(1);
            _unitOfWork.Setup(x => x.Repository<Category>().GetEntityWithSpec(spec)).ReturnsAsync(newCategory)
                .Verifiable();

            //Act

            var result = await _unitOfWork.Object.Repository<Category>().GetEntityWithSpec(spec);


            //Assert

            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
            result.Priority.ShouldBe(1);
            result.Name.ShouldBeEquivalentTo(newCategory.Name);
        }

        private Category CreateTestCategory()
        {
            return new Category()
            {
                Id = 1,
                Priority = 1,
                Name = "Test Category",
                Enabled = true,
                Details = "Testing category data"
            };
        }
    }
}
