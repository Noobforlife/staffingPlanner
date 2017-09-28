using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StaffingPlanner.Models
{
    public class EmploymentLevel
    {
        private int _employment;

        public int Value
        {
            get { return _employment; }
        }

        /// <summary>
        /// Represents the degree of employment, where 100 is full employment.
        /// </summary>
        /// <param name="employment"></param>
        public EmploymentLevel(int employment)
        {
            if (employment < 0 || employment > 100)
            {
                throw new ArgumentOutOfRangeException();
            }

            _employment = employment;
        }

        public decimal GetWorkload()
        {
            return _employment;
        }

        public decimal GetFraction()
        {
            return _employment / 100m;
        }

        public override string ToString()
        {
            return _employment.ToString();
        }
    }


}