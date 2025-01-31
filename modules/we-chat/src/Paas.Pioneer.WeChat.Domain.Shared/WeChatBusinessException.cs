﻿using Volo.Abp;

namespace Paas.Pioneer.WeChat.Domain.Shared;

public class WeChatBusinessException : BusinessException
{
    public WeChatBusinessException(int errorCode, string errorMessage) : base(message: $"WeChat error: [{errorCode}] {errorMessage}")
    {

    }
}