using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICapstone.Models.PuppyRecipe
{
    public class RecipeResultsViewModel
    {
        public List<Recipe> SearchResults { get; set; }
        public string SearchKeyword { get; set; }
    }
}
