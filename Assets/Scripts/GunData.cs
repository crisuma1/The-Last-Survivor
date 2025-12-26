using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/GunData", fileName = "Gun Data")]
public class GunData : ScriptableObject
{
    public FireState GunState; //단발총인지연발총인지구분
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리

    public float damage = 25; // 공격력

    public int startAmmoRemain = 100; // 처음에 주어질 전체 탄약
    public int magCapacity = 25; // 탄창 용량

    public float timeBetFire = 0.12f; // 총알 발사 간격
    public float reloadTime = 1.8f; // 재장전 소요 시간

    public float defaultFOV = 60f; //기본줌
    public float adsFOV = 40f; //견착줌
    public float scopeFOV = 10f; //확대줌
    public string recoilTriggerName; //총마다다른반동애니메이션의이름
    public float scopeShakeStrength;//스코프상태일때카메라흔들림강도
    public float scopeShakeDuration;//스코프상태일때카메라흔들림시간
    public float scopeShakeFrequency;//스코프상태일때카메라흔들림횟수

}