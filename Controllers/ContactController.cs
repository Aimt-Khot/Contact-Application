using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Contact_Application.Data;
using Contact_Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Contact_Application.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContactController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        // GET: Contact
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection"))) 
            {
               sqlConnection.Open();
               SqlDataAdapter sqlda = new SqlDataAdapter("ContactVeiwAll", sqlConnection);
               sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
               sqlda.Fill(dtbl);
            }                        
            return View(dtbl);
        }
        // GET: Contact/CreateOrEdit/
        public IActionResult CreateOrEdit(int? id)
        {
            ContactViewModel contactViewModel = new ContactViewModel();
            if (id > 0)
            {
                contactViewModel = FetchcontactById(id);
            }
            return View(contactViewModel);
        }
        // POST: Contact/CreateOrEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int id, [Bind("ContactID,FirstName,LastName,PhoneNO,EmailID")] ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlcmd = new SqlCommand("ContactCreateOrEdit", sqlConnection);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("ContactID", contactViewModel.ContactID);
                    sqlcmd.Parameters.AddWithValue("FirstName", contactViewModel.FirstName);
                    sqlcmd.Parameters.AddWithValue("LastName", contactViewModel.LastName);
                    sqlcmd.Parameters.AddWithValue("PhoneNO", contactViewModel.PhoneNO);
                    sqlcmd.Parameters.AddWithValue("EmailID", contactViewModel.EmailID);
                    sqlcmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contactViewModel);
        }
        // GET: Contact/Delete/5
        public IActionResult Delete(int? id)
        {
            ContactViewModel contactViewModel = FetchcontactById(id);
            return View(contactViewModel);
        }
        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlcmd = new SqlCommand("ContactDeleteByID", sqlConnection);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("ContactID", id );
                sqlcmd.ExecuteNonQuery();
            }   
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public ContactViewModel FetchcontactById(int? id)
        {
            ContactViewModel contactViewModel = new ContactViewModel();           
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("ContactVeiwByID", sqlConnection);
                sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlda.SelectCommand.Parameters.AddWithValue("ContactID", id);
                sqlda.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    contactViewModel.ContactID = Convert.ToInt32(dtbl.Rows[0]["ContactID"].ToString()) ;
                    contactViewModel.FirstName = dtbl.Rows[0]["FirstName"].ToString();
                    contactViewModel.LastName = dtbl.Rows[0]["LastName"].ToString();
                    contactViewModel.PhoneNO = dtbl.Rows[0]["PhoneNO"].ToString();
                    contactViewModel.EmailID = dtbl.Rows[0]["EmailID"].ToString();
                }
                return contactViewModel;
            }
        }
    }
}
