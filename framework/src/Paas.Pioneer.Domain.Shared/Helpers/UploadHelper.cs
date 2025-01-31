﻿using Microsoft.AspNetCore.Http;
using Paas.Pioneer.AutoWrapper;
using Paas.Pioneer.Domain.Shared.Configs;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Paas.Pioneer.Domain.Shared.Helpers
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    public class UploadHelper : ISingletonDependency
    {
        #region 上传单文件

        /// <summary>
        /// 上传单文件
        /// </summary>
        /// <param name="file">文件流</param>
        /// <param name="config">文件配置</param>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseOutput<Files.FileInfo>> UploadAsync(IFormFile file, FileUploadConfig config, object args, CancellationToken cancellationToken = default)
        {
            var res = new ResponseOutput<Files.FileInfo>();

            if (file == null || file.Length < 1)
            {
                throw new BusinessException("请上传文件！");
            }

            //格式限制
            if (!config.ContentType.Contains(file.ContentType))
            {
                throw new BusinessException("文件格式错误");
            }

            //大小限制
            if (file.Length > config.MaxSize)
            {
                throw new BusinessException("文件过大");
            }

            var fileInfo = new Files.FileInfo(file.FileName, file.Length)
            {
                UploadPath = config.UploadPath,
                RequestPath = config.RequestPath
            };

            var dateTimeFormat = !config.DateTimeFormat.IsNullOrEmpty() ? DateTime.Now.ToString(config.DateTimeFormat) : "";
            var format = !config.Format.IsNullOrEmpty() ? StringHelper.Format(config.Format, args) : "";
            fileInfo.RelativePath = Path.Combine(dateTimeFormat, format).Replace(@"\", "/");

            if (!Directory.Exists(fileInfo.FileDirectory))
            {
                Directory.CreateDirectory(fileInfo.FileDirectory);
            }

            fileInfo.SaveName = $"{IdWorkerHelper.GenId64()}.{fileInfo.Extension}";

            await SaveAsync(file, fileInfo.FilePath, cancellationToken);

            return res.Succees(fileInfo);
        }

        #endregion

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SaveAsync(IFormFile file, string filePath, CancellationToken cancellationToken = default)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }
        }
    }
}