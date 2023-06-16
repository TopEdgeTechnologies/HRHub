
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HRHUBAPI.Models.Configuration;
using System.Data;
using System.ComponentModel.Design;
using static System.Collections.Specialized.BitVector32;

namespace HRHUBAPI.Models
{



    public partial class SectionAnswer
    {
		DbConnection _db = new DbConnection();



        [NotMapped]
        public int? CheckUpdate { get; set; }

        [NotMapped]
        public int? StaffId { get; set; }
       
        [NotMapped]
        public int? DesignationId { get; set; }
        [NotMapped]
		public string? DepartmentTitle { get; set; }
		
       [NotMapped]
		public string? FirstName { get; set; }
		[NotMapped]
		public string? SectionName { get; set; }
		[NotMapped]
		public string? SectionDescription { get; set; }
		
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
        public decimal? SectionWeightage { get; set; }

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
        
        [NotMapped]
        public IEnumerable<string>? ListQuestion { get; set; }
        [NotMapped]
        public IEnumerable<int>? ListSectionQuestionId { get; set; }
        [NotMapped]
        public IEnumerable<string>? ListTotalQuestionWeightage { get; set; }
         [NotMapped]
        public IEnumerable<string>? ListAnswerWeightage { get; set; }



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



        // Update_Answer_Question Load 
        public async Task<List<SectionAnswer>> StaffSectionAnswerList(int CompanyId, int ReviewFormId, int Reviewed_StaffID, int Reviewer_StaffID, HrhubContext _context)
        {
            try
            {
                string query = "EXEC dbo.sp_AnswerSection_Update " + CompanyId + " , " + ReviewFormId + ", " + Reviewed_StaffID + "," + Reviewer_StaffID + "";
                DataTable dt = _db.ReturnDataTable(query);

                var obj = dt.AsEnumerable()
                    .Select(row => new SectionAnswer
                    {
                        AnswerId = Convert.ToInt32(row["AnswerID"]),                      
                        StaffId = Convert.ToInt32(row["Reviewed_StaffID"]),                        
                        QuestionTitle = row["QuestionTitle"].ToString(),                       
                        SectionQuestionId = Convert.ToInt32(row["SectionQuestionId"]),
                        SectionId = Convert.ToInt32(row["SectionId"]),
                        SectionWeightage = Convert.ToInt32(row["SectionWeightage"]),                                       
                        SectionName = row["SectionTitle"].ToString(),
                        SectionDescription = row["SectionDescription"].ToString(),
                        AnswerWeightage = Convert.ToDecimal(row["AnswerWeightage"]),                  
                        AnswerComments = row["AnswerComments"].ToString()                     
                         
                    }).OrderByDescending(x=>x.AnswerId)
                    .ToList();
                return obj;
            }
            catch { throw; }
        }








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
        public async Task<List<SectionQuestion>> GetSectionQuestion(int ReviewFormId, HrhubContext _context)
        {
            try
            {

                try
                {
                    string query = "EXEC dbo.sp_Load_Section_Question " + ReviewFormId + "";
                    DataTable dt = _db.ReturnDataTable(query);

                    var obj = dt.AsEnumerable()
                        .Select(row => new SectionQuestion
                        {
                            QuestionMaxLimit = row["QuestionMaxLimit"] != DBNull.Value ? Convert.ToInt32(row["QuestionMaxLimit"]) : 0,
                            QuestionId = row["QuestionId"] != DBNull.Value ? Convert.ToInt32(row["QuestionId"]) : 0,
                            Weightage = row["Weightage"] != DBNull.Value ? Convert.ToInt32(row["Weightage"]) : 0,
                            SectionQuestionId = row["SectionQuestionId"] != DBNull.Value ? Convert.ToInt32(row["SectionQuestionId"]) : 0,
                            SectionId = row["SectionId"] != DBNull.Value ? Convert.ToInt32(row["SectionId"]) : 0,
                            SectionName = row["SectionName"] != DBNull.Value ? row["SectionName"].ToString() : string.Empty,
                            ReviewName = row["ReviewName"] != DBNull.Value ? row["ReviewName"].ToString() : string.Empty,
                            SectionDescription = row["SectionDescription"] != DBNull.Value ? row["SectionDescription"].ToString() : string.Empty,
                            ReviewDescription = row["ReviewDescription"] != DBNull.Value ? row["ReviewDescription"].ToString() : string.Empty,
                            QuestionName = row["QuestionName"] != DBNull.Value ? row["QuestionName"].ToString() : string.Empty,
                            IsAnswerWeightage = row["IsAnswerWeightage"] != DBNull.Value && Convert.ToBoolean(row["IsAnswerWeightage"])

                        }).OrderByDescending(x => x.SectionQuestionId)
                        .ToList();
                    return obj;
                }
                catch { throw; }




                //var List = from S in _context.SectionQuestions
                //           join Q in _context.Questions on S.QuestionId equals Q.QuestionId
                //           join section in _context.Sections on S.SectionId equals section.SectionId
                //           join Performance in _context.PerformanceForms on section.ReviewFormId equals Performance.ReviewFormId
                //           where Q.CompanyId== CompanyId && Q.IsDeleted == false && section.ReviewFormId== ReviewFormId && Performance.IsDeleted == false && Performance.CompanyId==CompanyId

                //           select new SectionQuestion
                //           {
                //               SectionQuestionId = S.SectionQuestionId,
                //               QuestionId = S.QuestionId,
                //               SectionId = S.SectionId,
                //               Weightage = S.Weightage,
                //               QuestionName = Q.Title,
                //               SectionName = section.Title,
                //               SectionDescription = section.Description,
                //               ReviewName = Performance.Title,
                //               ReviewDescription = Performance.Description,
                //               AnswerWeightage = S.AnswerWeightage,

                //           };
                //return List != null ? List.OrderByDescending(x => x.SectionQuestionId).ToList() : new List<SectionQuestion>();


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


        public async Task<SectionAnswer> PostSectionAnswer(SectionAnswer SectionAnswerInfo, HrhubContext _context)
        {


			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{

			try
            {
                    var ObjsectionAnswer = new SectionAnswer();
                    

                  var getStaffReviewer = SectionAnswerInfo.ReviewerStaffId;                                
                    var reviewer_StaffId = _context.SectionAnswers.Any(x => x.ReviewerStaffId == getStaffReviewer);
                    if (reviewer_StaffId == true)
                    {

                    //    if (SectionAnswerInfo.ListAnswerWeightage != null)
                    //    {
                    //        _context.SectionAnswers.UpdateRange(SectionAnswerInfo);
                    //        await _context.SaveChangesAsync();
                    //    }
                    }
                    else
                    {


                    //---------------------------------------------- insert Answer Section ----------------------------

                    List<SectionAnswer> lsobjAca = new List<SectionAnswer>();

                        int a = 0;
                        if (SectionAnswerInfo.ListAnswerWeightage != null)
                        {

                            foreach (var item in SectionAnswerInfo.ListAnswerWeightage)
                            {

                                SectionAnswer objAca = new SectionAnswer();

                                objAca.AnswerWeightage = Convert.ToDecimal(item);
                                objAca.ReviewedStaffId = Convert.ToInt32(SectionAnswerInfo.ReviewedStaffId);
                                objAca.SectionQuestionId = (int)SectionAnswerInfo.ListSectionQuestionId.ToArray()[a];
                                objAca.ReviewerStaffId = SectionAnswerInfo.ReviewerStaffId;
                                objAca.ReviewerDesignationId = SectionAnswerInfo.ReviewerDesignationId;
                                objAca.CreatedOn = DateTime.Now;                               
                                objAca.CreatedBy = SectionAnswerInfo.CreatedBy;                                
                                lsobjAca.Add(objAca);
                                a++;

                            }


                        _context.SectionAnswers.AddRange(lsobjAca);
                        await _context.SaveChangesAsync();



                        //---------------------------------------------- Answer Section End ----------------------------


                        //---------------------------------------------- insert Question ----------------------------

                        List<Question> objQ = new List<Question>();

                        int b = 0;
                        if (SectionAnswerInfo.ListQuestion != null)
                        {

                            foreach (var item in SectionAnswerInfo.ListQuestion)
                            {

                                Question objAca = new Question();

                                objAca.Title = item;
                                //objAca.CompanyId = Miss here
                                objAca.CreatedOn = DateTime.Now;
                                objAca.IsDeleted = false;
                                objAca.CreatedBy = SectionAnswerInfo.CreatedBy;
                                objQ.Add(objAca);
                                b++;

                            }
                        }

                            _context.Questions.AddRange(objQ);
                            await _context.SaveChangesAsync();

                        }

                    //---------------------------------------------- End Question ----------------------------
                    }

                    var checkStaffReview = await _context.StaffReviewFormProcesseds.FirstOrDefaultAsync(x => x.ReviewedStaffId == SectionAnswerInfo.ReviewedStaffId);
                        if (checkStaffReview != null && checkStaffReview.ReviewedStaffId > 0)
                        {

                        //decimal? sumAnser = SectionAnswerInfo.Sum(ListAnswerWeightage);
                        //decimal? sumQuestion = SectionAnswerInfo.Sum(total => total.TotalQuestionWeightage);
                        //checkStaffReview.TotalWeightage = sumQuestion;
                        //checkStaffReview.EarnedWeightage = sumAnser;
                        //checkStaffReview.UpdatedBy = SectionAnswerInfo.CreatedBy;
                        //    checkStaffReview.UpdatedOn = DateTime.Now;

                        //    await _context.SaveChangesAsync();

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
