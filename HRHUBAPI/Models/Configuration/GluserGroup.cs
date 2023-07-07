using HRHUBAPI.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace HRHUBAPI.Models
{
    public partial class GluserGroup
    {
       

       

        [NotMapped]
        public IEnumerable<string>? ListForm { get; set; }
        [NotMapped]
        public IEnumerable<string>? ListAssign { get; set; }
        [NotMapped]
        public IEnumerable<string>? ListIsEdit { get; set; }
        [NotMapped]
        public IEnumerable<string>? ListIsDelete { get; set; }
        [NotMapped]
        public IEnumerable<string>? ListIsPrint { get; set; }
        [NotMapped]
        public IEnumerable<string>? ListisNew { get; set; }


        [NotMapped]
		public IEnumerable<GluserGroupDetail>? ListGluserGroupDetail { get; set; }

		



		public async Task<List<GluserGroup>> GetGluserGroup(HrhubContext _context)
        {
            try
            {
                return await _context.GluserGroups.Where(x=>x.IsDeleted==false).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;

            }
        }



        // Load userforms details in Edit modes...
        public async Task<List<GluserGroupDetail>> GetGlUserByGroupId(int id, DbConnection _db)
        {
            try
            {

                List<GluserGroupDetail> GetGroupDetails = new List<GluserGroupDetail>();
              


                string query = "exec sp_getUserGroupDetail " + id;
                DataTable dt = _db.ReturnDataTable(query);

                GetGroupDetails = dt.AsEnumerable()
                .Select(row => new GluserGroupDetail
                {
                    UserGroupDetailId = Convert.ToInt32(row["UserGroupDetailId"]),
                    UserGroupId = Convert.ToInt32(row["UserGroupId"]),                            
                    FormId = Convert.ToInt32(row["Formid"]),
                    Assign= Convert.ToBoolean(row["Assign"]),
                    IsEdit = Convert.ToBoolean(row["IsEdit"]),
                    IsDelete = Convert.ToBoolean(row["IsDelete"]),
                    IsPrint = Convert.ToBoolean(row["IsPrint"]),
                    IsNew = Convert.ToBoolean(row["Isnew"]),
                    FormTitle = row["FormTitle"].ToString(),
					level = Convert.ToInt32(row["level"]),
					form_path = row["form_path"].ToString(),


				})
                .ToList();


                return GetGroupDetails;





            }
            catch (Exception ex)
            {

                throw;

            }
        }




        public async Task<GluserGroup> GetGluserGroupById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.GluserGroups.FirstOrDefaultAsync(x => x.GroupId == id && x.IsDeleted==false);
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


        public async Task<GluserGroup> PostGluserGroup(GluserGroup ObjGluserGroup, HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {


                try
                {


                    string msg = "";
                    var checkuserGroup = await _context.GluserGroups.FirstOrDefaultAsync(x => x.GroupId == ObjGluserGroup.GroupId && x.IsDeleted == false);
                    if (checkuserGroup != null && checkuserGroup.GroupId > 0)
                    {
                        checkuserGroup.GroupId = ObjGluserGroup.GroupId;
                        checkuserGroup.GroupTitle = ObjGluserGroup.GroupTitle;
                        checkuserGroup.Description = ObjGluserGroup.Description;
                        checkuserGroup.IsActive = ObjGluserGroup.IsActive;
                        checkuserGroup.UpdatedOn = DateTime.Now;
                        checkuserGroup.UpdatedBy = ObjGluserGroup.CreateBy;
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ObjGluserGroup.CreatedOn = DateTime.Now;
                        ObjGluserGroup.CreateBy = CreateBy;
                        ObjGluserGroup.IsDeleted = false;
                        _context.GluserGroups.Add(ObjGluserGroup);
                        await _context.SaveChangesAsync();

                    }



                    // ---------------------------------------------Save and update User Groups Details records



                    var acadresult = _context.GluserGroupDetails.Where(x => x.UserGroupId == ObjGluserGroup.GroupId).ToList();
                    if (  acadresult != null && acadresult.Count() > 0)
                    {

                        _context.RemoveRange(acadresult);
                        //foreach (var item in acadresult)
                        //{

                        //}

                        await _context.SaveChangesAsync();

                    }

                    List<GluserGroupDetail> lsobjAca = new List<GluserGroupDetail>();

                    int a = 0;
                    if (ObjGluserGroup.ListForm != null)
                    {

                        foreach (var item in ObjGluserGroup.ListForm)
                        {

                            GluserGroupDetail objAca = new GluserGroupDetail();

                            objAca.FormId = Convert.ToInt32(item);
                            objAca.Assign = Convert.ToBoolean(Convert.ToInt32( ObjGluserGroup.ListAssign.ToArray()[a]));
                            objAca.IsEdit = Convert.ToBoolean(Convert.ToInt32(ObjGluserGroup.ListIsEdit.ToArray()[a]));
                            objAca.IsDelete = Convert.ToBoolean(Convert.ToInt32(ObjGluserGroup.ListIsDelete.ToArray()[a]));
                            objAca.IsPrint = Convert.ToBoolean(Convert.ToInt32(ObjGluserGroup.ListIsPrint.ToArray()[a]));
                            objAca.IsNew = Convert.ToBoolean(Convert.ToInt32(ObjGluserGroup.ListisNew.ToArray()[a]));
                            
                           
                            objAca.UserGroupId = ObjGluserGroup.GroupId;
                            lsobjAca.Add(objAca);
                            a++;

                        }


                        _context.GluserGroupDetails.AddRange(lsobjAca);
                        await _context.SaveChangesAsync();
                    }


                    //--------------------------------------------------------------------------------


                    dbContextTransaction.Commit();
                    return ObjGluserGroup;


                }
                catch (Exception ex)
                {

                    dbContextTransaction.Rollback();

                    throw;

                }
               // return null;
            }
        }

        public async Task<bool> DeleteGluserGroup(int id, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var GluserGroup = await _context.GluserGroups.FirstOrDefaultAsync(x => x.GroupId == id && x.IsDeleted == false);

                if (GluserGroup != null)
                {
                    GluserGroup.IsDeleted= true;   
                    GluserGroup.UpdatedOn= DateTime.Now;
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


        public async Task<bool> AlreadyExist(int GluserGroupId, string GluserGroupname, HrhubContext _context)
        {
            try
            {
                if (GluserGroupId > 0)
                {
                    var result = await _context.GluserGroups.FirstOrDefaultAsync(x => x.GroupTitle.ToLower() == GluserGroupname.ToLower() && x.GroupId != GluserGroupId && x.IsDeleted==false);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.GluserGroups.FirstOrDefaultAsync(x => x.GroupTitle.ToLower() == GluserGroupname.ToLower() && x.IsDeleted == false);
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
