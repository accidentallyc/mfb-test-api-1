using System.Collections.Generic;
using System.Linq;
using mfbcustomizerserver.Models;

namespace mfbcustomizerserver.Services
{
    public class FoodBagService
    {
        public static FoodBag CompleteFoodBag(MyContext ctx, FoodBag foodBag, string[] includes)
        {
            if (includes != null)
            {
                foreach (var str in includes)
                {
                    switch (str.Trim())
                    {
                        case "creator":
                            foodBag.Creator = ctx.Users.FirstOrDefault(u => u.Id == foodBag.CreatorId);
                            break;
                        case "description":
                            var descp = ctx.ObjectMetas.FirstOrDefault(m => m.ObjectId == foodBag.Id && m.Name == str);
                            if (descp != null)
                            {
                                foodBag.Description = descp.Value;
                            }

                            break;
                        case "ingredientStacks":
                            var temp = ctx
                                .ObjectStacks
                                .Where(s => s.ParentId == foodBag.Id && s.Type == ObjectStack.StackTypes.Ingredient)
                                .ToList();

                            foodBag.IngredientStacks = ctx
                                .ObjectStacks
                                .Where(s => s.ParentId == foodBag.Id && s.Type == ObjectStack.StackTypes.Ingredient)
                                .AsEnumerable()
                                .Select(IngredientStack =>
                                {
                                    return IngredientStackService.CompleteIngredientStack(ctx, IngredientStack);
                                })
                                .ToList();
                            break;
                        case "recipeStacks":
                            foodBag.RecipeStacks = ctx
                                .ObjectStacks
                                .Where(s => s.ParentId == foodBag.Id && s.Type == ObjectStack.StackTypes.Recipe)
                                .AsEnumerable()
                                .Select(recipeStack =>
                                {
                                    return RecipeStackService.CompleteRecipeStack(ctx, recipeStack);
                                })
                                .ToList();
                            break;
                    }
                }
            }

            return foodBag;
        }

        public static FoodBag GetFoodBagById(MyContext ctx, string id, string[] includes)
        {
            var foodBag = ctx.FoodBags.FirstOrDefault(fb => fb.Id == id);
            if (foodBag != null)
            {
                foodBag = CompleteFoodBag(ctx,foodBag,includes);
            }

            return foodBag;
        }
    }
}