using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleUIManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject infoPanel;
    public TextMeshProUGUI infoTitleText;
    public TextMeshProUGUI infoContentText;

    public void OnClickStart()
    {
        SceneManager.LoadScene("real scene");
    }

    public void OnClickGoal()
    {
        ShowInfo("게임 목표", "SCF에서 몬스터들을 무찌르고 이곳을탈출한다!");
    }

    public void OnClickControl()
    {
        ShowInfo("조작법", "W,S:앞뒤 이동    A, D: 회전    Space: 점프 \n마우스 좌클릭:총알발사    R:장전 ");
    }

    public void OnClickResolution()
    {
        ShowInfo("해상도", "권장 해상도: 1920x1080");
    }

    public void OnClickAbout()
    {
        ShowInfo("제작자", "제작: 박민준");
    }

    public void OnClickBack()
    {
        mainPanel.SetActive(true);
        infoPanel.SetActive(false);
    }

    void ShowInfo(string title, string content)
    {
        mainPanel.SetActive(false);
        infoPanel.SetActive(true);
        infoTitleText.text = title;
        infoContentText.text = content;
    }
}
