using DBRepository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentPortalDTO.Models;
using StudentPortalDTO.ViewModels;
using System.Collections.Generic;
using WebApi.ViewModels;

namespace UnitTestStudentPortal.Controllers
{
    [TestClass]
    public class StudentControllerTest : BaseTest
    {

        [TestMethod]
        public void Get()
        {
            var list = studentController.Get().Result as List<StudentViewModel>;
            Assert.AreEqual(list.Count, 1);
            int qw = 1;
        }

        [DataTestMethod]
        [DataRow("Ivan", "Ivanov", "Ivanovich", "Nick123", true)]
        public void AddStudent(string firstName, string lastName, string patronymic, string nick, bool isMale)
        {
            var student = studentController.AddStudent(new StudentViewModel()
            {
                FirstName = firstName,
                LastName = lastName,
                Patronymic = patronymic,
                isMale = isMale,
                Nick = nick,
                userId = testUser.Id
            }).Result;
            var list = studentController.GetStudents(0, 0, "Ivan", null, null, null).Result;
            var okResult = list as OkObjectResult;
            var value = okResult.Value as Page<StudentViewModel>;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreNotEqual(value.Records.Count, 0);
        }



        [DataTestMethod]
        [DataRow("Ivan", "Ivanov", "Ivanovich", "Nick123", true)]
        public void DeleteStudent(string firstName, string lastName, string patronymic, string nick, bool isMale)
        {
            var student = studentController.AddStudent(new StudentViewModel()
            {
                FirstName = firstName,
                LastName = lastName,
                Patronymic = patronymic,
                isMale = isMale,
                Nick = nick,
                userId = testUser.Id
            }).Result;

            var okResult = student as OkObjectResult;
            var value = okResult.Value as StudentViewModel;
            Assert.AreEqual(200, okResult.StatusCode);
            var list = studentController.Get().Result as List<StudentViewModel>;
            Assert.AreNotEqual(list.Count, 0);
            studentController.DeleteStudent(value.Id).Wait();
            var _student = studentController.GetStudentById(value.Id).Result;
            Assert.IsNull(_student);
        }

        [DataTestMethod]
        [DataRow("Ivan", "Ivanov", "Ivanovich", "Nick123", true)]
        public void UpdateStudent(string firstName, string lastName, string patronymic, string nick, bool isMale)
        {
            var student = studentController.AddStudent(new StudentViewModel()
            {
                FirstName = firstName,
                LastName = lastName,
                Patronymic = patronymic,
                isMale = isMale,
                Nick = nick,
                userId = testUser.Id
            }).Result;
            var list = studentController.Get().Result as List<StudentViewModel>;
            Assert.AreEqual(list.Count, 2);
            firstName = "Elena";
            lastName = "Elenova";
            patronymic = "Elenovna";
            nick = "Elena123";
            isMale = false;
            var okResult = student as OkObjectResult;
            var value = okResult.Value as StudentViewModel;
            Assert.AreEqual(200, okResult.StatusCode);

            value.FirstName = firstName;
            value.LastName = lastName;
            value.Patronymic = patronymic;
            value.Nick = nick;
            value.isMale = isMale;
            studentController.UpdateStudent(value).Wait();
            var _student = studentController.GetStudentById(value.Id).Result;
            Assert.AreEqual(_student.FirstName, firstName);
            Assert.AreEqual(_student.LastName, lastName);
            Assert.AreEqual(_student.Patronymic, patronymic);
            Assert.AreEqual(_student.Nick, nick);
            Assert.AreEqual(_student.isMale, isMale);
            Assert.AreEqual(_student.Id, value.Id);
        }

        [TestMethod]
        public void AddGroup()
        {
            var group = studentController.AddGroup(new GroupViewModel() { Name = "Group1" }).Result;
            var list = serviceStudentPortal.GetAllGroups().Result as List<GroupViewModel>;
            Assert.AreEqual(list.Count, 1);
            Assert.IsNotNull(group);
        }

        [TestMethod]
        public void DeleteGroup()
        {
            var group = studentController.AddGroup(new GroupViewModel() { Name = "Group1" }).Result;
            var list = serviceStudentPortal.GetAllGroups().Result as List<GroupViewModel>;
            Assert.AreEqual(list.Count, 1);
            var okResult = group as OkObjectResult;
            var value = okResult.Value as GroupViewModel;
            Assert.AreEqual(200, okResult.StatusCode);


            studentController.DeleteGroup(value.Id).Wait();
            var list2 = serviceStudentPortal.GetAllGroups().Result as List<GroupViewModel>;
            Assert.AreEqual(list2.Count, 0);
        }

        [DataTestMethod]
        [DataRow("Group123")]
        public void UpdateGroup(string name)
        {
            var group = studentController.AddGroup(new GroupViewModel()
            {
                Name = name
            }).Result;
            var list = serviceStudentPortal.GetAllGroups().Result as List<GroupViewModel>;
            Assert.AreNotEqual(list.Count, 0);
            name = "Group321";

            var okResult = group as OkObjectResult;
            var value = okResult.Value as GroupViewModel;
            Assert.AreEqual(200, okResult.StatusCode);

            value.Name = name;
            studentController.UpdateGroup(value).Wait();
            var _group = studentController.GetGroupById(value.Id).Result;
            Assert.AreEqual(_group.Name, name);
            Assert.AreEqual(_group.Id, value.Id);
        }

        [TestMethod]
        public void AddGroupToStudent()
        {
            string login = "user2";
            string pass = "user2";
            AuthenticateViewModel user = serviceIdentity.GetUser(login, pass).Result;
            if (user == null) user = serviceIdentity.Create(new StudentPortalDTO.Models.AuthenticateModel() { Login = login, Password = pass }).Result;
            Assert.AreNotEqual(user.UserId, 0);
            var group = studentController.AddGroup(new GroupViewModel()
            {
                Name = "Group1"
            }).Result;
            var group1 = studentController.AddGroup(new GroupViewModel()
            {
                Name = "Group1"
            }).Result;
            var group2 = studentController.AddGroup(new GroupViewModel()
            {
                Name = "Group2"
            }).Result;
            var result = studentController.AddStudent(new StudentViewModel()
            {
                FirstName = "Sidor",
                LastName = "Sidorov",
                Patronymic = "Sidorovich",
                isMale = true,
                Nick = "Sidor123",
                userId = user.UserId
            }).Result;
            var okResult = group1 as OkObjectResult;
            var value1 = okResult.Value as GroupViewModel;
            Assert.AreEqual(200, okResult.StatusCode);

            var okResult2 = group2 as OkObjectResult;
            var value2 = okResult2.Value as GroupViewModel;
            Assert.AreEqual(200, okResult2.StatusCode);

            var okResult3 = result as OkObjectResult;
            var value3 = okResult3.Value as StudentViewModel;
            Assert.AreEqual(200, okResult3.StatusCode);



            studentController.AddGroupToStudent(new GroupStudentViewModel() { GroupIds = new int[] { value1.Id, value2.Id }, StudentId = value3.Id }).Wait();
            var listAll = serviceStudentPortal.GetAllGroups().Result as List<GroupViewModel>;
            StudentViewModel student = serviceStudentPortal.GetStudentById(value3.Id).Result;
            Assert.AreEqual(listAll.Count, 3);
            Assert.AreNotEqual(student.Groups.Count, 0);
        }
    }
}
