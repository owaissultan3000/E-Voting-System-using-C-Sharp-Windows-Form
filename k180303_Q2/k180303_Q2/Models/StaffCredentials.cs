using System;
using System.Collections.Generic;
using System.Text;

namespace k180303_Q2.Models
{
   public class StaffCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string StationId { get; set; }
        public StaffCredentials()
        {

        }

        public StaffCredentials(string email,string password,string satationid)
        {
            Email = email;
            Password = password;
            StationId = satationid;
        }

        public StaffCredentials(StaffCredentials staffCredentials)
        {
            Email = staffCredentials.Email;
            Password = staffCredentials.Password;
            StationId = staffCredentials.StationId;
        }
    }
}
