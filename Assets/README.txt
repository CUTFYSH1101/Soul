如果腳本出現錯誤，Unity 編輯器會因為檢查到出錯而無法進入運行模式，
這時可以在項目視圖中新建資料夾 WebplayerTemplates，然後將出錯的腳本拖入此資料夾下，
所有位於該資料夾下的檔都會被識別為一般檔從而不會當作腳本被編譯，這樣就可以運行遊戲了。

作者：siki老师
链接：https://www.zhihu.com/question/306013515/answer/553918435
来源：知乎
著作权归作者所有。商业转载请联系作者获得授权，非商业转载请注明出处。

Sandbox = test的意思，是unity保留資料夾(Editor、Resources)

---
日期：2021.10.11
統一命名空間，concrete、sub合為sub
修改EventLib資料夾結構，抽出Sub、修改命名
修改Test.Scene資料夾結構，抽出Sub、修改命名
將UnityCoroutine.cs所在資料夾移至Game，並取名為Coroutine

再次修改IEvent_n
    triggerable => IsOpen && FilterIn
    統一FilterOut與ToInterrupt，合為ToInterrupt
    
修正專案內NormalMonster箭頭參數，避免atkNormal.cs的isTag("Attack")恆為false的異常

---
日期：2021.10.07
修改IEvent成員參數名稱
修改IEvent子類Interrupt相關
修改res相關

新增enum random方法，並修正部分沒有none的enum，修正qte骰形狀的方法
日期：2021.10.08
修正
PIG動畫
事件系統重構...修正interrupt...修正名稱
菜單名稱修正CreatureBehavior
IEvent4、IEventBuilder4底層全修改，可正常執行
把角色圖片替換成prefab的

出現新問題
IEvent3、FinalAct設計方式與IEvent4不同 (對IE4來說，程式的終點會執行FinalAct)

修正IEvent各事件名稱(Act1,Act2,Act3)(toInterrupt)

---
日期：2021.10.05
修正：
    gamepad判空，修正錯誤
    重構UnityCoroutine，CreateEvent2 3 4彙整為一個方法了，允許1個event有超過5種以上action
    重構三層地板
    修正三層地板xml，使更簡潔易懂
    修正資料結構，合併AnimAndAudio與圖片為Res;修改CreatureStrategy為CreatureAI
    
---
日期：2021.10.02
修正：
    tooltip（來自unity）為description（c#原生），降低unityEngine依賴
    input資料結構
    input部分類別名稱
    AnimAndAudioFacade（原：AnimAndAudioSystem）
    CreatureAnimInterface（原：CreatureAnimManager）
修正註解：
    jumpAttack註解
    knockback註解
