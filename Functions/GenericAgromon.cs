/*  Filename: GenericAgromon.cs
    Author: Bloom
*/

using RBot;
using RBot.Monsters;
using System;
using System.Collections.Generic;
public class Script 
{
   public ScriptInterface bot => ScriptInterface.Instance;


   public void ScriptMain(ScriptInterface bot) {
        string packet = GetAgroPacket("r3c");
        bot.Log($"{packet}");
   }


/// <summary>
/// Returns an agromon packet of every monster in a chosen room.
/// DO NOT forget to add `using RBot.Monsters;` on top of your script
/// else none of these will work.
/// </summary>
/// <param name="Cell">The cell name</param>
   public string GetAgroPacket(string Cell, string packet="None") {
    if (!bot.Map.Cells.Contains(Cell)) {
        return packet;
    }
    packet = $"%xt%zm%aggroMon%{bot.Map.RoomID.ToString()}%";

    foreach(Monster mons in bot.Monsters.GetMonstersByCell(Cell)) {
        packet += $"{mons.MapID}%";
    }

    return packet;
   }

}