﻿using Microsoft.Azure.Documents;
using ParkingGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ParkingGarage.Repositories {
    public interface ICosmosDBRepository<T> where T : class {

        Task<T> GetItemAsync(string id);

        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);

        Task<Document> CreateItemAsync(T item);

        Task<Document> UpdateItemAsync(string id, T item);

        Task DeleteItemAsync(string id);

        Task Initialize();
    }
}