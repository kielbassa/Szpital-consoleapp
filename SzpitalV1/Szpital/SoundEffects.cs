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
        public void Select()
        {
            Console.Beep(400, 50);
            Console.Beep(800, 80);
        }
    }
}