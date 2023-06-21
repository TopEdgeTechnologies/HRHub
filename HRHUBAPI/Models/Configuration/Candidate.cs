using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace HRHUBAPI.Models
{



    public partial class Candidate
    {

        [NotMapped]
        public int? UrlRequestSatausID { get; set; }

        [NotMapped]
        public string? DesignationTitle { get; set; }

        [NotMapped]
        public IEnumerable<string>? ListSkillTitle { get; set; }
        [NotMapped]
        public IEnumerable<string>? ListSkillStatus { get; set; }
        [NotMapped]
        public IEnumerable<Candidate>? ListCandidate { get; set; }
        [NotMapped]
        public int? Flag { get; set; }


        // Search Candidate by Name or Designation and Experince

        public async Task<List<Candidate>> SearchCandidates(string Name, int DesignationId,int ExperienceId,int CompanyId, HrhubContext _context)
        {
            try
            {
                //var list = await _context.Candidates.Where(x => x.Name == Name && x.DesignationId == DesignationId && x.ExperienceInYears== ExperienceId && x.CompanyId==CompanyId && x.IsDeleted == false).ToListAsync();


                var query = from cs in _context.Candidates
                            join d in _context.Designations on cs.DesignationId equals d.DesignationId
                            
                            where cs.Name == Name && cs.DesignationId==DesignationId && cs.ExperienceInYears==ExperienceId && cs.CompanyId==CompanyId && cs.IsDeleted == false
                            select new Candidate
                            {
                                CandidateId = cs.CandidateId,
                                Name = cs.Name,
                                DesignationId = cs.DesignationId,
                                DesignationTitle = d.Title,
                                CoverLetter = cs.CoverLetter,
                                Email = cs.Email,
                                Phone= cs.Phone,
                                CurrentCompany = cs.CurrentCompany,
                                CurrentDesignation= cs.CurrentDesignation,
                                CurrentSalary= cs.CurrentSalary,
                                ExpectedSalary = cs.ExpectedSalary,
                                ExperienceInMonths= cs.ExperienceInMonths,
                                ExperienceInYears= cs.ExperienceInYears,
                                Dob= cs.Dob,
                                Gender= cs.Gender,
                                City= cs.City,
                                Address= cs.Address,
                                Qualification = cs.Qualification,
                                ApplyDate = cs.ApplyDate,
                                Picture = string.IsNullOrWhiteSpace(cs.Picture) ? "" : cs.Picture,
                                CompanyId= cs.CompanyId,
                                StatusId = cs.StatusId,
                                CreatedOn = cs.CreatedOn,
                                CreatedBy = cs.CreatedBy,                        
                                AttachmentPath = string.IsNullOrWhiteSpace(cs.AttachmentPath) ? "" : cs.AttachmentPath




                            };

                return query != null ? query.OrderByDescending(x => x.CandidateId).ToList() : new List<Candidate>();








               


            }
            catch (Exception ex)
            {

                throw;

            }
        }




        public async Task<List<Candidate>> GetCandidate(int CompanyId,HrhubContext _context)
        {
            try
            {
                //var list = await _context.Candidates.Where(x=>x.IsDeleted==false && x.CompanyId== CompanyId).ToListAsync();
                var query = from cs in _context.Candidates
                            join d in _context.Designations on cs.DesignationId equals d.DesignationId

                            where  cs.CompanyId == CompanyId && cs.IsDeleted == false
                            select new Candidate
                            {
                                CandidateId = cs.CandidateId,
                                Name = cs.Name,
                                DesignationId = cs.DesignationId,
                                DesignationTitle = d.Title,
                                CoverLetter = cs.CoverLetter,
                                Email = cs.Email,
                                Phone = cs.Phone,
                                CurrentCompany = cs.CurrentCompany,
                                CurrentDesignation = cs.CurrentDesignation,
                                CurrentSalary = cs.CurrentSalary,
                                ExpectedSalary = cs.ExpectedSalary,
                                ExperienceInMonths = cs.ExperienceInMonths,
                                ExperienceInYears = cs.ExperienceInYears,
                                Dob = cs.Dob,
                                Gender = cs.Gender,
                                City = cs.City,
                                Address = cs.Address,
                                Qualification = cs.Qualification,
                                ApplyDate = cs.ApplyDate,                               
                                Picture = string.IsNullOrWhiteSpace(cs.Picture) ? "/Images/StaffImageEmpty.jpg" : cs.Picture,
                                CompanyId = cs.CompanyId,
                                StatusId = cs.StatusId,
                                CreatedOn = cs.CreatedOn,
                                CreatedBy = cs.CreatedBy,
                                AttachmentPath = string.IsNullOrWhiteSpace(cs.AttachmentPath) ? "" : cs.AttachmentPath




                            };

                return query != null ? query.OrderByDescending(x => x.CandidateId).ToList() : new List<Candidate>();






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


            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {

                try
                {
                    string msg = "";
                    var checkCandidateInfo = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == CandidateInfo.CandidateId && x.IsDeleted == false);
                    if (checkCandidateInfo != null && checkCandidateInfo.CandidateId > 0)
                    {
                        checkCandidateInfo.CandidateId = CandidateInfo.CandidateId;
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
                        checkCandidateInfo.ReasonToLeft = CandidateInfo.ReasonToLeft;
                        checkCandidateInfo.Picture = CandidateInfo.Picture;
                        checkCandidateInfo.UpdatedOn = DateTime.Now;
                        checkCandidateInfo.Status = CandidateInfo.Status;
                        checkCandidateInfo.UpdatedBy = CandidateInfo.CreatedBy;
                        CandidateInfo.Flag = 2;
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        CandidateInfo.CreatedOn = DateTime.Now;
                        CandidateInfo.StatusId = 1;
                        CandidateInfo.Flag = 1;
                        if (CandidateInfo.ExperienceInMonths == null)
                        {
                            CandidateInfo.ExperienceInMonths = 0;
                        }
                        CandidateInfo.IsDeleted = false;
                        _context.Candidates.Add(CandidateInfo);
                        await _context.SaveChangesAsync();


                        // ---------------------------------------------Save and Insert Candidate Status records




                        CandidateScreening objscreen = new CandidateScreening();
                        objscreen.StatusId = CandidateInfo.StatusId;
                        objscreen.CandidateId = CandidateInfo.CandidateId;
                        objscreen.Remarks = "Applied";
                        objscreen.ScreeningDate = DateTime.Now;
                        objscreen.CreatedOn = DateTime.Now;
                        objscreen.CreatedBy = CandidateInfo.CreatedBy;
                        objscreen.IsDeleted = false;
                        _context.CandidateScreenings.Add(objscreen);
                        await _context.SaveChangesAsync();

                        // ---------------------------------------------


                    }




                    // ---------------------------------------------Save and update Candidate Skills records



                    var acadresult = _context.CandidateSkills.Where(x => x.CandidateId == CandidateInfo.CandidateId).ToList();
                    if (acadresult != null && acadresult.Count() > 0)
                    {
                        foreach (var item in acadresult)
                        {
                            item.IsDeleted = true;
                            item.UpdatedBy = CandidateInfo.CreatedBy;
                            item.UpdatedOn = DateTime.Now;
                        }

                        await _context.SaveChangesAsync();

                    }

                    List<CandidateSkill> lsobjAca = new List<CandidateSkill>();

                    int a = 0;
                    if (CandidateInfo.ListSkillTitle != null)
                    {

                        foreach (var item in CandidateInfo.ListSkillTitle)
                        {

                            CandidateSkill objAca = new CandidateSkill();

                            objAca.Title = item;
                            objAca.SkillStatus = CandidateInfo.ListSkillStatus.ToArray()[a];
                            
                            objAca.CreatedOn = DateTime.Now;
                            objAca.CandidateId = CandidateInfo.CandidateId;
                            objAca.CreatedBy = CreatedBy;
                            objAca.IsDeleted= false;
                            lsobjAca.Add(objAca);
                            a++;

                        }


                        _context.CandidateSkills.AddRange(lsobjAca);
                        await _context.SaveChangesAsync();
                    }


                    //--------------------------------------------------------------------------------



                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return CandidateInfo;


                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;

                }


            }







           
        }



        public async Task<CandidateScreening> PostScreening(CandidateScreening objscreen , HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {

                try
                {
                    var checkCandidateResult = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == objscreen.CandidateId && x.IsDeleted == false);
                    if (checkCandidateResult != null && checkCandidateResult.CandidateId > 0)
                    {
                        checkCandidateResult.StatusId = Convert.ToInt32(objscreen.StatusId);
                        checkCandidateResult.UpdatedBy = objscreen.CreatedBy;
                        
                        checkCandidateResult.UpdatedOn=DateTime.Now;

                        await _context.SaveChangesAsync();
                    }




                    // Update CandidateScreening details table

                    _context.CandidateScreenings.Add(objscreen);
                    objscreen.CreatedOn= DateTime.Now;
                    objscreen.IsDeleted = false;
                    
                    await _context.SaveChangesAsync();


                    // -------------------Save and Staff records

                    if (objscreen.StatusId == 8) // Entry in Staff Table  When Candidate Offered Accepted 
                    {
                        Staff objStaff = new Staff();
                        var ResultCandidate = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == objscreen.CandidateId && x.IsDeleted == false);
                        if (ResultCandidate != null)
                        {

                            objStaff.FirstName = ResultCandidate.Name;
                            objStaff.Dob = ResultCandidate.Dob;
                            objStaff.ContactNumber1 = ResultCandidate.Phone;
                            objStaff.Gender = ResultCandidate.Gender;
                            objStaff.JobTitle = ResultCandidate.CurrentDesignation;
                            objStaff.PermanentAddress = ResultCandidate.Address;
                            objStaff.PresentAddress = ResultCandidate.City;
                            objStaff.Email = ResultCandidate.Email;
                            objStaff.DesignationId = ResultCandidate.DesignationId;
                            objStaff.SnapPath = ResultCandidate.Picture;
                            objStaff.CompanyId = ResultCandidate.CompanyId;
                            objStaff.Status = true;
                            objStaff.CreatedOn = DateTime.Now;
                            objStaff.CreatedBy = objscreen.CreatedBy;
                            objStaff.IsDeleted = false;
                        }
                        
                        _context.Staff.Add(objStaff);
                        await _context.SaveChangesAsync();
                    }
                    // ---------------------------------------------


                    dbContextTransaction.Commit();
                    return objscreen;
                }
                catch (Exception)
                {

                    dbContextTransaction.Rollback();
                    throw;

                }
            }

        }




        public async Task<bool> DeleteCandidate(int id, int UserId, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var CandidateInfo = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == id && x.IsDeleted == false);

                if (CandidateInfo != null)
                {
                    CandidateInfo.IsDeleted= true;   
                    CandidateInfo.UpdatedOn= DateTime.Now;
                    CandidateInfo.UpdatedBy = UserId;
                   

                }
                await _context.SaveChangesAsync();
                return check;
            }
            catch (Exception)
            {

                throw;
            }


        }


        public async Task<bool> AlreadyExist(int CandidateInfoId, string email,int CompanyId, HrhubContext _context)
        {
            try
            {
                if (CandidateInfoId > 0)
                {
                    var result = await _context.Candidates.FirstOrDefaultAsync(x => x.Email == email && x.CompanyId== CompanyId && x.CandidateId != CandidateInfoId && x.IsDeleted==false);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.Candidates.FirstOrDefaultAsync(x => x.Email == email && x.CompanyId == CompanyId  && x.IsDeleted == false);
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




        // load data CandidatesStatus in table on Update mode
        public async Task<List<StatusInfo>> GetCandidateStatus(HrhubContext _context)
        {
            try
            {
                var list = await _context.StatusInfos.Where(x => x.IsDeleted == false).ToListAsync();


                return list;



            }
            catch (Exception ex)
            {

                throw;

            }
        }







        // load data Candidateskills in table on Update mode
        public async Task<List<CandidateSkill>> GetCandidateSkill(int CandidateId,HrhubContext _context)
        {
            try
            {
                var list = await _context.CandidateSkills.Where(x => x.IsDeleted == false && x.CandidateId== CandidateId).ToListAsync();
                

                return list;



            }
            catch (Exception ex)
            {

                throw;

            }
        }





        // load data GetCandidateStatus in table on Update mode
        public async Task<List<CandidateScreening>> GetCandidateStatus(int CandidateId, HrhubContext _context)
        {
            try
            {
                ///   var list = await _context.CandidateScreenings.Where(x => x.IsDeleted == false && x.CandidateId == CandidateId).ToListAsync();

                var query = from cs in _context.CandidateScreenings
                            join s in _context.StatusInfos on cs.StatusId equals s.StatusId
                            join cand in _context.Candidates on cs.CandidateId equals cand.CandidateId
                            where cs.CandidateId == CandidateId && cs.IsDeleted == false
                            select new CandidateScreening
                            {
                                CandidateId = cs.CandidateId,
                                CandidateName = cand.Name,
                                ScreeningDate = cs.ScreeningDate,
                                StatusId = cs.StatusId,
                                CreatedOn = cs.CreatedOn,
                                CreatedBy = cs.CreatedBy,
                                StatusTitle = s.Title,
                                Remarks = cs.Remarks,
                                ScreeningId = cs.ScreeningId,
                                Month = cs.ScreeningDate.Value.Date.ToString("MMM"),
                                day = cs.ScreeningDate.Value.Date.Day,
                                CssColor=s.BackGroundClass,
                                AttachmentPath = string.IsNullOrWhiteSpace(cs.AttachmentPath)?"" : cs.AttachmentPath
                                



                            };

             return query!=null? query.OrderByDescending(x=>x.ScreeningId). ToList() : new List<CandidateScreening>();


            }
            catch (Exception ex)
            {

                throw;

            }
        }








    }
}
