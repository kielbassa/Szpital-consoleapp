namespace Program
{
    public class SoundEffects
    {
        [SupportedOSPlatform("windows")]
        public void StartJingle()
        {
            Console.Beep(300, 100);
            Console.Beep(600, 100);
            Console.Beep(1200, 200);
        }
        [SupportedOSPlatform("windows")]
        public void CycleOption()
        {
            Console.Beep(400, 50);
            Console.Beep(800, 80);
        }
        [SupportedOSPlatform("windows")]
        public void BackOption()
        {
            Console.Beep(600, 50);
            Console.Beep(300, 100);
        }
        [SupportedOSPlatform("windows")]
        public void StopJingle()
        {
            Console.Beep(1000, 100);
            Console.Beep(600, 100);
            Console.Beep(300, 100);
            Console.Beep(150, 200);
        }
    }
}