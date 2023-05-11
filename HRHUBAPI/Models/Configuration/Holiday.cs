using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class Holiday
    {
        [NotMapped]
        public IEnumerable<Holiday>? ListOfHolidays { get; set; }
        [NotMapped]
        public int TransFlag { get; set; }

        public async Task<List<Holiday>> GetHolidays(int CompanyID, HrhubContext _context)
        {

            try
            {
                List<Holiday> list = new List<Holiday>();

                list = await _context.Holidays.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId).ToListAsync();
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
                    objHoliday.DayName = obj.DayName;
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
                    var result = await _context.Holidays.FirstOrDefaultAsync(x => x.HolidayDate == Daydate && x.HolidayId != HolidayID && x.CompanyId == CompanyID && x.IsDeleted == false);
                    if (result != null)
                    {
                        returnResult = true;
                    }
                }
                else
                {
                    var result = await _context.Holidays.FirstOrDefaultAsync(x => x.HolidayDate == Daydate && x.CompanyId == CompanyID && x.IsDeleted == false);
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
