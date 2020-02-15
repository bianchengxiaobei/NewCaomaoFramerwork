using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace CaomaoFramework
{
    /// <summary>
    /// 字符过滤
    /// </summary>
    [Module(false)]
    public class WordFilterModule : IModule,IWordFilterModule
    {
        private IWordFilterModule WordFilterModuleImp = new TrieTreeWordFilterModule();
        public bool ContainsWord(string text)
        {
            return WordFilterModuleImp.ContainsWord(text);
        }

        public void Init()
        {            
            //加载非法字符集
            WordFilterModuleImp.Init();
        }

        public string Replace(string text, char replaceChar = '*')
        {
            return WordFilterModuleImp.Replace(text, replaceChar);
        }

        public void Update()
        {
            
        }
    }
}
