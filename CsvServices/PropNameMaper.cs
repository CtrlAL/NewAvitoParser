namespace NewAvitoParser.CsvServices
{
	public class PropNameMaper
	{
		public int Id { get; set; }
		public string SubCategoryName { get; set; }
		public string Name { get; set; }

		public override bool Equals(object obj)
		{
			var item = obj as PropNameMaper;

			return item != null &&
			   Name == item.Name &&
			   SubCategoryName == item.SubCategoryName;
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(Name, SubCategoryName);
		}
	}
}
