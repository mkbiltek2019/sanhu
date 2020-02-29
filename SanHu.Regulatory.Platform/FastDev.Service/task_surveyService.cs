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
            task_survey task = new task_survey();
            task.CaseId = "123";
            task.EventInfoId = "123";
            task.EventType = "case_info";
            task.ExistCrim = "true";
            task.Result = "123";
            task.TaskId= "71be0281-2ced-42a4-bc92-396791a8f197";
            task.ProcessingDecisions = 1;
            law_party law1 = new law_party();
            law1.Name = "kk";
            law1.address = "china";
            law1.Gender = "男";
            law1.CaseId = "122";
            law_party law2 = new law_party();
            law2.Name = "kk2";
            law2.address = "china";
            law2.Gender = "女";
            law2.CaseId = "111";
            List<law_party> law_Parties = new List<law_party>();
            law_Parties.Add(law1);
            law_Parties.Add(law2);
            task_surveyFinishReq tq = new task_surveyFinishReq();
            tq.TaskSurvey = task;
            tq.LawParties = law_Parties;
            work_task workTask = new work_task();
            workTask.CaseID = "123";
            workTask.LaskTaskId = "1221";
            workTask.TaskContent = "手动创建新任务";
            workTask.TaskType = "创建案件";
            tq.NextTasks = new work_task[] { workTask };
            tq.LawParties = law_Parties;
            var M = JsonConvert.SerializeObject(tq);


            _sHBaseService = ServiceHelper.GetService("SHBaseService") as SHBaseService;
            return Handle;
        }



        private SHBaseService _sHBaseService;

        public object Handle(APIContext context)
        {
            var data = JsonHelper.DeserializeJsonToObject<task_surveyFinishReq>(context.Data);
            if (data.TaskSurvey == null) return null;
            QueryDb.BeginTransaction();
            try
            {
                CreateInfo(data.TaskSurvey, data.LawParties);
                switch (data.TaskSurvey.ProcessingDecisions)
                {
                    case 0:
                         EndEvent(data.SourceTaskId, data.EventInfoId);
                        break;
                    default:
                        _sHBaseService.CreatTasksAndCreatWorkrecor(data.NextTasks, data.SourceTaskId);
                        break;
                }
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
        /// 创建表单和当事人
        /// </summary>
        /// <param name="TaskSurvey"></param>
        /// <param name="law_Parties"></param>
        /// <returns></returns>
       private void CreateInfo(task_survey TaskSurvey, List<law_party> law_Parties)
        {
            var tasksurvey = base.Create(TaskSurvey) as string;
            var _Lawpartys = ServiceHelper.GetService("law_partyService");
            if (law_Parties != null && law_Parties.Count > 0)//创建当事人
            {
                foreach (var l in law_Parties)//原始的当事人
                {
                    l.Associatedobjecttype = "task_survey";
                    l.AssociationobjectID = tasksurvey;
                  //  l.ID = Guid.NewGuid().ToString();
                   // QueryDb.Insert(l);
                    ServiceHelper.GetService("law_partyService").Create(l);
                }
            }
        }

        /// <summary>
        /// 结束事件和任务
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="EventId"></param>
        /// <returns></returns>
        public object EndEvent(string TaskId,string EventId)
        {
            try
            {
                QueryDb.BeginTransaction();
                _sHBaseService.UpdateWorkTaskState(TaskId, WorkTaskStatus.Close);//关闭任务
                _sHBaseService.UpdateEventState(EventId, EventStatus.finish);//事件改为完成                         
            }
            catch (Exception)
            {
                QueryDb.AbortTransaction();
                return false;
            }
            QueryDb.CompleteTransaction();
            return true;
        }


        ///// <summary>
        ///// 创建后续任务
        ///// </summary>
        ///// <returns></returns>
        //public object CreatTask(work_task[] NextTasks,string sourcetaskid)
        //{
        //    if (NextTasks == null) return null;
        //    if (NextTasks.Length < 1) return null;
        //    foreach (var Task in NextTasks)
        //    {
        //        Task.LaskTaskId = sourcetaskid;
        //       var task= _sHBaseService.SaveWorkTask(Task);
        //        _sHBaseService.CreateWorkrecor(Task.AssignUsersID, "案件待办", Task.RemoteLinks+"?taskid="+task.ID, Task.TaskType, Task.TaskContent);
        //    }
        //    return true;
        //}


        /*

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
                //CreateInfo(TaskSurvey);
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

    */
    }


}