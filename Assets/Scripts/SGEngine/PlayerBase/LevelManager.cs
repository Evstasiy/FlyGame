using System;
using System.Collections.Generic;
using System.Linq;

public class LevelManager
{
    //предыдущий опыт прибавляется к следующему
    private List<int> experienceThresholds = new List<int> {
        0,
        30,
        230,
        1030,
        2630,
        4930,
        8830,
        15030,
        25130,
        41430,
        67830,
        110530,
        179630,
        291430,
        472330,
        765030,
        1238630,
        2004930,
        3244830,
        5251030,
        8497130
    };

    public int GetLevelInExperience(int experience)
    {
        int left = 0;
        int right = experienceThresholds.Count - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;

            if (experience >= experienceThresholds[mid] && (mid == experienceThresholds.Count - 1 || experience < experienceThresholds[mid + 1]))
            {
                return mid + 1;
            } else if (experience < experienceThresholds[mid])
            {
                right = mid - 1;
            } else
            {
                left = mid + 1;
            }
        }
        return 0;
    }

    public int GetPrevExperienceInLvl(int lvl)
    {
        if(lvl == 0) 
        { 
            return 0;
        } 
        else
        {
            return experienceThresholds[lvl - 1];
        }
    }

    public int GetExperienceInLvl(int lvl)
    {
        if (lvl < 0 || lvl >= experienceThresholds.Count)
        {
            return experienceThresholds.Max();
            //throw new ArgumentException("Уровень должен быть в допустимом диапазоне.");
        }

        return experienceThresholds[lvl];
    }


}
