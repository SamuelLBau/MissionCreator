using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MissionCreator
{
    class GV
    {
        //while editing, set Debug == true, for release, set to false
        static public bool debug = true;



        static public int mainFormNumber = 0;
        static public int goalPanelHeight = 311;
        static public int detailPanelInitialY = 290;
        static public int maxNumberSubPanels = 20;
        static public int goalBoxIncrement = 90;

        static public string newMissionName = "New Mission";


        static public Color inactiveMissionColor = Color.LightGray;
        static public Color inactiveGoalColor = Color.LightGray;

        static public Color activeMissionColor = Color.DimGray;
        static public Color activeGoalColor = Color.DimGray;

        static public Color inactiveButtonColor = Color.White;
        static public Color activeButtonColor = Color.Lime;
        static public Color invalidButtonColor = Color.Red;
        static public Color goalPanelColor = Color.DimGray;

        static public Bitmap backPanelImage = global::MissionCreator.Properties.Resources.PanelBack;
        static public Bitmap goaLPanelImage = global::MissionCreator.Properties.Resources.goalPanel_3;


        static public Font generalFont = new Font("Century Schoolbook", 9, FontStyle.Bold);
        static public Font titleFont = new Font("MS Reference Sans Serif", 10, FontStyle.Bold | FontStyle.Italic);

        //------------------------------THESE VARIABLES ARE FOR GOAL PARAMATERS / OTHER DATA------------------
        static public string defaultValue = "-19023123";  //This is the value assigned if it is not included, will be "ignored" when printing to file

        //All of the below are the different possible values for everything
        static public int CLBItemHeight = 16;

        static public string missionInfoPanelName = "missionInfoPanel";
        static public string goalCreatorPanelName = "goalCreatorPanel";
        static public string goalDetailPanelName = "goalDetailPanel";
        static public string missionPackagePanelName = "missionPackagePanel";
        static public string allMissionInfoPanelName = "allMissionInfoPanel";

        static public string numberPartTypesTBName = "numberPartTypesTextBox";
        static public string numberResourceTypesTBName = "numberResourceTypesTextBox";

        static public string partNameTextBoxName = "partNameTextBox";
        static public string minPartRequiredName = "minPartsRequired";
        static public string maxPartRequiredName = "maxPartsRequired";

        static public string resourceNameTextBoxName = "resourceNameTextBox";
        static public string minResourceRequiredName = "minResourcesRequired";
        static public string maxResourceRequiredName = "maxResourcesRequired";



        static public string[] bodyList = new string[] { "Kerbin", "Mun", "Minmus", "Kerbol", "Moho", "Eve", "Gilly", "Duna", "Ike", "Dres", "Jool", "Laythe", "Vall", "Tylo", "Bop", "Pol", "Eeloo" };


        static public string[] categoryArray = new string[] { "ORBIT", "PROBE", "IMPACT", "LANDING", "LAUNCH", "DOCKING", "SATELLITE", "MINING", "EVA", "TIME", "SCIENCE", "AVIATION", "MANNED", "ROVER", "REPAIR" };

        static public Bitmap upArrowIcon = global::MissionCreator.Properties.Resources.up_arrow;
        static public Bitmap downArrowIcon = global::MissionCreator.Properties.Resources.down_arrow;
        static public Bitmap removeIcon = global::MissionCreator.Properties.Resources.removeIcon;
        static public List<Bitmap> categoryIconList = new List<Bitmap>()
        {
            null,  //no ORBIT icon
            global::MissionCreator.Properties.Resources.PROBECategory,
            global::MissionCreator.Properties.Resources.IMPACTCategory,
            global::MissionCreator.Properties.Resources.LandingCategory,
            global::MissionCreator.Properties.Resources.LAUNCHCategory,
            global::MissionCreator.Properties.Resources.DockingCategory,
            global::MissionCreator.Properties.Resources.SATELLITECategory,
            null,  //no MINING icon
            global::MissionCreator.Properties.Resources.EVA2Category,
            global::MissionCreator.Properties.Resources.TIMECategory,
            global::MissionCreator.Properties.Resources.SCIENCECategory,
            global::MissionCreator.Properties.Resources.planeCategory,
            global::MissionCreator.Properties.Resources.EVACategory,
            global::MissionCreator.Properties.Resources.ROVERCategory,
            global::MissionCreator.Properties.Resources.REPAIRCategory,
        };
        //Note: LaunchZoneList is different than body list by adding launch pad and runway and perhaps more later. May look into changing the arrays to jagged arrays so I can list these separatly. Not major issue.
        static public string[] launchZoneList = new string[] { "Kerbin", "Mun", "Minmus", "Kerbol", "Moho", "Eve", "Gilly", "Duna", "Ike", "Dres", "Jool", "Laythe", "Vall", "Tylo", "Bop", "Pol", "Eeloo", "launch pad", "runway" };

        /* Each field array follows the format {"what the code expects name to be"}
         *                                     {"What type of value it is"        } If this value is N/A, it will not be included in the list, mostly for names / descriptions (TO BE PROGRAMMED)
         *                                     {"Units"                           } Note: if it requires a supplemental array, such as Category in missionFIeldArray, it is not yet properly programmed  
         *                                     {"Name seen on GUI"                }


         * */
        //NOTE MISSION NAME, DESCRIPTION, ORDER AND SPECIAL SHOULD BE HANDLED ELSEWHERE IN THE PROGRAM

        static public string[] missionPackageArray = new string[] { "name", "description", "ownOrder", };

        static public string[,] missionFieldArray = new string[,] {{"name"        ,"description" ,"category"          , "reward"        , "scienceReward"          , "requiresMission"        , "repeatable"        , "inOrder"               , "vesselIndependent"    , "passiveMission" , "passiveReward" , "clientControlled" , "destroyPunishment"     , "lifeTime" ,  "repeatableSameVessel" , "special"},
                                                                   {"N/A"         ,"N/A"         ,"checkedListbox"    , "int"           , "int"                    , "checkedListbox"         , "bool"              , "bool"                  , "bool"                 , "bool"           , "int"           , "bool"             , "int"                   , "TIME()"   ,  "bool"                 , "bool"   },
                                                                   {"N/A"         ,"N/A"         ,"categoryList"      , "K"             , "Sci."                   , "N/A"                    , "N/A"               , "N/A"                   , "N/A"                  , "N/A"            , "K"             , "N/A"              , "K"                     , "TIME()"   ,  "N/A"                  , "N/A"    },
                                                                   {"Mission Name","N/A"         ,"Category"          , "Mission reward", "Science reward"         , "Requires missions"      , "Repeatable Mission", "Goals must be in order", "Independant of vessel", "Passive mission", "Passive reward", "Client controlled", "Destruction punishment", "Life time", "Repeatable same vessel", "special"}};


        static public string[,] allGoalsFieldArray = new string[,] {{"goalType","description", "reward"                    , "repeatable"    , "optional"     , "minSeconds"       , "minTotalMass"       , "maxTotalMass"       , "crewCount"          , "throttleDown"    },
                                                                    {"N/A"     ,"description", "int"                       , "bool"           , "bool"         , "TIME()"           , "double"             , "double"             , "int"                , "bool"            },
                                                                    {"N/A"     ,"N/A"        , "K"                         , "N/A"            , "N/A"          , "TIME()"           , "Tons"               , "Tons"               , "Crew"               , "N/A"             },
                                                                    {"N/A"     ,"N/A"        , "Reward for goal completion", "Repeatable goal", "Optional goal", "Minimum goal time", "Minimum vessel mass", "Maximum vessel mass", "Minimum vessel crew", "Throttle down"   }};


        static public string[,] orbitFieldArray = new string[,] {{"body"       , "minEccentricity"     , "maxEccentricity"     ,"eccentricity" , "eccentricityPrecision" , "minPeA"           , "maxPeA"           , "minApA"          , "maxApA"          , "minLan"     , "maxLan"     , "minInclination"     , "maxInclination"     , "minOrbitalPeriod"      , "maxOrbitalPeriod"      , "minAltitude"     , "maxAltitude"     , "minSpeedOverGround"  , "maxSpeedOverGround"  , "minGForce"      , "maxGForce"      , "minVerticleSpeed"      , "maxVerticleSpeed"      },
                                                                 {"selection"  , "double"              , "double"              ,"double"       , "double"                , "double"           , "double"           , "double"          , "double"          , "double"     , "double"     , "double"             , "double"             , "TIME()"                , "TIME()"                , "double"          , "double"          , "double"              , "double"              , "double"         , "double"         , "double"                , "double"                },
                                                                 {"bodyList"   , "Ecc"                 , "Ecc"                 ,"Ecc"          , "%"                     , "m"                , "m"                , "m"               , "m"               , "°"          , "°"          , "°"                  , "°"                  , "TIME()"                , "TIME()"                , "m"               , "m"               , "m"                   , "m"                   , "g's"            , "g's"            , "m/s"                   , "m/s"                   },
                                                                 {"Target Body", "Minimum Eccentricity", "Maximum Eccentricity","Eccentricity" , "Eccentricity Precision", "Minimum periapsis", "Maximum periapsis", "Minimum Apoapsis", "Maximum Apoapsis", "Minimum LAN", "Maximum LAN", "Minimum inclination", "Maximum inclination", "Minimum orbital period", "Maximum orbital period", "Minimum Altitude", "Maximum Altitude", "Minimum ground speed", "Maximum ground speed", "Minimum G force", "Maximum G force", "Minimum verticle speed", "Maximum verticle speed"}};

        //note partsPanel is not currently prepared to add fields dynamically
        static public string[,] partFieldArray = new string[,] {  {"partList"     , "partName"    , "partCount"     , "maxPartCount"},
                                                                  {"partSubPanel" , "partValue"   , "partValue"     ,"partValue"    },
                                                                  {"N/A"          , "N/A"         , "N/A"           , "N/A"         },
                                                                  {"N/A"          , "N/A"         , "N/A"           , "N/A"         }  };

        //note EVAPanel is not currently prepared to add fields dynamically
        static public string[,] EVAFieldArray = new string[,] { { },
                                                                { },
                                                                { },
                                                                { }  };

        //note resourcePanel is not currently prepared to add fields dynamically
        static public string[,] resourceFieldArray = new string[,] { {"resourceList"     , "name"            , "minAmount"         , "maxAmount"       },
                                                                     {"resourceSubPanel" , "resourceValue"   , "resourceValue"     ,"resourceValue"    },
                                                                     {"N/A"              , "N/A"             , "N/A"               , "N/A"             },
                                                                     {"N/A"              , "N/A"             , "N/A"               , "N/A"             }  };

        static public string[,] crashFieldArray = new string[,] {{"body"      },
                                                                 {"selection"  },
                                                                 {"bodyList"   },
                                                                 {"Target Body"}};


        static public string[,] landingFieldArray = new string[,] { {"body"       , "splashedValid", "minLatitude"     , "maxLatitude"     , "minLongitude"     , "maxLongitude"     , "targetLatitude" , "targetLongitude" , "targetName" , "targetMinDistance" , "targetMaxDistance" },
                                                                    {"selection"  , "bool"         , "double"          , "double"          , "double"           , "double"           , "double"         , "double"          , "string"     , "double"            , "double"            },
                                                                    {"bodyList"   , "N/A"          , "°"               , "°"               , "°"                , "°"                , "°"              , "°"               , ""           , "m"                 , "m"                 },
                                                                    {"Target Body", "Ocean OK"     , "Minimum Latitude", "Maximum Latitude", "Minimum Longitude", "Maximum Longitude", "Target Latitude", "Target Longitude", "Target Name", "Min dist to Target", "Max dist to Target"}  };


        static public string[,] launchFieldArray = new string[,] { { "launchZone" },
                                                                   { "selection"  },
                                                                   { "launchZones"   },
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

        static public string[,] noCrewFieldArray = new string[,] { { },
                                                                   { },
                                                                   { },
                                                                   { } };

        //NOTE: TO ADD TO THIS YOU MUST ADD THE GOAL TYPE AND THE CORRESPONDING ARRAY
        static public string[][,] goalTypeList = { new string[,] {{ "Orbit"        , "Part"         , "EVA"        , "Resource"        , "Crash"        , "Landing"        , "Launch"        , "Docking"        , "Undocking"        , "Repair"       , "NoCrew"         },
                                                                  { ""             , ""             , ""           , ""                , ""             , ""               , ""              , ""               , ""                 , ""             , ""               }},
                                                                    orbitFieldArray, partFieldArray, EVAFieldArray, resourceFieldArray, crashFieldArray, landingFieldArray, launchFieldArray, dockingFieldArray, undockingFieldArray, repairFieldArray, noCrewFieldArray };

        static public string[] partsList = returnPartsList();
        static public string[] resourceList = returnResourceList();



        static GV()
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
        static private string[] returnArray(string[,] goalList)
        {
            int length = goalList.GetLength(0);
            string[] stringArray = new string[length];
            for (int i = 0; i < length; i++)
            {
                stringArray[i] = goalList[0, i];
            }
            return stringArray;
        }
        static private string[] returnPartsList()
        {
            string[] partsList = { };
            string gameDataPath = System.Reflection.Assembly.GetExecutingAssembly().Location;


            return partsList;
        }
        static private string[] returnResourceList()
        {
            string[] resourceList = { };

            return resourceList;
        }
    }
}
