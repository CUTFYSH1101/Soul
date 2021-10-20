namespace Main.Entity
{
    public interface IComponent
    {
        EnumComponentTag Tag { get; }
        void Update();
    }
}