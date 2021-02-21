using APICapstone.DALModels;
using APICapstone.Data;
using APICapstone.Models.ApiModels;
using APICapstone.Models.PuppyRecipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public IActionResult RecipeNotFound()
        {
            return View();
        }

        public async Task<IActionResult> Favorites()
        {
            var viewModel = new FavoritesViewModel();

            var user = await _userManager.GetUserAsync(User);

            var favorites = _applicationDbContext.Favorites
                .Where(favorite => favorite.Id == user.Id).ToList();

            viewModel.Favorites = favorites.Select(faveDAL => new Recipe
            {
                ID = faveDAL.FaveId,
                title = faveDAL.Title,
                ingredients = faveDAL.Ingredients,
                href = faveDAL.href

            }).ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> AddToFavorite(string search, int index)
        {
            var response = await _recipePuppyClient.GetRecipes(search);

            var recipeModel = new RecipeResultsViewModel();

            recipeModel.SearchResults = response.results.
                Select(
                result => new Recipe()
                {
                    title = result.title,
                    ingredients = result.ingredients,
                    thumbnail = result.thumbnail,
                    href = result.href

                }).ToList();

            var faveDAL = new FavoritesDAL();
            faveDAL.Title = recipeModel.SearchResults[index].title;
            faveDAL.Ingredients = recipeModel.SearchResults[index].ingredients;
            faveDAL.href = recipeModel.SearchResults[index].href;

            var user = await _userManager.GetUserAsync(User);
            faveDAL.Id = user.Id;

            _applicationDbContext.Favorites.Add(faveDAL);
            _applicationDbContext.SaveChanges();

            //CREATE USERs LIST TO SHOW ON THEIR VIEW
            var viewModel = new FavoritesViewModel();

            viewModel.Favorites = MapFavoritesList(user.Id);

            return View("Favorites", viewModel);
        }

        public async Task<IActionResult> RemoveItem(int id)
        {
            FavoritesDAL FaveDAL = _applicationDbContext.Favorites
                .Where(favorite => favorite.FaveId == id).FirstOrDefault();

            _applicationDbContext.Favorites.Remove(FaveDAL);
            _applicationDbContext.SaveChanges();

            var user = await _userManager.GetUserAsync(User);

            //new list of tasks to display w/USER ID!!
            var viewModel = new FavoritesViewModel();
            viewModel.Favorites = MapFavoritesList(user.Id);

            return View("Favorites", viewModel);
        }

        public async Task<IActionResult> RecipeResults(string SearchString)
        {
            if (String.IsNullOrEmpty(SearchString))
            {
                return View("RecipeSearch");
            }

            var response = await _recipePuppyClient.GetRecipes(SearchString);

            //if(response.results.Length==0)
            //{
            //    var model = new RecipeNotFoundViewModel();
            //    model.Ingredients = SearchString;
            //    return View("RecipeNotFound", model);
            //}

            var viewModel = new RecipeResultsViewModel();

            //viewModel.SearchResults = MapSearchesList(SearchString, response);

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

        private List<Recipe>  MapFavoritesList(string userID)
        {
            List<Recipe> MapToFave = new List<Recipe>();

            var favorites = _applicationDbContext.Favorites
               .Where(favorite => favorite.Id == userID).ToList();

            MapToFave = favorites.Select(faveDAL => new Recipe
            {
                ID = faveDAL.FaveId,
                title = faveDAL.Title,
                ingredients = faveDAL.Ingredients,
                href = faveDAL.href

            }).ToList();

            return MapToFave;

        }

        //private async  Task<List<Recipe>> MapSearchesList(string search, RecipeResponseModel response)
        //{
        //    //var response = await _recipePuppyClient.GetRecipes(search);

        //    List<Recipe> searchResults = new List<Recipe>();

        //    searchResults = response.results.
        //            Select(
        //            result => new Recipe()
        //            {
        //                title = result.title,
        //                ingredients = result.ingredients,
        //                thumbnail = result.thumbnail,
        //                href = result.href

        //            }).ToList();

        //    return searchResults;

        //}


    }
}
