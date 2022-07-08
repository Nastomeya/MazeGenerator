/// < summary > I can use this to simply draw an box on GUI show as following
///private void OnGUI()
///{
///    if (dragSelect)
///    {
///        var rect = Utils.GetScreenRect(startPoint, Input.mousePosition);
///        Utils.DrawScreenRect(rect, new Color(1f, 1f, 1f, 0.25f));
///        Utils.DrawScreenRectBorder(rect, 2, new Color(1f, 1f, 1f));
///    }
///}
/// </summary>
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using DataExtensions;
/// <summary>
/// Reference as below, this is not my code(By TheScreamingFedora)
/// The reset of the code can be download in
/// https://github.com/pickles976/RTS_selection/commits/master
/// </summary>
//https://www.youtube.com/watch?v=OL1QgwaDsqo&list=PLROHqCOfMPkNvsMiLFNol0NqRjl9GzwFW
public class Utils
{
    #region GUI
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }
    #endregion

    #region True Random
    private static RNGCryptoServiceProvider rng;
    public static float RandomRange(float min, float max)
    {
        rng = new RNGCryptoServiceProvider();
        InvalidSizeCheck(min, max);
        return (float)(min + ((double)max - min) * NextDouble());
    }
    public static int RandomRange(int min, int max)
    {
        rng = new RNGCryptoServiceProvider();
        InvalidSizeCheck(min, max);
        return (int)Math.Floor((min + ((double)max - min) * NextDouble()));
    }
    private static double NextDouble()
    {
        var buffer = new byte[sizeof(uint)];
        rng.GetBytes(buffer);
        var randUint = BitConverter.ToUInt32(buffer, 0);
        return randUint / (uint.MaxValue + 1.0);
    }
    private static void InvalidSizeCheck(float min, float max)
    {
        if (min > max)
            throw new ArgumentOutOfRangeException();
    }
    #endregion

    #region IO in Json format
    private static void MekeSureTheFileExist(string directoryPath, string fileNamePath)
    {
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);//if it doesn't, create it

        if (!File.Exists(fileNamePath))//if it doesn't, create it
            File.WriteAllText(fileNamePath, "");
    }
    public static T LoadDataFromFile<T>(string directoryPath, string fileNamePath)
    {
        MekeSureTheFileExist(directoryPath, fileNamePath);
        return JsonUtility.FromJson<T>(File.ReadAllText(fileNamePath));
    }
    public static void SaveDataToFile<T>(T data, string targetFileName)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(targetFileName, json);
    }
    public static Dictionary<int, bool> LoadDicFromFile(string directoryPath, string fileNamePath)
    {
        MekeSureTheFileExist(directoryPath, fileNamePath);
        var json = File.ReadAllText(fileNamePath);
        return FormatToJson.Deserialize_ToDicIntBool(json);
    }
    public static void SaveDicToFile<T1, T2>(Dictionary<T1, T2> _dictionary, string targetFileName)
    {
        var json = FormatToJson.Serialize_FromDic(_dictionary);
        File.WriteAllText(targetFileName, json);
    }
    #endregion


}
