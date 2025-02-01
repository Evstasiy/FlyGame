using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUIItem : MonoBehaviour
{
    [SerializeField]
    private Animation animation;

    [SerializeField]
    private TMP_Text MainText;

    [SerializeField]
    private TMP_Text DescriptionText;

    [SerializeField]
    private Image IconImg;

    [SerializeField]
    private Image LockImg;

    [SerializeField]
    private Image MainBackgroudImg;

    [SerializeField]
    private Color colorForUnlock;
    [SerializeField]
    private Color colorForLock;

    private AchievementModel model;

    public void SetModel(AchievementModel model)
    {
        this.model = model;
        MainText.text = model.Name;
        IconImg.sprite = Resources.Load<Sprite>(model.IconPath);

        if (DescriptionText != null)
        {
            DescriptionText.text = model.Description;
        }
        if(LockImg != null)
        {
            SetLock(true);
        }
    }
    
    public void SetLock(bool isLock)
    {
        LockImg.gameObject.SetActive(isLock);
        MainBackgroudImg.color =(isLock) ? colorForLock : colorForUnlock;
    }
    public int AchievementId => model.Id;
}