using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using HRHUBAPI.Models.Configuration;
using System.Data;

namespace HRHUBAPI.Models
{



    public partial class LoanType
    {
        [NotMapped]
        public IEnumerable<LoanType>? ListLoanTypes { get; set; }
        [NotMapped]
        public decimal? TotalLoanAmount { get; set; }
        [NotMapped]
        public decimal? LoanPaidAmount { get; set; }
        [NotMapped]
        public decimal? RemainingLoanAmount { get; set; }

        public async Task<List<LoanType>> GetLoanType(int CompanyId, HrhubContext _context)
        {
            try
            {
                List<LoanType> list = new List<LoanType>();
                list = await _context.LoanTypes.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId).OrderByDescending(x=> x.LoanTypeId).ToListAsync();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }
      

        public async Task<LoanType> GetLoanTypeById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.LoanTypes.FirstOrDefaultAsync(x => x.LoanTypeId == id && x.IsDeleted == false);
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
        public async Task<LoanType> UpdateStatusByLoanTypeId(LoanType ObjLoanTypeInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkLoanTypeInfo = await _context.LoanTypes.FirstOrDefaultAsync(x => x.LoanTypeId == ObjLoanTypeInfo.LoanTypeId && x.IsDeleted == false);
                if (checkLoanTypeInfo != null && checkLoanTypeInfo.LoanTypeId > 0)
                {
                    checkLoanTypeInfo.LoanTypeId = ObjLoanTypeInfo.LoanTypeId;
                    checkLoanTypeInfo.UpdatedOn = DateTime.Now;
                    checkLoanTypeInfo.Status = ObjLoanTypeInfo.Status;
                    checkLoanTypeInfo.IsNeedApproval = ObjLoanTypeInfo.IsNeedApproval;
                    checkLoanTypeInfo.UpdatedBy = ObjLoanTypeInfo.UpdatedBy;

                    await _context.SaveChangesAsync();
                    return ObjLoanTypeInfo;

                }
                return ObjLoanTypeInfo;
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<LoanType> PostLoanType(LoanType ObjLoanTypeInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkLoanTypeInfo = await _context.LoanTypes.FirstOrDefaultAsync(x => x.LoanTypeId == ObjLoanTypeInfo.LoanTypeId && x.IsDeleted == false);
                if (checkLoanTypeInfo != null && checkLoanTypeInfo.LoanTypeId > 0)
                {
                    checkLoanTypeInfo.LoanTypeId = ObjLoanTypeInfo.LoanTypeId;
                    checkLoanTypeInfo.Title = ObjLoanTypeInfo.Title;
                    checkLoanTypeInfo.Status = ObjLoanTypeInfo.Status;
                    checkLoanTypeInfo.IsNeedApproval = ObjLoanTypeInfo.IsNeedApproval;
                    checkLoanTypeInfo.UpdatedOn = DateTime.Now;
                    checkLoanTypeInfo.UpdatedBy = ObjLoanTypeInfo.CreatedBy;
                    checkLoanTypeInfo.CompanyId = ObjLoanTypeInfo.CompanyId;
                    await _context.SaveChangesAsync();

                }
                else
                {
                    ObjLoanTypeInfo.CreatedOn = DateTime.Now;
                    ObjLoanTypeInfo.IsDeleted = false;
                    _context.LoanTypes.Add(ObjLoanTypeInfo);
                }
                await _context.SaveChangesAsync();

                return checkLoanTypeInfo;


            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<bool> DeleteLoanTypeInfo(int id, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var LoanTypeInfo = await _context.LoanTypes.FirstOrDefaultAsync(x => x.LoanTypeId == id && x.IsDeleted == false);

                if (LoanTypeInfo != null)
                {
                    LoanTypeInfo.IsDeleted = true;
                    LoanTypeInfo.UpdatedOn = DateTime.Now;
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


        public async Task<bool> AlreadyExist(int LoanTypeInfoId, string title, int CompanyId, HrhubContext _context)
        {
            try
            {
                if (LoanTypeInfoId > 0)
                {
                    var result = await _context.LoanTypes.FirstOrDefaultAsync(x => x.Title == title && x.CompanyId == CompanyId && x.LoanTypeId != LoanTypeInfoId && x.IsDeleted == false);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.LoanTypes.FirstOrDefaultAsync(x => x.Title == title && x.CompanyId == CompanyId && x.IsDeleted == false);
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
