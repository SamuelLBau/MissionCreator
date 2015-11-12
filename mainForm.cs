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
/* Here is where I will keep various TO-DO lists, important notes for updating as well as notes about how to adapt the program for future version of KSP
 * 
 * All information on the formatting, field information and other important stuff can be found at
 *      https://github.com/malkuth1974/KSPMissionController/blob/master/missions.md
 *      
 * -----------------------------------HOW TO EDIT THE PROGRAM FOR NEW CAPACITIES-------------------------
 * 
 *                              Adding parameter to an existing goal type:
 *                      Go to GlobalVariables in mainForm.cs, look for the appropriate fieldArray, further information in that section
 *                      resource and parts do not fall under this category
 * 
 *                                       Adding a new goal type:
 *                      add it to the goalArray list in GlobalVariables
 *                      make a panel for it
 *                      make a fieldArray for it
 *                      under mission.cs add it to the addGoal switch case
 *                      
 * 
 *      
 * ------------------------------------------OTHER NOTES---------------------------------------
 *      The parts panel and the resource panel are currently programmed differently than the others, all of their fields are coded into the class, instead
 * of arrays as are most of the rest. I would like to change this, but will focus on getting everything else functional first.
 *      Because they are programmed differently, parts and resource cannot accept additional fields simply (using simple dictionary)
 *      
 *      There is a class GlobalVariables where I will hold important things that might be used by multiple classes, 
 * could be used to replace searching for the mainform methods etc.
 *      I will be using dictionaries to dynamically create variables for use in the classes. 
 *      To call the values, use dictName.TryGetValue("variableName",out variableValue)
 *      
 *      
 * --------------------------------------------Currently Working on------------------------------------------------------------
 *            
 *                  Saving base information to mission
 *                  Saving goal information to mission
 *                  Changing all cases of Mission this.Parent.Parent to Application.OpenForms[0]
 *          
 *      
 * 
 * 
 * 
 *      ------------------------------------Primary TO-DO LIST----------------------------------------------------------
 *      
 *   -
 *   -   Need to add The full detailed description of the current active mission in the adjacent panel
 *    -  Need to create a method that converts all of the input data into a mission class, thus saving it. (input types eventhandler)
 *    -  Need to then convert the mission object information to a text save file
 *      
 *    -  requiresMissions needs access to missionList
 * 
 * --------------------------------------Secondary TO-DO LIST------------------------------------------------------
 *   -  Look into using dictionaries to link checkBoxes and Textboxes to save to searching for them every time one is clicked
 *   -  Find a way to make panel creation dynamic
 *   -  Replace DrawcheckedListBox to let it handle different arrays, also clean up InputTypes
 *   -  Add tooltip to each type to make understanding each goal easier
 *      
 * */
namespace MissionCreator
{
    public partial class mainForm : Form
    {
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();


        public string temporaryFileName = "tempPackage.txt";
        public string textFilePath;  
        public MissionPackage missionPackage = new MissionPackage();
        public mainForm()
        {
            InitializeComponent();
            housekeeping();
        }

        //---------------------------------------BEGIN HOUSEKEEPING----------------------------------------
        public void housekeeping()
        {
            if (GV.debug == false)
            {
                this.Controls["savePackPanel"].Controls["testButton"].Hide();
            }
            if (GV.debug == true)
            {
                AllocConsole();
            }

            //this segment gets the location of where the text file will be saved
            textFilePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string temp = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            textFilePath = textFilePath.Remove(textFilePath.Length - temp.Length - 4);






            missionPackage.MF = this;

            //-------------------------------------Begin top panel creation----------------------------
            GoalCreatorPanel goalCreatorPanel = new GoalCreatorPanel();
            goalCreatorPanel.Name = GV.goalCreatorPanelName;
            this.Controls.Add(goalCreatorPanel);

            GoalDetailPanel goalDetailPanel = new GoalDetailPanel();
            goalDetailPanel.Name = GV.goalDetailPanelName;
            this.Controls.Add(goalDetailPanel);

            MissionInfoPanel missionInfoPanel = new MissionInfoPanel();
            missionInfoPanel.Name = GV.missionInfoPanelName;
            this.Controls.Add(missionInfoPanel);

            MissionPackagePanel missionPackagePanel = new MissionPackagePanel();
            missionPackagePanel.Name = GV.missionPackagePanelName;
            this.Controls.Add(missionPackagePanel);

            AllMissionInfoPanel allMissionInfoPanel = new AllMissionInfoPanel();
            allMissionInfoPanel.Name = GV.allMissionInfoPanelName;
            this.Controls.Add(allMissionInfoPanel);
            //-------------------------------------End top panel creation----------------------------

            //-------------------------------------Begin bottom panel creation----------------------------
            int numberGoalTypes = GV.goalTypeList[0].GetLength(1);      //returns number of types of goals
            for (int i = 0; i < numberGoalTypes; i++)
            {

                Panel panel = new Panel();
                panel.Name = GV.goalTypeList[0][0, i];
                createPanel(panel, GV.goalTypeList[i + 1]);
                panel.Location = new Point(5, GV.detailPanelInitialY);
                this.Controls[GV.goalDetailPanelName].Controls.Add(panel);
                panel.Hide();
                panel.BackgroundImage = GV.goaLPanelImage;
                panel.BackgroundImageLayout = ImageLayout.Stretch;
                //panel.BackColor = GV.goalPanelColor;
                panel.BackColor = Color.Transparent;
                panel.SizeChanged += new EventHandler(panel_SizeChanged);
                //panel.VisibleChanged +=new EventHandler(panel_VisibleChanged);

                Label titleLabel = new Label();
                titleLabel.Text = GV.goalTypeList[0][0, i] + " Parameters";
                titleLabel.AutoSize = true;
                titleLabel.BackColor = Color.Transparent;
                titleLabel.Font = GV.titleFont;
                int titleLocationX = (GV.goalPanelHeight - TextRenderer.MeasureText(titleLabel.Text, titleLabel.Font).Width) / 2;
                titleLabel.Location = new Point(titleLocationX, 6);
                panel.Controls.Add(titleLabel);
            }

            //string[] t = GV.goalTypeList[0];


            //This code automatically turns the orbit radioButton on
            RadioButton rb = (RadioButton)goalCreatorPanel.Controls["orbitGoalRadioButton"];
            rb.Checked = true;
            rb.ForeColor = Color.Lime;

            for (int i = missionPackage.missionList.Count - 1; i >= 0; i--)
            {
                missionPackage.missionList.RemoveAt(i);
            }
            missionPackage.addMission();






            //This segment does final preparations for stuff
            exceptionObjectHandling();



            missionPackage.changeActiveMission(0);
            missionPackage.clearAllFields();
            missionPackage.populateGoalPanels();
        }

        //----------------------------END HOUSEKEEPING---
        //----------BEGIN PANEL CREATION----------------------------



        //-----------Begin Panel Handling---------------

        private void panel_SizeChanged(object sender, EventArgs e)
        {
            movePanels((Panel)this.Controls[GV.goalDetailPanelName]);
        }
        private void createPanel(Panel panel, string[,] fieldArray)
        {
            panel.BackColor = Color.DarkRed;
            panel.Width = 311;
            panel.AutoSize = true;
            panel.MinimumSize = new Size(311, 0);
            panel.MaximumSize = new Size(311, 5000);
            panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            panel.VisibleChanged += new EventHandler(Panel_VisibleChanged);

            Point checkBoxCurrentLocation = new Point(4, 30);
            Point textBoxCurrentLocation = new Point(191, 27);
            Size textBoxSize = new Size(74, 20);
            int checkBoxIncrement = 27;
            int textBoxIncrement = checkBoxIncrement;

            int length = fieldArray.GetLength(1);
            if (length == 0)
            {
                Label noParameterLabel = new Label();
                noParameterLabel.Text = "No Additional Parameters";
                noParameterLabel.Font = GV.generalFont;
                noParameterLabel.Location = textBoxCurrentLocation;
                noParameterLabel.AutoSize = true;
                panel.Controls.Add(noParameterLabel);

                int titleLocationX = (GV.goalPanelHeight - TextRenderer.MeasureText(noParameterLabel.Text, noParameterLabel.Font).Width) / 2;
                noParameterLabel.Location = new Point(titleLocationX, 27);

            }
            else
            {
                for (int i = 0; i < fieldArray.GetLength(1); i++)    //creates each of the buttons for the control
                {
                    InputTypes newGroup = new InputTypes(panel, fieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement);
                }
            }
        }
        public void changeGoalPanels(object sender)
        {
            string panelName = "";
            bool isChecked = false;
            RadioButton rb = sender as RadioButton;
            CheckBox cb = sender as CheckBox;
            Panel pn = sender as Panel;

            int currentY = GV.detailPanelInitialY;


            if (rb != null) { panelName = rb.Name.Replace("GoalRadioButton", ""); isChecked = rb.Checked; }
            else if (cb != null) { panelName = cb.Name.Replace("GoalCheckBox", ""); isChecked = cb.Checked; }             //gets the name of the panel used to input
            else if (pn != null && pn.Visible) { panelName = pn.Name; isChecked = true; }                                      //If the panel is visible and not null changes stuff, .Visible negates it if panel is invisible
            if (panelName != "")
            {
                int initialX = 5;

                Panel detailPanel = (Panel)this.Controls["goalDetailPanel"];
                if (detailPanel != null)
                {
                    Panel panel = (Panel)detailPanel.Controls[panelName];
                    if (panel != null)
                    {
                        if (isChecked == true)                                    //If the checkitem is checked, it turns the panel visible and moves it into place
                        {
                            panel.Show();
                        }
                        else if (isChecked == false)
                        {
                            if (panel.Visible)                                  //checks each control on the panel, if the control is a checkbox, it is unchecked
                            {
                                panel.Hide();
                                panel.Location = new Point(initialX, currentY);
                                if (panel.Name == "Part")
                                {
                                    panel.Controls[GV.numberPartTypesTBName].Text = "0";
                                }
                                else if (panel.Name == "Resource")
                                {
                                    panel.Controls[GV.numberResourceTypesTBName].Text = "0";
                                }
                                else
                                {
                                    for (int uncheckCheckBoxesCounter = 0; uncheckCheckBoxesCounter < panel.Controls.Count; uncheckCheckBoxesCounter++)
                                    {
                                        Control currentControl = panel.Controls[uncheckCheckBoxesCounter];
                                        CheckBox CB = currentControl as CheckBox;
                                        if (CB != null)
                                        {
                                            CB.Checked = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                movePanels(detailPanel);                       //Actually moves the panels into place
            }
        }
        private void movePanels(Panel detailPanel)
        {
            int panelSeparation = 5;    //If you change these, you must also change them in the changePanels() above
            int initialX = 5;
            int currentY = GV.detailPanelInitialY;                // detailPanelInitialY is a class constant

            int verticlePosition = detailPanel.VerticalScroll.Value;
            Point autoScrollPosition = detailPanel.AutoScrollPosition;
            detailPanel.VerticalScroll.Value = 0;
            detailPanel.AutoScrollPosition = new Point(0, 0);
            for (int findSubPanelCounter = detailPanel.Controls.Count - 1; findSubPanelCounter > 0; findSubPanelCounter--)
            {
                Panel panel;
                Control detailControl2 = detailPanel.Controls[findSubPanelCounter];
                panel = detailControl2 as Panel;
                if (panel != null)
                {
                    if (panel.Visible)
                    {

                        panel.Location = new Point(initialX, currentY);
                        currentY += panel.Height + panelSeparation;
                    }
                }
            }
            if (verticlePosition > detailPanel.VerticalScroll.Maximum)                               //Handles the autoscrolling to put stuff back in position
            {
                detailPanel.VerticalScroll.Value = detailPanel.VerticalScroll.Maximum;
                detailPanel.AutoScrollPosition = new Point(0, detailPanel.VerticalScroll.Value * -1);
                detailPanel.PerformLayout();
            }
            else
            {
                detailPanel.AutoScrollPosition = autoScrollPosition;
                detailPanel.VerticalScroll.Value = verticlePosition;
                detailPanel.PerformLayout();
            }
        }



        //-------Other Vital Methods---------
        private void exceptionObjectHandling()    //Exceptions to the usual Control Rules
        {
            //code that adds bodyList to Landing, Launch, Orbit boxes
            GoalDetailPanel GDP = (GoalDetailPanel)this.Controls[GV.goalDetailPanelName];
            Panel editPanel;

            foreach (Control control in GDP.Controls)
            {
                editPanel = control as Panel;
                if (editPanel != null)
                {
                    if (editPanel.Name == "Orbit")
                    {
                        foreach (Control subControl in editPanel.Controls)
                        {
                            ComboBox CB = subControl as ComboBox;
                            if (CB != null && CB.Name == "body")
                            {
                                CB.Items.AddRange(GV.bodyList);
                                CB.SelectedIndex = 0;
                            }
                            else if (CB != null && CB.Name == "launchZone")
                            {
                                CB.Items.AddRange(GV.launchZoneList);
                                CB.SelectedIndex = 0;
                            }
                        }
                    }
                    else if (editPanel.Name == "Landing")
                    {
                        foreach (Control subControl in editPanel.Controls)
                        {
                            ComboBox CB = subControl as ComboBox;
                            if (CB != null && CB.Name == "body")
                            {
                                CB.Items.AddRange(GV.bodyList);
                                CB.SelectedIndex = 0;
                            }
                            else if (CB != null && CB.Name == "launchZone")
                            {
                                CB.Items.AddRange(GV.launchZoneList);
                                CB.SelectedIndex = 0;
                            }
                        }
                    }
                    else if(editPanel.Name == "Launch")
                    {
                        foreach (Control subControl in editPanel.Controls)
                        {
                            ComboBox CB = subControl as ComboBox;
                            if (CB != null && CB.Name == "body")
                            {
                                CB.Items.AddRange(GV.bodyList);
                                CB.SelectedIndex = 0;
                            }
                            else if (CB != null && CB.Name == "launchZone")
                            {
                                CB.Items.AddRange(GV.launchZoneList);
                                CB.SelectedIndex = 0;
                            }
                        }
                    }
                    else if(editPanel.Name == "Crash")
                    {
                        foreach (Control subControl in editPanel.Controls)
                        {
                            ComboBox CB = subControl as ComboBox;
                            if (CB != null && CB.Name == "body")
                            {
                                CB.Items.AddRange(GV.bodyList);
                                CB.SelectedIndex = 0;
                            }
                            else if (CB != null && CB.Name == "launchZone")
                            {
                                CB.Items.AddRange(GV.launchZoneList);
                                CB.SelectedIndex = 0;
                            }
                        }
                    }
                    else if (editPanel.Name == "Part")
                    {
                        //editPanel.Controls[GV.partNameTextBoxName].Text = "0";
                    }
                }
            }

            //Adds category list to mission information
            MissionInfoPanel MIF = (MissionInfoPanel)this.Controls[GV.missionInfoPanelName];
            CheckedListBox CLB = (CheckedListBox)MIF.Controls["category"];
            CLB.Items.AddRange(GV.categoryArray);
            CLB.SelectedIndex = 0;



            //Code that adds part and resource lists

        }
        public void Panel_VisibleChanged(object sender, EventArgs e)
        {
            string goalType = "";
            foreach (Control control in this.Controls["goalCreatorPanel"].Controls)
            {
                CheckBox CB = control as CheckBox;
                if (CB != null)
                {
                    string name = CB.Name.Replace("CheckBox", "");
                    if (name == "SubMission" || name == "OrMission" || name == "NorMission")
                    {
                        if (CB.Checked == true) { goalType += name + ','; }
                    }
                }
            }
            foreach (Control control in this.Controls["goalDetailPanel"].Controls)
            {
                Panel panel = control as Panel;
                if (panel != null)
                {
                    if (panel.Visible == true)
                    {
                        goalType += panel.Name + ',';
                    }
                }
            }
            if (goalType.Length > 0) { goalType = goalType.Remove(goalType.Length - 1); }
                missionPackage.editMissionAllGoal("goalType", goalType);
        }

        //mainForm.cs[Design] mandated eventhandlers (should ultimatly be sparse)

        private void mainForm_FormClosed(object sender, EventArgs e)
        {
            string file = textFilePath + temporaryFileName;
            if(File.Exists(@file))
            {
                File.Delete(@file);
            }
        }
        private void newMissionButton_Click(object sender, EventArgs e)
        {
        }
        private void saveMissionPackButton_Click(object sender, EventArgs e)
        {
            string textFileName;
            if(missionPackage.packageDictionary["name"] == GV.defaultValue)
            {
                textFileName = "UnnamedMissionPackage.MPKG";
            }
            else
            {
                textFileName = missionPackage.packageDictionary["name"].Replace(" ", "_") + ".MPKG";
            }

            string textFileLocation = textFilePath + textFileName;
            string tempTextFileLocation = textFilePath + temporaryFileName;

            if (File.Exists(@tempTextFileLocation))
            {
                if (File.Exists(@textFileLocation))
                {
                    File.Delete(@textFileLocation);
                }

                File.Copy(tempTextFileLocation, textFileLocation);
            }

        }
        private void mainForm_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //This is for debugging
            if (GV.debug == true)
            {
                Button B = (Button)sender;
                mainForm MF = (mainForm)B.Parent.Parent;
                MissionInfoPanel MIP = (MissionInfoPanel)MF.Controls[GV.missionInfoPanelName];

                MF.missionPackage.printMissionValues();
                foreach(Control control in MIP.Controls)
                {
                    if(control is GoalPanel)
                    {
                        Console.WriteLine(control.ToString());
                    }
                }

            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void missionPackNameBox_TextChanged(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];
            MF.missionPackage.editMissionPackage("name", TB.Text);
        }

        private void creditsLabel_Click(object sender, EventArgs e)
        {
            CreditsForm CF = new CreditsForm();
            CF.ShowDialog();
        }
    }
}
