using DataExtensions;
using UnityEngine;

public class SquareCirclePackingWall : BaseCirclePacking, IMazeObjects
{
    private readonly SquareFormation topBottomPair = SquareFormation.top | SquareFormation.bottom;
    private readonly SquareFormation rightLeftPair = SquareFormation.right | SquareFormation.left;

    private bool isEnable = true;
    public void SetEnble(bool _isEnable)
    {
        //Reveal The Objects Or Not
        var allValidObjects = validObjects.GetEnumerator();
        while (allValidObjects.MoveNext())
        {
            allValidObjects.Current.gameObject.SetActive(_isEnable);
        }
        isEnable = _isEnable;
    }
    public override void SetMazeFormation(int _objFormation)
    {
        if (validObjects == null)
            return;
        if (!isEnable)
            return;

        if (_objFormation != 0)
        {
            SetEachElementEnable(_objFormation);
            return;
        }

        SetEnble(_objFormation != 0);
    }
    protected override void SetNumberOfObject(int _wallFormation)
    {
        int[] bitwiseElements = Bitwise.GetElements(new SquareFormation());
        var division = bitwiseElements.Length;
        var numberOfObjectPerWall = setting.NumberOfObjects / division;
        currentNumOfObject = numberOfObjectPerWall * Bitwise.CombinationCount(bitwiseElements, _wallFormation);
    }
    protected override Vector3 GetRandomPosition(int _wallFormation)
    {
        Vector3 RandomPosition;
        float RandomXPosition, RandomYPosition;

        if (Bitwise.OnlyOppositePairCheck(_wallFormation, (int)rightLeftPair))//if only right and left
        {
            RandomXPosition = GetSignleAxisRandomPosition((int)SquareFormation.left, (int)SquareFormation.right, (int)rightLeftPair, _wallFormation);
            RandomYPosition = GetPosoition();
        }
        else//a mixture of the top, right, bottom and left
        {
            SetTopBottomXThreshold(out float min, out float max, _wallFormation);

            RandomXPosition = GetPosoition();
            if (RandomXPosition > min && RandomXPosition <= max)//top and bottom Y
                RandomYPosition = GetSignleAxisRandomPosition((int)SquareFormation.bottom, (int)SquareFormation.top, (int)topBottomPair, _wallFormation);
            else//right and left Y
                RandomYPosition = GetPosoition();
        }

        RandomPosition = new Vector3(transform.position.x + RandomXPosition, objectPrefabs[objectIndex].transform.position.y, transform.position.z + RandomYPosition);
        return RandomPosition;
    }
    private float GetPosoition() => Utils.RandomRange(-setting.FullRange, setting.FullRange);
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
    private void SetTopBottomXThreshold(out float min, out float max, int _wallFormation)
    {
        min = -setting.FullRange;
        max = setting.FullRange;

        if (Bitwise.IsContainedOneOfPairSet(_wallFormation, (int)SquareFormation.right, (int)rightLeftPair))
            max = setting.HalfRange;
        else if (Bitwise.IsContainedOneOfPairSet(_wallFormation, (int)SquareFormation.left, (int)rightLeftPair))
            min = -setting.HalfRange;
        else if (Bitwise.IsContainedBothPairSet(_wallFormation, (int)rightLeftPair))
        {
            max = setting.HalfRange;
            min = -setting.HalfRange;
        }
    }
    private float GetSignleAxisRandomPosition(int negativeFormation, int positiveFormation, int oppositePair, int _wallFormation)
    {
        var positive = Utils.RandomRange(setting.HalfRange, setting.FullRange);
        var negative = Utils.RandomRange(-setting.FullRange, -setting.HalfRange);

        float RandomPosition = positive;

        if (Bitwise.IsContainedOneOfPairSet(_wallFormation, negativeFormation, oppositePair))
            RandomPosition = negative;
        else if (Bitwise.IsContainedBothPairSet(_wallFormation, (negativeFormation | positiveFormation)))
        {
            var posOrNag = Mathf.Sign(Utils.RandomRange(-100, 100));
            RandomPosition = (posOrNag > 0) ? positive : negative;
        }

        return RandomPosition;
    }
}