using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mfbcustomizerserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace mfbcustomizerserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly ILogger<IngredientController> _logger;

        public IngredientController(ILogger<IngredientController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Ingredient> Get([FromQuery] String term)
        {
            using (var context = MyContext.New())
            {
                if (String.IsNullOrEmpty(term))
                {
                    return context.Ingredients.ToList();
                }
                else
                {
                    var lowered = term.ToLower();
                    return context.Ingredients.Where(i => i.Name.ToLower().Contains(lowered)).ToList();
                }
            }
        }
    }
}