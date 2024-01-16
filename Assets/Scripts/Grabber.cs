
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private Vector3 target;
    private int k = 1;
    private GameState gameState = GameState.Plaing;

    public GameState GameState { get => gameState; set => gameState = value; }

    void Start()
    {
        target = transform.position;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f);

            if (hit && GameState == GameState.Plaing)
            {
                var cube = hit.collider.gameObject.GetComponent<Cell>();

                if (cube != null)
                {
                    cube.Click(k,true);
                }
            }
        }
    }
}
