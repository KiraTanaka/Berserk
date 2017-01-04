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

        // ReSharper disable once NonReadonlyMemberInGetHashCode
//        int GetHashCode() => Guid.GetHashCode();
//        bool Equals(object obj) => (obj as IBaseCard)?.Guid == Guid;
    }
}
