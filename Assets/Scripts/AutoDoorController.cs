using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class AutoDoorController : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public GameObject monsterBlock;
    public TMP_InputField passwordInput;
    public GameObject passwordUI;
   
    public string correctPassword = "1234";

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    public Vector3 leftOpenOffset = new Vector3(-2f, 0, 0);
    public Vector3 rightOpenOffset = new Vector3(2f, 0, 0);
    public float moveSpeed = 2f;
    public float openTime = 3f; // 문 열리고 유지되는 시간
    private AutoDoorTrigger autoDoorTrigger;

    private bool isMoving = false;
    private CameraControl cameraControl;

    void Start()
    {
        leftClosedPos = leftDoor.position;
        rightClosedPos = rightDoor.position;
        cameraControl = Camera.main.GetComponentInParent<CameraControl>();

        autoDoorTrigger = GetComponent<AutoDoorTrigger>();
    }

    public void CheckPassword()
    {
        if (passwordInput.text == correctPassword)
        {
            StartCoroutine(OpenAndCloseDoor());
            passwordUI.SetActive(false);
            Time.timeScale = 1f;
            cameraControl.UnLockCamera();
            autoDoorTrigger.isActivate = false;
        }
        else
        {
            Debug.Log("틀린 암호");
        }
    }

    public void OnClickBack()
    {
        
        passwordUI.SetActive(false);
        cameraControl.UnLockCamera();
        Time.timeScale = 1f;
    }


    IEnumerator OpenAndCloseDoor()
    {
        Vector3 leftOpenPos = leftClosedPos + leftOpenOffset;
        Vector3 rightOpenPos = rightClosedPos + rightOpenOffset;

        // 문 열기
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            leftDoor.position = Vector3.Lerp(leftClosedPos, leftOpenPos, t);
            rightDoor.position = Vector3.Lerp(rightClosedPos, rightOpenPos, t);
            yield return null;
        }

        Destroy(monsterBlock); // 몬스터 막는 벽 제거
        yield return new WaitForSeconds(openTime);

        // 문 닫기
        /*
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            leftDoor.position = Vector3.Lerp(leftOpenPos, leftClosedPos, t);
            rightDoor.position = Vector3.Lerp(rightOpenPos, rightClosedPos, t);
            yield return null;
        }
        */
    }
}
