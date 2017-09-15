using System;
using System.Collections.Generic;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext.Repository
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
