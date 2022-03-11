using System.IO;
using UnityEngine;
public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // string path = @"E:\工具\bilibili视频下载器1.0.7\bilibili视频下载器\download\RPG\";
        string targetpath = @"E:\工具\bilibili视频下载器1.0.7\bilibili视频下载器\download\Test\";
        //DirectoryInfo directoryInfo = new DirectoryInfo(path);
        //DirectoryInfo[] files = directoryInfo.GetDirectories();
        //for (int i = 0; i < files.Length; i++)
        //{
        //    string s = files[i].Name;
        //  s=  s.Replace("Unity3D游戏开发教程_Core核心功能", "");
        //  s=  s.Replace("Unity_3D_RPG_Core核心功能", "");            
        //        Debug.Log(s);
        //    FileInfo[] info= files[i].GetFiles();

        //    if (info.Length==1)
        //    {
        //        //  Debug.Log(info[0].DirectoryName);
        //        File.Move(info[0].FullName, targetpath+s);
        //    }
        //}
        DirectoryInfo directory = new DirectoryInfo(targetpath);
        var info = directory.GetFiles();
        for (int i = 0; i < info.Length; i++)
        {
            string s = info[i].Name;
            Debug.Log(s);
            File.Move(info[i].FullName, targetpath + s + ".mp4");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
