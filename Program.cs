using NewAvitoParser.CsvServices;
using NewAvitoParser.Coomon;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections;
using NewAvitoParser;

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
			var chromeOptions = new ChromeOptions();
			ChromeOptions options = new ChromeOptions();
			options.AddArgument("headless");
			options.AddArgument("disable-gpu");
			options.AddArgument("--no-sandbox");
			//options.BinaryLocation = "/opt/google/chrome/google-chrome";
			//options.AddArgument("--proxy-server=http://20.206.106.192:8123");
			ChromeDriver driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
			
			driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(30));

			var urlList = new HashSet<string>();
			var properties = new HashSet<Property>();
			string currentUrl = string.Empty;

			using (driver)
			{
				try
				{
					foreach (var url in Constants.ElectronicSectionsList)
					{
						for (int i = 0; i < 100; i++)
						{
							ConnectToUrl(driver, url + $"?p={i}");
							currentUrl = url + $"?p={i}";

							Console.WriteLine($"LINK{i}:{currentUrl}");
							var links = driver.FindElements(By.XPath("//*[@id]/div/div/div[2]/div[2]/div/a"))
							.Select(item => item.GetAttribute("href"));

							File.WriteAllLines(path: Constants.Files.Links, contents: links.ToList());
						}
					}

					{
						var links = File.ReadAllLines(Constants.Files.Links).ToList();
						var propertiesList = new List<Property>();

						foreach (var url in links)
						{
							ConnectToUrl(driver, url);
							currentUrl = url;
							Console.WriteLine($"PRODUCT_LINK:{currentUrl}");
							var attributes = driver.FindElements(By.ClassName("params-paramsList__item-_2Y2O"));
							foreach (var elem in attributes)
							{
								string[] nameValuePare;

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

