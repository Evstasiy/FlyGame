using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectObject", menuName = "ScriptableObjects/EffectObject", order = 1)]
public class EffectObjectModel : ScriptableObject
{
    public EffectObjectController EffectUIObjectController;

    public EffectEnum EffectType;
    public Sprite EffectIconImg;
    public string EffectName;

    public bool IsEffectNotTime;
    public float EffectBaseLifeTime;
    public int EffectBaseCountToDisabled;

    [System.NonSerialized]
    private int UpdateEffectCountToDisabled;
    [System.NonSerialized]
    private float UpdateEffectLifeTimeSec;

    public List<EffectData> EffectData = new List<EffectData>();

    public EffectObjectController InstantiateEffectToPosition(Transform parentTransform)
    {
        var effectObject = Instantiate(EffectUIObjectController, parentTransform.position, Quaternion.identity, parentTransform);
        effectObject.SetEffectObjectModel(this);

        return effectObject;
    }

    public void AddUpdateEffectLifeTimeSec(int addEffectLifeTimeSec)
    {
        UpdateEffectLifeTimeSec = addEffectLifeTimeSec;
    }
    
    public void AddEffectCountToDisabled(int addEffectCount)
    {
        UpdateEffectCountToDisabled += addEffectCount;
    }
    
    public int GetFinalEffectCountToDisabled()
    {
        return EffectBaseCountToDisabled + UpdateEffectCountToDisabled;
    }
    
    public float GetFinalEffectLifeTime()
    {
        return EffectBaseLifeTime + UpdateEffectLifeTimeSec;
    }
}

[System.Serializable]
public class EffectData
{
    /// <summary>
    /// Ключ переменной для эффекта
    /// </summary>
    public EffectDataEnum Key;

    /// <summary>
    /// Значение для эффекта
    /// </summary>
    public float Value;
}

public enum EffectDataEnum
{
    Speed,
    Mobility
}
