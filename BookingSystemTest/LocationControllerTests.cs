using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using BookingSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BookingSystem.Controllers;
using BookingSystem.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystemTest
{
  public class FakeDbSet<T> : DbSet<T> where T : class
  {
    public override IEntityType EntityType { get; }
  }
  [TestClass]
  public class LocationControllerTests
  {

    [TestMethod]
    public async Task GetAllDesks()
    {
      var controller = CreateInstance();
      var actionResult = await controller.GetDeskModel();
      var result = actionResult.Result as OkObjectResult;
      Assert.IsNotNull(result);
      var valueResult = result.Value as List<DeskModel>;
      Assert.IsNotNull(valueResult);
      Assert.AreEqual(4, valueResult.Count());
    }
    [TestMethod]
    public async Task GetDesk()
    {
      var controller = CreateInstance();
      var actionResult = await controller.GetDeskModel(1);
      var result = actionResult.Result as OkObjectResult;
      Assert.IsNotNull(result);
      var valueResult = result.Value as DeskModel;
      Assert.IsNotNull(valueResult);
      var expectedResult = new DeskModel()
      {
        Id = 1,
        DeskStatus = "available",
        LocationID = 1
      };
      Assert.AreEqual(expectedResult, valueResult);
    }
    [TestMethod]
    public async Task UpdateDesk()
    {
      var controller = CreateInstance();
      var actionResult = await controller.PutDeskModel(1, "unavailable");
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }
    [TestMethod]
    public async Task UpdateBookedDesk()
    {
      var controller = CreateInstance();
      var actionResult = await controller.PutDeskModel(2, "available");
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    [TestMethod]
    public async Task DeleteDesk()
    {
      var controller = CreateInstance();
      var actionResult = await controller.DeleteDeskModel(1);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }
    [TestMethod]
    public async Task DeleteBookedDesk()
    {
      var controller = CreateInstance();
      var actionResult = await controller.DeleteDeskModel(2);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
    }

    private DeskModelsController CreateInstance()
    {
      var desks = new List<DeskModel>
      {
        new DeskModel()
        {
          Id = 1,
          DeskStatus = "available",
          LocationID = 1
        },
        new DeskModel()
        {
          Id = 2,
          DeskStatus = "unavailable",
          LocationID = 1
        },
        new DeskModel()
        {
          Id = 3,
          DeskStatus = "unavailable",
          LocationID = 1
        },
        new DeskModel()
        {
          Id = 4,
          DeskStatus = "available",
          LocationID = 1
        }

      }.AsQueryable();

      var reservations = new List<ReservationModel>
      {
        new ReservationModel()
        {
          Id=1,
          BookingBeginDate = new System.DateTime(2024,9,6,10,0,0),
          BookignEndDate = new System.DateTime(2024,9,8,10,0,0),
          DeskId = 2,
          UserId = 1
        }
      }.AsQueryable();

      var mockSetDesks = new Mock<DbSet<DeskModel>>();
      mockSetDesks.As<IQueryable<DeskModel>>().Setup(m => m.Provider).Returns(desks.Provider);
      mockSetDesks.As<IQueryable<DeskModel>>().Setup(m => m.Expression).Returns(desks.Expression);
      mockSetDesks.As<IQueryable<DeskModel>>().Setup(m => m.ElementType).Returns(desks.ElementType);
      mockSetDesks.As<IQueryable<DeskModel>>().Setup(m => m.GetEnumerator()).Returns(desks.GetEnumerator());

      var mockSetReservations = new Mock<DbSet<ReservationModel>>();
      mockSetReservations.As<IQueryable<ReservationModel>>().Setup(m => m.Provider).Returns(reservations.Provider);
      mockSetReservations.As<IQueryable<ReservationModel>>().Setup(m => m.Expression).Returns(reservations.Expression);
      mockSetReservations.As<IQueryable<ReservationModel>>().Setup(m => m.ElementType).Returns(reservations.ElementType);
      mockSetReservations.As<IQueryable<ReservationModel>>().Setup(m => m.GetEnumerator()).Returns(reservations.GetEnumerator());

      var mockContext = new Mock<BookingSystemContext>();
      mockContext.Setup(c => c.DeskModel).Returns(mockSetDesks.Object);
      mockContext.Setup(c => c.ReservationModel).Returns(mockSetReservations.Object);
      mockContext.Setup(c => c.Set<DeskModel>()).Returns(mockSetDesks.Object);

      var controller = new DeskModelsController(mockContext.Object);

      return controller;
    }
  }
}