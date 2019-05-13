using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class RestaurantData : IRestaurantData
    {
        private readonly OdeToFoodDbContext _db;

        public RestaurantData(OdeToFoodDbContext db)
        {
            _db = db;
        }


        public IEnumerable<Restaurant> GetAll()
        {
            return _db.Restaurants;
        }

        public IEnumerable<Restaurant> GetRestaurantByName(string name = null)
        {
            return _db.Restaurants.Where(x => x.Name.StartsWith(name) || string.IsNullOrEmpty(name))
                .OrderBy(x => x.Name);
        }

        public Restaurant GetById(int id)
        {
            return _db.Restaurants.Find(id);
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var entity = _db.Restaurants.Attach(updatedRestaurant);
            entity.State = EntityState.Modified;
            return updatedRestaurant;
        }

        public Restaurant Create(Restaurant newRestaurant)
        {
            _db.Add(newRestaurant);
            return newRestaurant;
        }

        public Restaurant Delete(int restaurantId)
        {
            var restaurant = GetById(restaurantId);
            _db.Remove(restaurant);
            return restaurant;
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public int GetCount()
        {
            return _db.Restaurants.Count();
        }
    }
}