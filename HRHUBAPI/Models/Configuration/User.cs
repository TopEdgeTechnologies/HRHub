using HRHUBAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

			[NotMapped]
			public string? StaffName { get; set; }

			[NotMapped]
			public bool? MonthlyIsSpecificDayofEveryMonth { get; set; }

			[NotMapped]
			public int? MonthlyDateOfEveryMonth { get; set; }

        #endregion

        public async Task<User> Login(User Obj, HrhubContext _context)
        {
            try
            {
                //Obj.Password = PasswordHasher.Encrypt(Obj.Password, true);
                Obj.Password = Obj.Password;
                // var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == Obj.UserName && x.Password == Obj.Password && x.IsActive == true);

				var user = from u in _context.Users
							join c in _context.Companies on u.CompanyId equals c.CompanyId
							join sss in _context.StaffSalarySettings on u.CompanyId equals sss.CompanyId
							join st in _context.Staff on u.StaffId equals st.StaffId
							into staffs
							from staff in staffs.DefaultIfEmpty()
							join d in _context.Designations on staff.DesignationId equals d.DesignationId
                            join dep in _context.Departments on staff.DepartmentId equals dep.DepartmentId

							where u.IsActive== true && u.Password== Obj.Password && u.UserName== Obj.UserName
							select  new User  {
							  UserId=  u.UserId,
								UserName= u.UserName,
								Password= u.Password,
								CompanyId= u.CompanyId,
								StaffId= u.StaffId,
								GroupId = u.GroupId,
								UserImage = staff.SnapPath,
								CompanyName = c.CompanyName,
								ContactPerson= c.ContactPerson,
								Email= c.Email,
								Currency= c.Currency,
								Language=  c.Language,
								Phone=   c.Phone,
								LogoAttachment= c.LogoAttachment,
								WebUrl= c.WebUrl,
                                MonthlyIsSpecificDayofEveryMonth = sss.MonthlyIsSpecificDayofEveryMonth,
                                MonthlyDateOfEveryMonth = sss.MonthlyDateOfEveryMonth,
                                DesignationName = d.Title,
								DesignationID = d.DesignationId,
								DepartmentId = dep.DepartmentId,
								Departmentname = dep.Title,
								EmailSendFrom=c.EmailSendFrom,
								EmailPassword=c.EmailPassword,
								EmailSMTPPort=c.EmailSmtpport,
								EmailServerHost=c.EmailServerHost,
                                StaffName= staff.FirstName
                            };

				if (user != null)
                {
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
            catch (Exception)
            {

                throw;
            }
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
			catch (Exception)
			{

				throw;
			}
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
			catch (Exception)
			{

				throw;
			}
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

	}
}
