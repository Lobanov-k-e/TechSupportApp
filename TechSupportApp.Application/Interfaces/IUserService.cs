using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Interfaces
{
    public interface IUserService
    {        
        Task<User> GetUserByIdentity(string id);
    }
}
