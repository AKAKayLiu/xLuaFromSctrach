using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil : MonoBehaviour
{
    public static readonly string AssetPath = Application.dataPath;
    public static readonly string BuildResourcesPath = AssetPath + "/BuildResources";

    //�����Ǵ�����Ǵ������ab����λ��
    public static readonly string BundleOutPath = Application.streamingAssetsPath;

    public static string GetStandardPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;
        return path.Trim().Replace("\\", "/");
    }

    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;
        return path.Substring(path.IndexOf("Assets"));//��ǰ������Ҵ��ڡ�Asset����λ�ã���Asset��ʼ����ȡ�����
    }
}
