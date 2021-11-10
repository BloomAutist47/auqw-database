/// <filename>: GameCalls.cs
/// <author>: Bloom 
/// <description>: List of useful flash calls not included in rbot yet


using RBot;


public class Testingclass {
    public ScriptInterface bot => ScriptInterface.Instance;

    public void ScriptMain(ScriptInterface bot) {


      HideMonsters(true);
      ShowFPSCounter(true);
      SetFPS(60);
    }



   /// <summary>
   /// Sets the game FPS
   /// </summary>
   /// <param name="FPS"> Frames per second </param>
   public void SetFPS(int FPS) {
      bot.SetGameObject("stage.frameRate", FPS);
   }


   /// <summary>
   /// Hides the monsters for performance
   /// </summary>
   /// <param name="Value"> true -> hides monsters. false -> reveals them </param>
   public void HideMonsters(bool Value) {
      switch(Value) {
         case true:
            if (!bot.GetGameObject<bool>("ui.monsterIcon.redX.visible")) {
               bot.CallGameFunction("world.toggleMonsters");
            }
            return;
         case false:
            if (bot.GetGameObject<bool>("ui.monsterIcon.redX.visible")) {
               bot.CallGameFunction("world.toggleMonsters");
            }
            return;

      }

   }

   /// <summary>
   /// Shows FPS counter
   /// </summary>
   /// <param name="Value"> true -> hides fps. false -> reveals them </param>
   public void ShowFPSCounter(bool Value) {
      switch(Value) {
         case true:
            if (!bot.GetGameObject<bool>("ui.mcFPS.visible")) {
               bot.CallGameFunction("world.toggleFPS");
            }
            return;
         case false:
            if (bot.GetGameObject<bool>("ui.mcFPS.visible")) {
               bot.CallGameFunction("world.toggleFPS");
            }
            return;
      }


   }

}