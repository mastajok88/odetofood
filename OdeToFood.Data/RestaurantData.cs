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

        public Restaurant GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            throw new System.NotImplementedException();
        }

        public int Commit()
        {
            throw new System.NotImplementedException();
        }
    }
}