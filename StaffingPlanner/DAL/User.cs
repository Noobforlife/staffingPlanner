using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaffingPlanner.DAL
{
    public class User
    {
        public string Name;
        public Guid TeacherId;
        public Role UserRole;

        public User(string name, Guid teacherId, Role userRole)
        {
            Name = name;
            TeacherId = teacherId;
            UserRole = userRole;
        }

    }

}