using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants 
{
    public static bool NoDamage;
    public static bool OpenGame;
    public static void SetContant(string name, int num)//To Set the player prefs  setcontant(gem,5);
    {
         PlayerPrefs.SetInt(name, num);
    }
    public static int GetContant(string name)//To Get the player prefs
    {
        return PlayerPrefs.GetInt(name);
    }

    public static void ChangeConstant(string name, int change)
    {
        PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + change);
    }
  
}
