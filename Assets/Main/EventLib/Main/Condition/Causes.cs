

// 所有的cause一定來自unity
namespace Main.EventLib.Condition
{
    /// 所有的cause一定來自unity
    public interface ICondition
    {
        bool AndCause();
        void Reset();
    }
}