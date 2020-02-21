﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FD.Model.Enum
{
    /// <summary>
    /// 事件状态
    /// </summary>
    public enum EventStatus
    {
        /// <summary>
        /// 未分配
        /// </summary>
        Undistributed = 0,
        /// <summary>
        /// 分配
        /// </summary>
        Allocation = 1,
        /// <summary>
        /// 已关闭
        /// </summary>
        Close = 2
    }

    public enum WorkTaskStatus
    {
        /// <summary>
        /// 关闭
        /// </summary>
        Close = 0,
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 拒绝
        /// </summary>
        Reject = 2,
        /// <summary>
        /// 已移交
        /// </summary>
        HandOver = 3
    }

    
}