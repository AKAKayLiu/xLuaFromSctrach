using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildTool : Editor
{   
    [MenuItem("Tools/Build Windows Bundle")]
    static void BundleWindowsBuild()
    {
        Build(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Tools/Build Android Bundle")]
    static void BundleAndroidBuild()
    {
        Build(BuildTarget.Android);
    }

    [MenuItem("Tools/Build IOS Bundle")]
    static void BundleIOSBuild()
    {
        Build(BuildTarget.iOS);
    }

    static void Build(BuildTarget target)
        {
            List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
            List<string> bundleInfos = new List<string>();
            string[] files = Directory.GetFiles(PathUtil.BuildResourcesPath, "*", SearchOption.AllDirectories);
            for (int i=0; i<files.Length; i++)
            {
                if (files[i].EndsWith(".meta")) continue;//meta�ļ��ǲ��ô����

                AssetBundleBuild assetBundle = new AssetBundleBuild();
                string fileName = PathUtil.GetStandardPath(files[i]);//�ļ��ĸ�ʽ���б�׼��
                Debug.Log("FileName : " + fileName);

                string assetName = PathUtil.GetUnityPath(fileName);//��ȡasset֮��Ĳ�����Ϊassetname����
                assetBundle.assetNames = new string[] { assetName };
                Debug.Log("AssetName: " + assetName );

                string bundleName = fileName.Replace(PathUtil.BuildResourcesPath, "").ToLower();//��׼��֮��BuildResource�Լ�֮ǰ�Ĳ��ֶ��滻������ΪBundle������
                assetBundle.assetBundleName = bundleName + ".ab";//Bundle�ĺ�׺��Ϊab
                Debug.Log("AssetBundleName: " + bundleName + ".ab");
                assetBundleBuilds.Add(assetBundle);

                //���������Ϣ
                List<string> dependenceInfo = GetDependence(assetName);
                string bundleInfo = assetName + "|" + bundleName + ".ab";

                if (dependenceInfo.Count > 0)
                    bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);

                bundleInfos.Add(bundleInfo);
            }

            if (Directory.Exists(PathUtil.BundleOutPath)) Directory.Delete(PathUtil.BundleOutPath, true);
            //���������Ŀ��
            Directory.CreateDirectory(PathUtil.BundleOutPath);

            //������Ǵ��
            BuildPipeline.BuildAssetBundles(PathUtil.BundleOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
            File.WriteAllLines(PathUtil.BundleOutPath + "/" , bundleInfos);

            AssetDatabase.Refresh();
        }
    

    //���ǻ�ȡ�����ļ�
    static List<string> GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();

        string[] files = AssetDatabase.GetDependencies(curFile);//����һ��������������Ľӿڣ����ڲ��ҵ�ǰ��Դ����������Դ
        dependence = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList();
        return dependence;
    }
}
