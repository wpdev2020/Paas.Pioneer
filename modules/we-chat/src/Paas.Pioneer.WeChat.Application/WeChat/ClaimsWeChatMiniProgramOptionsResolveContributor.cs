﻿using System;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.MiniProgram;
using EasyAbp.Abp.WeChat.MiniProgram.Infrastructure.OptionsResolve;
using Microsoft.Extensions.DependencyInjection;
using Paas.Pioneer.WeChat.Domain.WeChatApps;
using Volo.Abp.Users;
using Paas.Pioneer.WeChat.Domain.Common.WeChatApps;

namespace Paas.Pioneer.WeChat.Application.WeChat;
public class ClaimsWeChatMiniProgramOptionsResolveContributor : IWeChatMiniProgramOptionsResolveContributor
{
    public const string ContributorName = "WeChatManagementClaims";

    public string Name => ContributorName;

    public async Task ResolveAsync(WeChatMiniProgramOptionsResolveContext context)
    {
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

        var appid = currentUser.FindClaim("appid");

        if (appid == null || appid.Value.IsNullOrEmpty())
        {
            return;
        }

        // Todo: should use IMiniProgramStore
        var weChatAppRepository = context.ServiceProvider.GetRequiredService<IWeChatAppRepository>();

        var miniProgram = await weChatAppRepository.GetMiniProgramAppByAppIdAsync(appid.Value);

        context.Options = new AbpWeChatMiniProgramOptions
        {
            OpenAppId = miniProgram.OpenAppIdOrName,
            AppId = miniProgram.AppId,
            AppSecret = miniProgram.AppSecret,
            EncodingAesKey = miniProgram.EncodingAesKey,
            Token = miniProgram.Token
        };
    }
}