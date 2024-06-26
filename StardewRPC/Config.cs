﻿using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewRPC.Config;

namespace StardewRPC {
    public sealed class Config {
        public bool Enable { get; set; } = true;
        public String ClientID { get; set; } = "1233785540982345870";
        public int Update_Rate_Seconds { get; set; } = 10;

        public class rpcButton {
            public Boolean enabled { get; set; }
            public string Label { get; set; }
            public string URL { get; set; }
        }

        public class mainMenu { 
            public String Details { get; set; } = "Playing modded Stardew Valley";
            public String State { get; set; } = "In the Main Menu";
            public String LargeImageKey { get; set; } = "stardew_logo";
            public String LargeImageText { get; set; } = "Playing modded Stardew Valley with %MODS% mods!";
            public String SmallImageKey { get; set; } = "chicken";
            public String SmallImageText { get; set; } = "Playing modded Stardew Valley with %MODS% mods!";
          
            public rpcButton Button1 { get; set; } = new rpcButton { enabled = false, Label = "Button 1", URL = "https://www.curseforge.com/stardewvalley/mods/stardewrpc" };
            public rpcButton Button2 { get; set;  } = new rpcButton { enabled = false, Label = "Button 2", URL = "https://www.curseforge.com/stardewvalley/mods/stardewrpc" };

            public void updatePlaceholders(Dictionary<string, string> dictionary) {
                foreach (var property in this.GetType().GetProperties()) {
                    if (property.PropertyType == typeof(string)) {
                        string value = (string)property.GetValue(this);

                        foreach (var kvp in dictionary) {
                            value = value.Replace(kvp.Key, kvp.Value);
                        }

                        property.SetValue(this, value);
                    }
                }
            }
        }

        public class singlePlayer
        {
            public String Details { get; set; } = "Playing modded Stardew Valley";
            public String State { get; set; } = "Playing Alone";
            public String LargeImageKey { get; set; } = "stardew_logo";
            public String LargeImageText { get; set; } = "Playing modded Stardew Valley with %MODS% mods!";
            public String SmallImageKey { get; set; } = "chicken";
            public String SmallImageText { get; set; } = "Playing modded Stardew Valley with %MODS% mods!";

            public rpcButton Button1 { get; set; } = new rpcButton { enabled = false, Label = "Button 1", URL = "https://www.curseforge.com/stardewvalley/mods/stardewrpc" };
            public rpcButton Button2 { get; set; } = new rpcButton { enabled = false, Label = "Button 2", URL = "https://www.curseforge.com/stardewvalley/mods/stardewrpc" };

            public void updatePlaceholders(Dictionary<string, string> dictionary)
            {
                foreach (var property in this.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(string))
                    {
                        string value = (string) property.GetValue(this);

                        foreach (var kvp in dictionary)
                        {
                            value = value.Replace(kvp.Key, kvp.Value);
                        }

                        property.SetValue(this, value);
                    }
                }
            }
        }

        public class multiPlayer
        {
            public String Details { get; set; } = "Playing Modded Stardew Valley";
            public String State { get; set; } = "Playing with %PLAYER_COUNT_FRIENDS% friends!";
            public String LargeImageKey { get; set; } = "stardew_logo";
            public String LargeImageText { get; set; } = "Playing modded Stardew Valley with %MODS% mods!";
            public String SmallImageKey { get; set; } = "chicken";
            public String SmallImageText { get; set; } = "Playing modded Stardew Valley with %MODS% mods!";

            public rpcButton Button1 { get; set; } = new rpcButton { enabled = false, Label = "Button 1", URL = "https://www.curseforge.com/stardewvalley/mods/stardewrpc" };
            public rpcButton Button2 { get; set;  } = new rpcButton { enabled = false, Label = "Button 2", URL = "https://www.curseforge.com/stardewvalley/mods/stardewrpc" };

            public void updatePlaceholders(Dictionary<string, string> dictionary)
            {
                foreach (var property in this.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(string))
                    {
                        string value = (string)property.GetValue(this);

                        foreach (var kvp in dictionary)
                        {
                            value = value.Replace(kvp.Key, kvp.Value);
                        }

                        property.SetValue(this, value);
                    }
                }
            }
        }

        public mainMenu Main_Menu { get; set; } = new mainMenu();
        public singlePlayer Singleplayer { get; set; } = new singlePlayer();
        public multiPlayer Multiplayer { get; set; } = new multiPlayer();
    }
}
