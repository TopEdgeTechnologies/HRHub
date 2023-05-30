using System.Data;
using System.Dynamic;

namespace HRHUBAPI.Extensions
{

	public static class DataTableExtensions
	{
		//public static dynamic[] ToDynamicList(this DataTable dt)
		//{
		//	var dynamicList = new dynamic[dt.Rows.Count];

		//	for (int i = 0; i < dt.Rows.Count; i++)
		//	{
		//		dynamic obj = new ExpandoObject();
		//		var row = dt.Rows[i];

		//		foreach (DataColumn column in dt.Columns)
		//		{
		//			((IDictionary<string, object>)obj)[column.ColumnName] = row[column];
		//		}

		//		dynamicList[i] = obj;
		//	}

		//	return dynamicList;
		//}


		public static List<dynamic> ToDynamicList(this DataTable dt)
		{
			List<dynamic> list = new List<dynamic>();

			foreach (DataRow row in dt.Rows)
			{
				dynamic obj = new System.Dynamic.ExpandoObject();

				foreach (DataColumn column in dt.Columns)
				{
					string propertyName = column.ColumnName;
					object value = row[column];

					((IDictionary<string, object>)obj)[propertyName] = value;
				}

				list.Add(obj);
			}

			return list;
		}


		/// <summary>
		/// List<Person> personList = dt.ToList<Person>();
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dt"></param>
		/// <returns></returns>

		public static List<T> ModelWithToList<T>(this DataTable dt) where T : class, new()
		{
			List<T> list = new List<T>();

			foreach (DataRow row in dt.Rows)
			{
				T obj = new T();

				foreach (DataColumn column in dt.Columns)
				{
					string propertyName = column.ColumnName;
					object value = row[column];

					var property = typeof(T).GetProperty(propertyName);
					if (property != null && value != DBNull.Value)
					{
						property.SetValue(obj, Convert.ChangeType(value, property.PropertyType));
					}
				}

				list.Add(obj);
			}

			return list;

		}

	}

}