using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes;

        public MainWindow()
        {
            InitializeComponent();
           
            LoadRecipes();
           
            DisplayRecipes();
        }

        private void LoadRecipes()
        {
            // Load recipes (in a real app, this might come from a database or file)
            recipes = new List<Recipe>
            {
               
                new Recipe { Name = "Spaghetti", Ingredients = "Pasta, Tomato Sauce", Steps = "Boil pasta, add sauce", FoodGroup = "Carbs", Calories = 400 },
                
                new Recipe { Name = "Salad", Ingredients = "Lettuce, Tomato, Cucumber", Steps = "Chop and mix ingredients", FoodGroup = "Vegetables", Calories = 150 }
            };
        }

        private void DisplayRecipes()
        {
            RecipeListBox.ItemsSource = recipes.OrderBy(r => r.Name).Select(r => r.Name);
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            
            string filter = FilterTextBox.Text.ToLower();
            
            RecipeListBox.ItemsSource = recipes
                .Where(r => r.Ingredients.ToLower().Contains(filter))


                .OrderBy(r => r.Name)

                .Select(r => r.Name);
        }

        private void RecipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipeListBox.SelectedItem != null)
            {
                string selectedRecipeName = RecipeListBox.SelectedItem.ToString();




                Recipe selectedRecipe = recipes.First(r => r.Name == selectedRecipeName);


                MessageBox.Show($"{selectedRecipe.Name}\n\nIngredients:\n{selectedRecipe.Ingredients}\n\nSteps:\n{selectedRecipe.Steps}", "Recipe Details");
            }
        }

        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            string name = RecipeNameTextBox.Text;

            string ingredients = IngredientsTextBox.Text;
            string steps = StepsTextBox.Text;


            string foodGroup = FoodGroupTextBox.Text;
            int calories;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ingredients) || string.IsNullOrWhiteSpace(steps) || string.IsNullOrWhiteSpace(foodGroup) || !int.TryParse(CaloriesTextBox.Text, out calories))
            {
                MessageBox.Show("Please fill out all fields correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newRecipe = new Recipe
            {
                Name = name,


                Ingredients = ingredients,

                Steps = steps,
                FoodGroup = foodGroup,


                Calories = calories
            };

            recipes.Add(newRecipe);
            DisplayRecipes();

            // Clear input fields
            RecipeNameTextBox.Clear();

            IngredientsTextBox.Clear();
            StepsTextBox.Clear();

            FoodGroupTextBox.Clear();

            CaloriesTextBox.Clear();

        }

        private void ShowPieChartButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPieChart();
        }

        private void ShowPieChart()
        {
            PieChart.Visibility = Visibility.Visible;
            var model = new PlotModel { Title = "Menu Composition" };


            var series = new PieSeries();


            foreach (var group in recipes.GroupBy(r => r.FoodGroup))
            {
                series.Slices.Add(new PieSlice(group.Key, group.Count()));
            }

            model.Series.Add(series);


            PieChart.Model = model;
        }
    }

    public class Recipe
    {
        public string Name { get; set; }


        public string Ingredients { get; set; }
        public string Steps { get; set; }


        public string FoodGroup { get; set; }
        public int Calories { get; set; }
    }
}
