using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UIControls
{
    public class UITranslatorController : MonoBehaviour
    {
        [SerializeField]
        private List<UiTextTranslatorItem> uiItems;

        [SerializeField]
        private List<UiTextMeshTranslatorItem> uiMashItems;

        [SerializeField]
        private List<UiTMPTranslatorItem> uiMashProItems;

        private DataBaseRepository dataBaseRepository => DataBaseRepository.dataBaseRepository;

        void Start ()
        {
            dataBaseRepository.IsReloadLocalizationEvent += ReloadTextInUI;
            ReloadTextInUI();

        }
        private void OnDestroy()
        {
            dataBaseRepository.IsReloadLocalizationEvent -= ReloadTextInUI;
        }

        public void ReloadTextInUI()
        {
            var uiItemsInRepo = dataBaseRepository.UITranslatorRepos.allItems;
            foreach (var uiItem in uiItems)
            {
                if (uiItemsInRepo.TryGetValue(uiItem.UiTextKey, out UIItem item))
                {
                    uiItem.UiText.text = item.Description;
                }
            }
            
            foreach (var uiItem in uiMashItems)
            {
                if (uiItemsInRepo.TryGetValue(uiItem.UiTextKey, out UIItem item))
                {
                    uiItem.UiText.text = item.Description;
                }
            }
            
            foreach (var uiItem in uiMashProItems)
            {
                if (uiItemsInRepo.TryGetValue(uiItem.UiTextKey, out UIItem item))
                {
                    uiItem.UiText.text = item.Description;
                }
            }
        }
    }
}
