using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ShoppingCartAPI.Data
{
    public class DapperContext : DbContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            if (configuration != null)
                _connectionString = configuration.GetConnectionString("GDCTConnection")
                                    ?? throw new InvalidOperationException("Connection string 'GDCTConnection' is not configured.");
            else
                throw new ArgumentNullException(nameof(configuration));
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
