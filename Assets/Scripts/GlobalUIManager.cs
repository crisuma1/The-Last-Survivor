using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GlobalUIManager : MonoBehaviour
{
    public static GlobalUIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GlobalUIManager>();
            }
            return m_instance;
        }
    }

    private static GlobalUIManager m_instance;

    public Text ammoText; // ź�� ǥ�ÿ� �ؽ�Ʈ
    public GameObject gameoverUI; // ���� ������ Ȱ��ȭ�� UI 


    //ź�� �ؽ�Ʈ ����
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    // ���� ���� UI Ȱ��ȭ
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }
    // ���� �����
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
