﻿using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface IProfileSettingsBusiness
    {

        Task<Response<ProfileSettingsModel>> GetProfileSettingsByEmail(string email);
        Task<Response<bool>> UpdateProfileSettings(ProfileSettingsModel profileSettingsModel, string customerId);
    }
}
