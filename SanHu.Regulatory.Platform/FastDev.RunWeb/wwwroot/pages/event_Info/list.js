define([],
function() {

    var exports = {
        type: 'list',
        options: {
            list: {
                columns: [{
                    name: "objId",
                    display: "主键ID",
                    type: "string"
                },
                {
                    name: "gldwOid",
                    display: "管理单位",
                    type: "string"
                },
                {
                    name: "isDangerReport",
                    display: "是否是隐患事件",
                    type: "string"
                },
                {
                    name: "dorgId",
                    display: "主管单位",
                    type: "string"
                }]
            },
            common: {
                formShowType: "tab",
                formShowPosition: "top",
                dialogWidth: 700,
                dialogHeight: 500
            },
            type: "list",
            filterFields: [{
                display: "主键ID",
                name: "objId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "管理单位",
                name: "gldwOid",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "上报类型：企业号10、公众号20",
                name: "reportType",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件编码",
                name: "evtCode",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件类型ID",
                name: "evtTypeId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "多个事件类型",
                name: "evtTypeIds",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件照片",
                name: "evtFileId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "位置照片",
                name: "posFileId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "x",
                name: "x1",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "y",
                name: "y1",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "百度坐标经度",
                name: "lng",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "百度坐标纬度",
                name: "lat",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "地址信息",
                name: "address",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件描述",
                name: "remark",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件对象ID",
                name: "targetId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件对象名称",
                name: "targetName",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件状态",
                name: "evtState",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "上报人微信ID",
                name: "wxUserId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "微信坐标",
                name: "wxCoordinate",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "排序",
                name: "orderNo",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "扩展属性",
                name: "extField",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "创建时间",
                name: "createTime",
                editor: {
                    type: "datepicker"
                },
                type: "datepicker"
            },
            {
                display: "完成时间",
                name: "finishTime",
                editor: {
                    type: "datepicker"
                },
                type: "datepicker"
            },
            {
                display: "上报时间",
                name: "reportTime",
                editor: {
                    type: "datepicker"
                },
                type: "datepicker"
            },
            {
                display: "事件状态图标编码",
                name: "stateIconCode",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "处理人",
                name: "finishUserId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "处理人姓名",
                name: "finishUserName",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "处理图片",
                name: "finishFileId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "上报照片url",
                name: "reportPhotoUrl",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "上报照片",
                name: "reportPhotoDto",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "整改照片url",
                name: "finishPhotoUrl",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "整改照片",
                name: "finishPhotoDto",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "上报照片GroupId",
                name: "reportPhotoGroupId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "整改照片GroupId",
                name: "finishPhotoGroupId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件状态名",
                name: "evtStateName",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "needNotice",
                name: "needNotice",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件类型显示名称",
                name: "evtTypeDisplayName",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "管理单位名称",
                name: "gldwName",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "上报人姓名",
                name: "reporterName",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "上报照片集合",
                name: "reportPhotoIdList",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "整改照片集合",
                name: "finishPhotoList",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "处理人姓名",
                name: "dealerName",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "审核状态名称",
                name: "auditStateName",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "审核状态",
                name: "auditState",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "整改期限时间",
                name: "finishLimitTime",
                editor: {
                    type: "datepicker"
                },
                type: "datepicker"
            },
            {
                display: "事件编号",
                name: "evtNos",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "对象ID",
                name: "objectId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "责任单位IDS",
                name: "responseIds",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "责任单位ID集合",
                name: "responseIdList",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "责任单位ID",
                name: "responseId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "责任单位名称",
                name: "responseName",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "查询方式",
                name: "queryBy",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "是否删除",
                name: "deleteFlagEk",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "关联表主键",
                name: "responseRefId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "查询时间",
                name: "dateRange",
                editor: {
                    type: "datepicker"
                },
                type: "datepicker"
            },
            {
                display: "查询开始时间",
                name: "startDay",
                editor: {
                    type: "datepicker"
                },
                type: "datepicker"
            },
            {
                display: "查询结束时间",
                name: "endDay",
                editor: {
                    type: "datepicker"
                },
                type: "datepicker"
            },
            {
                display: "上报时间段",
                name: "dateType",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "上报人",
                name: "reporterIds",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件状态List",
                name: "evtStateList",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件类型List",
                name: "evtTypeList",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "管理单位List",
                name: "gldwOidList",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "关联表主键集合",
                name: "responseRefList",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "关键字",
                name: "keyWord",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "公众事件查询",
                name: "queryModeMp",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "是否超期",
                name: "timeoutFlag",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "完成期限查询条件，未超期和超期的条件",
                name: "finishLimitStartTime",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "完成期限查询条件，未超期和超期的条件",
                name: "finishLimitEndTime",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "创建人",
                name: "createUser",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "tipId",
                name: "tipId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "事件类型图标编码",
                name: "evtTypeIconCode",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "整改期限ID",
                name: "finishLimitId",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "填写的整改时间（小时）",
                name: "timeLimit",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "整改描述",
                name: "finishRemark",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "整改期限类型（1手选 2填写）",
                name: "limitType",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "是否是隐患事件",
                name: "isDangerReport",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "主管单位",
                name: "dorgId",
                editor: {
                    type: "string"
                },
                type: "string"
            }],
            link: {},
            addins: {}
        },
        dataset: 'web/dataset?model=event_info&viewname=list'
    };
    exports.options.model = {
        name: 'event_info',
        title: '事件信息'
    };

    exports.service = function service(page) {

};

    return exports;
});