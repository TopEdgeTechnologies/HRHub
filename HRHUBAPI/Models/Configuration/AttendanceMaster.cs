using HRHUBAPI.Models.Configuration;
using HRHUBAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data;
using System;


namespace HRHUBAPI.Models
{
    public partial class AttendanceMaster
    {
		DbConnection _db = new DbConnection();

		[NotMapped]
		public int? CompanyID { get; set; }
	
		[NotMapped]
		public int? TotalCount { get; set; }
	

		[NotMapped]
        public string? Attendancestatus { get; set; }
		[NotMapped]
        public string? RegistrationNo { get; set; }

		[NotMapped]
		public string? DepartmentName { get; set; }
		[NotMapped]
		public string? DesignationName { get; set; }
		[NotMapped]
		public string? StaffName { get; set; }
		[NotMapped]
		public string? TitleStatus { get; set; }

		[NotMapped]
		public string? CssClass { get; set; }

		[NotMapped]
		public string? AttendanceFormateDate { get; set; }


		[NotMapped]
		public string? AttendanceFormateDateIn { get; set; }
		[NotMapped]
		public string? AttendanceFormateDateOut { get; set; }

		[NotMapped]
        public IEnumerable<AttendanceMaster>? listAttendance { get; set; }

        public Task<List<AttendanceMaster>> GetAttendance(int staffid,  string datefrom, string dateTo,  HrhubContext _context)
        {
            try
            {
				//DateTime Newdatefrom = DateTime.ParseExact(datefrom, "dd MMM yyyy", CultureInfo.InvariantCulture);
				//DateTime Newdateto = DateTime.ParseExact(dateTo, "dd MMM yyyy", CultureInfo.InvariantCulture);
				var query = from at in _context.AttendanceMasters
                            join s in _context.AttendanceStatuses on at.AttendanceStatusId equals s.AttendanceStatusId
                            where at.StaffId == staffid && at.IsDeleted == false && at.AttendanceDate >= Convert.ToDateTime(datefrom) && at.AttendanceDate <= Convert.ToDateTime(dateTo)
							select new AttendanceMaster
                            {
                                AttendanceId = at.AttendanceId,
                                Attendancestatus = s.Title,
                                AttendanceDate = at.AttendanceDate,
                                FirstPunchIn = at.FirstPunchIn,
                                LastPunchOut = at.LastPunchOut,
                                TotalWorkingMinutes = at.TotalWorkingMinutes,
                                TotalDefinedMinutes = at.TotalDefinedMinutes,
                                AttendanceStatusId = at.AttendanceStatusId,
                                LateMinutes = at.LateMinutes,
                                CreatedBy = at.CreatedBy,
                                LeaveTypeId = at.LeaveTypeId,
                                MarkAsHalfLeave = at.MarkAsHalfLeave,
                                NonPaidAttendance = at.NonPaidAttendance,
                                OverTimeMinutes = at.OverTimeMinutes,
                                Remarks = at.Remarks,
                                StaffId = at.StaffId,
                                UpdatedBy = at.UpdatedBy,
                                CreatedOn = at.CreatedOn,
                                GraceLateMinutes = at.GraceLateMinutes,
                                UpdatedOn = at.UpdatedOn,
                                MarkedAsHalfDay = at.MarkedAsHalfDay,
                                MarkAsShortLeave = at.MarkAsShortLeave,
                                MarkedAsShortDay = at.MarkedAsShortDay,
                                IsDeleted = at.IsDeleted,
                                AttendanceFormateDate = at.AttendanceDate.Value.Date.ToString("dd-MMM-yyyy"),
								CssClass = s.CssClass

								// AttendanceFormateDateIn = at.FirstPunchIn.Value.ToString("h:mm tt"),
								// AttendanceFormateDateOut = at.LastPunchOut.Value.ToString("h:mm tt")
							};
               return Task.FromResult(query != null ? query.OrderByDescending(x => x.AttendanceDate).ToList() : new List<AttendanceMaster>());
            }
            catch (Exception ex) { throw; }
        }

		// Get Attendence Over View List From and To Date Wise
		public async Task<List<dynamic>> GetAttendanceOverViewList(int StaffId, int DepartmentId, int monthId, int yearId, HrhubContext _context)
		{
			try
			{
				string query = "EXEC dbo.sp_getAttendanceMonthlyReport " + monthId + "," + yearId + "," + DepartmentId + "," + StaffId + " ";
				return  _db.ReturnDataTable(query).ToDynamicList();
			}
			catch { throw; }
		}

        // Get Attendence Over View List From and To Date Wise
        public async Task<List<dynamic>> GetAttendanceLeaveWiseList(int StaffId, int DepartmentId, string datefrom, string dateTo, HrhubContext _context)
        {




            try
            {
                string query = "EXEC dbo.sp_getAttendanceMonthlyReport " + datefrom + "," + dateTo + "," + DepartmentId + "," + StaffId + " ";
                return _db.ReturnDataTable(query).ToDynamicList();





            }
            catch { throw; }


        }




        // Get Attendence date vise
        public async Task<List<AttendanceMaster>> GetAttendancedatevise(int CompanyId,int DepartmentId, string Attendencedate,  HrhubContext _context)
		{
			List<AttendanceMaster> lis = new List<AttendanceMaster>();
			
			try
			{
				string query = "EXEC HR.Get_AttendanceDateWise " + CompanyId + " ," + DepartmentId + " , '" + Attendencedate + "' ";
				DataTable dt = _db.ReturnDataTable(query);

				lis = dt.AsEnumerable()

					.Select(row => new AttendanceMaster
					{
						AttendanceId = string.IsNullOrWhiteSpace(row["AttendanceId"].ToString()) ? 0 : Convert.ToInt32(row["AttendanceId"]),
						StaffId = Convert.ToInt32(row["StaffID"]),
						RegistrationNo = row["RegistrationNo"].ToString(),
						StaffName = row["FullName"].ToString(),
						DepartmentName = row["DepartmentName"].ToString(),
						DesignationName = row["DesignationName"].ToString(),
						TitleStatus = string.IsNullOrWhiteSpace(row["StatusTitle"].ToString()) ? "" : row["StatusTitle"].ToString(),						
						FirstPunchIn =  string.IsNullOrWhiteSpace(row["FirstPunchIn"].ToString())?null: (TimeSpan)row["FirstPunchIn"] ,
						LastPunchOut = string.IsNullOrWhiteSpace(row["LastPunchOut"].ToString()) ? null : (TimeSpan)row["LastPunchOut"],
						AttendanceStatusId =  string.IsNullOrWhiteSpace(row["AttendanceStatusId"].ToString())?null:  Convert.ToInt32(row["AttendanceStatusId"]),
						CssClass = string.IsNullOrWhiteSpace(row["CssClass"].ToString()) ? "" : row["CssClass"].ToString(),

					}).OrderByDescending(x=>x.AttendanceId).ToList();

		
		

				return lis;
			}
			catch { throw; }



		}

		
		private int calculteHourstoMinute(TimeSpan? to , TimeSpan? from )
		{
			try
			{
				TimeSpan timeDifference = (TimeSpan)to - (TimeSpan)from;
				return (int)timeDifference.TotalMinutes;

			}
			catch (Exception)
			{

				throw;
			}
	 

		}

		// Check Attendence weekend wise and holiday wise
		private bool CheckAttendanceIN(DateTime? holidaydate, HrhubContext _context)
		{
			try
			{
				var checkHoliday =_context.Holidays.Any(x => x.HolidayDate == holidaydate);
				var checkWeek = _context.WeekendRules.Any(x => x.DayName == Convert.ToString(holidaydate.Value.DayOfWeek));
				if(checkHoliday == true || checkWeek == true)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}




		// post Attendence

		public async Task<AttendanceMaster> PostAttendence(AttendanceMaster ObjAttendanceMaster, HrhubContext _context)
		{
		
			var companyobj= _context.Companies.FirstOrDefault(x => x.CompanyId == ObjAttendanceMaster.CompanyID);




			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{

				try
				{
					string msg = "";
					var checkAttendenceInfo = await _context.AttendanceMasters.FirstOrDefaultAsync(x => x.AttendanceDate == ObjAttendanceMaster.AttendanceDate
					&& x.StaffId == ObjAttendanceMaster.StaffId && x.IsDeleted == false);
					if (checkAttendenceInfo != null && checkAttendenceInfo.AttendanceId > 0)
					{
						checkAttendenceInfo.UpdatedOn = DateTime.Now;
						checkAttendenceInfo.UpdatedBy = ObjAttendanceMaster.CreatedBy;
						checkAttendenceInfo.LastPunchOut = ObjAttendanceMaster.LastPunchOut;
						checkAttendenceInfo.TotalWorkingMinutes = calculteHourstoMinute(ObjAttendanceMaster.LastPunchOut, checkAttendenceInfo.FirstPunchIn);
						checkAttendenceInfo.LateMinutes = Convert.ToInt32(checkAttendenceInfo.TotalDefinedMinutes - checkAttendenceInfo.TotalWorkingMinutes);

						await _context.SaveChangesAsync();



						var detailAttendance = _context.AttendanceDetails.Where(x => x.AttendanceId == checkAttendenceInfo.AttendanceId).OrderByDescending(x => x.AttendanceDetailId).FirstOrDefault();




						if (detailAttendance != null && detailAttendance.AttendanceDetailId > 0 && detailAttendance.TimeOut == null)
						{
							detailAttendance.TimeOut = checkAttendenceInfo.LastPunchOut;
							detailAttendance.UpdatedOn = DateTime.Now;
							detailAttendance.UpdatedBy = ObjAttendanceMaster.CreatedBy;

							detailAttendance.IsDeleted = false;
							detailAttendance.WorkingMinutes = calculteHourstoMinute(ObjAttendanceMaster.LastPunchOut, detailAttendance.TimeIn);

							await _context.SaveChangesAsync();


						}
						else
						{
							AttendanceDetail objdetail = new AttendanceDetail();
							objdetail.AttendanceId = checkAttendenceInfo.AttendanceId;
							objdetail.TimeIn = ObjAttendanceMaster.FirstPunchIn;
							objdetail.CreatedOn = DateTime.Now;
							objdetail.UpdatedOn = DateTime.Now;
							objdetail.UpdatedBy = ObjAttendanceMaster.CreatedBy;
							objdetail.CreatedBy = ObjAttendanceMaster.CreatedBy;
							objdetail.IsDeleted = false;
							detailAttendance.WorkingMinutes = calculteHourstoMinute(ObjAttendanceMaster.LastPunchOut, detailAttendance.TimeIn);

							_context.AttendanceDetails.Add(objdetail);
							await _context.SaveChangesAsync();


						}


						dbContextTransaction.Commit();
						return checkAttendenceInfo;



					}
					else
					{
						if (!CheckAttendanceIN(ObjAttendanceMaster.AttendanceDate, _context)) { return new AttendanceMaster(); }


						/// Intial Entry Attendence Master						
						ObjAttendanceMaster.CreatedOn = DateTime.Now;
						ObjAttendanceMaster.AttendanceStatusId = 1;
						ObjAttendanceMaster.LastPunchOut = null;

						ObjAttendanceMaster.IsDeleted = false;
						ObjAttendanceMaster.TotalDefinedMinutes = calculteHourstoMinute(companyobj.OfficeEndTime, companyobj.OfficeStartTime);
						_context.AttendanceMasters.Add(ObjAttendanceMaster);
						await _context.SaveChangesAsync();



						//detail insert

						AttendanceDetail objdetail = new AttendanceDetail();
						objdetail.AttendanceId = ObjAttendanceMaster.AttendanceId;
						objdetail.TimeIn = ObjAttendanceMaster.FirstPunchIn;
						objdetail.TimeOut = null;

						objdetail.IsDeleted = false;
						objdetail.CreatedBy = ObjAttendanceMaster.CreatedBy;
						objdetail.CreatedOn = DateTime.Now;
						_context.AttendanceDetails.Add(objdetail);
						await _context.SaveChangesAsync();

						dbContextTransaction.Commit();
						return ObjAttendanceMaster;

				
					
				}
					

				}
				catch (Exception ex)
				{
					dbContextTransaction.Rollback();
					throw;

				}
			}

		}




		// post Hr Mark Attendence pending

		public async Task<AttendanceMaster> PostMarkAttendenceInBulk(AttendanceMaster ObjAttendanceMaster, HrhubContext _context)
		{

			var companyobj = _context.Companies.FirstOrDefault(x => x.CompanyId == ObjAttendanceMaster.CompanyID);




			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{

				try
				{
					string msg = "";
					var checkAttendenceInfo = await _context.AttendanceMasters.FirstOrDefaultAsync(x => x.AttendanceDate == ObjAttendanceMaster.AttendanceDate
					&& x.StaffId == ObjAttendanceMaster.StaffId && x.IsDeleted == false);
					if (checkAttendenceInfo != null && checkAttendenceInfo.AttendanceId > 0)
					{
						checkAttendenceInfo.UpdatedOn = DateTime.Now;
						checkAttendenceInfo.UpdatedBy = ObjAttendanceMaster.CreatedBy;
						checkAttendenceInfo.LastPunchOut = ObjAttendanceMaster.LastPunchOut;
						checkAttendenceInfo.TotalWorkingMinutes = calculteHourstoMinute(ObjAttendanceMaster.LastPunchOut, checkAttendenceInfo.FirstPunchIn);

						await _context.SaveChangesAsync();



						var detailAttendance = _context.AttendanceDetails.Where(x => x.AttendanceId == checkAttendenceInfo.AttendanceId).OrderByDescending(x => x.AttendanceDetailId).FirstOrDefault();




						if (detailAttendance != null && detailAttendance.AttendanceDetailId > 0 && detailAttendance.TimeOut == null)
						{
							detailAttendance.TimeOut = checkAttendenceInfo.LastPunchOut;
							detailAttendance.UpdatedOn = DateTime.Now;
							detailAttendance.UpdatedBy = ObjAttendanceMaster.CreatedBy;

							detailAttendance.IsDeleted = false;
							detailAttendance.WorkingMinutes = calculteHourstoMinute(ObjAttendanceMaster.LastPunchOut, detailAttendance.TimeIn);

							await _context.SaveChangesAsync();


						}
						else
						{
							AttendanceDetail objdetail = new AttendanceDetail();
							objdetail.AttendanceId = checkAttendenceInfo.AttendanceId;
							objdetail.TimeIn = ObjAttendanceMaster.FirstPunchIn;
							objdetail.CreatedOn = DateTime.Now;
							objdetail.UpdatedOn = DateTime.Now;
							objdetail.UpdatedBy = ObjAttendanceMaster.CreatedBy;
							objdetail.CreatedBy = ObjAttendanceMaster.CreatedBy;
							objdetail.IsDeleted = false;
							detailAttendance.WorkingMinutes = calculteHourstoMinute(ObjAttendanceMaster.LastPunchOut, detailAttendance.TimeIn);
							_context.AttendanceDetails.Add(objdetail);
							await _context.SaveChangesAsync();


						}


						dbContextTransaction.Commit();
						return checkAttendenceInfo;



					}
					else
					{



						/// Intial Entry Attendence Master						
						ObjAttendanceMaster.CreatedOn = DateTime.Now;
						ObjAttendanceMaster.AttendanceStatusId = 1;
						ObjAttendanceMaster.LastPunchOut = null;

						ObjAttendanceMaster.IsDeleted = false;
						ObjAttendanceMaster.TotalDefinedMinutes = calculteHourstoMinute(companyobj.OfficeEndTime, companyobj.OfficeStartTime);
						_context.AttendanceMasters.Add(ObjAttendanceMaster);
						await _context.SaveChangesAsync();



						//detail insert

						AttendanceDetail objdetail = new AttendanceDetail();
						objdetail.AttendanceId = ObjAttendanceMaster.AttendanceId;
						objdetail.TimeIn = ObjAttendanceMaster.FirstPunchIn;
						objdetail.TimeOut = null;

						objdetail.IsDeleted = false;
						objdetail.CreatedBy = ObjAttendanceMaster.CreatedBy;
						objdetail.CreatedOn = DateTime.Now;
						_context.AttendanceDetails.Add(objdetail);
						await _context.SaveChangesAsync();

						dbContextTransaction.Commit();
						return ObjAttendanceMaster;

					}


				}
				catch (Exception ex)
				{
					dbContextTransaction.Rollback();
					throw;

				}
			}

		}

















		// Check In and Check Out button check Status update
		public async Task<AttendanceDetail>CheckStatus(int StaffId,HrhubContext _context)
		{
			try
			{
				if (StaffId > 0)
				{
					var currentDate = DateTime.Now.Date;

					var query = from at in _context.AttendanceDetails
								join M in _context.AttendanceMasters on at.AttendanceId equals M.AttendanceId
								where M.StaffId == StaffId && at.IsDeleted == false && M.IsDeleted == false && M.AttendanceDate == currentDate
								//orderby at.AttendanceId descending
								select new AttendanceDetail
								{
									AttendanceDetailId =at.AttendanceDetailId, 
									TimeIn = at.TimeIn,
									TimeOut = at.TimeOut
								};

					AttendanceDetail? resultlist=	query.OrderByDescending(x => x.AttendanceDetailId).FirstOrDefault();

					return await Task.FromResult(resultlist!=null? resultlist:new AttendanceDetail());

				}
				return null;
			}
			catch (Exception)
			{

				throw;
			}
		}

		// Load store procedure Get attendance Status sp_getattendanceStatus
		public async Task<List<AttendanceMaster>> GetattendanceStatusStaff(int StaffId)
		{
			
			try
			{
				string query = "EXEC dbo.sp_getattendanceStatus " + StaffId;
				DataTable dt = _db.ReturnDataTable(query);

				var StatusStaff = dt.AsEnumerable()

					.Select(row => new AttendanceMaster
					{
						TotalCount = Convert.ToInt32(row["countof"]),
						TitleStatus = row["TitleStatus"].ToString(),
						CssClass = row["CssClass"].ToString()
					})
					.ToList();
				return StatusStaff;
			}
			catch { throw; }
		}






	}
}
