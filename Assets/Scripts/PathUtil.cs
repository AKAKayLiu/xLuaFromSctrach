using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil : MonoBehaviour
{
    public static readonly string AssetPath = Application.dataPath;
    public static readonly string BuildResourcesPath = AssetPath + "/BuildResources";

    //这里是存放我们打出来的ab包的位置
    public static readonly string BundleOutPath = Application.streamingAssetsPath;

    public static string GetStandardPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;
        return path.Trim().Replace("\\", "/");
    }

    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;
        return path.Substring(path.IndexOf("Assets"));//从前往后查找存在“Asset”的位置，从Asset开始，截取到最后
    }
}
