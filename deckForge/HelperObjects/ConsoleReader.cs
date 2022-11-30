using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckForge.HelperObjects
{
    /// <summary>
    /// Reads input from a console.
    /// </summary>
    public class ConsoleReader : IInputReader
    {
        /// <inheritdoc/>
        public string? GetInput()
        {
            return Console.ReadLine();
        }
    }
}
