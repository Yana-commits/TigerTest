using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject firstPos;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float currX;
    private GameState gameState = GameState.Plaing;

    private void Start()
    {
        currX = this.transform.position.x;
    }

    private void Update()
    {
        if (gameState == GameState.Done)
        {
            CheckDir();
        }
    }
    public void Walk(bool isWalking)
    {
        animator.SetBool("walk", isWalking);
    }
    public void Dead(bool isDead)
    {
        animator.SetBool("dead", isDead);
    }
    public void Win(bool isWin)
    {
        animator.SetBool("win", isWin);
    }
    public void Idle(bool isIdle)
    {
        animator.SetBool("return", isIdle);
    }
    public void DoCharPath(Vector3[] way, EndStatements statement)
    {
        gameState = GameState.Done;
        Idle(false);
        Walk(true);
        this.gameObject.transform.DOPath(way, 3f).OnComplete(() => FaceTarget(statement));
    }
    private void FaceTarget(EndStatements statement)
    {
        Walk(false);

        if (statement == EndStatements.Victory)
        {
            Win(true);
        }
        else
            Dead(true);
    }

    public void TakeStartPlace()
    {
        Idle(true);
        Win(false);
        Dead(false);

        this.gameObject.transform.DOMove(firstPos.transform.position, 1);
        this.gameObject.transform.position = firstPos.transform.position;
        gameState = GameState.Plaing;
    }
    private void CheckDir()
    {
        var pos = this.gameObject.gameObject.transform.position.x;
        if (pos > currX)
        {
            spriteRenderer.flipX = false;
            currX = pos;
        }
        else if (pos < currX)
        {
            spriteRenderer.flipX = true;
            currX = pos;
        }

    }
}
