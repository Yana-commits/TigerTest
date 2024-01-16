

using UnityEngine;


public class LevelParameters : MonoBehaviour
{
    private int bombNum;
    private int bombSteps;
    private int index;
    private int level;
    public LevelSettings settings { get; set; }
    public void NextLevel(UserData userData)
    {
        bombNum = userData.bombNum;
        bombSteps = userData.bombSteps;
        index = userData.index;
        level = userData.level;

        bombSteps--;
        level++;

        if (level >= 7)
        {
            SaveSystem.instance.Resest();
        }
        else 
        {
            if (bombSteps < 0)
            {
                bombNum++;
                index++;
                bombSteps = 2;
            }

            SaveSystem.instance.SetLevel(level, index, bombNum, bombSteps);
        }

        SaveSystem.instance.SaveUserData();
    }
}
