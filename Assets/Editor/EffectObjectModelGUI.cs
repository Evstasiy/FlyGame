using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectObjectModel))]
public class EffectObjectModelGUI : Editor
{
    SerializedProperty isStaticProp;
    SerializedProperty countProp;
    SerializedProperty timeProp;
    void OnEnable()
    {
        isStaticProp = serializedObject.FindProperty("IsEffectNotTime");
        countProp = serializedObject.FindProperty("EffectBaseCountToDisabled");
        timeProp = serializedObject.FindProperty("EffectBaseLifeTime");
    } 

    public override void OnInspectorGUI()
    {
        EffectObjectModel myScriptableObject = (EffectObjectModel)target;

        DrawPropertiesExcluding(serializedObject, 
            nameof(myScriptableObject.IsEffectNotTime),
            nameof(myScriptableObject.EffectBaseCountToDisabled),
            nameof(myScriptableObject.EffectBaseLifeTime)
            );

        EditorGUILayout.PropertyField(isStaticProp);

        if (isStaticProp.boolValue)
        {
            EditorGUILayout.PropertyField(countProp);
        } else
        {
            EditorGUILayout.PropertyField(timeProp);
        }


        serializedObject.ApplyModifiedProperties();
    }
}
