﻿using FastDev.Common;
using FastDev.DevDB;
using FastDev.DevDB.Model.Config;
using FastDev.IServices;
using FastDev.Model.Entity;
using FD.Model.Dto;
using FD.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastDev.Service
{
    class case_reportService : ServiceBase, IService
    {
        public case_reportService()
        {
            OnGetAPIHandler += case_reportService_OnGetAPIHandler;
        }

        private SHBaseService _sHBaseService;
        private Func<APIContext, object> case_reportService_OnGetAPIHandler(string id)
        {
            _sHBaseService = ServiceHelper.GetService("SHBaseService") as SHBaseService;
            return Handle;
        }
        public object Handle(APIContext context)
        {
            var data = JsonHelper.DeserializeJsonToObject<case_reportFinishReq>(context.Data);
            if (data.CaseReport == null) throw new Exception("没有主体数据");
            data.CaseReport.TaskId = data.SourceTaskId;
            data.CaseReport.EventInfoId = data.EventInfoId;
            QueryDb.BeginTransaction();
            try
            {
                #region 发起钉钉的审批 并将其返回的ID写入Task内
                if (data.oapiProcessinstanceCreateRequest != null)
                {
                    //填值
                    var UsrService = SysContext.GetService<IUserServices>();
                    var loginClientInfo = SysContext.GetService<WanJiang.Framework.Infrastructure.Logging.ClientInfo>();
                    var te = loginClientInfo.AccountId;

                    //ServiceConfig userServiceConfig = ServiceHelper.GetServiceConfig("user");
                    //var OTDB = SysContext.GetOtherDB(userServiceConfig.model.dbName);

                    //var deptId = OTDB.FirstOrDefault<long>(@"SELECT org.id FROM organization org 
                    //                        inner join organizationuser ou on ou.OrganizationId = org.Id
                    //                        inner join user usr on usr.Id = ou.UserId
                    //                        where usr.AccountId = @0", loginClientInfo.AccountId);
                    ////
                    //if (deptId == null)
                    //    throw new Exception("无组织部门");


                    var usrDetail = UsrService.GetUserDetails(loginClientInfo.UserId);
                    var ddService = SysContext.GetService<IDingDingServices>();
                    if (usrDetail.Result.Organizations == null)
                        throw new Exception("无组织部门");
                    var deptId = usrDetail.Result.Organizations[0].Id;


                    data.oapiProcessinstanceCreateRequest.DeptId = deptId;

                    var result = ddService.ProcessInstaceCreateAsync(data.oapiProcessinstanceCreateRequest);
                    if (result.Result.Errmsg.ToLower() != "ok")
                        throw new Exception("发起审核流失败");
                    //var targetId = result.Result.ProcessInstanceId;
                    if (data.CaseReport.TaskId == null || data.CaseReport.TaskId == "")
                        throw new Exception("Task为空");
                    var taskObj = QueryDb.FirstOrDefault<work_task>("where TaskID =@0", data.CaseReport.TaskId);
                    if (taskObj == null)
                        throw new Exception("该Task不存在");
                    //更新值
                    taskObj.processInstanceId = result.Result.ProcessInstanceId;
                    //data.CaseReport.
                    QueryDb.Update(taskObj);
                    //ServiceHelper.GetService("work_task").Update(taskObj);
                }
                #endregion
                CreateInfo(data.CaseReport);
                _sHBaseService.CreatTasksAndCreatWorkrecor(data.NextTasks, data.SourceTaskId);
                _sHBaseService.UpdateWorkTaskState(data.SourceTaskId, WorkTaskStatus.Close);//关闭任务

                //打印预生成
                var PDFSerivce = ServiceHelper.GetService("form_printPDFService") as form_printPDFService;
                PDFSerivce.AsposeToPdf(new APIContext() { Data = @"{""formId"":""" + data.CaseReport.ID + @""",""formName"":""case_report""}" });
            }
            catch (Exception e)
            {
                QueryDb.AbortTransaction();
                throw e;
            }
            QueryDb.CompleteTransaction();
            return true;
        }

        /// <summary>
        /// 创建表单
        /// </summary>
        /// <param name="TaskSurvey"></param>
        /// <param name="law_Parties"></param>
        /// <returns></returns>
        private void CreateInfo(case_report caserport)
        {
            var CaseInfoSource = base.Create(caserport) as string;
            ///更新案件信息

            var tasknow = ServiceHelper.GetService("work_task").GetDetailData(caserport.TaskId, null);
            if (tasknow != null)
            {
                var caseid = (string)tasknow["CaseID"];
                if (string.IsNullOrEmpty(caseid))
                {
                    var caseinfo = QueryDb.FirstOrDefault<case_Info>("where CaseId=@0", caseid);
                    if (caseinfo != null)
                    {
                        caseinfo.CaseStatus = "已结案";
                        QueryDb.Update(caseinfo);
                    }
                }
            }

        }

    }
}
