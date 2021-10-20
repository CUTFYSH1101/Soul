using Main.EventLib.Main.EventSystem.Main;
using Main.Game;
using Main.Res.Resources;
using UnityEngine;
using UnityEngine.UI;

/*
 * subject.invoke,
 * if onTrigger && isQTEEvent
 * QTEEvent.Invoke
 */
namespace Main.EventLib.Sub.UIEvent.QTE
{
    public class QteUIEvent : UIEvent
    {
        private Sprite _origin;
        private readonly QteImageList _imageList;
        private readonly Transform _compUI;
        private readonly Image _compCover;
        private readonly Image _compBackground;
        private QteImageList.ImageData icon;
        public bool Miss { get; set; }
        private float _timer;

        /// 1.自帶傷害事件 或
        /// 2.由呼叫的Skill判斷，移除此項
        public QteUIEvent(string hierarchyPath)
        {
            this.Build();

            _imageList = Loader.Find<QteImageList>(Loader.Tag.BloodQte);
            _compUI = UnityTool.GetComponentByPath<Transform>(hierarchyPath);
            _compBackground = _compUI == null ? null : _compUI.GetFirstComponentInChildren<Image>("Background");
            _compCover = _compUI == null ? null : _compUI.GetFirstComponentInChildren<Image>("Cover");
            FilterIn = () => !IsEmpty();
            ToInterrupt = () => Miss; // if keydown or duration.isTimeUp

            if (_compUI == null)
                return;
            _compUI.gameObject.SetActive(false);

            Debug.Log($"QTE is missing?\t{Miss}");
        }

        private bool IsEmpty() =>
            _compBackground == null || _compCover == null;

        public void Execute(EnumQteShape icon)
        {
            Miss = false;
            this.icon = _imageList.Find(icon);
            Director.CreateEvent();
            // Debug.Log("Invoke");
            Debug.Log($"invoke {icon.ToString()}");
        }

        public override void Enter()
        {
            // Debug.Log("Enter");
            // 顯示UI
            _compUI.gameObject.SetActive(true);
            _origin = _compBackground.sprite;
            // Debug.Log(icon.image==null);
            _compBackground.sprite = icon.image;
            _compBackground.rectTransform.sizeDelta = new Vector2(icon.size, icon.size);
            _timer = EventAttr.MaxDuration;
            _compCover.fillAmount = 1;
        }

        public override void Update()
        {
            // Debug.Log("Update");
            _timer -= Util.Time.DeltaTime;
            _compCover.fillAmount = _timer / EventAttr.MaxDuration;
        }

        public override void Exit()
        {
            // 關閉UI
            _compUI.gameObject.SetActive(false);
            _compBackground.sprite = _origin;
        }
    }
    /*public class QteUIEvent : UIEvent
    {
        private Sprite _origin;
        private readonly QteImageList _imageList;
        private readonly Transform _compUI;
        private readonly Image _compCover;
        private readonly Image _compBackground;
        private QteImageList.CompImage icon;
        public bool Miss { get; set; }
        private float _timer;

        /// 1.自帶傷害事件 或
        /// 2.由呼叫的Skill判斷，移除此項
        public QteUIEvent(string hierarchyPath)
        {
            _imageList = UnityRes.GetQteImageList();
            _compUI = UnityTool.GetComponentByPath<Transform>(hierarchyPath);
            _compBackground = _compUI == null ? null : _compUI.GetFirstComponentInChildren<Image>("Background");
            _compCover = _compUI == null ? null : _compUI.GetFirstComponentInChildren<Image>("Cover");
            CauseEnter = () => !IsEmpty());
            CauseExit = () => Miss); // if keydown or duration.isTimeUp

            if (_compUI == null)
                return;
            _compUI.gameObject.SetActive(false);
        }
        public Func<bool> CauseEnter { get; }
        public Func<bool> CauseExit { get; }
        private bool IsEmpty() =>
            _compBackground == null || _compCover == null;

        public void Invoke(EnumQteSymbol icon)
        {
            Miss = false;
            this.icon = _imageList.Get(icon);
            base.Invoke();
        }

        public float Duration
        {
            get => EventAttr.MaxDuration;
            set => EventAttr.MaxDuration = value;
        }

        protected override void Enter()
        {
            // 顯示UI
            _compUI.gameObject.SetActive(true);
            _origin = _compBackground.sprite;
            _compBackground.sprite = icon.image;
            _compBackground.rectTransform.sizeDelta = icon.size;
            _timer = EventAttr.MaxDuration;
            _compCover.fillAmount = 1;
        }

        protected override void Update()
        {
            _timer -= Time.DeltaTime;
            _compCover.fillAmount = _timer / EventAttr.MaxDuration;
        }

        protected override void Exit()
        {
            // 關閉UI
            _compUI.gameObject.SetActive(false);
            _compBackground.sprite = _origin;
        }
    }*/
}