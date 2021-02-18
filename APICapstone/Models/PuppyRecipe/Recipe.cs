using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICapstone.Models.PuppyRecipe
{
    public class Recipe
    {
        public string title { get; set; }
        public string href { get; set; }
        public string ingredients { get; set; }
        public string thumbnail { get; set; }

        public int ID { get; set; }
    }
}
