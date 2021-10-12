using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Contact_Application.Models;

namespace Contact_Application.Data
{
    public class Contact_ApplicationContext : DbContext
    {
        public Contact_ApplicationContext (DbContextOptions<Contact_ApplicationContext> options)
            : base(options)
        {
        }
        public DbSet<Contact_Application.Models.ContactViewModel> ContactViewModel { get; set; }
    }
}
