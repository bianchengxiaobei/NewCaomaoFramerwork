using System.IO;
using EasyZip.Zip;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
public static class FileAccessModule
{
    /// <summary>
    /// 解压文件到资源文件目录
    /// </summary>
    /// <param name="filePath"></param>
    public static void DecompressFile(string filePath)
    {
        DecompressToDirectory(Application.persistentDataPath, filePath);
    }
    /// <summary>
    /// 解压文件到指定文件路径
    /// </summary>
    /// <param name="targetPath"></param>
    /// <param name="zipFilePath"></param>
    public static void DecompressToDirectory(string targetPath, string zipFilePath)
    {
        if (File.Exists(zipFilePath))
        {
            Stream compressed = File.OpenRead(zipFilePath);
            compressed.DecompressToDirectory(targetPath);
        }
        else
        {
            Debug.LogError("解压文件不存在");
        }
    }
    /// <summary>
    /// 压缩文件
    /// </summary>
    /// <param name="filePath">zip文件路径</param>
    /// <param name="zipPath">压缩到哪个文件路径</param>
    public static void ZipFile(string filePath, string zipPath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("需要压缩的文件不存在");
        }
        string zipFileName = zipPath + Path.GetFileNameWithoutExtension(filePath) + ".zip";
        Debug.Log(zipFileName);
        using (FileStream fs = File.Create(zipFileName))
        {
            using (ZipOutputStream zipStream = new ZipOutputStream(fs))
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    string fileName = Path.GetFileName(filePath);
                    ZipEntry zipEntry = new ZipEntry(fileName);
                    zipStream.PutNextEntry(zipEntry);
                    byte[] buffer = new byte[1024];
                    int sizeRead = 0;
                    try
                    {
                        do
                        {
                            sizeRead = stream.Read(buffer, 0, buffer.Length);
                            zipStream.Write(buffer, 0, sizeRead);
                        } while (sizeRead > 0);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    stream.Close();
                }
                zipStream.Finish();
                zipStream.Close();
            }
            fs.Close();
        }
    }
    /// <summary>
    /// 压缩多个文件
    /// </summary>
    /// <param name="filePaths"></param>
    /// <param name="zipPath"></param>
    public static void ZipFiles(string[] filePaths, string zipPath)
    {
        if (filePaths == null || filePaths.Length == 0)
        {
            Debug.LogError("filePaths == null");
            return;
        }
        if (File.Exists(zipPath))
        {
            File.Delete(zipPath);
        }
        using (FileStream fs = File.Create(zipPath))
        {
            using (ZipOutputStream zipStream = new ZipOutputStream(fs))
            {
                foreach (var filePath in filePaths)
                {
                    if (!File.Exists(filePath))
                    {
                        Debug.LogError("需要压缩的文件不存在");
                    }

                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        string fileName = Path.GetFileName(filePath);
                        ZipEntry zipEntry = new ZipEntry(fileName);
                        zipStream.PutNextEntry(zipEntry);
                        byte[] buffer = new byte[1024];
                        int sizeRead = 0;
                        try
                        {
                            do
                            {
                                sizeRead = stream.Read(buffer, 0, buffer.Length);
                                zipStream.Write(buffer, 0, sizeRead);
                            } while (sizeRead > 0);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                        stream.Close();
                    }
                }
                zipStream.Finish();
                zipStream.Close();
            }
            fs.Close();
        }
    }
    private static void DecompressToDirectory(this Stream source, string targetPath)
    {
        targetPath = Path.GetFullPath(targetPath);
        using (ZipInputStream decompressor = new ZipInputStream(source))
        {
            ZipEntry entry;

            while ((entry = decompressor.GetNextEntry()) != null)
            {
                string name = entry.Name;
                if (entry.IsDirectory && entry.Name.StartsWith("\\"))
                    name = entry.Name.ReplaceFirst("\\", "");
                //name = ReplaceFirst(entry.Name, "\\", "");
                string filePath = Path.Combine(targetPath, name);
                string directoryPath = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                if (entry.IsDirectory)
                    continue;

                byte[] data = new byte[2048];
                using (FileStream streamWriter = File.Create(filePath))
                {
                    int bytesRead;
                    while ((bytesRead = decompressor.Read(data, 0, data.Length)) > 0)
                    {
                        streamWriter.Write(data, 0, bytesRead);
                    }
                }
            }
        }
    }
    public static string ReplaceFirst(this string source, string oldString, string newString)
    {
        Regex regEx = new Regex(oldString, RegexOptions.Multiline);
        return regEx.Replace(source, newString == null ? "" : newString, 1);
    }
}

