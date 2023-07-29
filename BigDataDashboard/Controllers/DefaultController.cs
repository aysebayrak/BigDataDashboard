using BigDataDashboard.DAL.DTOs;
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
            //var values = (await connection.QueryAsync<PLATES>("Select * from PLATES")).AsList();
            //return View(values);



            //markaların sayılarına göre
            var brandMax = (await connection.QueryAsync<BrandConclusion>("SELECT TOP 1 BRAND, COUNT(*) AS count FROM PLATES GROUP BY BRAND ORDER BY count DESC")).FirstOrDefault();
            var brandMin = (await connection.QueryAsync<BrandConclusion>("SELECT TOP 1 BRAND, COUNT(*) AS count FROM PLATES GROUP BY BRAND ORDER BY count ASC")).FirstOrDefault();


            // renk
            var colorMax = (await connection.QueryAsync<ColorConclusion>("SELECT TOP 1 COLOR, COUNT(*) AS count FROM PLATES GROUP BY COLOR ORDER BY count DESC")).FirstOrDefault();
            var colorMin = (await connection.QueryAsync<ColorConclusion>("SELECT TOP 1 COLOR, COUNT(*) AS count FROM PLATES GROUP BY COLOR ORDER BY count ASC")).FirstOrDefault();

            //plaka
            var plateMax = (await connection.QueryAsync<PlateConclusion>("SELECT TOP 1 SUBSTRING(PLATE, 1, 2) AS plate, COUNT(*) AS count FROM PLATES GROUP BY SUBSTRING(PLATE, 1, 2) ORDER BY count DESC")).FirstOrDefault();
            var plateMin = (await connection.QueryAsync<PlateConclusion>("SELECT TOP 1 SUBSTRING(PLATE, 1, 2) AS plate, COUNT(*) AS count FROM PLATES GROUP BY SUBSTRING(PLATE, 1, 2) ORDER BY count ASC")).FirstOrDefault();


            var fuelType = (await connection.QueryAsync<FuelConclusion>("SELECT TOP 1 FUEL, COUNT(*) AS count FROM PLATES GROUP BY FUEL ORDER BY count DESC")).FirstOrDefault();

            //marka
            ViewData["brandMax"] = brandMax.BRAND;
            ViewData["countMax"] = brandMax.COUNT;

            ViewData["brandMin"] = brandMin.BRAND;
            ViewData["countMin"] = brandMin.COUNT;

            //color
            ViewData["colorMax"] = colorMax.COLOR;
            ViewData["countMin"] = colorMax.COUNT;

            ViewData["colorMax"] = colorMin.COLOR;
            ViewData["countMin"] = colorMin.COUNT;

            //plaka
            ViewData["plateMax"] = plateMax.PLATE;
            ViewData["countMax"] = plateMax.COUNT;

            ViewData["plateMin"] = plateMin.PLATE;
            ViewData["countMin"] = plateMin.COUNT;

            //yakıt türü
            ViewData["fuelType"] = fuelType.FUEL;
            ViewData["fuelTypeCount"] = fuelType.COUNT;





            return View();

        }
    }
}
