using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;


namespace HRHUBAPI.Models
{



    public partial class LoanApplicationProcess
    {
        [NotMapped]
        public string? ApprovedByStaffSnap { get; set; }
        [NotMapped]
        public string? ApprovedByStaffName { get; set; }
        [NotMapped]
        public string? ApprovedByDesignation { get; set; }
        [NotMapped]
        public string? LoanProcessDate { get; set; }
        [NotMapped]
        public DateTime? PaymentDate { get; set; }

        public async Task<List<LoanApplicationProcess>> GetLoanAprrovalDetail(int LoanId, HrhubContext _context)
        {
            try
            {

                var list = await (from la in _context.LoanApplicationProcesses
                                  join sa in _context.Staff on la.ApprovedByStaffId equals sa.StaffId
                                  join d in _context.Designations on sa.DesignationId equals d.DesignationId

                                  where la.LoanApplicationId == LoanId
                                  select new LoanApplicationProcess()
                                  {
                                      LoanApplicationId = la.LoanApplicationId,
                                      ApprovedByStaffId = la.ApprovedByStaffId,
                                      ApprovedByStaffName = sa.FirstName,
                                      LoanProcessDate = la.ProcessDate == null ? "DD-MMM-YYYY" : Convert.ToDateTime(la.ProcessDate).ToString("dd-MMM-yyyy"),

                                      ApprovedByStaffSnap = string.IsNullOrWhiteSpace(sa.SnapPath) ? "/Images/StaffImageEmpty.jpg" : sa.SnapPath.ToString(),
                                      ApprovedByDesignation = d.Title,
                                      Remarks = la.Remarks,
                                      LoanStatusId = la.LoanStatusId

                                  }).ToListAsync();

                return list;



            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<LoanApplication> PostLoanApprovalProcess(LoanApplication LoanInfo, HrhubContext _context)
        {
            // using (var dbContextTransaction = _context.Database.BeginTransaction())
            //{
            try
            {
                string msg = "";

                List<LoanApplicationProcess> lsobjAca = new List<LoanApplicationProcess>();

                int a = 0;
                if (LoanInfo.ForwardToStaffID != null)
                {

                    foreach (var item in LoanInfo.ForwardToStaffID)
                    {

                        LoanApplicationProcess objAca = new LoanApplicationProcess();


                        objAca.LoanApplicationId = LoanInfo.LoanApplicationId;
                        objAca.ApprovedByStaffId = LoanInfo.ForwardToStaffID.ToArray()[a];
                        objAca.IsDelete = false;
                        //objAca.LoanStatusId = 2; // Pending;
                        lsobjAca.Add(objAca);


                        a++;

                    }


                    _context.LoanApplicationProcesses.AddRange(lsobjAca);
                    await _context.SaveChangesAsync();
                }
                return LoanInfo;

                //var checkLeaveInfo = await _context.LoanApplications.FirstOrDefaultAsync(x => x.LoanApplicationId == LoanInfo.LoanApplicationId && x.IsDeleted == false);
                //if (checkLeaveInfo != null && checkLeaveInfo.LoanApplicationId > 0)
                //{
                //    checkLeaveInfo.LoanStatusId = 2; // Pending

                //    await _context.SaveChangesAsync();

                //}
                //else
                //{
                //     LoanInfo.CreatedOn = DateTime.Now;
                //    _context.LoanApplications.Add(LoanInfo);
                //}
                //await _context.SaveChangesAsync();
                //dbContextTransaction.Commit();
                //return checkLeaveInfo;


            }
            catch (Exception ex)
            {

                throw;

            }
            // }
        }

        public async Task<LoanApplicationProcess> SaveLoanApprovalDetail(LoanApplicationProcess obj, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var Info = await hrhubContext.LoanApplications.FirstOrDefaultAsync(x => x.LoanApplicationId == obj.LoanApplicationId && x.IsDeleted == false);
                    if (Info != null && Info.LoanApplicationId > 0)
                    {
                        Info.PaymentDate = obj.PaymentDate;
                        Info.LoanStatusId = obj.LoanStatusId;
                        Info.UpdatedBy = obj.ApprovedByStaffId;
                        Info.UpdatedOn = DateTime.Now;
                        await hrhubContext.SaveChangesAsync();
                    }

                    var dbResult = await hrhubContext.LoanApplicationProcesses.FirstOrDefaultAsync(x => x.LoanApplicationId == obj.LoanApplicationId && x.ApprovedByStaffId == obj.ApprovedByStaffId);
                    if (dbResult != null)
                    {
                        dbResult.LoanApplicationId = obj.LoanApplicationId;
                        dbResult.ApprovedByStaffId = obj.ApprovedByStaffId;
                        dbResult.ProcessDate = obj.ProcessDate;
                        dbResult.LoanStatusId = obj.LoanStatusId;
                        dbResult.Remarks = obj.Remarks;

                        await hrhubContext.SaveChangesAsync();
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {

                        hrhubContext.LoanApplicationProcesses.Add(obj);
                        await hrhubContext.SaveChangesAsync();
                        dbContextTransaction.Commit();
                        return obj;
                    }

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback(); throw;
                }

            }
        }




    }
}
