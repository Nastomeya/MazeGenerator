using DataExtensions;
using System.Collections.Generic;
using UnityEngine;

public class HexagonCirclePackingWall : CirclePackingBaseForWall, IMazeObjects
{
    private readonly HexagonFormation topBottomPair = HexagonFormation.top | HexagonFormation.bottom;
    private readonly HexagonFormation topRightBotLeftPair = HexagonFormation.topRight | HexagonFormation.botLeft;
    private readonly HexagonFormation botRightTopLeftPair = HexagonFormation.botRight | HexagonFormation.topLeft;
    private readonly List<int> bitwiseElements = new List<int>();

    public override void SetEnable(int _wallFormation)
    {
        var _isEnable = _wallFormation == 0 ? false : true;
        if (validObjects != null)
        {
            if (_isEnable)
            {
                SetEachWallEnable(_wallFormation);
                return;
            }

            //Reveal The Walls Or Not
            var allValidObjects = validObjects.GetEnumerator();
            while (allValidObjects.MoveNext())
                allValidObjects.Current.gameObject.SetActive(_isEnable);
        }
    }
    protected override void SetNumberOfObject(int _wallFormation)
    {
        int[] bitwiseElements = Bitwise.GetElements(new HexagonFormation());
        var division = bitwiseElements.Length;
        var numberOfObjectPerWall = setting.NumberOfObjects / division;
        currentNumOfObject = numberOfObjectPerWall * Bitwise.CombinationCount(bitwiseElements, _wallFormation);
    }
    protected override Vector3 GetRandomPosition(int _wallFormation)
    {
        Vector3 RandomPosition;
        float RandomXPosition, RandomYPosition;

        bitwiseElements.Clear();
        Bitwise.GetCombinationValues(bitwiseElements, (HexagonFormation)_wallFormation, _wallFormation);

        var randomIndex = Utils.RandomRange(0, bitwiseElements.Count);
        if ((bitwiseElements[randomIndex] | (int)topBottomPair) == (int)topBottomPair)// top and bottom
        {
            RandomXPosition = Utils.RandomRange(-setting.HalfRange, setting.HalfRange);
            RandomYPosition = GetTopOrBotOfYPosi(bitwiseElements[randomIndex]);
        }
        else if ((bitwiseElements[randomIndex] | (int)topRightBotLeftPair) == (int)topRightBotLeftPair)
        {
            RandomYPosition = GetHypotenuseOfYPosi((int)HexagonFormation.botLeft, (int)topRightBotLeftPair, bitwiseElements[randomIndex]);
            RandomXPosition = GetHypotenuseOfXPosi(60f, RandomYPosition, (int)HexagonFormation.botLeft, (int)topRightBotLeftPair, bitwiseElements[randomIndex]);
        }
        else
        {
            RandomYPosition = GetHypotenuseOfYPosi((int)HexagonFormation.botRight, (int)botRightTopLeftPair, bitwiseElements[randomIndex]);
            RandomXPosition = GetHypotenuseOfXPosi(60f, RandomYPosition, (int)HexagonFormation.topLeft, (int)botRightTopLeftPair, bitwiseElements[randomIndex]);
        }


        RandomPosition = new Vector3(transform.position.x + RandomXPosition, objectPrefabs[objectIndex].transform.position.y, transform.position.z + RandomYPosition);
        return RandomPosition;
    }
    private float GetTopOrBotOfYPosi(int _wallFormation)
    {
        float RandomPosition = Utils.RandomRange(setting.HalfRange, setting.FullRange);//top

        if ((_wallFormation | (int)HexagonFormation.bottom) == (int)HexagonFormation.bottom)//bottom
            RandomPosition = Utils.RandomRange(-setting.FullRange, -setting.HalfRange);

        return RandomPosition;
    }
    private float GetHypotenuseOfXPosi(float degree, float yPosi, int negativeFormation, int oppositePair, int _wallFormation)
    {
        float halfOfThickness = (setting.WallThickness / 2f);
        float hypotenuse = GetHypotenuseRange(halfOfThickness);

        var slotPosi = hypotenuse / Mathf.Sin(degree * Mathf.PI / 180f);
        var length = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(slotPosi, 2f) - Mathf.Pow(yPosi, 2f)));

        if (length < halfOfThickness)
            length = (slotPosi < halfOfThickness) ? slotPosi : slotPosi - halfOfThickness;

        bool isOpposited = Bitwise.IsContainedOneOfPairSet(_wallFormation, negativeFormation, oppositePair);
        if (isOpposited)
            length = -length;

        return length;
    }
    private float GetHypotenuseRange(float halfOfThickness)
    {
        var _max = 1f;

        var _min = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(_max, 2f) - Mathf.Pow(_max / 2f, 2f)));
        _min = (_min < halfOfThickness) ? _min : _min - halfOfThickness;

        return Utils.RandomRange(_min, _max);
    }
    private float GetHypotenuseOfYPosi(int negativeFormation, int oppositePair, int _wallFormation)
    {
        var slotPosi = Mathf.Sqrt(Mathf.Pow(setting.HalfRange, 2f) + Mathf.Pow(setting.HalfRange, 2f));
        var result = Bitwise.IsContainedOneOfPairSet(_wallFormation, negativeFormation, oppositePair) ? Utils.RandomRange(-slotPosi, 0) : Utils.RandomRange(0, slotPosi);
        return result;
    }
    protected override void SetRandomObjectIndex()
    {
        base.SetRandomObjectIndex();
        if (setting.IsRandomRotation)
            SetRandomRotation();
    }
    private void SetRandomRotation()
    {
        var angle = Utils.RandomRange(0f, 359f);
        objectRotation = Quaternion.AngleAxis(angle, Vector3.one);
    }
}