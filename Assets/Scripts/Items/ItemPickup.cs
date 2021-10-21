using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;   // Scriptable 로 만들어진 item.cs 는 자체로써 컴포넌트로 참조가 불가능하기 때문에 할당 받을수 있는 컴포넌트를 새로 마련
}
