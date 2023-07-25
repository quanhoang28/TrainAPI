using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TrainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiaVeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public GiaVeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("getBangGia")]
        [HttpGet]
        public JsonResult GetBangGia() 
        {
            string query = "select * from BangGia";
            DataTable table=new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QLVT");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
