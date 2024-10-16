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

		public static class Files
		{
			public static string Properties = "..\\..\\Properties.csv";
			public static string PropertyNames = "..\\..\\PropertyNames.csv";
			public static string PropertyValues = "..\\..\\PropertyValues.csv";
			public static string Links = "..\\..\\Links.csv";
			public static string LogFile = "..\\..\\LogFile.csv";
		};
	}
}
