using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavenGSI.Player
{
    public class Spell
    {
        public string Name { get; set; }
        public string Element { get; set; }
        public int Cost { get; set; }
        public bool IsAvailable { get; set; }

        public Spell() { }
    }
}
