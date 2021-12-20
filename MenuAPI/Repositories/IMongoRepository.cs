using MenuAPI.Models;
using System;
using System.Collections.Generic;

namespace MenuAPI.Repositories
{
    public interface IMongoRepository<T> where T : MenuItem//MongoDocument
    {
        List<T> GetAllMenuItems();
        T InsertMenuItem(T item);
        T GetMenuItemById(Guid id);
        List<T> GetMenuItemByCategory(String Category);

        void UpsertItem(T item);

        void DeleteItem(Guid id);

    }
}
