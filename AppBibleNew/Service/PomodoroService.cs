using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace AppBibleNew.Services
{
    public class PomodoroService : IDisposable
    {
        private Timer? _timer;
        private TimeSpan _timeLeft;

        public int WorkMinutes { get; set; } = 25;
        public int WorkSeconds { get; set; } = 00;

        public int BreakMinutes { get; set; } = 0;
        public int BreakSeconds { get; set; } = 10;

        public bool IsRunning { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsBreak { get; private set; }

        public event Action<TimeSpan>? OnTick;
        public event Action? OnWorkEnd;
        public event Action? OnBreakEnd;

        public void Start()
        {
            if (IsRunning) return;

            IsRunning = true;
            IsPaused = false;
            IsBreak = false;

            _timeLeft = new TimeSpan(0, WorkMinutes, WorkSeconds);

            _timer = new Timer(1000);
            _timer.Elapsed += HandleTick;
            _timer.AutoReset = true;
            _timer.Start();
        }

        public void StartBreak()
        {
            if (IsRunning) return;

            IsRunning = true;
            IsPaused = false;
            IsBreak = true;

            _timeLeft = new TimeSpan(0, BreakMinutes, BreakSeconds);

            _timer = new Timer(1000);
            _timer.Elapsed += HandleTick;
            _timer.AutoReset = true;
            _timer.Start();
        }

        public void Pause()
        {
            if (!IsRunning) return;
            _timer?.Stop();
            IsRunning = false;
            IsPaused = true;
        }

        public void Resume()
        {
            if (!IsPaused) return;
            _timer?.Start();
            IsRunning = true;
            IsPaused = false;
        }

        public void Reset()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;

            IsRunning = false;
            IsPaused = false;
            IsBreak = false;

            _timeLeft = new TimeSpan(0, WorkMinutes, WorkSeconds);
            OnTick?.Invoke(_timeLeft);
        }

        private void HandleTick(object? sender, ElapsedEventArgs e)
        {
            if (_timeLeft.TotalSeconds > 0)
            {
                _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(1));
                OnTick?.Invoke(_timeLeft);
            }
            else
            {
                _timer?.Stop();
                IsRunning = false;

                if (!IsBreak)
                {
                    OnWorkEnd?.Invoke();
                }
                else
                {
                    OnBreakEnd?.Invoke();
                }
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
