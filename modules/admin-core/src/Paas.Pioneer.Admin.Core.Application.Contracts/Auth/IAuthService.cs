using Paas.Pioneer.Admin.Core.Application.Contracts.Auth.Dto.Input;
using Paas.Pioneer.Admin.Core.Application.Contracts.Auth.Dto.Output;
using Paas.Pioneer.Domain.Shared.Dto.Output;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Auth
{
    /// <summary>
    /// Ȩ�޷���
    /// </summary>
    public interface IAuthService : IApplicationService
    {
        Task<ResponseOutput<AuthLoginOutput>> LoginAsync(AuthLoginInput input);

        Task<ResponseOutput<AuthUserInfoOutput>> GetUserInfoAsync();

        Task<ResponseOutput<GetPassWordEncryptKeyOutput>> GetPassWordEncryptKeyAsync();

        Task<IEnumerable<string>> GetPermissionsCodeListAsync(Guid? userId);
    }
}