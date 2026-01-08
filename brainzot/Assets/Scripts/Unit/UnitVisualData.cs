using UnityEngine;

[CreateAssetMenu(menuName = "Units/Unit Visual Data")]
public class UnitVisualData : ScriptableObject
{
    public int level;
    public Sprite sprite;
    public Vector3 scale = Vector3.one;
}
