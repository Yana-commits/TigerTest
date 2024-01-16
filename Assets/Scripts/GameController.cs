using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Character character;
    [SerializeField]
    private Hud hud;
    [SerializeField]
    private MazeGenerator mazeGenerator;
    [SerializeField]
    private SpritesMenu spritesMenu;
    [SerializeField]
    private SetSpriteManager spriteManager;
    [SerializeField]
    private PahtChecker pahtChecker;
    [SerializeField]
    private LevelParameters levelParameters;
    [SerializeField]
    private Grabber grabber;
    [SerializeField]
    private Audio m_audio = new Audio();

    [Header("Settings")]
    [SerializeField]
    private int mazeWidth;
    [SerializeField]
    private int mazeDepth;
    [SerializeField]
    private int rewardCoins;

    private UserData userData;
    private int userCoins;

    public Audio Audio { get => m_audio; set => m_audio = value; }

    private void Awake()
    {
        Audio.SourceMusic = gameObject.AddComponent<AudioSource>();
        Audio.SourceRandomPitchSFX = gameObject.AddComponent<AudioSource>();
        Audio.SourceSFX = gameObject.AddComponent<AudioSource>();

        NewLevel();
    }

    private void NewLevel()
    {
        SaveSystem.instance.OnLoad += LoadLevelFirst;
        SaveSystem.instance.LoadUserData();
        hud.popUpPanel.OnStartGame += StartGame;
        hud.popUpPanel.menuPanel.OnReset += NextLevel;
        hud.OnPauseGame += PauseGame;
        StartGame();
    }
    private void LoadLevelFirst(UserData _userData)
    {
        userData = _userData;
        userCoins = userData.coins;
        Audio.sfxVolume = userData.soundVol;
        Audio.musicVolume = userData.musicVol;
        hud.Init(userCoins.ToString(), userData.level.ToString());
        spriteManager.Init(spritesMenu);
        mazeGenerator._goPathChecker += LoadLevelSecond;
        mazeGenerator.Init(spriteManager, mazeWidth, mazeDepth, CheckPath, _userData);
        SaveSystem.instance.OnLoad -= LoadLevelFirst;
    }
    private void LoadLevelSecond()
    {
        pahtChecker.Init(mazeWidth, mazeDepth, mazeGenerator._mazeGrid);
        pahtChecker.OnCheckedPath += TheEnd;
        Audio.PlayMusic(true);
        hud.HideLoadng(true);
        mazeGenerator._goPathChecker -= LoadLevelSecond;
    }
    private void CheckPath()
    {
        Audio.PlaySound("Click");
        pahtChecker.CheckPath(mazeGenerator.mazeCells);
    }
    private void TheEnd(Cell cell, List<Vector3> way)
    {
        PauseGame();
        pahtChecker.GameState = GameState.Done;
        var firstStep = new Vector3(way[0].x,character.gameObject.transform.position.y,0);
        way.Insert(0, firstStep);

        if (cell.IsRbbit == true)
        {
            Debug.Log("U win!");
            levelParameters.NextLevel(userData);
            StartCoroutine(EndActons(way, "Victory", EndStatements.Victory, 1));
            
        }
        if (cell.IsTrap == true)
        {
            Debug.Log("Game over");
            SaveSystem.instance.Lost();
            StartCoroutine(EndActons(way, "Drag", EndStatements.GameOver, -1));
        }
    }
    private IEnumerator EndActons(List<Vector3> way, string sound, EndStatements statement, int k)
    {
        character.DoCharPath(way.ToArray(),statement);
        yield return new WaitForSeconds(4f);

        Audio.PlaySound(sound);
        hud.ShowLoading(statement);
        yield return new WaitForSeconds(2f);

        EndActions(sound, statement, k);
        NextLevel();
    }
    private void EndActions(string sound,EndStatements statement,int k)
    {
        userCoins = userCoins + rewardCoins*k;

        if (userCoins < 0)
            userCoins = 0;

        userData.coins = userCoins;
        SaveSystem.instance.SaveUserData();

        character.TakeStartPlace();
    }
    private void NextLevel()
    {
        hud.popUpPanel.menuPanel.OnReset -= NextLevel;
        hud.popUpPanel.OnStartGame -= StartGame;
        hud.OnPauseGame -= PauseGame;
        pahtChecker.OnCheckedPath -= TheEnd;
       
        StartCoroutine(nameof(LoadScene));
    }
    private IEnumerator LoadScene()
    {
        
        mazeGenerator.DestroyCells();
        yield return new WaitForSeconds(2f);
        mazeGenerator.RemoveParent();
        yield return new WaitForSeconds(2f);
        hud.HideLoadng(false);
        NewLevel();
    }
    private void StartGame()
    {
        grabber.GameState = GameState.Plaing;
    }
    private void PauseGame()
    {
        grabber.GameState = GameState.Done;
    }
}
