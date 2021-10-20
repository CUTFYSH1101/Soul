# Blood System

##### Author : HemyO
##### Version : 0.3.0

---

### How to check the elements the game use?
1. Open the **Prefabs** in Assets Folders.
2. There' re two folder inside, open the **Registration**.
3. Drag the prefab to an empty game object at hierarchy in unity enegine.
4. After dragging, it' s all complete.
  
* If the data slot of Registration List is empty, please drag **BloodElementsRegistrationList.asset** (At **Assets** Folder) into the slot.
* If the **BloodElementsRegistrationList.asset** slots are still empty, please drag the specified objects to the specified slots.
  
![](https://cdn.discordapp.com/attachments/795499230369808384/894868873185230868/unknown.png)
  
### What is BloodLoader?
BloodLoader is the class which link and manage the Registration List, to offer all clients object to copy. Normally, we do not need to modify it.
  
### What is BloodHandler?
BloodHandler is the class which control the blood data of client object, there' re some members you should know.
  
#### Engine Members
* `bloodLoad` : In engine, you should pull the **Registration List** object into the slot.
* `bloodData` : In engine, you can configure the blood counts and choose elements in each index.
  
#### Private Members
* `_bloodQueue` : Store the game object which will show above the client objects.
* `_posArr` : Store the Position Data to use at the position configuring of `RectTransform`.
  
#### Private Methods
* `TranslateDataToObject ( ) -> void` : According the array of data, copy the object from `bloodLoad` and configure its position from `_posArr`.
* `PosAlgo ( ) -> Vector2[]` : Calc the position data and return Vector2 array.

#### Public Methods
* `DiscardBlood ( int ) -> bool` : Repeat the blood elements dequeuing, and modify the position.
* `PenerateBlood ( BloodType) -> bool` : Ignore the common rule to discard specified elements.