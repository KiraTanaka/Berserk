using System;

namespace Domain.CardData
{
    public interface IFeatureable
    {
        Action Feature { get; }
    }
}