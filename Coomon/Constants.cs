namespace NewAvitoParser.Coomon
{
	public static class Constants
	{
		public static class Files
		{
#if OS_WINDOWS
			public static string Properties = "..\\..\\Properties.csv";
			public static string PropertyNames = "..\\..\\PropertyNames.csv";
			public static string PropertyValues = "..\\..\\PropertyValues.csv";
			public static string Links = "..\\..\\Links.csv";
			public static string LogFile = "..\\..\\LogFile.csv";
			public static string LinksVaritant(string variant) => $"..\\..\\{variant}\\Links{variant}.csv";
			public static string PropertiesVaritant(string variant) => $"..\\..\\{variant}\\Properties{variant}.csv";
			public static string PropertyNamesVariant(string variant) => $"..\\..\\{variant}\\PropertyNames{variant}.csv";
			public static string PropertyValuesVariant(string variant) => $"..\\..\\{variant}\\PropertyValues{variant}.csv";
#else
			public static string Properties = "bin/LINUX_Properties.csv";
			public static string PropertyNames = "bin/LINUX_PropertyNames.csv";
			public static string PropertyValues = "bin/LINUX_PropertyValues.csv";
			public static string Links = "bin/Links.csv";
			public static string LogFile = "bin/LogFile.csv";
			public static string LinksVaritant(string variant) => $"bin/{variant}/LINUX_Links{variant}.csv";
			public static string PropertiesVaritant(string variant) => $"bin/{variant}/LINUX_Properties{variant}.csv";
			public static string PropertyNamesVariant(string variant) => $"bin/{variant}/LINUX_PropertyNames{variant}.csv";
			public static string PropertyValuesVariant(string variant) => $"bin/{variant}/LINUX_PropertyValues{variant}.csv";
#endif
		};
	}
}
