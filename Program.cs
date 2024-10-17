using NewAvitoParser.CsvServices;
using NewAvitoParser.Coomon;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections;
using NewAvitoParser;
using NewAvitoParser.Enums;
using CategoryId = NewAvitoParser.Coomon.Constants.CategoryId;

namespace AvitoParser
{
	internal class Program
	{
		public static async Task NavigateAcyns(WebDriver driver, string url)
		{
			await Task.Run(() =>

			{
				driver.Navigate().GoToUrl(url);
			}
			);
		}

		public static IEnumerable Conection(WebDriver driver, string url)
		{
			var task = Task.Run(() => NavigateAcyns(driver, url));
			int i = 0;
			while (!task.IsCompleted)
			{
				Thread.Sleep(1000);
				i++;
				yield return i;
			}
		}

		public static void ConnectToUrl(WebDriver driver, string url)
		{
			foreach (var second in Conection(driver, url))
			{
				Console.WriteLine($"NavigateToUrl/{second}");
			}
		}
		
		static void Main(string[] args)
		{
			foreach (var elem in Enum.GetValues(typeof(CategoryId)))
			{
				string path1 = Constants.Files.PropertiesVaritant(Enum.GetName(typeof(CategoryId), elem));
				string path2 = Constants.Files.PropertyNamesVariant(Enum.GetName(typeof(CategoryId), elem));
				string path3 = Constants.Files.PropertyValuesVariant(Enum.GetName(typeof(CategoryId), elem));
				string path4 = Constants.Files.LinksVaritant(Enum.GetName(typeof(CategoryId), elem));

				if (!File.Exists(path1))
				{
					Directory.CreateDirectory($"..\\..\\{Enum.GetName(typeof(CategoryId), elem)}");
					File.Create(path1);
				}

				if (!File.Exists(path2))
				{
					Directory.CreateDirectory($"..\\..\\{Enum.GetName(typeof(CategoryId), elem)}");
					File.Create(path2);
				}

				if (!File.Exists(path3))
				{
					Directory.CreateDirectory($"..\\..\\{Enum.GetName(typeof(CategoryId), elem)}");
					File.Create(path3);
				}

				if (!File.Exists(path4))
				{
					Directory.CreateDirectory($"..\\..\\{Enum.GetName(typeof(CategoryId), elem)}");
					File.Create(path4);
				}
			}

			Console.WriteLine("Set parser mode/ Enter 0 if you want to Links/ Enter 1 if you want to parse attributes from links file");
			ParserMode mode = (ParserMode)int.Parse(Console.ReadLine());
			CategoryId categoryId = default;

			
			Console.Write(
				"Aloowed Categories for parse:\n" +
				$"	0:{nameof(CategoryId.Electronis)}\n" +
				$"	1:{nameof(CategoryId.RealEstate)}\n"
			);

			Console.WriteLine("Enter SectionId which u want to parse:");
			categoryId = (CategoryId)int.Parse(Console.ReadLine());
			
			ChromeOptions options = new ChromeOptions();
			options.AddArgument("headless");
			options.AddArgument("disable-gpu");
			options.AddArgument("no-sandbox");

			ChromeDriver driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
			driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(30));

			var urlList = new HashSet<string>();
			var properties = new HashSet<Property>();
			string currentUrl = string.Empty;


			using (driver)
			{
				try
				{	
					if(mode == ParserMode.Links)
					{
						var links = new List<string>();
						foreach (var url in Constants.CategoriesList[(int)categoryId])
						{
							try
							{
								for (int i = 0; i < 100; i++)
								{
									ConnectToUrl(driver, url + $"?p={i}");
									currentUrl = url + $"?p={i}";

									Console.WriteLine($"LINK{i}:{currentUrl}");
									var hrefs = driver.FindElements(By.XPath("//*[@id]/div/div/div[2]/div[2]/div/a"))
									.Select(item => item.GetAttribute("href")).ToArray();

									links.AddRange(hrefs);
								}
							}
							catch(Exception ex)
							{
								continue;
							}
						}

						File.WriteAllLines(path: Constants.Files.Links, contents: links);

					}

					mode = ParserMode.Attributes;

					
					if (mode == ParserMode.Attributes)
					{
						var links = File.ReadAllLines(Constants.Files.LinksVaritant(Enum.GetName(typeof(CategoryId), categoryId)));
						var propertiesList = new List<Property>();

						foreach (var url in links)
						{
							ConnectToUrl(driver, url);
							currentUrl = url;
							Console.WriteLine($"PRODUCT_LINK:{currentUrl}");
							var attributes = driver.FindElements(By.ClassName("params-paramsList__item-_2Y2O"));

							if (attributes != null)
							{
								foreach (var elem in attributes)
								{
									string[] nameValuePare;

									if (elem != null)
									{
										if (!elem.Text.Contains(':'))
										{
											var text = elem.Text.Substring(1);
											var upperCase = text.Where(char.IsUpper).First();
											var index = text.IndexOf(upperCase);
											text = elem.Text.Insert(index + 1, " ");
											nameValuePare = text.Split(" ");
										}
										else
										{
											nameValuePare = elem.Text.Split(':');
										}

										var property = new Property
										{
											Name = nameValuePare[0].Trim(' '),
											Value = nameValuePare[1].Trim(' ')
										};

										propertiesList.AddRange(AvitoParamsConverter.ParamDisoposer(property));
									}
								}
							}
						}

						properties = new HashSet<Property>(propertiesList);
					}
				}
				catch (Exception ex)
				{
					string log = 
						"ERROR:\n{\n" 
						+ $"Message: {ex.Message}\n" 
						+ $"StackTrace: {ex.StackTrace}\n"
						+ $"LastUrl: {currentUrl}\n"
						+ "\n}\n";

					Console.WriteLine(log);
					HelperCsv.WriteFile(Constants.Files.PropertiesVaritant(Enum.GetName(typeof(CategoryId), categoryId)) , properties);
					File.AppendAllText(Constants.Files.LogFile, log);
					Console.WriteLine(ex.Message);
					return;
				}
				finally
				{
					HelperCsv.WriteFile(Constants.Files.Properties, properties);
					driver.Quit();
				}
			}
		}
	}
}

