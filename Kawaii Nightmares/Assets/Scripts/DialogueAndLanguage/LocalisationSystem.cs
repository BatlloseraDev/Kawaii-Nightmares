using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalisationSystem 
{
 
    public enum Language 
    { 
        English,
        Spanish
    }

    public static Language language = Language.Spanish;

    private static Dictionary<string, string> localisedEN;
    private static Dictionary<string, string> localisedSP;

    public static bool isInit; 


    public static void Init()
    {
        CSVLoader csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        localisedEN = csvLoader.GetDictionaryValues("en");
        localisedSP = csvLoader.GetDictionaryValues("sp");

        isInit = true; 
    }

    public static string GetLocalisedValue(string key)
    {
        if (!isInit) { Init(); }
        string value = key;

        switch (language)
        {
            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;

            case Language.Spanish:
                localisedSP.TryGetValue(key, out value);
                break; 
        }
        return value; 
    }
}
