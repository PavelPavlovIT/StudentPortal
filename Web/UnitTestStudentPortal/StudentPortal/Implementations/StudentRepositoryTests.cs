using DBRepository.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentPortalDTO.ViewModels;
using System.Collections.Generic;
using UnitTestStudentPortal;


namespace DBRepository.StudentPortal.Implementations.Tests
{
    [TestClass()]
    public class StudentRepositoryTests : BaseTest
    {
        [TestMethod()]
        public void StudentRepository()
        {
            Assert.IsNotNull(studentRepository);
        }

        [DataTestMethod]
        [DataRow("Test group")]
        public void AddGroup(string groupName)
        {
            var group = studentRepository.AddGroup(new Group() { GroupName = groupName }).Result;
            Assert.AreNotEqual(group.Id, 0);
            Assert.AreEqual(group.GroupName, groupName);
        }

        [DataTestMethod]
        [DataRow("Test group")]
        public void AddGroupToStudent(string groupName)
        {
            string login = "user2";
            string pass = "user2";
            AuthenticateViewModel user = serviceIdentity.GetUser(login, pass).Result;
            if (user == null) user = serviceIdentity.Create(new StudentPortalDTO.Models.AuthenticateModel() { Login = login, Password = pass }).Result;
            Assert.AreNotEqual(user.UserId, 0);

            var group = studentRepository.AddGroup(new Group()
            {
                GroupName = "Group1"
            }).Result;
            var group1 = studentRepository.AddGroup(new Group()
            {
                GroupName = "Group1"
            }).Result;
            var group2 = studentRepository.AddGroup(new Group()
            {
                GroupName = "Group2"
            }).Result;
            Student student = studentRepository.AddStudent(user.UserId, new Student()
            {
                FirstName = "Sidor",
                LastName = "Sidorov",
                Patronymic = "Sidorovich",
                isMale = true,
                Nick = "Sidor123",
            }).Result;



            studentRepository.AddGroupToStudent(new int[] { group1.Id, group2.Id }, student.Id).Wait();
            var listAll = studentRepository.GetGroups().Result as List<Group>;
            student = studentRepository.GetStudentById(student.Id).Result;
            Assert.AreEqual(listAll.Count >= 3, true);
            Assert.AreEqual(student.GroupStudents.Count, 2);
        }

        [DataTestMethod]
        [DataRow("Delete group")]
        public void DeleteGroup(string groupName)
        {
            var group2 = studentRepository.AddGroup(new Group()
            {
                GroupName = "Group2"
            }).Result;
            var list1 = studentRepository.GetGroups().Result;
            studentRepository.DeleteGroup(group2.Id).Wait();
            var list2 = studentRepository.GetGroups().Result;
            Assert.AreEqual(list2.Count < list1.Count, true);
        }


        [DataTestMethod]
        [DataRow("Ivan", "Ivanov", "Ivanovich", "Ivan---")]
        public void GetStudents(string firstName, string lastName, string patronymic, string nick)
        {
            int PageSize = 10;
            for (int i = 0; i < 10; i++)
            {
                studentRepository.AddStudent(testUser.Id, new Student() { FirstName = firstName, LastName = lastName, Patronymic = patronymic, isMale = true, Nick = nick + i.ToString() });
            }
            for (int i = 0; i < 10; i++)
            {
                studentRepository.AddStudent(testUser.Id, new Student() { FirstName = firstName, LastName = lastName, Patronymic = patronymic, isMale = false });
            }
            List<Student> list = studentRepository.GetStudents().Result as List<Student>;
            Assert.AreEqual(list.Count, 21);
            var page = studentRepository.GetStudents(0, 10, 0, null, null, null, null).Result;
            Assert.AreEqual(page.CurrentPage, 0);
            Assert.AreEqual(page.TotalRecords, 21);
            Assert.AreEqual(page.Records.Count, 10);
            //filter = test
            var page2 = studentRepository.GetStudents(0, 10, 0, "test", null, null, null).Result;
            Assert.AreEqual(page2.CurrentPage, 0);
            Assert.AreEqual(page2.Records.Count, 0);
            //check ismale=true
            var page3 = studentRepository.GetStudents(0, 10, 0, null, true, null, null).Result;
            Assert.AreEqual(page3.CurrentPage, 0);
            Assert.AreEqual(page3.Records.Count, 10);
            var page3_1 = studentRepository.GetStudents(1, 10, 0, null, true, null, null).Result;
            Assert.AreEqual(page3_1.CurrentPage, 1);
            Assert.AreEqual(page3_1.Records.Count, 1);
            //order by nick desc = false
            var page4 = studentRepository.GetStudents(0, 10, 0, null, null, "isMale", false).Result;
            Assert.AreEqual(page4.CurrentPage, 0);
            //filter = name it equals only testUser
            var page5 = studentRepository.GetStudents(0, 10, 0, "name", null, null, null).Result;
            Assert.AreEqual(page5.CurrentPage, 0);
            Assert.AreEqual(page5.Records.Count, 1);

            //int index, int pageSize, int id, string filter, bool? isMale, string orderby, bool? desc
        }
    }
}