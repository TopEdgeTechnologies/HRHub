using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class WeekendRule
    {
        [NotMapped]
        public int TransFlag { get; set; }


        public async Task<List<WeekendRule>> GetWeekendRules(int CompanyID, HrhubContext _context)
        {
            try
            {
                List<WeekendRule> objList = new List<WeekendRule>();

                objList = await _context.WeekendRules.Where(x => x.CompanyId == CompanyID && x.IsDeleted == false).ToListAsync();
                return objList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        public async Task<WeekendRule> GetWeekendRuleByID(int ID, HrhubContext _context)
        {
            try
            {
                var result = await _context.WeekendRules.FirstOrDefaultAsync(x => x.WeekendRuleId == ID && x.IsDeleted == false);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception Ex)
            {

                throw;
            }
        }

        public async Task<WeekendRule> UpdateStatusByWeekendRuleId(WeekendRule ObjWeekendRuleInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkWeekendRuleInfo = await _context.WeekendRules.FirstOrDefaultAsync(x => x.WeekendRuleId == ObjWeekendRuleInfo.WeekendRuleId && x.IsDeleted == false);
                if (checkWeekendRuleInfo != null && checkWeekendRuleInfo.WeekendRuleId > 0)
                {
                    checkWeekendRuleInfo.UpdatedOn = DateTime.Now;
                    checkWeekendRuleInfo.Status = ObjWeekendRuleInfo.Status;
                    checkWeekendRuleInfo.UpdatedBy = ObjWeekendRuleInfo.UpdatedBy;

                    await _context.SaveChangesAsync();

                }
                return ObjWeekendRuleInfo;
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<WeekendRule> PostWeekendRule(WeekendRule objRule, HrhubContext _context)
        {

            var objPreviousRecord = await _context.WeekendRules.FirstOrDefaultAsync(x => x.WeekendRuleId == objRule.WeekendRuleId && x.IsDeleted == false);
            if (objPreviousRecord != null && objPreviousRecord.WeekendRuleId > 0)
            {
                objPreviousRecord.WeekendRuleId = objRule.WeekendRuleId;
                objPreviousRecord.DayName = objRule.DayName;
                objPreviousRecord.CompanyId = objRule.CompanyId;
                objPreviousRecord.Status = objRule.Status;
                objPreviousRecord.UpdatedBy = objRule.UpdatedBy;
                objPreviousRecord.UpdatedOn = DateTime.Now;
                objPreviousRecord.TransFlag = 2;
                return objPreviousRecord;

            }
            else
            {
                objRule.CreatedOn = DateTime.Now;
                objRule.TransFlag = 1;
                objRule.IsDeleted = false;

                _context.WeekendRules.Add(objRule);
                await _context.SaveChangesAsync();
                return objRule;
            }


        }

        public async Task<bool> DeleteWeekendRuleInfo(int id,int UserId, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var WeekendRuleInfo = await _context.WeekendRules.FirstOrDefaultAsync(x => x.WeekendRuleId == id && x.IsDeleted == false);

                if (WeekendRuleInfo != null)
                {
                    WeekendRuleInfo.IsDeleted = true;
                    WeekendRuleInfo.UpdatedBy = UserId;
                    WeekendRuleInfo.UpdatedOn = DateTime.Now;
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

    }
}
