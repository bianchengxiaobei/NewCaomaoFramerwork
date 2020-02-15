using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaomaoFramework
{
    public class UpdateProgressArgs
    {
        public float progress;
        public float downloadSpeed;
        public float downloadFileSize;
        public int curDownloadFileIndex;
        public int allDownloadFileNum;
    }
}
