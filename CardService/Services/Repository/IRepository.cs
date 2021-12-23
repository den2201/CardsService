using CardService.Domain;
using CardService.Models.Request;
using CardService.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardService.Services
{
    public interface IRepository
    {
       Task Add<T>(T entity) where T: IEntity;

       Task<bool> Delete<T>(T enity) where T: IEntity;

    }
}
