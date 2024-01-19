using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.Memory;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Game
{
    public static class Variables
    {
        public static int Target_FPS
        {
            get
            {
                return MemoryManager.Read<int>(Offsets.TARGET_FPS);
            }
            set
            {
                MemoryManager.Write<int>(Offsets.TARGET_FPS, value);
            }
        }

        public static int LevelId
        {
            get
            {
                return MemoryManager.Read<int>(Offsets.LEVEL_ID);
            }
        }

        public static IntPtr LevelInstance
        {
            get
            {
                return MemoryManager.Read<IntPtr>(Offsets.LEVEL_INSTANCE);
            }
        }

        public static bool IsLoading
        {
            get
            {
                return MemoryManager.Read<int>(Offsets.IS_LOADING) != 0;
            }
        }

        public static float PLAYER_X
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.PLAYER_X);
            }
            set
            {
                MemoryManager.Write<float>(Offsets.PLAYER_X, value);
            }
        }

        public static float PLAYER_Y
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.PLAYER_Y);
            }
            set
            {
                MemoryManager.Write<float>(Offsets.PLAYER_Y, value);
            }
        }

        public static float PLAYER_Z
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.PLAYER_Z);
            }
            set
            {
                MemoryManager.Write<float>(Offsets.PLAYER_Z, value);
            }
        }

        public static float PLAYER_RX
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.PLAYER_RX);
            }
            set
            {
                MemoryManager.Write<float>(Offsets.PLAYER_RX, value);
            }
        }

        public static float PLAYER_RY
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.PLAYER_RY);
            }
            set
            {
                MemoryManager.Write<float>(Offsets.PLAYER_RY, value);
            }
        }

        public static float PLAYER_RZ
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.PLAYER_RZ);
            }
            set
            {
                MemoryManager.Write<float>(Offsets.PLAYER_RZ, value);
            }
        }

        public static vec3_t Position
        {
            get
            {
                return MemoryManager.Read<vec3_t>(Offsets.PLAYER_X);
            }
            set
            {
                MemoryManager.Write<vec3_t>(Offsets.PLAYER_X, value);
            }
        }

        public static vec3_t Angles
        {
            get
            {
                return MemoryManager.Read<vec3_t>(Offsets.PLAYER_RX);
            }
            set
            {
                MemoryManager.Write<vec3_t>(Offsets.PLAYER_RX, value);
            }
        }

        public static float ViewDistance
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.VIEWDISTANCE);
            }
            set
            {
                MemoryManager.Write<float>(Offsets.VIEWDISTANCE, value);
            }
        }

        public static float FieldOfView
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.FIELDOFVIEW);
            }
            set
            {
                MemoryManager.Write<float>(Offsets.FIELDOFVIEW, value);
            }
        }

        public static int Width
        {
            get
            {
                return MemoryManager.Read<int>(Offsets.RESOLUTION_WIDTH);
            }
        }
        public static int Height
        {
            get
            {
                return MemoryManager.Read<int>(Offsets.RESOLUTION_HEIGHT);
            }
        }
        public static float WidthFloat
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.RESOLUTION_WIDTHFLOAT);
            }
        }

        public static float HeightFloat
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.RESOLUTION_HEIGHTFLOAT);
            }
        }

        public static float AspectRatio
        {
            get
            {
                return MemoryManager.Read<float>(Offsets.RESULTION_ASPECTRATIO);
            }
            set
            {
                MemoryManager.Write<float>(Offsets.RESULTION_ASPECTRATIO, value);
            }
        }

        public static int WindowTime
        {
            get
            {
                return MemoryManager.Read<int>(Offsets.WINDOW_TIME);
            }
        }

        public static int WindowTimeFocused
        {
            get
            {
                return MemoryManager.Read<int>(Offsets.WINDOW_TIME_FOCUS);
            }
        }

        public static int GameTime
        {
            get
            {
                return MemoryManager.Read<int>(Offsets.GAME_TIME);
            }
        }

        public static string GetLevelName(int levelId)
        {
            switch (levelId)
            {
                // 2033 Redux
                case 13613070:
                    return "Tower (Prologue)";
                case 11364390:
                    return "Hunter";
                case 12111516:
                    return "Exhibition";
                case 23459760:
                    return "Chase";
                case 7555304:
                    return "Riga";
                case 43309382:
                    return "Lost Tunnel";
                case 5055304:
                    return "Market";
                case 17551496:
                    return "Dead City";
                case 11632932:
                    return "Dry";
                case 42393914:
                    return "Ghosts";
                case 24570920:
                    return "Cursed";
                case 22705244:
                    return "Armory";
                case 20191422:
                    return "Front Line";
                case 25020278:
                    return "Trolley Combat";
                case 28304314:
                    return "Depot";
                case 46971900:
                    return "Defense";
                case 18233862:
                    return "Outpost";
                case 25194692:
                    return "Black Station";
                case 12945624:
                    return "Polis";
                case 15794468:
                    return "Alley";
                case 15579794:
                    return "Depository";
                case 8224820:
                    return "Archives";
                case 14964468:
                    return "Church";
                case 21847646:
                    return "Dark Star";
                case 24894220:
                    return "Cave";
                case 27371888:
                    return "D6";
                case 40533228:
                    return "Tower (Finale)";
                
                // Last Light (Redux)
                case 10156116:
                    return "Sparta";
                case 10825266:
                    return "Ashes";
                case 1070380:
                    return "Pavel";
                case 4115038:
                    return "Reich";
                case 7994250:
                    return "Seperation";
                case 7519198:
                    return "Facility";
                case 25987280:
                    return "Torchlight";
                case 9097920:
                    return "Echoes";
                case 9156618:
                    return "Bolshoi";
                case 2142010:
                    return "Korbut";
                case 13314050:
                    return "Revolution";
                case 8270544:
                    return "Regina";
                case 11343060:
                    return "Bandits";
                case 15610894:
                    return "Dark Water";
                case 4525036:
                    return "Venice";
                case 11142052:
                    return "Sundown";
                case 11910180:
                    return "Nightfall";
                case 17885320:
                    return "Undercity";
                case 15690530:
                    return "Contagion";
                case 8137878:
                    return "Quaratine";
                case 11073562:
                    return "Khan";
                case 12986928:
                    return "The Chase";
                case 11808914:
                    return "The Crossing";
                case 12119850:
                    return "Bridge";
                case 10080826:
                    return "Depot";
                case 13882126:
                    return "The Dead City";
                case 15135450:
                    return "Red Square";
                case 9863802:
                    return "The Garden";
                case 4239678:
                    return "Polis";
                case 4005458:
                    return "D6";

                // Last Light (Redux DLC)
                case 1948462:
                    return "DLC - Heavy Squad";
                case 8659570:
                    return "DLC - Kshatriya";
                case 12786154:
                    return "DLC - Sniper Team";
                case 4461344:
                    return "DLC - Tower Pack";
                case 11236060:
                    return "DLC - Spider Lair";
                case 11230334:
                    return "DLC - Pavel";
                case 5808252:
                    return "DLC - Khan";
                case 6718786:
                    return "DLC - Anna";
            }
            return "Unknown";
        }

        public static bool GetIsLevelValidLevel(int levelId)
        {
            switch (levelId)
            {
                // 2033 Redux
                case 13613070:
                    return true;
                case 11364390:
                    return true;
                case 12111516:
                    return true;
                case 23459760:
                    return true;
                case 7555304:
                    return true;
                case 43309382:
                    return true;
                case 5055304:
                    return true;
                case 17551496:
                    return true;
                case 11632932:
                    return true;
                case 42393914:
                    return true;
                case 24570920:
                    return true;
                case 22705244:
                    return true;
                case 20191422:
                    return true;
                case 25020278:
                    return true;
                case 28304314:
                    return true;
                case 46971900:
                    return true;
                case 18233862:
                    return true;
                case 25194692:
                    return true;
                case 12945624:
                    return true;
                case 15794468:
                    return true;
                case 15579794:
                    return true;
                case 8224820:
                    return true;
                case 14964468:
                    return true;
                case 21847646:
                    return true;
                case 24894220:
                    return true;
                case 27371888:
                    return true;
                case 40533228:
                    return true;

                // Last Light (Redux)
                case 10156116:
                    return true;
                case 10825266:
                    return true;
                case 1070380:
                    return true;
                case 4115038:
                    return true;
                case 7994250:
                    return true;
                case 7519198:
                    return true;
                case 25987280:
                    return true;
                case 9097920:
                    return true;
                case 9156618:
                    return true;
                case 2142010:
                    return true;
                case 13314050:
                    return true;
                case 8270544:
                    return true;
                case 11343060:
                    return true;
                case 15610894:
                    return true;
                case 4525036:
                    return true;
                case 11142052:
                    return true;
                case 11910180:
                    return true;
                case 17885320:
                    return true;
                case 15690530:
                    return true;
                case 8137878:
                    return true;
                case 11073562:
                    return true;
                case 12986928:
                    return true;
                case 11808914:
                    return true;
                case 12119850:
                    return true;
                case 10080826:
                    return true;
                case 13882126:
                    return true;
                case 15135450:
                    return true;
                case 9863802:
                    return true;
                case 4239678:
                    return true;
                case 4005458:
                    return true;

                // Last Light (Redux DLC)
                case 1948462:
                    return true;
                case 8659570:
                    return true;
                case 12786154:
                    return true;
                case 4461344:
                    return true;
                case 11236060:
                    return true;
                case 11230334:
                    return true;
                case 5808252:
                    return true;
                case 6718786:
                    return true;
            }
            return false;
        }
    }
}
