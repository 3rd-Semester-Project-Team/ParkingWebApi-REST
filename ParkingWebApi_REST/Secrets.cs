using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_REST
{
    public class Secrets
    {
        public static readonly string ConnectionString =
            "Server=tcp:sensor-sql-server.database.windows.net,1433;Initial Catalog=Sensor_DataBase;Persist Security Info=False;User ID=******;Password=******;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
       

    }
}
