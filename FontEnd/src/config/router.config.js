import { RouteView, BlankLayout, PageView } from '@/components/layouts'
/**
 *  配置路由信息
 *  其中meta包含：
 *  title：路由的描述信息,建议设置
 *  keepAlive:设置访问路由是否保持组件，功能参见Vue的keepAlive描述
 *  hiddenHeaderContent:设置是否需要隐藏页面头信息
 */

export const asyncRouterMap = [
  {
    path: '/',
    name: 'index',
    component: BlankLayout,
    meta: { title: '首页' },
    redirect: '/mission/dealt',
    children: [
      {
        path: '/dashboard',
        name: 'dashboard',
        redirect: '/dashboard/workplace',
        component: RouteView,
        meta: { title: '仪表盘', keepAlive: true },
        children: [
          {
            path: '/dashboard/monitor',
            name: 'Monitor',
            component: () => import('@/views/dashboard/Monitor'),
            meta: { title: '监控页', keepAlive: true }
          },
          {
            path: '/dashboard/workplace',
            name: 'Workplace',
            component: () => import('@/views/dashboard/Workplace'),
            meta: { title: '工作台', keepAlive: true }
          }
        ]
      },

      // forms
      {
        path: '/form',
        redirect: '/form/base-form',
        component: PageView,
        meta: { title: '表单页' },
        children: [
          {
            path: '/form/base-form',
            name: 'Index',
            component: () => import('@/views/form/index'),
            meta: { title: '表单列表', keepAlive: true }
          },
          {
            path: '/form/form-details',
            name: 'FormDetails',
            component: () => import('@/views/form/formDetails'),
            meta: { title: '表单详情', keepAlive: false }
          },
          {
            path: '/form/form-edit',
            name: 'FormEdit',
            component: () => import('@/views/form/formEdit'),
            meta: { title: '编辑表单', keepAlive: false }
          },
          {
            path: '/form/form-add-list',
            name: 'FormAddList',
            component: () => import('@/views/form/formAddList'),
            meta: { title: '新建表单列表', keepAlive: true }
          },
          {
            path: '/form/form-add',
            name: 'FormAdd',
            component: () => import('@/views/form/formAdd'),
            meta: { title: '新建表单', keepAlive: false }
          },
          {
            path: '/form/form-print',
            name: 'FormPrint',
            component: () => import('@/views/form/formPrint'),
            meta: { title: '表单打印', keepAlive: false }
          },
          {
            path: '/form/form-approval',
            name: 'FormApproval',
            component: () => import('@/views/form/formAddroval'),
            meta: { title: '表单审批', keepAlive: false }
          },
          {
            path: '/form/close-report',
            name: 'CloseReport',
            component: () => import('@/views/form/closeReport'),
            meta: { title: '结案报告', keepAlive: false }
          },
          {
            path: '/form/file-cover',
            name: 'FileCover',
            component: () => import('@/views/form/fileCover'),
            meta: { title: '卷宗封面', keepAlive: false }
          }
        ]
      },

      // list
      {
        path: '/list',
        name: 'list',
        component: PageView,
        redirect: '/list/query-list',
        meta: { title: '列表页' },
        children: [
          {
            path: '/list/query-list',
            name: 'QueryListWrapper',
            component: () => import('@/views/list/TableList'),
            meta: { title: '查询表格', keepAlive: true }
          },
          {
            path: '/list/tree-list',
            name: 'TreeList',
            component: () => import('@/views/list/TreeList'),
            meta: { title: '树目录表格', keepAlive: true }
          },
          {
            path: '/list/edit-table',
            name: 'EditList',
            component: () => import('@/views/list/TableInnerEditList'),
            meta: { title: '内联编辑表格', keepAlive: true }
          },
          {
            path: '/list/basic-list',
            name: 'BasicList',
            component: () => import('@/views/list/StandardList'),
            meta: { title: '标准列表', keepAlive: true }
          },
          {
            path: '/list/card',
            name: 'CardList',
            component: () => import('@/views/list/CardList'),
            meta: { title: '卡片列表', keepAlive: true }
          },
          {
            path: '/list/search',
            name: 'SearchList',
            component: () => import('@/views/list/search/SearchLayout'),
            redirect: '/list/search/article',
            meta: { title: '搜索列表', keepAlive: true },
            children: [
              {
                path: '/list/search/article',
                name: 'SearchArticles',
                component: () => import('../views/list/TableList'),
                meta: { title: '搜索列表（文章）' }
              },
              {
                path: '/list/search/project',
                name: 'SearchProjects',
                component: () => import('../views/list/TableList'),
                meta: { title: '搜索列表（项目）' }
              },
              {
                path: '/list/search/application',
                name: 'SearchApplications',
                component: () => import('../views/list/TableList'),
                meta: { title: '搜索列表（应用）' }
              }
            ]
          }
        ]
      },
      // profile
      {
        path: '/mission',
        name: 'Mission',
        component: RouteView,
        redirect: '',
        meta: { title: '我的任务' },
        children: [
          {
            path: '/mission/dealt',
            name: 'Dealt',
            component: () => import('@/views/mymission/Index'),
            meta: { title: '待办任务' }
          },
          {
            path: '/mission/donetask',
            name: 'DoneTask',
            component: () => import('@/views/mymission/doneTask'),
            meta: { title: '已办任务' }
          },
          {
            path: '/mission/mymission',
            name: 'MyMission',
            component: () => import('@/views/mymission/myMission'),
            meta: { title: '我发起的任务' }
          },
          {
            path: '/mission/eventinspection',
            name: 'EventInspeion',
            component: () => import('@/views/mymission/eventInspection'),
            meta: { title: '事件巡查' }
          },
          {
            path: '/mission/sceneInvestigation',
            name: 'SceneInvestigation',
            component: () => import('@/views/mymission/sceneInvestigation'),
            meta: { title: '现场勘查' }
          }
        ]
      },

      // result
      {
        path: '/result',
        name: 'result',
        component: PageView,
        redirect: '/result/success',
        meta: { title: '结果页' },
        children: [
          {
            path: '/result/success',
            name: 'ResultSuccess',
            component: () => import(/* webpackChunkName: "result" */ '@/views/result/Success'),
            meta: { title: '成功', keepAlive: false, hiddenHeaderContent: true }
          },
          {
            path: '/result/fail',
            name: 'ResultFail',
            component: () => import(/* webpackChunkName: "result" */ '@/views/result/Error'),
            meta: { title: '失败', keepAlive: false, hiddenHeaderContent: true }
          }
        ]
      },

      // Exception
      {
        path: '/exception',
        name: 'exception',
        component: RouteView,
        redirect: '/exception/403',
        meta: { title: '异常页' },
        children: [
          {
            path: '/exception/403',
            name: 'Exception403',
            component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/403'),
            meta: { title: '403' }
          },
          {
            path: '/exception/404',
            name: 'Exception404',
            component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/404'),
            meta: { title: '404' }
          },
          {
            path: '/exception/500',
            name: 'Exception500',
            component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/500'),
            meta: { title: '500' }
          }
        ]
      },

      // workbench
      {
        path: '/workbench',
        name: 'workbench',
        redirect: '/workbench/backlog',
        component: RouteView,
        meta: { title: '我的工作台' },
        children: [
          {
            path: '/workbench/backlog',
            name: 'Backlog',
            component: () => import(/* webpackChunkName: "fail" */ '@/views/workbench/Backlog')
          }
        ]
      }
    ]
  },
  {
    path: '*', redirect: '/404'
  }
]

/**
 * 基础路由
 * @type { *[] }
 */
export const constantRouterMap = [
  {
    path: '/login',
    name: 'login',
    component: () => import(/* webpackChunkName: "user" */ '@/views/user/Login'),
    meta: { title: '登录页面' }
  },
  {
    path: '/404',
    component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/404'),
    meta: { title: '404页面' }
  }
]
