using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class Announcement
    {

        [NotMapped]
        public IEnumerable<Announcement>? ListAnnouncements { get; set; }      

        public async Task<List<Announcement>> GetAnnouncement(int CompanyId, HrhubContext hrhubContext)
        {
            try
            {
                List<Announcement> objAnnouncements = new List<Announcement>();
				objAnnouncements = await hrhubContext.Announcements.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId).ToListAsync();
                return objAnnouncements;
            }
            catch { throw; }
        }

        public async Task<Announcement> GetAnnouncementById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.Announcements.FirstOrDefaultAsync(x => x.IsDeleted == false && x.AnnouncementId == Id);
                if (dbResult != null)
                {
                    return dbResult;
                }
                else
                {
                    return null;
                }
            }
            catch { throw; }
        }
        public async Task<Announcement> UpdateStatusByAnnouncementId(Announcement ObjAnnouncementInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkAnnouncementInfo = await _context.Announcements.FirstOrDefaultAsync(x => x.AnnouncementId == ObjAnnouncementInfo.AnnouncementId && x.IsDeleted == false);
                if (checkAnnouncementInfo != null && checkAnnouncementInfo.AnnouncementId > 0)
                {
                    checkAnnouncementInfo.AnnouncementId = ObjAnnouncementInfo.AnnouncementId;
                    checkAnnouncementInfo.Status = ObjAnnouncementInfo.Status;
                    checkAnnouncementInfo.UpdatedBy = ObjAnnouncementInfo.UpdatedBy;
                    checkAnnouncementInfo.UpdatedOn = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return ObjAnnouncementInfo;

                }
                return ObjAnnouncementInfo;
            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<Announcement> PostAnnouncement(Announcement ObjAnnouncement, HrhubContext hrhubContext)
        {
          
                try
            {
                var dbResult = await hrhubContext.Announcements.FirstOrDefaultAsync(x => x.IsDeleted == false && x.AnnouncementId == ObjAnnouncement.AnnouncementId);
                if (dbResult != null && dbResult.AnnouncementId > 0)
                {
                    dbResult.AnnouncementId = ObjAnnouncement.AnnouncementId;
                    dbResult.Title = ObjAnnouncement.Title;
                    dbResult.AnnouncementDate = ObjAnnouncement.AnnouncementDate;
                    dbResult.Description = ObjAnnouncement.Description;

					dbResult.Status = ObjAnnouncement.Status;
                    dbResult.UpdatedBy = ObjAnnouncement.UpdatedBy;
                    dbResult.UpdatedOn = DateTime.Now;
                    await hrhubContext.SaveChangesAsync();
                   
                 
                    return dbResult;
                }
                else
                {
						ObjAnnouncement.CreatedOn = DateTime.Now;
						ObjAnnouncement.IsDeleted = false;
                    hrhubContext.Announcements.Add(ObjAnnouncement);
                    await hrhubContext.SaveChangesAsync();
                  
                  
                    return ObjAnnouncement;
                }
            }
            catch {  throw; }
           
        }

        public async Task<bool> DeleteAnnouncement(int id , int userid, HrhubContext hrhubContext)
        {
            try
            {
                bool recordDeleted = false;
                var dbResult = await hrhubContext.Announcements.FirstOrDefaultAsync(x => x.IsDeleted == false && x.AnnouncementId == id);
                if (dbResult != null)
                {
                    dbResult.IsDeleted = true;
                    dbResult.UpdatedOn = DateTime.Now;
                    dbResult.UpdatedBy = userid;

                    recordDeleted = true;
                }
                await hrhubContext.SaveChangesAsync();
                return recordDeleted;
            }
            catch { throw; }
        }

        public async Task<bool> AlreadyExists(int CompanyId, int Id, string title, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.Announcements.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.Title == title && x.AnnouncementId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.Announcements.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.Title == title);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch { throw; }
        }

    }
}