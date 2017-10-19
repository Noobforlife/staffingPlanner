using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaffingPlanner.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public virtual TeacherCourseWorkload Workload { get; set; }
        public virtual CourseOffering Course { get; set; }
        public DateTime Datetime { get; set; }
        public bool DOSonly { get; set; }
        public bool Seen { get; set; }
    }
}