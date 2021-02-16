using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Helpers
{
    public static class Paginador
    {
        
            public static IEnumerable<T> Page<T>(this IEnumerable<T> en, int pageSize, int page)
            {
                return en.Skip(page * pageSize).Take(pageSize);
            }
            public static IQueryable<T> Page<T>(this IQueryable<T> en, int pageSize, int page)
            {
                return en.Skip(page * pageSize).Take(pageSize);
            }
        
    }
}
