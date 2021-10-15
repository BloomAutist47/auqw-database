/*  Filename: SaveStateItem.cs
    Author: Bloom
    Summary: AQW saves your current sesssion/progres whenever you
             Change Maps, equip an item, or logout.
             This function "SaveStateItem()" will enter equip a cheap
             item to save your state.
    Remark:  DONT USE THIS. THIS IS TERRIBLE SHIT FUNCTION
             Because SaveState() is faster.
*/

using System;
using RBot;
using RBot.Items;
using System.Linq;

public class Script {
	public ScriptInterface bot => ScriptInterface.Instance;
    public string mapNumber = "9343";
    public int SavedState = 0;

	public void ScriptMain(ScriptInterface bot){


        // Just drop the function anywhere
        SaveStateItem();
        
	}


    /// <summary> 
    /// Buys a cheap item and equips it to save your state. Will re-equip your original weapon. 
    /// 1. Make sure to have a handler to sell "Horatio's Staff" once the bot ends.
    /// 2. If Inventory is full, will simply jump to yulgar then RETURN back to the place where you came from.
    /// 3. If Agro is on, will turn it off then on upon function finish
    /// 4. Returns player back to original position upon function finish
    /// </summary>
    /// <remarks> Finish time:
    /// Start to buy item -> 6s
    /// If not in combat -> 2s
    /// If in combat -> 5s
    /// If Join yulgar -> 6s
    /// </remarks>
    public void SaveStateItem() {
        // Start
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Saved State Started.");

        // Remembers location
        string CurrentMap = bot.Map.Name;
        string CurrentCell = bot.Player.Cell;
        string CurrentPad = bot.Player.Pad;

        // Turns off Agro. Will turn it on later if its on at first
        bool AgroTrue = false;
        if (bot.Options.AggroMonsters) {
            bot.Options.AggroMonsters = false;
            AgroTrue = true;
        }

        // Exits combat state
        ExitCombat();
        // if (AgroTrue) bot.Sleep(1500);

        // Saves Originally equipped Weapon
        Enum[] WeaponCategory = {ItemCategory.Staff, ItemCategory.Polearm, ItemCategory.Mace, ItemCategory.Bow, ItemCategory.Gun, ItemCategory.Dagger, ItemCategory.Axe, ItemCategory.Sword};

        InventoryItem EquippedItem = bot.Inventory.Items.Find(i => i.Equipped && WeaponCategory.Contains(i.Category));
        string OriginalWeapon = EquippedItem.Name;

        // Checks if player has the cheap item
        if (bot.Inventory.Contains("Horatio's Staff", 1)) {
            while (!bot.Inventory.IsEquipped("Horatio's Staff")) bot.Player.EquipItem("Horatio's Staff");
        } else {
            // Gets free space
            int FreeSpace = bot.GetGameObject<int>("world.myAvatar.objData.iBagSlots") - bot.GetGameObject<int>("world.myAvatar.items.length");
            // If enough Space, buys the item and equips it to save
            if (FreeSpace > 0) {
                bot.Shops.BuyItem(1, "Horatio's Staff");
                bot.Sleep(1500);
                bot.Player.EquipItem("Horatio's Staff");
            } 
            // if not enough Space, jumps to yulgar then returns back.
            else {
                if (bot.Map.Name != "yulgar") {
                    SafeMapJoin("yulgar", mapNumber, "Enter", "Spawn");
                } else {
                    SafeMapJoin("tavern", mapNumber, "Enter", "Spawn");
                }
                // bot.Sleep(1000);
                SafeMapJoin(CurrentMap, mapNumber, CurrentCell, CurrentPad);
                // bot.Sleep(1000);
                // Logs and returns
                SavedState += 1;
                bot.Log($"[{DateTime.Now:HH:mm:ss}] Saved State Ended {SavedState} time(s).");
                
                if (AgroTrue == true) bot.Options.AggroMonsters = true;
                return;

            }
        }

        bot.Sleep(500);

        // Equips Original item
        while (!bot.Inventory.IsEquipped(OriginalWeapon)) bot.Player.EquipItem(OriginalWeapon);
        

        // Returns back to cell
        if (bot.Player.Cell != CurrentCell) bot.Player.Jump(CurrentCell, CurrentPad);

        // Logs and returns
        SavedState += 1;
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Saved State Ended {SavedState} time(s).");

        if (AgroTrue == true) bot.Options.AggroMonsters = true;
        return;
    }


    public void ExitCombat() {

        // Moves Player to safezone in case in combat
        if (bot.Player.State == 2) bot.Player.Jump("Wait", "Spawn");
        while (bot.Player.State == 2) {}
    }

    public void SafeMapJoin(string mapName, string mapNumber, string cellName, string padName) {
        //Joins the specified map.

        maintainJoinLoop:
            ExitCombat();
            bot.Player.Join($"{mapName}-{mapNumber}", cellName, padName);
            bot.Wait.ForMapLoad(mapName);
            if (bot.Map.Name != mapName) goto maintainJoinLoop;
            if (bot.Player.Cell != cellName) bot.Player.Jump(cellName, padName);
            bot.Log($"[{DateTime.Now:HH:mm:ss}] Joined map {mapName}-{mapNumber} ({padName}, {cellName}).");
    }


}

