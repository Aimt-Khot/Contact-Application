using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Contact_Application.Models
{
    public class ContactViewModel
    {
        [Key]
        public int ContactID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNO { get; set; }
        [Required]
        public string EmailID { get; set; }
    }
}