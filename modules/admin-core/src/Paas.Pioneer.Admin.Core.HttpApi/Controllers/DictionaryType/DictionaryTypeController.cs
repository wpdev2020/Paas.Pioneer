﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paas.Pioneer.Admin.Core.Application.Contracts.DictionaryType;
using Paas.Pioneer.Admin.Core.Application.Contracts.DictionaryType.Dto.Input;
using Paas.Pioneer.Admin.Core.Application.Contracts.DictionaryType.Dto.Output;
using Paas.Pioneer.Domain.Shared.Dto.Input;
using Paas.Pioneer.AutoWrapper;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Paas.Pioneer.Domain.Shared.Dto.Output;
using System.Collections.Generic;
using Paas.Pioneer.Admin.Core.Application.Contracts.Dictionary.Dto.Output;

namespace Paas.Pioneer.Admin.Core.HttpApi.Controllers
{
    /// <summary>
    /// 数据字典类型
    /// </summary>
    [Route("api/admin/[controller]/[action]")]
    [Authorize]
    public class DictionaryTypeController : AbpControllerBase
    {
        private readonly IDictionaryTypeService _dictionaryTypeService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dictionaryTypeService"></param>
        public DictionaryTypeController(IDictionaryTypeService dictionaryTypeService)
        {
            _dictionaryTypeService = dictionaryTypeService;
        }

        /// <summary>
        /// 查询单条数据字典类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DictionaryTypeGetOutput> Get(Guid id)
        {
            return await _dictionaryTypeService.GetAsync(id);
        }

        /// <summary>
        /// 查询分页数据字典类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Page<DictionaryTypeOutput>> GetPageList([FromBody] PageInput<DictionaryTypeInput> model)
        {
            return await _dictionaryTypeService.GetPageListAsync(model);
        }

        /// <summary>
        /// 查询Code列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<GetDictionaryOutput>> GetCodeList(GetCodeListInput input)
        {
            return await _dictionaryTypeService.GetCodeListAsync(input);
        }

        /// <summary>
        /// 新增数据字典类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Add([FromBody] DictionaryTypeAddInput input)
        {
            await _dictionaryTypeService.AddAsync(input);
        }

        /// <summary>
        /// 修改数据字典类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task Update([FromBody] DictionaryTypeUpdateInput input)
        {
            await _dictionaryTypeService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除数据字典类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task SoftDelete(Guid id)
        {
            await _dictionaryTypeService.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除数据字典类型
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task BatchSoftDelete([FromBody] Guid[] ids)
        {
            await _dictionaryTypeService.BatchSoftDeleteAsync(ids);
        }
    }
}