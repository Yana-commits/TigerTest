using System;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour, IClickable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private SetSpriteManager spriteManager;
  
    public List<Sides> openWalls = new List<Sides>();

    public bool IsVisited { get; private set; }
    public bool IsTrap { get; set; } = false;
    public bool IsRbbit { get; set; } = false;
    public int X { get; set; }
    public int Y { get; set; }

    public Action OnEnd;

    public void Init(SetSpriteManager _spriteManager,int x,int y)
    {
        spriteManager = _spriteManager;
        X = x;
        Y = y;
    }
    public void Visit()
    {
        IsVisited = true;
    }

    public void ClearLeftWall()
    {
        openWalls.Add(Sides.LeftWall);
    }
    public void ClearFrontWall()
    {
        openWalls.Add(Sides.FrontWall);
    }
    public void ClearRightWall()
    {
        openWalls.Add(Sides.RightWall);
    }
    public void ClearBackWall()
    {
        openWalls.Add(Sides.BackWall);
    }

    public void SetSprite()
    {
        spriteRenderer.sprite = spriteManager.SetSprite(openWalls);
    }
    public void Click(int k,bool IsCreate)
    { 
        this.gameObject.transform.Rotate(0, 0, 90*k);
        ChangeSides(openWalls,k);
       
        if (IsCreate)
        {
            OnEnd?.Invoke();
        }
       
    }
    public void ChangeSides(List<Sides> sides,int k)
    {
        for (var i = 0; i < sides.Count; i++)
        {
            var side = (int)sides[i];
            side =side+k;
            if (side > 3)
            {
                side = side-4;
            }
            sides[i] = (Sides)side;
        }
    }
  
}