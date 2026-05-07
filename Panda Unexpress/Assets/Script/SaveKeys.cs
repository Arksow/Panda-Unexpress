using UnityEngine;

public static class SaveKeys
{
    public const string CURRENT_PLAYER = "CurrentPlayer";

    public static string GetHighScoreKey(string playerName)
    {
        return $"{playerName}_HighScore";
    }

    public static string GetTotalMoneyKey(string playerName)
    {
        return $"{playerName}_TotalMoney";
    }
}