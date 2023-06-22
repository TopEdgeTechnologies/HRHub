using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace HRHUBAPI.Models
{



    public partial class EmailNotificationSetting
    {


        [NotMapped]
        public int? Flag { get; set; } 

        [NotMapped]
        public IEnumerable<EmailNotificationSetting>? ListEmailNotificationSetting { get; set; }
        public IEnumerable<EmailTemplate>? ListEmailTemplate { get; set; }
        public async Task<List<EmailTemplate>> GetEmailTemplate(int CompanyId,HrhubContext _context)
        {
            try
            {
			
				var list =await _context.EmailTemplates.Where(x=> x.CompanyId==CompanyId).OrderByDescending(x=> x.TemplateId).ToListAsync();

                return list;
               
            }
            catch (Exception ex)
            {

                throw;

            }
        }

       

      
        public async Task<EmailNotificationSetting> PostEmailNotificationSetting(EmailNotificationSetting ObjEmailNotificationSettingInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkEmailNotificationSettingInfo = await _context.EmailNotificationSettings.FirstOrDefaultAsync(x => x.NotificationId == ObjEmailNotificationSettingInfo.NotificationId);
                if (checkEmailNotificationSettingInfo != null && checkEmailNotificationSettingInfo.NotificationId > 0)
                {
                    checkEmailNotificationSettingInfo.NotificationId = ObjEmailNotificationSettingInfo.NotificationId;
                    checkEmailNotificationSettingInfo.OnCandidateEnrollment = ObjEmailNotificationSettingInfo.OnCandidateEnrollment;
                    checkEmailNotificationSettingInfo.OnCandidateEnrollmentTemplateId = ObjEmailNotificationSettingInfo.OnCandidateEnrollmentTemplateId;
                    checkEmailNotificationSettingInfo.OnStatusChange = ObjEmailNotificationSettingInfo.OnStatusChange;
                    checkEmailNotificationSettingInfo.OnStatusChangeTemplateId = ObjEmailNotificationSettingInfo.OnStatusChangeTemplateId;
                    checkEmailNotificationSettingInfo.OnApproved = ObjEmailNotificationSettingInfo.OnApproved;
                    checkEmailNotificationSettingInfo.OnApprovedTemplateId = ObjEmailNotificationSettingInfo.OnApprovedTemplateId;
                    checkEmailNotificationSettingInfo.OnRejection = ObjEmailNotificationSettingInfo.OnRejection;
                    checkEmailNotificationSettingInfo.OnRejectionTemplateId = ObjEmailNotificationSettingInfo.OnRejectionTemplateId;
                    checkEmailNotificationSettingInfo.OnSalaryGeneration = ObjEmailNotificationSettingInfo.OnSalaryGeneration;
                    checkEmailNotificationSettingInfo.OnSalaryGenerationTemplateId = ObjEmailNotificationSettingInfo.OnSalaryGenerationTemplateId;
                    checkEmailNotificationSettingInfo.CompanyId = ObjEmailNotificationSettingInfo.CompanyId;
                   
                    
					await _context.SaveChangesAsync();
					checkEmailNotificationSettingInfo.Flag = 2;
					return ObjEmailNotificationSettingInfo;

				}
                else
                {
                   
                   
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


        

     
        


    }
}
