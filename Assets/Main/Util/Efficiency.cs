using System;
using JetBrains.Annotations;
using UnityEngine;
// 更新：
//   這樣寫沒有比較不耗時：
//     包含var pp, 區域變數pp, 創一個當地物件, 呼叫遠端物件，這幾種耗能相同
namespace Main.Util
{
    public static class Efficiency
    {
        public static float UnityEventTest([NotNull] Action action, int count = 10000, string tag = null)
        {
            var t1 = UnityEngine.Time.realtimeSinceStartup;
            for (var i = 0; i < count; i++)
                action();
            var t2 = UnityEngine.Time.realtimeSinceStartup;

            var timer = t2 - t1;
            if (tag == null || tag.Equals("")) Debug.Log($"程式耗時: {timer:E}");
            else Debug.Log($"{tag}:\t\t程式耗時: {timer:E}");

            return timer;
        }
    }
}
/*
Efficiency.UnityEventTest(() =>
        player.BoxCastAll(new Vector2(50, 50)).Filter(collider2D =>
            collider2D.CompareLayer("Ground"))
    , 10000); // 0.11
Efficiency.UnityEventTest(() =>
        player.BoxCastAll(new Vector2(50, 50)).FirstOrNull(collider2D =>
            collider2D.CompareLayer("Ground"))
    , 10000); // 0.09
Efficiency.UnityEventTest(() =>
        player.BoxCastAll(new Vector2(50, 50)).FirstOrNull(collider2D =>
            collider2D.CompareTag("Wall"))
    , 10000); // 0.08
*/
/*
Efficiency.UnityEventTest(() =>
        player.BoxCastAll(new Vector2(50, 50)).FirstOrNull(collider2D =>
            collider2D.CompareTag("Wall"))
    , 1000000); // 8.31 8.47 8.55 8.39 8.64
Efficiency.UnityEventTest(() =>
        player.BoxCastAll(new Vector2(50, 50), collider2D =>
            collider2D.CompareTag("Wall")).FirstOrNull()
    , 1000000);// 8.84 8.80 8.55 8.43 8.62
*/