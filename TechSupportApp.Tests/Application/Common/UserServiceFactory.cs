using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Models;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;
using TechSupportApp.Infrastructure.Identity;
using TechSupportApp.Infrastructure.Persistence;

namespace TechSupportApp.Tests.Application.Common
{
    class UserServiceFactory
    {
        public UserServiceFactory()
        {

        }

        public FakeIdentityService Create()
        {
            return new FakeIdentityService();
        }     
    }
    //less code than mocks переделать в  моки
    class FakeIdentityService : IIdentityService
    {
        public string DomainId { get; set; }
        //переделать
        public bool ShouldFailNextCall { get; set; } = false;


        public Task<(Result result, string userId)> CreateAsync(string name, string email, string password, int domainId)
        {
            throw new NotImplementedException();
        }

        public async Task<(Result result, int id)> GetDomainId(string id)
        {
            DomainId = id;

            if (ShouldFailNextCall)
            {
                ShouldFailNextCall = false;
                return await Task.FromResult((Result.Failure(new string[] { } ), -1));
            }

            return await Task.FromResult((Result.Success(), int.Parse(id)) );
        }
    }
}
