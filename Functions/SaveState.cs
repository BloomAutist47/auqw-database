/*  Filename: SaveState.cs
    Author: Bloom
    Summary: AQW saves your current sesssion/progres whenever you
             Change Maps, equip an item, or logout.
             This function "SaveState()" will enter /yulgar to
             save your state.
*/

using System;
using RBot;


public class Script {
	public ScriptInterface bot => ScriptInterface.Instance;
    public string mapNumber = "9343";
    public int SavedState = 0;

	public void ScriptMain(ScriptInterface bot){

        // Just drop the function anywhere
        SaveState();
        
	}


    /// <summary>
    /// Joins /yulgar or /tavern to save your current session.
    /// Will return back to initial map and return back agro state.
    /// </summary>
    /// <remarks>Takes 4s.</remarks>
    public void SaveState() {

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

        ExitCombat();

        if (bot.Map.Name != "yulgar") {
            SafeMapJoin("yulgar", mapNumber, "Enter", "Spawn");
        } else {
            SafeMapJoin("tavern", mapNumber, "Enter", "Spawn");
        }
        SafeMapJoin(CurrentMap, mapNumber, CurrentCell, CurrentPad);

        // Logs and returns
        SavedState += 1;
        bot.Log($"[{DateTime.Now:HH:mm:ss}] Saved State Ended {SavedState} time(s).");

        // Turns agro back on if initially turned on.
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

