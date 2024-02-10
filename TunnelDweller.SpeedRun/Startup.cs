using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Moduling;
using TunnelDweller.NetCore.Rendering;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.SpeedRun
{
    public class Statup : ModuleEntryPoint
    {
        public static int CurrentLevel = -1;

        public static int Time = 0;

        public static bool IsRunning = false;

        public static TabItem SpeedRunTab = new TabItem("Speedrun");

        public static ComboBox cmbLevel = new ComboBox("Level", new string[] {
            "Prologue (Tower 1)",  
            "Hunter",
            "Exhibition",
            "Chase",
            "Riga",
            "Lost Tunnel",
            "Market",
            "Dead City",
            "Dry",
            "Ghosts",
            "Cursed",
            "Armory",
            "Front Line",
            "Trolley Combat",
            "Depot",
            "Defense",
            "Outpost",
            "Black Station",
            "Polis",
            "Alley",
            "Depository",
            "Archives",
            "Church",
            "Dark Star",
            "Cave",
            "D6",
            "Tower"
        });

        public static Button btnStart = new Button("Start Run", Begin);


        public override void Initialize()
        {
            SpeedRunTab.Controls.Add(cmbLevel);
            SpeedRunTab.Controls.Add(btnStart);

            Window.MainTabs.Items.Add(SpeedRunTab);

            Renderer.OnRender += Renderer_OnRender;
        }

        private void Renderer_OnRender(object sender, EventArgs e)
        {
            End();

            Font.Default().PushFont();

            ImGui.ImDrawText($"{cmbLevel.Values[cmbLevel.SelectedIndex]}: {TimeSpan.FromMilliseconds(Time).ToString("hh\\:mm\\:ss\\.fff")}", 16, 32, 12f, 255, 255, 255, 255);

            Font.Default().PopFont();
        }

        public static void Begin()
        {
            new Task(() =>
            {
                CConsole.ExecuteDeferred($"change_map {GetLevelFromIndex(cmbLevel.SelectedIndex)}");

                while (!Variables.IsLoading) { }

                while (Variables.IsLoading) 
                { 
                    Variables.IFT_ACUTAL = 0;
                    CurrentLevel = Variables.LevelId;
                    IsRunning = true;
                }

            }).Start();
        }

        public static void End()
        {
            if (IsRunning)
                Time = Variables.GameTime;

            if(Variables.IsLoading && Variables.LevelId != CurrentLevel)
            {
                IsRunning = false;
            }
        }

        public static string GetLevelFromIndex(int index)
        {
            switch(index)
            {
                case 0:
                    return @"2033\l00_intro";
                case 1:
                    return @"2033\l01_hunter";
                case 2:
                    return @"2033\l02_exhibition";
                case 3:
                    return @"2033\l03_chase";
                case 4:
                    return @"2033\l04_riga";
                case 5:
                    return @"2033\l05_lost_tunnel";
                case 6:
                    return @"2033\l08_market";
                case 7:
                    return @"2033\l09_dead_city_1";
                case 8:
                    return @"2033\l11_dry";
                case 9:
                    return @"2033\l12_ghosts";
                case 10:
                    return @"2033\l14_cursed";
                case 11:
                    return @"2033\l15_armory";
                case 12:
                    return @"2033\l16_frontline";
                case 13:
                    return @"2033\l17_trolley_combat";
                case 14:
                    return @"2033\l18_depot";
                case 15:
                    return @"2033\l19_defence";
                case 16:
                    return @"2033\l21_nazi_outpost";
                case 17:
                    return @"2033\l22_black";
                case 18:
                    return @"2033\l23_polis";
                case 19:
                    return @"2033\l24_alley";
                case 20:
                    return @"2033\l26_depository";
                case 21:
                    return @"2033\l27_archives";
                case 22:
                    return @"2033\l28_driving";
                case 23:
                    return @"2033\l30_darkstar";
                case 24:
                    return @"2033\l32_cave";
                case 25:
                    return @"2033\l33_d6";
                case 26:
                    return @"2033\l36_ostankino";
                default:
                    return @"2033\000";
            }
        }
    }
}
