public class StraightPathController
{
    internal int NextIndex { get; private set; }
    //private readonly int straightPathScale = 2;//1;//3;
    private int lastNeibour, IdenticalRandomCounter;

    internal StraightPathController()
    {
        NextIndex = -1;
        lastNeibour = -1;
        IdenticalRandomCounter = 0;
    }
    //internal bool IsOutOfStraightPathRule(int currNeighbour) => IdenticalRandomCounter >= straightPathScale && currNeighbour == lastNeibour;
    internal bool IsOutOfStraightPathRule(int currNeighbour, int _straightPathScale) => IdenticalRandomCounter >= _straightPathScale && currNeighbour == lastNeibour;

    internal void SetNextIndex(int _nextIndex) => NextIndex = _nextIndex;
    internal void IdenticalRandomCheck(int currNeighbour)
    {
        if (currNeighbour != lastNeibour)
        {
            IdenticalRandomCounter = 1;
            lastNeibour = currNeighbour;
        }
        else
            IdenticalRandomCounter++;
    }
}
