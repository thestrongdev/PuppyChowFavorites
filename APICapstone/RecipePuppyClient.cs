using APICapstone.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace APICapstone
{
    public class RecipePuppyClient
    {
        private HttpClient _client;

        public RecipePuppyClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<RecipeResponseModel> GetRecipes(string searchKeyword)
        {
            if (searchKeyword.Contains(",")){

                return await GetAsync<RecipeResponseModel>($"?i={searchKeyword}");
            }

            return await GetAsync<RecipeResponseModel>($"?q={searchKeyword}");
        }


        private async Task<T> GetAsync<T>(string endPoint)
        {

            var response = await _client.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStreamAsync();

                // var jsonOptions = new JsonSerializerOptions();

                var model = await JsonSerializer.DeserializeAsync<T>(content);

                return model;
            }
            else
            {
                throw new HttpRequestException("Puppy Recipe API returned bad response");
            }
        }
    }
}
