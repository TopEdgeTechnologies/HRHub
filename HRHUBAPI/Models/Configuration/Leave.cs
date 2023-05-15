using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace HRHUBAPI.Models
{



    public partial class Leave
    {
        
        [NotMapped]
        public string? LeaveTypeTitle { get; set; }
        [NotMapped]
        public string? Days { get; set; }
        [NotMapped]
        public string? LeaveStartDate { get; set; }
        [NotMapped]
        public string? LeaveEndDate { get; set; }
        [NotMapped]
        public string? LeaveAppliedOnDate { get; set; }
        [NotMapped]
        public string? LeaveStatus { get; set; }
        public async Task<List<Leave>> GetLeave(HrhubContext _context)
        {
            try
            {
               // var list = await _context.Leaves.Where(x=>x.IsDeleted==false).ToListAsync();
                var list = await (from l in _context.Leaves
                                  join lt in _context.LeaveTypes on l.LeaveTypeId equals lt.LeaveTypeId

                                  where l.IsDeleted == false
                                  && lt.IsDeleted == false
                                  select new Leave()
                                  {
                                      LeaveId = l.LeaveId,
                                      LeaveTypeTitle = lt.Title,
                                      LeaveStartDate = Convert.ToDateTime(l.StartDate).ToString("dd-MMM-yyyy"),
                                      LeaveEndDate = Convert.ToDateTime(l.EndDate).ToString("dd-MMM-yyyy") ,
                                      Days = l.MarkAsHalfLeave == true? "Half Day" : l.MarkAsShortLeave == true ? "Short Day" : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString(),
                                      LeaveSubject = l.LeaveSubject,
                                      LeaveAppliedOnDate = Convert.ToDateTime(l.AppliedOn).ToString("dd-MMM-yyyy"),
                                      LeaveStatusId = l.LeaveStatusId


                                  }).ToListAsync();

                return list  ;


              
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<Leave> GetLeaveById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == id && x.IsDeleted==false);
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


        public async Task<Leave> PostLeave(Leave LeaveInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkLeaveInfo = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == LeaveInfo.LeaveId && x.IsDeleted==false);
                if (checkLeaveInfo != null && checkLeaveInfo.LeaveId > 0)
                {
                    checkLeaveInfo.LeaveId = LeaveInfo.LeaveId;
                    checkLeaveInfo.LeaveTypeId = LeaveInfo.LeaveTypeId;
                    checkLeaveInfo.StartDate = LeaveInfo.StartDate;
                    checkLeaveInfo.EndDate = LeaveInfo.EndDate;
                    checkLeaveInfo.LeaveSubject = LeaveInfo.LeaveSubject;
                    checkLeaveInfo.ApplicationHtml = LeaveInfo.ApplicationHtml;
                    checkLeaveInfo.UpdatedOn = DateTime.Now;
                    checkLeaveInfo.UpdatedBy = LeaveInfo.CreatedBy;

                    await _context.SaveChangesAsync();

                }
                else
                {
                    LeaveInfo.CreatedOn = DateTime.Now;
                    _context.Leaves.Add(LeaveInfo);
                }
                await _context.SaveChangesAsync();

                return checkLeaveInfo;


            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<bool> DeleteLeaveInfo(int id, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var LeaveInfo = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == id && x.IsDeleted == false);

                if (LeaveInfo != null)
                {
                    LeaveInfo.IsDeleted= true;   
                    LeaveInfo.UpdatedOn= DateTime.Now;
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


        public async Task<bool> AlreadyExist(int LeaveInfoId, string email, HrhubContext _context)
        {
            try
            {
                //if (LeaveInfoId > 0)
                //{
                //    var result = await _context.Leaves.FirstOrDefaultAsync(x => x.Email == email && x.LeaveId != LeaveInfoId && x.IsDeleted==false);
                //    if (result != null)
                //    {
                //        return true;
                //    }


                //}
                //else
                //{
                //    var result = await _context.Leaves.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);
                //    if (result != null)
                //    {
                //        return true;
                //    }

                //}

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }


        //Load dropdown Leave data Id vise
        //public async Task<List<Leave>> GetLeaveIdVise(int LeaveId,HrhubContext _context)
        //{
        //    try
        //    {
        //        //var list = await _context.LeaveInfos.Where(x=>x.IsDeleted==false).ToListAsync();
        //        var list = await (from c in _context.Leaves
        //                          join cl in _context.ClassInfos on c.AppliedForClassId equals cl.ClassId
        //                          join g in _context.GroupInfos on c.GroupId equals g.GroupId
        //                          join s in _context.Sessions on c.SessionId equals s.SessionId

        //                          where c.IsDeleted == false
        //                          && cl.IsDeleted == false
        //                          && g.IsDeleted == false
        //                          && s.IsDeleted == false
        //                          && c.LeaveId == LeaveId
        //                          select new Leave()
        //                          {
        //                              LeaveId = c.LeaveId,
        //                              Name = c.Name,
        //                              AppliedForClassId = cl.ClassId,
        //                              ClassTitle = cl.Title,
        //                              GroupId = g.GroupId,
        //                              GroupName = g.Title,
        //                              SessionId = s.SessionId,
        //                              SessionName = s.Title,
        //                              Cnic = c.Cnic,
        //                              AdmissionDate = c.AdmissionDate,
        //                              LeaveNo = c.LeaveNo,
        //                              Dob = c.Dob,
        //                              FatherName = c.FatherName,
        //                              Gender = c.Gender,
        //                              Address= c.Address,
        //                              City= c.City,
        //                              Mobile = c.Mobile,
        //                              Email= c.Email,
        //                              PreviousSchool= c.PreviousSchool,
        //                              FatherQualification = c.FatherQualification,
        //                              MotherQualification = c.MotherQualification,
        //                              MotherName= c.MotherName,
        //                              ParentStaffId= c.ParentStaffId,
        //                              FirstName = c.FirstName,
        //                              LastName = c.LastName,
        //                              IsActive = c.IsActive


        //                          }).ToListAsync();

        //        return list;



        //    }
        //    catch (Exception ex)
        //    {

        //        throw;

        //    }
        //}




    }
}
