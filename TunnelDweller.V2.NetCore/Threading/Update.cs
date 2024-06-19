using System;


namespace TunnelDweller.NetCore.Threading
{
    public static class Update
    {
        public static event EventHandler OnUpdate;
        public static float UpdateRate { get; set; } = (1000 / 60);


        public static void Initialize()
        {
            Timer = new HighResolutionTimer(UpdateRate);
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        private static void Timer_Elapsed(object sender, HighResolutionTimerElapsedEventArgs e)
        {
            OnUpdate?.Invoke(null, null);
        }

        private static HighResolutionTimer Timer { get; set; }
    }
}
