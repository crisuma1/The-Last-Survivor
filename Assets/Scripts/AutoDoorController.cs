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
    public float openTime = 3f; // �� ������ �����Ǵ� �ð�

    private bool isMoving = false;

    void Start()
    {
        leftClosedPos = leftDoor.position;
        rightClosedPos = rightDoor.position;
    }

    public void CheckPassword()
    {
        if (passwordInput.text == correctPassword)
        {
            StartCoroutine(OpenAndCloseDoor());
            passwordUI.SetActive(false);
        }
        else
        {
            Debug.Log("Ʋ�� ��ȣ");
        }
    }

    IEnumerator OpenAndCloseDoor()
    {
        Vector3 leftOpenPos = leftClosedPos + leftOpenOffset;
        Vector3 rightOpenPos = rightClosedPos + rightOpenOffset;

        // �� ����
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            leftDoor.position = Vector3.Lerp(leftClosedPos, leftOpenPos, t);
            rightDoor.position = Vector3.Lerp(rightClosedPos, rightOpenPos, t);
            yield return null;
        }

        Destroy(monsterBlock); // ���� ���� �� ����
        yield return new WaitForSeconds(openTime);

        // �� �ݱ�
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            leftDoor.position = Vector3.Lerp(leftOpenPos, leftClosedPos, t);
            rightDoor.position = Vector3.Lerp(rightOpenPos, rightClosedPos, t);
            yield return null;
        }
    }
}
