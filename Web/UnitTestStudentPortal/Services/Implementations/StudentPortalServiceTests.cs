using DBRepository.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentPortalDTO.ViewModels;
using System.Collections.Generic;
using System.Linq;
using UnitTestStudentPortal;

namespace WebApi.Services.Implementations.Tests
{
    [TestClass()]
    public class StudentPortalServiceTests : BaseTest
    {

        [TestMethod()]
        public void StudentPortalService()
        {
            Assert.IsNotNull(serviceStudentPortal);
        }

        [DataTestMethod]
        [DataRow("Ivan", "Ivanov", "Ivanovich", "Nick123", true)]
        public void AddStudent(string firstName, string lastName, string patronymic, string nick, bool isMale)
        {
            var group = serviceStudentPortal.AddGroup(new GroupViewModel() { Name = "Group" }).Result;
            var _student = new StudentViewModel() { FirstName = firstName, LastName = lastName, Patronymic = patronymic, Nick = nick, isMale = isMale };
            var student2 = serviceStudentPortal.AddStudent(testUser.Id, _student).Result;
            serviceStudentPortal.AddGroupToStudent(new int[] { group.Id }, student2.Id);
            var student = serviceStudentPortal.GetStudentById(student2.Id).Result;
            Assert.AreNotEqual(student.Id, 0);
            Assert.AreNotEqual(student.Groups, 0);
            Assert.AreEqual(student.FirstName, firstName);
            Assert.AreEqual(student.LastName, lastName);
            Assert.AreEqual(student.Patronymic, patronymic);
            Assert.AreEqual(student.Nick, nick);
            Assert.AreEqual(student.isMale, isMale);
        }

        [TestMethod()]
        public void AddGroup()
        {
            var group = serviceStudentPortal.AddGroup(new GroupViewModel() { Name = "Group" }).Result;
            Assert.AreNotEqual(group.Id, 0);
        }

        [TestMethod()]
        public void DeleteStudent()
        {
            var student = serviceStudentPortal.AddStudent(testUser.Id, new StudentViewModel() { FirstName = "Ivanov", LastName = "Ivan", Patronymic = "Ivanovich", Nick = "Ivan123", isMale = true }).Result;
            int id = student.Id;
            Assert.AreNotEqual(id, 0);
            serviceStudentPortal.DeleteStudent(id).Wait();
            var _student = serviceStudentPortal.GetStudentById(id).Result;
            Assert.IsNull(_student);
        }

        [DataTestMethod]
        [DataRow("Ivan", "Ivanov", "Ivanovich", "Nick123", true)]
        public void GetAllStudents(string firstName, string lastName, string patronymic, string nick, bool isMale)
        {
            List<StudentViewModel> students = serviceStudentPortal.GetAllStudents().Result as List<StudentViewModel>;
            if (students.Count == 0)
            {
                AddStudent(firstName, lastName, patronymic, nick, isMale);
            }
            students = serviceStudentPortal.GetAllStudents().Result as List<StudentViewModel>;
            Assert.AreNotEqual(students.Count, 0);
        }

        [TestMethod()]
        public void GetStudents()
        {
            AddStudent("123", "123", "123", null, true);
            Page<StudentViewModel> students = serviceStudentPortal.GetStudents(0, 0, null, null, null, null).Result;
            Assert.AreNotEqual(students.TotalRecords, 0);

        }

        [TestMethod()]
        public void UpdateStudent()
        {
            var student = serviceStudentPortal.AddStudent(testUser.Id, new StudentViewModel() { FirstName = "Ivanov", LastName = "Ivan", Patronymic = "Ivanovich", Nick = "Ivan123", isMale = true }).Result;
            string firstName = student.FirstName;
            student.FirstName = "UpdateFirstName1";
            serviceStudentPortal.UpdateStudent(student).Wait();
            var _student = serviceStudentPortal.GetStudentById(student.Id).Result;
            _student.FirstName = "UpdateFirstName2";
            serviceStudentPortal.UpdateStudent(_student).Wait();
            var _student2 = serviceStudentPortal.GetStudentById(_student.Id).Result;
            Assert.AreNotEqual(firstName, _student2.FirstName);
        }

        [TestMethod()]
        public void GetAllGroups()
        {
            Page<GroupViewModel> groups = serviceStudentPortal.GetGroups(0).Result;
            if (groups.TotalRecords == 0)
            {
                AddGroup();
            }
            groups = serviceStudentPortal.GetGroups(0).Result;
            Assert.AreNotEqual(groups.Records.Count, 0);
        }

        [TestMethod()]
        public void DeleteGroup()
        {
            var group = serviceStudentPortal.AddGroup(new GroupViewModel() { Name = "test" }).Result;
            int id = group.Id;
            Assert.AreNotEqual(id, 0);
            serviceStudentPortal.DeleteGroup(id).Wait();
            var _group = serviceStudentPortal.GetGroupById(id).Result;
            Assert.IsNull(_group);
        }

        [TestMethod()]
        public void UpdateGroup()
        {
            var group = serviceStudentPortal.AddGroup(new GroupViewModel() { Name = "groupName" }).Result;
            string groupName = group.Name;
            group.Name = "UpdateName1";
            serviceStudentPortal.UpdateGroup(group).Wait();
            var _group = serviceStudentPortal.GetGroupById(group.Id).Result;
            _group.Name = "UpdateName2";
            serviceStudentPortal.UpdateGroup(_group).Wait();
            var _group2 = serviceStudentPortal.GetGroupById(_group.Id).Result;
            Assert.AreNotEqual(groupName, _group2.Name);
        }
    }
}