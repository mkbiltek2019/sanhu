define([],
function() {

    var exports = {
        type: 'list',
        options: {
            list: {
                columns: [{
                    name: "customerNo",
                    display: "客户编码",
                    type: "string"
                },
                {
                    name: "Ordernumber",
                    display: "订单号",
                    type: "string"
                },
                {
                    name: "Phone",
                    display: "联系电话",
                    type: "string"
                },
                {
                    name: "Address",
                    display: "地址",
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
                display: "客户编码",
                name: "customerNo",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "订单号",
                name: "Ordernumber",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "联系电话",
                name: "Phone",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "地址",
                name: "Address",
                editor: {
                    type: "string"
                },
                type: "string"
            },
            {
                display: "备注",
                name: "Remark",
                editor: {
                    type: "string"
                },
                type: "string"
            }],
            link: {},
            addins: {},
            defaultFilters: [{
                name: "{CurrentUserID}",
                id: "c1e858b6-48bc-4498-7cf6-42a7d486749a",
                value: {
                    rules: [{
                        field: "CreateUserID",
                        op: "equal",
                        value: "{CurrentUserID}",
                        type: "string"
                    }],
                    op: "and"
                }
            }]
        },
        dataset: 'web/dataset?model=case_saleOrder&viewname=list2'
    };
    exports.options.model = {
        name: 'case_saleOrder',
        title: '实例-销售订单'
    };

    exports.service = function service(page) {

};

    return exports;
});