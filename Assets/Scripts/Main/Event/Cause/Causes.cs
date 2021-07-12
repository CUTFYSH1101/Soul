

// 所有的cause一定來自unity
namespace Main.Event
{
    /// 所有的cause一定來自unity
    public interface ICause
    {
        bool Cause();
        void Reset();
    }
}