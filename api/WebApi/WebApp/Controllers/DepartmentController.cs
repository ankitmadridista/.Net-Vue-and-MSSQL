using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ExceptionConverter : JsonConverter<Exception>
    {
        public override Exception Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Message", value.Message);
            // Add any other propoerties that you may want to include in your JSON.
            // ...
            writer.WriteEndObject();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {        
        private readonly WebAppDbContext dbContext;
        public DepartmentController(WebAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            return Ok(await dbContext.Departments.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetDepartment([FromRoute] Guid id)
        {

            var department = await dbContext.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();                
            }

            return Ok(department);
        }


        [HttpPost]
        public async Task<IActionResult> AddDepartments(AddDepartmentRequest addDepartmentRequest)
        {
            var dept = new Department()
            {
                DepartmentId = Guid.NewGuid(),
                DepartmentName = addDepartmentRequest.DepartmentName
            };

            await dbContext.Departments.AddAsync(dept);
            await dbContext.SaveChangesAsync();

            return Ok(dept);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] Guid id, UpdateDepartmentRequest updateDepartmentRequest)
        {
            var department =  await dbContext.Departments.FindAsync(id);

            if (department != null)
            {
                department.DepartmentName = updateDepartmentRequest.DepartmentName;

                await dbContext.SaveChangesAsync();

                return Ok(department);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteDepartment([FromRoute] Guid id)
        {
            var department = await dbContext.Departments.FindAsync(id);

            if (department != null)
            {
                dbContext.Remove(department);
                await dbContext.SaveChangesAsync();
                return Ok(department);
            }

            return NotFound();
        }

        #region without entity framework
        //private readonly IConfiguration _configuration;
        //public DepartmentController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        //[HttpGet]
        //public JsonResult Get()
        //{
        //    var options = new JsonSerializerOptions();
        //    options.Converters.Add(new ExceptionConverter());

        //    string query = "getDepartmentList";

        //    DataTable table = new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
        //    SqlDataReader myReader;
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myReader = myCommand.ExecuteReader();
        //            table.Load(myReader);
        //            myReader.Close();
        //            myCon.Close();
        //        }
        //    }

        //    try
        //    {
        //        return new JsonResult(table);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(JsonSerializer.Serialize(ex, options));
        //        return new JsonResult(JsonSerializer.Serialize(ex, options));
        //    }

        //}

        //[HttpPost]
        //public JsonResult Post(Department department)
        //{
        //    string query = "addDepartment";

        //    string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myCommand.Parameters.AddWithValue("@p_DepartmentName", department.DepartmentName);
        //            myCommand.ExecuteReader();
        //            myCon.Close();
        //        }
        //    }
        //    return new JsonResult("Added Successfully");
        //}

        //[HttpPut]
        //public JsonResult Put(Department department)
        //{
        //    string query = "chgDepartment";

        //    string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myCommand.Parameters.AddWithValue("@p_DepartmentId", department.DepartmentId);
        //            myCommand.Parameters.AddWithValue("@p_DepartmentName", department.DepartmentName);
        //            myCommand.ExecuteReader();
        //            myCon.Close();
        //        }
        //    }

        //    return new JsonResult("Updated Successfully");
        //}

        //[HttpDelete("{id}")]
        //public JsonResult Delete(int id)
        //{
        //    string query = "delDepartment";

        //    string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myCommand.Parameters.AddWithValue("@p_DepartmentId", id);
        //            myCommand.ExecuteReader();
        //            myCon.Close();
        //        }
        //    }

        //    return new JsonResult("Deleted Successfully");
        //}
        #endregion
    }
}
