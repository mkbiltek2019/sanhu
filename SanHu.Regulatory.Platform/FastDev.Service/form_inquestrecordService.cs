﻿using FastDev.Common;
using FastDev.DevDB;
using FastDev.Model.Entity;
using FD.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastDev.Service
{
    /// <summary>
    /// 勘验笔录
    /// </summary>
    public class Form_inquestrecordService : SHBaseService, IService
    {
        public Form_inquestrecordService()
        {
            OnGetAPIHandler += Form_inquestrecordService_OnGetAPIHandler;
        }

        private Func<APIContext, object> Form_inquestrecordService_OnGetAPIHandler(string id)
        {
            switch (id.ToUpper())
            {
                case "FINISH":
                    return Finish;
            }
            return null;
        }
        public object Finish(APIContext context)
        {
            var data = JsonHelper.DeserializeJsonToObject<form_inquestrecordFinishReq>(context.Data);

            QueryDb.BeginTransaction();
            try
            {
                data.forminquestrecord.TaskId = data.SourceTaskId;
                data.forminquestrecord.EventInfoId = data.EventInfoId;
                var form = ServiceHelper.GetService("form_inquestrecord").Create(data.forminquestrecord);
                if (string.IsNullOrEmpty((string)form)) throw new Exception();
                var formid = form.ToString();
                if (data.lawStaffs != null)
                {
                    foreach (var l in data.lawStaffs)
                    {
                        l.Associatedobjecttype = "form_inquestrecord";
                        l.AssociatedobjectID = formid;
                        l.CreateDate = DateTime.Now;
                        l.CreateUserID = SysContext.WanJiangUserID;
                        QueryDb.Insert(l);
                    }
                }
                if (data.lawParties != null)
                {
                    foreach (var l in data.lawParties)
                    {
                        l.Associatedobjecttype = "form_inquestrecord";
                        l.AssociationobjectID = formid;
                        l.CreateDate = DateTime.Now;
                        l.CreateUserID = SysContext.WanJiangUserID;
                        QueryDb.Insert(l);
                    }
                }
                CreatTasksAndCreatWorkrecor(data.NextTasks, data.SourceTaskId);
                //PDF打印预生成
                var PDFSerivce = ServiceHelper.GetService("form_printPDFService") as form_printPDFService;
                PDFSerivce.AsposeToPdf(new APIContext() { Data = @"{""formId"":""" + formid + @""",""formType"":""form_inquestrecord""}" });
                QueryDb.CompleteTransaction();
            }
            catch (Exception e)
            {
                QueryDb.AbortTransaction();
                return false;
            }
            
            return true;
        }
    }
}
