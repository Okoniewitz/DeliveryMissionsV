using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using NativeUI;

namespace DeliveryMissionsV
{
    public static class Mission
    {
        private static bool MenuActivateOnce, GoBackTextOnce;
        public static bool MissionStarted;
        private static Vehicle MissionVehicle;
        public static Blip MissionVehicleBlip, MissionDestBlip;
        public static int NearStartBlipDelay;
        public static int DeliveryTimer, DestID, PotPayment;
        public static float DestDist;

        public static List<Vector3> DeliveryDestinations = new List<Vector3>();

        public static void Main()
        {
            if (Helpers.NearStartBlip(DeliveryMissionsV.Main.StartBlipPosition, 1) && NearStartBlipDelay <= Game.GameTime)
            {
                if (Game.MissionFlag && !MenuActivateOnce) {Helpers.DisplayHelpTextThisFrame("You are already on a mission."); goto AfterNearBlip; }
                if (MissionStarted) { Helpers.DisplayHelpTextThisFrame("You have already started Delivery Mission.\n Press ~INPUT_PICKUP~ to stop it."); goto AfterNearBlip; }
                if (!MenuActivateOnce) DeliveryMenu.mainMenu.Visible = true;
                MenuActivateOnce = true;
            }
            else MenuActivateOnce = false;
            AfterNearBlip:;
            if (MissionStarted)
            {
                if (Game.Player.Character.IsInVehicle() && Game.Player.Character.CurrentVehicle == MissionVehicle) { MissionVehicleBlip.Alpha = 0; MissionDestBlip.Alpha = 255; GoBackTextOnce = false; }
                else
                {
                    if (!GoBackTextOnce)
                    {
                        UI.ShowSubtitle("Get into the van");
                        MissionVehicleBlip.Alpha = 255;
                        MissionDestBlip.Alpha = 0;
                        GoBackTextOnce = true;
                    }
                    if (Game.Player.Character.Position.DistanceTo2D(MissionVehicle.Position) > 200) MissionFailed(0);
                }
                if (MissionVehicle.IsDead) MissionFailed(1);
                if (Game.Player.Character.IsDead) MissionFailed(2);
                if (DeliveryMenu.TimerEnabled && DeliveryTimer <= Game.GameTime) MissionFailed(3);
                if (Helpers.NearStartBlip(DeliveryDestinations[DestID], 10f))
                {
                    MissionComplete();
                }
            }
        }
        public static void Start()
        {
            try { MissionVehicle.Delete(); } catch { }
            MissionStarted = true;
            MissionVehicle = World.CreateVehicle("Burrito", new Vector3(64.5216f, 122.1049f, 79.1449f), 150);
            MissionVehicle.IsPersistent = true;
            MissionVehicle.LockStatus = VehicleLockStatus.Unlocked;
            MissionVehicle.NeedsToBeHotwired = false;
            MissionVehicle.PlaceOnGround();
            MissionVehicleBlip = MissionVehicle.AddBlip();
            MissionVehicleBlip.Color = BlipColor.Green;
            MissionVehicleBlip.ShowRoute = false;
            MissionVehicleBlip.Name = "Delivery Mission Van";

            Random rnd = new Random();
            DestID = rnd.Next(DeliveryDestinations.Count);
            MissionDestBlip = World.CreateBlip(DeliveryDestinations[DestID]);
            MissionDestBlip.Color = BlipColor.Green;
            MissionDestBlip.Name = "Delivery Mission Destination";
            MissionDestBlip.ShowRoute = true;

            DestDist = (Game.Player.Character.Position.DistanceTo(DeliveryDestinations[DestID]) / 1000);
            int DestDistPay = (int)DestDist * 25;
            PotPayment = DestDistPay + 300;
        }

        private static void MissionComplete()
        {
            UI.ShowSubtitle("Get out of the van to complete the delivery");
            if (Game.Player.Character.CurrentVehicle!=MissionVehicle)
            {
                //int Payment = (PotPayment - 300) * (MissionVehicle.Health / 1000) + 300;

                BigMessageThread.MessageInstance.ShowColoredShard("Delivery Complete!", "Pay: " + PotPayment + "$ of " + PotPayment + "$ Vehicle health: "+(MissionVehicle.Health/100)+"%" , HudColor.HUD_COLOUR_BLACK, HudColor.HUD_COLOUR_YELLOW, 5000);
                Game.Player.Money += PotPayment;
                MissionStarted = false;
                MissionVehicle.LockStatus = VehicleLockStatus.Locked;
                MissionVehicle.IsPersistent = false;
                MissionVehicleBlip.Remove();
                MissionDestBlip.Remove();
            }
        }

        private static void MissionFailed(int Reason)
        {
            MissionStarted = false;
            string FailText = "";
            switch (Reason)
            {
                case 0: FailText = "You have abandoned the van"; break;
                case 1: FailText = "You have destroyed the van"; break;
                case 2: FailText = "You are dead"; break;
                case 3: FailText = "You ran out of time"; break;
            }

            BigMessageThread.MessageInstance.ShowColoredShard("Failed", FailText, HudColor.HUD_COLOUR_BLACK, HudColor.HUD_COLOUR_RED, 5000);
            MissionVehicleBlip.Remove();
            MissionDestBlip.Remove();
            NearStartBlipDelay = Game.GameTime + 5000;
            MissionVehicle.IsPersistent = false;
            Game.Player.Money -= 5000;
        }
    }
}
