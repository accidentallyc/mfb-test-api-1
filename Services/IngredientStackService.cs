using System.Linq;
using mfbcustomizerserver.Models;

namespace mfbcustomizerserver.Services
{
    public class IngredientStackService
    {
        public static ObjectStack GetIngredientStackById(MyContext ctx, string ingredientStackId, bool isRecursive = true)
        {
            var ingredientStack = ctx.ObjectStacks.First(r => r.Id == ingredientStackId && r.IsDeleted == 0);
            return isRecursive ? CompleteIngredientStack(ctx, ingredientStack) : ingredientStack;
        }
        
        public static ObjectStack CompleteIngredientStack(MyContext ctx, ObjectStack ingredientStack)
        {
            var ingredient = ctx.Ingredients.First(i => i.Id == ingredientStack.ItemId && i.IsDeleted == 0);
            ingredientStack.Item = ingredient;
            return ingredientStack;
        }
    }
}