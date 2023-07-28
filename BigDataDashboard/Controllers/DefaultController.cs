using BigDataDashboard.DAL.Entites;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BigDataDashboard.Controllers
{
    public class DefaultController : Controller
    {
        private readonly string _connectionString = "Server =DESKTOP-D19MH5E\\SQLEXPRESS; initial catalog = CARPLATES; integrated security = true";
        public async Task<IActionResult> Index()
        {
            await using var connection = new SqlConnection(_connectionString);
            var values = (await connection.QueryAsync<PLATES>("Select * from PLATES")).AsList();
            return View(values);
        }
    }
}
