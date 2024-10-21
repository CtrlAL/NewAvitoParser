using NewAvitoParser.CsvServices;

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
		
		public static void WriteToFile()
		{
			var categories = new List<CategoryMapper>();
			foreach (var category in CategoryList)
			{
				categories.Add(new CategoryMapper
				{
					Id = category.Key,
					Name = category.Value.Name,
					Link = category.Value.Link,
				});
			}
#if OS_WINDOWS
			HelperCsv.WriteFile("..\\..\\Categories.csv", categories);
			HelperCsv.WriteFile("..\\..\\SubCategories.csv", SubCategoryList);
#else
			HelperCsv.WriteFile("bin/Categories.csv", categories);
			HelperCsv.WriteFile("bin/SubCategories.csv", categories);
#endif
		}

		public static void FromFile()
		{

		}
	}
}
