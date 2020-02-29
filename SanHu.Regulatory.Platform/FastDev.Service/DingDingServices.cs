﻿using FastDev.IServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using FD.Common.Extensions;
using System.Threading.Tasks;
using FastDev.DevDB.Common;
using DingTalk.Api.Response;
using DingTalk.Api.Request;
using FastDev.Common.Extensions;
using Microsoft.Extensions.Options;
using FD.Model.Configs;

namespace FastDev.Service
{
    public class DingDingServices : ApplicationServices, IDingDingServices
    {
        /// <summary>
        /// httpclient工厂
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;
        readonly ServerNameConfigModel _serverNameConfig;
        public DingDingServices(IOptionsSnapshot<ServerNameConfigModel> appsettingsModel, IHttpClientFactory clientFactory)
        {
            _serverNameConfig = appsettingsModel.Value;
            _clientFactory = clientFactory;
        }

        public Task<OapiWorkrecordAddResponse> WorkrecordAdd(OapiWorkrecordAddRequest oapiWorkrecordAddRequest)
        {
            var url = "framework/api/dingding/workrecordadd?" + GetAgentIDString();

            return PostFrameWork<OapiWorkrecordAddResponse>(url, oapiWorkrecordAddRequest);
        }
        public void CreateWorkrecor(string userId, string title, string url, string formTitle, string fromContent)
        {
            OapiWorkrecordAddRequest oapiWorkrecordAddRequest = new OapiWorkrecordAddRequest()
            {
                Userid = userId.ToString(),//user的accountID
                CreateTime = DateTime.Now.GetTimeStampM(),
                Title = title,//待办事项的标题
                Url = url,//"https://oa.dingtalk.com",//待办事项的跳转链接
                FormItemList_ = new List<OapiWorkrecordAddRequest.FormItemVoDomain>()
                {
                    new OapiWorkrecordAddRequest.FormItemVoDomain
                    {
                        Title=formTitle,
                        Content=fromContent
                    },
                }
            };
            WorkrecordAdd(oapiWorkrecordAddRequest);
        }

        /// <summary>
        /// 框架post数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiurl"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<T> PostFrameWork<T>(string apiurl, object data)
        {
            var client = _clientFactory.CreateClient(HostData.FrameWorkSeverName);
            return await client.PostAsync<T>(apiurl, data);
        }
        private string GetAgentIDString()
        {
            return $"agentId={_serverNameConfig.AgentId}";
        }
    }
}