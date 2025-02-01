using System;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.UIControls
{
    [Serializable]
    public class UiTextTranslatorItem
    {
        public Text UiText; 
        public string UiTextKey;
    }
    
    [Serializable]
    public class UiTextMeshTranslatorItem
    {
        public UnityEngine.TextMesh UiText; 
        public string UiTextKey;
    }
    
    [Serializable]
    public class UiTMPTranslatorItem
    {
        public TMP_Text UiText; 
        public string UiTextKey;
    }
}
