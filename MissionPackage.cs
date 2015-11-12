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
    public class MissionPackage
    {
        public mainForm MF;
        public List<Mission> missionList = new List<Mission>();
        public Dictionary<string, string> packageDictionary = new Dictionary<string, string>();
        public int activeMissionID = 0;

        public MissionPackage()
        {
            missionList.Add(new Mission("BlankMission", this));
            missionList[0].goalList.Add(new AllGoals());

            for (int i = 0; i < GV.missionPackageArray.GetLength(0); i++)
            {
                packageDictionary.Add(GV.missionPackageArray[i], GV.defaultValue);
            }


        }

        //Editing Mission Package Values
        public void addMission()
        {

            string missionName = GV.newMissionName;
            //Adds mission to missionPackage amd list
            Mission newMission = new Mission(missionName, this);
            missionList.Add(newMission);


            //Adds goal to mission on newMission, done here to ensure goalBox added
            int tempID = activeMissionID;
            activeMissionID = missionList.Count - 1;
            addGoal();

            activeMissionID = tempID;

            //Creates new goal in the mission

            //Adds a mission box and moves it into place
            //Comes before combobox to ensure color changes
            MissionPackagePanel panel = (MissionPackagePanel)MF.Controls["missionPackagePanel"];
            MissionPackMissionPanel subPanel = new MissionPackMissionPanel(missionName);
            panel.Controls.Add(subPanel);
            subPanel.Location = new Point(12, 89 + (missionList.Count - 1) * 95);  //if the y here is changed it must also my changed in missionPackagePanel
            subPanel.panelID = missionList.Count - 1;
            panel.editMissionboxOrder();        
            subPanel.parent = (MissionPackagePanel)subPanel.Parent;

            //Changes the selected mission in the combo box, which in turn changes active mission
            ComboBox CB = (ComboBox)MF.Controls[GV.missionInfoPanelName].Controls["missionNameComboBox"];            //Finds the missionNameComboBox
            CB.Items.Add(newMission.getMissionValue("name"));
            CB.SelectedIndex = CB.Items.Count - 1;

            /*empties all goals by checking / unchechecking submission box
            CheckBox tb = (CheckBox)parent.Controls["goalCreatorPanel"].Controls["subMissionCheckBox"];
            tb.Checked = true; tb.Checked = false;*/


            //Edits the list on the requires missions segment
            CheckedListBox CLB = (CheckedListBox)MF.Controls[GV.missionInfoPanelName].Controls["requiresMission"];
            CLB.Items.Add(newMission.getMissionValue("name"));




        }
        public void editMissionOrder(int firstID, int secondID)
        {


            //edits order on visible combobox
            ComboBox CB = (ComboBox)MF.Controls[GV.missionInfoPanelName].Controls["missionNameComboBox"];
            string Otemp = CB.Items[firstID].ToString();
            CB.Items[firstID] = CB.Items[secondID];
            CB.Items[secondID] = Otemp;

            //edits visible order on requiresMission Listbox
            CheckedListBox CLB = (CheckedListBox)MF.Controls[GV.missionInfoPanelName].Controls["requiresMission"];
            bool firstBool = false, secondBool = false;
            foreach (int i in CLB.CheckedIndices)
            {
                if (i == firstID) { firstBool = true; }
                else if (i == secondID) { secondBool = true; }
                CLB.SetItemChecked(i, false);
            }

            string Ptemp = CLB.Items[firstID].ToString();
            CLB.Items[firstID] = CLB.Items[secondID];
            CLB.Items[secondID] = Ptemp;

            //Resets checked items so data stored properly
            CLB.SetItemChecked(firstID, secondBool);
            CLB.SetItemChecked(secondID, firstBool);



            //edits order in the missionList
            Mission temp = missionList[firstID];
            missionList[firstID] = missionList[secondID];
            missionList[secondID] = temp;


        }
        public void changeActiveMission(int ID)
        {
            if (ID != activeMissionID || missionList.Count == 1)
            {
                //changes selected ComboBox item
                missionList[activeMissionID].activeGoalID = 0;
                activeMissionID = ID;
                ComboBox CB = (ComboBox)this.MF.Controls[GV.missionInfoPanelName].Controls["missionNameComboBox"];
                CB.SelectedIndex = activeMissionID;
                foreach (Control control in MF.Controls["missionPackagePanel"].Controls)
                {
                    if (control is MissionPackMissionPanel)
                    {
                        MissionPackMissionPanel panel = (MissionPackMissionPanel)control;
                        if (panel.panelID == activeMissionID) { panel.BackColor = GV.activeMissionColor; }
                        else { panel.BackColor = GV.inactiveMissionColor; }
                    }
                }

                clearAllFields();
                populateMission();
                populateGoalPanels();
                populateGoals();

                printValuesFinalPanel();
            }
        }

        public void editMissionPackage(string parameter, string newValue)
        {
            packageDictionary[parameter] = newValue;
            writeToTextFile();
        }


        //Editing Mission Values
        public void addGoal()
        {

            missionList[activeMissionID].addGoal();

            MissionInfoPanel MIP = (MissionInfoPanel)MF.Controls[GV.missionInfoPanelName];
            GoalPanel subPanel = new GoalPanel(MIP);
            MIP.Controls.Add(subPanel);
            subPanel.Location = new Point(12, MIP.panelStartingLocation.Y + (missionList[activeMissionID].goalList.Count - 1) * GV.goalBoxIncrement);  //if the y here is changed it must also my changed in missionPackagePanel
            subPanel.panelID = missionList[activeMissionID].goalList.Count - 1;
            changeActiveGoal(missionList[activeMissionID].activeGoalID);
        }
        public void removeGoal(int goalID) //needs to also remove from goalList
        {
            MissionInfoPanel MIP = (MissionInfoPanel)MF.Controls[GV.missionInfoPanelName];
            if (missionList[activeMissionID].goalList.Count == 1)  //if there is only one goal, the information is merely cleared
            {
                GoalDetailPanel GDP = (GoalDetailPanel)MF.Controls[GV.goalDetailPanelName];
                CheckBox tb = (CheckBox)MF.Controls[GV.goalCreatorPanelName].Controls["subMissionCheckBox"];
                tb.Checked = true; tb.Checked = false;
                foreach (Control control in GDP.Controls)
                {
                    if (control is CheckBox)
                    {
                        CheckBox CB = (CheckBox)control;
                        CB.Checked = false;
                    }
                    else if (control is TextBox)
                    {
                        TextBox TB = (TextBox)control;
                        if (TB.Name == "description") { TB.Text = ""; }
                    }
                }
            }
            else
            {
                for (int i = MIP.Controls.Count - 1; i >= 0; i--)
                {
                    GoalPanel panel = MIP.Controls[i] as GoalPanel;
                    if (panel != null)
                    {
                        if (panel.panelID == goalID)
                        {
                            MIP.Controls.Remove(panel);
                        }
                    }
                }
                for (int i = MIP.Controls.Count - 1; i >= 0; i--)
                {
                    GoalPanel panel = MIP.Controls[i] as GoalPanel;
                    if (panel != null)
                    {
                        if (panel.panelID > goalID) { panel.panelID--; }
                    }
                }
                missionList[activeMissionID].goalList.RemoveAt(goalID);
                if (goalID == missionList[activeMissionID].activeGoalID)
                {
                    if (goalID == 0) { changeActiveGoal(goalID); }
                    else if (missionList[activeMissionID].goalList.Count <= goalID)
                    {
                        changeActiveGoal(goalID - 1);
                    }
                    else
                    {
                        changeActiveGoal(goalID);
                    }
                }
            }
        }
        public void editMission(string missionParameter, string newValue)
        {
            switch (missionParameter)
            {
                case "name":
                    CheckedListBox CLB = (CheckedListBox)MF.Controls[GV.missionInfoPanelName].Controls["requiresMission"];
                    CLB.Items[activeMissionID] = newValue;
                    ComboBox CB = (ComboBox)MF.Controls[GV.missionInfoPanelName].Controls["missionNameComboBox"];
                    CB.Items[activeMissionID] = newValue;
                    break;
                case "reward":
                case "science":
                    MissionPackagePanel MIP = (MissionPackagePanel)MF.Controls[GV.missionPackagePanelName];
                    foreach (Control control in MIP.Controls)
                    {
                        if (control is MissionPackMissionPanel)
                        {
                            MissionPackMissionPanel MP = (MissionPackMissionPanel)control;
                            if (MP.panelID == activeMissionID)
                            {
                                MP.rewardAmountChanged(getMissionValue("reward"), getMissionValue("scienceReward"));
                            }
                        }
                    }
                    break;
            }

            missionList[activeMissionID].setMissionValue(missionParameter, newValue);
            if (missionParameter == "category")
            {
                int nullValue = GV.categoryArray.Length + 1; //just ensure this stays larger than the possible choices
                int i = 0;
                int firstIcon = nullValue;
                int secondIcon = nullValue;

                bool firstFound = false;
                bool secondFound = false;
                string tempValue = newValue.Replace(" ", "");
                string[] categoryList = tempValue.Split(',');
                for (int categoryCounter = 0; categoryCounter < categoryList.Length; categoryCounter++)
                {
                    foreach (string s in GV.categoryArray)
                    {
                        if (firstFound == false)
                        {
                            if (s == categoryList[categoryCounter])
                            {
                                if (GV.categoryIconList[i] != null)
                                {
                                    firstFound = true;
                                    firstIcon = i;
                                }
                                i = 0;
                                break;
                            }
                        }
                        else
                        {
                            if (secondFound == false)
                            {
                                if (s == categoryList[categoryCounter])
                                {
                                    if (GV.categoryIconList[i] != null)
                                    {
                                        secondFound = true;
                                        secondIcon = i;
                                    }
                                    i = 0;
                                    break;
                                }
                            }
                        }
                        i++;
                    }
                    if (secondFound == true) break;
                    i = 0;
                }
                editCategoryIcons(firstIcon, secondIcon);
            }
            printValuesFinalPanel();
        }

        public void editMissionGoal(string goalParameter, string newValue, string goalTypeName)
        {
            missionList[activeMissionID].setGoalValue(goalParameter, newValue, goalTypeName);
            printValuesFinalPanel();
            switch (goalParameter)
            {
            }
        }
        public void editMissionAllGoal(string goalParameter, string newValue)
        {
            missionList[activeMissionID].setAllGoalValue(goalParameter, newValue);
            switch (goalParameter)
            {
                case "reward":
                    MissionInfoPanel MIP = (MissionInfoPanel)MF.Controls[GV.missionInfoPanelName];
                    foreach (Control control in MIP.Controls)
                    {
                        if (control is GoalPanel)
                        {
                            GoalPanel GP = (GoalPanel)control;
                            if (GP.panelID == missionList[activeMissionID].activeGoalID)
                            {
                                GP.rewardAmountChanged(getAllGoalValue("reward"));
                            }
                        }
                    }
                    break;
                case "goalType":
                    MissionInfoPanel backPanel = (MissionInfoPanel)MF.Controls[GV.missionInfoPanelName];
                    foreach (Control control in backPanel.Controls)
                    {
                        if (control is GoalPanel)
                        {
                            GoalPanel GP = (GoalPanel)control;
                            if (GP.panelID == missionList[activeMissionID].activeGoalID)
                            {
                                GP.goalTypeChanged(getAllGoalValue("goalType"));
                            }
                        }
                    }
                    break;
            }
            printValuesFinalPanel();

        }



        public void changeActiveGoal(int newGoalID)
        {
            missionList[activeMissionID].changeActiveGoal(newGoalID);
            clearGoalFields();
            foreach (Control control in MF.Controls[GV.missionInfoPanelName].Controls)
            {
                 if (control is GoalPanel)
                 {
                       GoalPanel panel = (GoalPanel)control;
                       if (panel.panelID == newGoalID) { panel.BackColor = GV.activeGoalColor; }
                       else { panel.BackColor = GV.inactiveGoalColor; }
                       panel.BackgroundImage = global::MissionCreator.Properties.Resources.MissionControllerButton1;
                       panel.BackgroundImageLayout = ImageLayout.Stretch;
                 }
            }
            populateGoals();
        }
        public void editGoalOrder(int firstID, int secondID)
        {
            Mission M = missionList[activeMissionID];
            AllGoals temp = M.goalList[firstID];
            M.goalList[firstID] = M.goalList[secondID];
            M.goalList[secondID] = temp;
        }
        public void removeMission(int missionID)
        {
            changeActiveMission(0);
            MissionPackagePanel MPP = (MissionPackagePanel)MF.Controls[GV.missionPackagePanelName];
            if (missionList.Count == 1)  //if there is only one goal, the information is merely cleared
            {
                addMission();
                for (int i = MPP.Controls.Count - 1; i >= 0; i--)
                {
                    MissionPackMissionPanel panel = MPP.Controls[i] as MissionPackMissionPanel;
                    if (panel != null)
                    {
                        if (panel.panelID == 0)
                        {
                            MPP.Controls.Remove(panel);
                        }
                    }
                }
                for (int i = MPP.Controls.Count - 1; i >= 0; i--)
                {
                    MissionPackMissionPanel panel = MPP.Controls[i] as MissionPackMissionPanel;
                    if (panel != null)
                    {
                        if (panel.panelID == 1)
                        {
                            panel.panelID = 0;
                        }
                    }
                }
                ComboBox CB = (ComboBox)MF.Controls[GV.missionInfoPanelName].Controls["missionNameComboBox"];            //Finds the missionNameComboBox
                CB.Items.RemoveAt(0);
                CB.SelectedIndex = 0;

                missionList.RemoveAt(0);


                activeMissionID = 0;
                MPP.editMissionboxOrder();
            }
            else
            {
                for (int i = MPP.Controls.Count - 1; i >= 0; i--)
                {
                    MissionPackMissionPanel panel = MPP.Controls[i] as MissionPackMissionPanel;
                    if (panel != null)
                    {
                        if (panel.panelID == missionID)
                        {
                            MPP.Controls.Remove(panel);
                        }
                    }
                }
                for (int i = MPP.Controls.Count - 1; i >= 0; i--)
                {
                    MissionPackMissionPanel panel = MPP.Controls[i] as MissionPackMissionPanel;
                    if (panel != null)
                    {
                        if (panel.panelID > missionID) { panel.panelID--; }
                    }
                }
                if (missionID == activeMissionID)
                {
                    if (missionID == 0) { activeMissionID = 0; }
                    else if (missionList.Count-1 <= missionID)
                    {
                        activeMissionID = missionID - 1;
                    }
                    else
                    {
                        activeMissionID = missionID;
                    }
                    changeActiveMission(activeMissionID);
                }
                ComboBox CB = (ComboBox)MF.Controls[GV.missionInfoPanelName].Controls["missionNameComboBox"];            //Finds the missionNameComboBox
                CB.SelectedIndex = activeMissionID;
                CB.Items.RemoveAt(missionID);

                missionList.RemoveAt(missionID);
                MPP.editMissionboxOrder();

                /*
            else
                if (goalID == missionList[activeMissionID].activeGoalID)
                {
                    if (goalID == 0) { changeActiveGoal(goalID); }
                    else if (missionList[activeMissionID].goalList.Count <= goalID)
                    {
                        changeActiveGoal(goalID - 1);
                    }
                    else
                    {
                        changeActiveGoal(goalID);
                    }
                }
            }*/
                /*
                if (missionList.Count == 0)
                {
                    clearAllFields();
                    clearGoalFields();
                }
                else if (ID == 0)
                {

                    //changeActiveMission();
                }*/
            }
            changeActiveMission(0);


        }

        //return Mission Values
        public string getMissionValue(string key)
        {
            return missionList[activeMissionID].getMissionValue(key);
        }
        public string getAllGoalValue(string key)
        {
            return missionList[activeMissionID].getAllGoalValue(key);
        }
        public string getGoalValue(string goalType, string key)
        {
            return missionList[activeMissionID].getGoalValue(goalType, key);
        }

        //Other supplementals
        private void editCategoryIcons(int firstID, int secondID)
        {
            foreach (Control control in MF.Controls[GV.missionPackagePanelName].Controls)
            {
                MissionPackMissionPanel MP = control as MissionPackMissionPanel;
                if (MP != null && MP.panelID == activeMissionID)
                {
                    if (firstID > GV.categoryIconList.Count - 1)
                    {
                        MP.firstIcon.BackgroundImage = null;
                    }
                    else
                    {
                        MP.firstIcon.BackgroundImage = GV.categoryIconList[firstID];
                    }
                    if (secondID > GV.categoryIconList.Count - 1)
                    {
                        MP.secondIcon.BackgroundImage = null;
                    }
                    else
                    {
                        MP.secondIcon.BackgroundImage = GV.categoryIconList[secondID];
                    }

                }
            }
        }
        private void printValuesFinalPanel()
        {
            string tab = "  ";
            AllMissionInfoPanel panel = (AllMissionInfoPanel)MF.Controls[GV.allMissionInfoPanelName];
            Label label = (Label)panel.Controls["infoLabel"];

            string newValue = "";
            foreach (KeyValuePair<string, string> pair in missionList[activeMissionID].fieldDictionary)
            {
                if (pair.Value != GV.defaultValue)
                {
                    newValue += tab + pair.Key + " = " + pair.Value + "\n";
                }
            }
            foreach (AllGoals G in missionList[activeMissionID].goalList)
            {
                //Prints allGoal Values
                newValue += "-------------------------------------\n";
                newValue += tab + "AllGoalValues\n";
                foreach (KeyValuePair<string, string> pair in G.allGoalFieldDictionary)
                {
                    if (pair.Value != GV.defaultValue)
                    {
                        newValue += tab + pair.Key + " = " + pair.Value + "\n";
                    }
                }
                newValue += "\n============================\n";
                string[] goalTypes = G.allGoalFieldDictionary["goalType"].Split(',');
                if (goalTypes[0] != "")
                {
                    if (goalTypes[0] == "SubMission" || goalTypes[0] == "OrMission" || goalTypes[0] == "NorMission") //if this isn't a submission
                    {
                        newValue += tab + goalTypes[0] + "\n";
                        for (int i = 1; i < goalTypes.Length; i++)
                        {
                            newValue += goalTypes[i] + "\n";
                            if (goalTypes[i] == "partSubPanel")   //special case for parts sub panels
                            {
                            }
                            else if (goalTypes[i] == "resourceSubPanel")//special case for resource sub panels
                            {
                            }
                            else if (goalTypes[i] == "SubMission" || goalTypes[i] == "OrMission" || goalTypes[i] == "NorMission")
                            {
                                //This is because problems
                            }
                            else
                            {
                                foreach (KeyValuePair<string, string> pair in G.allFieldDictionary[goalTypes[i]])
                                {
                                    if (pair.Value != GV.defaultValue)
                                    {
                                        newValue += tab + tab + String.Format("{0,-17}", pair.Key) + " = " + pair.Value + "\n";
                                    }
                                }

                            }
                            newValue += "============================\n";
                        }
                    }
                    else //if this isn't a submission
                    {
                        if (goalTypes[0] == "partSubPanel")
                        {
                        }
                        else if (goalTypes[0] == "resourceSubPanel")
                        {
                        }
                        else
                        {
                            foreach (KeyValuePair<string, string> pair in G.allFieldDictionary[goalTypes[0]])
                            {
                                if (pair.Value != GV.defaultValue)
                                {
                                    newValue += tab + String.Format("{0,-22}", pair.Key) + " = " + pair.Value + "\n";
                                }
                            }
                        }
                    }
                }
            }
            label.Text = newValue;
            writeToTextFile();
        }




        //Changing Panel Stuff
        public void populateGoalPanels()
        {
            MissionInfoPanel MIP = (MissionInfoPanel)MF.Controls[GV.missionInfoPanelName];
            int currentGoalPanelY = MIP.panelStartingLocation.Y;
            int goalCount = 0;
            foreach (AllGoals goal in missionList[activeMissionID].goalList)
            {
                GoalPanel subPanel = new GoalPanel(MIP);
                MIP.Controls.Add(subPanel);
                subPanel.Location = new Point(12, currentGoalPanelY);  //if the y here is changed it must also my changed in missionPackagePanel
                subPanel.panelID = goalCount++;
                currentGoalPanelY += GV.goalBoxIncrement;
                subPanel.rewardAmountChanged(goal.getAllGoal("reward"));  //These two lines ensure labels remain proper
                subPanel.goalTypeChanged(goal.getAllGoal("goalType"));    //These two lines ensure labels remain proper
            }
            changeActiveGoal(0);
        }
        public void populateMission()
        {
            MissionInfoPanel MIP = (MissionInfoPanel)MF.Controls[GV.missionInfoPanelName];
            foreach (Control control in MIP.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox CB = (CheckBox)control;
                    for (int i = 0; i < GV.missionFieldArray.GetLength(1); i++)
                    {
                        if (CB.Name == GV.missionFieldArray[0, i] + "CheckBox")
                        {
                            string valueType = GV.missionFieldArray[1, i];
                            string valueField = GV.missionFieldArray[0, i];
                            string currentValue = missionList[activeMissionID].getMissionValue(valueField);
                            if (currentValue != GV.defaultValue)
                            {
                                updateValue(MIP, valueType, valueField, currentValue, i);
                                continue;
                            }
                        }
                    }
                }
                if (control is TextBox)
                {
                    TextBox TB = (TextBox)control;
                    if (TB.Name == "description")
                    {
                        if (getMissionValue("description") == GV.defaultValue) { TB.Text = ""; }
                        else                                                   { TB.Text = getMissionValue("description"); }
                    }
                }
            }
        }
        public void populateGoals()
        {
            //Gets the current list of information
            if (Application.OpenForms.Count != 0)
            {
                mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];
                GoalCreatorPanel GCP = (GoalCreatorPanel)MF.Controls[GV.goalCreatorPanelName];
                GoalDetailPanel GDP = (GoalDetailPanel)MF.Controls[GV.goalDetailPanelName];
                AllGoals currentGoals = missionList[activeMissionID].goalList[missionList[activeMissionID].activeGoalID];

                for(int i = 0; i < GV.allGoalsFieldArray.GetLength(1); i++)
                {
                    string goalKey = GV.allGoalsFieldArray[0,i];
                    if (currentGoals.allGoalFieldDictionary[goalKey] != GV.defaultValue)
                    {
                        updateValue(GDP, GV.allGoalsFieldArray[1, i], goalKey, currentGoals.allGoalFieldDictionary[goalKey], i);
                    }
                }

                string[] goalType = getAllGoalValue("goalType").Split(',');

                if (goalType[0] == "SubMission" || goalType[0] == "OrMission" || goalType[0] == "NorMission")
                { //If it is a special Mission, checks all appropriate goalBoxes
                    foreach (Control control in GCP.Controls)   //FIRST CHECKS SPECIAL GOAL
                    {
                        if (control is CheckBox)
                        {
                            CheckBox CB = (CheckBox)control;
                            if (CB.Name == goalType[0]+ "CheckBox")
                            {
                                CB.Checked = true;
                                break;
                            }
                        }

                    }
                    for (int i = 1; i < goalType.Length; i++)  //Then checks goalTypes
                    {
                        foreach (Control control in GCP.Controls)
                        {
                            if (control is CheckBox)
                            {
                                CheckBox CB = (CheckBox)control;
                                if (CB.Name == goalType[i] + "GoalCheckBox")
                                {
                                    CB.Checked = true;
                                    for (int panelCounter = 0; panelCounter < GV.goalTypeList[0].GetLength(1); panelCounter++) //for each desired panel
                                    {
                                        if (goalType[i] == GV.goalTypeList[0][0, panelCounter])
                                        {
                                            Panel panel = (Panel)GDP.Controls[GV.goalTypeList[0][0, panelCounter]];
                                            int ID = panelCounter+1;
                                            for (int valueCounter = 0; valueCounter < GV.goalTypeList[ID].GetLength(1); valueCounter++)
                                            {
                                                string valueType = GV.goalTypeList[ID][1, valueCounter];
                                                string valueField = GV.goalTypeList[ID][0, valueCounter];
                                                string currentValue = currentGoals.allFieldDictionary[goalType[i]][GV.goalTypeList[ID][0, valueCounter]]; //gets current goalValue
                                                if(currentValue != GV.defaultValue || valueType == "partSubPanel" || valueType == "resourceSubPanel")
                                                {
                                                    updateValue(panel, valueType, valueField, currentValue, valueCounter);
                                                }
                                            }
                                        }
                                    }
                                    continue;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < goalType.Length; i++)
                    {
                    }
                }
                else //If it is not a special mission, only checks radioButtons
                {
                    foreach (Control control in GCP.Controls)
                    {
                        if (control is RadioButton)
                        {
                            RadioButton RB = (RadioButton)control;
                            if (RB.Name == goalType[0] + "GoalRadioButton")
                            {
                                RB.Checked = true;
                                for (int panelCounter = 0; panelCounter < GV.goalTypeList[0].GetLength(1); panelCounter++) //for each desired panel
                                    {
                                        if (goalType[0] == GV.goalTypeList[0][0, panelCounter])
                                        {
                                            Panel panel = (Panel)GDP.Controls[GV.goalTypeList[0][0, panelCounter]];
                                            int ID = panelCounter + 1;
                                            for (int valueCounter = 0; valueCounter < GV.goalTypeList[ID].GetLength(1); valueCounter++)
                                            {
                                                string valueType = GV.goalTypeList[ID][1,valueCounter];
                                                string valueField = GV.goalTypeList[ID][0,valueCounter];
                                                string currentValue = currentGoals.allFieldDictionary[goalType[0]][GV.goalTypeList[ID][0, valueCounter]]; //gets current goalValue
                                                if(currentValue != GV.defaultValue || valueType == "partSubPanel" || valueType == "resourceSubPanel")
                                                {
                                                    updateValue(panel, valueType, valueField, currentValue, valueCounter);
                                                }
                                            }
                                        }
                                    }
                                break;
                            }
                        }

                    }
                }
            }
        }



        public void updateValue(Panel sender, string valueType, string valueField, string value, int valueID)
        {
            Mission M = missionList[activeMissionID];


            switch (valueType)
            {
                case "N/A": 
                case "string":
                case "double":   //double, int and string are treated the same
                case "int":
                    foreach (Control control in sender.Controls)
                    {
                        if (control is CheckBox)
                        {
                            CheckBox CB = (CheckBox)control;
                            if (CB.Name == valueField + "CheckBox")
                            {
                                CB.Checked = true;
                                continue;
                            }
                        }
                        if (control is TextBox)
                        {
                            TextBox TB = (TextBox)control;
                            if (TB.Name == valueField)
                            {
                                TB.Text = value;
                                continue;
                            }
                        }
                    }
                    break;
                case "selection":
                    foreach (Control control in sender.Controls)
                    {
                        if (control is CheckBox)
                        {
                            CheckBox CB = (CheckBox)control;
                            if (CB.Name == valueField + "CheckBox")
                            {
                                CB.Checked = true;
                                continue;
                            }
                        }
                        if (control is ComboBox)
                        {
                            ComboBox CB = (ComboBox)control;
                            if (CB.Name == valueField)
                            {
                                CB.SelectedItem = value;
                                break;
                            }
                        }
                    }
                    break;
                case "TIME()":
                    string tempTIMElist = value;
                    //gets rid of letters
                    tempTIMElist = tempTIMElist.Replace("y", "").Replace("d","").Replace("h","").Replace("m","").Replace("s","");
                    tempTIMElist = tempTIMElist.Replace("(","").Replace(")","").Replace("TIME","");

                    string[] TIMElist = tempTIMElist.Split(' ');
                    
                    foreach (Control control in sender.Controls)
                    {
                        if (control is CheckBox)
                        {
                            CheckBox CB = (CheckBox)control;
                            if (CB.Name == valueField + "TIMECheckBox")
                            {
                                CB.Checked = true;
                                continue;
                            }
                        }
                        if (control is TextBox)
                        {
                            TextBox TB = (TextBox)control;
                            if (TB.Name == valueField + "TIMEYears") { TB.Text = TIMElist[0]; }
                            if (TB.Name == valueField + "TIMEDays") { TB.Text = TIMElist[1]; }
                            if (TB.Name == valueField + "TIMEHours") { TB.Text = TIMElist[2]; }
                            if (TB.Name == valueField + "TIMEMinutes") { TB.Text = TIMElist[3]; }
                            if (TB.Name == valueField + "TIMESeconds") { TB.Text = TIMElist[4]; }
                        }
                    }
                    break;
                case "checkedListbox":
                    string[] CLBlist = value.Replace(", ", ",").Split(',');
                    
                    foreach (Control control in sender.Controls)
                    {
                        if (control is CheckBox)
                        {
                            CheckBox CB = (CheckBox)control;
                            if (CB.Name == valueField + "CheckBox")
                            {
                                CB.Checked = true;
                                continue;
                            }
                        }
                        if (control is CheckedListBox)
                        {
                            CheckedListBox CLB = (CheckedListBox)control;
                            if (CLB.Name == valueField)
                            {
                                for (int i = 0; i < CLB.Items.Count; i++)
                                {
                                    for (int j = 0; j < CLBlist.Length; j++)
                                    {
                                        if (CLB.Items[i].ToString() == CLBlist[j])
                                        {
                                            CLB.SetItemChecked(i, true);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "partSubPanel":
                    CheckBox partCB = new CheckBox();
                    TextBox partTB = new TextBox();

                    string tempPartName;
                    string[] nameList = M.getGoalValue("Part", "partName").Split(',');
                    string[] minList = M.getGoalValue("Part", "partCount").Split(',');
                    string[] maxList = M.getGoalValue("Part", "maxPartCount").Split(',');

                    int partNumberCount = 0;
                    int panelCount = nameList.Length;
                    if (valueID == 0) { valueID = maxList.Length; }
                    foreach (Control control in sender.Controls)
                    {
                        if (control.Name == "numberPartTypesTextBox")
                        {
                            control.Text = valueID.ToString();
                            break;
                        }
                    }
                    foreach (Control control in sender.Controls)
                    {
                        if (control is partsSubPanel && partNumberCount < panelCount)
                        {
                            partsSubPanel PSP = control as partsSubPanel;
                            if (nameList[partNumberCount] != GV.defaultValue) { PSP.Controls[GV.partNameTextBoxName].Text = nameList[partNumberCount]; }

                            if (minList[partNumberCount] != GV.defaultValue)
                            {
                                tempPartName = GV.minPartRequiredName + "CheckBox";
                                partCB = (CheckBox)PSP.Controls[tempPartName];
                                partCB.Checked = true;
                                tempPartName = GV.minPartRequiredName + "TextBox";
                                partTB = (TextBox)PSP.Controls[tempPartName];
                                partTB.Text = minList[partNumberCount];
                            }
                            if (maxList[partNumberCount] != GV.defaultValue)
                            {
                                tempPartName = GV.maxPartRequiredName + "CheckBox";
                                partCB = (CheckBox)PSP.Controls[tempPartName];
                                partCB.Checked = true;
                                tempPartName = GV.maxPartRequiredName + "TextBox";
                                partTB = (TextBox)PSP.Controls[tempPartName];
                                partTB.Text = maxList[partNumberCount];
                            }
                            partNumberCount++;

                        }
           
                    }
                    break;
                case "resourceSubPanel":
                    CheckBox resourceCB = new CheckBox();
                    TextBox resourceTB = new TextBox();

                    string tempResourceName;
                    string[] resourceNameList = M.getGoalValue("Resource", "name").Split(',');
                    string[] minResourceList = M.getGoalValue("Resource", "minAmount").Split(',');
                    string[] maxResourceList = M.getGoalValue("Resource", "maxAmount").Split(',');

                    int resourceNumberCount = 0;
                    int resourcePanelCount = resourceNameList.Length;
                    if (valueID == 0) { valueID = maxResourceList.Length; } //only populates as many as it needs to
                    foreach (Control control in sender.Controls)
                    {
                        if (control.Name == "numberResourceTypesTextBox")
                        {
                            control.Text = valueID.ToString();
                            break;
                        }
                    }
                    foreach (Control control in sender.Controls)
                    {
                        if (control is resourceSubPanel && resourceNumberCount < resourcePanelCount)
                        {
                            resourceSubPanel PSP = control as resourceSubPanel;
                            if (resourceNameList[resourceNumberCount] != GV.defaultValue) { PSP.Controls[GV.resourceNameTextBoxName].Text = resourceNameList[resourceNumberCount]; }

                            if (minResourceList[resourceNumberCount] != GV.defaultValue)
                            {
                                tempResourceName = GV.minResourceRequiredName + "CheckBox";
                                resourceCB = (CheckBox)PSP.Controls[tempResourceName];
                                resourceCB.Checked = true;
                                tempResourceName = GV.minResourceRequiredName + "TextBox";
                                resourceTB = (TextBox)PSP.Controls[tempResourceName];
                                resourceTB.Text = minResourceList[resourceNumberCount];
                            }
                            if (maxResourceList[resourceNumberCount] != GV.defaultValue)
                            {
                                tempResourceName = GV.maxResourceRequiredName + "CheckBox";
                                resourceCB = (CheckBox)PSP.Controls[tempResourceName];
                                resourceCB.Checked = true;
                                tempResourceName = GV.maxResourceRequiredName + "TextBox";
                                resourceTB = (TextBox)PSP.Controls[tempResourceName];
                                resourceTB.Text = maxResourceList[resourceNumberCount];
                            }
                            resourceNumberCount++;

                        }
           
                    }
                    break;
                case "bool":
                    foreach (Control control in sender.Controls)
                    {
                        if (control is CheckBox)
                        {
                            CheckBox CB = (CheckBox)control;
                            if (CB.Name == valueField + "CheckBox")
                            {
                                CB.Checked = true;
                                break;
                            }
                        }
                    }
                    break;
                case "description":
                    foreach (Control control in sender.Controls)
                    {
                        if (control is TextBox)
                        {
                            TextBox TB = (TextBox)control;
                            if (TB.Name == valueField)
                            {
                                TB.Text = value;
                                break;
                            }
                        }
                    }
                    break;
            }
        }




        public void clearGoalFields()
        {
            string rewardValue = missionList[activeMissionID].goalList[0].getAllGoal("reward");      //This grabs it itself because needs to grab first one
            string goalTypeValue = missionList[activeMissionID].goalList[0].getAllGoal("goalType");

            int tempActiveID = activeMissionID;
            missionList.Add(new Mission("empty", this));
            activeMissionID = missionList.Count - 1;
            missionList[activeMissionID].addGoal();

            GoalDetailPanel GDP = (GoalDetailPanel)MF.Controls[GV.goalDetailPanelName];
            foreach (Control control in GDP.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox CB = (CheckBox)control;
                    CB.Checked = false;
                }
                if (control is TextBox)
                {
                    if (control is TextBox)
                    {
                        TextBox TB = (TextBox)control;
                        if (TB.Name == "description")
                        {
                            TB.Text = "";
                        }
                    }
                }
            }


            CheckBox tb = (CheckBox)MF.Controls[GV.goalCreatorPanelName].Controls["SubMissionCheckBox"];
            tb.Checked = true; tb.Checked = false;

            missionList.RemoveAt(missionList.Count - 1);
            activeMissionID = tempActiveID;

            int tempGoalID = missionList[activeMissionID].activeGoalID;
            missionList[activeMissionID].activeGoalID = 0;
            editMissionAllGoal("reward", rewardValue);         //these two lines ensure goalType stays the same
            editMissionAllGoal("goalType", goalTypeValue);     //these two lines ensure goalType stays the same
            missionList[activeMissionID].activeGoalID = tempGoalID;
        }
        public void clearGoalPanels()
        {
            MissionInfoPanel MIP = (MissionInfoPanel)MF.Controls[GV.missionInfoPanelName];
            for (int i = MIP.Controls.Count - 1; i >= 0; i--)
            {
                GoalPanel panel = MIP.Controls[i] as GoalPanel;
                if (panel != null)
                {
                    MIP.Controls.Remove(panel);
                    panel.Dispose();
    
                }
            }
        }
        public void clearAllFields()
        {

            //creates an empty mission and changes active ID so all changing data is made to the empty mission
            int tempActiveID = activeMissionID;
            missionList.Add(new Mission("empty", this));
            activeMissionID = missionList.Count - 1;
            missionList[activeMissionID].addGoal();

            MissionInfoPanel MIP = (MissionInfoPanel)MF.Controls[GV.missionInfoPanelName];
            GoalDetailPanel GDP = (GoalDetailPanel)MF.Controls[GV.goalDetailPanelName];
            foreach (Control control in MIP.Controls)
            {
                CheckBox CB = control as CheckBox;
                if (CB != null) { CB.Checked = false; }
            }

            foreach (Control control in GDP.Controls)
            {
                CheckBox CB = control as CheckBox;
                if (CB != null) { CB.Checked = false; }
            }
            /*
            //clears orbitgoal data
            CheckBox tb = (CheckBox)MF.Controls[GV.goalCreatorPanelName].Controls["subMissionCheckBox"];
            tb.Checked = true; tb.Checked = false;*/

            missionList.RemoveAt(missionList.Count - 1);
            activeMissionID = tempActiveID;

            clearGoalPanels();



        }
        /*public void editMissionBoxOrder()
        {
            int missionBoxIncrement = 95;
            int firstPanelY = 89;
            MissionPackagePanel missionPackPanel = (MissionPackagePanel)MF.Controls["missionPackagePanel"];
            List<int> heightList = new List<int>();
            List<MissionPackMissionPanel> panelList = new List<MissionPackMissionPanel>();
            foreach (Control panel in MF.Controls["missionPackagePanel"].Controls)  //collects list of all missionOverviewPanels
            {
                if (panel is MissionPackMissionPanel)
                {
                    panelList.Add((MissionPackMissionPanel)panel);
                    heightList.Add(panel.Location.Y);
                }
            }
            //Gets the panels in order according to their ID numbers
            //THIS IS NOT ORDERING THEM BY HEIGHT
            for (int j = 0; j < panelList.Count - 1; j++)
            {
                for (int i = 0; i < panelList.Count - 1; i++)
                {
                    if (panelList[i].panelID > panelList[i + 1].panelID)
                    {
                        //gets panels in correct order
                        MissionPackMissionPanel temp = (MissionPackMissionPanel)panelList[i];
                        panelList[i] = panelList[i + 1];
                        panelList[i + 1] = temp;

                        //gets heightlist in correct order
                        int stemp = heightList[i];
                        heightList[i] = heightList[i + 1];
                        heightList[i + 1] = stemp;
                    }
                }
            }

            //Places the panels in the desired order

            if (panelList.Count == 1) { panelList[0].Location = new Point(12, firstPanelY); }
            for (int j = 0; j < panelList.Count; j++)
            {
                for (int i = 0; i < panelList.Count - 1; i++)
                {
                    if (heightList[i] >= heightList[i + 1])
                    {
                        MF.missionPackage.editMissionOrder(i + 1, i);
                        int temp = heightList[i];
                        heightList[i] = heightList[i + 1];
                        heightList[i + 1] = temp;
                        //changes order on GUI


                        int tID = panelList[i].panelID;
                        panelList[i].panelID = panelList[i + 1].panelID;
                        panelList[i + 1].panelID = tID;

                        MissionPackMissionPanel tPanel = panelList[i];
                        panelList[i] = panelList[i + 1];
                        panelList[i + 1] = tPanel;
                    }

                }
            }

            //moves panel to top scroll position to allow proper panel placement
            Point autoScrollPosition = missionPackPanel.AutoScrollPosition;
            int verticlePosition = missionPackPanel.VerticalScroll.Value;
            missionPackPanel.VerticalScroll.Value = 0;
            missionPackPanel.AutoScrollPosition = new Point(0, 0);

            foreach (Control panel in MF.Controls["missionPackagePanel"].Controls)  //Collects list of all panels
            {
                if (panel is MissionPackMissionPanel)
                {
                    MissionPackMissionPanel tPanel = (MissionPackMissionPanel)panel;
                    tPanel.Location = new Point(12, firstPanelY + missionBoxIncrement * tPanel.panelID);
                    Console.WriteLine(tPanel.panelID + tPanel.Location.ToString());
                }
            }


            Console.WriteLine("---------------------------------");

            //Returns panel to desired position
            if (verticlePosition > missionPackPanel.VerticalScroll.Maximum)
            {
                missionPackPanel.VerticalScroll.Value = missionPackPanel.VerticalScroll.Maximum;
                missionPackPanel.AutoScrollPosition = new Point(0, missionPackPanel.VerticalScroll.Value * -1);
                missionPackPanel.PerformLayout();
            }
            else
            {
                missionPackPanel.AutoScrollPosition = autoScrollPosition;
                missionPackPanel.VerticalScroll.Value = verticlePosition;
                missionPackPanel.PerformLayout();
            }

        }
         * */
        //DEBUGGING
        public void printMissionValues()
        {
            if (GV.debug == true)
            {
                Console.Clear();
                Console.WriteLine("Mission Package \n");
                Console.WriteLine("activeID = " + activeMissionID.ToString());
                for (int i = 0; i < GV.missionPackageArray.GetLength(0); i++)
                {
                    Console.WriteLine(String.Format("{0,-22}", GV.missionPackageArray[i]) + " = " + packageDictionary[GV.missionPackageArray[i]]);
                }
                Console.WriteLine("===============================\n");
                int length = GV.missionFieldArray.GetLength(1);
                foreach (Mission M in missionList)
                {
                    Console.WriteLine("Active Goal ID =          " + M.activeGoalID.ToString());
                    for (int i = 0; i < length; i++)
                    {
                        Console.WriteLine(String.Format("{0,-22}", GV.missionFieldArray[0, i]) + " = " + M.getMissionValue(GV.missionFieldArray[0, i]));
                    }
                    Console.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
                    Console.WriteLine("Begin Goal List \n");
                    foreach (AllGoals G in M.goalList)
                    {
                        //Prints allGoal Values
                        Console.WriteLine("All Goals \n");
                        foreach (KeyValuePair<string, string> pair in G.allGoalFieldDictionary)
                        {

                            Console.WriteLine(String.Format("{0,-22}", pair.Key) + " = " + pair.Value);
                        }
                        Console.WriteLine('\n');
                        //Prints Values to
                        Console.WriteLine('\n');
                        string[] goalTypes = G.allGoalFieldDictionary["goalType"].Split(',');
                        for (int i = 0; i < goalTypes.Length; i++)
                        {
                            if (goalTypes[i] == "SubMission" || goalTypes[i] == "OrMission" || goalTypes[i] == "NorMission")
                            {
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("goal Type = " + goalTypes[i] + '\n');
                                foreach (KeyValuePair<string, string> pair in G.allFieldDictionary[goalTypes[i]])
                                {
                                    Console.WriteLine(String.Format("{0,-22}", pair.Key) + " = " + pair.Value);
                                }
                                Console.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
                            }
                        }
                    }
                    Console.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
                    Console.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
                }
            };

   
        }

        public void writeToTextFile()
        {
            string tab = "     ";

            string newValue = "";
            int missionCount = 1;

            newValue += "MissionPackage\n{\n";
            foreach(KeyValuePair<string, string> pair in packageDictionary)
            {
                if (pair.Value == GV.defaultValue)
                {
                    if (pair.Key == "ownOrder")
                    {
                        newValue += tab + pair.Key + " = " + "false";
                    }
                    else
                    {
                        newValue += tab + pair.Key + " = " + "";
                    }
                }
                else //if the value isn't defaultValue
                {
                    newValue += tab + pair.Key + " = " + pair.Value;
                }
                newValue += '\n';
            }
            newValue += "\n\n\n";
            foreach (Mission M in missionList)  //BEGIN EACH MISSION PRINT
            {
                newValue += tab + "Mission\n" + tab + "{\n";
                foreach (KeyValuePair<string, string> pair in M.fieldDictionary)   //Prints all MissionFields
                {
                    if (pair.Value != GV.defaultValue)
                    {
                        newValue += tab + tab + pair.Key + " = " + pair.Value + "\n";
                    }
                }
                newValue += tab + tab + "packageOrder = " + missionCount.ToString() + "\n";
                newValue += "\n\n";
                foreach (AllGoals G in M.goalList)
                {
                    string[] goalType = G.allGoalFieldDictionary["goalType"].Split(',');

                    //Capitalizes all neccessary


                    //------------------------BEGIN SUBMISSIONGOAL SECTION---------------------------.

                    if (goalType[0] == "SubMission" || goalType[0] == "OrMission" || goalType[0] == "NorMission")
                    {
                        newValue += tab + tab + goalType[0] + "Goal" + "\n" + tab + tab + "{\n";


                        //SUBMISSION ALLGOALVALUES
                        foreach (KeyValuePair<string, string> pair in G.allGoalFieldDictionary)
                        {
                            if (pair.Value != GV.defaultValue && pair.Key != "goalType")
                            {
                                newValue += tab + tab + tab + pair.Key + " = " + pair.Value + "\n";
                            }
                        }

                        //Prints each goal within the submission
                        for (int i = 1; i < goalType.Length; i++)
                        {
                            if (goalType[i] == "Part")
                            {
                                string[] partList = G.allFieldDictionary["Part"]["partName"].Split(',');
                                string[] minPartCountList = G.allFieldDictionary["Part"]["partCount"].Split(',');
                                string[] maxPartCountList = G.allFieldDictionary["Part"]["maxPartCount"].Split(',');

                                int partCount = partList.Length;
                                if (partCount > maxPartCountList.Length) { partCount = maxPartCountList.Length; }


                                for (int j = 0; j < partCount; j++)
                                {
                                    newValue += tab + tab + tab + "PartGoal\n" + tab + tab + tab + "{\n";
                                    newValue += tab + tab + tab + tab + "partName" + " = " + partList[j] + "\n";
                                    if (minPartCountList[j] != GV.defaultValue)
                                    {
                                        newValue += tab + tab + tab + tab + "partCount" + " = " + minPartCountList[j] + "\n";
                                    }
                                    if (maxPartCountList[j] != GV.defaultValue)
                                    {
                                        newValue += tab + tab + tab + tab + "maxPartCount" + " = " + maxPartCountList[j] + "\n";
                                    }
                                    newValue += tab + tab + tab + "}\n"; //closes each PartGoal
                                }
                            }
                            else if (goalType[i] == "Resource")
                            {
                                string[] resourceList = G.allFieldDictionary["Resource"]["name"].Split(',');
                                string[] minResourceCountList = G.allFieldDictionary["Resource"]["minAmount"].Split(',');
                                string[] maxResourceCountList = G.allFieldDictionary["Resource"]["maxAmount"].Split(',');

                                int resourceCount = resourceList.Length;
                                if (resourceCount > maxResourceCountList.Length) { resourceCount = maxResourceCountList.Length; }

                                for (int j = 0; j < resourceCount; j++)
                                {
                                    newValue += tab + tab + tab + "ResourceGoal\n" + tab + tab + tab + "{\n";
                                    newValue += tab + tab + tab + tab + "name" + " = " + resourceList[j] + "\n";
                                    if (minResourceCountList[j] != GV.defaultValue)
                                    {
                                        newValue += tab + tab + tab + tab + "minAmount" + " = " + minResourceCountList[j] + "\n";
                                    }
                                    if (maxResourceCountList[j] != GV.defaultValue)
                                    {
                                        newValue += tab + tab + tab + tab + "maxAmount" + " = " + maxResourceCountList[j] + "\n";
                                    }
                                    newValue += tab + tab + tab + "}\n"; //closes each ResourceGoal
                                }
                            }
                            else if (goalType[0] == "SubMission" || goalType[0] == "OrMission" || goalType[0] == "NorMission")
                            {
                                //This is to catch some issues
                            }
                            else
                            {
                                newValue += tab + tab + tab + goalType[i] + "Goal\n" + tab + tab + tab + "{\n";

                                //EachGoal Value within the SubGoal
                                foreach (KeyValuePair<string, string> pair in G.allFieldDictionary[goalType[i]])
                                {
                                    if (pair.Value != GV.defaultValue)
                                    {
                                        newValue += tab + tab + tab + tab + pair.Key + " = " + pair.Value + "\n";
                                    }
                                }
                                newValue += tab + tab + tab + "}\n";
                            }
                            //Closes each subgoal


                        }//CLOSES EACH SUBMISSION GOAL
                    }
                    //-------------------END SUBMISSION GOAL SECTION-----------------------




                    //------------------BEGIN SINGLE GOAL SECTION-------------------------
                    else
                    {
                        //Prints Goal {
                        if (goalType[0] == "Part")
                        {
                            string[] partList = G.allFieldDictionary["Part"]["partName"].Split(',');
                            string[] minPartCountList = G.allFieldDictionary["Part"]["partCount"].Split(',');
                            string[] maxPartCountList = G.allFieldDictionary["Part"]["maxPartCount"].Split(',');
                            int partCount = partList.Length;
                            if(partCount > maxPartCountList.Length) { partCount = maxPartCountList.Length;}   //Only populates as many values as it can
                            
                            for (int j = 0; j < partCount; j++)
                            {
                                newValue += tab + tab + "PartGoal\n" + tab + tab + "{\n";
                                newValue += tab + tab + tab + "partName" + " = " + partList[j] + "\n";
                                if (minPartCountList[j] != GV.defaultValue)
                                {
                                    newValue += tab + tab + tab + "partCount" + " = " + minPartCountList[j] + "\n";
                                }
                                if (maxPartCountList[j] != GV.defaultValue)
                                {
                                    newValue += tab + tab + tab + "maxPartCount" + " = " + maxPartCountList[j] + "\n";
                                }
                                newValue += tab + tab + "}\n"; //closes each PartGoal
                            }
                        }
                        else if (goalType[0] == "Resource")
                        {
                            string[] resourceList = G.allFieldDictionary["Resource"]["name"].Split(',');
                            string[] minResourceCountList = G.allFieldDictionary["Resource"]["minAmount"].Split(',');
                            string[] maxResourceCountList = G.allFieldDictionary["Resource"]["maxAmount"].Split(',');
                            int resourceCount = resourceList.Length;
                            if (resourceCount > maxResourceCountList.Length) { resourceCount = maxResourceCountList.Length; }

                            for (int j = 0; j < resourceCount; j++)
                            {
                                newValue += tab + tab + "ResourceGoal\n" + tab + tab + "{\n";
                                newValue += tab + tab + tab + "name" + " = " + resourceList[j] + "\n";
                                if (minResourceCountList[j] != GV.defaultValue)
                                {
                                    newValue += tab + tab + tab + "minAmount" + " = " + minResourceCountList[j] + "\n";
                                }
                                if (maxResourceCountList[j] != GV.defaultValue)
                                {
                                    newValue += tab + tab + tab + "maxAmount" + " = " + maxResourceCountList[j] + "\n";
                                }
                                newValue += tab + tab + "}\n"; 
                                //closes each ResourceGoal
                            }
                        }
                        else if(goalType[0] != "")
                        {

                            newValue += tab + tab + goalType[0] + "Goal" + "\n" + tab + tab + "{\n";
                            foreach (KeyValuePair<string, string> pair in G.allFieldDictionary[goalType[0]])
                            {
                                if (pair.Value != GV.defaultValue)
                                {
                                    newValue += tab + tab + tab + pair.Key + " = " + pair.Value + "\n";
                                }
                            }
                        }

                    } //ENDS SOLO GOAL PRINTING




                    newValue += tab + tab + "}\n"; //ENDS EACH GOAL PRINTING
                }
                newValue += tab + "}\n"; //CLOSES EACH MISSION PRINT
                missionCount++;
            } //ends each missionPrint
            newValue += '}';
            //Closes MissionPackage

            string tempFileLocation = MF.textFilePath + MF.temporaryFileName;
            System.IO.StreamWriter tempFile = new System.IO.StreamWriter(@tempFileLocation);



            string[] segmentedNewValue = newValue.Split('\n');
            using (tempFile)
            {
                for (int i = 0; i < segmentedNewValue.Length; i++)
                {
                    tempFile.WriteLine(segmentedNewValue[i]);
                }
            }

        }
    }
}
