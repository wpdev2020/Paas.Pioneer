﻿using Lazy.SlideCaptcha.Core;
using Lazy.SlideCaptcha.Core.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Paas.Pioneer.Admin.Core.Application.Contracts.Auth;
using Paas.Pioneer.Admin.Core.Application.Contracts.Auth.Dto.Input;
using Paas.Pioneer.Admin.Core.Application.Contracts.Auth.Dto.Output;
using Paas.Pioneer.Admin.Core.Application.Contracts.LoginLog;
using Paas.Pioneer.Admin.Core.Application.Contracts.LoginLog.Dto.Input;
using Paas.Pioneer.Admin.Core.Application.Contracts.User;
using Paas.Pioneer.Admin.Core.Application.Contracts.User.Dto.Output;
using Paas.Pioneer.Domain.Shared.Auth;
using Paas.Pioneer.Domain.Shared.Extensions;
using Paas.Pioneer.Domain.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Paas.Pioneer.Admin.Core.HttpApi.Controllers
{
    /// <summary>
    /// 授权管理
    /// </summary>
    [Route("api/admin/[controller]")]
    [Authorize]
    public class AuthController : AbpControllerBase
    {
        private readonly IUserToken _userToken;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILoginLogService _loginLogService;
        private readonly ICaptcha _captcha;
        private readonly IDataFilter _dataFilter;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="authService"></param>
        /// <param name="userService"></param>
        /// <param name="loginLogService"></param>
        /// <param name="captcha"></param>
        /// <param name="dataFilter"></param>
        /// <param name="logger"></param>
        public AuthController(
            IUserToken userToken,
            IAuthService authService,
            IUserService userService,
            ILoginLogService loginLogService,
            ICaptcha captcha,
            IDataFilter dataFilter,
            ILogger<AuthController> logger)
        {
            _userToken = userToken;
            _authService = authService;
            _userService = userService;
            _loginLogService = loginLogService;
            _captcha = captcha;
            _dataFilter = dataFilter;
            _logger = logger;
        }

        /// <summary>
        /// 获得token
        /// </summary>
        /// <returns></returns>
        private LoginSuccessOutput GetToken(AuthLoginOutput user)
        {
            if (user == null)
            {
                throw new BusinessException("用户信息错误");
            }
            IEnumerable<Claim> claimList = new[]
            {
                new Claim(ClaimAttributes.UserId, user.Id.ToString()),
                new Claim(ClaimAttributes.UserName, user.UserName),
                new Claim(ClaimAttributes.UserNickName, user.NickName),
                new Claim(ClaimAttributes.TenantId, user.TenantId.ToString()),
                new Claim(ClaimAttributes.TenantType, user.TenantType.ToString()),
            };
            var token = _userToken.Create(claimList);
            return new LoginSuccessOutput
            {
                token = token
            };
        }

        /// <summary>
        /// 获取验证数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("getCaptcha")]
        [AllowAnonymous]
        public CaptchaData GetCaptcha()
        {
            var data = _captcha.Generate();
            return data;
        }

        /// <summary>
        /// 检查验证数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkCaptcha")]
        [AllowAnonymous]
        public ValidateResult CheckCaptcha([FromQuery] string id, SlideTrack track)
        {
            var result = _captcha.Validate(id, track);
            result.Result = 0;
            return result;
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        [HttpGet("getPassWordEncryptKey")]
        [AllowAnonymous]
        public async Task<GetPassWordEncryptKeyOutput> GetPassWordEncryptKey()
        {
            return await _authService.GetPassWordEncryptKeyAsync();
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getUserInfo")]
        public async Task<AuthUserInfoOutput> GetUserInfo()
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                return await _authService.GetUserInfoAsync();
            }
        }

        /// <summary>
        /// 用户登录
        /// 根据登录信息生成Token
        /// </summary>
        /// <param name="input">登录信息</param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<LoginSuccessOutput> Login([FromBody] AuthLoginInput input)
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = await _authService.LoginAsync(input);
                sw.Stop();

                #region 添加登录日志

                var loginLogAddInput = new LoginLogAddInput()
                {
                    CreatedUserName = input.UserName,
                    ElapsedMilliseconds = sw.ElapsedMilliseconds,
                    Status = true,
                    Msg = "登录成功"
                };
                loginLogAddInput.CreatedUserId = res.Id;
                loginLogAddInput.NickName = res.NickName;
                loginLogAddInput.TenantId = res.TenantId;

                await _loginLogService.AddAsync(loginLogAddInput);

                #endregion 添加登录日志
                return GetToken(res);
            }
        }

        /// <summary>
        /// 刷新Token
        /// 以旧换新
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("refresh")]
        [AllowAnonymous]
        public async Task<LoginSuccessOutput> Refresh([BindRequired] string token)
        {
            var userClaims = _userToken.Decode(token);
            if (userClaims == null || !userClaims.Any())
            {
                throw new BusinessException("错误token");
            }

            var refreshExpires = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.RefreshExpires)?.Value;
            if (refreshExpires.IsNullOrEmpty())
            {
                throw new BusinessException("错误token");
            }

            if (refreshExpires.ToLong() <= DateTime.Now.ToTimestamp())
            {
                throw new BusinessException("登录信息已过期");
            }

            var userId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.UserId)?.Value;
            if (userId.IsNullOrEmpty())
            {
                throw new BusinessException("登录信息已失效");
            }
            using (_dataFilter.Disable<IMultiTenant>())
            {
                var output = await _userService.GetLoginUserAsync(Guid.Parse(userId));
                return GetToken(output);
            }
        }
    }
}
