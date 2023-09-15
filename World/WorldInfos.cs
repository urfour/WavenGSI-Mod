using Ankama.Cube.Data;
using Ankama.Cube.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavenGSI.World
{
    public class WorldInfos
    {
        public string LevelName { get; set; }
        public string LevelFullName { get; set; }
        public WorldInfos() { }
        public void Update()
        {
            if (WorldMap.instance != null)
            {
                LevelName = WorldMap.instance.levelDefinition.name;
                LevelFullName = WorldMap.instance.extendedLevelDefinition.name;
            }
        }
    }


}
