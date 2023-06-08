using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using HRHUBAPI.Models.Configuration;

namespace HRHUBAPI.Models
{



    public partial class Section
    {
        DbConnection _db = new DbConnection();

       
        [NotMapped]
        public int? Flag { get; set; } 
        [NotMapped]
        public string? EmployementType { get; set; }
        [NotMapped]
        public string? StaffImg { get; set; }
        [NotMapped]
        public string? SatffFName { get; set; }
        [NotMapped]
        public string? SatffLName { get; set; }

        [NotMapped]
        public string? ExpirayStatus { get; set; }

        [NotMapped]
        public int? Renew { get; set; }
        [NotMapped]
        public IEnumerable<Section>? AllSection { get; set; }

		[NotMapped]
		public IEnumerable<string>? ListQuestion { get; set; }
		[NotMapped]
		public IEnumerable<decimal>? ListWeight { get; set; }



		#region Section


		public async Task<List<Section>> GetSection(int ReviewFormId,HrhubContext _context)
        {
            try
            {

                var list = await _context.Sections.Where(x => x.IsDeleted == false && x.ReviewFormId== ReviewFormId).ToListAsync();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<Section> SectionById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.Sections.FirstOrDefaultAsync(x => x.SectionId == id && x.IsDeleted == false);
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


        public async Task<Section> PostSection(Section ObjSectionInfo, HrhubContext _context)
        {


            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {

                try
                {


                    string msg = "";
                    var checkSectionInfo = await _context.Sections.FirstOrDefaultAsync(x => x.SectionId == ObjSectionInfo.SectionId && x.IsDeleted == false);
                    if (checkSectionInfo != null && checkSectionInfo.SectionId > 0 )
                    {
                        checkSectionInfo.SectionId = ObjSectionInfo.SectionId;
                        checkSectionInfo.ReviewFormId = ObjSectionInfo.ReviewFormId;
                        checkSectionInfo.OrderNo = ObjSectionInfo.OrderNo;
                        checkSectionInfo.Title = ObjSectionInfo.Title;
                        checkSectionInfo.Description = ObjSectionInfo.Description;
                        checkSectionInfo.IsAnswerWeightage = ObjSectionInfo.IsAnswerWeightage;
                        checkSectionInfo.AllowSelfScoring = ObjSectionInfo.AllowSelfScoring;
                        checkSectionInfo.QuestionMaxLimit = ObjSectionInfo.QuestionMaxLimit;
                        checkSectionInfo.TotalWeightage = ObjSectionInfo.TotalWeightage;                     
                             
                        checkSectionInfo.UpdatedBy = ObjSectionInfo.CreatedBy;
                        ObjSectionInfo.Flag = 2;
                        await _context.SaveChangesAsync();



                    }
                    else
                    {
                        // Section Table Entry 


                        ObjSectionInfo.CreatedOn = DateTime.Now;
                        ObjSectionInfo.TotalWeightage = 0;
                      
                        ObjSectionInfo.IsDeleted = false;
                        _context.Sections.Add(ObjSectionInfo);
                        ObjSectionInfo.Flag = 1;
                        await _context.SaveChangesAsync();


                        ///////


                    }

                    // ---------------------------------------------Save and update Candidate Skills records



                    var acadresult = _context.SectionQuestions.Where(x => x.SectionId == ObjSectionInfo.SectionId).ToList();
                    if (acadresult != null && acadresult.Count() > 0)
                    {
                        foreach (var item in acadresult)
                        {
                            
                            item.UpdatedBy = ObjSectionInfo.CreatedBy;
                            item.UpdatedOn = DateTime.Now;
                            _context.SectionQuestions.RemoveRange(item);
                        }

                        await _context.SaveChangesAsync();

                    }

                    List<SectionQuestion> lsobjAca = new List<SectionQuestion>();

                    int a = 0;
                    if (ObjSectionInfo.ListQuestion != null)
                    {

                        foreach (var item in ObjSectionInfo.ListQuestion)
                        {

                            SectionQuestion objAca = new SectionQuestion();

                            objAca.QuestionId = Convert.ToInt32(item);
                            objAca.Weightage = ObjSectionInfo.ListWeight.ToArray()[a]!=null? ObjSectionInfo.ListWeight.ToArray()[a]:null;
                            objAca.CreatedOn = DateTime.Now;
                            objAca.SectionId = ObjSectionInfo.SectionId;
                            objAca.CreatedBy = ObjSectionInfo.CreatedBy;                            
                            lsobjAca.Add(objAca);
                            a++;

                        }

                        _context.SectionQuestions.AddRange(lsobjAca);
                        await _context.SaveChangesAsync();
                    }





                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return ObjSectionInfo;



                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;

                }


            }
        }


        public async Task<Section> DeleteSectionInfo(int id, int Userid, HrhubContext _context)
        {
            try
            {
               
                var SectionInfo = await _context.Sections.FirstOrDefaultAsync(x => x.SectionId == id && x.IsDeleted == false);

                if (SectionInfo != null)
                {
                    SectionInfo.IsDeleted = true;
                    SectionInfo.UpdatedOn = DateTime.Now;
                    SectionInfo.UpdatedBy = Userid;
                   

                }
                await _context.SaveChangesAsync();
                return SectionInfo;
            }
            catch (Exception)
            {

                throw;
            }


        }


        public async Task<bool> AlreadyExist(int SectionID, string title, HrhubContext _context)
        {
            try
            {

                if (SectionID > 0)
                {
                    var result = await _context.Sections.FirstOrDefaultAsync(x => x.Title == title && x.SectionId != SectionID && x.IsDeleted == false);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.Sections.FirstOrDefaultAsync(x => x.Title == title && x.IsDeleted == false);
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

		#endregion


		#region Question 
		public async Task<List<Question>> GetQuestion(int CompanyId, HrhubContext _context)
		{
			try
			{

				var list = await _context.Questions.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId).ToListAsync();

				return list;

			}
			catch (Exception ex)
			{

				throw;

			}
		}


		public async Task<Question> QuestionById(int id, HrhubContext _context)
		{
			try
			{

				var result = await _context.Questions.FirstOrDefaultAsync(x => x.QuestionId == id && x.IsDeleted == false);
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

		public async Task<Question> PostQuestion(Question ObjQuestionInfo, HrhubContext _context)
		{


			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{

				try
				{


					string msg = "";
					var checkSectionInfo = await _context.Questions.FirstOrDefaultAsync(x => x.QuestionId == ObjQuestionInfo.QuestionId && x.IsDeleted == false);
					if (checkSectionInfo != null && checkSectionInfo.QuestionId > 0)
					{
						checkSectionInfo.QuestionId = ObjQuestionInfo.QuestionId;
						checkSectionInfo.Title = ObjQuestionInfo.Title;					

						checkSectionInfo.UpdatedBy = ObjQuestionInfo.CreatedBy;                      
                        await _context.SaveChangesAsync();



					}
					else
					{



						ObjQuestionInfo.CreatedOn = DateTime.Now;
						ObjQuestionInfo.IsDeleted = false;
						_context.Questions.Add(ObjQuestionInfo);
						await _context.SaveChangesAsync();



					}
					await _context.SaveChangesAsync();
					dbContextTransaction.Commit();
					return ObjQuestionInfo;



				}
				catch (Exception ex)
				{
					dbContextTransaction.Rollback();
					throw;

				}


			}
		}

		public async Task<Question> DeleteQuestionInfo(int id, int Userid, HrhubContext _context)
		{
			try
			{

				var QuestionInfo = await _context.Questions.FirstOrDefaultAsync(x => x.QuestionId == id && x.IsDeleted == false);

				if (QuestionInfo != null)
				{
					QuestionInfo.IsDeleted = true;
					QuestionInfo.UpdatedOn = DateTime.Now;
					QuestionInfo.UpdatedBy = Userid;


				}
				await _context.SaveChangesAsync();
				return QuestionInfo;
			}
			catch (Exception)
			{

				throw;
			}


		}




		public async Task<bool> AlreadyExistQuestion(int QuestionId, string title,int CompanyId, HrhubContext _context)
		{
			try
			{

				if (QuestionId > 0)
				{
					var result = await _context.Questions.FirstOrDefaultAsync(x => x.Title == title && x.QuestionId != QuestionId && x.IsDeleted == false && x.CompanyId==CompanyId);
					if (result != null)
					{
						return true;
					}


				}
				else
				{
					var result = await _context.Questions.FirstOrDefaultAsync(x => x.Title == title && x.IsDeleted == false && x.CompanyId == CompanyId);
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

        #endregion


        #region Section Question
        public async Task<List<SectionQuestion>> GetSectionQuestion(int SectionId, HrhubContext _context)
        {
            try
            {

                


                var List = from SQ in _context.SectionQuestions
                           join Q in _context.Questions on SQ.QuestionId equals Q.QuestionId
                           where SQ.SectionId == SectionId && Q.IsDeleted == false
                           select new SectionQuestion
                           {
                               SectionQuestionId = SQ.SectionQuestionId,
                               QuestionId = SQ.QuestionId,
                               Weightage = SQ.Weightage,
                               QuestionName = Q.Title

                           };
                return List != null ? List.OrderByDescending(x => x.SectionQuestionId).ToList() : new List<SectionQuestion>();





                

            }
            catch (Exception ex)
            {

                throw;

            }
        } 
        #endregion
    }
}
