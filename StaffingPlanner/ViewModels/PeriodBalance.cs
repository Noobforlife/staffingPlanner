using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaffingPlanner.ViewModels
{
    public class PeriodBalance
    {
        public int P1Balance { get; set; }
        public int P2Balance { get; set; }
        public int P3Balance { get; set; }
        public int P4Balance { get; set; }
        public PeriodBalance(int hoursPerPeriod, int p1Allocation, int p2Allocation, int p3Allocation, int p4Allocation)
        {
            P1Balance = hoursPerPeriod - p1Allocation;
            P2Balance = hoursPerPeriod - p2Allocation;
            P3Balance = hoursPerPeriod - p3Allocation;
            P4Balance = hoursPerPeriod - p4Allocation;
        }
    }
}