namespace NewAvitoParser.Coomon
{
	public static class Avito
	{
		public struct SubCategory
		{
			public int CategoryId;
			public string Name;
			public string Link;
		}
		public struct Category
		{
			public string Name;
			public string Link;
		}
		public static Dictionary<int, Category> CategoryList = new Dictionary<int, Category>();
		public static List<SubCategory> SubCategoryList = new List<SubCategory>();
		public static Dictionary<int, string> CategoryId = new Dictionary<int, string>();
		public static string NameById(int id) => CategoryId[id];
	}
}
