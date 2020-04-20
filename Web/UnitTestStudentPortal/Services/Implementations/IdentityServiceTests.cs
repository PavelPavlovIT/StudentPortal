using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentPortalDTO.Models;
using StudentPortalDTO.ViewModels;
using System;
using UnitTestStudentPortal;

namespace WebApi.Services.Implementations.Tests
{
    [TestClass()]
    public class IdentityServiceTests : BaseTest
    {

        [TestMethod()]
        public void IdentityServiceTest()
        {
            Assert.IsNotNull(serviceIdentity);
        }

        [TestMethod()]
        public void AuthenticateTest()
        {
            string login = Guid.NewGuid().ToString();
            string pass = Guid.NewGuid().ToString();
            AuthenticateViewModel user = serviceIdentity.Create(new AuthenticateModel() { Login = login, Password = pass }).Result;
            Assert.IsNotNull(user);
            user = serviceIdentity.Authenticate(new AuthenticateModel() { Login = login, Password = pass }).Result;
            Assert.IsNotNull(user);
        }

        [TestMethod()]
        public void GetByIdTest()
        {
            var user = serviceIdentity.GetById(testUser.Id).Result;
            Assert.IsNotNull(user);
        }

        [TestMethod()]
        public void Create()
        {
            string login = Guid.NewGuid().ToString();
            string pass = Guid.NewGuid().ToString();
            var user = serviceIdentity.Create(new AuthenticateModel() { Login = login, Password = pass }).Result;
            Assert.IsNotNull(user);
        }

        [TestMethod()]
        public void GetUserTest()
        {
            string login = "user";
            string pass = "user";
            var user = serviceIdentity.GetUser(login, pass).Result;
            Assert.IsNotNull(user);

        }

        [TestMethod()]
        public void DeleteTest()
        {
            string login = Guid.NewGuid().ToString();
            string pass = Guid.NewGuid().ToString();
            var user = serviceIdentity.Create(new AuthenticateModel() { Login = login, Password = pass }).Result;
            var _user1 = serviceIdentity.GetUser(login, pass).Result;
            Assert.IsNotNull(user);
            serviceIdentity.Delete(_user1.UserId);
            var _user2 = serviceIdentity.GetUser(login, pass).Result;
            Assert.IsNull(_user2);
        }

        [TestMethod()]
        public void SetStudentTest()
        {
            int studentid = 7;
            string login = Guid.NewGuid().ToString();
            string pass = Guid.NewGuid().ToString();
            var user = serviceIdentity.Create(new AuthenticateModel() { Login = login, Password = pass }).Result;
            var user1 = serviceIdentity.GetUser(login, pass).Result;
            Assert.IsNotNull(user1);
            Assert.AreEqual(user1.StudentId, 0);
            serviceIdentity.SetStudent(user1.UserId, studentid).Wait();
            var user2 = serviceIdentity.GetUser(login, pass).Result;
            Assert.AreEqual(user2.StudentId, studentid);
        }
    }
}