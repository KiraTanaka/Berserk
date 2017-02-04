namespace Infrastructure.Cloneable
{
    public interface ICloneable<out T>
    {
        T Clone();
    }
}