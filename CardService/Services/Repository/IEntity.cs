using System;

namespace CardService.Services.Repository
{
    public interface IEntity 
    {
        Guid Id { get; set; }
    }
}
