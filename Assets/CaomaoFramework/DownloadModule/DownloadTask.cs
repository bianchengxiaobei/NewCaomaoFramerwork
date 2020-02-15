using System;
using System.Collections.Generic;

namespace CaomaoFramework
{
    public class DownloadTask
    {
        private string m_sUrl;
        private bool m_bFinished = false;
        private int m_iTaskId = 0;
        public DownloadTask(string url,int id)
        {
            this.m_sUrl = url;
            this.m_iTaskId = id;
            this.m_bFinished = false;
        }
        public DownloadTask()
        {
            this.m_sUrl = null;
            this.m_iTaskId = 0;
            this.m_bFinished = false;
        }

        public string URL
        {
            get
            {
                return this.m_sUrl;
            }
            set
            {
                this.m_sUrl = value;
            }
        }
        public bool Finished
        {
            get
            {
                return this.m_bFinished;
            }
            set
            {
                this.m_bFinished = value;
            }
        }

        public int TaskId
        {
            get
            {
                return this.m_iTaskId;
            }
            set
            {
                this.m_iTaskId = value;
            }
        }

        public void OnRelease()
        {
            this.Finished = false;
            this.m_sUrl = null;
            this.m_iTaskId = 0;
        }
    }
}
