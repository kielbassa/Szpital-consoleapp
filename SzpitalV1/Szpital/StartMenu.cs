namespace Program
{
    public class Menu
    {
        int windowHeight = 30;
        int windowWidth = 100;
        public void SetWindowSize()  // ustawianie odpowieniego rozmiaru okna konsoli ze sprawdzeniem czy nie jest wiekszy od dostepnego ekranu
        {
            
            int maxWindowHeight = Console.LargestWindowHeight;
            int maxWindowWidth = Console.LargestWindowWidth;
            if(maxWindowHeight > windowHeight || maxWindowWidth > windowWidth)
            {
                Console.SetWindowSize(windowWidth, windowHeight);
            }
            else
            {
                Console.WriteLine("Skala konsoli nie może być ustawiona");
                Environment.Exit(1);
            }
        }
    }
}
