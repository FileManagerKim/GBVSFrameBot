using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using GBVSFrameBot.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace GBVSFrameBot.Services
{
    public class DatabaseService
    {   
        
        // CRUD : Generic Retrieval function that uses a passed dbcontext and Linq expression
        public IEnumerable<T> getQueryResult<T>(Expression<Func<T,bool>> predicate, DbContext ctx) where T : class
        {
            List<T> queryResults = new List<T>();
            try
            {
                // Query for a specific input.
                queryResults = ctx.Set<T>().AsQueryable().Where(predicate).ToList();
            }
            catch(Exception e)
            {
                // Log to console.
                Console.WriteLine($"[General/Database] {e}");
            }

            return queryResults;
        }
   
    }
}