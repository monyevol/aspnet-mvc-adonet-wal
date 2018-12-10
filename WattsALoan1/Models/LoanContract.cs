using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WattsALoan1.Models
{
    public class LoanContract
    {
        [Display(Name = "Loan Contract ID")]
        public int LoanContractID { get; set; }
        [Display(Name = "Loan #")]
        public int LoanNumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date Allocated")]
        public DateTime DateAllocated { get; set; }
        [Display(Name = "Employee ID")]
        public int EmployeeID { get; set; }
        [Display(Name = "First Name")]
        public string CustomerFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string CustomerLastName { get; set; }
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; } // => "Personal Loan";
        [Display(Name = "Loan Amount")]
        public decimal LoanAmount { get; set; }
        [Display(Name = "Interest Rate")]
        public decimal InterestRate { get; set; }
        public int Periods { get; set; }
        [Display(Name = "Monthly Payment")]
        public decimal MonthlyPayment { get; set; }
        [Display(Name = "Future Value")]
        public decimal FutureValue { get; set; }
        [Display(Name = "Interest Amount")]
        public decimal InterestAmount { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Payment Start Date")]
        public DateTime PaymentStartDate { get; set; }
    }
}