﻿using Volo.Abp.DependencyInjection;

namespace Paas.Pioneer.Domain.Shared.Configs
{
	/// <summary>
	/// 应用配置
	/// </summary>
	public class AppConfig : ISingletonDependency
	{
		/// <summary>
		/// Api地址，默认 http://*:8000
		/// </summary>
		public string[] Urls { get; set; }

		/// <summary>
		/// 跨域地址，默认 http://*:9000
		/// </summary>
		public string[] CorUrls { get; set; }

		/// <summary>
		/// 租户类型
		/// </summary>
		public bool Tenant { get; set; } = false;

		/// <summary>
		/// Swagger文档
		/// </summary>
		public bool Swagger { get; set; } = false;

		/// <summary>
		/// 统一认证授权服务器
		/// </summary>
		public IdentityServer IdentityServer { get; set; } = new IdentityServer();

		/// <summary>
		/// Aop配置
		/// </summary>
		public AopConfig Aop { get; set; } = new AopConfig();

		/// <summary>
		/// 日志配置
		/// </summary>
		public LogConfig Log { get; set; } = new LogConfig();

		/// <summary>
		/// 限流
		/// </summary>
		public bool RateLimit { get; set; } = false;

		/// <summary>
		/// 验证码配置
		/// </summary>
		public VarifyCodeConfig VarifyCode { get; set; } = new VarifyCodeConfig();
	}

	/// <summary>
	/// 统一认证授权服务器配置
	/// </summary>
	public class IdentityServer
	{
		/// <summary>
		/// 启用
		/// </summary>
		public bool Enable { get; set; } = false;

		/// <summary>
		/// 地址
		/// </summary>
		public string Url { get; set; } = "https://localhost:5000";
	}

	/// <summary>
	/// Aop配置
	/// </summary>
	public class AopConfig
	{
		/// <summary>
		/// 事物
		/// </summary>
		public bool Transaction { get; set; } = true;
	}

	/// <summary>
	/// 日志配置
	/// </summary>
	public class LogConfig
	{
		/// <summary>
		/// 操作日志
		/// </summary>
		public bool Operation { get; set; } = true;
	}

	/// <summary>
	/// 验证码配置
	/// </summary>
	public class VarifyCodeConfig
	{
		/// <summary>
		/// 启用
		/// </summary>
		public bool Enable { get; set; } = true;

		/// <summary>
		/// 操作日志
		/// </summary>
		public string[] Fonts { get; set; }// = new[] { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };
	}
}