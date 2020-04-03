using System.Linq;
using mfbcustomizerserver.Models;

namespace mfbcustomizerserver.Services
{
    public class IngredientService
    {
        public static Ingredient GetIngredientById(MyContext ctx, string ingredientId)
        {
            return ctx.Ingredients.First(r => r.Id == ingredientId && r.IsDeleted == 0);
        }

    }
}