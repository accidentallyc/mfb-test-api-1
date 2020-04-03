using System.Linq;
using mfbcustomizerserver.Models;

namespace mfbcustomizerserver.Services
{
    public class RecipeStackService
    {
        public static ObjectStack GetIngredientStackById(MyContext ctx, string recipeStackId, bool isRecursive = true)
        {
            var recipeStack = ctx.ObjectStacks.First(r => r.Id == recipeStackId && r.IsDeleted == 0);
            return isRecursive ? CompleteRecipeStack(ctx, recipeStack) : recipeStack;
        }
        
        public static ObjectStack CompleteRecipeStack(MyContext ctx, ObjectStack recipeStack)
        {
            var recipe = ctx.Recipes.First(i => i.Id == recipeStack.ItemId && i.IsDeleted == 0);
            recipe.IngredientStacks = ctx.ObjectStacks
                .Where(s => s.ParentId == recipe.Id 
                            && s.IsDeleted == 0 
                            && s.Type == ObjectStack.StackTypes.Ingredient)
                .AsEnumerable()
                .Select(s =>
                {
                    s.Item = IngredientService.GetIngredientById(ctx, s.ItemId);
                    return s;
                })
                .ToList();
            recipeStack.Item = recipe;
            return recipeStack;
        }
    }
}