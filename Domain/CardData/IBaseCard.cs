using System;

namespace Domain.CardData
{
    public interface IBaseCard
    {
        Guid PlayerGuid { get; }
        string Id { get; }
        string Name { get; }
        Currency Cost { get; }
        ElementEnum Element { get; }
        Feature Feature { get; }
    }
}
