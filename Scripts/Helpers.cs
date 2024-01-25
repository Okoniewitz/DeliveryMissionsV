using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Globalization;
using System.IO;

namespace DeliveryMissionsV
{
    public static class Helpers
    {
        public static bool NearStartBlip(Vector3 pos, float dist)
        {
            return (Game.Player.Character.Position.DistanceTo(pos) <= dist);
        }
        public static void DisplayHelpTextThisFrame(string text) 
        {
            Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "STRING"); 
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text); 
            Function.Call(Hash._0x238FFE5C7B0498A6, 0, 0, 1, -1); 
        }

        public static void ReadPositionsFromIni(List<Vector3> ListToSave)
        {
            string[] Positions = File.ReadAllLines("scripts\\Okoniewitz\\Deliveries.ini");
            foreach(string Position in Positions)
            {
                float[] Pos = new float[3];
                string FromPosY = Position.Substring(Position.IndexOf(";")+1);
                string FromPosZ = FromPosY.Substring(FromPosY.IndexOf(";")+1);

                string[] PosString = new string[3];

                Pos[0] = float.Parse(Position.Substring(0, Position.IndexOf(";")));
                Pos[1] = float.Parse(FromPosY.Substring(0, FromPosY.IndexOf(";")));
                Pos[2] = float.Parse(FromPosZ);

                UI.ShowSubtitle("X: " + Pos[0] + " Y: " + Pos[1] + " Z: " + Pos[2]);
                ListToSave.Add(new Vector3(Pos[0], Pos[1], Pos[2]));
            }
        }
    }
}
