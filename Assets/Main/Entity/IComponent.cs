namespace Main.Entity
{
    public interface IComponent
    {
        int Id { get; }
        void Update();
    }
}