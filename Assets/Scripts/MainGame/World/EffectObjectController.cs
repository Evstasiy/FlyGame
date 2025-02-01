using Assets.Scripts.MainGame.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectObjectController : MonoBehaviour
{
    [SerializeField]
    private EffectObjectModel effectObjectModel;

    [SerializeField]
    private TMP_Text effectText;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Image effectIcon;

    private float effectTimeToEnd = 0;
    public float effectCountToEnd = 0;
    Dictionary<EffectDataEnum, float> effectDataDict = new Dictionary<EffectDataEnum, float>();

    public delegate void RemoveEffect(EffectObjectModel effect);
    public event RemoveEffect isEffectRomove;

    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    public EffectObjectModel Model
    {
        get 
        { 
            return effectObjectModel; 
        } 
    }
    private void Start()
    {
        animator.Play("UIScaleOpen", -1, 0f);
    }

    void FixedUpdate()
    {
        if (isPaused)
        {
            return;
        }

        foreach (var effectData in effectDataDict)
        {
            if (effectData.Key == EffectDataEnum.Speed)
            {
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(effectDataDict[EffectDataEnum.Speed]);
            } 
            else if (effectData.Key == EffectDataEnum.Mobility)
            {
                GlobalPlayerInfo.playerInfoModel.AddPlayerMobility(effectDataDict[EffectDataEnum.Mobility]);
            }
        }
        if (effectObjectModel.IsEffectNotTime)
        {
            effectText.text = effectCountToEnd.ToString("0");
        } 
        else
        {
            effectTimeToEnd -= Time.deltaTime;
            effectText.text = effectTimeToEnd.ToString("0.0");
        }
    }

    public void SetEffectObjectModel(EffectObjectModel effectObjectModel)
    {
        this.effectObjectModel = effectObjectModel;
        effectTimeToEnd = effectObjectModel.GetFinalEffectLifeTime();
        effectCountToEnd += effectObjectModel.GetFinalEffectCountToDisabled();
        effectIcon.sprite = effectObjectModel.EffectIconImg;

        foreach (var effect in effectObjectModel.EffectData)
        {
            effectDataDict.Add(effect.Key, effect.Value);
        }
    }

    public void NeedRemoveEffect()
    {
        isEffectRomove?.Invoke(effectObjectModel);
    }

    public void RestartEffect()
    {
        effectTimeToEnd = effectObjectModel.GetFinalEffectLifeTime();
        effectCountToEnd = effectObjectModel.GetFinalEffectCountToDisabled();
        animator.Play("UIScaleOpen", -1, 0f);
    }

    private void OnDestroy()
    {
        foreach (var effectData in effectDataDict)
        {
            if (effectData.Key == EffectDataEnum.Mobility)
            {
                GlobalPlayerInfo.playerInfoModel.SetDefaultPlayerMobility();
            }
        }
    }
}
