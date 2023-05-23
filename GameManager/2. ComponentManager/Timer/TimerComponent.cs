using System;

namespace MonogameExamples
{
    public class TimerComponent : Component
    {
        public float CountdownValue { get; set; }

        public TimerComponent(float initialValue)
        {
            this.CountdownValue = initialValue;
        }

        public bool IsCountdownFinished
        {
            get { return CountdownValue <= 0; }
        }
    }
}
