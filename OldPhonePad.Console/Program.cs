using OldPhonePad;

namespace OldPhonePad.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("------ Old Phone Pad -------");
            System.Console.WriteLine("Enter key sequences ending with # (or `quit` to exit)");
            System.Console.WriteLine();

            while (true)
            {
                System.Console.Write("Enter number: ");
                var input = System.Console.ReadLine();
                if (string.IsNullOrEmpty(input) || string.Equals(input, "quit"))
                {
                    break;
                }

                try
                {
                    var result = OldPhonePad.Process(input);
                    System.Console.WriteLine("Output: " + result);
                    System.Console.WriteLine("------------------");
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Unknown error: " + e.Message);
                    throw;
                }
            }
            System.Console.WriteLine("Bye Bye!");
        }
    }
};