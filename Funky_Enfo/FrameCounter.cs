using System.Collections.Generic;
using System.Linq;

namespace FunkyEnfo
{
    // http://stackoverflow.com/questions/20676185/xna-monogame-getting-the-frames-per-second
    public class FrameCounter
    {
        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        public const int MaximumSamples = 100;

        private Queue<float> sampleBuffer = new Queue<float>();

        public void Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            this.sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (this.sampleBuffer.Count > MaximumSamples)
            {
                this.sampleBuffer.Dequeue();
                AverageFramesPerSecond = this.sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
        }
    }
}
