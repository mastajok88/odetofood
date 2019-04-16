using System.Collections.Generic;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class RestaurantData : IRestaurantData
    {
        public IEnumerable<Restaurant> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Restaurant> GetRestaurantByName(string name = null)
        {
            throw new System.NotImplementedException();
        }
    }
}