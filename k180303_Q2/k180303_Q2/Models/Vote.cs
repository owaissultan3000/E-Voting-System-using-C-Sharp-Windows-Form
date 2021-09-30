using System;
using System.Collections.Generic;
using System.Text;

namespace k180303_Q2.Models
{
    public class Vote
    {
        public string NIC;
        public string Position;
        public string ID;

        public Vote()
        {

        }

        public Vote(Vote vote)
        {
            NIC = vote.NIC;
            Position = vote.Position;
            ID = vote.ID;
        }
    }
}
