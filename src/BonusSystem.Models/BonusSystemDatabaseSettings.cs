using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusSystem.Models
{
    public class BonusSystemDatabaseSettings
    {
        public string BonusCardCollection { get; set; }

        public string ClientCollection { get; set; }

        public string DebitCollection { get; set; }

        public string CreditCollection { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
