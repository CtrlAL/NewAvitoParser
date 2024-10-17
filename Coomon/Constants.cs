namespace NewAvitoParser.Coomon
{
	public static class Constants
	{
		public static readonly string BaseUrl = "https://ru.wikipedia.org/wiki/%D0%97%D0%B0%D0%B3%D0%BB%D0%B0%D0%B2%D0%BD%D0%B0%D1%8F_%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%B8%D1%86%D0%B0";

		public static readonly string[] ElectronicSectionsList = 
		{
			"https://www.avito.ru/all/telefony",
			"https://www.avito.ru/all/audio_i_video",
			"https://www.avito.ru/all/tovary_dlya_kompyutera",
			"https://www.avito.ru/all/igry_pristavki_i_programmy",
			"https://www.avito.ru/all/noutbuki",
			"https://www.avito.ru/all/nastolnye_kompyutery",
			"https://www.avito.ru/all/fototehnika",
			"https://www.avito.ru/all/planshety_i_elektronnye_knigi",
			"https://www.avito.ru/all/orgtehnika_i_rashodniki",
		};

		public static readonly string[] RealEstateList =
		{
			"https://www.avito.ru/all/kvartiry/prodam-ASgBAgICAUSSA8YQ",
			"https://www.avito.ru/all/kvartiry/prodam/vtorichka-ASgBAgICAkSSA8YQ5geMUg",
			"https://www.avito.ru/all/kvartiry/prodam/novostroyka-ASgBAgICAkSSA8YQ5geOUg?context=",
			"https://www.avito.ru/all/doma_dachi_kottedzhi/prodam-ASgBAgICAUSUA9AQ?context=",
			"https://www.avito.ru/all/komnaty/prodam-ASgBAgICAUSQA7wQ?context=",
			"https://www.avito.ru/all/kvartiry/sdam/posutochno/-ASgBAgICAkSSA8gQ8AeSUg?context=",
			"https://www.avito.ru/all/doma_dachi_kottedzhi/sdam/posutochno-ASgBAgICAkSUA9IQoAjKVQ?context=",
			"https://www.avito.ru/all/komnaty/sdam/posutochno/-ASgBAgICAkSQA74QqAn4YA?context=",
			"https://www.avito.ru/all/realty_rent/hotels-ASgBAgICAUSksxTkoY8D?f=ASgBAgICAkSksxTkoY8DyP4UAg",
			"https://www.avito.ru/all/kvartiry/sdam/na_dlitelnyy_srok-ASgBAgICAkSSA8gQ8AeQUg?context=",
			"https://www.avito.ru/all/doma_dachi_kottedzhi/sdam/na_dlitelnyy_srok-ASgBAgICAkSUA9IQoAjIVQ?context=",
			"https://www.avito.ru/all/komnaty/sdam/na_dlitelnyy_srok-ASgBAgICAkSQA74QqAn2YA?context=",
			"https://www.avito.ru/all/kommercheskaya_nedvizhimost/prodam-ASgBAgICAUSwCNJW",
			"https://www.avito.ru/all/kommercheskaya_nedvizhimost/sdam-ASgBAgICAUSwCNRW",
			"https://www.avito.ru/all/zemelnye_uchastki",
			"https://www.avito.ru/all/garazhi_i_mashinomesta",
			"https://www.avito.ru/all/nedvizhimost_za_rubezhom",
		};

		public enum CategoryId
		{
			Electronis = 0,
			RealEstate = 1,
		}

		public static List<string[]> CategoriesList = new List<string[]>
		{
			ElectronicSectionsList,
			RealEstateList,
		};

		public static class Files
		{
#if OS_WINDOWS
			public static string Properties = "..\\..\\Properties.csv";
			public static string PropertyNames = "..\\..\\PropertyNames.csv";
			public static string PropertyValues = "..\\..\\PropertyValues.csv";
			public static string Links = "..\\..\\Links.csv";
			public static string LogFile = "..\\..\\LogFile.csv";
			public static string LinksVaritant(string variant) => $"..\\..\\Links{variant}.csv";
			public static string PropertiesVaritant(string varitant) => $"..\\..\\Properties{varitant}.csv";
			public static string PropertyNamesVariant(string varitant) => $"..\\..\\PropertyNames{varitant}.csv";
			public static string PropertyValuesVariant(string varitant) => $"..\\..\\PropertyValues{varitant}.csv";
#else
			public static string Properties = "bin/LINUX_Properties.csv";
			public static string PropertyNames = "bin/LINUX_PropertyNames.csv";
			public static string PropertyValues = "bin/LINUX_PropertyValues.csv";
			public static string Links = "bin/Links.csv";
			public static string LogFile = "bin/LogFile.csv";
			public static string LinksVaritant(string variant) => $"bin/LINUX_Links{variant}.csv";
			public static string PropertiesVaritant(string varitant) => $"bin/LINUX_Properties{varitant}.csv";
			public static string PropertyNamesVariant(string varitant) => $"bin/LINUX_PropertyNames{varitant}.csv";
			public static string PropertyValuesVariant(string varitant) => $"bin/LINUX_PropertyValues{varitant}.csv";
#endif
		};
	}
}
