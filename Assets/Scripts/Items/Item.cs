using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFile", menuName = "NewItem/item")]
public class Item : ScriptableObject    // ScripteableObject = 데이터 컨테이너 에셋, 동일한 아이템들은 모두 이 정보를 참조한다.
{
    public enum ItemType    // enum은 열거형 데이터, 따로 선언하지 않으면 순서대로 0부터 +1 되는 값들을 가진다.
    {
        Equipment,
        Conmsumables,
        Ingredient,
        Etc
    }
    
    // 에셋 생성시 인스펙터 창에서 보여지는 아이템의 정보들
    public string itemName;     // 아이템 이름
    public ItemType itemType;   // 아이템 유형
    public Sprite itemImage;    // 아이템 이미지 (인벤토리에서의 이미지)
    public GameObject itemPrefab; // 아이템 프리팹 (아이템 생성시 찍어낼 프리팹)
}