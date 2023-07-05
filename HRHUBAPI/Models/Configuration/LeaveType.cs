using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using HRHUBAPI.Models.Configuration;
using System.Data;

namespace HRHUBAPI.Models
{



    public partial class LeaveType
    {
        [NotMapped]
        public IEnumerable<LeaveType>? ListLeaveTypes { get; set; }
        [NotMapped]
        public decimal? ConsumedLeave { get; set; }
        [NotMapped]
        public decimal? RemainingLeaves { get; set; }
        
        public async Task<List<LeaveType>> GetLeaveType(int CompanyId, HrhubContext _context)
        {
            try
            {
                List<LeaveType> list = new List<LeaveType>();
                list = await _context.LeaveTypes.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId).ToListAsync();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }
        // using this in leave apply form
        public async Task<List<LeaveType>> GetStaffWiseLeaveType(int StaffId, HrhubContext _context)
        {

            DbConnection _db = new DbConnection();
            try
            {
                string query = "EXEC HR.sp_Get_StaffWiseLeaves "  + StaffId;
                DataTable dt = _db.ReturnDataTable(query);

                var leavetypes = dt.AsEnumerable()
                    .Select(row => new LeaveType
                    {
                        LeaveTypeId = Convert.ToInt32(row["LeaveTypeId"]),
                        Title = row["Title"].ToString(),
                        NoOfLeaves = Convert.ToInt32(row["AllowedLeaves"]),
                        ConsumedLeave = Convert.ToDecimal(row["ConsumedLeave"]),
                        RemainingLeaves = Convert.ToDecimal(row["RemainingLeaves"]),
                        Cssclass = row["CSSClass"].ToString()
                    })
                    .ToList();
                return leavetypes;
            }
            catch (Exception ex) 
            { throw; }



            //try
            //{
            //    List<LeaveType> list = new List<LeaveType>();


            //    list = await (from l in _context.StaffLeaveAllocations
            //                  join lt in _context.LeaveTypes on l.LeaveTypeId equals lt.LeaveTypeId

            //                  where l.IsDeleted == false
            //                  && lt.IsDeleted == false
            //                  && l.StaffId == StaffId
            //                  select new LeaveType()
            //                  {
            //                      LeaveTypeId = lt.LeaveTypeId,
            //                      Title = lt.Title,
            //                      NoOfLeaves = l.AllowedLeaves

            //                  }).ToListAsync();

            //    return list;

            //}
            //catch (Exception ex)
            //{

            //    throw;

            //}
        }

        public async Task<LeaveType> GetLeaveTypeById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.LeaveTypeId == id && x.IsDeleted == false);
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
        public async Task<LeaveType> UpdateStatusByLeaveTypeId(LeaveType ObjLeaveTypeInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkLeaveTypeInfo = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.LeaveTypeId == ObjLeaveTypeInfo.LeaveTypeId && x.IsDeleted == false);
                if (checkLeaveTypeInfo != null && checkLeaveTypeInfo.LeaveTypeId > 0)
                {
                    checkLeaveTypeInfo.LeaveTypeId = ObjLeaveTypeInfo.LeaveTypeId;
                    checkLeaveTypeInfo.UpdatedOn = DateTime.Now;
                    checkLeaveTypeInfo.NoOfLeaves = ObjLeaveTypeInfo.NoOfLeaves;
                    checkLeaveTypeInfo.Status = ObjLeaveTypeInfo.Status;
                    checkLeaveTypeInfo.IsNonPaid = ObjLeaveTypeInfo.IsNonPaid;
                    checkLeaveTypeInfo.UpdatedBy = ObjLeaveTypeInfo.UpdatedBy;

                    await _context.SaveChangesAsync();
                    return ObjLeaveTypeInfo;

                }
                return ObjLeaveTypeInfo;
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<LeaveType> PostLeaveType(LeaveType ObjLeaveTypeInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkLeaveTypeInfo = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.LeaveTypeId == ObjLeaveTypeInfo.LeaveTypeId && x.IsDeleted == false);
                if (checkLeaveTypeInfo != null && checkLeaveTypeInfo.LeaveTypeId > 0)
                {
                    checkLeaveTypeInfo.LeaveTypeId = ObjLeaveTypeInfo.LeaveTypeId;
                    checkLeaveTypeInfo.Title = ObjLeaveTypeInfo.Title;
                    checkLeaveTypeInfo.NoOfLeaves = ObjLeaveTypeInfo.NoOfLeaves;
                    checkLeaveTypeInfo.IsNonPaid = ObjLeaveTypeInfo.IsNonPaid;
                    checkLeaveTypeInfo.Status = ObjLeaveTypeInfo.Status;
                    checkLeaveTypeInfo.UpdatedOn = DateTime.Now;
                    checkLeaveTypeInfo.UpdatedBy = ObjLeaveTypeInfo.CreatedBy;
                    checkLeaveTypeInfo.CompanyId = ObjLeaveTypeInfo.CompanyId;
                    await _context.SaveChangesAsync();

                }
                else
                {
                    ObjLeaveTypeInfo.CreatedOn = DateTime.Now;
                    ObjLeaveTypeInfo.IsDeleted = false;
                    _context.LeaveTypes.Add(ObjLeaveTypeInfo);
                }
                await _context.SaveChangesAsync();

                return checkLeaveTypeInfo;


            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<bool> DeleteLeaveTypeInfo(int id, int UserId, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var LeaveTypeInfo = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.LeaveTypeId == id && x.IsDeleted == false);

                if (LeaveTypeInfo != null)
                {
                    LeaveTypeInfo.IsDeleted = true;
                    LeaveTypeInfo.UpdatedBy = UserId;
                    LeaveTypeInfo.UpdatedOn = DateTime.Now;
                    check = true;

                }
                await _context.SaveChangesAsync();
                return check;
            }
            catch (Exception)
            {

                throw;
            }


        }


        public async Task<bool> AlreadyExist(int LeaveTypeInfoId, string title, int CompanyId, HrhubContext _context)
        {
            try
            {
                if (LeaveTypeInfoId > 0)
                {
                    var result = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Title == title && x.CompanyId == CompanyId && x.LeaveTypeId != LeaveTypeInfoId && x.IsDeleted == false);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Title == title && x.CompanyId == CompanyId && x.IsDeleted == false);
                    if (result != null)
                    {
                        return true;
                    }

                }

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
