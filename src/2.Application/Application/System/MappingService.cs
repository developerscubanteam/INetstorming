using Application.System.Contracts;
using System.Collections.Generic;

namespace Application.System
{
    public class MappingService : IMappingService
    {
        private HashSet<string> _mappingMealplan;

        public MappingService()
        {
            _mappingMealplan = new HashSet<string>();
            _mappingMealplan.Add("1073742786"); //Free Breakfast;
            _mappingMealplan.Add("1073742857"); //Breakfast for 1;
            _mappingMealplan.Add("1073744459"); //Coffee/Pastries Each Morning;
            _mappingMealplan.Add("1073744734"); //Japanese-style breakfast;
            _mappingMealplan.Add("1073744735"); //Taiwanese-style breakfast;
            _mappingMealplan.Add("1073744736"); //Vegetarian dinner;
            _mappingMealplan.Add("1073744737"); //Chinese-style dinner;
            _mappingMealplan.Add("1073744738"); //Kaiseki dinner;
            _mappingMealplan.Add("1073744739"); //Kaiseki seafood dinner;
            _mappingMealplan.Add("1073744750"); //Kaiseki meat dinner;
            _mappingMealplan.Add("1073744751"); //Kaiseki vegetarian dinner;
            _mappingMealplan.Add("1073744752"); //Buffet dinner;
            _mappingMealplan.Add("1073744753"); //Half board (Chinese-style dinner);
            _mappingMealplan.Add("1073744754"); //Half board (Kaiseki dinner);
            _mappingMealplan.Add("1073744755"); //Half board (buffet dinner);
            _mappingMealplan.Add("2102"); //All Meals;
            _mappingMealplan.Add("2103"); //Continental Breakfast;
            _mappingMealplan.Add("2104"); //Full Breakfast;
            _mappingMealplan.Add("2105"); //English Breakfast;
            _mappingMealplan.Add("2106"); //Free Lunch;
            _mappingMealplan.Add("2107"); //Free Dinner;
            _mappingMealplan.Add("2111"); //All-Inclusive;
            _mappingMealplan.Add("2193"); //Continental Breakfast for 2;
            _mappingMealplan.Add("2194"); //Breakfast for 2;
            _mappingMealplan.Add("2205"); //Breakfast Buffet;
            _mappingMealplan.Add("2206"); //Half Board;
            _mappingMealplan.Add("2207"); //Full Board;
        }

        public bool IsMealplan(string externalCode)
        {
            return _mappingMealplan.Contains(externalCode);            
        }

    }
}
