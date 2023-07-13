using HRHUBAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Numerics;

namespace HRHUBAPI.Models
{
    public partial class User
    {
        #region [NotMapped]

			[NotMapped]
			public string? CompanyName { get; set; }
			[NotMapped]
			public string? ContactPerson { get; set; }
			[NotMapped]
			public string? Email { get; set; }
			[NotMapped]
			public string? Currency { get; set; }
			[NotMapped]
			public string? Language { get; set; }
			[NotMapped]
			public string? Phone { get; set; }
			[NotMapped]
			public string? LogoAttachment { get; set; }
			[NotMapped]
			public string? WebUrl { get; set; }
			[NotMapped]
			public int? DesignationID { get; set; }
			[NotMapped]
			public string? DesignationName { get; set; }
			[NotMapped]
			public int? DepartmentId { get; set; }
			[NotMapped]
			public string? Departmentname { get; set; }
			[NotMapped]
			public string? OldPasword { get; set; }
			[NotMapped]
			public int? status { get; set; }

			[NotMapped]
			public string? EmailSendFrom { get; set; }

			[NotMapped]
			public string? EmailPassword { get; set; }

			[NotMapped]
			public int? EmailSMTPPort { get; set; }

			[NotMapped]
			public string? EmailServerHost { get; set; }

			[NotMapped] public string? StaffName { get; set; }
			[NotMapped] public string? GroupName { get; set; }

			[NotMapped] public bool? IsMarkHalfDayAllow { get; set; }
			[NotMapped] public string? SalaryFrequency { get; set; }
			[NotMapped] public string? SalaryWeekDay { get; set; }
            [NotMapped] public int? SalaryFirstDateNumber { get; set; }
            [NotMapped] public int? SalarySecondDateNumber { get; set; }

        #endregion

        public async Task<User> Login(User Obj, HrhubContext _context)
        {
            try
			{
                //Obj.Password = PasswordHasher.Encrypt(Obj.Password, true);
                var user = from u in _context.Users
                            join s in _context.VInfoStaffs on u.StaffId equals s.StaffId

                           where u.IsActive == true && u.Password == Obj.Password && u.UserName == Obj.UserName
                           select new User
                           {
                               UserId = u.UserId,
                               UserName = u.UserName,
                               Password = u.Password,
                               CompanyId = u.CompanyId,
                               GroupId = u.GroupId,
                               StaffId = u.StaffId,
                               StaffName = s.FirstName,
                               UserImage = s.SnapPath,
                               ContactPerson = s.ContactPerson,
                               DesignationName = s.DesignationTitle,
                               DesignationID = s.DesignationId,
                               DepartmentId = s.DepartmentId,
                               Departmentname = s.DepartmentTitle,
                               CompanyName = s.CompanyName,
                               SalaryFrequency = s.SalaryFrequency,
                               SalaryWeekDay = s.SalaryWeekDay,
                               SalaryFirstDateNumber = s.SalaryFirstDateNumber,
                               SalarySecondDateNumber = s.SalarySecondDateNumber,
                               Email = s.CompanyEmail,
                               Currency = s.CompanyCurrency,
                               Language = s.CompanyLanguage,
                               Phone = s.CompanyPhone,
                               LogoAttachment = s.CompanyLogoAttachment,
                               WebUrl = s.CompanyWebUrl,
                               EmailSendFrom = s.CompanyEmailSendFrom,
                               EmailPassword = s.CompanyEmailPassword,
                               EmailSMTPPort = s.CompanyEmailSmtpport,
                               EmailServerHost = s.CompanyEmailServerHost,
                               IsMarkHalfDayAllow = s.CompanyIsMarkHalfDayAllow
                           };
                //Obj.Password = PasswordHasher.Encrypt(Obj.Password, true);
                // var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == Obj.UserName && x.Password == Obj.Password && x.IsActive == true);
                //var user = from u in _context.Users
                //			join c in _context.Companies on u.CompanyId equals c.CompanyId
                //			join sss in _context.StaffSalarySettings on u.CompanyId equals sss.CompanyId
                //			join st in _context.Staff on u.StaffId equals st.StaffId
                //			into staffs
                //			from staff in staffs.DefaultIfEmpty()
                //			join d in _context.Designations on staff.DesignationId equals d.DesignationId
                //                        join dep in _context.Departments on staff.DepartmentId equals dep.DepartmentId

                //			where u.IsActive== true && u.Password== Obj.Password && u.UserName== Obj.UserName
                //			select  new User  {
                //			  UserId=  u.UserId,
                //				UserName= u.UserName,
                //				Password= u.Password,
                //				CompanyId= u.CompanyId,
                //				StaffId= u.StaffId,
                //				GroupId = u.GroupId,
                //				UserImage = staff.SnapPath,
                //				CompanyName = c.CompanyName,
                //				ContactPerson= c.ContactPerson,
                //				Email= c.Email,
                //				Currency= c.Currency,
                //				Language=  c.Language,
                //				Phone=   c.Phone,
                //				LogoAttachment= c.LogoAttachment,
                //				WebUrl= c.WebUrl,
                //                            //SalaryFrequency = sss.SalaryFrequency,
                //                            //MonthlyDateOfEveryMonth = sss.MonthlyDateOfEveryMonth,
                //                            DesignationName = d.Title,
                //				DesignationID = d.DesignationId,
                //				DepartmentId = dep.DepartmentId,
                //				Departmentname = dep.Title,
                //				EmailSendFrom=c.EmailSendFrom,
                //				EmailPassword=c.EmailPassword,
                //				EmailSMTPPort=c.EmailSmtpport,
                //				EmailServerHost=c.EmailServerHost,
                //                            StaffName= staff.FirstName,
                //				IsMarkHalfDayAllow = c.IsMarkHalfDayAllow
                //			};

                if ( user != null && user.Count() > 0)
                {
					await UserLogOutHistory(user.FirstOrDefault().UserId, _context);

					UserLoginHistory objHistory = new UserLoginHistory();
					objHistory.UserId = user.FirstOrDefault().UserId;
					objHistory.SessionFrom = DateTime.Now; 
					objHistory.CreateBy = objHistory.UserId;
					objHistory.CreatedOn= DateTime.Now;
					_context.Add(objHistory);
					_context.SaveChanges();

					return await user.FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception) { throw; }
        }

        public async Task<Company> UserCompany(int? companyid, HrhubContext _context)
        {
            try
            {
                var result = await _context.Companies.FirstOrDefaultAsync(x=>x.CompanyId== companyid);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new Company();
                }

              
                
            }
            catch (Exception)
            {

                throw;
            }
        }

		public async Task<bool> UserLogOutHistory(int userID, HrhubContext _context)
		{
			try
			{
				var result = await _context.UserLoginHistories.Where(x => x.UserId ==  userID && x.CreatedOn.Value.Date==DateTime.Now.Date && x.SessionTo==null ).ToListAsync();
				if (result != null)
				{
					foreach (var item in result)
					{
						item.UpdatedOn = DateTime.Now;
						item.UpdatedBy = userID;
						item.SessionTo = DateTime.Now;					 
					}
					_context.SaveChanges();
					return true;
				}
				else
				{
					return false;
				}
			}
            catch (Exception) { throw; }
        }

		public async Task<User> RegisterUser(User Obj, HrhubContext _context)
        {
            try
            {
                //Obj.Password = PasswordHasher.Encrypt(Obj.Password, true);
                //Obj.Password = Obj.Password;
                //Obj.Token = "";
                Obj.CreatedOn = DateTime.Now;
                Obj.IsActive = true;
                await _context.Users.AddAsync(Obj);
                await _context.SaveChangesAsync();
                return Obj;
            }
            catch (Exception) { throw; }
        }

		public async Task<User> ChangePassword(User Obj, HrhubContext _context)
		{
			try
			{
				var result=await _context.Users.FirstOrDefaultAsync(x=>x.UserId== Obj.UserId);

				if (result!=null)
                {
					result.Password=Obj.Password;
                    result.UpdatedBy = Obj.CreateBy;
                    result.UpdatedOn = DateTime.Now;

					await _context.SaveChangesAsync();
				}
                return Obj;
			}
            catch (Exception) { throw; }
        }

		public async Task<bool> AlreadyExist(int userid, string username, HrhubContext _context)
		{
			try
			{
				if (userid > 0)
				{
					var result = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username  && x.UserId != userid);
					if (result != null)
					{
						return true;
					}
				}
				else
				{
					var result = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
					if (result != null)
					{
						return true;
					}
				}
				return false;
			}
            catch (Exception) { throw; }
        }

		// Get single record of User by company ID
		public async Task<User> GetUserCompanyVise(int CompanyId, HrhubContext hrhubContext)
		{
			try
			{
				var dbResult = await hrhubContext.Users.FirstOrDefaultAsync(x =>  x.CompanyId == CompanyId);
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

        // Get single record of User by company ID and UserId
        public async Task<User> GetUserIdVise(int CompanyId,int UserId ,HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.Users.FirstOrDefaultAsync(x => x.CompanyId == CompanyId && x.UserId==UserId);
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

        #region System User
        public async Task<List<User>> GetUser(int CompanyId, HrhubContext _context)
        {
            try
            {
                //var list = await _context.Candidates.Where(x=>x.IsDeleted==false && x.CompanyId== CompanyId).ToListAsync();
                var query = from u in _context.Users
                            join S in _context.Staff on u.StaffId equals S.StaffId
                            join G in _context.GluserGroups on u.GroupId equals G.GroupId

                            where u.CompanyId == CompanyId
                            select new User
                            {
                                UserId = u.UserId,
                                UserName = u.UserName,
                                StaffId = S.StaffId,
                                StaffName = S.FirstName,
                                GroupId = G.GroupId,
                                GroupName = G.GroupTitle,
                                IsActive = u.IsActive

                            };

                return query != null ? query.OrderByDescending(x => x.UserId).ToList() : new List<User>();



            }
            catch (Exception ex)
            {

                throw;

            }
        }





        // Update Active and inactive Status User

        public async Task<bool> UpdateStatusUser(int id, bool status, int CreatedBy, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var check = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
                if (check != null && check.UserId > 0)
                {
                    check.UserId = id;
                    check.IsActive = status;
                    check.UpdatedBy = CreatedBy;
                    check.UpdatedOn = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {

                throw;

            }
        }











		#endregion

		#region Reset Password Via Email

		public async Task<bool> SendLinkPasswordReset(string Email, HrhubContext _context)
		{
			try
			{
				string msg = "";
				var check = await _context.Users.FirstOrDefaultAsync(x => x.Email == Email);
				if (check != null && check.UserId > 0)
				{
					//check.UserId = id;
					//check.IsActive = status;
					//check.UpdatedBy = CreatedBy;
					//check.UpdatedOn = DateTime.Now;

					await _context.SaveChangesAsync();
					return true;

				}
				return false;
			}
			catch (Exception ex)
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
