public interface ICycle<out TType>
{
    public TType Increment();
    public TType Decrement();
    public TType Retrieve();
}
