using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    public GameObject pausePanel; 
    public GameObject inforPanel;
    public TextMeshProUGUI infoTitleText;
    public TextMeshProUGUI infoContentText;

    void Start()
    {
        pausePanel.SetActive(false);

      
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pausePanel.SetActive(true);
    }



    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene"); 
    }

    public void ShowControls()
    {
        ShowInfo("기믹정보", "몬스터:가까이가면 피해를입음 \n총으로공격가능 \n 터렛:터렛의 사정범위안에들어올시 피해를입음,파괴불가");
    }

    public void ShowObjectInfo()
    {
        ShowInfo("조작법", "W,S:앞뒤 이동    A, D: 회전    Space: 점프 \n마우스 좌클릭:총알발사    R:장전 ");

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnClickBack()
    {
        pausePanel.SetActive(true);
        inforPanel.SetActive(false);
    }


    void ShowInfo(string title, string content)
    {
        pausePanel.SetActive(false);
        inforPanel.SetActive(true);
        infoTitleText.text = title;
        infoContentText.text = content;
    }
}
