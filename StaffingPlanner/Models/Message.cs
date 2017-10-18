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
        public TeacherCourseWorkload Workload { get; set; }
        public CourseOffering Course { get; set; }
        public bool DOSonly { get; set; }
    }
}