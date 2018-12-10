using System.Net;
using System.Data;
using System.Web.Mvc;
using WattsALoan1.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace WattsALoan1.Controllers
{
    public class EmployeesController : Controller
    {
        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection scWattsALoan = new SqlConnection(System.
                                                                            Configuration.
                                                                            ConfigurationManager.
                                                                            ConnectionStrings["csWattsALoan"].
                                                                            ConnectionString))
            {
                SqlCommand cmdEmployees = new SqlCommand("SELECT EmployeeID, EmployeeNumber, " +
                                                         "       FirstName, LastName, EmploymentTitle " +
                                                         "FROM HumanResources.Employees;",
                                                         scWattsALoan);

                scWattsALoan.Open();
                cmdEmployees.ExecuteNonQuery();

                SqlDataAdapter sdaEmployees = new SqlDataAdapter(cmdEmployees);
                DataSet dsEmployees = new DataSet("employees");

                sdaEmployees.Fill(dsEmployees);

                Employee staff = null;

                for (int i = 0; i < dsEmployees.Tables[0].Rows.Count; i++)
                {
                    DataRow drEmployee = dsEmployees.Tables[0].Rows[i];

                    staff = new Employee()
                    {
                        EmployeeID = int.Parse(drEmployee[0].ToString()),
                        EmployeeNumber = drEmployee[1].ToString(),
                        FirstName = drEmployee[2].ToString(),
                        LastName = drEmployee[3].ToString(),
                        EmploymentTitle = drEmployee[4].ToString()
                    };

                    employees.Add(staff);
                }
            }

            return employees;
        }

        // GET: Employees
        public ActionResult Index()
        {
            return View(GetEmployees());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            Employee employee = null;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            foreach (var staff in GetEmployees())
            {
                if (staff.EmployeeID == id)
                {
                    employee = staff;
                    break;
                }
            }

            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                using (SqlConnection scRentManagement = new SqlConnection(System.Configuration.
                                                                                 ConfigurationManager.
                                                                                 ConnectionStrings["csWattsALoan"].
                                                                                 ConnectionString))
                {
                    SqlCommand cmdEmployees = new SqlCommand("INSERT INTO HumanResources.Employees(EmployeeNumber, FirstName, LastName, EmploymentTitle) " +
                                                             "VALUES(N'" + collection["EmployeeNumber"] + "', " +
                                                             "       N'" + collection["FirstName"] + "', " +
                                                             "       N'" + collection["LastName"] + "', " +
                                                             "       N'" + collection["EmploymentTitle"] + "');",
                                                             scRentManagement);

                    scRentManagement.Open();
                    cmdEmployees.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = null;

            foreach (var staff in GetEmployees())
            {
                if (staff.EmployeeID == id)
                {
                    employee = staff;
                    break;
                }
            }

            return employee == null ? HttpNotFound() : (ActionResult)View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                using (SqlConnection scRentManagement = new SqlConnection(System.Configuration.
                                                                                 ConfigurationManager.
                                                                                 ConnectionStrings["csWattsALoan"].
                                                                                 ConnectionString))
                {
                    SqlCommand cmdEmployees = new SqlCommand("UPDATE HumanResources.Employees           " +
                                                             "SET    EmployeeNumber  = N'" + collection["EmployeeNumber"] + "', " +
                                                             "       FirstName       = N'" + collection["FirstName"] + "', " +
                                                             "       LastName        = N'" + collection["LastName"] + "', " +
                                                             "       EmploymentTitle = N'" + collection["EmploymentTitle"] + "'  " +
                                                             "WHERE  EmployeeID      =   " + id + ";",
                                                             scRentManagement);

                    scRentManagement.Open();
                    cmdEmployees.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int ?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = null;

            foreach (var staff in GetEmployees())
            {
                if (staff.EmployeeID == id)
                {
                    employee = staff;
                    break;
                }
            }

            return employee == null ? HttpNotFound() : (ActionResult)View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (SqlConnection scRentManagement = new SqlConnection(System.Configuration.
                                                                                 ConfigurationManager.
                                                                                 ConnectionStrings["csWattsALoan"].
                                                                                 ConnectionString))
                {
                    SqlCommand cmdEmployees = new SqlCommand("DELETE FROM HumanResources.Employees " +
                                                             "WHERE EmployeeID = " + id + ";",
                                                             scRentManagement);

                    scRentManagement.Open();
                    cmdEmployees.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
