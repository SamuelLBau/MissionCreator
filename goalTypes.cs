using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//TODO NOTE: need to add a saver method or class that can be called and will search each item on a panel and, if the checkbox is checked
//will save it to something
namespace MissionCreator
{
    public class AllGoals
    {
        public Dictionary<string, string> allGoalFieldDictionary = new Dictionary<string, string>();
        public Dictionary<string, Dictionary<string, string>> allFieldDictionary = new Dictionary<string, Dictionary<string, string>>();
        public AllGoals()
        {
            int length = GV.goalTypeList[0].GetLength(1);
            for(int i = 0; i < length; i++)
            {
                allFieldDictionary.Add(GV.goalTypeList[0][0,i], GV.getDictionary(GV.goalTypeList[i+1]));    //ties together a goalType and it's respective array of values
            }

            //GlobalVariables GlobalVariables = new GlobalVariables("populate");
            allGoalFieldDictionary = GV.getDictionary(GV.allGoalsFieldArray);
            allGoalFieldDictionary["goalType"] = "Orbit";
        }
        public void editAllGoal(string key, string newValue)
        {
            if (allGoalFieldDictionary.ContainsKey(key))
            {
                allGoalFieldDictionary[key] = newValue;
            }
            else { Console.WriteLine("Error, Not AllGoalFieldDictionary"); }
        }
        public void editGoalField(string goalType, string key, string newValue)
        {
            if (allFieldDictionary[goalType].ContainsKey(key))
            {
                allFieldDictionary[goalType][key] = newValue;
            }
            else 
            { 
                Console.WriteLine("Error, not a Valid goal Edit"); 
            }
        }

        public string getAllGoal(string key)
        {
            return allGoalFieldDictionary[key];
        }
        public string getGoalField(string goalType, string key)
        {
            return allFieldDictionary[goalType][key];
        }
    }
}
