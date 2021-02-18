using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICapstone.Models.PuppyRecipe
{
    public class FavoritesViewModel
    {
        public List<Recipe> Favorites { get; set; }

        public string LoggedInUser { get; set; }
    }
}
