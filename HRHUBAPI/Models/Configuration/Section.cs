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
        public string? DesigantionName { get; set; }
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


        public async Task<List<Section>> GetSection(HrhubContext _context)
        {
            try
            {

                var list = await _context.Sections.Where(x => x.IsDeleted == false).ToListAsync();

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
                    if (checkSectionInfo != null && checkSectionInfo.SectionId > 0 && ObjSectionInfo.Renew == 0)
                    {
                        //checkSectionInfo.SectionId = ObjSectionInfo.SectionId;
                        //checkSectionInfo.StaffId = ObjSectionInfo.StaffId;
                        //checkSectionInfo.EmploymentTypeId = ObjSectionInfo.EmploymentTypeId;
                        //checkSectionInfo.ContractDuration = ObjSectionInfo.ContractDuration;
                        //checkSectionInfo.Attachment = ObjSectionInfo.Attachment;
                        //checkSectionInfo.AdditionalDetails = ObjSectionInfo.AdditionalDetails;
                        //checkSectionInfo.StartDate = ObjSectionInfo.StartDate;
                        //checkSectionInfo.EndDate = ObjSectionInfo.EndDate;
                        //checkSectionInfo.UpdatedOn = DateTime.Now;
                        //checkSectionInfo.Status = ObjSectionInfo.Status;
                        checkSectionInfo.UpdatedBy = ObjSectionInfo.CreatedBy;

                        await _context.SaveChangesAsync();



                    }
                    else
                    {



                        ObjSectionInfo.CreatedOn = DateTime.Now;
                        ObjSectionInfo.IsDeleted = false;
                        _context.Sections.Add(ObjSectionInfo);
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




    }
}
