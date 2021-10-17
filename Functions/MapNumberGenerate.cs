/*  Filename: MapNumberGenerate.cs
    Author: NNg_
    Summary: converts "1e99" into random digits
*/
using System;
using RBot;
using System.Collections.Generic;


public class Script{
	public ScriptInterface bot => ScriptInterface.Instance;
    public string mapNumber = "1e99";
    

	public void ScriptMain(ScriptInterface bot){
        mapNumber = MapNumberConverter(mapNumber);
    }

    public string MapNumberConverter(string mapNumber){
        if(mapNumber=="1e99"){
            Random rnd = new Random();
            int randomDigits = rnd.Next(10000,99999);
            string radomDigitsText = randomDigits.ToString();
            mapNumber = radomDigitsText;
            return mapNumber;
        }
        else{
            return mapNumber;
        }
    }
}