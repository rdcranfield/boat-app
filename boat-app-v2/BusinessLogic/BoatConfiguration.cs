namespace boat_app_v2.BusinessLogic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

public class BoatConfiguration : DbConfiguration
{
    public BoatConfiguration()
    {
        SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
    }
}