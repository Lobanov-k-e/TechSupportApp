using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Infrastructure.Identity;

namespace TechSupportApp.Tests.Infrastructure
{
    [TestFixture]
    class IdentityServiceTests
    {
        [Test]
        public async Task GetDomainId_canGetId()
        {            
            const int ExpectedDomainId = 10;

            var userStoreMock = new Mock<IUserStore<AppIdentityUser>>();

            userStoreMock.Setup(m => m.FindByIdAsync(It.IsAny<string>(), new CancellationToken()))
                .ReturnsAsync(new AppIdentityUser { DomainId = ExpectedDomainId });

            var userManager = GetUserManager(userStoreMock);

            var service = GetService(userManager);

            var act = await service.GetDomainIdAsync(It.IsAny<string>());

            Assert.IsTrue(act.result.Succeeded);
            Assert.AreEqual(act.id, ExpectedDomainId);
        }

        [Test]
        public async Task GetDomainId_getFailResult_IfUserNotFound()
        {
            var userStoreMock = new Mock<IUserStore<AppIdentityUser>>();

            userStoreMock.Setup(m => m.FindByIdAsync(It.IsAny<string>(), new CancellationToken()))
                .ReturnsAsync((AppIdentityUser)null);

            var userManager = GetUserManager(userStoreMock);

            var service = GetService(userManager);


            var act = await service.GetDomainIdAsync(It.IsAny<string>());

            Assert.IsFalse(act.result.Succeeded);
        }

        [Test]
        public async Task Create_canCreateUser()
        {
            var user = new AppIdentityUser
            {
                UserName = "test",
                Email = "test@test.com",
                DomainId = 10
            };

            var userStoreMock = new Mock<IUserStore<AppIdentityUser>>();

            userStoreMock
                .Setup(m => m.CreateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()))
                .ReturnsAsync(IdentityResult.Success);

            userStoreMock
                .Setup(m => m.UpdateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()))
                .ReturnsAsync(IdentityResult.Success);            

            userStoreMock.As<IUserRoleStore<AppIdentityUser>>();           
            userStoreMock.As<IUserPasswordStore<AppIdentityUser>>();

            var service = GetService(userStoreMock);

            var act = await service.CreateAsync(user.UserName, user.Email, "testPass456%", user.DomainId);

            Assert.IsTrue(act.result.Succeeded);           
        }

        [Test]
        public async Task Create_GetFailResult_IfCantCreateUser()
        {
            var user = new AppIdentityUser
            {
                UserName = "test",
                Email = "test@test.com",
                DomainId = 10
            };

            const string Error = "test create identity error";

            var userStoreMock = new Mock<IUserStore<AppIdentityUser>>();

            userStoreMock
                .Setup(m => m.CreateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Description = Error}));

            userStoreMock
                .Setup(m => m.UpdateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()))
                .ReturnsAsync(IdentityResult.Success);

            userStoreMock.As<IUserRoleStore<AppIdentityUser>>();
            userStoreMock.As<IUserPasswordStore<AppIdentityUser>>();

            var service = GetService(userStoreMock);

            var act = await service.CreateAsync(user.UserName, user.Email, "testPass456%", user.DomainId);

            Assert.IsFalse(act.result.Succeeded);
            StringAssert.AreEqualIgnoringCase(Error, act.result.Errors.First());
        }

        [Test]
        public async Task Create_GetFailResult_IfCantAssignRole()
        {
            var user = new AppIdentityUser
            {
                UserName = "test",
                Email = "test@test.com",
                DomainId = 10
            };

            const string Error = "test assign role error";

            var userStoreMock = new Mock<IUserStore<AppIdentityUser>>();

            userStoreMock
                .Setup(m => m.CreateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()))
                .ReturnsAsync(IdentityResult.Success);

            userStoreMock
                .Setup(m => m.UpdateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Description = Error }));

            userStoreMock.As<IUserRoleStore<AppIdentityUser>>();
            userStoreMock.As<IUserPasswordStore<AppIdentityUser>>();

            var service = GetService(userStoreMock);

            var act = await service.CreateAsync(user.UserName, user.Email, "testPass456%", user.DomainId);

            Assert.IsFalse(act.result.Succeeded);
            StringAssert.AreEqualIgnoringCase(Error, act.result.Errors.First());
        }

        [Test]
        public async Task Create_Call_AddToRole_onCreate()
        {
            var user = new AppIdentityUser
            {
                UserName = "test",
                Email = "test@test.com",
                DomainId = 10
            };           


            var userStoreMock = new Mock<IUserStore<AppIdentityUser>>();

            userStoreMock
                .Setup(m => m.CreateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()))
                .ReturnsAsync(IdentityResult.Success);

            userStoreMock
                .Setup(m => m.UpdateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()))
                .ReturnsAsync(IdentityResult.Success);

            userStoreMock.As<IUserRoleStore<AppIdentityUser>>();
            userStoreMock.As<IUserPasswordStore<AppIdentityUser>>();

            var service = GetService(userStoreMock);

            var act = await service.CreateAsync(user.UserName, user.Email, "testPass456%", user.DomainId);

            Assert.IsTrue(act.result.Succeeded);
            userStoreMock.Verify(m => m.UpdateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()), Times.Once());
        }

        [Test]
        public async Task Create_notCall_AddToRole_onCreateError()
        {
            var user = new AppIdentityUser
            {
                UserName = "test",
                Email = "test@test.com",
                DomainId = 10
            };

            const string Error = "test create identity error";            

            var userStoreMock = new Mock<IUserStore<AppIdentityUser>>();

            userStoreMock
                .Setup(m => m.CreateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Description = Error }));

            userStoreMock
                .Setup(m => m.UpdateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()))
                .ReturnsAsync(IdentityResult.Success);

            userStoreMock.As<IUserRoleStore<AppIdentityUser>>();
            userStoreMock.As<IUserPasswordStore<AppIdentityUser>>();

            var service = GetService(userStoreMock);

            var act = await service.CreateAsync(user.UserName, user.Email, "testPass456%", user.DomainId);

            Assert.IsFalse(act.result.Succeeded);
            userStoreMock.Verify(m => m.UpdateAsync(It.IsAny<AppIdentityUser>(), new CancellationToken()), Times.Never());
        }




        private IdentityService GetService(UserManager<AppIdentityUser> userManager)
        {
            return new IdentityService(userManager);
        }

        private IdentityService GetService(Mock<IUserStore<AppIdentityUser>> userStore)
        {
            return GetService(GetUserManager(userStore));
        }

        private UserManager<TUser> GetUserManager<TUser> ( Mock<IUserStore<TUser>> userStore) where TUser : class
        {
            

            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();

            SetOptions(idOptions);

            options.Setup(o => o.Value).Returns(idOptions);

            var userValidators = new List<IUserValidator<TUser>>();
            var validator = new Mock<IUserValidator<TUser>>();
            

            userValidators.Add(validator.Object);


            var passValidator = new PasswordValidator<TUser>();
            var pwdValidators = new List<IPasswordValidator<TUser>>();

            pwdValidators.Add(passValidator);

            var userManager = new UserManager<TUser>(userStore.Object, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);

            validator
                .Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            return userManager;

            static void SetOptions(IdentityOptions idOptions)
            {
                //this should be keep in sync with settings in ConfigureIdentity in WebApi -> Startup.cs
                idOptions.Lockout.AllowedForNewUsers = false;
                //idOptions.Password.RequireDigit = true;
                //idOptions.Password.RequireLowercase = true;
                //idOptions.Password.RequireNonAlphanumeric = true;
                //idOptions.Password.RequireUppercase = true;
                //idOptions.Password.RequiredLength = 8;
                //idOptions.Password.RequiredUniqueChars = 1;

                idOptions.SignIn.RequireConfirmedEmail = false;

                // Lockout settings.
                idOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                idOptions.Lockout.MaxFailedAccessAttempts = 5;
                idOptions.Lockout.AllowedForNewUsers = true;
            }
        }
    }
}
