using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaffingPlanner.Models
{
    public enum MessageType
    {
        Notification,
        WorkloadApproval,
        CourseApproval,
        Request,
        Reminder,
        Comment
    }
}