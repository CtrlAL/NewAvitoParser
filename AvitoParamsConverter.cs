using NewAvitoParser.CsvServices;
namespace NewAvitoParser
{
	public static class AvitoParamsConverter
	{
		static public List<Property> ParamDisoposer(Property property)
		{
			var result = new List<Property>();
			if (NeedDispose(property))
			{
				foreach (var val in property.Value.Split(','))
				{
					result.Add(new Property
					{
						SubCategoryName = property.SubCategoryName,
						Name = property.Name,
						Value = val,
					});
				}
				return result;
			}
			else
			{
				result.Add(property);
				return result;
			}
		}

		private static bool NeedDispose(Property property)
		{
			var value = property.Value;
			if (value.Split(',').Count() > 1)
			{
				return true;
			}
			return false;
		}
	}
}
