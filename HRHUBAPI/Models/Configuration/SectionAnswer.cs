
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HRHUBAPI.Models.Configuration;
using System.Data;

namespace HRHUBAPI.Models
{



    public partial class SectionAnswer
    {
		DbConnection _db = new DbConnection();

		[NotMapped]
		public string? DesigantionName { get; set; }
		

		[NotMapped]
		public int? Renew { get; set; }
		[NotMapped]
        public Staff? StaffList { get; set; }
        [NotMapped]
        public IEnumerable<PerformanceForm>? ListPerformanceForm { get; set; }

        // list all active contract data
        public async Task<Staff> GetStaffProfile(int StaffId,HrhubContext _context)
        {
            try
            {
                var List = from S in _context.Staff
                           join Design in _context.Designations on S.DesignationId equals Design.DesignationId
                           join D in _context.Departments on S.DepartmentId equals D.DepartmentId                          
                           where S.IsDeleted == false && Design.IsDeleted == false && S.Status == true && Design.Status == true && D.IsDeleted == false && D.Status == true && S.StaffId==StaffId

                           select new Staff
                           {
                               StaffId = S.StaffId,
                               FirstName = S.FirstName,
                               LastName = S.LastName,
                               DesignationTitle = Design.Title,
                               DepartmentTitle = D.Title,
                               DepartmentId = S.DepartmentId,
                               DesignationId =S.DesignationId,
                               SnapPath = string.IsNullOrWhiteSpace(S.SnapPath) ? "/Images/StaffImageEmpty.jpg" : S.SnapPath,
                               RegistrationNo = S.RegistrationNo,
                               ContactNumber1 = S.ContactNumber1,
                               Email = S.Email,
                               PermanentAddress = S.PermanentAddress,
                               NationalIdnumber = S.NationalIdnumber,
                               

                           };
                return List != null ?   List.FirstOrDefault() : new Staff();



            }
            catch (Exception ex)
            {

                throw;

            }

            return null;
        }
        
        // Load all section and Question 
        public async Task<List<SectionQuestion>> GetSectionQuestion(int CompanyId,int ReviewFormId, HrhubContext _context)
        {
            try
            {
                var List = from S in _context.SectionQuestions
                           join Q in _context.Questions on S.QuestionId equals Q.QuestionId
                           join section in _context.Sections on S.SectionId equals section.SectionId
                           join Performance in _context.PerformanceForms on section.ReviewFormId equals Performance.ReviewFormId
                           where Q.CompanyId== CompanyId && Q.IsDeleted == false && Performance.ReviewFormId== ReviewFormId && Performance.IsDeleted == false && Performance.CompanyId==CompanyId

                           select new SectionQuestion
                           {
                               SectionQuestionId = S.SectionQuestionId,
                               QuestionId = S.QuestionId,
                               SectionId = S.SectionId,
                               Weightage = S.Weightage,
                               QuestionName = Q.Title,
                               SectionName = section.Title,
                               SectionDescription = section.Description,
                               ReviewName = Performance.Title,
                               ReviewDescription = Performance.Description,
                               

                           };
                return List != null ? List.OrderByDescending(x => x.SectionQuestionId).ToList() : new List<SectionQuestion>();


            }
            catch (Exception ex)
            {

                throw;

            }

            return null;
        }

        public async Task<SectionAnswer> SectionAnswerById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.SectionAnswers.FirstOrDefaultAsync(x => x.AnswerId == id);
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


        public async Task<SectionAnswer> PostSectionAnswer(SectionAnswer ObjSectionAnswerInfo, HrhubContext _context)
        {


			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{

			try
            {


                string msg = "";
                var checkSectionAnswerInfo = await _context.SectionAnswers.FirstOrDefaultAsync(x => x.AnswerId == ObjSectionAnswerInfo.AnswerId);
                if (checkSectionAnswerInfo != null && checkSectionAnswerInfo.AnswerId > 0 && ObjSectionAnswerInfo.Renew==0)
                {
                   // checkSectionAnswerInfo.SectionAnswerId = ObjSectionAnswerInfo.SectionAnswerId;
                   // checkSectionAnswerInfo.StaffId = ObjSectionAnswerInfo.StaffId;
                   // checkSectionAnswerInfo.EmploymentTypeId = ObjSectionAnswerInfo.EmploymentTypeId;                   
                   // checkSectionAnswerInfo.ContractDuration = ObjSectionAnswerInfo.ContractDuration;
                   // checkSectionAnswerInfo.Attachment = ObjSectionAnswerInfo.Attachment;
                   // checkSectionAnswerInfo.AdditionalDetails = ObjSectionAnswerInfo.AdditionalDetails;
                   //checkSectionAnswerInfo.StartDate = ObjSectionAnswerInfo.StartDate;
                   //checkSectionAnswerInfo.EndDate = ObjSectionAnswerInfo.EndDate;
                   // checkSectionAnswerInfo.UpdatedOn = DateTime.Now;
                   // checkSectionAnswerInfo.Status = ObjSectionAnswerInfo.Status;
                    //checkSectionAnswerInfo.UpdatedBy = ObjSectionAnswerInfo.CreatedBy;

                    await _context.SaveChangesAsync();
                   
                  

                }
                else
                {


     //           var result=    _context.SectionAnswers.Where(x => x.StaffId == ObjSectionAnswerInfo.StaffId && x.IsDeleted == false);
     //               if (result!=null && result.Count()>0)
     //               {
     //                   foreach (var item in result)
     //                   {
     //                       item.Status = false;
     //                       item.UpdatedOn = DateTime.Now;
     //                       item.UpdatedBy = ObjSectionAnswerInfo.CreatedBy;
     //                       item.IsDeleted = true;
					//	}
					//	await _context.SaveChangesAsync();
					//}
     //                   if (ObjSectionAnswerInfo.Renew == 1)
     //                       ObjSectionAnswerInfo.SectionAnswerId = 0;


                    ///new button bit in object , check the current object staffid in contract , previous records of indiviual staffid  status inactive (0)

                    //ObjSectionAnswerInfo.CreatedOn = DateTime.Now;
                    //ObjSectionAnswerInfo.IsDeleted = false;
                    _context.SectionAnswers.Add(ObjSectionAnswerInfo);
                    await _context.SaveChangesAsync();

						

                }
					await _context.SaveChangesAsync();
					dbContextTransaction.Commit();
					return ObjSectionAnswerInfo;



				}
            catch (Exception ex)
            {
					dbContextTransaction.Rollback();
					throw;

            }
       
        
        }
        }


        public async Task<bool> DeleteSectionAnswerInfo(int id,int Userid, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var SectionAnswerInfo = await _context.SectionAnswers.FirstOrDefaultAsync(x => x.AnswerId == id);

                if (SectionAnswerInfo != null)
                {
                    //SectionAnswerInfo.IsDeleted= true;   
                    //SectionAnswerInfo.UpdatedOn= DateTime.Now;
                    //SectionAnswerInfo.UpdatedBy = Userid;
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


        //public async Task<bool> AlreadyExists(int SectionAnswerInfoId, DateTime enddate, int staffId, HrhubContext _context)
        //{
        //    try
        //    {

        //        if (SectionAnswerInfoId > 0)
        //        {
        //            var result = await _context.SectionAnswers.FirstOrDefaultAsync(x => x.EndDate == enddate && x.StaffId == staffId && x.SectionAnswerId != SectionAnswerInfoId && x.IsDeleted == false && x.Status==true);
        //            if (result != null)
        //            {
        //                return true;
        //            }


        //        }
        //        else
        //        {
        //            var result = await _context.SectionAnswers.FirstOrDefaultAsync(x => x.EndDate == enddate && x.StaffId == staffId && x.IsDeleted == false && x.Status == true);
        //            if (result != null)
        //            {
        //                return true;
        //            }

        //        }

        //        return false;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}



  //      //Load dropdown Employeement Type
  //      public async Task<List<EmploymentType>> GetEmploymentType(int CompnayId, HrhubContext hrhubContext)
		//{
		//	try
		//	{
		//		List<EmploymentType> objEmploymentType = new List<EmploymentType>();
		//		objEmploymentType = await hrhubContext.EmploymentTypes.Where(x => x.IsDeleted == false && x.Status == true && x.CompanyId== CompnayId).ToListAsync();
		//		return objEmploymentType;
		//	}
		//	catch { throw; }
		//}




	}
}
