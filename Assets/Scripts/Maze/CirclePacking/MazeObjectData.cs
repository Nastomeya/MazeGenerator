public class MazeObjectData : BaseObjectData
{
    public float Angle { get; }
    public MazeObjectData(float _angle, int _position) : base(_position)
    {
        Angle = _angle;
    }
}