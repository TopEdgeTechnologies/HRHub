using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace HRHUBAPI.Models
{



    public partial class Candidate
    {
        
        [NotMapped]
        public string? ClassTitle { get; set; }
        [NotMapped]
        public string? GroupName { get; set; }
        [NotMapped]
        public string? SessionName { get; set; }
        public async Task<List<Candidate>> GetCandidate(HrhubContext _context)
        {
            try
            {
                var list = await _context.Candidates.Where(x=>x.IsDeleted==false).ToListAsync();
                //var list = await (from c in _context.Candidates
                //                  join cl in _context.ClassInfos on c.AppliedForClassId equals cl.ClassId
                //                  join g in _context.GroupInfos on c.GroupId equals g.GroupId
                //                  join s in _context.Sessions on c.SessionId equals s.SessionId

                //                  where c.IsDeleted == false
                //                  && cl.IsDeleted == false
                //                  && g.IsDeleted == false
                //                  && s.IsDeleted == false
                //                  select new Candidate()
                //                  {
                //                      CandidateId = c.CandidateId,
                //                      Name = $"{c.FirstName} {c.LastName}",//c.Name,
                //                      AppliedForClassId = cl.ClassId,
                //                      ClassTitle = cl.Title,
                //                      GroupId = g.GroupId,
                //                      GroupName = g.Title,
                //                      SessionId = s.SessionId,
                //                      SessionName = s.Title,
                //                      Cnic = c.Cnic,
                //                      AdmissionDate = c.AdmissionDate,
                //                      CandidateNo= c.CandidateNo,
                //                      IsActive = c.IsActive


                //                  }).ToListAsync();

                return list  ;


              
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<Candidate> GetCandidateById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == id && x.IsDeleted==false);
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


        public async Task<Candidate> PostCandidate(Candidate CandidateInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkCandidateInfo = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == CandidateInfo.CandidateId && x.IsDeleted==false);
                if (checkCandidateInfo != null && checkCandidateInfo.CandidateId > 0)
                {
                    checkCandidateInfo.CandidateId =CandidateInfo.CandidateId;                                                  
                    checkCandidateInfo.Name = CandidateInfo.Name;                 
                    checkCandidateInfo.Dob = CandidateInfo.Dob;             
                    checkCandidateInfo.Gender = CandidateInfo.Gender; 
                    checkCandidateInfo.Address = CandidateInfo.Address; 
                    checkCandidateInfo.City = CandidateInfo.City;                 
                    checkCandidateInfo.Email = CandidateInfo.Email;             
                    checkCandidateInfo.CoverLetter = CandidateInfo.CoverLetter;             
                    checkCandidateInfo.DesignationId = CandidateInfo.DesignationId;             
                    checkCandidateInfo.CurrentCompany = CandidateInfo.CurrentCompany;             
                    checkCandidateInfo.CurrentDesignation = CandidateInfo.CurrentDesignation;             
                    checkCandidateInfo.CurrentSalary = CandidateInfo.CurrentSalary;             
                    checkCandidateInfo.ExpectedSalary = CandidateInfo.ExpectedSalary;             
                    checkCandidateInfo.AttachmentPath = CandidateInfo.AttachmentPath;             
                    checkCandidateInfo.Qualification = CandidateInfo.Qualification;             
                    checkCandidateInfo.ApplyDate = CandidateInfo.ApplyDate;             
                    checkCandidateInfo.ExperienceInYears = CandidateInfo.ExperienceInYears;             
                    checkCandidateInfo.StatusId = CandidateInfo.StatusId;             
                    checkCandidateInfo.ResonToLeft = CandidateInfo.ResonToLeft;             
                    checkCandidateInfo.UpdatedOn = DateTime.Now;
                    checkCandidateInfo.Status = CandidateInfo.Status;
                    checkCandidateInfo.UpdatedBy = CandidateInfo.CreatedBy;
                   
                    await _context.SaveChangesAsync();

                }
                else
                {
                    CandidateInfo.CreatedOn = DateTime.Now;
                    _context.Candidates.Add(CandidateInfo);
                }
                await _context.SaveChangesAsync();

                return checkCandidateInfo;


            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<bool> DeleteCandidateInfo(int id, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var CandidateInfo = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == id && x.IsDeleted == false);

                if (CandidateInfo != null)
                {
                    CandidateInfo.IsDeleted= true;   
                    CandidateInfo.UpdatedOn= DateTime.Now;
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


        public async Task<bool> AlreadyExist(int CandidateInfoId, string email, HrhubContext _context)
        {
            try
            {
                if (CandidateInfoId > 0)
                {
                    var result = await _context.Candidates.FirstOrDefaultAsync(x => x.Email == email && x.CandidateId != CandidateInfoId && x.IsDeleted==false);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.Candidates.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);
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


        //Load dropdown candidate data Id vise
        //public async Task<List<Candidate>> GetCandidateIdVise(int candidateId,HrhubContext _context)
        //{
        //    try
        //    {
        //        //var list = await _context.CandidateInfos.Where(x=>x.IsDeleted==false).ToListAsync();
        //        var list = await (from c in _context.Candidates
        //                          join cl in _context.ClassInfos on c.AppliedForClassId equals cl.ClassId
        //                          join g in _context.GroupInfos on c.GroupId equals g.GroupId
        //                          join s in _context.Sessions on c.SessionId equals s.SessionId

        //                          where c.IsDeleted == false
        //                          && cl.IsDeleted == false
        //                          && g.IsDeleted == false
        //                          && s.IsDeleted == false
        //                          && c.CandidateId == candidateId
        //                          select new Candidate()
        //                          {
        //                              CandidateId = c.CandidateId,
        //                              Name = c.Name,
        //                              AppliedForClassId = cl.ClassId,
        //                              ClassTitle = cl.Title,
        //                              GroupId = g.GroupId,
        //                              GroupName = g.Title,
        //                              SessionId = s.SessionId,
        //                              SessionName = s.Title,
        //                              Cnic = c.Cnic,
        //                              AdmissionDate = c.AdmissionDate,
        //                              CandidateNo = c.CandidateNo,
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
