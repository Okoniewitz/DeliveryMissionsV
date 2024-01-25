using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeUI;
using GTA;
using System.Windows.Input;
using System.IO;
using System.Windows.Forms;

namespace DeliveryMissionsV
{
    public static class DeliveryMenu
    {
        private static MenuPool menuPool;
        public static UIMenu mainMenu;

        public static bool TimerEnabled;
        public static string DifficultyLevel;
        public static void Init()
        {
            menuPool = new MenuPool();
            mainMenu = new UIMenu("Delivery Missions", "~b~ Delivery Missions Menu");
            menuPool.Add(mainMenu);
            AddMenuTimer(mainMenu);
            AddMenuDifficulty(mainMenu);
            AddMenuStart(mainMenu);
            menuPool.RefreshIndex();
        }
        public static void AddMenuTimer(UIMenu menu)
        {
            var newitem = new UIMenuCheckboxItem("Timer", TimerEnabled);
            menu.AddItem(newitem);
            menu.OnCheckboxChange += (sender, item, checked_) =>
            {
                if (item == newitem)
                {
                    TimerEnabled = checked_;
                }
            };
        }
        public static void AddMenuDifficulty(UIMenu menu)
        {
            var newitem = new UIMenuListItem("Difficulty", new List<dynamic> { "Easy", "Normal", "Hard" }, 1);
            menu.AddItem(newitem);
            menu.OnListChange += (sender, item, index) =>
            {
                if (item == newitem)
                {
                    DifficultyLevel = item.IndexToItem(index).ToString();
                }

            };
        }
        public static void AddMenuStart(UIMenu menu)
        {
            var newitem = new UIMenuItem("Start!", "Start the mission.");
            newitem.SetRightBadge(UIMenuItem.BadgeStyle.Tick);
            menu.AddItem(newitem);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == newitem)
                {
                    if(Main.Debug)UI.ShowSubtitle("Difficulty: " + DifficultyLevel + " Timer: " + TimerEnabled);
                    Mission.Start();
                    Mission.NearStartBlipDelay = Game.GameTime + 2000;
                    mainMenu.Visible = false;
                }
            };
        }
        public static void Tick(object sender, EventArgs e)
        {
            menuPool.ProcessMenus();
            if (mainMenu.Visible) Game.Player.CanControlCharacter = false; else Game.Player.CanControlCharacter = true;
        }
    }
}
