using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using System.Collections;

public class UIHelper
{
    public static IEnumerator DisplayScores(Dictionary<TMP_Text, float> displayItems, float startValue = 0f, Action actionAfterFinish = null)
    {
        float duration = 0.4f;// Время анимации для каждого параметра
        float currentIndex = startValue;
        // Обрабатываем каждый параметр по очереди
        foreach (var field in displayItems)
        {
            float currentValue = 0f;
            float targetValue = field.Value;
            float timeElapsed = 0f;
            if (targetValue == 0)
            {
                currentIndex++;
                continue;
            }

            while (timeElapsed < duration)
            {
                currentValue = Mathf.Lerp(startValue, targetValue, timeElapsed / duration);
                field.Key.text = Mathf.RoundToInt(currentValue).ToString();
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            field.Key.text = Mathf.RoundToInt(targetValue).ToString();

            currentIndex++;

            if (currentIndex >= displayItems.Count)
                break;
        }
        actionAfterFinish?.Invoke();
    }
}
