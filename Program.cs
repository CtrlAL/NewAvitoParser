using NewAvitoParser.CsvServices;
using NewAvitoParser.Coomon;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections;

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
			options.AddArgument("no-sandbox");
			options.AddArgument("headless");
			options.BinaryLocation = "/usr/local/bin/chromedriver";
			//options.AddArgument("--proxy-server=http://20.206.106.192:8123");
			ChromeDriver driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
			
			driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(30));

			var urlList = new HashSet<string>();
			var properties = new HashSet<Property>();

			using (driver)
			{
				try
				{
					ConnectToUrl(driver, Constants.BaseUrl);

					var title = driver.FindElement(By.ClassName("main-top-header"));
					Console.WriteLine(title.Text);

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

