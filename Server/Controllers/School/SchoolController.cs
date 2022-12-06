using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SNICKERS.EF;
using SNICKERS.EF.Models;
using SNICKERS.Server.Models;
using SNICKERS.Shared;
using SNICKERS.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using SNICKERS.EF.Data;
using SNICKERS.Shared.Utils;
using SNICKERS.Shared.Errors;
using System.Text.Json;

namespace SNICKERS.Server.Controllers.School
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : Controller
    {
        protected readonly SNICKERSOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly OraTransMsgs _OraTranslateMsgs;

        public SchoolController(SNICKERSOracleContext context,
            IHttpContextAccessor httpContextAccessor,
             OraTransMsgs OraTranslateMsgs)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;


        }
        [HttpGet]
        [Route("GetSchools")]
        public async Task<IActionResult> GetSchools()
        {
            try
            {
                List<SchoolDTO> lstSchools = await _context.Schools.OrderBy(x => x.SchoolName)
                   .Select(sp => new SchoolDTO
                   {
                       SchoolId = sp.SchoolId,
                       CreatedBy = sp.CreatedBy,
                       CreatedDate = sp.CreatedDate,
                       ModifiedBy = sp.ModifiedBy,
                       ModifiedDate = sp.ModifiedDate,
                       SchoolName = sp.SchoolName

                   }).ToListAsync();

                return Ok(lstSchools);
            }
            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }
        }

        [HttpGet]
        [Route("GetSchools/{pSchoolID}")]
        public async Task<IActionResult> GetSchools(int pSchoolID)
        {
            try
            {

                SchoolDTO itmSchool = await _context.Schools
                    .Where(x => x.SchoolId == pSchoolID)
                    .OrderBy(x => x.SchoolId)
                   .Select(sp => new SchoolDTO
                   {
                       SchoolId = sp.SchoolId,
                       SchoolName = sp.SchoolName,
                       CreatedBy = sp.CreatedBy,
                       CreatedDate = sp.CreatedDate,
                       ModifiedBy = sp.ModifiedBy,
                       ModifiedDate = sp.ModifiedDate
                   }).FirstOrDefaultAsync();

                return Ok(itmSchool);
            }
            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }
        }

        //[HttpPost]
        //[Route("PostSchool")]
        //public async Task<IActionResult> PostSchool([FromBody] string _SchoolDTO_String)
        //{

        //    try
        //    {
        //        SchoolDTO _SchoolDTO = JsonSerializer.Deserialize<SchoolDTO>(_SchoolDTO_String);
        //        await this.PostSchool(_SchoolDTO);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    return Ok();
        //}

        //[HttpPost]
        //public async Task<IActionResult> PostSchool([FromBody] SchoolDTO _SchoolDTO)
        //{
        //    try
        //    {
        //        var trans = await _context.Database.BeginTransactionAsync();
        //        School s = new School
        //        {
        //            SchoolId = _SchoolDTO.SchoolId,
        //            SchoolName = _SchoolDTO.SchoolName,
        //            CreatedBy = _SchoolDTO.CreatedBy,
        //            CreatedDate = _SchoolDTO.CreatedDate,
        //            ModifiedBy = _SchoolDTO.ModifiedBy,
        //            ModifiedDate = _SchoolDTO.ModifiedDate
        //        };
        //        _context.Schools.Add(s);
        //        await _context.SaveChangesAsync();
        //        await _context.Database.CommitTransactionAsync();
        //        return Ok();

        //    }
        //    catch (DbUpdateException Dex)
        //    {
        //        List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
        //    }
        //    catch (Exception ex)
        //    {
        //        _context.Database.RollbackTransaction();
        //        List<OraError> errors = new List<OraError>();
        //        errors.Add(new OraError(1, ex.Message.ToString()));
        //        string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
        //    }
        //}

        //[HttpPut]
        //public async Task<IActionResult> PutSchool(SchoolDTO _SchoolDTO)
        //{

        //    try
        //    {
        //        var trans = await _context.Database.BeginTransactionAsync();
        //        School s = await _context.Schools.Where(x => x.SchoolId.Equals(_SchoolDTO.SchoolId)).FirstOrDefaultAsync();

        //        if (s != null)
        //        {
        //            s.SchoolId = _SchoolDTO.SchoolId;
        //            s.SchoolName = _SchoolDTO.SchoolName;
        //            s.CreatedBy = _SchoolDTO.CreatedBy;
        //            s.CreatedDate = _SchoolDTO.CreatedDate;
        //            s.ModifiedBy = _SchoolDTO.ModifiedBy;
        //            s.ModifiedDate = _SchoolDTO.ModifiedDate;

        //            _context.Schools.Update(c);
        //            await _context.SaveChangesAsync();
        //            await _context.Database.CommitTransactionAsync();
        //        }
        //    }


        //    catch (DbUpdateException Dex)
        //    {
        //        List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
        //    }
        //    catch (Exception ex)
        //    {
        //        _context.Database.RollbackTransaction();
        //        List<OraError> errors = new List<OraError>();
        //        errors.Add(new OraError(1, ex.Message.ToString()));
        //        string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
        //    }



        //    return Ok();
        //}

        //[HttpDelete]
        //[Route("DeleteSchool/{pSchoolID}")]
        //public async Task<IActionResult> DeleteCourse(int pSchoolID)
        //{

        //    try
        //    {


        //        var trans = await _context.Database.BeginTransactionAsync();
        //        School s = await _context.Schools.Where(x => x.SchoolId.Equals(pSchoolID)).FirstOrDefaultAsync();
        //        _context.Schools.Remove(s);

        //        await _context.SaveChangesAsync();
        //        await _context.Database.CommitTransactionAsync();
        //    }
        //    catch (DbUpdateException Dex)
        //    {
        //        List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
        //    }
        //    catch (Exception ex)
        //    {
        //        _context.Database.RollbackTransaction();
        //        List<OraError> errors = new List<OraError>();
        //        errors.Add(new OraError(1, ex.Message.ToString()));
        //        string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
        //    }
        //    return Ok();
        //}

        [HttpPost]
        [Route("GetSchools")]
        public async Task<DataEnvelope<SchoolDTO>> GetSchoolsPost([FromBody] DataSourceRequest gridRequest)
        {
            DataEnvelope<SchoolDTO> dataToReturn = null;
            IQueryable<SchoolDTO> queriableStates = _context.Schools
                    .Select(sp => new SchoolDTO
                    {
                        SchoolId = sp.SchoolId,
                        SchoolName = sp.SchoolName,
                        CreatedBy = sp.CreatedBy,
                        CreatedDate = sp.CreatedDate,
                        ModifiedBy = sp.ModifiedBy,
                        ModifiedDate = sp.ModifiedDate
                    });

            // use the Telerik DataSource Extensions to perform the query on the data
            // the Telerik extension methods can also work on "regular" collections like List<T> and IQueriable<T>
            try
            {

                DataSourceResult processedData = await queriableStates.ToDataSourceResultAsync(gridRequest);

                if (gridRequest.Groups.Count > 0)
                {
                    // If there is grouping, use the field for grouped data
                    // The app must be able to serialize and deserialize it
                    // Example helper methods for this are available in this project
                    // See the GroupDataHelper.DeserializeGroups and JsonExtensions.Deserialize methods
                    dataToReturn = new DataEnvelope<SchoolDTO>
                    {
                        GroupedData = processedData.Data.Cast<AggregateFunctionsGroup>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
                else
                {
                    // When there is no grouping, the simplistic approach of 
                    // just serializing and deserializing the flat data is enough
                    dataToReturn = new DataEnvelope<SchoolDTO>
                    {
                        CurrentPageData = processedData.Data.Cast<SchoolDTO>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
            }
            catch (Exception e)
            {
                //fixme add decent exception handling
            }
            return dataToReturn;
        }

    }
}