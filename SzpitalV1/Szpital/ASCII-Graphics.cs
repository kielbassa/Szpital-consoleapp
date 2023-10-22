namespace Program
{
    public class ASCII_Graphics
    {
        public void MainLogo()
        {
            string MainLogo =
                    @"
                                                                              █████████   R                 
          ███████ ███████ ██████  ██ ████████  █████  ██      ███████       ███       ███                   
          ██         ███  ██   ██ ██    ██    ██   ██ ██      ██           ██   █   █   ██                  
          ███████   ███   ██████  ██    ██    ███████ ██      █████        ██   █████   ██                  
               ██  ███    ██      ██    ██    ██   ██ ██      ██           ██   █   █   ██                  
          ███████ ███████ ██      ██    ██    ██   ██ ███████ ███████       ███       ███                   
                                                                              █████████                     ";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(MainLogo);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
