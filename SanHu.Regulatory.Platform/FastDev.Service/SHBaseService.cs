﻿using FastDev.Common;
using FastDev.DevDB;
using FastDev.DevDB.Model.Config;
using FastDev.IServices;
using FastDev.Model.Entity;
using FD.Common;
using FD.Model.Dto;
using FD.Model.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WanJiang.Framework.Infrastructure.Logging;

namespace FastDev.Service
{

    /// <summary>
    /// 三湖业务基础server
    /// </summary>
    public class SHBaseService : ServiceBase
    {
        protected const string AccountId = "165906044420484870";
        protected const string AccountName = "余盛全";

        string publishtypet = null;
        string closeDate = null;
        string publishTitle = null;
        /// <summary>
        /// 发送待办
        /// </summary>
        /// <param name="userId">钉钉用户id</param>
        /// <param name="title">待办标题</param>
        /// <param name="url">跳转地址</param>
        /// <param name="formTitle">待办表单标题</param>
        /// <param name="fromContent">待办表单内容</param>
        public string CreateWorkrecor(string userId, string title, string url, Dictionary<string, string> formInfo)
        {
            var ddService = SysContext.GetService<IDingDingServices>();
            return ddService.CreateWorkrecor(userId, title, url, formInfo);
        }

        /// <summary>
        /// 撤回待办
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recordId"></param>
        public void WorkrecordUpdate(string userId, string recordId)
        {
            var ddService = SysContext.GetService<IDingDingServices>();
            ddService.WorkrecordUpdate(userId, recordId);
        }

        /// <summary>
        /// 修改事件状态
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventStatus"></param>
        /// <returns></returns>
        public bool UpdateEventState(string eventId, EventStatus eventStatus)
        {
            return base.QueryDb.Execute("update event_info set evtState = @0 where objid=@1", (int)eventStatus, eventId) > 0;
        }
        /// <summary>
        /// 修改事件状态和办理人信息
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventStatus"></param>
        /// <returns></returns>
        public bool UpdateEventStateHandler(string eventId, EventStatus eventStatus, string handler)
        {
            return base.QueryDb.Execute("update event_info set evtState = @0,finishUserName=@1 where objid=@2", (int)eventStatus, handler, eventId) > 0;
        }

        /// <summary>
        /// 保存任务
        /// </summary>
        /// <param name="workTask"></param>
        public string SaveWorkTask(work_task workTask)
        {
            return ServiceHelper.GetService("work_task").Create(workTask).ToString();
        }

        /// <summary>
        /// 任务状态更新
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="workTaskStatus"></param>
        public void UpdateWorkTaskState(string taskid, WorkTaskStatus workTaskStatus)
        {
            if (taskid == null) return;
            var taskInfo = GetWorkTask(taskid);
            if (taskInfo == null) return;
            taskInfo.TaskStatus = (int)workTaskStatus;
            taskInfo.CompleteTime = DateTime.Now;

            if (workTaskStatus == WorkTaskStatus.Normal)
            {
                WorkrecordUpdate(taskInfo.AssignUsers, taskInfo.TodotaskID);
            }

            QueryDb.Update(taskInfo);
        }

        protected work_task GetWorkTask(string taskid)
        {
            return QueryDb.FirstOrDefault<work_task>(" where id=@0", taskid);
        }


        public object FormData(FormDataReq data)
        {
            var res_dictionarycase = QueryDb.FirstOrDefault<res_dictionary>("where DicCode=@0", "CaseType");
            var dicItemscase = QueryDb.Query<res_dictionaryItems>("SELECT * FROM res_dictionaryitems where DicID=@0", res_dictionarycase.ID).ToDictionary(k => k.ItemCode, v => v.Title);
            if (data.Model.ToUpper() == "case_Info".ToUpper())
            {
                data.Model = "case_Info";
            }
            IService service = ServiceHelper.GetService(data.Model);

            //根据事件id或者id查询数据
            var filter = new FilterGroup();

            if (!string.IsNullOrEmpty(data.FormId))
            {
                filter.rules.Add(new FilterRule("ID", data.FormId, "equal"));
            }
            if (!string.IsNullOrEmpty(data.EventInfoId))
            {
                filter.rules.Add(new FilterRule("EventInfoId", data.EventInfoId, "equal"));
            }
            object atlist = null;
            //案件特殊判断
            if (data.Model.ToUpper() == "case_Info".ToUpper())
            {
                filter.rules.Add(new FilterRule("PreviousformID", "", "notequal"));
                atlist = Getpunishattchment(data.FormId);
            }
            ServiceConfig userServiceConfig = ServiceHelper.GetServiceConfig("user");
            //查询主表数据
            var obj = service.GetListData(filter).OrderByDescending(s => s.Keys.Contains("createTime") ? s["createTime"] : s["CreateDate"]).FirstOrDefault();  //查询主表单
            if (obj == null) return null;//throw new Exception("未取得关联数据");
            obj.Add("publishtype", publishtypet);
            if (obj.ContainsKey("PunishmentTitle"))
                obj["PunishmentTitle"] = publishTitle;
            else
                obj.Add("PunishmentTitle", publishTitle);
            if (obj["CreateUserID"] != null)
            {
                var user = SysContext.GetOtherDB(userServiceConfig.model.dbName).First<user>($"select * from user where Id='{obj["CreateUserID"]}'");
                if (user != null)
                    obj["CreateUserID"] = user.Name;
            }
            if (data.Model.ToUpper() == "case_Info".ToUpper())
            {
                if (obj.ContainsKey("CaseType"))
                {
                    if (obj["CaseType"] != null)
                    {
                        if (dicItemscase.ContainsKey(obj["CaseType"].ToString()))  // CaseType 显示中文title
                        {
                            obj["CaseType"] = dicItemscase[obj["CaseType"].ToString()];
                        }
                    }
                }
            }
            string formId = obj["ID"].ToString();  //得到id
            string eventinfoid = null;
            if (obj.ContainsKey("EventInfoId")) eventinfoid = obj["EventInfoId"] != null ? obj["EventInfoId"].ToString() : string.Empty;
            if (data.Model.ToLower() == "task_survey")  // 现场勘查 EventType 显示中文title
            {
                var res_dictionary = QueryDb.FirstOrDefault<res_dictionary>("where DicCode=@0", "EventType");
                var dicItems = QueryDb.Query<res_dictionaryItems>("SELECT * FROM res_dictionaryitems where DicID=@0", res_dictionary.ID).ToDictionary(k => k.ItemCode, v => v.Title);
                var eventType = obj["EventType"].ToString();
                if (dicItems.ContainsKey(eventType))
                {
                    obj["EventType"] = dicItems[eventType];
                }
            }

            //构建其他需要查询的数据
            var dicData = BuildData(data, formId, eventinfoid);
            obj.Add("closeDate", closeDate);
            dicData.Add("MainForm", obj);
            //添加处罚决定书证据附件
            if (atlist != null)
            {
                if (dicData.ContainsKey("attachment"))
                {
                    var als = dicData["attachment"] as List<attachment>;
                    if (als != null)
                    {
                        als.AddRange(atlist as List<attachment>);
                        dicData["attachment"] = als;
                    }
                    else
                    {
                        dicData["attachment"] = atlist;
                    }

                }
                else
                {
                    dicData.Add("attachment", atlist);
                }
            }
            return dicData;
        }
        private Dictionary<string, object> BuildData(FormDataReq data, string formId, string eventinfoid)
        {
            var dic = new Dictionary<string, object>();
            if (data.FilterModels == null || data.FilterModels.Count() < 1)
            {
                dic.Add("law_party", Getlaw_partyByFormId(formId));
                dic.Add("law_staff", Getlaw_staffByFormId(formId));
                dic.Add("attachment", GetattachmentByFormId(formId));
                return dic;
            }

            if (data.FilterModels.Contains("law_party"))
            {
                dic.Add("law_party", Getlaw_partyByFormId(formId));
            }
            if (data.FilterModels.Contains("law_staff"))
            {
                dic.Add("law_staff", Getlaw_staffByFormId(formId));
            }
            if (data.FilterModels.Contains("attachment"))
            {
                dic.Add("attachment", GetattachmentByFormId(formId));
            }
            if (data.FilterModels.Contains("casedetail"))
            {
                dic.Add("law_party", Getlaw_partyByFormId(formId));
                dic.Add("attachment", GetattachmentByFormId(formId));
                dic.Add("casetimeline", GetAllFormByEventId(eventinfoid));
            }
            return dic;
        }
        private object Getlaw_partyByFormId(string formId)
        {
            var filter = new FilterGroup();
            filter.rules.Add(new FilterRule("AssociationobjectID", formId, "equal"));
            var list = ServiceHelper.GetService("law_party").GetListData(filter);
            return ServiceHelper.GetService("law_party").GetListData(filter);
        }
        private object Getlaw_staffByFormId(string formId)
        {
            var filter = new FilterGroup();
            filter.rules.Add(new FilterRule("AssociatedobjectID", formId, "equal"));
            return ServiceHelper.GetService("law_staff").GetListData(filter);
        }
        private object GetattachmentByFormId(string formId)
        {
            var filter = new FilterGroup();
            filter.rules.Add(new FilterRule("AssociationobjectID", formId, "equal"));
            return ServiceHelper.GetService("attachment").GetListData(filter);
        }

        private object Getpunishattchment(string formId)
        {
            var filter = new FilterGroup();
            var law = QueryDb.FirstOrDefault<law_punishmentInfo>("select * from law_punishmentInfo where CaseId=@0", formId);
            if (law == null) return null;
            string publishtype = "";
            if (law.IsConfiscationgoods)//1为真
            {
                publishtype = "没收物品";
            }
            if (law.Isfine)
            {
                publishtype = publishtype + " " + "罚款";
            }
            publishtypet = publishtype;
            publishTitle = law.PunishmentTitle;
            filter.rules.Add(new FilterRule("AssociationobjectID", law.ID, "equal"));
            return ServiceHelper.GetService("attachment").GetListData(filter);
        }

        private object GetParties(string formid, string formType)
        {
            List<law_party> lawParties = new List<law_party>();
            DataTable dt = new DataTable();
            QueryDb.Fill(dt, "where Associatedobjecttype=@0 and AssociationobjectID=@1", formType, formid);
            if (dt.Rows.Count > 0)
            {
                foreach (var d in dt.Rows)
                {
                    var dr = d as DataRow;
                    law_party lawParty = new law_party();
                    lawParty.address = (string)dr["address"];
                    lawParty.Contactnumber = (string)dr["Contactnumber"];
                    lawParty.IDcard = (string)dr["IDcard"];
                    lawParty.Name = (string)dr["Name"];
                    lawParty.Nameoflegalperson = (string)dr["Nameoflegalperson"];
                    lawParty.Nationality = (string)dr["Nationality"];
                    lawParty.Occupation = (string)dr["Occupation"];
                    lawParty.Typesofparties = (string)dr["TypesofpartiesID"];
                    lawParty.Gender = (string)dr["Gender"];
                    lawParties.Add(lawParty);
                }
                return lawParties;
            }
            return null;
        }


        private object GetAllFormByEventId(string eventinfoid)
        {
            if (string.IsNullOrEmpty(eventinfoid)) return null;
            ServiceConfig userServiceConfig = ServiceHelper.GetServiceConfig("user");
            var formall = QueryDb.Query<formwith_eventcase>("where EventInfoId=@0 order by CreateDate", eventinfoid);
            List<Dictionary<string, object>> formlist = new List<Dictionary<string, object>>();
            foreach (var f in formall)
            {
                try
                {
                    Dictionary<string, object> temp = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(f.CreateUserID))
                    {
                        var user = SysContext.GetOtherDB(userServiceConfig.model.dbName).First<user>($"select * from user where Id='{f.CreateUserID}'");
                        temp.Add("CreateUser", user.Name);
                    }
                    else
                    {
                        temp.Add("CreateUser", "");
                    }
                    temp.Add("CreateDate", f.CreateDate);

                    if (f.FormType == "case_report")
                    {
                        temp.Add("state", f.FormState);
                        if (f.FormState == "审核通过")
                        {
                            closeDate = f.CreateDate.ToString();
                        }
                    }
                    else { temp.Add("state", "已完成"); }

                    temp.Add("FormType", f.FormName);
                    formlist.Add(temp);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return formlist;
        }


        private object GetSurvey(string taskid)
        {
            var form = QueryDb.FirstOrDefault<task_survey>(" where TaskId=@0 order PreviousformID!=null by CreateDate desc", taskid);
            return form;
        }

        private object GetPatrol(string taskid)
        {
            var form = QueryDb.FirstOrDefault<task_patrol>(" where TaskId=@0 and  order by CreateDate desc", taskid);
            return form;
        }

        /// <summary>
        /// 创建后续任务
        /// </summary>
        /// <param name="NextTasks"></param>
        /// <param name="sourcetaskid"></param>
        public object CreatTasksAndCreatWorkrecor(work_task[] NextTasks, string sourcetaskid)
        {
            if (NextTasks == null) return null;
            if (NextTasks.Length < 1) return null;
            foreach (var Task in NextTasks)
            {
                //保存任务
                Task.LaskTaskId = sourcetaskid;  //上一个任务id
                Task.InitiationTime = DateTime.Now;  //状态
                Task.TaskStatus = (int)WorkTaskStatus.Normal;  //状态
                Task.ExpectedCompletionTime = DateTime.Now.AddDays(1);  //期望完成时间
                var loginClientInfo = SysContext.GetService<ClientInfo>();
                if (loginClientInfo != null)
                {
                    Task.CreateUserID = loginClientInfo.UserId ?? null;  //任务创建人
                }
                string id = SaveWorkTask(Task);
                Task.ID = id;
                //发送待办
                if (!string.IsNullOrEmpty(Task.AppLinks))
                    Task.AppLinks += (Task.AppLinks.Contains("?") ? "&" : "?") + "taskid=" + Task.ID;
                if (!string.IsNullOrEmpty(Task.PCLinks))
                    Task.PCLinks += (Task.PCLinks.Contains("?") ? "&" : "?") + "taskid=" + Task.ID;
                var dic = new Dictionary<string, string>();
                dic.Add("任务说明", Task.TaskContent);
                dic.Add("任务发起时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                dic.Add("期望完成时间", Task.ExpectedCompletionTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));

                if (Task.TaskType.ToUpper() == TaskType.Punishment.ToString().ToUpper())
                {
                    string taskTypeStr = QueryDb.ExecuteScalar<string>("select title from res_dictionaryitems where itemcode=@0", Task.TaskType);  //获取任务类型中文描述
                    string caseNumber = QueryDb.ExecuteScalar<string>("select caseNumber from case_info where id=@0", Task.CaseID);
                    Task.TaskTitle = caseNumber + "-" + taskTypeStr;
                }

                Task.TodotaskID = CreateWorkrecor(Task.AssignUsers, Task.TaskTitle, Task.AppLinks, dic);   //待办id

                //记录待办id
                ServiceHelper.GetService("work_task").Update(Task);  //修改关联的

                //修改关联事件状态已分配任务
                UpdateEventState(Task.EventInfoId, EventStatus.dispose);
            }
            return true;
        }



    }
}
