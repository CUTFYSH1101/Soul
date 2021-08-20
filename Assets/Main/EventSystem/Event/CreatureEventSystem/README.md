Unity Game Framework
=====


| 以下僅僅是概括，主要還仰賴Creature、Event

0. 實例化以下CreatureThreadSystem 與 CreatureEvent，兩者必須傳入觀測對象Creature
    - new CreatureThreadSystem(Creature);
    - new NormalAttack(Creature);
1. 在 UnityEngine.Update 中呼叫 CreatureThreadSystem.Update 方法
2. 呼叫旗下任一個 CreatureEvent子類 的 Invoke
3. 如此，第2.項的事件就會被排入1.的事件隊列中，根據順位執行原本的事件

Kaohsiung, Taiwan, ROC
