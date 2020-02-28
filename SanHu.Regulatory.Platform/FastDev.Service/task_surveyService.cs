﻿using FastDev.Common;
using FastDev.DevDB;
using FastDev.DevDB.Model.Config;
using FastDev.DevDB.Workflow;
using FastDev.Model.Entity;
using FD.Common;
using FD.Model.Dto;
using FD.Model.Enum;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace FastDev.Service
{
    /// <summary>
    /// 任务-勘察
    /// </summary>
    class task_surveyService : ServiceBase, IService
    {

        public task_surveyService()
        {
            OnGetAPIHandler += Task_surveyService_OnGetAPIHandler;
        }
        /// <summary>
        /// 是否需要创建新任务
        /// 1、表单类型， 2、表单
        /// </summary>
        /// <returns></returns>
        private bool NeedCreateNewTask(object postdata)
        {
            return true;
        }


        
        /*
        public override object WfCreate(object postdata, params string[] exeUserIds)
        {
            var nId = base.Create(postdata);
            ((Model.Form.task_survey)postdata).ID = nId.ToString();
            var wfId = AdvanceWorkflow(postdata, exeUserIds);
            if (wfId != null) return wfId;
            return nId;
        }

        private object AdvanceWorkflow(object postdata, params string[] exeUserIds)
        {
            var surveyData = (Model.Form.task_survey)postdata;
            if (!string.IsNullOrEmpty(surveyData.TaskId))
            {//如果该表单有任务id，则，检查是否完成了某任务

                workflowService.DbContext = QueryDb;
                Dictionary<string, object> wfContext = (Dictionary<string, object>)workflowService.GetContext(new DevDB.Workflow.WorkflowContext() { TaskID = surveyData.TaskId, Action = "advance", Model = "task_survey", Context = surveyData.ID });
                //wfContext[]
                if ((wfContext.ContainsKey("Success") && Convert.ToBoolean(wfContext["Success"]) || wfContext.ContainsKey("nodes")))
                {//如果工作流可以往下走
                    var nodes = ((List<object>)wfContext["nodes"]);
                    if (nodes.Count > 0)
                    {
                        var ExeNode = ((Dictionary<string, object>)(nodes[0]));
                        object nodeId = ExeNode["node"].GetType().GetProperty("id").GetValue(ExeNode["node"]);

                        //nodeId = node.id.ToString();
                        if (!string.IsNullOrEmpty(nodeId.ToString()))
                        {
                            WorkflowContext wfExe = new WorkflowContext()
                            {
                                Model = "task_survey",
                                Action = "advance",
                                Context = surveyData.ID,
                                TaskID = surveyData.TaskId,
                                Remark = "完成了事件勘察表单的填写",
                                ExecuteNodes = new List<ExecuteNode>()
                            };
                            List<string> excutors = new List<string>();//用户id一个字符串，用户名一个字符串，用户名其实没有使用
                            if (exeUserIds != null && exeUserIds.Length > 0)
                            {
                                for (int i = 0; i < exeUserIds.Length; i++)
                                {
                                    excutors.Add(exeUserIds[i]);            //使用哪个用户来执行 这里需要不同的情况的来处理
                                    excutors.Add("用户名");//第二个参数 其实没有用到
                                }
                            }
                            else
                            {//默认使用当前用户来执行任务
                                excutors.Add(SysContext.WanJiangUserID);            //使用哪个用户来执行 这里需要不同的情况的来处理
                                excutors.Add("用户名");//第二个参数 其实没有用到
                            }
                            //如果由多个用户来执行，那 Executors可以是多个人，        
                            //这里任务的下一步仍然由填表人完成，某些情况些，会由指定的人来完成，比如：？想到了再说？？？
                            wfExe.ExecuteNodes.Add(new ExecuteNode() { Executors = new List<List<string>> { excutors }, NodeId = nodeId.ToString() });
                            workflowService.Execute(wfExe);//工作流向下一步
                            if (surveyData.TaskId == "MANUALLY_CREATE_TASK_ID")
                            {//如果该任务是手动创建
                                string latestWorkTaskId = workflowService.LatestWorkTaskId;

                                QueryDb.Update<Model.Form.task_survey>("set TaskId=@0 where ID=@1", new object[] { latestWorkTaskId, surveyData.ID });
                                return latestWorkTaskId;//返回所创建的任务Id,然后 work_task那边拿到以后，更新work_task自己的相关字段
                            }
                        }
                    }

                }
            }
            return null;
        }
        public override object Create(object postdata)
        {
            return WfCreate(postdata, new string[] { SysContext.WanJiangUserID });
        }

        public override object Update(object postdata)
        {
            var rev = base.Update(postdata);
            var wfId = AdvanceWorkflow(postdata);
            return rev;
        }
        */



        private Func<APIContext, object> Task_surveyService_OnGetAPIHandler(string id)
        {
            _sHBaseService= ServiceHelper.GetService("SHBaseService") as SHBaseService;
            //switch (id.ToUpper())
            //{
            //    case "FINISH":
            //        return Handle;
            //}
            //return null;
            return Handle;
        }


        private SHBaseService _sHBaseService;

        public object Handle(APIContext context)
        {
            var data = JsonHelper.DeserializeJsonToObject<task_surveyFinishReq>(context.Data);
            task_survey taskSurvey = new task_survey();
            List<work_task> workTasks = new List<work_task>();
            List<law_party> lawParties = new List<law_party>();
            if (!string.IsNullOrEmpty(data.TaskSurvey.TaskId)) return false;
            if (!string.IsNullOrEmpty(data.TaskSurvey.EventInfoId)) return false;
            taskSurvey = data.TaskSurvey;
            CreateInfo(taskSurvey,lawParties);
            //0:不予处罚
            //1:移交其它部门
            //2:处罚程序
            switch (data.TaskSurvey.ProcessingDecisions)
            {
                case 0:
                    // return EndEvent(taskSurvey);
                    break;
                case 1:
                    return ToEpart(taskSurvey);
                case 2:
                    //  return CreateCase(taskSurvey);
                    break;
            }
            return false;
        }


        /// <summary>
        /// 创建表单和当事人
        /// </summary>
        /// <param name="TaskSurvey"></param>
        /// <param name="law_Parties"></param>
        /// <returns></returns>
       private void CreateInfo(task_survey TaskSurvey, List<law_party> law_Parties)
        {
            var tasksurvey = base.Create(TaskSurvey) as case_Info;
            var _Lawpartys = ServiceHelper.GetService("law_partyService");
            if (law_Parties != null && law_Parties.Count > 0)//创建当事人
            {
                foreach (var l in law_Parties)//原始的当事人
                {
                    l.Associatedobjecttype = "task_survey";
                    l.AssociationobjectID = tasksurvey.ID;
                    _Lawpartys.Create(l);
                }
            }
        }
        public object EndEvent(task_survey TaskSurvey, List<law_party> lawParties)
        {
            try
            {
                QueryDb.BeginTransaction();
                CreateInfo(TaskSurvey, lawParties);
                _sHBaseService.UpdateWorkTaskState(TaskSurvey.TaskId, WorkTaskStatus.Close);//关闭任务
                _sHBaseService.UpdateEventState(TaskSurvey.EventInfoId, EventStatus.finish);//事件改为完成                         
            }
            catch (Exception)
            {
                QueryDb.AbortTransaction();
                return false;
            }
            QueryDb.CompleteTransaction();
            return true;
        }





        /// <summary>
        /// 勘察完结
        /// </summary>
        /// <param name="TaskSurvey"></param>
        /// <returns></returns>
        public object Finish(task_survey TaskSurvey,List<law_party> lawParties)
        {
            try {
                QueryDb.BeginTransaction();
                CreateInfo(TaskSurvey,lawParties);
                _sHBaseService.UpdateWorkTaskState(TaskSurvey.TaskId, WorkTaskStatus.Close);//关闭任务
                _sHBaseService.UpdateEventState(TaskSurvey.EventInfoId,EventStatus.finish);//事件改为完成                         
            }
            catch (Exception)
            {
                QueryDb.AbortTransaction();
                return false;
            }
            QueryDb.CompleteTransaction();
            return true;
        }

        /// <summary>
        /// 转交其他部门
        /// </summary>
        /// <param name="task_Survey"></param>
        /// <returns></returns>
        public object ToEpart(task_survey task_Survey)
        {
            //TODO 事件转交
            //TODO 写待办
            return null;
        }

        /// <summary>
        /// 创建案件
        /// </summary>
        /// <returns></returns>
        public object CreateCase(task_survey TaskSurvey,string url)
        {
            try
            {
                QueryDb.BeginTransaction();
                //创建当前表单信息
                CreateInfo(TaskSurvey);
                //关闭当前任务
                _sHBaseService.UpdateWorkTaskState(TaskSurvey.TaskId, WorkTaskStatus.Close);
                //TODO分配人员
                _sHBaseService.CreateSaveWorkTask(TaskSurvey.TaskId,TaskType.Case,"1","系统默认");
                //事件改为完成 
                _sHBaseService.UpdateEventState(TaskSurvey.EventInfoId, EventStatus.toCase);
                //转发待办
                _sHBaseService.CreateWorkrecor("165906044420484870", "案件待办",url,"案件待办","案件待办");
            }
            catch (Exception ex)
            {
                QueryDb.AbortTransaction();
                return false;
            }
            QueryDb.CompleteTransaction();
            return true;
        }


    }


}
