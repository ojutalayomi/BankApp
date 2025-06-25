namespace BankApp.CLI.Tests
{
    public static class TestHelpers
    {
        /// <summary>
        /// Captures console output for testing
        /// </summary>
        /// <param name="action">Action to execute while capturing output</param>
        /// <returns>Captured console output</returns>
        public static string CaptureConsoleOutput(Action action)
        {
            var originalOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            
            try
            {
                action();
                return stringWriter.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        /// <summary>
        /// Simulates console input for testing
        /// </summary>
        /// <param name="input">Input to simulate</param>
        /// <param name="action">Action to execute with simulated input</param>
        public static void SimulateConsoleInput(string input, Action action)
        {
            var originalIn = Console.In;
            using var stringReader = new StringReader(input);
            Console.SetIn(stringReader);
            
            try
            {
                action();
            }
            finally
            {
                Console.SetIn(originalIn);
            }
        }
    }
}

