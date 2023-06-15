using HRHUBAPI.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace HRHUBAPI.Models
{
    public partial class Holiday
    {
        [NotMapped]
        public IEnumerable<Holiday>? ListOfHolidays { get; set; }
        [NotMapped]
        public int TransFlag { get; set; }

       



       

        // Get Attendence date vise
        public async Task<List<Holiday>> LoadCalanderEvent(int CompanyId, int month, int year)
        {
            DbConnection _db = new DbConnection();


            try
            {
                string query = "EXEC dbo.sp_GetHoliday_Events " + CompanyId + ", " + month + " , "+ year +" ";
                DataTable dt = _db.ReturnDataTable(query);

                var List = dt.AsEnumerable()

                    .Select(row => new Holiday
                    {
                        HolidayDate = Convert.ToDateTime(row["HolidayDate"]),
                        Title = row["Title"].ToString(),
                        
                    })
                    .ToList();
                return List;
            }
            catch { throw; }





        }




        public async Task<List<Holiday>> GetHolidays(int CompanyID, HrhubContext _context)
        {

            try
            {
                List<Holiday> list = new List<Holiday>();

                list = await _context.Holidays.Where(x => x.IsDeleted == false && x.CompanyId == CompanyID).ToListAsync();
                return list;
            }
            catch (Exception Ex)
            {

                throw;
            }

        }





        //filter holiday
        public async Task<List<Holiday>> GetFilterHolidayList(int CompanyId, string selectdate, int Yeardate, HrhubContext _context)
        {

            try
            {
                List<Holiday> list = new List<Holiday>();

                if (Yeardate == 0)
                {
                    list = await _context.Holidays.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.HolidayDate == Convert.ToDateTime(selectdate)).ToListAsync();


                }
                else
                {
                    list = await _context.Holidays.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.HolidayDate.Value.Year == Yeardate).ToListAsync();

                }

                return list;
            }
            catch (Exception Ex)
            {

                throw;
            }

        }

        public async Task<Holiday> GetHOlidayByID(int id, HrhubContext _context)
        {
            try
            {
                var result = await _context.Holidays.FirstOrDefaultAsync(x => x.HolidayId == id && x.IsDeleted == false);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Holiday> UpdateStatusByHolidayId(Holiday ObjHolidayInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkHolidayInfo = await _context.Holidays.FirstOrDefaultAsync(x => x.HolidayId == ObjHolidayInfo.HolidayId && x.IsDeleted == false);
                if (checkHolidayInfo != null && checkHolidayInfo.HolidayId > 0)
                {
                    checkHolidayInfo.HolidayId = ObjHolidayInfo.HolidayId;
                    checkHolidayInfo.Status = ObjHolidayInfo.Status;
                    checkHolidayInfo.UpdatedBy = ObjHolidayInfo.UpdatedBy;
                    checkHolidayInfo.UpdatedOn = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return ObjHolidayInfo;

                }
                return ObjHolidayInfo;
            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<Holiday> PostHoliday(Holiday obj, HrhubContext _context)
        {
            try
            {
                string msg = "";
                // company id is missing do this


                var objHoliday = await _context.Holidays.FirstOrDefaultAsync(x => x.HolidayId == obj.HolidayId && x.IsDeleted == false); // where clause with company id is missing
                if (objHoliday != null && objHoliday.HolidayId > 0)
                {
                    objHoliday.HolidayId = obj.HolidayId;

                    DateTime dtH = Convert.ToDateTime(obj.HolidayDate);
                    objHoliday.DayName = dtH.Date.DayOfWeek.ToString();  // Getting DayName from Dayof We

                    objHoliday.HolidayDate = obj.HolidayDate;
                    objHoliday.Title = obj.Title;
                    objHoliday.Status = obj.Status;
                    objHoliday.UpdatedOn = DateTime.Now;
                    objHoliday.UpdatedBy = obj.CreatedBy;
                    objHoliday.TransFlag = 2; //  Record Updated 
                    await _context.SaveChangesAsync();
                    return objHoliday;
                }
                else
                {

                    obj.CreatedOn = DateTime.Now;
                    DateTime dtH = Convert.ToDateTime(obj.HolidayDate);
                    obj.DayName = dtH.Date.DayOfWeek.ToString();
                    obj.IsDeleted = false;
                    obj.TransFlag = 1; // New Record Inserted

                    _context.Holidays.Add(obj);

                    await _context.SaveChangesAsync();
                    return obj;
                }



            }
            catch (Exception Ex)
            {

                throw;
            }
        }


        public async Task<bool> DeleteHoliday(int id,int UserID, HrhubContext _context)
        {
            try
            {
                bool DeleteSuccessfull = false;
                var objHoliday = await _context.Holidays.FirstOrDefaultAsync(x => x.HolidayId == id && x.IsDeleted == false);

                if (objHoliday != null)
                {
                    objHoliday.IsDeleted = true;
                    objHoliday.UpdatedOn = DateTime.Now;
                    objHoliday.UpdatedBy = UserID;
                    DeleteSuccessfull = true;
                    

                }
                await _context.SaveChangesAsync();
                return DeleteSuccessfull;

            }
            catch (Exception)
            {

                throw;
            }
        }




        public async Task<bool> AlredyExist(int HolidayID, DateTime Daydate, int CompanyID, HrhubContext _context)
        {
            try
            {
                bool returnResult = false;
                if (HolidayID > 0)
                {
                    var result = await _context.Holidays.FirstOrDefaultAsync(x => x.HolidayDate.Value.Date  == Daydate.Date && x.HolidayId != HolidayID && x.CompanyId == CompanyID && x.IsDeleted == false);
                    if (result != null)
                    {
                        returnResult = true;
                    }
                }
                else
                {
                    var result = await _context.Holidays.FirstOrDefaultAsync(x => x.HolidayDate.Value.Date == Daydate.Date && x.CompanyId == CompanyID && x.IsDeleted == false);
                    if (result != null)
                    {
                        returnResult = true;
                    }
                }
                return returnResult;
            }
            catch (Exception)
            {

                throw;
            }
        }





    }
}
