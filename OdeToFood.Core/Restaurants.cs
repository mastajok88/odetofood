using System;

namespace OdeToFood.Core
{
    public class Restaurants
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public CuisineType Cuisine { get; set; }
    }
}
