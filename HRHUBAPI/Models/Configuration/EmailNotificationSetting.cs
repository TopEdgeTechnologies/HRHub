using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace HRHUBAPI.Models
{



    public partial class EmailNotificationSetting
    {


        [NotMapped]
        public int? Flag { get; set; } 

        [NotMapped]       
        public IEnumerable<EmailTemplate>? ListEmailTemplate { get; set; }

        #region EmailNotificationSetting    

        public async Task<EmailNotificationSetting> PostEmailNotificationSetting(EmailNotificationSetting ObjEmailNotificationSettingInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkEmailNotificationSettingInfo = await _context.EmailNotificationSettings.FirstOrDefaultAsync(x => x.NotificationId == ObjEmailNotificationSettingInfo.NotificationId);
                if (checkEmailNotificationSettingInfo != null && checkEmailNotificationSettingInfo.NotificationId > 0)
                {
                    checkEmailNotificationSettingInfo.NotificationId =                  ObjEmailNotificationSettingInfo.NotificationId;
                    checkEmailNotificationSettingInfo.OnCandidateEnrollment =           ObjEmailNotificationSettingInfo.OnCandidateEnrollment;
                    checkEmailNotificationSettingInfo.OnCandidateEnrollmentTemplateId = ObjEmailNotificationSettingInfo.OnCandidateEnrollmentTemplateId;
                    checkEmailNotificationSettingInfo.OnStatusChange                  = ObjEmailNotificationSettingInfo.OnStatusChange;
                    checkEmailNotificationSettingInfo.OnStatusChangeTemplateId        = ObjEmailNotificationSettingInfo.OnStatusChangeTemplateId;
                    checkEmailNotificationSettingInfo.OnApproved                      = ObjEmailNotificationSettingInfo.OnApproved;
                    checkEmailNotificationSettingInfo.OnApprovedTemplateId            = ObjEmailNotificationSettingInfo.OnApprovedTemplateId;
                    checkEmailNotificationSettingInfo.OnRejection                     = ObjEmailNotificationSettingInfo.OnRejection;
                    checkEmailNotificationSettingInfo.OnRejectionTemplateId           = ObjEmailNotificationSettingInfo.OnRejectionTemplateId;
                    checkEmailNotificationSettingInfo.OnSalaryGeneration              = ObjEmailNotificationSettingInfo.OnSalaryGeneration;
                    checkEmailNotificationSettingInfo.OnSalaryGenerationTemplateId    = ObjEmailNotificationSettingInfo.OnSalaryGenerationTemplateId;
                    checkEmailNotificationSettingInfo.CompanyId = ObjEmailNotificationSettingInfo.CompanyId;
                   
                    
					await _context.SaveChangesAsync();
					checkEmailNotificationSettingInfo.Flag = 2;
					return ObjEmailNotificationSettingInfo;

				}
                else
                {

                    ObjEmailNotificationSettingInfo.OnApprovedTemplateId = ObjEmailNotificationSettingInfo.OnApprovedTemplateId ?? 0;
                    ObjEmailNotificationSettingInfo.OnCandidateEnrollmentTemplateId = ObjEmailNotificationSettingInfo.OnCandidateEnrollmentTemplateId ?? 0;
                    ObjEmailNotificationSettingInfo.OnRejectionTemplateId = ObjEmailNotificationSettingInfo.OnRejectionTemplateId ?? 0;
                    ObjEmailNotificationSettingInfo.OnSalaryGenerationTemplateId = ObjEmailNotificationSettingInfo.OnSalaryGenerationTemplateId ?? 0;
                    ObjEmailNotificationSettingInfo.OnStatusChangeTemplateId = ObjEmailNotificationSettingInfo.OnStatusChangeTemplateId ?? 0;
                    _context.EmailNotificationSettings.Add(ObjEmailNotificationSettingInfo);
					await _context.SaveChangesAsync();

					ObjEmailNotificationSettingInfo.Flag = 1;
					return ObjEmailNotificationSettingInfo;

				}

           


            }
            catch (Exception ex)
            {

                throw;

            }
        }

        #endregion


        #region Email Template
        public async Task<List<EmailTemplate>> GetEmailTemplate(int CompanyId, HrhubContext _context)
        {
            try
            {

                var list = await _context.EmailTemplates.Where(x => x.CompanyId == CompanyId).OrderByDescending(x => x.TemplateId).ToListAsync();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<EmailTemplate> PostEmailTemplate(EmailTemplate ObjEmailTemplateInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkObjEmailTemplateInfo = await _context.EmailTemplates.FirstOrDefaultAsync(x => x.TemplateId == ObjEmailTemplateInfo.TemplateId);
                if (checkObjEmailTemplateInfo != null && checkObjEmailTemplateInfo.TemplateId > 0)
                {

                    checkObjEmailTemplateInfo.TemplateId = ObjEmailTemplateInfo.TemplateId;
                    checkObjEmailTemplateInfo.Title =   ObjEmailTemplateInfo.Title;
                    checkObjEmailTemplateInfo.Subject = ObjEmailTemplateInfo.Subject;
                    checkObjEmailTemplateInfo.Body= ObjEmailTemplateInfo.Body;
                    checkObjEmailTemplateInfo.Status = ObjEmailTemplateInfo.Status;
                    checkObjEmailTemplateInfo.UpdatedBy = ObjEmailTemplateInfo.CreatedBy;
                    checkObjEmailTemplateInfo.UpdatedOn= DateTime.Now;
                    checkObjEmailTemplateInfo.CompanyId= ObjEmailTemplateInfo.CompanyId;


                    await _context.SaveChangesAsync();
                  
                    return ObjEmailTemplateInfo;

                }
                else
                {

                    ObjEmailTemplateInfo.CreatedOn = DateTime.Now;
                    _context.EmailTemplates.Add(ObjEmailTemplateInfo);

                    await _context.SaveChangesAsync();

                   
                    return ObjEmailTemplateInfo;

                }




            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<EmailTemplate> GetEmailTemplateById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.EmailTemplates.FirstOrDefaultAsync(x=>x.TemplateId == Id);
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

        public async Task<bool> DeleteEmailTemplate(int id, int userid, HrhubContext hrhubContext)
        {
            try
            {
                bool recordDeleted = false;
                var dbResult = await hrhubContext.EmailTemplates.FirstOrDefaultAsync(x => x.TemplateId == id);
                if (dbResult != null)
                {
                   
                    dbResult.UpdatedOn = DateTime.Now;
                    dbResult.UpdatedBy = userid;

                    recordDeleted = true;
                }
                hrhubContext.Remove(dbResult);
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
                    var dbResult = await hrhubContext.EmailTemplates.FirstOrDefaultAsync(x =>  x.CompanyId == CompanyId && x.Title == title && x.TemplateId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.EmailTemplates.FirstOrDefaultAsync(x =>  x.CompanyId == CompanyId && x.Title == title);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch { throw; }
        }





        // update only status 
        public async Task<EmailTemplate> UpdateStatusByEmailTempId(EmailTemplate ObjEmailTemplate, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var check = await _context.EmailTemplates.FirstOrDefaultAsync(x => x.TemplateId == ObjEmailTemplate.TemplateId);
                if (check != null && check.TemplateId > 0)
                {
                    check.TemplateId = ObjEmailTemplate.TemplateId;
                    check.Status = ObjEmailTemplate.Status;
                    check.UpdatedBy = ObjEmailTemplate.UpdatedBy;
                    check.UpdatedOn = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return ObjEmailTemplate;

                }
                return ObjEmailTemplate;
            }
            catch (Exception ex)
            {

                throw;

            }
        }


        #endregion





    }
}
