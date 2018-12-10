using System;
using System.Data;
using System.Web.Mvc;
using WattsALoan1.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace WattsALoan1.Controllers
{
    public class LoansContractsController : Controller
    {
        public List<LoanContract> GetLoanContracts()
        {
            List<LoanContract> contracts = new List<LoanContract>();

            using (SqlConnection scWattsALoan = new SqlConnection(System.Configuration.
                                                                         ConfigurationManager.
                                                                         ConnectionStrings["csWattsALoan"].
                                                                         ConnectionString))
            {
                SqlCommand cmdLoanContracts = new SqlCommand("SELECT LoanContractID, LoanNumber, DateAllocated, EmployeeID, " +
                                                             "       CustomerFirstName, CustomerLastName, LoanType, " +
                                                             "       LoanAmount, InterestRate, Periods, MonthlyPayment, " +
                                                             "       FutureValue, InterestAmount, PaymentStartDate " +
                                                             "FROM Management.LoanContracts;",
                                                         scWattsALoan);

                scWattsALoan.Open();
                cmdLoanContracts.ExecuteNonQuery();

                SqlDataAdapter sdaLoanContracts = new SqlDataAdapter(cmdLoanContracts);
                DataSet dsLoanContracts = new DataSet("loans-contracts");

                sdaLoanContracts.Fill(dsLoanContracts);

                LoanContract contract = null;

                for (int i = 0; i < dsLoanContracts.Tables[0].Rows.Count; i++)
                {
                    DataRow drLoanContract = dsLoanContracts.Tables[0].Rows[i];

                    contract = new LoanContract()
                    {
                        LoanContractID = int.Parse(drLoanContract[0].ToString()),
                        LoanNumber = int.Parse(drLoanContract[1].ToString()),
                        DateAllocated = DateTime.Parse(drLoanContract[2].ToString()),
                        EmployeeID = int.Parse(drLoanContract[3].ToString()),
                        CustomerFirstName = drLoanContract[4].ToString(),
                        CustomerLastName = drLoanContract[5].ToString(),
                        LoanType = drLoanContract[6].ToString(),
                        LoanAmount = decimal.Parse(drLoanContract[7].ToString()),
                        InterestRate = decimal.Parse(drLoanContract[8].ToString()),
                        Periods = int.Parse(drLoanContract[9].ToString()),
                        MonthlyPayment = decimal.Parse(drLoanContract[10].ToString()),
                        FutureValue = decimal.Parse(drLoanContract[11].ToString()),
                        InterestAmount = decimal.Parse(drLoanContract[12].ToString()),
                        PaymentStartDate = DateTime.Parse(drLoanContract[13].ToString())
                    };

                    contracts.Add(contract);
                }
            }

            return contracts;
        }

        // GET: LoansContracts
        public ActionResult Index()
        {
            return View(GetLoanContracts());
        }

        // GET: LoansContracts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoansContracts/LoanContractStartUp
        public ActionResult LoanContractStartUp()
        {
            return View();
        }

        // GET: LoansContracts/LoanContractPreparation
        public ActionResult LoanContractPreparation(string EmployeeID, string LoanAmount, string InterestRate, string Periods,
                                                    FormCollection collection)
        {
            //int loanNbr = 100000;
            Random rndNumber = new Random();

            ViewBag.LoanNumber = rndNumber.Next(100001, 999999).ToString();

            // Create a list of loans types for a combo box
            List<SelectListItem> loanTypes = new List<SelectListItem>();

            loanTypes.Add(new SelectListItem() { Text = "Personal Loan", Value = "Personal Loan" });
            loanTypes.Add(new SelectListItem() { Text = "Car Financing", Value = "Car Financing" });
            loanTypes.Add(new SelectListItem() { Text = "Boat Financing", Value = "Boat Financing" });
            loanTypes.Add(new SelectListItem() { Text = "Furniture Purchase", Value = "Furniture Purchase" });
            loanTypes.Add(new SelectListItem() { Text = "Musical Instrument", Value = "Musical Instrument" });

            // Store the list in a View Bag so it can be access by a combo box
            ViewBag.LoanType = loanTypes;

            if (!string.IsNullOrEmpty(EmployeeID))
            {
                EmployeesController ec = new EmployeesController();

                using (SqlConnection scWattsALoan = new SqlConnection(System.Configuration.
                                                                             ConfigurationManager.
                                                                             ConnectionStrings["csWattsALoan"].
                                                                             ConnectionString))
                {
                    foreach (var staff in ec.GetEmployees())
                    {
                        if (staff.EmployeeID == int.Parse(EmployeeID))
                        {
                            ViewBag.EmployeeDetails = staff.EmployeeNumber + " - " +
                                                      staff.FirstName + " " + staff.LastName +
                                                      " (" + staff.EmploymentTitle + ")";
                            break;
                        }
                    }
                }
            }

            int periods = 0;
            decimal principal = 0, interestRate = 0;

            if (!string.IsNullOrEmpty(LoanAmount))
            {
                principal = decimal.Parse(LoanAmount);
            }

            if (!string.IsNullOrEmpty(InterestRate))
            {
                interestRate = decimal.Parse(InterestRate) / 100;
            }

            if (!string.IsNullOrEmpty(Periods))
            {
                periods = int.Parse(Periods);
            }

            decimal interestAmount = principal * interestRate * periods / 12;
            decimal futureValue = principal + interestAmount;
            decimal monthlyPayment = futureValue / periods;

            ViewBag.FutureValue = futureValue.ToString("F");
            ViewBag.InterestAmount = interestAmount.ToString("F");
            ViewBag.MonthlyPayment = monthlyPayment.ToString("F");

            return View();
        }

        // GET: LoansContracts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoansContracts/Create
        [HttpPost]
        public ActionResult Create(string LoanNumber, string EmployeeID,
                                   string InterestRate, string Periods, string FutureValue,
                                   string MonthlyPayment, string InterestAmount,
                                   FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (!string.IsNullOrEmpty(EmployeeID))
                {
                    using (SqlConnection scWattsALoan = new SqlConnection(System.Configuration.
                                                                             ConfigurationManager.
                                                                             ConnectionStrings["csWattsALoan"].
                                                                             ConnectionString))
                    {
                        SqlCommand cmdLoanAllocation =
                            new SqlCommand("INSERT Management.LoanContracts(LoanNumber, DateAllocated, EmployeeID, " +
                                           "                                CustomerFirstName, CustomerLastName, LoanType, " +
                                           "                                LoanAmount, InterestRate, Periods, MonthlyPayment, " +
                                           "                                FutureValue, InterestAmount, PaymentStartDate) " +
                                           "VALUES(" + int.Parse(LoanNumber) + ", N'" + collection["DateAllocated"] +
                                           "', " + int.Parse(collection["EmployeeID"]) + ", N'" +
                                           collection["CustomerFirstName"] + "', N'" + collection["CustomerLastName"] +
                                           "', N'" + collection["LoanType"] + "', " + decimal.Parse(collection["LoanAmount"]) +
                                           ", " + decimal.Parse(collection["InterestRate"]) + ", " +
                                           int.Parse(collection["Periods"]) + ", " + decimal.Parse(MonthlyPayment) +
                                           ", " + decimal.Parse(FutureValue) + ", " + decimal.Parse(InterestAmount) +
                                           ", N'" + collection["PaymentStartDate"] + "');");
                        cmdLoanAllocation.Connection = scWattsALoan;

                        scWattsALoan.Open();

                        cmdLoanAllocation.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: LoansContracts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoansContracts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: LoansContracts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoansContracts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: LoansContracts/ReviewCustomersAccounts
        public ActionResult ReviewCustomersAccounts()
        {
            return View();
        }
    }
}
