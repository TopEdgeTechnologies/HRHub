using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace HRHUBAPI.Models
{



    public partial class PerformanceForm
    {


        [NotMapped]
        public int? Flag { get; set; } 

        [NotMapped]
        public IEnumerable<PerformanceForm>? ListPerformanceForm { get; set; }
        public async Task<List<PerformanceForm>> GetPerformanceForm(int CompanyId,HrhubContext _context)
        {
            try
            {
			
				var list =await _context.PerformanceForms.Where(x=>x.IsDeleted==false && x.CompanyId==CompanyId).ToListAsync();

                return list;
              
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<PerformanceForm> GetPerformanceFormById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.PerformanceForms.FirstOrDefaultAsync(x => x.ReviewFormId == id  && x.IsDeleted==false);
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


        public async Task<PerformanceForm> PostPerformanceForm(PerformanceForm ObjPerformanceFormInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkPerformanceFormInfo = await _context.PerformanceForms.FirstOrDefaultAsync(x => x.ReviewFormId == ObjPerformanceFormInfo.ReviewFormId && x.IsDeleted==false);
                if (checkPerformanceFormInfo != null && checkPerformanceFormInfo.ReviewFormId > 0)
                {
                    checkPerformanceFormInfo.ReviewFormId = ObjPerformanceFormInfo.ReviewFormId;
                    checkPerformanceFormInfo.Title = ObjPerformanceFormInfo.Title;
                    checkPerformanceFormInfo.EndDate = ObjPerformanceFormInfo.EndDate;
                    checkPerformanceFormInfo.StartDate = ObjPerformanceFormInfo.StartDate;
                    checkPerformanceFormInfo.Description = ObjPerformanceFormInfo.Description;
                    checkPerformanceFormInfo.UpdatedOn = DateTime.Now;                    
                    checkPerformanceFormInfo.UpdatedBy = ObjPerformanceFormInfo.CreatedBy;
                   
					  await _context.SaveChangesAsync();
					checkPerformanceFormInfo.Flag = 2;
					return ObjPerformanceFormInfo;

				}
                else
                {
                    ObjPerformanceFormInfo.CreatedOn = DateTime.Now;
                    ObjPerformanceFormInfo.IsDeleted=false;
                    _context.PerformanceForms.Add(ObjPerformanceFormInfo);
					await _context.SaveChangesAsync();

					ObjPerformanceFormInfo.Flag = 1;
					return ObjPerformanceFormInfo;

				}

           


            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<bool> DeletePerformanceFormInfo(int id,int Userid, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var PerformanceFormInfo = await _context.PerformanceForms.FirstOrDefaultAsync(x => x.ReviewFormId == id  && x.IsDeleted == false);

                if (PerformanceFormInfo != null)
                {
                    PerformanceFormInfo.IsDeleted= true;   
                    PerformanceFormInfo.UpdatedOn= DateTime.Now;
                    PerformanceFormInfo.UpdatedBy = Userid;
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


        public async Task<bool> AlreadyExist(int PerformanceFormInfoId, string title,int CompanyId, HrhubContext _context)
        {
            try
            {

                if (PerformanceFormInfoId > 0)
                {
                    var result = await _context.PerformanceForms.FirstOrDefaultAsync(x => x.Title == title && x.CompanyId==CompanyId && x.ReviewFormId != PerformanceFormInfoId && x.IsDeleted==false);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.PerformanceForms.FirstOrDefaultAsync(x => x.Title == title && x.CompanyId==CompanyId && x.IsDeleted == false);
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
