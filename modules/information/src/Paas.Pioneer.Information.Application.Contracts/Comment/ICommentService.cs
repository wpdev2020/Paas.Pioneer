﻿using Paas.Pioneer.Domain.Shared.Dto.Input;
using Paas.Pioneer.AutoWrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Paas.Pioneer.Domain.Shared.Dto.Output;
using Paas.Pioneer.Information.Application.Contracts.Comment.Dto.Input;
using Paas.Pioneer.Information.Application.Contracts.Comment.Dto.Output;

namespace Paas.Pioneer.Information.Application.Contracts.Comment
{
    /// <summary>
    /// 评论管理接口
    /// </summary>
    public interface ICommentService : IApplicationService
    {
        /// <summary>
        /// 查询评论管理
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<GetCommentOutput> GetAsync(Guid id);

        /// <summary>
        /// 查询分页评论管理
        /// </summary>
        /// <param name="input">入参</param>
        /// <returns></returns>
        Task<Page<GetCommentPageListOutput>> GetPageListAsync(PageInput<GetCommentPageListInput> model);

        /// <summary>
        /// 新增评论管理
        /// </summary>
        /// <param name="input">入参</param>
        /// <returns></returns>
        Task AddAsync(AddCommentInput input);

        /// <summary>
        /// 修改评论管理
        /// </summary>
        /// <param name="input">入参</param>
        /// <returns></returns>
        Task UpdateAsync(UpdateCommentInput input);

        /// <summary>
        /// 删除评论管理
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 批量删除评论管理
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
        Task BatchSoftDeleteAsync(IEnumerable<Guid> ids);
    }
}