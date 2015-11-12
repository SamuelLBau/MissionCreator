using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;

namespace MissionCreator
{
    class GoalCreatorPanel : Panel
    {

        public GoalCreatorPanel()
        {
            Point checkBoxStartingLocation = new Point(6, 31 + 17 * 4); //17 * #special types of missions
            int checkBoxIncrement = 20;

            this.Size = new Size(204, 605);
            this.Location = new Point(4, 50);
            this.AutoSize = true;
            this.BackColor = Color.Transparent;
            this.BackgroundImage = global::MissionCreator.Properties.Resources.PanelBack;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            Point checkBoxCurrentLocation = checkBoxStartingLocation;

            Label titleLabel = new Label();
            titleLabel.Text = "Goal Creator";
            titleLabel.AutoSize = true;
            titleLabel.Name = "goalCreatorPanelTitleLabel";
            titleLabel.Font = GV.titleFont;
            titleLabel.Location = new Point(5, 5);
            this.Controls.Add(titleLabel);

            Panel addGoalPanel = new Panel();
            addGoalPanel.BackColor = Color.Transparent;
            addGoalPanel.BackgroundImage = GV.backPanelImage;
            addGoalPanel.BackgroundImageLayout = ImageLayout.Stretch;
            addGoalPanel.Location = new Point(125, 5);
            addGoalPanel.Size = new Size(70, 20);
            addGoalPanel.Click += new EventHandler(newGoalButton_Click);
            this.Controls.Add(addGoalPanel);

                Label addGoalLabel = new Label();
                addGoalLabel.Location = new Point(3,3);
                addGoalLabel.Font = GV.generalFont;
                addGoalLabel.Text = "Add Goal";
                addGoalLabel.Name = "newGoalButtonLabel";
                addGoalLabel.Click += new EventHandler(newGoalButton_Click);
                addGoalPanel.Controls.Add(addGoalLabel);







            CheckBox subMissioncb = new CheckBox();
            subMissioncb.Text = "SubMission";
            subMissioncb.AutoSize = true;
            subMissioncb.ForeColor = GV.inactiveButtonColor;
            subMissioncb.Name = "SubMissionCheckBox";
            subMissioncb.Location = new Point(6, 31);
            subMissioncb.Font = GV.generalFont;
            subMissioncb.CheckedChanged += new EventHandler(subMissioncb_CheckedChanged);
            this.Controls.Add(subMissioncb);

            CheckBox orMissioncb = new CheckBox();
            orMissioncb.Text = "OrMission";
            orMissioncb.ForeColor = GV.inactiveButtonColor;
            orMissioncb.AutoSize = true;
            orMissioncb.Name = "OrMissionCheckBox";
            orMissioncb.Location = new Point(6, 48);
            orMissioncb.Font = GV.generalFont;
            orMissioncb.CheckedChanged += new EventHandler(subMissioncb_CheckedChanged);
            this.Controls.Add(orMissioncb);

            CheckBox norMissioncb = new CheckBox();
            norMissioncb.Text = "NorMission";
            norMissioncb.AutoSize = true;
            norMissioncb.ForeColor = GV.inactiveButtonColor;
            norMissioncb.Name = "NorMissionCheckBox";
            norMissioncb.Location = new Point(6, 65);
            norMissioncb.Font = GV.generalFont;
            norMissioncb.CheckedChanged += new EventHandler(subMissioncb_CheckedChanged);
            this.Controls.Add(norMissioncb);


            for (int i = 0; i < GV.goalTypeList[0].GetLength(1); i++)
            {
                createNewCheckRadioButton(GV.goalTypeList[0][0,i], ref checkBoxCurrentLocation, checkBoxIncrement);
            }



        }
        private void newGoalButton_Click(object sender, EventArgs e)
        {
            mainForm MF = (mainForm)Application.OpenForms[0];
            MF.missionPackage.addGoal();
        }
        private void subMissioncb_CheckedChanged(object sender, EventArgs e)
        {
            if (Application.OpenForms.Count != 0) //this ensures no error if called before mainform initialized
            {
                CheckBox CB = (CheckBox)sender;
                mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];
                MF.Panel_VisibleChanged(sender, e);
                if (CB.Checked == true)
                {
                    string goalType = CB.Name.Replace("CheckBox", "");
                    MF.missionPackage.editMissionAllGoal("goalType", goalType);
                }
                else
                {
                    MF.missionPackage.editMissionAllGoal("goalType", "Orbit");
                }


                changeBoxColor(sender);
                changeAllControls(sender);
            }

        }
        private void button_CheckedChanged(object sender, EventArgs e)
        {
            changeBoxColor(sender);
            mainForm parentForm = this.Parent as mainForm;
            parentForm.changeGoalPanels(sender);

        }
        private void changeBoxColor(object sender)
        {
            //Used to change the label color of radio / checkboxes. 
            //Accepts the checkbox/radioButton and changes its own labelcolor.
            CheckBox box = sender as CheckBox;
            RadioButton button = sender as RadioButton;
            if (box != null)
            {
                if (box.Checked == true) { box.ForeColor = GV.activeButtonColor; }
                else { box.ForeColor = GV.inactiveButtonColor; }
            }
            else if (button != null)
            {
                if (button.Checked == true) { button.ForeColor = GV.activeButtonColor; }
                else { button.ForeColor = GV.inactiveButtonColor; }
            }
        }
        private void changeAllControls(object sender)
        {
            CheckBox specialMissionBox = sender as CheckBox;
            bool isChecked = specialMissionBox.Checked;

            //pulls up every control on goalCreator
            for (int i = this.Controls.Count - 1; i > 0; i--)
            {
                Control control = this.Controls[i];
                RadioButton rb = control as RadioButton;
                CheckBox cb = control as CheckBox;
                if (isChecked)
                {
                    if (rb != null)
                    {
                        rb.Checked = false;
                        rb.Hide();
                    }
                    else if (cb != null)
                    {
                        cb.Show();
                        if (cb != specialMissionBox)
                        {
                            cb.Checked = false;
                        }
                    }
                }
                else
                {
                    if (rb != null)
                    {
                        rb.Show();
                        if (rb.Name.Contains("Orbit")) { rb.Checked = true; }
                        //Sets the orbit radioButton to checked

                    }
                    else if (cb != null)
                    {
                        cb.Checked = false;
                        cb.Hide();
                        if (cb.Name.Contains("Mission")) { cb.Show(); }   //Keeps the submissionBox Visible
                    }
                }
            }
            specialMissionBox.Checked = isChecked;
        }
        private void createNewCheckRadioButton(string name, ref Point checkBoxCurrentLocation, int checkBoxIncrement)
        {
            RadioButton rb = new RadioButton();
            rb.Name = name + "GoalRadioButton";
            rb.Height = 16;
            rb.Text = capitalizeString(name);
            rb.ForeColor = GV.inactiveButtonColor;
            rb.Font = GV.generalFont;
            rb.Location = checkBoxCurrentLocation;
            rb.CheckedChanged += new EventHandler(button_CheckedChanged);
            rb.AutoSize = true;
            this.Controls.Add(rb);

            CheckBox cb = new CheckBox();
            cb.Name = name + "GoalCheckBox";
            cb.Text = capitalizeString(name);
            cb.ForeColor = GV.inactiveButtonColor;
            cb.Font = GV.generalFont;
            cb.Location = new Point(checkBoxCurrentLocation.X, checkBoxCurrentLocation.Y + 1); ;
            cb.Height = 16;
            cb.CheckedChanged += new EventHandler(button_CheckedChanged);
            this.Controls.Add(cb);
            cb.AutoSize = true;
            cb.Hide();

            checkBoxCurrentLocation = new Point(checkBoxCurrentLocation.X, checkBoxCurrentLocation.Y + checkBoxIncrement);

        }
        private string capitalizeString(string name)
        {
            string firstLetter = name.Substring(0, 1);
            firstLetter.ToUpper();
            name = name.Substring(1);
            firstLetter = firstLetter.ToUpper();
            name = firstLetter + name;
            return name;
        }
    }
    class GoalDetailPanel : Panel
    {
        public GoalDetailPanel()
        {
            this.BackgroundImage = GV.backPanelImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BackColor = Color.Transparent;
            this.AutoScroll = true;
            this.Size = new Size(340, 605);
            this.Location = new Point(210, 50);


            Point checkBoxStartingLocation = new Point(5, 53);
            Point textBoxStartingLocation = new Point(240, 50);
            Size textBoxSize = new Size(35, 20);
            int checkBoxIncrement = 27;


            Point checkBoxCurrentLocation = checkBoxStartingLocation;
            Point textBoxCurrentLocation = textBoxStartingLocation;
            int textBoxIncrement = checkBoxIncrement;


            Label goalDescriptionLabel = new Label();
            goalDescriptionLabel.Name = "goalDescriptionLabel";
            goalDescriptionLabel.Text = "Goal description (optional)";
            goalDescriptionLabel.Font = GV.generalFont;
            goalDescriptionLabel.AutoSize = true;
            goalDescriptionLabel.Location = new Point(138, 34);
            this.Controls.Add(goalDescriptionLabel);


            Label goalDetailPanelTitleLabel = new Label();
            goalDetailPanelTitleLabel.Name = "goalDetailPanelTitleLabel";
            goalDetailPanelTitleLabel.Text = "Goal parameter editor";
            goalDetailPanelTitleLabel.Font = GV.titleFont;
            goalDetailPanelTitleLabel.AutoSize = true;
            goalDetailPanelTitleLabel.Location = new Point(70, 6);
            this.Controls.Add(goalDetailPanelTitleLabel);

            TextBox goalDescriptionTextBox = new TextBox();
            goalDescriptionTextBox.Name = "description";
            goalDescriptionTextBox.Size = new Size(127, 20);
            goalDescriptionTextBox.Location = new Point(5, 29);
            goalDescriptionTextBox.Multiline = true;
            goalDescriptionTextBox.WordWrap = true;
            goalDescriptionTextBox.MouseEnter += new EventHandler(goalDescriptionBox_MouseEnter);
            goalDescriptionTextBox.MouseLeave += new EventHandler(goalDescriptionBox_MouseLeave);
            goalDescriptionTextBox.TextChanged += new EventHandler(goalDescriptionTextBox_TextChanged);
            goalDescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Controls.Add(goalDescriptionTextBox);


            int length = GV.allGoalsFieldArray.GetLength(1);

            for (int i = 0; i < length; i++)    //creates each of the buttons for the control
            {
                InputTypes newGroup = new InputTypes(this, GV.allGoalsFieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement);
            }
            goalDescriptionTextBox.BringToFront();   //gets the description TextBox to overlay all other items
        }
        private void addGoalButton_Clicked(object sender, EventArgs e)
        {
        }
        private void goalDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            mainForm MF = (mainForm)Application.OpenForms[0];
            MF.missionPackage.editMissionAllGoal("description", TB.Text);

        }
        private void goalDescriptionBox_MouseEnter(object sender, EventArgs e)
        {
            TextBox goalDescriptionTextBox = sender as TextBox;
            goalDescriptionTextBox.Width = 280;
            goalDescriptionTextBox.Height = 250;
        }
        private void goalDescriptionBox_MouseLeave(object sender, EventArgs e)
        {
            TextBox goalDescriptionTextBox = sender as TextBox;
            goalDescriptionTextBox.Width = 127;
            goalDescriptionTextBox.Height = 20;
        }

    }
    class MissionInfoPanel : Panel
    {
        public Point panelStartingLocation;
        public MissionInfoPanel()
        {
            Point checkBoxStartingLocation = new Point(3, 50);
            Point textBoxStartingLocation = new Point(checkBoxStartingLocation.X + 200, checkBoxStartingLocation.Y -3);
            Size textBoxSize = new Size(30, 20);
            int checkBoxIncrement = 23;

            Point checkBoxCurrentLocation = checkBoxStartingLocation;
            Point textBoxCurrentLocation = textBoxStartingLocation;


            this.Location = new Point(553, 6);
            this.Size = new Size(310, 652);                                                 //To change this, also change Maximum size property
            this.MaximumSize = new Size(310, 652);
            this.BackgroundImage = GV.backPanelImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BackColor = Color.Transparent;
            this.AutoScroll = true;


            Label missionNameLabel = new Label();
            missionNameLabel.Name = "missionNameLabel";
            missionNameLabel.Text = "Mission name";
            missionNameLabel.AutoSize = true;
            missionNameLabel.Font = GV.generalFont;
            missionNameLabel.Location = new Point(13, 7);
            this.Controls.Add(missionNameLabel);

            ComboBox missionNameComboBox = new ComboBox();
            missionNameComboBox.Name = "missionNameComboBox";
            missionNameComboBox.Size = new Size(150, 21);
            missionNameComboBox.Location = new Point(113, 3);
            missionNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            missionNameComboBox.SelectedIndexChanged += new EventHandler(missionNameCB_SelectionChanged);
            this.Controls.Add(missionNameComboBox);

            Label missionDescriptionLabel = new Label();
            missionDescriptionLabel.Name = "missionDescriptionLabel";
            missionDescriptionLabel.Text = "Mission description";
            missionDescriptionLabel.AutoSize = true;
            missionDescriptionLabel.Font = GV.generalFont;
            missionDescriptionLabel.Location = new Point(160, 30);  //5,30
            this.Controls.Add(missionDescriptionLabel);

            TextBox missionDescriptionTextBox = new TextBox();
            missionDescriptionTextBox.Name = "description";
            missionDescriptionTextBox.Location = new Point(5, 27);    //140,20
            missionDescriptionTextBox.Size = new Size(145, 20);
            missionDescriptionTextBox.Multiline = true;
            missionDescriptionTextBox.WordWrap = true;
            missionDescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            missionDescriptionTextBox.MouseHover += new EventHandler(missionDescriptionTextBox_MouseHover);
            missionDescriptionTextBox.MouseLeave += new EventHandler(missionDescriptionTextBox_MouseLeave);
            missionDescriptionTextBox.TextChanged += new EventHandler(missionDescriptionTextBox_TextChanged);
            this.Controls.Add(missionDescriptionTextBox);
            missionDescriptionTextBox.BringToFront();



            int length = GV.missionFieldArray.GetLength(1);


            for (int i = 0; i < length; i++)    //creates each of the buttons for the control
            {
                InputTypes newGroup = new InputTypes(this, GV.missionFieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement);
            }
            panelStartingLocation.X = checkBoxCurrentLocation.X +10;
            panelStartingLocation.Y = checkBoxCurrentLocation.Y;

        }



        private void missionNameCB_SelectionChanged(object sender, EventArgs e)
        {
            ComboBox CB = (ComboBox)sender;
            mainForm MF = (mainForm)this.Parent;
            MF.missionPackage.changeActiveMission(CB.SelectedIndex);
        }
        private void missionDescriptionTextBox_MouseHover(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb != null)
            {
                tb.Size = new Size(284, 225);
            }
        }
        private void missionDescriptionTextBox_MouseLeave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb != null)
            {
                tb.Size = new Size(145, 20);
            }

        }
        private void missionDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];
            MF.missionPackage.editMission("description", TB.Text);
        }
        public void editGoalPanelOrder()
        {
            mainForm MF = (mainForm)this.Parent;
            List<int> heightList = new List<int>();
            List<GoalPanel> panelList = new List<GoalPanel>();

            foreach (Control control in this.Controls)
            {
                GoalPanel P = control as GoalPanel;
                if (P != null)
                {
                    panelList.Add(P);
                    heightList.Add(P.Location.Y);
                }
            }
            //DOES NOT ORDER BY HEIGHT
            for (int j = 0; j < panelList.Count; j++) //This section puts goals in the correct order
            {
                for (int i = 0; i < panelList.Count - 1; i++)
                {
                    if (panelList[i].panelID > panelList[i + 1].panelID)
                    {
                        GoalPanel ptemp = panelList[i];
                        panelList[i] = panelList[i + 1];
                        panelList[i + 1] = ptemp;

                        //gets heightlist in correct order
                        int htemp = heightList[i];
                        heightList[i] = heightList[i + 1];
                        heightList[i + 1] = htemp;
                    }
                }
            }

            //--------------EDIT FROM HERE BELOW FOR GOAL VALUES
            if (panelList.Count == 1)
            {
                panelList[0].Location = panelStartingLocation;
                MF.missionPackage.editGoalOrder(0, 0);
            }
            for (int j = 0; j < panelList.Count; j++)
            {
                for (int i = 0; i < panelList.Count - 1; i++)
                {
                    if (heightList[i] >= heightList[i + 1])
                    {
                        MF.missionPackage.editGoalOrder(i + 1, i);
                        int temp = heightList[i];
                        heightList[i] = heightList[i + 1];
                        heightList[i + 1] = temp;
                        //changes order on GUI


                        int tID = panelList[i].panelID;
                        panelList[i].panelID = panelList[i + 1].panelID;
                        panelList[i + 1].panelID = tID;

                        GoalPanel tPanel = panelList[i];
                        panelList[i] = panelList[i + 1];
                        panelList[i + 1] = tPanel;
                    }

                }
            }

            //moves panel to top scroll position to allow proper panel placement
            Point autoScrollPosition = this.AutoScrollPosition;
            int verticlePosition = this.VerticalScroll.Value;
            this.VerticalScroll.Value = 0;
            this.AutoScrollPosition = new Point(0, 0);

            int newID = 0;
            foreach(GoalPanel tPanel in panelList)
            {
                tPanel.panelID = newID;
                tPanel.Location = new Point(12, panelStartingLocation.Y + GV.goalBoxIncrement * newID);
                newID++;
            }

            Console.WriteLine("---------------------------------");

            //Returns panel to desired position
            if (verticlePosition > this.VerticalScroll.Maximum)
            {
                this.VerticalScroll.Value = this.VerticalScroll.Maximum;
                this.AutoScrollPosition = new Point(0, this.VerticalScroll.Value * -1);
                this.PerformLayout();
            }
            else
            {
                this.AutoScrollPosition = autoScrollPosition;
                this.VerticalScroll.Value = verticlePosition;
                this.PerformLayout();
            }
        }
    }
    class MissionPackagePanel : Panel
    {
        TextBox descriptionTB;
        int missionPanelStartingX = 5;
        public MissionPackagePanel()
        {
            this.Location = new Point(865, 6);
            this.Size = new Size(295, 652);                                                 //To change this, also change Maximum size property
            this.MaximumSize = new Size(295, 652);
            this.BackgroundImage = GV.backPanelImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BackColor = Color.Transparent;
            this.AutoScroll = true;
            this.AllowDrop = false;

            Label packageLabel = new Label();
            packageLabel.Location = new Point(this.Width / 2, 6);

            
            Label packageNameLabel = new Label();
            packageNameLabel.Name = "packageNameLabel";
            packageNameLabel.Text = "Package Title";
            packageNameLabel.AutoSize = true;
            packageNameLabel.Font = GV.generalFont;
            packageNameLabel.Location = new Point(170, 8);
            this.Controls.Add(packageNameLabel);

            TextBox packageNameTextBox = new TextBox();
            packageNameTextBox.Name = "packageNameTextBox";
            packageNameTextBox.Text = "";
            packageNameTextBox.Size = new Size(150, 20);
            packageNameTextBox.Location = new Point(13, 5);
            packageNameTextBox.TextChanged +=new EventHandler(packageNameTextBox_TextChanged);
            this.Controls.Add(packageNameTextBox);

            Label descriptionLabel = new Label();
            descriptionLabel.Name = "descriptionNameLabel";
            descriptionLabel.Text = "Pack description";
            descriptionLabel.AutoSize = true;
            descriptionLabel.Font = GV.generalFont;
            descriptionLabel.Location = new Point(150, 30);
            this.Controls.Add(descriptionLabel);

            descriptionTB = new TextBox();
            descriptionTB.Name = "descriptionTextBox";
            descriptionTB.Text = "";
            descriptionTB.Size = new Size(145, 17);
            descriptionTB.Location = new Point(3, 30);
            descriptionTB.MouseEnter +=new EventHandler(TB_MouseEnter);
            descriptionTB.MouseLeave +=new EventHandler(TB_MouseLeave);
            descriptionTB.TextChanged +=new EventHandler(TB_TextChanged);
            descriptionTB.BringToFront();
            descriptionTB.Multiline = true;
            descriptionTB.WordWrap = true;
            descriptionTB.ScrollBars = ScrollBars.Vertical;
            this.Controls.Add(descriptionTB);
            descriptionTB.BringToFront();

            Button newMissionButton = new Button();
            newMissionButton.Size = new Size(135, 28);
            newMissionButton.Location = new Point(15, 55);
            newMissionButton.Text = "Add new mission";
            newMissionButton.Font = GV.generalFont;
            newMissionButton.ForeColor = Color.White;
            newMissionButton.FlatStyle = FlatStyle.Popup;
            newMissionButton.BackColor = Color.Transparent;
            newMissionButton.BackgroundImage = global::MissionCreator.Properties.Resources.MissionControllerButton;
            newMissionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            newMissionButton.Click +=new EventHandler(newMissionButton_Click);
            this.Controls.Add(newMissionButton);

            CheckBox ownOrderCheckBox = new CheckBox();
            ownOrderCheckBox.Location = new Point(160, 60);
            ownOrderCheckBox.Text = "Sort in order?";
            ownOrderCheckBox.CheckedChanged += new EventHandler(ownOrderCheckBox_CheckedChanged);
            ownOrderCheckBox.AutoSize = true;
            ownOrderCheckBox.Font = GV.generalFont;
            this.Controls.Add(ownOrderCheckBox);
        }
        public void editMissionboxOrder()
        {
            
            int missionBoxIncrement = 95;
            int firstPanelY = 89;
            mainForm MF = (mainForm)this.Parent;
            List<int>heightList = new List<int>();
            List<MissionPackMissionPanel> panelList= new List<MissionPackMissionPanel>();
            foreach (Control panel in this.Controls)  //collects list of all missionOverviewPanels
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

            if (panelList.Count == 1) { panelList[0].Location = new Point (12,firstPanelY); }
            for (int j = 0; j < panelList.Count; j++)
            {
                for (int i = 0; i < panelList.Count-1; i++)
                {
                    if (heightList[i] >= heightList[i + 1])
                    {
                        MF.missionPackage.editMissionOrder(i+1,i);
                        int temp = heightList[i];
                        heightList[i] = heightList[i + 1];
                        heightList[i + 1] = temp;
                        //changes order on GUI
                        

                        int tID = panelList[i].panelID;
                        panelList[i].panelID = panelList[i+1].panelID;
                        panelList[i + 1].panelID = tID;

                        MissionPackMissionPanel tPanel = panelList[i];
                        panelList[i] = panelList[i + 1];
                        panelList[i + 1] = tPanel;
                    }

                }
            }

            //moves panel to top scroll position to allow proper panel placement
            Point autoScrollPosition = this.AutoScrollPosition;
            int verticlePosition = this.VerticalScroll.Value;
            this.VerticalScroll.Value = 0;
            this.AutoScrollPosition = new Point(0, 0);

            foreach (Control panel in this.Controls)  //Collects list of all panels
            {
                if (panel is MissionPackMissionPanel)
                {
                    MissionPackMissionPanel tPanel = (MissionPackMissionPanel)panel;
                    tPanel.Location = new Point(missionPanelStartingX, firstPanelY + missionBoxIncrement * tPanel.panelID);
                    Console.WriteLine(tPanel.panelID + tPanel.Location.ToString());
                }
            }


            Console.WriteLine("---------------------------------");

            //Returns panel to desired position
            if (verticlePosition > this.VerticalScroll.Maximum)
            {
                this.VerticalScroll.Value = this.VerticalScroll.Maximum;
                this.AutoScrollPosition = new Point(0, this.VerticalScroll.Value * -1);
                this.PerformLayout();
            }
            else
            {
                this.AutoScrollPosition = autoScrollPosition;
                this.VerticalScroll.Value = verticlePosition;
                this.PerformLayout();
            }
            descriptionTB.BringToFront();

        }
        private void TB_MouseEnter(object sender, EventArgs e)
        {

            TextBox TB = (TextBox)sender;
            if (sender != null)
            {
                TB.Size = new Size(282, 186);
            }
        }
        private void TB_MouseLeave(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            if (sender != null)
            {
                TB.Size = new Size(145,17);
            }
        }
        private void TB_TextChanged(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];
            MF.missionPackage.editMissionPackage("description", TB.Text);
        }
        private void newMissionButton_Click(object sender, EventArgs e)
        {
            mainForm form = (mainForm)this.Parent;
            form.missionPackage.addMission();
        }
        private void titleLabelSizeChange(object sender, EventArgs e)
        {
            Label title = (Label)sender;
            if (title != null)
            {
                MissionPackagePanel panel = (MissionPackagePanel)title.Parent;
                title.Location = new Point((panel.Width - title.Width) / 2, 6);
            }
        }
        private void ownOrderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox CB = (CheckBox)sender;
            changeBoxColor(sender);
            mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];
            if (CB.Checked == true)
            {
                MF.missionPackage.editMissionPackage("ownOrder", "true");
            }
            else
            {
                MF.missionPackage.editMissionPackage("ownOrder", GV.defaultValue);
            }
        }
        private void packageNameTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];
            MF.missionPackage.editMissionPackage("name", TB.Text);
        }
        private void changeBoxColor(object sender)
        {
            //Used to change the label color of radio / checkboxes. 
            //Accepts the checkbox/radioButton and changes its own labelcolor.
            var box = sender as CheckBox;
            if (box == null)
            {
                RadioButton button = sender as RadioButton;
                if (button != null)
                {
                    if (button.Checked == true) { button.ForeColor = Color.Lime; }
                    else { button.ForeColor = Color.Black; }
                }
            }
            else if (box != null)
            {
                if (box.Checked == true) { box.ForeColor = Color.Lime; }
                else { box.ForeColor = Color.Black; }
            }
        }
    }
    class AllMissionInfoPanel : Panel
    {
        public AllMissionInfoPanel()
        {
            this.Location = new Point(1163, 6);
            this.Size = new Size(270, 652);                                                 //To change this, also change Maximum size property
            this.MaximumSize = new Size(2270, 652);
            this.BackgroundImage = GV.backPanelImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BackColor = Color.Transparent;
            this.AutoScroll = true;
            this.AllowDrop = false;

            Label allInfoLabel = new Label();
            allInfoLabel.Name = "infoLabel";
            allInfoLabel.ForeColor = Color.Lime;
            allInfoLabel.Text = "";
            allInfoLabel.MaximumSize = new Size(260, 5000);
            allInfoLabel.Font = GV.generalFont;
            allInfoLabel.AutoSize = true;
            allInfoLabel.Location = new Point(5, 10);
            this.Controls.Add(allInfoLabel);
        }

    }
}
