namespace Hourglass.Pages.Components
{
    public partial class StopWatch
    {
        TimeSpan stopwatchvalue = new TimeSpan();
        bool isRunning = false;
        CancellationTokenSource cts = new CancellationTokenSource();
        async Task StartTimer()
        {
            if (isRunning) return; // Prevents multiple concurrent loops
            isRunning = true;
            cts = new CancellationTokenSource();
            try
            {
                while (isRunning)
                {
                    await Task.Delay(1000, cts.Token);
                    stopwatchvalue = stopwatchvalue.Add(new TimeSpan(0, 0, 1));
                    StateHasChanged();
                }
            }
            catch (OperationCanceledException)
            {
                // Caught the exception thrown by the cancellation token. Do nothing here.
            }
        }

        void Pause()
        {
            isRunning = false;
            cts.Cancel();
        }

        void Reset()
        {
            isRunning = false;
            stopwatchvalue = new TimeSpan();
            cts.Cancel();
            StateHasChanged();
        }
    }
}