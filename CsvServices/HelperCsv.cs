using CsvHelper;
using System.Globalization;

namespace NewAvitoParser.CsvServices
{
	public static class HelperCsv
	{
		public static void WriteFile<T>(string path, ICollection<T> content)
		{
			using (var writer = new StreamWriter(path))
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.WriteRecords(content);
			}
		}

		public static List<T> ReadFile<T>(string path)
		{
			var content = new List<T>();
			using (var reader = new StreamReader(path))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				return csv.GetRecords<T>().ToList();
			}
		}
	}
}
