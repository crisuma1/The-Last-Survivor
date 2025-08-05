using UnityEngine;

// 아이템 타입들이 반드시 구현해야하는 인터페이스
public interface IItem
{
    string Name { get; }           // 이름을 읽는 기능만 필요
    Sprite Icon { get; }           // 아이콘 읽는 기능만 필요
    int Quantity { get; set; }     // 수량은 읽고 쓰는 기능 필요
    void Use(GameObject target);   // 아이템을 사용하는 기능
}