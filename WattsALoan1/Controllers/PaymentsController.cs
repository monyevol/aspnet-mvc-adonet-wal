using System;
using System.Data;
using System.Web.Mvc;
using WattsALoan1.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace WattsALoan1.Controllers
{
    public class PaymentsController : Controller
    {
        private List<Payment> payments = new List<Payment>();

        public List<Payment> GetPayments()
        {
            using (SqlConnection scWattsALoan = new SqlConnection(System.Configuration.
                                                                         ConfigurationManager.
                                                                         ConnectionStrings["csWattsALoan"].
                                                                         ConnectionString))
            {
                SqlCommand cmdPayments = new SqlCommand("SELECT PaymentID, ReceiptNumber, " +
                                                         "      PaymentDate, EmployeeID, " +
                                                         "      LoanContractID, PaymentAmount, Balance " +
                                                         "FROM  Management.Payments;",
                                                         scWattsALoan);

                scWattsALoan.Open();
                cmdPayments.ExecuteNonQuery();

                SqlDataAdapter sdaPayments = new SqlDataAdapter(cmdPayments);
                DataSet dsPayments = new DataSet("payments");

                sdaPayments.Fill(dsPayments);

                for (int i = 0; i < dsPayments.Tables[0].Rows.Count; i++)
                {
                    DataRow drPayment = dsPayments.Tables[0].Rows[i];

                    payments.Add(new Payment()
                    {
                        PaymentID = int.Parse(drPayment[0].ToString()),
                        ReceiptNumber = int.Parse(drPayment[1].ToString()),
                        PaymentDate = DateTime.Parse(drPayment[2].ToString()),
                        EmployeeID = int.Parse(drPayment[3].ToString()),
                        LoanContractID = int.Parse(drPayment[4].ToString()),
                        PaymentAmount = decimal.Parse(drPayment[5].ToString()),
                        Balance = decimal.Parse(drPayment[6].ToString())
                    });
                }
            }

            return payments;
        }

        // GET: Payments
        public ActionResult Index()
        {
            return View(GetPayments());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoansContracts/PaymentStartUp
        public ActionResult PaymentStartUp()
        {
            return View();
        }

        // GET: LoansContracts/LoanContractPreparation
        public ActionResult PaymentPreparation(string EmployeeID, string LoanContractID)
        {
            decimal paymentAmount = 0;
            decimal previousBalance = 0;
            Random rndNumber = new Random();

            ViewBag.ReceiptNumber = rndNumber.Next(100001, 999999);

            using (SqlConnection scWattsALoan = new SqlConnection(System.Configuration.
                                                                         ConfigurationManager.
                                                                         ConnectionStrings["csWattsALoan"].
                                                                         ConnectionString))
            {
                // Locate the employee based on the employee number that was entered
                SqlCommand cmdEmployees = new SqlCommand("SELECT FirstName, LastName, EmploymentTitle " +
                                                         "FROM   HumanResources.Employees " +
                                                         "WHERE  EmployeeID = " + EmployeeID + ";")
                {
                    Connection = scWattsALoan
                };

                scWattsALoan.Open();

                // Store the employee in a data set.
                SqlDataAdapter sdaEmployees = new SqlDataAdapter(cmdEmployees);
                DataSet dsEmployees = new DataSet("employees");

                sdaEmployees.Fill(dsEmployees);

                // If there is an employee record for the employee number that was provided, ...
                if (dsEmployees.Tables[0].Rows.Count > 0)
                {
                    // ... create a string using that employee information and store that string in a view bag.
                    ViewBag.EmployeeDetails = dsEmployees.Tables[0].Rows[0][0].ToString() + " " + dsEmployees.Tables[0].Rows[0][1].ToString() + " (" + dsEmployees.Tables[0].Rows[0][2].ToString() + ")";
                }
                else
                {
                    // If there is no employee for the employee number that was provided, don't sweat.
                    return RedirectToAction("LoanContractStartUp");
                }
            }

            // Open a contract based on the loan number that was provided
            using (SqlConnection scWattsALoan = new SqlConnection(System.Configuration.
                                                                         ConfigurationManager.
                                                                         ConnectionStrings["csWattsALoan"].
                                                                         ConnectionString))
            {
                SqlCommand cmdContracts = new SqlCommand("SELECT CustomerFirstName, CustomerLastName, " +
                                                         "       LoanType, LoanAmount, " +
                                                         "       InterestRate, Periods, MonthlyPayment, " +
                                                         "       EmployeeID, FutureValue, " +
                                                         "       InterestAmount, PaymentStartDate " +
                                                         "FROM   Management.LoanContracts " +
                                                         "WHERE  LoanContractID = " + LoanContractID + ";");
                cmdContracts.Connection = scWattsALoan;

                scWattsALoan.Open();

                SqlDataAdapter sdaContracts = new SqlDataAdapter(cmdContracts);
                DataSet dsContracts = new DataSet("loans-contracts");

                sdaContracts.Fill(dsContracts);

                // If the loan number exists, ...
                if (dsContracts.Tables[0].Rows.Count > 0)
                {
                    // Prepare some information that will be displayed on a form
                    ViewBag.LoanDetails = "Granted to " + dsContracts.Tables[0].Rows[0]["CustomerFirstName"].ToString() + " " +
                                          dsContracts.Tables[0].Rows[0]["CustomerLastName"].ToString() + " for a " +
                                          dsContracts.Tables[0].Rows[0]["LoanType"].ToString() + " loan of " +
                                          dsContracts.Tables[0].Rows[0]["LoanAmount"].ToString() + " (" +
                                          dsContracts.Tables[0].Rows[0]["InterestRate"].ToString() + "% interest rate for " +
                                          dsContracts.Tables[0].Rows[0]["Periods"].ToString() + " months).";
                    paymentAmount = decimal.Parse(dsContracts.Tables[0].Rows[0]["MonthlyPayment"].ToString());
                    /* We need the future value of the loan. 
                     * It could be used as the previous balance if no payment has even been made on the loan. */
                    previousBalance = decimal.Parse(dsContracts.Tables[0].Rows[0]["FutureValue"].ToString());
                }
            }

            // Open the list of payments if it contains some records
            using (SqlConnection scWattsALoan = new SqlConnection(System.Configuration.
                                                                         ConfigurationManager.
                                                                         ConnectionStrings["csWattsALoan"].
                                                                         ConnectionString))
            {
                // Get the list of payments that use the provided loan number
                SqlCommand cmdContracts = new SqlCommand("SELECT Balance " +
                                                         "FROM   Management.Payments " +
                                                         "WHERE LoanContractID = " + LoanContractID)
                {
                    Connection = scWattsALoan
                };

                scWattsALoan.Open();

                // Store the list of payments in a data set
                SqlDataAdapter sdaPayments = new SqlDataAdapter(cmdContracts);
                DataSet dsPayments = new DataSet("payments");

                sdaPayments.Fill(dsPayments);

                // If there is at least one payment made for the provided loan number, ...
                if (dsPayments.Tables[0].Rows.Count > 0)
                {
                    // ... scan the list of record from begining to end
                    for (int i = 0; i < dsPayments.Tables[0].Rows.Count; i++)
                    {
                        // The goal is to get the last balance that was set for the loan
                        previousBalance = decimal.Parse(dsPayments.Tables[0].Rows[i]["Balance"].ToString());
                    }
                }

                /* If no payment was ever made for the loan, then the previous balance is the future value.
                 * If there was at least one payment made for the loan, then a balance had been set.
                 * That balance will be used as the previous balance. */

                // Calculate the ne balance by monthly payment from the previous balance
                // Prepare the values to be sent to a form
                ViewBag.Balance = previousBalance - paymentAmount;
                ViewBag.PaymentAmount = paymentAmount;
                ViewBag.PreviousBalance = previousBalance;
            }

            return View();
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (!string.IsNullOrEmpty(collection["EmployeeID"]))
                {
                    using (SqlConnection scWattsALoan = new SqlConnection(System.Configuration.
                                                                                 ConfigurationManager.
                                                                                 ConnectionStrings["csWattsALoan"].
                                                                                 ConnectionString))
                    {
                        SqlCommand cmdLoanPayment =
                            new SqlCommand("INSERT Management.LoanPayment " +
                                           "VALUES(" + collection["ReceiptNumber"] + ", " + collection["EmployeeID"] +
                                           ", " + collection["LoanContractID"] + ", N'" + collection["PaymentDate"] +
                                           "', " + collection["PaymentAmount"] + ", " + collection["Balance"] + ");");

                        cmdLoanPayment.Connection = scWattsALoan;

                        scWattsALoan.Open();

                        cmdLoanPayment.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Payments/Edit/5
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

        // GET: Payments/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Payments/Delete/5
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
    }
}
