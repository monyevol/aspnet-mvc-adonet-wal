using System;
using System.ComponentModel.DataAnnotations;

namespace WattsALoan1.Models
{
    public class Payment
    {
        [Display(Name = "Payment ID")]
        public int PaymentID { get; set; }
        [Display(Name = "Receipt #")]
        public int ReceiptNumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }
        [Display(Name = "Employee ID")]
        public int EmployeeID { get; set; }
        [Display(Name = "Loan Contract ID")]
        public int LoanContractID { get; set; }
        [Display(Name = "Payment Amount")]
        public decimal PaymentAmount { get; set; }
        public decimal Balance { get; set; }
    }
}