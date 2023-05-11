using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace HRHUBAPI.Models
{



    public partial class LeaveType
    {

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


        public async Task<bool> DeleteLeaveTypeInfo(int id, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var LeaveTypeInfo = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.LeaveTypeId == id && x.IsDeleted == false);

                if (LeaveTypeInfo != null)
                {
                    LeaveTypeInfo.IsDeleted = true;
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
