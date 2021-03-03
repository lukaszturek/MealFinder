using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace MealFinder
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var meal = await ProcessMeal();

			PrintRecipe(meal);

		}

		private static async Task<Meal> ProcessMeal()
		{
			HttpClient client = new HttpClient();
			string filePath = @"..\..\..\files\Meal.txt";
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.Add("x-rapidapi-key", "d0d13c1d59mshf5a6f85d3cec2b8p12f4c4jsnd51"); //key is changed to protect privacy
			client.DefaultRequestHeaders.Add("x-rapidapi-host", "themealdb.p.rapidapi.com");

			// consume API
			//string stringTask = await client.GetStringAsync("https://themealdb.p.rapidapi.com/random.php");
			//stringTask = stringTask.Substring(10, stringTask.Length - 12);

			string stringTask = await File.ReadAllTextAsync(filePath);
			var meal = JsonSerializer.Deserialize<Meal>(stringTask);


			// serialize to file
			//await File.WriteAllTextAsync(filePath, stringTask);

			return meal;

		}

		private static void PrintRecipe(Meal meal)
		{
			Console.WriteLine("Meal name: " + meal.Name);
			Console.WriteLine("{0} is a {1} {2} dish.", meal.Name, meal.Area.ToLower(), meal.Category.ToLower());
			Console.WriteLine("\nIndredients:");
			for (int i = 1; i <= 20; i++)
			{
				var ingredient = meal.GetType().GetProperty("Ingredient" + i).GetValue(meal).ToString();
				var measure = meal.GetType().GetProperty("Measure" + i).GetValue(meal).ToString();

				if (!string.IsNullOrEmpty(ingredient) && !string.IsNullOrEmpty(measure))
				{
					Console.WriteLine($"\t{measure} of {ingredient.ToLower()}");
				}
			}
			Console.WriteLine($"\nInstructions:\n{meal.Instruction}");
		}
	}


}
