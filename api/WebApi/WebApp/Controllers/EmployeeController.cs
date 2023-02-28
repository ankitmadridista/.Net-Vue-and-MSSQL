using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;
using WebApp.Models;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly WebAppDbContext dbContext;
        public EmployeeController(WebAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            return Ok(await dbContext.Employees.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {

            var employee = await dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }


        [HttpPost]
        public async Task<IActionResult> AddEmployees(AddEmployeeRequest addEmployeeRequest)
        {
            var employee = new Employee()
            {
                EmployeeId = Guid.NewGuid(),
                EmployeeName = addEmployeeRequest.EmployeeName,
                DateOfJoining= addEmployeeRequest.DateOfJoining,
                Department=addEmployeeRequest.Department,
                PhotoFileName=addEmployeeRequest.PhotoFileName
            };

            await dbContext.Employees.AddAsync(employee);
            await dbContext.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, UpdateEmployeeRequest updateEmployeeRequest)
        {
            var employee = await dbContext.Employees.FindAsync(id);

            if (employee != null)
            {   
                employee.EmployeeName = updateEmployeeRequest.EmployeeName;
                employee.DateOfJoining = updateEmployeeRequest.DateOfJoining;
                employee.Department = updateEmployeeRequest.Department;
                employee.PhotoFileName= updateEmployeeRequest.PhotoFileName;

                await dbContext.SaveChangesAsync();

                return Ok(employee);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await dbContext.Employees.FindAsync(id);

            if (employee != null)
            {
                dbContext.Remove(employee);
                await dbContext.SaveChangesAsync();
                return Ok(employee);
            }

            return NotFound();
        }

        #region without entity framework

        //private readonly IConfiguration _configuration;
        //private readonly IWebHostEnvironment _environment;
        //public EmployeeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        //{
        //    _configuration = configuration;
        //    _environment = webHostEnvironment;
        //}
        //[HttpGet]
        //public JsonResult Get()
        //{
        //    var options = new JsonSerializerOptions();
        //    options.Converters.Add(new ExceptionConverter());

        //    string query = @"
        //                    select EmployeeId, EmployeeName, Department, convert(varchar(10), DateOfJoining, 120) as DateOfJoining, PhotoFileName 
        //                    FROM dbo.Employee
        //                    ";

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
        //public JsonResult Post(Employee employee)
        //{
        //    string query = @"
        //                    INSERT INTO dbo.Employee(EmployeeName, Department, DateOfJoining, PhotoFileName)
        //                    VALUES (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)
        //                    ";

        //    string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
        //            myCommand.Parameters.AddWithValue("@Department", employee.Department);
        //            myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
        //            myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
        //            myCommand.ExecuteReader();
        //            myCon.Close();
        //        }
        //    }
        //    return new JsonResult("Added Successfully");
        //}

        //[HttpPut]
        //public JsonResult Put(Employee employee)
        //{
        //    string query = @"
        //                    UPDATE dbo.Employee
        //                    SET EmployeeName = @EmployeeName,
        //                    Department = @Department,
        //                    DateOfJoining = @DateOfJoining,
        //                    PhotoFileName = @PhotoFileName
        //                    WHERE EmployeeId = @EmployeeId
        //                    ";

        //    string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
        //            myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
        //            myCommand.Parameters.AddWithValue("@Department", employee.Department);
        //            myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
        //            myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
        //            myCommand.ExecuteReader();
        //            myCon.Close();
        //        }
        //    }

        //    return new JsonResult("Updated Successfully");
        //}

        //[HttpDelete("{id}")]
        //public JsonResult Delete(int id)
        //{
        //    string query = @"
        //                    DELETE dbo.Employee
        //                    WHERE EmployeeId = @EmployeeId
        //                    ";

        //    string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myCommand.Parameters.AddWithValue("@EmployeeId", id);
        //            myCommand.ExecuteReader();
        //            myCon.Close();
        //        }
        //    }

        //    return new JsonResult("Deleted Successfully");
        //}

        //[Route("SaveFile")]
        //[HttpPost]
        //public JsonResult SaveFile()
        //{
        //    try
        //    {
        //        var httpRequest = Request.Form;
        //        var postedFIle = httpRequest.Files[0];
        //        string filename = postedFIle.FileName;
        //        var physicalPath = _environment.ContentRootPath+ "/Photos/" + filename;

        //        using (var stream = new FileStream(physicalPath, FileMode.Create)) 
        //        {
        //            postedFIle.CopyTo(stream);
        //        }

        //        return new JsonResult(filename);
        //    }
        //    catch (Exception)
        //    {

        //        return new JsonResult("anonymous.png");
        //    }
        //}
        #endregion
    }
}
