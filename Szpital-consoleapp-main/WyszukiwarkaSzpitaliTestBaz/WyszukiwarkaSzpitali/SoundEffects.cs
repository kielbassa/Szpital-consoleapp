namespace Program
{
    public class SoundEffects
    {
        public void StartJingle()
        {
            Console.Beep(300, 100);
            Console.Beep(600, 100);
            Console.Beep(1200, 200);
        }
        public void CycleOption()
        {
            Console.Beep(400, 50);
            Console.Beep(800, 80);
        }
        public void BackOption()
        {
            Console.Beep(600, 50);
            Console.Beep(300, 100);
        }
        public void StopJingle()
        {
            Console.Beep(1000, 100);
            Console.Beep(600, 100);
            Console.Beep(300, 100);
            Console.Beep(150, 200);
        }
    }
}