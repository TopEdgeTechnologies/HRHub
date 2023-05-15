using Microsoft.EntityFrameworkCore;

namespace HRHUBAPI.Models
{
	public partial class ExceptionLog
	{


		public async Task<bool> PostExecptiontion(ExceptionLog exceptionLogObj, HrhubContext _context)
		{
			try
			{
					exceptionLogObj.ExceptionDateTime = DateTime.Now;
					_context.ExceptionLogs.Add(exceptionLogObj);
					await _context.SaveChangesAsync();
					return true;

			}
			catch (Exception ex)
			{

				throw;

			}
		}



	}
}
