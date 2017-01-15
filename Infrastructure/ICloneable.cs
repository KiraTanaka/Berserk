namespace Infrastructure
{
    public interface ICloneable<out T>
    {
        T Clone();
    }
}