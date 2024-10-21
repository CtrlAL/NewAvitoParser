using NewAvitoParser.CsvServices;
using NewAvitoParser.Coomon;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections;
using NewAvitoParser;
using NewAvitoParser.Enums;
using Category = NewAvitoParser.Coomon.Avito.Category;
using SubCategory = NewAvitoParser.Coomon.Avito.SubCategory;
using Translite = NewAvitoParser.Coomon.Healpers.Translite;
namespace AvitoParser
{
	internal class Program
	{
		private static ChromeDriver _driver = null;
		private static bool _createSatus = false;
		static Program()
		{
			try
			{
				ChromeOptions options = new ChromeOptions();
				options.AddArgument("headless=new");
				options.AddArgument("disable-gpu");
				options.AddArgument("no-sandbox");
				options.AddArgument("window-size=1920,1080");

				_driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
				_driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(30));
				_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

				ConnectToUrl("https://www.avito.ru/");
				var button = _driver.FindElement(By.XPath("//*[@id]/div/div[4]/div/div[1]/div/div/div[3]/div[1]/div/div/div[1]"));
				button.Click();
				
				var categoryBoard = _driver.FindElement(By.ClassName("new-rubricator-content-root-_qZMR"));
				var categoryList = categoryBoard.FindElement(By.ClassName("new-rubricator-content-leftcontent-_hhyV")).FindElements(By.ClassName("new-rubricator-content-rootCategory-S2VPI"));

				for (int i = 0; i < categoryList.Count; i++)
				{
					categoryList[i].Click();
					var categoryInfo = _driver.FindElement(By.ClassName("new-rubricator-content-rightContent-zbUZa"));

					var moreButton = categoryInfo.FindElements(By.ClassName("new-rubricator-content-moreButton-i6UsT"));

					//Clisk More Buttons for load attitional content
                    foreach (var item in moreButton)
                    {
						item.Click();
                    }

                    var category = categoryInfo.FindElement(By.ClassName("desktop-16cl456"));
					Avito.CategoryList.Add(i, new Category { Name = category.Text.Replace("›", " ").Trim(), Link = category.GetAttribute("href") });

					var subCategory = categoryInfo.FindElements(By.ClassName("new-rubricator-content-child-_bmMo"));

					foreach (var item in subCategory)
					{
						try
						{
							var subSubCategories = item.FindElements(By.ClassName("desktop-mv9h2z"));

							if (subSubCategories.Count == 0)
							{
								var link = item.FindElement(By.ClassName("desktop-rlpl5r")).GetAttribute("href");
								var name = item.FindElement(By.ClassName("styles-module-root-bLKnd")).Text.Replace("›", " ").Trim();
								Console.WriteLine($"Категория:{name}");
								Avito.SubCategoryList.Add(new SubCategory { CategoryId = i, Link = link, Name = name });
							}


							foreach (var childCategory in subSubCategories)
							{
								var link = childCategory.GetAttribute("href");
								var name = childCategory.FindElement(By.ClassName("styles-module-size_s-xb_uK")).Text;
								Console.WriteLine($"Категория:{name}");
								Avito.SubCategoryList.Add(new SubCategory { CategoryId = i, Link = link, Name = name });
							}
						}
						catch(Exception ex)
						{
							continue;
						}
					}
				}
			}
			catch(Exception ex)
			{
				LogException(ex, string.Empty);
				return;
			}

			_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
			_createSatus = true;
		}

		public static IEnumerable Conection(string url)
		{
			var task = _driver.Navigate().GoToUrlAsync(url);
			int i = 0;
			while (!task.IsCompleted)
			{
				Thread.Sleep(1000);
				i++;
				yield return i;
			}
		}

		public static void ConnectToUrl(string url)
		{
			foreach (var second in Conection(url))
			{
				Console.WriteLine($"NavigateToUrl/{second}");
			}
		}
		
		public static void SetDirictories()
		{
			foreach (var elem in Avito.CategoryList)
			{
				var name = Translite.CyrilicToLatin(elem.Value.Name);

				string path1 = Constants.Files.PropertiesVaritant(name);
				string path2 = Constants.Files.PropertyNamesVariant(name);
				string path3 = Constants.Files.PropertyValuesVariant(name);
				string path4 = Constants.Files.LinksVaritant(name);

				if (!File.Exists(path1))
				{
					Directory.CreateDirectory($"..\\..\\{name}");
					File.Create(path1);
				}

				if (!File.Exists(path2))
				{
					Directory.CreateDirectory($"..\\..\\{name}");
					File.Create(path2);
				}

				if (!File.Exists(path3))
				{
					Directory.CreateDirectory($"..\\..\\{name}");
					File.Create(path3);
				}

				if (!File.Exists(path4))
				{
					Directory.CreateDirectory($"..\\..\\{name}");
					File.Create(path4);
				}
			}
		}

		static void Main(string[] args)
		{
			if (_createSatus == false)
			{
				Dispose();
				return;
			}

			SetDirictories();

			Console.WriteLine("Set parser mode/ Enter 0 if you want to Links/ Enter 1 if you want to parse attributes from links file");
			int categoryId = default;
			var mode = (ParserMode)int.Parse(Console.ReadLine());
			

			Console.WriteLine("Aloowed Categories for parse:");
			foreach (var category in Avito.CategoryList)
			{
				Console.WriteLine($"	{category.Key}:{category.Value.Name}");
			}


			Console.WriteLine("Enter SectionId which u want to parse:");
			int.TryParse(Console.ReadLine(), out categoryId);

			var dirNameTemplate =	Translite.CyrilicToLatin(Avito.CategoryList[categoryId].Name);
			var properties = new HashSet<Property>();
			var propertiesNames = new List<PropNameMaper>();
			var propertiesValues = new List<PropValueMaper>();
			string currentUrl = string.Empty;


			using (_driver)
			{
				try
				{	
					if(mode == ParserMode.Links)
					{
						var links = new List<LinksMapper>();
						foreach (var category in Avito.SubCategoryList.Where(item => item.CategoryId == categoryId))
						{
							var url = category.Link;

							try
							{
								for (int i = 0; i < 100; i++)
								{
									ConnectToUrl(url + $"?p={i}");
									currentUrl = url + $"?p={i}";

									Console.WriteLine($"LINK{i}:{currentUrl}");
									var hrefs = _driver.FindElements(By.XPath("//*[@id]/div/div/div[2]/div[2]/div/a"))
										.Select(item => 
											new LinksMapper {
												Link = item.GetAttribute("href"),
												SubCategoryName = category.Name
											}
										)
										.ToArray();

									links.AddRange(hrefs);
								}
							}
							catch(Exception ex)
							{
								LogException(ex, currentUrl);
								continue;
							}
						}

						HelperCsv.WriteFile(path: Constants.Files.LinksVaritant(dirNameTemplate), content: links);
					}

					mode = ParserMode.Attributes;

					
					if (mode == ParserMode.Attributes)
					{
						var links = HelperCsv.ReadFile<LinksMapper>(Constants.Files.LinksVaritant(dirNameTemplate));

						var propertiesList = new List<Property>();

						foreach (var link in links)
						{
							var url = link.Link;
							var subCategoryName = link.SubCategoryName;

							ConnectToUrl(url);
							currentUrl = url;
							Console.WriteLine($"PRODUCT_LINK:{currentUrl}");
							var attributes = _driver.FindElements(By.ClassName("params-paramsList__item-_2Y2O"));

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
											SubCategoryName = subCategoryName,
											Name = nameValuePare[0].Trim(' '),
											Value = nameValuePare[1].Trim(' ')
										};

										propertiesList.AddRange(AvitoParamsConverter.ParamDisoposer(property));
									}
								}
							}
						}

						properties = new HashSet<Property>(propertiesList);

						propertiesNames = new HashSet<PropNameMaper>(properties.Select(p => new PropNameMaper
						{
							Id = 0,
							Name = p.Name,
							SubCategoryName = p.SubCategoryName,
						})).Select((item, Index) =>
							{
								item.Id = Index;
								return item;
							}
						).ToList();

						propertiesValues = new HashSet<PropValueMaper>(properties.Select((item) => new PropValueMaper
						{
							Id = 0,
							PropertyId = propertiesNames.Find(pn => item.Name == pn.Name).Id,
							Value = item.Value,
							SubCategoryName = item.SubCategoryName
						})).Select((item, Index) =>
						{
							item.Id = Index;
							return item;
						}
						).ToList();
					}
				}
				catch (Exception ex)
				{
					LogException(ex, currentUrl);
				}
				finally
				{
					HelperCsv.WriteFile(Constants.Files.PropertiesVaritant(dirNameTemplate), properties);
					HelperCsv.WriteFile(Constants.Files.PropertyNamesVariant(dirNameTemplate), propertiesValues);
					HelperCsv.WriteFile(Constants.Files.PropertyValuesVariant(dirNameTemplate), propertiesNames);
					Dispose();
				}
			}
		}

		public static void LogException(Exception exception, string url)
		{
			string log =
						"ERROR:\n{\n"
						+ $"Message: {exception.Message}\n"
						+ $"StackTrace: {exception.StackTrace}\n"
						+ $"LastUrl: {url}\n"
						+ "\n}\n";

			Console.WriteLine(log);
			File.AppendAllText(Constants.Files.LogFile, log);
			Console.WriteLine(exception.Message);
		}
		public static void Dispose()
		{
			_driver.Quit();
			if (_driver != null)
			{
				_driver.Dispose();
			}
		}
	}
}

