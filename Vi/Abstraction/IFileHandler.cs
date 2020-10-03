namespace Vi.Abstraction
{
    public interface IFileHandler<T>
    {
        T Decode(string value);
    }
}