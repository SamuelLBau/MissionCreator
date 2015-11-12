using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace MissionCreator
{
    public class Mission
    {
        public int activeGoalID = 0;
        public Dictionary<string, string> fieldDictionary = new Dictionary<string, string>();
        private MissionPackage missionPackage;
        public List<AllGoals> goalList = new List<AllGoals>();


        public Mission(string missionName, MissionPackage sender)
        {
            missionPackage = sender;                                                           //used to navigate to missionPackage stuff
            string[,] missionFieldArray = GV.missionFieldArray;
            for (int i = 0; i < missionFieldArray.GetLength(1); i++)
            {
                fieldDictionary.Add(missionFieldArray[0, i], GV.defaultValue);
            }
            fieldDictionary["name"] = missionName;
        }
        public void setMissionValue(string key, string value)                             //This is here to add an "EVENT" when the dictionary is changed
        {
            fieldDictionary[key] = value;
            doStuff(key, value);
        }
        public string getMissionValue(string key)
        {
            //GlobalVariables GV = new GlobalVariables();
            string value = GV.defaultValue;
            fieldDictionary.TryGetValue(key, out value);
            return value;
        }
        public void doStuff(string key, string value)
        {
            mainForm MF = missionPackage.MF;
            switch (key)
            {
                case "scienceReward":
                case "reward":
                    MissionPackagePanel MIP = (MissionPackagePanel)MF.Controls[GV.missionPackagePanelName];
                    foreach(Control control in MIP.Controls)
                    {
                        if(control is MissionPackMissionPanel)
                        {
                            MissionPackMissionPanel MP = (MissionPackMissionPanel)control;
                            if(MP.panelID == missionPackage.activeMissionID)
                            {
                                MP.rewardAmountChanged(fieldDictionary["reward"],fieldDictionary["scienceReward"]);
                            }
                        }
                    }
                    break;

            }
        }
        public void changeActiveGoal(int newID)
        {
                activeGoalID = newID;
        }
        public void setGoalValue(string goalKey, string newValue, string goalType)
        {
            goalList[activeGoalID].editGoalField(goalType, goalKey, newValue);
        }
        public void setAllGoalValue(string goalKey, string newValue)
        {
                goalList[activeGoalID].editAllGoal(goalKey, newValue);
        }
        public string getGoalValue(string goalType, string goalKey)
        {
            return goalList[activeGoalID].getGoalField(goalType, goalKey);
        }
        public string getAllGoalValue(string goalKey)
        {
            return goalList[activeGoalID].getAllGoal(goalKey);
        }
        public void addGoal()
        {
            goalList.Add(new AllGoals());
        }
    }
}
  
