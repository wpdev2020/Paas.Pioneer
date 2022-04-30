﻿using System;
using Paas.Pioneer.Domain.Shared.ModelValidation;
using System.ComponentModel.DataAnnotations;
namespace Paas.Pioneer.Admin.Core.Application.Contracts.News.Dto.Input
{
    /// <summary>
    /// 新闻管理修改
    /// </summary>
    public class UpdateNewsInput
    {

        /// <summary>
        /// 字典Id
        /// </summary>
        [Required(ErrorMessage = "请输入字典Id")]
public Guid DictionaryId { get; set; }


        /// <summary>
        /// 图片
        /// </summary>
        [Required(ErrorMessage = "请输入图片")]
public string Portrait { get; set; }


        /// <summary>
        /// 发布时间
        /// </summary>
        [Required(ErrorMessage = "请输入发布时间")]
public DateTime PushTime { get; set; }


        /// <summary>
        /// 新闻内容
        /// </summary>
        [Required(ErrorMessage = "请输入新闻内容")]
public string NewsContent { get; set; }


        /// <summary>
        /// 隐藏
        /// </summary>
        [Required(ErrorMessage = "请输入隐藏")]
public bool Hidden { get; set; }


        /// <summary>
        /// 启用
        /// </summary>
        [Required(ErrorMessage = "请输入启用")]
public bool Enabled { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        [Required(ErrorMessage = "请输入排序")]
public int Sort { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        [Required(ErrorMessage = "请输入描述")]
public string Description { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        [Required(ErrorMessage = "请输入修改时间")]
public DateTime? LastModificationTime { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Required(ErrorMessage = "请输入创建时间")]
public DateTime CreationTime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [NotEqual("00000000-0000-0000-0000-000000000000", ErrorMessage = "请输入")]
[Required(ErrorMessage = "请输入")]
public Guid Id { get; set; }

    }
}