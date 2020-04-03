using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using mfbcustomizerserver.Models;
using mfbcustomizerserver.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.Extensions.Logging;
using static mfbcustomizerserver.Models.ObjectStack;

namespace mfbcustomizerserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodBagController : ControllerBase
    {
        private readonly ILogger<FoodBagController> _logger;

        public FoodBagController(ILogger<FoodBagController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<FoodBag>> Get(String include)
        {
            using (var ctx = MyContext.New())
            {
                var includes = include.Split(",").Select(s => s.Trim()).ToArray();
                return ctx.FoodBags.AsEnumerable().Select(fb => FoodBagService.CompleteFoodBag(ctx,fb,includes)).ToList();
            }
        }

        [HttpPost]
        public ActionResult<FoodBag> CreateFoodBag(FoodBag foodBag)
        {
            using (var ctx = MyContext.New())
            {
                foodBag.IsPersisted = 1;
                
                ctx.FoodBags.Add(foodBag);
                ctx.ObjectMetas.Add(new ObjectMeta
                {
                    Id = Guid.NewGuid().ToString(),
                    ObjectId = foodBag.Id,
                    Name = "description",
                    Value = foodBag.Description ?? "",
                });
                foreach (var ingredientStack in foodBag.IngredientStacks)
                {
                    var s = new ObjectStack
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = StackTypes.Ingredient,
                        ParentId = foodBag.Id,
                        ItemId = ingredientStack.ItemId,
                        TotalAmount = ingredientStack.TotalAmount,
                        IsDeleted = 0,
                        IsPersisted = 1,
                    };
                    ctx.ObjectStacks.Add(s);
                }
                
                foreach (var recipeStack in foodBag.RecipeStacks)
                {
                    var s = new ObjectStack
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = StackTypes.Recipe,
                        ParentId = foodBag.Id,
                        ItemId = recipeStack.ItemId,
                        TotalAmount = recipeStack.TotalAmount,
                        IsDeleted = 0,
                        IsPersisted = 1,
                    };
                    ctx.ObjectStacks.Add(s);
                }
                
                ctx.SaveChanges();
                return foodBag;
            }
        }
        
        [HttpGet]
        [Route("{bagId}")]
        public ActionResult<FoodBag> Get(String bagId, String include)
        {
            using (var ctx = MyContext.New())
            {
                var includes = include.Split(",").Select(s => s.Trim()).ToArray();
                return FoodBagService.GetFoodBagById(ctx,bagId,includes);
            }
        }

        [HttpPut]
        [Route("{foodBagId}")]
        public ActionResult<FoodBag> Put(String foodBagId, [FromBody]FoodBag foodBag)
        {
            using (var ctx = MyContext.New())
            {
                ctx.FoodBags.Update(foodBag);
                foreach (var ingredientStack in foodBag.IngredientStacks)
                {
                    if (ingredientStack.IsPersisted == 0)
                    {
                        ingredientStack.IsPersisted = 1;
                        ingredientStack.ParentId = foodBagId;
                        ingredientStack.Type = StackTypes.Ingredient;
                        ctx.ObjectStacks.Add(ingredientStack);
                    }
                    else
                    {
                        ctx.ObjectStacks.Update(ingredientStack);
                    }
                }
                
                foreach (var recipeStack in foodBag.RecipeStacks)
                {
                    if (recipeStack.IsPersisted == 0)
                    {
                        recipeStack.IsPersisted = 1;
                        recipeStack.ParentId = foodBagId;
                        recipeStack.Type = StackTypes.Recipe;
                        ctx.ObjectStacks.Add(recipeStack);
                    }
                    else
                    {
                        ctx.ObjectStacks.Update(recipeStack);
                    }
                }
                
                var descp = ctx.ObjectMetas.FirstOrDefault(m => m.ObjectId == foodBagId && m.Name == "description");
                if (descp != null)
                {
                    descp.Value = foodBag.Description;
                    ctx.ObjectMetas.Update(descp);
                }
                
                ctx.SaveChanges();
            }
            return foodBag;
        }
        
    }
}