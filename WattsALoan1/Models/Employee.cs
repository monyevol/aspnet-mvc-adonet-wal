using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WattsALoan1.Models
{
    public class Employee
    {
        [Display(Name = "Employee ID")]
        public int EmployeeID { get; set; }
        [Display(Name = "Employee #")]
        public string EmployeeNumber { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Employment Title")]
        public string EmploymentTitle { get; set; }

        public string Identification
        {
            get
            {
                return EmployeeNumber + " - " + FirstName + " " + LastName + " (" + EmploymentTitle + ")";
            }
        }
    }
}