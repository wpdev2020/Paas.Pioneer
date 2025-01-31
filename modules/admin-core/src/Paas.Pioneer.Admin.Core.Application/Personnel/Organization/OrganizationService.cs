﻿using Paas.Pioneer.Admin.Core.Application.Contracts.Personnel.Organization;
using Paas.Pioneer.Admin.Core.Application.Contracts.Personnel.Organization.Dto.Input;
using Paas.Pioneer.Admin.Core.Application.Contracts.Personnel.Organization.Dto.Output;
using Paas.Pioneer.Admin.Core.Domain.Personnel.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Paas.Pioneer.Admin.Core.Application.Personnel.Organization
{
    public class OrganizationService : ApplicationService, IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<OrganizationGetOutput> GetAsync(Guid id)
        {
            var result = await _organizationRepository.GetAsync(
                expression: x => x.Id == id,
                selector: x => new OrganizationGetOutput
                {
                    Id = x.Id,
                    ParentId = x.ParentId,
                    Name = x.Name,
                    Code = x.Code,
                    Value = x.Value,
                    PrimaryUserId = x.PrimaryEmployeeId,
                    EmployeeCount = x.EmployeeCount,
                    Enabled = x.Enabled,
                    Description = x.Description
                });
            return result;
        }

        public async Task<List<OrganizationListOutput>> GetListAsync(string key)
        {

            Expression<Func<Pe_OrganizationEntity, bool>> predicate = x => true;

            if (!key.IsNullOrEmpty())
            {
                predicate = predicate.And(a => a.Name.Contains(key) || a.Code.Contains(key));
            }

            var data = await _organizationRepository.GetResponseOutputListAsync(
                expression: predicate,
                selector: x => new OrganizationListOutput
                {
                    Id = x.Id,
                    ParentId = x.ParentId,
                    Name = x.Name,
                    Code = x.Code,
                    Value = x.Value,
                    Description = x.Description,
                    Enabled = x.Enabled,
                    CreatedTime = x.CreationTime,
                }, x => x.OrderBy(p => p.Sort));

            return data;
        }

        public async Task AddAsync(OrganizationAddInput input)
        {
            var dictionary = ObjectMapper.Map<OrganizationAddInput, Pe_OrganizationEntity>(input);
            await _organizationRepository.InsertAsync(dictionary);
        }

        public async Task UpdateAsync(OrganizationUpdateInput input)
        {
            var entity = await _organizationRepository.GetAsync(input.Id);
            if (entity?.Id == Guid.Empty)
            {
                throw new BusinessException("数据字典不存在！");
            }
            ObjectMapper.Map(input, entity);
            await _organizationRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _organizationRepository.DeleteAsync(a => a.Id == id);
        }
    }
}