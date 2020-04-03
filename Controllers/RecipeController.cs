using System;
using System.Collections.Generic;
using System.Linq;
using mfbcustomizerserver.Models;
using mfbcustomizerserver.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace mfbcustomizerserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly ILogger<RecipeController> _logger;

        public RecipeController(ILogger<RecipeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Recipe> Get([FromQuery] String term, [FromQuery] String ingredients)
        {
            using (var ctx = MyContext.New())
            {
                var query = ctx.Recipes.AsQueryable();
                if (!String.IsNullOrEmpty(term))
                {
                    var lowered = term.ToLower();
                    query = query.Where(r => r.Name.ToLower().Contains(lowered));
                }

                query = query
                    .AsEnumerable()
                    .Select(r =>
                    {
                        return RecipeService.CompleteRecipeTree(ctx, r);
                    })
                    .AsQueryable();
                
                if (!String.IsNullOrEmpty(ingredients))
                {
                    var ingredientIds = ingredients.Split(",").Select(s => s.Trim());
                    // too lazy to write a proper query
                    query = query
                        .AsEnumerable()
                        .Where(r =>
                        {
                            bool containsAll = r.IngredientStacks.Count > 0;
                            r.IngredientStacks.ForEach(i => containsAll &= ingredientIds.Contains(i.ItemId));
                            return containsAll;
                        })
                        .AsQueryable();
                }                
                


                return query.ToList();
            }
        }
    }
}