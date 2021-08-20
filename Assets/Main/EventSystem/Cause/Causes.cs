

// 所有的cause一定來自unity
namespace Main.EventSystem.Cause
{
    /// 所有的cause一定來自unity
    public interface ICause
    {
        bool AndCause();
        void Reset();
    }
}