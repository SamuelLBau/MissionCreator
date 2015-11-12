using System;

/// <summary>
/// Summary description for Class1
/// </summary>
class GlobalVariables2
{
    //-----------------------------THESE VARIABLES ARE ACROSS THE MAINFORM------------------------





    //------------------------------THESE VARIABLES ARE FOR GOAL PARAMATERS / OTHER DATA------------------
    static public string defaultValue = "-19023123";  //This is the value assigned if it is not included, will be "ignored" when printing to file

    //All of the below are the different possible values for everything

    //Goal array lists each type of goal, each value in this list should be associated with a unique panel and fieldArray, also requires initialization in housekeeping
    static public string[] goalArray = new string[] { "orbit", "parts", "EVA", "resource", "crash", "landing", "launch", "docking", "undocking", "repair", "noCrewGoal" };


    static public string[] bodyList = new string[] { "Kerbin", "Mun", "Minmus", "Kerbol", "Moho", "Eve", "Gilly", "Duna", "Ike", "Dres", "Jool", "Laythe", "Vall", "Tylo", "Bop", "Pol", "Eeloo" };


    static public string[] categoryArray = new string[] { "ORBIT", "PROBE", "IMPACT", "LANDING", "LAUNCH", "DOCKING", "SATELLITE", "MINING", "EVA", "TIME", "SCIENCE", "AVIATION", "MANNED", "ROVER", "REPAIR" };


    //Note: LaunchZoneList is different than body list by adding launch pad and runway and perhaps more later. May look into changing the arrays to jagged arrays so I can list these separatly. Not major issue.
    static public string[] launchZoneList = new string[] { "Kerbin", "Mun", "Minmus", "Kerbol", "Moho", "Eve", "Gilly", "Duna", "Ike", "Dres", "Jool", "Laythe", "Vall", "Tylo", "Bop", "Pol", "Eeloo", "launch pad", "runway" };

    /* Each field array follows the format {"what the code expects name to be"}
     *                                     {"What type of value it is"        } If this value is N/A, it will not be included in the list, mostly for names / descriptions (TO BE PROGRAMMED)
     *                                     {"Units"                           } Note: if it requires a supplemental array, such as Category in missionFIeldArray, it is not yet properly programmed  
     *                                     {"Name seen on GUI"                }


     * */
    //NOTE MISSION NAME, DESCRIPTION, ORDER AND SPECIAL SHOULD BE HANDLED ELSEWHERE IN THE PROGRAM
    static public string[,] missionFieldArray = new string[,] {{"name"        ,"category"          , "reward"        , "scienceReward"          , "requiresMission"        , "repeatable"        , "inOrder"               , "vesselIndependent"    , "passiveMission" , "passiveReward" , "clientControlled" , "destroyPunishment"     , "lifeTime" ,  "repeatableSameVessel" , "special",},
                                                                   {"N/A"         ,"checkedListbox"    , "int"           , "int"                    , "checkedListbox"         , ""                  , ""                      , ""                     , ""               , "int"           , ""                 , "int"                   , "TIME()"   ,  ""                     , ""       ,},
                                                                   {"N/A"         ,"categoryList"      , "K"             , "Science"                , "N/A"                    , "N/A"               , "N/A"                   , "N/A"                  , "N/A"            , "K"             , "N/A"              , "K"                     , "TIME()"   ,  "N/A"                  , "N/A"    ,},
                                                                   {"Mission Name","Category"          , "Mission reward", "Science reward"         , "Requires missions"      , "Repeatable Mission", "Goals must be in order", "Independant of vessel", "Passive mission", "Passive reward", "Client controlled", "Destruction punishment", "Life time", "Repeatable same vessel", "special",}};


    static public string[,] allGoalsFieldArray = new string[,] {{"reward"                    , "repeatable"     , "optional"     , "minSeconds"       , "minTotalMass"       , "maxTotalMass"       , "crewCount"          , "throttleDown"    },
                                                                    {"int"                       , ""               , ""             , "TIME()"           , "double"             , "double"             , "int"                , ""                },
                                                                    {"K"                         , "N/A"            , "N/A"          , "TIME()"           , "Tons"               , "Tons"               , "Crew"               , "N/A"             },
                                                                    {"Reward for goal completion", "Repeatable goal", "Optional goal", "Minimum goal time", "Minimum vessel mass", "Maximum vessel mass", "Minimum vessel crew", "Throttle down"   }};


    static public string[,] orbitFieldArray = new string[,] {{"body"       , "minEccentricity"     , "maxEccentricity"     ,"eccentricity" , "eccentricityPrecision" , "minPeA"           , "maxPeA"           , "minApA"          , "maxApA"          , "minLan"     , "maxLan"     , "minInclination"     , "maxInclination"     , "minOrbitalPeriod"      , "maxOrbitalPeriod"      , "minAltitude"     , "maxAltitude"     , "minSpeedOverGround"  , "maxSpeedOverGround"  , "minGForce"      , "maxGForce"      , "minVerticleSpeed"      , "maxVerticleSpeed"      },
                                                                 {"selection"  , "double"              , "double"              ,"double"       , "double"                , "double"           , "double"           , "double"          , "double"          , "double"     , "double"     , "double"             , "double"             , "TIME()"                , "TIME()"                , "double"          , "double"          , "double"              , "double"              , "double"         , "double"         , "double"                , "double"                },
                                                                 {"bodyList"   , "Ecc"                 , "Ecc"                 ,"Ecc"          , "%"                     , "Km"               , "Km"               , "Km"              , "Km"              , "°"          , "°"          , "°"                  , "°"                  , "TIME()"                , "TIME()"                , "Km"              , "Km"              , "Km"                  , "Km"                  , "g's"            , "g's"            , "m/s"                   , "m/s"                   },
                                                                 {"Target Body", "Minimum Eccentricity", "Maximum Eccentricity","Eccentricity" , "Eccentricity Precision", "Minimum periapsis", "Maximum periapsis", "Minimum Apoapsis", "Maximum Apoapsis", "Minimum LAN", "Maximum LAN", "Minimum inclination", "Maximum inclination", "Minimum orbital period", "Maximum orbital period", "Minimum Altitude", "Maximum Altitude", "Minimum ground speed", "Maximum ground speed", "Minimum G force", "Maximum G force", "Minimum verticle speed", "Maximum verticle speed"}};

    //note partsPanel is not currently prepared to add fields dynamically
    static public string[,] partsFieldArray = new string[,] { { },
                                                                  { },
                                                                  { },
                                                                  { }  };

    //note EVAPanel is not currently prepared to add fields dynamically
    static public string[,] EVAFieldArray = new string[,] { { },
                                                                { },
                                                                { },
                                                                { }  };

    //note resourcePanel is not currently prepared to add fields dynamically
    static public string[,] resourceFieldArray = new string[,] { { },
                                                                     { },
                                                                     { },
                                                                     { }  };

    static public string[,] crashFieldArray = new string[,] {{"body"      },
                                                                 {"selection"  },
                                                                 {"bodyList"   },
                                                                 {"Target Body"}};


    static public string[,] landingFieldArray = new string[,] { {"body"       , "splashedValid", "minLatitude"     , "maxLatitude"     , "minLongitude"     , "maxLongitude"     , "targetLatitude" , "targetLongitude" , "targetName" , "targetMinDistance" , "targetMaxDistance" },
                                                                    {"selection"  , ""             , "double"          , "double"          , "double"           , "double"           , "double"         , "double"          , "string"     , "double"            , "double"            },
                                                                    {"bodyList"   , "N/A"          , "°"               , "°"               , "°"                , "°"                , "°"              , "°"               , ""           , "m"                 , "m"                 },
                                                                    {"Target Body", "Ocean OK"     , "Minimum Latitude", "Maximum Latitude", "Minimum Longitude", "Maximum Longitude", "Target Latitude", "Target Longitude", "Target Name", "Min dist to Target", "Max dist to Target"}  };


    static public string[,] launchFieldArray = new string[,] { { "launchZone" },
                                                                   { "selection"  },
                                                                   { "bodyList"   },
                                                                   { "Target Body"}  };

    static public string[,] dockingFieldArray = new string[,] { { },
                                                                    { },
                                                                    { },
                                                                    { }  };


    static public string[,] undockingFieldArray = new string[,] { { },
                                                                      { },
                                                                      { },
                                                                      { }  };


    static public string[,] repairFieldArray = new string[,]  { {"minSeconds" },
                                                                    {"TIME()"     },
                                                                    {"TIME()"     },
                                                                    {"Minimum repair time" }  };




    static GlobalVariables2()
    {

    }
    static public Dictionary<string, string> getDictionary(string[,] fieldArray)                         //Dynamically creates each of the variables and sets it to the defaultValue (so it can be noted as unused later)
    {
        Dictionary<string, string> fieldDictionary = new Dictionary<string, string>();

        for (int i = 0; i < fieldArray.GetLength(1); i++)
        {
            fieldDictionary.Add(fieldArray[0, i], defaultValue);
        }
        return fieldDictionary;
    }
}
