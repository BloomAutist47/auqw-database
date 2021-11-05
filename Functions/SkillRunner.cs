/// <filename>: SkillRunner.cs
/// <author>: Bloom 
/// <description>: A complex Skill system that allows wait, health, and mana evaluation.
/// <note>: Automatically breaks when used by a retard.


using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using RBot;

/// <summary>
/// The Data structure for a skill. MUST HAVE
/// </summary>
public class DataSkill {
   public string Type;
   public int Index;
   public int sValue;

   public DataSkill(string Type, int Index=0, int sValue=0) {
      this.Type = Type;
      this.Index = Index-1;
      this.sValue = sValue;
   }
}



public class Script {
	public ScriptInterface bot => ScriptInterface.Instance;
	public bool EnableSkillRun = true;
   
   // Class Variable
   public List<DataSkill> SkillFarm = new List<DataSkill>();
   public List<DataSkill> SkillSolo = new List<DataSkill>();
   public string ClassFarm = "Legion Revenant";
   public string ClassSolo = "Void Highlord";
   public int SkillWait = 0; // inherent wait between each skill

	public void ScriptMain(ScriptInterface bot){
      /// <note> The skill index is the same as the aqw UI keys. that means auto attack is skill 1 and potion is skill 6
      // 1.) Declare skill string
      string Skill_VHL = "h2222>4, 6, 5, 3, h1000>2, 1";
		string Skill_LR = "1,2,3,4, w500, 5,6";

            // Skill String explanation
            
            /// Example: w500
            /// "w" = wait 
            /// "500" = milliseconds
            /// Therefore: Wait 500 miliseconds

            // Example: h222>4 
            /// "h" = health
            ///  "222" = health amount
            /// ">" = greater than
            /// "4" = skill to use
            /// Therefore: If health greater than 222, use skill 4

            // Example: m50<2
            /// "m" = mana
            ///  "50" = mana amount
            /// "<" = less than
            /// "2" = skill to use
            /// Therefore: If mana less than 50, use skill 2

            /// Format: mValueOs
            /// Notations:
            ///  m = h, m, w
            ///  Value = any positive integer
            ///  O = >, <
            ///  s = skill index

      // 2.) Convert Skills
      SkillConvert(new Dictionary<string, List<DataSkill>>() {
            {Skill_VHL, SkillSolo},
            {Skill_LR, SkillFarm}
         });

      // 3.) Skill use
      SkillUse("vhl");
      while (true) {
         if (bot.Player.Cell != "Enter") bot.Player.Jump("Enter", "Spawn");
         SkillUse("solo");
      }
   


	}

   /// ================================================ ///
   ///             Skill Section 
   /// ================================================ ///

   /// <summary>Converts Skill to readable format by the system</summary>
   /// <param name="Skillset">A dictionary containing the string list and datalist</param>
   public void SkillConvert(IDictionary<string, List<DataSkill>> Skillset) {
      List<int> result = new List<int>();
      foreach(KeyValuePair<string, List<DataSkill>> kvp in Skillset) {
         // bot.Log(kvp.Value);
         foreach(string a in kvp.Key.Split(',')) {
            string raw = a.Trim();
            if (string.IsNullOrEmpty(raw)) continue;
            if (raw.Contains("w")) {
               kvp.Value.Add(new DataSkill("w", sValue:GetNumbers(raw)) );
               continue;
            }
            if (raw.Contains("h") && raw.Contains(">")) {
               result = SplitInt(raw, '>');
               bot.Log($"{kvp.Key}");
               kvp.Value.Add(new DataSkill("h>", Index:result[1], sValue:result[0]) );
               continue;
            }
            if (raw.Contains("h") && raw.Contains("<")) {
               result = SplitInt(raw, '<');
               kvp.Value.Add(new DataSkill("h<", Index:result[1], sValue:result[0]) );
               continue;
            }
            if (raw.Contains("m") && raw.Contains(">")) {
               result = SplitInt(raw, '>');
               kvp.Value.Add(new DataSkill("m>", Index:result[1], sValue:result[0]) );
               continue;
            }
            if (raw.Contains("m") && raw.Contains("<")) {
               result = SplitInt(raw,'<');
               kvp.Value.Add(new DataSkill("m<", Index:result[1], sValue:result[0]) );
               continue;
            }
            try {
               kvp.Value.Add(new DataSkill("s", Index:int.Parse(raw)));
            } catch { }
            continue;
         }
      }
      


   }

   /// <summary>Starts attacking the target</summary>
   /// <param name="SkillSet">The skillset int list</param>
   public void SkillActivate(List<DataSkill> SkillSet, string MonsterTarget="*") {
      if (!EnableSkillRun) return;
      EnableSkillRun = false;

      if (bot.Monsters.Exists(MonsterTarget)) {
         bot.Player.Attack(MonsterTarget);
      } else {
         bot.Player.Attack("*");
      }

      foreach (DataSkill data in SkillSet) {
         bot.Log($"Type: {data.Type}  |  Index {data.Index}  |  sValue  {data.sValue}");
         switch(data.Type) {
            case "s":
               bot.Player.UseSkill(data.Index); 
               if (SkillWait  != 0) bot.Sleep(SkillWait); 
               continue;
            case "w":
               bot.Sleep(data.sValue);
               continue;
            case "h>":
               if (bot.Player.Health > data.sValue) {
                  bot.Player.UseSkill(data.Index); 
                  if (SkillWait  != 0) bot.Sleep(SkillWait); 
               }
               continue;
            case "h<":
               if (bot.Player.Health < data.sValue) {
                  bot.Player.UseSkill(data.Index); 
                  if (SkillWait  != 0) bot.Sleep(SkillWait); 
               }
               continue;
            case "m>":
               if (bot.Player.Mana > data.sValue) {
                  bot.Player.UseSkill(data.Index); 
                  if (SkillWait  != 0) bot.Sleep(SkillWait); 
               }
               continue;
            case "m<":
               if (bot.Player.Mana < data.sValue) {
                  bot.Player.UseSkill(data.Index); 
                  if (SkillWait  != 0) bot.Sleep(SkillWait); 
               }
               continue;
         }

      }
      EnableSkillRun = true;
   }


   /// <summary>
   /// Chooses which skill to use
   /// </summary>
   /// <param name="skill">The skill name</param>
   /// <param name="MonsterTarget">The monster name</param>
   public void SkillUse(string skill, string MonsterTarget="*") {
      if (EnableSkillRun == false) return;
      switch(skill) {
         case "farm":
            SafeEquip(ClassFarm);
            SkillActivate(SkillFarm, MonsterTarget);
            return;
         case "solo":
            SafeEquip(ClassSolo);
            SkillActivate(SkillSolo, MonsterTarget);
            return;
      }
   }

   /// ================================================ ///
   ///             Complementary Section 
   /// ================================================ ///

   /// <summary>
   /// Equips an item.
   /// </summary>
   public void SafeEquip(string ItemName) {
      while (InvHas(ItemName) && !bot.Inventory.IsEquipped(ItemName)) {
         ExitCombat();
         bot.Player.EquipItem(ItemName);
      }
   }     

   /// <summary> Leaves Combat</summary>
   public void ExitCombat() {
      bot.Options.AggroMonsters = false;
      bot.Player.Jump("Wait", "Spawn");
      while (bot.Player.State == 2) { }
      bot.Sleep(500);
   }




   /// ================================================ ///
   ///                  Utility Section 
   /// ================================================ ///

   /// <summary>
   /// Cleans a string of non-numerics and returns an integer
   /// </summary>
   /// <param name="delim">string to extract ints from</param>
   public int GetNumbers(string input) {
      return int.Parse(Regex.Replace(input, @"[^\d]+", "\n").Trim());
   }

   /// <summary>
   /// Extracts only the integers in a string
   /// </summary>
   /// <param name="input">string to split and extract ints from</param>
   /// <param name="delim">the delimeter char</param>
   public List<int> SplitInt(string input, char delim) {
      List<int> result = new List<int>();
      foreach(string health_item in input.Split(delim)) {
         result.Add(GetNumbers(health_item));
      }
      if (result.Count != 2) {
         result.Add(0);
      }
      return result;
   }

   /// <summary>
   /// Shortened inventory checker
   /// </summary>
   /// <param name="item">Item Name</param>
   /// <param name="Qty">Quantity of item to check</param>
   /// <param name="IsTemp">Where the item is temp or not</param>
   public bool InvHas(string item, int Qty=1, bool IsTemp=false) {
      if (!IsTemp) {
         return bot.Inventory.Contains(item, Qty);
      } else {
         return bot.Inventory.ContainsTempItem(item, Qty);
      }
      
   }


}


