using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using System.IO;
using GTA.Native;
using GTA.Math;
using NativeUI;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace DeliveryMissionsV
{
    public class Main : Script
    {
        public static bool Debug = true, MarkerVisible;

        public static Vector3 StartBlipPosition = new Vector3(78.61977f, 111.8911f, 81.16819f);
        public static Blip StartBlip;

        public Main()
        {
            Tick += MainTick;
            Tick += DeliveryMenu.Tick;
            KeyDown += SavePos;
            Interval = 1;

            Helpers.ReadPositionsFromIni(Mission.DeliveryDestinations);
            DeliveryMenu.Init();

            StartBlip = World.CreateBlip(StartBlipPosition);
            StartBlip.Color = BlipColor.Green;
            StartBlip.Sprite = (BlipSprite)272;
            StartBlip.Name = "Delivery Missions";
        }

        public static void SavePos(object sender, KeyEventArgs e)
        {
            if (Game.IsKeyPressed(Keys.K))
            {
                File.WriteAllText("D:\\Programowanie\\GTA 5 Scripts\\DeliveryMissionsV\\SavedPos.txt",Game.Player.Character.Position.X.ToString() + ";" + Game.Player.Character.Position.Y.ToString() + ";" + Game.Player.Character.Position.Z.ToString());
                UI.ShowSubtitle("new Vector3(" + Game.Player.Character.Position.X.ToString("0.0", CultureInfo.GetCultureInfo("en-US")) + "," + Game.Player.Character.Position.Y.ToString("0.0", CultureInfo.GetCultureInfo("en-US")) + "," + Game.Player.Character.Position.Z.ToString("0.0", CultureInfo.GetCultureInfo("en-US")) + ")");
            }
        }

        public static void MainTick(object sender, EventArgs e)
        {
            MarkerVisible = !Game.MissionFlag;
            if (Game.Player.Character.IsInVehicle()) Helpers.DisplayHelpTextThisFrame(Game.Player.Character.CurrentVehicle.Health.ToString());
            if (MarkerVisible) World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(StartBlipPosition.X, StartBlipPosition.Y, StartBlipPosition.Z - 1.088f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Green);
            if (Mission.MissionStarted && Mission.MissionDestBlip.Alpha>0) World.DrawMarker(MarkerType.HorizontalCircleSkinny, Mission.DeliveryDestinations[Mission.DestID], new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(3, 3, 3), Color.Green);
            Mission.Main();
        }
    }
}



//cancel misji
//naprawić get back to van z początku ;c
//dodać configi
//dodać płace i notification na początku
//dodać timer