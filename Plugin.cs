using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Player;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Timers;
using WavenGSI.Player;
using WavenGSI.World;

namespace WavenGSI
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        private PlayerInfos PlayerInfos = new();
        private WorldInfos WorldInfos = new();
        private static HttpClient client = new();
        public override void Load()
        {
            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            var artemisFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), 
                "Artemis/webserver.txt"
            );
            var baseAddress = File.ReadAllText(artemisFolder);
            client.BaseAddress = new Uri(baseAddress);
            Timer timer = new Timer();
            timer.AutoReset = true;
            timer.Interval = 1000;
            timer.Elapsed += (s, e) => SendInfos();
            timer.Start();

        }

        private async void SendInfos()
        {
            PlayerInfos.Update();
            WorldInfos.Update();
            string playerInfosSerialized = JsonSerializer.Serialize(
                new
                {
                    Player = PlayerInfos,
                    World = WorldInfos
                });
            Log.LogInfo(playerInfosSerialized);
            using StringContent jsonContent = new(
                playerInfosSerialized, 
                Encoding.UTF8, 
                "application/json");
            try
            {
                using HttpResponseMessage response = await client.PostAsync(
                    "plugins/3C54C8B4-6674-456F-8A09-AD3813BC438F/Waven",
                    jsonContent);

                Log.LogInfo(playerInfosSerialized);
            } catch (Exception ex)
            {
                Log.LogWarning(ex);
            }

        }
    }
}
