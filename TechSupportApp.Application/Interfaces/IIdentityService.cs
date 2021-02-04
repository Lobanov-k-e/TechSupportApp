﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Models;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Interfaces
{
    /*
     * подумать над интерфейсом current user service
     */
    public interface IIdentityService
    {        
        Task<(Result result, string userId)> CreateAsync(string name, string email, string password, int domainId);       
        Task<(Result result, int id)> GetDomainId(string id);
    }
}
