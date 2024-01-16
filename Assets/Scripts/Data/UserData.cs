using System;

[Serializable]
public class UserData
{
    public int level;
    public int index;
    public int coins;
    public int bombNum;
    public int bombSteps;
    public int efforts;
    public float musicVol;
    public float soundVol;
    public UserData()
    {
        level = 1;
        coins = 0;
        index = 1;
        bombNum = 1;
        bombSteps = 2;
        efforts = 1;
        musicVol = 0.75f;
        soundVol = 0.75f;
    }
}
