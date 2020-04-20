using AutoMapper;
using DBRepository.Models;
using DBRepository.StudentPortal.Implementations;
using DBRepository.StudentPortal.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentPortalDTO.Models;
using StudentPortalDTO.Services.Implementations;
using StudentPortalDTO.Services.Interfaces;
using StudentPortalDTO.ViewModels;
using System;
using System.Collections.Generic;
using WebApi.Controllers;

namespace UnitTestStudentPortal
{
    public class BaseTest
    {
        protected static User testUser = new User { Login = "user", Password = "user" };
        protected static Student testStudent = new Student() { FirstName = "Firstname", LastName = "Lastname", Patronymic = "Patronymic", Nick = "NickName", isMale = true };

        static ServiceProvider serviceProvider;
        protected static IStudentPortalService serviceStudentPortal;
        protected static IIdentityService serviceIdentity;
        protected static IStudentRepository studentRepository;
        protected static IIdentityRepository identityRepository;
        protected static StudentController studentController;
        protected static IdentityController identityController;
        object lockObject = new object();
        public BaseTest()
        {
            lock (lockObject)
            {
                if (serviceStudentPortal == null)
                {
                    IServiceCollection services = new ServiceCollection();
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                    services.AddSingleton<IIdentityService, IdentityService>();
                    services.AddSingleton<IRepositoryContextFactory, RepositoryContextFactory>();
                    services.AddSingleton<IIdentityRepository>(provider => new IdentityRepository(null
                        , provider.GetService<IRepositoryContextFactory>()));
                    services.AddSingleton<IStudentRepository>(provider => new StudentRepository(null
                        , provider.GetService<IRepositoryContextFactory>()));
                    services.AddSingleton<IStudentPortalService, StudentPortalService>();


                    services.AddSingleton<IConfiguration>(Config.configuration);
                    serviceProvider = services.BuildServiceProvider();
                    serviceStudentPortal = (IStudentPortalService)serviceProvider.GetService(typeof(IStudentPortalService));
                    studentRepository = (IStudentRepository)serviceProvider.GetService(typeof(IStudentRepository));
                    identityRepository = (IIdentityRepository)serviceProvider.GetService(typeof(IIdentityRepository));
                    serviceIdentity = (IIdentityService)serviceProvider.GetService(typeof(IIdentityService));
                    var serviceMapper = (IMapper)serviceProvider.GetService(typeof(IMapper));
                    identityController = new IdentityController(Config.configuration, serviceIdentity, serviceMapper);
                    studentController = new StudentController(serviceStudentPortal, serviceMapper);

                    testUser = identityRepository.Add(testUser).Result;
                    testStudent = studentRepository.AddStudent(testUser.Id, testStudent).Result;

                }
            }
        }
    }
}
