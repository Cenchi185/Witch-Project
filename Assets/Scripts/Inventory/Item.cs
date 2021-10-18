using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFile", menuName = "NewItem/item")]
public class Item : ScriptableObject    // ScripteableObject = ������ �����̳� ����, ������ �����۵��� ��� �� ������ �����Ѵ�.
{
    public enum ItemType    // enum�� ������ ������, ���� �������� ������ ������� 0���� +1 �Ǵ� ������ ������.
    {
        Equipment,
        Conmsumables,
        Ingredient,
        Etc
    }
    
    // ���� ������ �ν����� â���� �������� �������� ������
    public string itemName;     // ������ �̸�
    public ItemType itemType;   // ������ ����
    public Sprite itemImage;    // ������ �̹��� (�κ��丮������ �̹���)
    public GameObject itemPrefab; // ������ ������ (������ ������ �� ������)
}