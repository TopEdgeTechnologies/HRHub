using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace HRHUBAPI.Models
{



    public partial class Designation
    {
        

        public async Task<List<Designation>> GetDesignation(int CompanyId,HrhubContext _context)
        {
            try
            {
                List<Designation> list = new List<Designation>();
                    list =await _context.Designations.Where(x=>x.IsDeleted==false && x.CompanyId==CompanyId).ToListAsync();

                return list;
              
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<Designation> GetDesignationById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.Designations.FirstOrDefaultAsync(x => x.DesignationId == id  && x.IsDeleted==false);
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


        public async Task<Designation> PostDesignation(Designation ObjDesignationInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkDesignationInfo = await _context.Designations.FirstOrDefaultAsync(x => x.DesignationId == ObjDesignationInfo.DesignationId && x.IsDeleted==false);
                if (checkDesignationInfo != null && checkDesignationInfo.DesignationId > 0)
                {
                    checkDesignationInfo.DesignationId = ObjDesignationInfo.DesignationId;
                    checkDesignationInfo.Title = ObjDesignationInfo.Title;
                    checkDesignationInfo.UpdatedOn = DateTime.Now;
                    checkDesignationInfo.Status = ObjDesignationInfo.Status;
                    checkDesignationInfo.UpdatedBy = ObjDesignationInfo.CreatedBy;
                  
                    await _context.SaveChangesAsync();
					return checkDesignationInfo;

				}
                else
                {
                    ObjDesignationInfo.CreatedOn = DateTime.Now;
                    ObjDesignationInfo.IsDeleted=false;
                    _context.Designations.Add(ObjDesignationInfo);
					await _context.SaveChangesAsync();

					return ObjDesignationInfo;

				}

           


            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<bool> DeleteDesignationInfo(int id, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var DesignationInfo = await _context.Designations.FirstOrDefaultAsync(x => x.DesignationId == id  && x.IsDeleted == false);

                if (DesignationInfo != null)
                {
                    DesignationInfo.IsDeleted= true;   
                    DesignationInfo.UpdatedOn= DateTime.Now;
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


        public async Task<bool> AlreadyExist(int DesignationInfoId, string title,int CompanyId, HrhubContext _context)
        {
            try
            {
                if (DesignationInfoId > 0)
                {
                    var result = await _context.Designations.FirstOrDefaultAsync(x => x.Title == title && x.CompanyId==CompanyId && x.DesignationId != DesignationInfoId && x.IsDeleted==false);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.Designations.FirstOrDefaultAsync(x => x.Title == title && x.CompanyId==CompanyId && x.IsDeleted == false);
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
