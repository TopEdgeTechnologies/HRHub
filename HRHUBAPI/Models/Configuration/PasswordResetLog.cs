using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace HRHUBAPI.Models
{



    public partial class PasswordResetLog
    {


        [NotMapped]
        public int? Flag { get; set; } 
        [NotMapped]
        public string? IPAddress { get; set; } 
        [NotMapped]
        public string? Email { get; set; } 
		 [NotMapped]
        public string? Url { get; set; } 
		[NotMapped]
        public string? Password { get; set; } 

        [NotMapped]
        public IEnumerable<PasswordResetLog>? Listdesignation { get; set; }




		#region Reset Password Via Email

		// Send Reset pasword Email Now
		public async Task<PasswordResetLog> SendLinkPasswordReset(PasswordResetLog passwordResetLog, HrhubContext _context)
		{
			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{

				try
				{

					Random random = new Random();
					string msg = "";
					var checkUserInfo = await _context.Users.FirstOrDefaultAsync(x => x.UserName == passwordResetLog.Email);
					if (checkUserInfo != null && checkUserInfo.UserId >0)
					{

						
						string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
						string randomString = new string(Enumerable.Repeat(characters, 5)
							.Select(s => s[random.Next(s.Length)]).ToArray());

						PasswordResetLog objReset = new PasswordResetLog();
						objReset.UserId = checkUserInfo.UserId;
						objReset.RequestOn = DateTime.Now;
						objReset.RequestFromIp = passwordResetLog.RequestFromIp;
						objReset.ResetStatus = false;
						objReset.ExpiryTime= DateTime.Now.AddMinutes(15);
						objReset.CreatedOn = DateTime.Now;
						objReset.RequestFromIp= passwordResetLog.RequestFromIp;
						objReset.Token = randomString + DateTime.Now.Ticks;
						// Entry Password Reset Log table





						_context.PasswordResetLogs.Add(objReset);
						await _context.SaveChangesAsync();









						// ---------------get subject and body from EmailTemplate_HRHUB

						var Getstaff = await _context.Staff.FirstOrDefaultAsync(x => x.StaffId == checkUserInfo.StaffId);

						//Get Staff name from Staff Table and Repalce it

						var obj = _context.EmailTemplateHrhubs.FirstOrDefault(x => x.TemplateId == 2);

						// template id  is Password Reset Email 
						string EmailSubject = obj.Subject;
						string EmailBody = obj.Body;

						EmailBody = EmailBody.Replace("[StaffName]", Getstaff.FirstName);
						EmailBody = EmailBody.Replace("[ResetPasswordLink]", passwordResetLog.Url + "/ResetPassword/NewChangePassword/" + objReset.Token); // enter reset password link 

						//------------------------------------------------------------------------------





						// ------------------------insert into Email Log table
						EmailLog ObjEmail = new EmailLog();
						ObjEmail.CompanyId = checkUserInfo.CompanyId;
						ObjEmail.Subject = EmailSubject;
						ObjEmail.Body = EmailBody;
						ObjEmail.EmailTo = Getstaff.Email;
						ObjEmail.IsSent = false;
						ObjEmail.CreatedOn = DateTime.Now;

						_context.EmailLogs.Add(ObjEmail);
						await _context.SaveChangesAsync();











					}

					await _context.SaveChangesAsync();
					dbContextTransaction.Commit();
					return passwordResetLog;



				}
				catch (Exception ex)
				{
					dbContextTransaction.Rollback();
					throw;

				}


			}
		}

		// Change Password 
		public async Task<bool> PasswordReset(PasswordResetLog passwordResetLog, HrhubContext _context)
		{
			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{

				try
				{

					
					string msg = "";


					PasswordResetLog? checkPasswordLog = await _context.PasswordResetLogs.OrderByDescending(x => x.PasswordResetId).FirstOrDefaultAsync(x => x.Token == passwordResetLog.Token);// 



					if (checkPasswordLog != null && checkPasswordLog.PasswordResetId > 0 && checkPasswordLog.ExpiryTime > DateTime.Now)
					{
						// Update User table Password

						checkPasswordLog.UserId = checkPasswordLog.UserId;
						checkPasswordLog.RequestOn = DateTime.Now;
						checkPasswordLog.ResetStatus = true;
						checkPasswordLog.UpdatedOn = DateTime.Now;
						checkPasswordLog.UpdatedFromIp = passwordResetLog.UpdatedFromIp;

						await _context.SaveChangesAsync();



						/////////////////////////

						// Entry Password Reset Log table
						var Obj = await _context.Users.FirstOrDefaultAsync(x => x.UserId == checkPasswordLog.UserId);
						if (Obj != null && Obj.UserId > 0)
						{

							Obj.Password = passwordResetLog.Password;
							Obj.UpdatedOn = DateTime.Now;
							await _context.SaveChangesAsync();
						}


						await _context.SaveChangesAsync();
						dbContextTransaction.Commit();
						return true;


					}
					else {
						dbContextTransaction.Rollback();
						return false;

					}

				}
				catch (Exception ex)
				{
					dbContextTransaction.Rollback();
					throw;

				}


			}
		}


		//Get expiry date time through token
		public async Task<PasswordResetLog> CheckEpiryDates(string Token, HrhubContext _context)
		{
			try
			{
				var result = await _context.PasswordResetLogs.FirstOrDefaultAsync(x => x.Token == Token);
				if (result != null)
				{
					return result;
				}


				return null;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<bool> CheckExistEmail(string Email, HrhubContext _context)
		{
			try
			{
				var result = await _context.Users.FirstOrDefaultAsync(x => x.UserName == Email);
				if (result != null)
				{
					return true;
				}


				return false;
			}
			catch (Exception)
			{

				throw;
			}
		}


		#endregion









		


      


      
        


    }
}
