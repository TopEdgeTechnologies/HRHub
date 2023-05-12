namespace HRHUBWEB.Models
{
    public class GetUserRightByUser
    {

        public bool? type { get; set; }
        public int Userid { get; set; }
        public string? UserName { get; set; }
        public string? PhotoPath { get; set; }
        public string? UserPassword { get; set; }
        public Nullable<int> GroupID { get; set; }
        public bool Active { get; set; }
        public string? GroupTitle { get; set; }
        public string? Description { get; set; }
        public bool? Inactive { get; set; }
        public Nullable<bool> Assign { get; set; }
        public Nullable<bool> IsEdit { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsPrint { get; set; }
        public Nullable<bool> Isnew { get; set; }
        public int Formid { get; set; }
        public string? FormTitle { get; set; }
        public string? controller { get; set; }
        public string? action { get; set; }
        public string? imageClass { get; set; }
        public Nullable<bool> status { get; set; }
        public Nullable<bool> isParent { get; set; }
        public Nullable<int> parentId { get; set; }
        public Nullable<bool> IsMenu { get; set; }
        public int? ReferenceID { get; set; }
        public int? titlewithcounter { get; set; }





    }
}
