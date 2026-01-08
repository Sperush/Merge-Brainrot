using UnityEngine;

public class Unit : MonoBehaviour
{
    public int gridX;
    public int gridY;

    public int level;
    public int hp;
    public int atk;
    public string type;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public UnitVisualData[] visuals;

    void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        UpdateVisual();
    }

    public void LevelUp()
    {
        level++;
        hp *= 2;
        atk *= 2;

        UpdateVisual();
    }

    void UpdateVisual()
    {
        foreach (var v in visuals)
        {
            if (v.level == level)
            {
                spriteRenderer.sprite = v.sprite;
                transform.localScale = v.scale;
                return;
            }
        }
    }

    public void SetGridPos(int x, int y)
    {
        gridX = x;
        gridY = y;
    }
}
