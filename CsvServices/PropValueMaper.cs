using System.Xml.Linq;

namespace NewAvitoParser.CsvServices
{
	public class PropValueMaper
	{
		public int Id { get; set; }
		public int PropertyId { get; set; }
		public string SubCategoryName { get; set; }
		public string Value { get; set; }

		public override bool Equals(object obj)
		{
			var item = obj as PropValueMaper;
			return item != null &&
			   Value == item.Value &&
			   SubCategoryName == item.SubCategoryName &&
			   PropertyId == item.PropertyId;
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(Value, SubCategoryName, PropertyId);
		}
	}
}
