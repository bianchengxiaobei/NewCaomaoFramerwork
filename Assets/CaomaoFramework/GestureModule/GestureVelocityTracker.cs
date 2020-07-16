using UnityEngine;
/// <summary>
/// Tracks and calculates velocity for gestures
/// </summary>
public class GestureVelocityTracker
{
    private struct VelocityHistory
    {
        public float VelocityX;
        public float VelocityY;
        public float Seconds;
    }

    private const int maxHistory = 8;

    private readonly System.Collections.Generic.Queue<VelocityHistory> history = new System.Collections.Generic.Queue<VelocityHistory>();
    private readonly System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
    private float previousX;
    private float previousY;

    private void AddItem(float velocityX, float velocityY, float elapsed)
    {
        VelocityHistory item = new VelocityHistory
        {
            VelocityX = velocityX,
            VelocityY = velocityY,
            Seconds = elapsed
        };
        history.Enqueue(item);
        if (history.Count > maxHistory)
        {
            history.Dequeue();
        }
        float totalSeconds = 0.0f;
        //VelocityX = VelocityY = 0.0f;
        this.Veloctiy = Vector2.zero;
        foreach (VelocityHistory h in history)
        {
            totalSeconds += h.Seconds;
        }
        foreach (VelocityHistory h in history)
        {
            float weight = h.Seconds / totalSeconds;
            //VelocityX += (h.VelocityX * weight);
            //VelocityY += (h.VelocityY * weight);
            this.Veloctiy.x += (h.VelocityX * weight);
            this.Veloctiy.y += (h.VelocityY * weight);
        }
        timer.Reset();
        timer.Start();
    }

    public void Reset()
    {
        timer.Reset();
        this.Veloctiy = Vector2.zero;
        //VelocityX = VelocityY = 0.0f;
        history.Clear();
    }

    public void Restart()
    {
        Restart(float.MinValue, float.MinValue);
    }

    public void Restart(float previousX, float previousY)
    {
        this.previousX = previousX;
        this.previousY = previousY;
        Reset();
        timer.Start();
    }

    public void Update(float x, float y)
    {
        float elapsed = ElapsedSeconds;
        if (previousX != float.MinValue)
        {
            float px = previousX;
            float py = previousY;
            float velocityX = (x - px) / elapsed;
            float velocityY = (y - py) / elapsed;
            AddItem(velocityX, velocityY, elapsed);
        }
        previousX = x;
        previousY = y;
    }

    public void Update(Vector2 pos)
    {
        float elapsed = ElapsedSeconds;
        if (previousX != float.MinValue)
        {
            float px = previousX;
            float py = previousY;
            float velocityX = (pos.x - px) / elapsed;
            float velocityY = (pos.y - py) / elapsed;
            AddItem(velocityX, velocityY, elapsed);
        }
        previousX = pos.x;
        previousY = pos.y;
    }


    public float ElapsedSeconds { get { return (float)timer.Elapsed.TotalSeconds; } }

    public Vector2 Veloctiy = Vector2.zero;
    //public float VelocityX { get; private set; }
    //public float VelocityY { get; private set; }
    public float Speed { get { return (float)Mathf.Sqrt(this.Veloctiy.x * this.Veloctiy.x + this.Veloctiy.y * this.Veloctiy.y); } }
    //public float Speed { get { return (float)Mathf.Sqrt(VelocityX * VelocityX + VelocityY * VelocityY); } }
}