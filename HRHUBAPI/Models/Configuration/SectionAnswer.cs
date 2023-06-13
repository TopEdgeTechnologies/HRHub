
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HRHUBAPI.Models.Configuration;
using System.Data;
using System.ComponentModel.Design;

namespace HRHUBAPI.Models
{



    public partial class SectionAnswer
    {
		DbConnection _db = new DbConnection();


        [NotMapped]
        public int? StaffId { get; set; }
        [NotMapped]
        public int? DesignationId { get; set; }
        [NotMapped]
		public string? DepartmentTitle { get; set; }
		
       [NotMapped]
		public string? FirstName { get; set; }
		
        [NotMapped]
		public string? LastName { get; set; }
		
        [NotMapped]
		public string? DesignationTitle { get; set; }
	    [NotMapped]
		public string? SnapPath { get; set; }
		
       
		 [NotMapped]
		public string? PerformanceFormTitle { get; set; }
		
        [NotMapped]
		public string? QuestionTitle { get; set; }
		
       
		
		 [NotMapped]
		public decimal? TotalWeightage { get; set; }
		
       
		 [NotMapped]
		public decimal? TotalQuestionWeightage { get; set; }
		
       
		
        
		 [NotMapped]
		public decimal? EarnedWeightage { get; set; }
		
       
		

		[NotMapped]
		public int? Renew { get; set; }
		[NotMapped]
        public Staff? StaffList { get; set; }
        [NotMapped]
        public IEnumerable<PerformanceForm>? ListPerformanceForm { get; set; }

        [NotMapped]
        public IEnumerable<SectionQuestion>? ListSectionQuestion { get; set; }

         [NotMapped]
        public IEnumerable<SectionAnswer>? ListSectionAnswer { get; set; }


        // list all Comments staff List

        public async Task<List<StaffReviewFormProcessed>> ListStaffSectionAnswer(int ReviewFormId, HrhubContext _context)
        {
            try
            {
                var List = from SR in _context.StaffReviewFormProcesseds                                              
                         
                           join ST in _context.Staff on SR.ReviewedStaffId equals ST.StaffId
                           where ST.IsDeleted == false  && ST.Status==true && SR.ReviewFormId == ReviewFormId 

                           select new StaffReviewFormProcessed
                           {
                               StaffSnap = ST.SnapPath,                             
                               FirstName = ST.FirstName,
                               LastName = ST.LastName,
                               ReviewFormId= SR.ReviewFormId,
                               ReviewedStaffId = SR.ReviewedStaffId,                              
                               TotalWeightage =Convert.ToDecimal(SR.TotalWeightage),
                               EarnedWeightage = Convert.ToDecimal(SR.EarnedWeightage)


                           };
                return List != null ? List.OrderByDescending(x => x.ReviewFormId).ToList() : new List<StaffReviewFormProcessed>();


            }
            catch (Exception ex)
            {

                throw;

            }
        }




      //  public async Task<List<SectionAnswer>> ListStaffSectionAnswer(int ReviewFormId, HrhubContext _context)
      //  {
      //      try
      //      {
      //          string query = "EXEC dbo.sp_List_Answer_Comment " + ReviewFormId;
      //          DataTable dt = _db.ReturnDataTable(query);

        //          var obj = dt.AsEnumerable()
        //              .Select(row => new SectionAnswer
        //              {
        //                  SnapPath = string.IsNullOrWhiteSpace(row["SnapPath"].ToString()) ? "/Images/Avatar.png" : row["SnapPath"].ToString(),
        //                  AnswerId = Convert.ToInt32(row["AnswerID"]),
        //                  FirstName = row["FirstName"].ToString(),
        //                  LastName = row["LastName"].ToString(),                       
        //                  StaffId = Convert.ToInt32(row["Reviewed_StaffID"]),
        //                  PerformanceFormTitle = row["PerformanceFormTitle"].ToString(),
        //QuestionTitle = row["QuestionTitle"].ToString(),
        //                  TotalWeightage = Convert.ToInt32(row["TotalWeightage"]),
        //                  EarnedWeightage = Convert.ToInt32(row["EarnedWeightage"])

        //              })
        //              .ToList();
        //          return obj;
        //      }
        //      catch { throw; }
        //  }








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
                           where Q.CompanyId== CompanyId && Q.IsDeleted == false && section.ReviewFormId== ReviewFormId && Performance.IsDeleted == false && Performance.CompanyId==CompanyId

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


        public async Task<SectionAnswer> PostSectionAnswer(List<SectionAnswer> listSectionAnswerInfo, HrhubContext _context)
        {


			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{

			try
            {

                    var ObjsectionAnswer = new SectionAnswer();
                    if (listSectionAnswerInfo.Count() > 0)
                    {
                        // save Appraisal data 
                        foreach (var item in listSectionAnswerInfo)
                        {
                            item.CreatedOn = DateTime.Now;
                            item.UpdatedOn = DateTime.Now; 

                        }
                        
                        _context.SectionAnswers.AddRange(listSectionAnswerInfo);
                        await _context.SaveChangesAsync();


                        foreach (var item in listSectionAnswerInfo)
                        {
                            var checkStaffReview = await _context.StaffReviewFormProcesseds.FirstOrDefaultAsync(x => x.ReviewedStaffId == item.ReviewedStaffId);
                            if (checkStaffReview != null && checkStaffReview.ReviewedStaffId > 0)
                            {

                                decimal? sumAnser = listSectionAnswerInfo.Sum(answer => answer.AnswerWeightage);
                                decimal? sumQuestion = listSectionAnswerInfo.Sum(total => total.TotalQuestionWeightage);
                                checkStaffReview.TotalWeightage = sumQuestion;
                                checkStaffReview.EarnedWeightage = sumAnser;
                                checkStaffReview.UpdatedBy = item.CreatedBy;
                                checkStaffReview.UpdatedOn = DateTime.Now;

                                await _context.SaveChangesAsync();

                            }
                        }




                    }

                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return ObjsectionAnswer;



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


        public async Task<bool> AlreadyExists(int SectionAnswerInfoId, DateTime enddate, int staffId, HrhubContext _context)
        {
            try
            {

                //if (SectionAnswerInfoId > 0)
                //{
                //    var result = await _context.SectionAnswers.FirstOrDefaultAsync(x => x.EndDate == enddate && x.StaffId == staffId && x.SectionAnswerId != SectionAnswerInfoId && x.IsDeleted == false && x.Status == true);
                //    if (result != null)
                //    {
                //        return true;
                //    }


                //}
                //else
                //{
                //    var result = await _context.SectionAnswers.FirstOrDefaultAsync(x => x.EndDate == enddate && x.StaffId == staffId && x.IsDeleted == false && x.Status == true);
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




    }
}
