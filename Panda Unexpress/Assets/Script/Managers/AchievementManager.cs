using System.Collections.Generic;
using UnityEngine;
using System;

public enum AchievementName
{
    FirstScoop
}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    private Dictionary<AchievementName, bool> achievements = new Dictionary<AchievementName, bool>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadAchievements()
    {
        foreach (AchievementName achievement in System.Enum.GetValues(typeof(AchievementName)))
        {
            bool unlocked = PlayerPrefs.GetInt(achievement.ToString(), 0) == 1;
            achievements[achievement] = unlocked;
        }
    }

    public void UnlockAchievement(AchievementName achievement)
    {
        if (achievements.ContainsKey(achievement) && achievements[achievement])
        {
            return;
        }

        achievements[achievement] = true;
        PlayerPrefs.SetInt(achievement.ToString(), 1);
        PlayerPrefs.Save();
    }

    public bool IsAchievementUnlocked(AchievementName achievement)
    {
        if (achievements.ContainsKey(achievement))
        {
            return achievements[achievement];
        }
        return false;
    }
}