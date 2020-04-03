using System.Linq;
using mfbcustomizerserver.Models;

namespace mfbcustomizerserver.Services
{
    public class RecipeService
    {
        public static Recipe GetRecipeById(MyContext ctx, string recipeId, bool isRecursive = true)
        {
            var recipe = ctx.Recipes.First(r => r.Id == recipeId && r.IsDeleted == 0);
            return isRecursive ? CompleteRecipeTree(ctx, recipe) : recipe;
        }
        
        public static Recipe CompleteRecipeTree(MyContext ctx, Recipe recipe)
        {
            recipe.IngredientStacks = ctx.ObjectStacks.Where(o =>
                    o.Type == ObjectStack.StackTypes.Ingredient
                    && o.IsDeleted == 0
                    && o.ParentId == recipe.Id
                )
                .AsEnumerable()
                .Select(s =>
                {
                    s.Item = IngredientService.GetIngredientById(ctx,s.ItemId);
                    return s; 
                })
                .ToList();
            return recipe;
        }
    }
}