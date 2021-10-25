using System;
using System.Linq;
using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;
using Array = System.Array;
using Object = UnityEngine.Object;

/*
設定血量->初始化生成->監聽/觸發扣血事件*
在*中，同時可以
判斷角色是否死亡
角色當前符號是否符合可以扣除條件 ？

角色當前血條 - BloodHandler
共享資料 - BloodCenter
從中取用prefab、圖片素材、腳本，克隆並設定參數
->append under parent `Instantiate(Canvas, parent);`
->set bloodType
->Init
->加到Entity陣列中

當角色要扣血：
void Spoiler.OnTriggerEnter2D() {
    var handler = BattleInterface.FindComponent();
    if (handler.DiscardBlood())
        HitEvent.Execute();
}
 */
namespace Main.Blood
{
    [Serializable]
    public class BloodHandler : MonoBehaviour, IComponent
    {
        public bool IsEmpty => !_bloodQueue.Any();
        private GameObject[] _bloodQueue;

        [SerializeField, Header("Blood Queue Selector")]
        private BloodType[] _bloodData; // 是否需要同步更新...？如何知道是否死亡？

        private Vector2[] _posArr;

        // 初始化1，克隆的canvas、設定parent、掛載handler腳本
        public static BloodHandler Instance([NotNull] Transform creatureTrans) =>
            creatureTrans.GetComponentInChildren<BloodHandler>();

        private void Start() => InitUIandData(_bloodData);

        public BloodHandler InitUIandData(BloodType[] bloodData)
        {
            _bloodData = bloodData;
            _posArr = PosAlgo(_bloodData.Length); // 計算位置
            _bloodQueue = TranslateDataToObject(FindObjectOfType<BloodLoader>(), _bloodData); // 產生物件
            return this;
        }

        //Calc Pos Array
        private Vector2[] PosAlgo(int count)
        {
            float e1 = -((count - 1) * 25 / 2F);
            var posArr = new Vector2 [count];
            for (var i = 0; i < count; i++)
            {
                posArr[i] = new Vector2(e1 + i * 25, 0);
            }

            return posArr;
        }

        // Translate Enum Data to GameObject Pos
        private GameObject[] TranslateDataToObject(BloodLoader loader, BloodType[] bloodData)
        {
            if (loader is null) throw new NullReferenceException();

            var bloodQueue = new GameObject[bloodData.Length];
            for (var i = 0; i < bloodData.Length; i++)
            {
                var imgGo = bloodData[i] switch
                {
                    BloodType.CCircle => loader.bloodElements.c250Circle,
                    BloodType.CCrossx => loader.bloodElements.c250Crossx,
                    BloodType.CSquare => loader.bloodElements.c250Square,
                    BloodType.XCircle => loader.bloodElements.x250Circle,
                    BloodType.XCrossx => loader.bloodElements.x250Crossx,
                    BloodType.XSquare => loader.bloodElements.x250Square,
                    _ => bloodQueue[i]
                };
                bloodQueue[i] = Instantiate(imgGo,
                    new Vector3(0, 0), Quaternion.identity, transform);
                bloodQueue[i].GetComponent<RectTransform>().anchoredPosition = _posArr[i];
            }

            return bloodQueue;
        }
        public bool DiscardBlood(BloodType bloodType)
        {
            if (IsEmpty || !bloodType.Equals(_bloodData[0])) return false;
            return DiscardBlood(1);
        }
        //Mono Blood and Multiple Blood Discard
        public bool DiscardBlood(int count = 1)
        {
            if (_bloodQueue.Length < 1) return false;
            GameObject[] objs = new GameObject[_bloodQueue.Length - count]; // 更新obj
            Array.Copy(_bloodQueue, count, objs, 0, _bloodQueue.Length - count);
            BloodType[] types = new BloodType[_bloodQueue.Length - count]; // 更新bloodData
            Array.Copy(_bloodData, count, types, 0, _bloodQueue.Length - count);
            for (var i = 0; i < count; i++)
            {
                Object.Destroy(_bloodQueue[i]);
            }

            UpdateQueue(objs);
            UpdateData(types);
            return true;
        }

        //Discard Specific Blood 
        public bool PenetrateBlood(BloodType bt)
        {
            if (_bloodQueue.Length < 1) return false;
            GameObject[] objs = new GameObject[_bloodQueue.Length - 1];
            BloodType[] types = new BloodType[_bloodQueue.Length];
            BloodType[] saved = new BloodType[_bloodQueue.Length - 1];
            bool isDiscarded = false;
            Array.Copy(_bloodData, _bloodData.Length - _bloodQueue.Length, types, 0, _bloodQueue.Length);
            for (var i = 0; i < types.Length; i++)
            {
                if (isDiscarded)
                {
                    Debug.Log("Start Copy!");
                    Array.Copy(_bloodQueue, i, objs, i - 1, _bloodQueue.Length - i);
                    Array.Copy(_bloodData, i, saved, i - 1, _bloodQueue.Length - i);
                    Object.Destroy(_bloodQueue[i - 1]);
                    break;
                }

                if (types[i].Equals(bt))
                {
                    isDiscarded = true;
                    continue;
                }

                objs[i] = _bloodQueue[i];
                objs[i].GetComponent<RectTransform>().anchoredPosition =
                    _bloodQueue[i].GetComponent<RectTransform>().anchoredPosition;
            }

            UpdateQueue(objs);
            UpdateData(saved);

            return true;
        }

        private void UpdateQueue(GameObject[] objs)
        {
            /*Array.Clear(_bloodQueue, 0, _bloodQueue.Length);
            Array.Copy(objs, 0, _bloodQueue, 0, objs.Length);*/
            for (var i = 0; i < objs.Length; i++)
            {
                objs[i].GetComponent<RectTransform>().anchoredPosition = _posArr[i];
            }

            _bloodQueue = objs;
        }

        private void UpdateData(BloodType[] types) => _bloodData = types;

        // ECS
        public EnumComponentTag Tag => EnumComponentTag.Blood;
        private bool _facingRight = true;

        public void Update()
        {
            var x = transform.root.localScale.x;
            if (x > 0.1f && !_facingRight)
                Flip(); //往左翻

            if (x < -0.1f && _facingRight)
                Flip(); //往右翻
        }
        private void Flip()
        {
            _facingRight = !_facingRight;
            transform.localScale =
                Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        }
    }
}