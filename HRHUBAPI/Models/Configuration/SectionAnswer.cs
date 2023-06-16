
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HRHUBAPI.Models.Configuration;
using System.Data;
using System.ComponentModel.Design;
using static System.Collections.Specialized.BitVector32;
using System.Net.WebSockets;
using NuGet.Packaging.Core;

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
        public int? ReviewerId { get; set; }
        [NotMapped]
        public int? GetSectionId { get; set; }
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
		public string? SelfQuestion { get; set; }
		
       
		
		 [NotMapped]
		public decimal? TotalWeightage { get; set; }


        [NotMapped]
        public decimal? SectionWeightage { get; set; }

        [NotMapped]
		public decimal? TotalQuestionWeightage { get; set; }
		
        
		 [NotMapped]
		public decimal? EarnedWeightage { get; set; }

        [NotMapped]
        public bool? IsAnswerWeightage { get; set; }

        //[NotMapped]
        //public int? ListAnswerId { get; set; }

        [NotMapped]
        public int? CompanyId { get; set; }
        
        [NotMapped]
        public int? SelfReviewExistance { get; set; }

        [NotMapped]
        public bool? AllowSelfScoring { get; set; }


        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
		public int? Renew { get; set; }
		[NotMapped]
        public Staff? StaffList { get; set; }

        [NotMapped]
        public int? QuestionMaxLimit { get; set; }
        [NotMapped]
        public IEnumerable<int>? ListQuestionWeigth { get; set; }

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
        public IEnumerable<int>? ListReviewedStaffId { get; set; }
        [NotMapped]
        public IEnumerable<int>? ListSectionId { get; set; }
        [NotMapped]
        public IEnumerable<string>? ListTotalQuestionWeightage { get; set; }
         [NotMapped]
        public IEnumerable<string>? ListAnswerWeightage { get; set; }
       
        [NotMapped]
        public IEnumerable<int>? ListAnswerId { get; set; }
       


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
                        AnswerId = Convert.IsDBNull(row["AnswerID"]) ? 0 : Convert.ToInt32(row["AnswerID"]),
                        SelfReviewExistance = Convert.IsDBNull(row["SelfReviewExistance"]) ? 0 : Convert.ToInt32(row["SelfReviewExistance"]),
                        StaffId = Convert.IsDBNull(row["Reviewed_StaffID"]) ? 0 : Convert.ToInt32(row["Reviewed_StaffID"]),
                        ReviewerStaffId = Convert.IsDBNull(row["Reviewer_StaffID"]) ? 0 : Convert.ToInt32(row["Reviewer_StaffID"]),
                        QuestionTitle = row["QuestionTitle"].ToString(),
                        SectionQuestionId = Convert.IsDBNull(row["SectionQuestionId"]) ? 0 : Convert.ToInt32(row["SectionQuestionId"]),
                        GetSectionId = Convert.IsDBNull(row["SectionId"]) ? 0 : Convert.ToInt32(row["SectionId"]),
                        SectionWeightage = Convert.IsDBNull(row["SectionWeightage"]) ? 0 : Convert.ToInt32(row["SectionWeightage"]),
                        SectionName = row["SectionTitle"].ToString(),
                        SectionDescription = row["SectionDescription"].ToString(),
                        SelfQuestion = row["SelfQuestion"].ToString(),
                        AnswerWeightage = Convert.IsDBNull(row["AnswerWeightage"]) ? 0 : Convert.ToDecimal(row["AnswerWeightage"]),
                        AnswerComments = row["AnswerComments"].ToString(),
                        IsAnswerWeightage = Convert.IsDBNull(row["AnswerWeightage"]) ? false : Convert.ToBoolean(row["AnswerWeightage"]),
                        AllowSelfScoring = Convert.IsDBNull(row["AllowSelfScoring"]) ? false : Convert.ToBoolean(row["AllowSelfScoring"]),
                        QuestionMaxLimit = Convert.IsDBNull(row["QuestionMaxLimit"]) ? 0 : Convert.ToInt32(row["QuestionMaxLimit"]),
                    }).OrderByDescending(x=>x.AnswerId)
                    .ToList();
                return obj;
            }
            catch { throw; }
        }


          // Update_Answer_Question Load 
       




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
        public async Task<List<SectionQuestion>> GetSectionQuestion(int ReviewFormId,int CompanyId, HrhubContext _context)
        {
            try
            {

                try
                {
                    string query = "EXEC dbo.sp_Load_Section_Question " + ReviewFormId + ","+ CompanyId +"";
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
                            IsAnswerWeightage = row["IsAnswerWeightage"] != DBNull.Value && Convert.ToBoolean(row["IsAnswerWeightage"]),
                            AllowSelfScoring = row["AllowSelfScoring"] != DBNull.Value && Convert.ToBoolean(row["AllowSelfScoring"]),
                            
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
        public async Task<List<SectionQuestion>> SectionQuestionListSelfScoring(int ReviewFormId, int CompanyId, HrhubContext _context)
        {
            try
            {

                try
                {
                    string query = "EXEC dbo.sp_Load_Section_Question_SelfScoring " + ReviewFormId + "," + CompanyId + "";
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
                            IsAnswerWeightage = row["IsAnswerWeightage"] != DBNull.Value && Convert.ToBoolean(row["IsAnswerWeightage"]),
                            AllowSelfScoring = row["AllowSelfScoring"] != DBNull.Value && Convert.ToBoolean(row["AllowSelfScoring"]),

                        }).OrderByDescending(x => x.SectionQuestionId)
                        .ToList();
                    return obj;
                }
                catch { throw; }






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

                  var formReviewID = _context.Sections.FirstOrDefault(x => x.SectionId == SectionAnswerInfo.ListSectionId.ToArray()[0]).ReviewFormId;
                  var ObjsectionAnswer = new SectionAnswer();
                  var getStaffReviewer = SectionAnswerInfo.ReviewerStaffId;                                
                  var resultAnsers = _context.SectionAnswers.
                        Where(x => x.ReviewedStaffId== SectionAnswerInfo.ReviewedStaffId && x.AnswerComments==null && x.ReviewerStaffId== SectionAnswerInfo.ReviewerStaffId);
                    if (resultAnsers !=null && resultAnsers.Count()>0)
                    {

                         _context.SectionAnswers.RemoveRange(resultAnsers);

                            await _context.SaveChangesAsync();
                       
                    }
                    


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
                                objAca.SectionId = (int)SectionAnswerInfo.ListSectionId.ToArray()[a];
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






                        //---------------------------------------------- insert StaffReviewFormProcesseds ----------------------------

                        var ResultProcessed = _context.StaffReviewFormProcesseds.FirstOrDefault(x => x.ReviewedStaffId == SectionAnswerInfo.ReviewedStaffId && x.ReviewFormId == formReviewID);
                        if (ResultProcessed != null)
                        {
                            ResultProcessed.TotalWeightage = SectionAnswerInfo.ListQuestionWeigth.Sum();
                            ResultProcessed.EarnedWeightage = lsobjAca.Sum(x => x.AnswerWeightage);
                            ResultProcessed.ReviewFormId = formReviewID;
                            ResultProcessed.UpdatedBy = SectionAnswerInfo.CreatedBy;
                            ResultProcessed.UpdatedOn = DateTime.Now;

                            await _context.SaveChangesAsync();

                        }
                        else
                        {

                            StaffReviewFormProcessed objProcessed = new StaffReviewFormProcessed();

                            objProcessed.EarnedWeightage = lsobjAca.Sum(x => x.AnswerWeightage);
                            objProcessed.TotalWeightage = SectionAnswerInfo.ListQuestionWeigth.Sum();
                            objProcessed.ReviewedStaffId = SectionAnswerInfo.ReviewedStaffId;
                            objProcessed.ReviewFormId = formReviewID;
                            objProcessed.CreatedBy = SectionAnswerInfo.CreatedBy;
                            objProcessed.CreatedOn = DateTime.Now;

                            _context.StaffReviewFormProcesseds.Add(objProcessed);

                            await _context.SaveChangesAsync();

                        }

                        //---------------------------------------------- StaffReviewFormProcesseds End ----------------------------





                    }





                    //---------------------------------------------- insert Self Questions ----------------------------

                    var StaffReviewer = SectionAnswerInfo.ReviewerStaffId;
                    var checkExitsViewer = _context.SectionAnswers.Any(x=>x.ReviewedStaffId== StaffReviewer);
                    if (checkExitsViewer == true)
                    {
                        if (SectionAnswerInfo.ListAnswerId != null && SectionAnswerInfo.ListAnswerId.Count() > 0)
                        {
                            int c = 0;
                            foreach (var item in SectionAnswerInfo.ListAnswerId)
                            {
                                var Sectiondetail = _context.SectionAnswers.FirstOrDefault(x => x.AnswerId == item);
                                Sectiondetail.AnswerComments = string.IsNullOrWhiteSpace(SectionAnswerInfo.ListQuestion.ToArray()[c]) ? "" : SectionAnswerInfo.ListQuestion.ToArray()[c];
                                Sectiondetail.UpdatedBy = SectionAnswerInfo.CreatedBy;
                                Sectiondetail.UpdatedOn = DateTime.Now;
                                c++;

                            }
                            await _context.SaveChangesAsync();

                        }
                    }
                    else
                    {

                        List<SectionAnswer> objQ = new List<SectionAnswer>();

                        int b = 0;
                        if (SectionAnswerInfo.ListQuestion != null)
                        {

                            foreach (var item in SectionAnswerInfo.ListQuestion)
                            {

                                SectionAnswer objAca = new SectionAnswer();
                                objAca.SectionQuestionId = 0;
                                objAca.AnswerComments = item;
                                objAca.AnswerWeightage = 0;
                                objAca.CreatedOn = DateTime.Now;
                                objAca.ReviewedStaffId = SectionAnswerInfo.ReviewedStaffId;
                                objAca.ReviewerDesignationId = SectionAnswerInfo.ReviewerDesignationId;
                                objAca.SectionId = (int)SectionAnswerInfo.ListSectionId.ToArray()[a];
                                objAca.ReviewerStaffId = SectionAnswerInfo.ReviewerStaffId;
                                objAca.CreatedBy = SectionAnswerInfo.CreatedBy;
                                objQ.Add(objAca);
                                b++;

                            }
                            _context.SectionAnswers.AddRange(objQ);


                            StaffReviewFormProcessed objProcessed = new StaffReviewFormProcessed();

                            objProcessed.EarnedWeightage = 0;
                            objProcessed.TotalWeightage = 0;
                            objProcessed.ReviewedStaffId = SectionAnswerInfo.ReviewedStaffId;
                            objProcessed.ReviewFormId = formReviewID;
                            objProcessed.CreatedBy = SectionAnswerInfo.CreatedBy;
                            objProcessed.CreatedOn = DateTime.Now;

                            _context.StaffReviewFormProcesseds.Add(objProcessed);

                            await _context.SaveChangesAsync();
                        }



                    }
                   
                    //---------------------------------------------- End Self Questions ----------------------------


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
