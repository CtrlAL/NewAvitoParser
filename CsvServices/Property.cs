namespace NewAvitoParser.CsvServices
{
	public class Property
	{
		public string Name { get; set; }
		public string SubCategoryName { get; set; }
		public string Value { get; set; }
		public override bool Equals(object obj)
		{
			var item = obj as Property;

			return item != null &&
			   Name == item.Name &&
			   Value == item.Value;
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(Name, Value);
		}
	}
}
