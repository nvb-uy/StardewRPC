using System;
using System.IO;
using DiscordRPC;
using DiscordRPC.Logging;
using Microsoft.Xna.Framework;

using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Network;

using Newtonsoft.Json;
using LogLevel = StardewModdingAPI.LogLevel;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace StardewRPC {
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod {

        private DiscordRpcClient? rpcClient;

        internal static Config CONFIG { get; private set; } = null!;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        /// 
        public override void Entry(IModHelper helper) {
            CONFIG = this.Helper.ReadConfig<Config>();

            rpcClient = new DiscordRpcClient(CONFIG.ClientID);

            rpcClient.OnReady += (sender, e) => {
                Console.WriteLine("RPC Loaded on discord user @{0}", e.User.Username);
            };

            rpcClient.Initialize();
            updateRPC(0);

            helper.Events.GameLoop.UpdateTicked += this.UpdateTicked;
        }

        private void UpdateTicked(object? sender, UpdateTickedEventArgs e) {
            if (e.IsMultipleOf(((uint) CONFIG.Update_Rate_Seconds) * 60)) {
                if (Context.IsWorldReady) {
                    if (Context.IsMultiplayer) {
                        updateRPC(2);
                    } else {
                        updateRPC(1);
                    }
                }
                else {
                    updateRPC(0);
                }
            }
        }

        private Dictionary<string, string> getPlaceholders() {
            return new Dictionary<string, string> {
                {"%MODS%",                      this.Helper.ModRegistry.GetAll().LongCount().ToString()},
                {"%PLAYER_COUNT_TOTAL%",        (this.getPlayerCount() + 1).ToString()},
                {"%PLAYER_COUNT_FRIENDS%",      this.getPlayerCount().ToString()}
            };
        }

        private int getPlayerCount() {
            if (Context.IsMultiplayer) {
                return ((int)Multiplayer.AllPlayers);
            }

            return 1;
        }

        // States:
        // 0: Main menu (Default)
        // 1: Singleplayer
        // 2: Co-op
        private void updateRPC(int state) {
            if (!CONFIG.Enable || rpcClient == null) return;

            string Details;
            string State;
            string LargeImageKey;
            string LargeImageText;
            string SmallImageKey;
            string SmallImageText;
            List<string> Button1 = new List<string> { "", "" };
            List<string> Button2 = new List<string> { "", "" }; ;

            switch (state)
            {
                case 1:
                    CONFIG.Singleplayer.updatePlaceholders(getPlaceholders());

                    Details = CONFIG.Singleplayer.Details;
                    State = CONFIG.Singleplayer.State;
                    LargeImageKey = CONFIG.Singleplayer.LargeImageKey;
                    LargeImageText = CONFIG.Singleplayer.LargeImageText;
                    SmallImageKey = CONFIG.Singleplayer.SmallImageKey;
                    SmallImageText = CONFIG.Singleplayer.SmallImageText;
                    if (CONFIG.Singleplayer.Button1.enabled) Button1 = new List<string> { CONFIG.Singleplayer.Button1.Label, CONFIG.Singleplayer.Button1.URL };
                    if (CONFIG.Singleplayer.Button2.enabled) Button2 = new List<string> { CONFIG.Singleplayer.Button2.Label, CONFIG.Singleplayer.Button2.URL };
                    break;

                case 2:
                    CONFIG.Multiplayer.updatePlaceholders(getPlaceholders());

                    Details = CONFIG.Multiplayer.Details;
                    State = CONFIG.Multiplayer.State;
                    LargeImageKey = CONFIG.Multiplayer.LargeImageKey;
                    LargeImageText = CONFIG.Multiplayer.LargeImageText;
                    SmallImageKey = CONFIG.Multiplayer.SmallImageKey;
                    SmallImageText = CONFIG.Multiplayer.SmallImageText;
                    if (CONFIG.Multiplayer.Button1.enabled) Button1 = new List<string> { CONFIG.Multiplayer.Button1.Label, CONFIG.Multiplayer.Button1.URL };
                    if (CONFIG.Multiplayer.Button2.enabled) Button2 = new List<string> { CONFIG.Multiplayer.Button2.Label, CONFIG.Multiplayer.Button2.URL };
                    break;

                default:
                    CONFIG.Main_Menu.updatePlaceholders(getPlaceholders());

                    Details = CONFIG.Main_Menu.Details;
                    State = CONFIG.Main_Menu.State;
                    LargeImageKey = CONFIG.Main_Menu.LargeImageKey;
                    LargeImageText = CONFIG.Main_Menu.LargeImageText;
                    SmallImageKey = CONFIG.Main_Menu.SmallImageKey;
                    SmallImageText = CONFIG.Main_Menu.SmallImageText;

                    if (CONFIG.Main_Menu.Button1.enabled) Button1 = new List<string> { CONFIG.Main_Menu.Button1.Label, CONFIG.Main_Menu.Button1.URL };
                    if (CONFIG.Main_Menu.Button2.enabled) Button2 = new List<string> { CONFIG.Main_Menu.Button2.Label, CONFIG.Main_Menu.Button2.URL };
                    break;
            }

            List<Button> buttonsList = new List<Button>();

            if (!string.IsNullOrEmpty(Button1[0]))
            {
                buttonsList.Add(new Button { Label = Button1[0], Url = Button1[1] });
            }

            if (!string.IsNullOrEmpty(Button2[0]))
            {
                buttonsList.Add(new Button { Label = Button2[0], Url = Button2[1] });
            }

            Button[] buttons = buttonsList.ToArray();

            rpcClient.SetPresence(new RichPresence() {
                Details = Details,
                State = State,
                Buttons = buttons,

                Assets = new Assets()
                {
                    LargeImageKey = LargeImageKey,
                    LargeImageText = LargeImageText,
                    SmallImageKey = SmallImageKey,
                    SmallImageText = SmallImageText
                }

            });
        }
    }
}