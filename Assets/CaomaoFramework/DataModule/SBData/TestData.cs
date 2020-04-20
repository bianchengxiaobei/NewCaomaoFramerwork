using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaomaoFramework
{
    public class TestData : CaomaoSBDataBase
    {
        public override EDataParserType Parser => EDataParserType.ScriptObject;

        public override string FilePath => "Localization_中国";
    }
}
