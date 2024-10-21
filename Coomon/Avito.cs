using NewAvitoParser.CsvServices;

namespace NewAvitoParser.Coomon
{
	public static class Avito
	{
		public struct SubCategory
		{
			public int CategoryId { get; set; }
			public string Name { get; set; }
			public string Link { get; set; }
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
			var categories = HelperCsv.ReadFile<CategoryMapper>("..\\..\\Categories.csv");

			foreach (var category in categories)
			{
				CategoryList.Add(category.Id, new Category { Link = category.Link, Name = category.Name});
			}

			SubCategoryList = HelperCsv.ReadFile<SubCategory>("..\\..\\SubCategories.csv");
		}
	}
}
