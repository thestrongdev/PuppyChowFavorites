using APICapstone.DALModels;
using APICapstone.Data;
using APICapstone.Models.PuppyRecipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICapstone.Controllers
{
    public class PuppyRecipeController : Controller
    {
        private RecipePuppyClient _recipePuppyClient;
        private ApplicationDbContext _applicationDbContext;
        private UserManager<IdentityUser> _userManager;

        public PuppyRecipeController(RecipePuppyClient recipePuppyClient,
            ApplicationDbContext applicationDbContext,
            UserManager<IdentityUser> userManager)
        {
            _recipePuppyClient = recipePuppyClient;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }



        public IActionResult Index()
        {
            return View();
        }

        [Authorize]

        public IActionResult RecipeSearch()
        {
            return View();
        }

        public IActionResult AddToFavorite()
        {
            //ADD and SAVE TO DATABASE 
            //get user id as well
            
            return View();
        }

        public IActionResult RemoveItem(int id, string userid)
        {
            FavoritesDAL FaveDAL = _applicationDbContext.Favorites
                .Where(favorite => favorite.FaveId == id).FirstOrDefault();

            _applicationDbContext.Favorites.Remove(FaveDAL);
            _applicationDbContext.SaveChanges();

            //new list of tasks to display w/USER ID!!
            var viewModel = new FavoritesViewModel();
            var favorites = _applicationDbContext.Favorites
                .Where(favorite => favorite.Id == userid).ToList();

            viewModel.Favorites = favorites.Select(faveDAL => new Recipe
            {
                ID = faveDAL.FaveId,
                title = faveDAL.Title,
                ingredients = faveDAL.Ingredients,
                href = faveDAL.href

            }).ToList();

            return View("Favorites", viewModel);
        }

        public async Task<IActionResult> RecipeResults(string SearchString)
        {
            if (String.IsNullOrEmpty(SearchString))
            {
                return View("RecipeSearch");
            }

            var response = await _recipePuppyClient.GetRecipes(SearchString);

            var viewModel = new RecipeResultsViewModel();

            viewModel.SearchResults = response.results.
                Select(
                result => new Recipe()
                {
                    title = result.title,
                    ingredients = result.ingredients,
                    thumbnail = result.thumbnail,
                    href = result.href

                }).ToList();

            viewModel.SearchKeyword = SearchString;

            


            return View(viewModel);
        }


    }
}
