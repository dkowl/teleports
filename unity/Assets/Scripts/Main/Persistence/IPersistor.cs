
public interface IPersistor<T>
{
    T Load();
    void Save(T value);
}
