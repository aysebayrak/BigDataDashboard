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

            var shiftType = (await connection.QueryAsync<ShiftConclusion>("SELECT TOP 1 SHIFTTYPE, COUNT(*) AS count FROM PLATES GROUP BY SHIFTTYPE ORDER BY count DESC")).FirstOrDefault();

            var caseType = (await connection.QueryAsync<CaseTypeConclusion>("SELECT TOP 1 CASATYPE, COUNT(*) AS count FROM PLATES GROUP BY CASETYPE ORDER BY count DESC")).FirstOrDefault();

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

            //vites
            ViewData["shiftType"] = shiftType.SHIFTTYPE;
            ViewData["shiftTypeCount"] = shiftType.COUNT;


            ViewData["caseType"] = caseType.CASETYPE;
            ViewData["caseTypeCount"] = caseType.COUNT;




            return View();

        }



        public async Task<IActionResult> Search(string value)
        {

            string query = @"
            SELECT TOP 10000 BRAND,COLOR, SUBSTRING(PLATE, 1, 2) AS PlateSet, SHIFTTYPE, FUEL
            FROM PLATES
            WHERE BRAND LIKE '%' + @Keyword + '%'
               OR COLOR LIKE '%' + @Keyword + '%'
               OR PLATE LIKE '%' + @Keyword + '%'
               OR SHIFTTYPE LIKE '%' + @Keyword + '%'
               OR FUEL LIKE '%' + @Keyword + '%'
        ";

            await using var connection = new SqlConnection(_connectionString);
            connection.Open();

            //sorguyu çalıştırmak ve sonuç alamak için 
            // "SearchConclusion" sınıfı, sorgu sonuçlarıyla eşleşen verileri temsil eden bir model sınıfı olmalıdır.
            var searchResults = await connection.QueryAsync<SearchConclusion>(query, new { Keyword = value });

            // josn şeklinde  JsonResult" tipinde sonuç döndürü
            return Json(searchResults);

        }




    }

}

