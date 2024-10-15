using NewAvitoParser.CsvServices;
using NewAvitoParser.Coomon;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections;
using System;
using System.Reflection.Metadata;
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
			options.AddArgument("no-sandbox");
			//options.BinaryLocation = "/opt/google/chrome/google-chrome";
			//options.AddArgument("--proxy-server=http://20.206.106.192:8123");
			ChromeDriver driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
			
			driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(30));

			var urlList = new HashSet<string>();
			var properties = new HashSet<Property>();

			using (driver)
			{
				try
				{
					ConnectToUrl(driver, Constants.ElectronicSectionsList[0]);
					//foreach (var url in Constants.ElectronicSectionsList)
					{
						var links = driver.FindElements(By.XPath("//*[@id]/div/div/div[2]/div[2]/div/a"))
							.Select(item => item.GetAttribute("href"));

						File.WriteAllLines(path: Constants.Files.Links, contents: links.ToList());
					}

					{
						var links = File.ReadAllLines(Constants.Files.Links).ToList();
						foreach (var url in links)
						{
							ConnectToUrl(driver, url);

							var attributes = driver.FindElements(By.ClassName("params-paramsList__item-_2Y2O")).Select(item =>
							{
								var list = item.Text.Split(" ");

								var property = new Property
								{
									Name = list[0],
									Value = list[1]
								};

								return AvitoParamsConverter.ParamDisoposer(property);
							}).ToList();
						}
					}

					

				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return;
				}
				finally
				{

				}
			}
		}
	}
}

