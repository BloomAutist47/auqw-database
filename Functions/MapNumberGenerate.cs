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
        Random rnd = new Random();
        int randomDigits = rnd.Next(1000,99999);
        if(mapNumber=="1e99"){
            string radomDigitsText = randomDigits.ToString();
            mapNumber = radomDigitsText;
            return mapNumber;
        }
        else{
            try{
                int mapNoInt = Integer.parseInt(mapNumber);
                if(mapNoInt > 99999){
                    mapNoInt = randomDigits;
                    mapNumber = mapNoInt.ToString();
                    return mapNumber;
                }          
                return mapNumber;
            }
            catch(NumberFormatException ex){
                ex.printStackTrace();
                return mapNumber;
            }
        }
    }
}