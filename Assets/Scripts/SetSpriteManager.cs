using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpriteManager : MonoBehaviour
{
    private SpritesMenu spritesMenu;
    public void Init(SpritesMenu _spritesMenu)
    {
        spritesMenu = _spritesMenu;
    }
    public Sprite SetSprite(List<Sides> strings)
    {
        Sprite sprite = null;

        if (strings.Count <= 2)
        {
            if (strings.Contains(Sides.LeftWall) && strings.Contains(Sides.RightWall))
            {
                sprite = spritesMenu.leftRight[0];
            }
            else if (strings.Contains(Sides.FrontWall) && strings.Contains(Sides.BackWall))
            {
                sprite = spritesMenu.backFront[0];
            }
            else if (strings.Contains(Sides.BackWall) && strings.Contains(Sides.LeftWall))
            {
                sprite = spritesMenu.backLeft[0];
            }
            else if (strings.Contains(Sides.BackWall) && strings.Contains(Sides.RightWall))
            {
                sprite = spritesMenu.rightBack[0];
            }
            else if (strings.Contains(Sides.LeftWall) && strings.Contains(Sides.FrontWall))
            {
                sprite = spritesMenu.leftFront[0];
            }
            else if (strings.Contains(Sides.FrontWall) && strings.Contains(Sides.RightWall))
            {
                sprite = spritesMenu.frontRight[0];
            }
            else
            {
                sprite = spritesMenu.blank;
            }
        }
        else 
        {
            if (strings.Contains(Sides.LeftWall) && strings.Contains(Sides.RightWall) && strings.Contains(Sides.FrontWall))
            {
                sprite = spritesMenu.leftFrontRight;
            }
            else if (strings.Contains(Sides.LeftWall) && strings.Contains(Sides.RightWall) && strings.Contains(Sides.BackWall))
            {
                sprite = spritesMenu.leftBackRight;
            }
            else if (strings.Contains(Sides.LeftWall) && strings.Contains(Sides.FrontWall) && strings.Contains(Sides.BackWall))
            {
                sprite = spritesMenu.backLeftFront;
            }
            else if (strings.Contains(Sides.RightWall) && strings.Contains(Sides.FrontWall) && strings.Contains(Sides.BackWall))
            {
                sprite = spritesMenu.backRightFront;
            }
            else 
            {
                sprite = spritesMenu.blank;
            }
        }
       

        return sprite;
    }
}
