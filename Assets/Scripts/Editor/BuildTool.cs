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
                if (files[i].EndsWith(".meta")) continue;//meta文件是不用打包的

                AssetBundleBuild assetBundle = new AssetBundleBuild();
                string fileName = PathUtil.GetStandardPath(files[i]);//文件的格式进行标准化
                Debug.Log("FileName : " + fileName);

                string assetName = PathUtil.GetUnityPath(fileName);//截取asset之后的部分作为assetname属性
                assetBundle.assetNames = new string[] { assetName };
                Debug.Log("AssetName: " + assetName );

                string bundleName = fileName.Replace(PathUtil.BuildResourcesPath, "").ToLower();//标准化之后将BuildResource以及之前的部分都替换掉，作为Bundle的名字
                assetBundle.assetBundleName = bundleName + ".ab";//Bundle的后缀改为ab
                Debug.Log("AssetBundleName: " + bundleName + ".ab");
                assetBundleBuilds.Add(assetBundle);

                //添加依赖信息
                List<string> dependenceInfo = GetDependence(assetName);
                string bundleInfo = assetName + "|" + bundleName + ".ab";

                if (dependenceInfo.Count > 0)
                    bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);

                bundleInfos.Add(bundleInfo);
            }

            if (Directory.Exists(PathUtil.BundleOutPath)) Directory.Delete(PathUtil.BundleOutPath, true);
            //创建输出的目标
            Directory.CreateDirectory(PathUtil.BundleOutPath);

            //这个就是打包
            BuildPipeline.BuildAssetBundles(PathUtil.BundleOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
            File.WriteAllLines(PathUtil.BundleOutPath + "/" , bundleInfos);

            AssetDatabase.Refresh();
        }
    

    //这是获取依赖文件
    static List<string> GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();

        string[] files = AssetDatabase.GetDependencies(curFile);//这是一个正向查找依赖的接口，用于查找当前资源所依赖的资源
        dependence = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList();
        return dependence;
    }
}
