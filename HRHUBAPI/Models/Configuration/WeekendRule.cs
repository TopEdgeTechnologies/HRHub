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



    }
}
