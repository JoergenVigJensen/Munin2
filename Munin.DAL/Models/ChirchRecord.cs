using System;

namespace Munin.DAL.Models
{
    public class ChirchRecord
    {
        public int ChirchRecorddId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ChirchEvent Event { get; set; }

        public DateTime Date { get; set; }

        public int PersonId { get; set; }

    }
}
