using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentPortalDTO.Models;
using StudentPortalDTO.ViewModels;
using System;

namespace UnitTestStudentPortal.Controllers
{
    [TestClass]
    public class IdentityControllerTest : BaseTest
    {
        public IdentityControllerTest()
        {
            Assert.IsNotNull(identityController);
        }

        [TestMethod]
        public void Authenticate()
        {
            string login = Guid.NewGuid().ToString();
            string pass = Guid.NewGuid().ToString();
            IActionResult actionResult = identityController.Register(new AuthenticateModel() { Login = login, Password = pass }).Result;
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            actionResult = identityController.Authenticate(new AuthenticateModel() { Login = login, Password = pass }).Result;
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public void Register()
        {
            string login = Guid.NewGuid().ToString();
            string pass = Guid.NewGuid().ToString();
            IActionResult actionResult = identityController.Register(new AuthenticateModel() { Login = login, Password = pass }).Result;
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            
        }
        [TestMethod]
        public void GetById()
        {
            IActionResult actionResult = identityController.GetById(testUser.Id).Result;
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }
        [TestMethod]
        public void SetStudent()
        {
            identityController.SetStudent(new WebApi.ViewModels.SetStudentModel() { StudentId = testStudent.Id, UserId = testUser.Id }).Wait();
        }

        [TestMethod]
        public void Delete()
        {
            string login = "userRemove";
            string pass = "userRemove";
            IActionResult actionResult = identityController.Register(new AuthenticateModel() { Login = login, Password = pass }).Result;
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            IActionResult actionResult1 = identityController.Authenticate(new AuthenticateModel() { Login = login, Password = pass }).Result;
            var okResult = actionResult1 as OkObjectResult;
            AuthenticateViewModel model = okResult.Value as AuthenticateViewModel;
            identityController.Delete(model.UserId).Wait();
            IActionResult actionResult2 = identityController.Authenticate(new AuthenticateModel() { Login = login, Password = pass }).Result;
            Assert.IsNotInstanceOfType(actionResult2, typeof(OkObjectResult));
        }
    }
}
