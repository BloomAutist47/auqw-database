/*  Filename: VersionEvaluator.cs
    Author: Bloom
*/


using RBot;
using System.Windows.Forms;

public class Script {
	public ScriptInterface bot => ScriptInterface.Instance;


	public void ScriptMain(ScriptInterface bot){

		VersionEvaluator("3.6.0.0");


	}

	///<summary>
	/// Checks if the current RBot version matches 
	/// which version of RBot you made this bot in.
	///
	/// Do NOT forget to use the namespace `using System.Windows.Forms;`
	/// </summary>
	/// <param name="NativeRBotVersion">The RBot version you made this bot in.</param>
	public void VersionEvaluator(string NativeRBotVersion) {
		var VersionEval = Application.ProductVersion.CompareTo(NativeRBotVersion);

		switch (VersionEval) {
			case 0:
				bot.Log($"[System] Bot Native Ver. ({NativeRBotVersion}) == RBot Current Version ({Application.ProductVersion}). Good.");
				break;
			case 1:
				bot.Log($"[System] Bot Native Ver. ({NativeRBotVersion}) < RBot Current Version ({Application.ProductVersion}). Good.");
				break;
			case -1:
				bot.Log($"[System] Bot Native Ver. ({NativeRBotVersion}) > RBot Current Version ({Application.ProductVersion}). Get the latest version. This bot will not work unless you do.");
				// Eternally loops and stops bot from going if this current rbot version is outdated relative to the native bot's version.
				while (true) {
					MessageNotify($"This bot uses RBot ({NativeRBotVersion}) Your version is ({Application.ProductVersion}). Get the Latest RBot you dumbfuck.");
					bot.Sleep(2000);
				}
		}
	}

	///<summary>
	/// Uses the black message bar on top of aqw UI to send a message
	/// </summary>
	/// <param name="Message">The text message</param>
	public void MessageNotify(string Message) {
		bot.CallGameFunction("MsgBox.notify", Message);
	}

}
