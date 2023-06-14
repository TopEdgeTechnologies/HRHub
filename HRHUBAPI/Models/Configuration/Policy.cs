using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace HRHUBAPI.Models
{
    public partial class Policy
    {


        [NotMapped]
        public int? Flag { get; set; } 

        [NotMapped]
        public IEnumerable<Policy>? ListPolicy { get; set; }
        public async Task<List<Policy>> GetPolicy(HrhubContext _context)
        {
            try
            {

                var list = await _context.Policies.Where(x=>x.Status==true).ToListAsync();

                return list;
              
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<Policy> GetPolicyById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.Policies.FirstOrDefaultAsync(x => x.PolicyId == id  && x.Status==true);
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


    //    public async Task<Policy> PostPolicy(Policy ObjPolicyInfo, HrhubContext _context)
    //    {
    //        try
    //        {
    //            string msg = "";
    //            var checkPolicyInfo = await _context.Policys.FirstOrDefaultAsync(x => x.PolicyId == ObjPolicyInfo.PolicyId && x.IsDeleted==false);
    //            if (checkPolicyInfo != null && checkPolicyInfo.PolicyId > 0)
    //            {
    //                checkPolicyInfo.PolicyId = ObjPolicyInfo.PolicyId;
    //                checkPolicyInfo.Title = ObjPolicyInfo.Title;
    //                checkPolicyInfo.UpdatedOn = DateTime.Now;
    //                checkPolicyInfo.Status = ObjPolicyInfo.Status;
    //                checkPolicyInfo.UpdatedBy = ObjPolicyInfo.CreatedBy;
                   
				//	  await _context.SaveChangesAsync();
				//	checkPolicyInfo.Flag = 2;
				//	return ObjPolicyInfo;

				//}
    //            else
    //            {
    //                ObjPolicyInfo.CreatedOn = DateTime.Now;
    //                ObjPolicyInfo.IsDeleted=false;
    //                _context.Policys.Add(ObjPolicyInfo);
				//	await _context.SaveChangesAsync();

				//	ObjPolicyInfo.Flag = 1;
				//	return ObjPolicyInfo;

				//}

           


    //        }
    //        catch (Exception ex)
    //        {

    //            throw;

    //        }
    //    }


    //    public async Task<bool> DeletePolicyInfo(int id,int Userid, HrhubContext _context)
    //    {
    //        try
    //        {
    //            bool check = false;
    //            var PolicyInfo = await _context.Policys.FirstOrDefaultAsync(x => x.PolicyId == id  && x.IsDeleted == false);

    //            if (PolicyInfo != null)
    //            {
    //                PolicyInfo.IsDeleted= true;   
    //                PolicyInfo.UpdatedOn= DateTime.Now;
    //                PolicyInfo.UpdatedBy = Userid;
    //                check = true;

    //            }
    //            await _context.SaveChangesAsync();
    //            return check;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }


    //    }


    //    public async Task<bool> AlreadyExist(int PolicyInfoId, string title,int CompanyId, HrhubContext _context)
    //    {
    //        try
    //        {

    //            if (PolicyInfoId > 0)
    //            {
    //                var result = await _context.Policys.FirstOrDefaultAsync(x => x.Title == title && x.CompanyId==CompanyId && x.PolicyId != PolicyInfoId && x.IsDeleted==false);
    //                if (result != null)
    //                {
    //                    return true;
    //                }


    //            }
    //            else
    //            {
    //                var result = await _context.Policys.FirstOrDefaultAsync(x => x.Title == title && x.CompanyId==CompanyId && x.IsDeleted == false);
    //                if (result != null)
    //                {
    //                    return true;
    //                }

    //            }

    //            return false;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }


        


    }
}
