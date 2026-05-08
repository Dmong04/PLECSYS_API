using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Context.Mongo.Config
{
    public class MongoDBSettings
    {
        public string? ConnectionString { get; set; }

        public string? DatabaseName { get; set; }
    }
}
