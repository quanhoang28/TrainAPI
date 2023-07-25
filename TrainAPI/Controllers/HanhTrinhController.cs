using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace TrainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HanhTrinhController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public HanhTrinhController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("GetTenGa")]
        [HttpGet]
        public JsonResult GetTenGa()
        {
            String query = "Select tengatau,idGaTau from GaTau";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QLVT");
            SqlDataReader myReader;
            using(SqlConnection myCon=new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query,myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        [Route("GetNgayDi")]
        [HttpGet]
        public JsonResult GetNgayDi(string gaDi,string gaDen)
        {
            String query = "SELECT distinct(FORMAT(NgayDi,'dd-MM-yyyy')) FROM CT_HanhTrinh ht join LichTrinh tl on ht.NgayDi=NgayBD where IDGaTau='" + gaDi+"' and exists (select * from CT_HanhTrinh where IDGaTau='"+gaDen+"')";
            DataTable table = new DataTable();
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
        [Route("GetHanhTrinh")]
        [HttpGet]
        public JsonResult GetHanhTrinh(string ngayDi,string gaTau)
        {
            String query = "select ht.idHanhTrinh,TenGaTau,TongQuangDuong,format(NgayDi,'dd-MM-yyy'),ThoiGianBD,ThoiGianKT " +
                "from CT_HanhTrinh ht join LichTrinh lt on ht.IDLichTrinh= lt.IDLichTrinh join GaTau gt on ht.IDGaTau=gt.IDGaTau " +
                " where format(NgayDi,'dd-MM-yyy')='" + ngayDi + "'and exists(select * from CT_HanhTrinh where IDGaTau='"+gaTau+"' and format(NgayDi,'dd-MM-yyy')='"+ngayDi+"')" ;
            DataTable table = new DataTable();
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
