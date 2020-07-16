using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
namespace CaomaoFramework
{
    public class TrieTreeWordFilterModule : IWordFilterModule
    {
        private TrieNode[] m_arrayRoot = new TrieNode[char.MaxValue + 1];
        public void Init()
        {
            var path = Application.dataPath + "/WordFilter.txt";
            if (File.Exists(path))
            {
                Debug.Log("Ok");
            }
            try
            {
                //var text = File.ReadAllText(path,Encoding.Unicode);
                //if (string.IsNullOrEmpty(text) == false)
                //{
                //    Debug.Log("3r432r");
                //}
                Debug.Log(char.MaxValue);
                //var allLine = File.ReadAllLines(path,Encoding.Unicode);
                //if (allLine != null && allLine.Length > 0)
                //{
                //    Debug.Log("fwefew");
                //}
                //this.InitWorld(allLine);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }                    
        }
        public void InitWorld(ICollection<string> words)
        {
            var root = new TrieNode();
            Debug.Log("Root");
            foreach (var p in words)
            {
                if (string.IsNullOrEmpty(p))
                {
                    Debug.Log("3443");
                    continue;
                }
                var ch = p[0];
                var newNode = root.Add(ch);
                this.m_arrayRoot[ch] = newNode;
                for (int i = 1; i < p.Length; i++)
                {
                    newNode = newNode.Add(p[i]);
                }
                newNode.SetResults(p);
            }
            Debug.Log("Init");
            Dictionary<TrieNode, TrieNode> links = new Dictionary<TrieNode, TrieNode>();
            foreach (var item in root.m_values)
            {
                TryLinks(item.Value, null, links);
            }
            Debug.Log("r3r3");
            foreach (var item in links)
            {
                item.Key.Merge(item.Value, links);
            }
            Debug.Log("r3232r");
        }
        private void TryLinks(TrieNode node, TrieNode node2, Dictionary<TrieNode, TrieNode> links)
        {
            foreach (var item in node.m_values)
            {
                TrieNode tn = null;
                if (node2 == null)
                {
                    tn = m_arrayRoot[item.Key];
                    if (tn != null)
                    {
                        links[item.Value] = tn;
                    }
                }
                else if (node2.TryGetValue(item.Key, out tn))
                {
                    links[item.Value] = tn;
                }
                TryLinks(item.Value, tn, links);
            }
        }
        /// <summary>
        /// 在文本中替换所有的关键字
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="replaceChar">替换符</param>
        /// <returns></returns>
        public string Replace(string text, char replaceChar = '*')
        {
            StringBuilder result = new StringBuilder(text);

            TrieNode ptr = null;
            for (int i = 0; i < text.Length; i++)
            {
                TrieNode tn;
                if (ptr == null)
                {
                    tn = m_arrayRoot[text[i]];
                }
                else
                {
                    if (ptr.TryGetValue(text[i], out tn) == false)
                    {
                        tn = m_arrayRoot[text[i]];
                    }
                }
                if (tn != null)
                {
                    if (tn.End)
                    {
                        var maxLength = tn.Results[0].Length;
                        var start = i + 1 - maxLength;
                        for (int j = start; j <= i; j++)
                        {
                            result[j] = replaceChar;
                        }
                    }
                }
                ptr = tn;
            }
            return result.ToString();
        }
        /// <summary>
        /// 判断文本是否包含关键字
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public bool ContainsWord(string text)
        {
            TrieNode ptr = null;
            foreach (char t in text)
            {
                TrieNode tn;
                if (ptr == null)
                {
                    tn = m_arrayRoot[t];
                }
                else
                {
                    if (ptr.TryGetValue(t, out tn) == false)
                    {
                        tn = m_arrayRoot[t];
                    }
                }
                if (tn != null)
                {
                    if (tn.End)
                    {
                        return true;
                    }
                }
                ptr = tn;
            }
            return false;
        }   
    }
}
