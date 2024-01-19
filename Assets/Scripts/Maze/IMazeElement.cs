using UnityEngine;
/// <summary>It could be a wall, torch, rocks, etc. that can fit in a maze tile.<see cref="IMazeObjects"/></summary>
public interface IMazeElement
{
    public GameObject gameObject { get; }
    public Transform transform { get; }
}