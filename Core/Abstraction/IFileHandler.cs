namespace Core.Abstraction
{
    public interface IFileHandler<T>
    {
        T Decode(string value);
    }
}