using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    private UserData userData;
    public Action<UserData> OnLoad;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(this);

        DontDestroyOnLoad(this);
    }
   
    public void LoadUserData()
    {
        string usersJson = GamePlayerPrefs.UserSaver;
        if (string.IsNullOrEmpty(usersJson))
        {
            userData = new UserData();
            SaveUserData();
        }
        else
            userData = JsonConvert.DeserializeObject<UserData>(usersJson);

        OnLoad?.Invoke(userData);
    }
    public UserData GetUser()
    { 
       return userData;
    }

    public void Resest()
    {
        userData = new UserData();
        SaveUserData();
    }
    public void Lost()
    {
        userData.efforts--;

        if (userData.efforts < 0)
            Resest();

        SaveUserData();
    }
    public void SetAudio(float musicVol,float soundVol)
    { 
        userData.musicVol = musicVol;
        userData.soundVol = soundVol;
        SaveUserData();
    }
    public void SaveUserData()
    {
        GamePlayerPrefs.UserSaver = JsonConvert.SerializeObject(userData);
    }
    public void SetLevel(int _level, int _index, int _bombNum, int _bombSteps)
    {
        userData.level = _level;
        userData.index = _index;
        userData.bombNum = _bombNum;
        userData.bombSteps = _bombSteps;
    }
    private void OnApplicationQuit()
    {
        SaveUserData();
    }
}
