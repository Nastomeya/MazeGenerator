using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableData/Random/ObjectsSelector", fileName = "ObjectsSelectorSetting")]
public class ObjectsSelectorSetting : ScriptableObject
{
    [SerializeField] private int frequentlyUsedTimes = 4;
    [SerializeField] private int appearedRatePerTile = 4;
    
    ///<summary>This is how frequently the 1st object appear in the amount of the rest objects</summary>
    internal int FrequentlyUsedTimes { get { return frequentlyUsedTimes; } }
    ///<summary>The hieher,  the less chance to show up, "1" meaning to show in everything single tile </summary>
    internal bool IsEnable() => Utils.RandomRange(1, 100) % appearedRatePerTile == 0;
}
