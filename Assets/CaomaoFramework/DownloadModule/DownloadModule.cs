using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace CaomaoFramework
{
    [Module(false)]
    public class DownloadModule : IDownloadModule, IModule
    {
        private Queue<DownloadTask> m_freeTaskPool = new Queue<DownloadTask>(2);
        private Queue<DownloadTask> m_queueWaitTasks = new Queue<DownloadTask>();
        private DownloadTask m_oCurTask;
        private Action<byte[]> m_actionHandleData;
        private Action<string> m_actionError;
        private static int TaskIdCounter = 0;
        //private float m_fProgress;
        public float Progress
        {
            get//   all-5,cur-0
            {
                if (this.m_oCurTask != null)
                {
                    var baseP = this.m_oCurTask.TaskId / TaskIdCounter;
                    return baseP + CaomaoDriver.WebRequestModule.Progress;
                }
                return 1f;
            }
        }

        public void Init()
        {
            this.m_freeTaskPool.Enqueue(new DownloadTask());
            this.m_freeTaskPool.Enqueue(new DownloadTask());
            //this.m_fProgress = 0;
        }

        public void AddDownloadTask(string url)
        {
            var task = this.GetFreeDownloadTask(url);
            this.m_queueWaitTasks.Enqueue(task);
        }

        public void StartDownload()
        {
            if (this.m_queueWaitTasks.Count > 0)
            {
                this.m_oCurTask = this.m_queueWaitTasks.Peek();
                CaomaoDriver.WebRequestModule.DownloadBytes(this.m_oCurTask.URL, this.DownloadFinishedCallback);
            }
            else
            {
                TaskIdCounter = 0;
                if (this.m_queueWaitTasks.Count > 0)
                {
                    Debug.LogError("task != null:" + this.m_queueWaitTasks.Count);
                }
                this.m_queueWaitTasks.Clear();
            }
        }

        private void DownloadFinishedCallback(byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                this.m_actionHandleData?.Invoke(data);
                //下一个task
                var task = this.m_queueWaitTasks.Dequeue();
                if (task == this.m_oCurTask)
                {
                    this.AddFreeDownloadTask(task);
                    this.StartDownload();
                }
            }
            else
            {
                this.m_actionError?.Invoke("下载失败:数据出错！");
            }
        }
    

        private DownloadTask GetFreeDownloadTask(string url)
        {
            DownloadTask task;
            if (this.m_freeTaskPool.Count > 0)
            {
                task = this.m_freeTaskPool.Dequeue();
                task.URL = url;
                task.TaskId = TaskIdCounter++;
                return task;
            }
            task = new DownloadTask(url, TaskIdCounter++);
            return task;
        }

        private void AddFreeDownloadTask(DownloadTask task)
        {
            if (task != null)
            {
                task.OnRelease();
                this.m_freeTaskPool.Enqueue(task);
            }
        }

        public void Update()
        {
            
        }
    }
}
