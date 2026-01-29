using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class GunTypeIcon
{
    public GunType type;
    public Image icon;
}

public class ActiveGunTypeUI : MonoBehaviour
{
    [SerializeField] private List<GunTypeIcon> gunTypeIcons;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        PlayerShooter.OnChange += UpdateUI;
    }

    private void OnDisable()
    {
        PlayerShooter.OnChange -= UpdateUI;
    }



    public void UpdateUI(GunType type)
    {
        foreach (var entry in gunTypeIcons)
        {
            if (entry.icon == null) continue;

            entry.icon.gameObject.SetActive(entry.type == type);
        }
    }
}
