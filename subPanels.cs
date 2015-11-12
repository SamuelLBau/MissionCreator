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
    public class partsSubPanel : Panel
    {
        public partsSubPanel(int iteration)
        {
            Point partNameLabelLocation = new Point(8, 11);
            Point numberRequiredLabelLocation = new Point(8, 37);
            Point partNameTextBoxLocation = new Point(87, 7);
            Point numberRequiredTextBoxLocation = new Point(109, 33);
            Point maxRadioButtonLocation = new Point(190, 32);
            Size partNameTextBoxSize = new Size(105, 20);
            Size numberRequiredTextBoxSize = new Size(23, 20);

            this.BackColor = Color.DarkGray;
            this.Size = new Size(241, 56);
            this.Show();

            CheckBox minCheckBox = new CheckBox();
            minCheckBox.Text = "Min";
            minCheckBox.Font = GV.generalFont;
            minCheckBox.Location = new Point(75, 33);
            this.Controls.Add(minCheckBox);
            minCheckBox.AutoSize = true;
            minCheckBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
            minCheckBox.Name = GV.minPartRequiredName + "CheckBox";   //"minNumberRequiredCheckBox";

            CheckBox maxCheckBox = new CheckBox();
            maxCheckBox.Text = "Max";
            maxCheckBox.Font = GV.generalFont;
            maxCheckBox.Location = new Point(158, 33);
            maxCheckBox.AutoSize = true;
            maxCheckBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
            this.Controls.Add(maxCheckBox);
            maxCheckBox.Name = GV.maxPartRequiredName + "CheckBox";//"maxNumberRequiredCheckBox";

            TextBox minNumberRequiredTextBox = new TextBox();
            minNumberRequiredTextBox.Size = numberRequiredTextBoxSize;
            minNumberRequiredTextBox.Text = "0";
            minNumberRequiredTextBox.Enabled = false;
            minNumberRequiredTextBox.Location = new Point(125, 31);
            this.Controls.Add(minNumberRequiredTextBox);
            minNumberRequiredTextBox.BringToFront();
            minNumberRequiredTextBox.TextChanged += new EventHandler(textBox_TextChanged);
            minNumberRequiredTextBox.Name = GV.minPartRequiredName + "TextBox"; //"minNumberRequiredTextBox";

            TextBox maxNumberRequiredTextBox = new TextBox();
            maxNumberRequiredTextBox.Size = numberRequiredTextBoxSize;
            maxNumberRequiredTextBox.Location = new Point(213, 31);
            maxNumberRequiredTextBox.Text = "0";
            this.Controls.Add(maxNumberRequiredTextBox);
            maxNumberRequiredTextBox.BringToFront();
            maxNumberRequiredTextBox.Enabled = false;
            maxNumberRequiredTextBox.TextChanged += new EventHandler(textBox_TextChanged);
            maxNumberRequiredTextBox.Name = GV.maxPartRequiredName + "TextBox"; //"maxNumberRequiredTextBox";


            Label partNameLabel = new Label();
            partNameLabel.Text = "Part name:";
            partNameLabel.Name = "partNameLabel";
            partNameLabel.Font = GV.generalFont;
            partNameLabel.Location = new Point(8, 9);
            partNameLabel.AutoSize = true;

            this.Controls.Add(partNameLabel);

            Label numberRequiredLabel = new Label();
            numberRequiredLabel.Text = " Number\nRequired";
            numberRequiredLabel.Name = "numberRequiredLabel";
            numberRequiredLabel.Font = GV.generalFont;
            numberRequiredLabel.Location = new Point(5, 25);
            numberRequiredLabel.AutoSize = true;
            this.Controls.Add(numberRequiredLabel);

            TextBox partNameTextBox = new TextBox();
            partNameTextBox.Size = partNameTextBoxSize;
            partNameTextBox.TextChanged +=new EventHandler(textBox_TextChanged);
            partNameTextBox.Location = new Point(87, 7);
            this.Controls.Add(partNameTextBox);
            partNameTextBox.BringToFront();
            partNameTextBox.Name = GV.partNameTextBoxName;


        }
        private void changeBoxColor(TextBox TB)
        {
            int garbage;
            //this segment edits the color of the checkbox text color
            if (int.TryParse(TB.Text, out garbage) == false)
            {
                if (TB.Name != GV.partNameTextBoxName) //this is here because was having problems with nameBox calling it
                {
                    TB.Parent.Controls[TB.Name.Replace("TextBox", "CheckBox")].ForeColor = Color.Red;
                }
            }
            else
            {
                if (TB.Name != GV.partNameTextBoxName) //this is here because was having problems with nameBox calling it
                {
                    TB.Parent.Controls[TB.Name.Replace("TextBox", "CheckBox")].ForeColor = Color.Lime;
                }
            }
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            int garbage;
            TextBox TB = (TextBox)sender;
            TextBox tempTB = TB;
            Panel PIP = (Panel)TB.Parent.Parent;


            string partNameList = ",";
            string minValueList = ",";
            string maxValueList = ",";

            //this segment collects the string values to be placed in the missionInfo
            foreach (Control control in PIP.Controls)
            {
                if (control is partsSubPanel)
                {
                    partsSubPanel PSP = (partsSubPanel)control;
                    string minNameCB = GV.minPartRequiredName + "CheckBox";
                    string maxNameCB = GV.maxPartRequiredName + "CheckBox";
                    string minNameTB = GV.minPartRequiredName + "TextBox";
                    string maxNameTB = GV.maxPartRequiredName + "TextBox";
                    CheckBox minCB = (CheckBox)PSP.Controls[minNameCB];
                    CheckBox maxCB = (CheckBox)PSP.Controls[maxNameCB];


                    //This either adds name to partslist or doesn't
                    if (minCB.Checked == true || maxCB.Checked == true)
                    {
                        TB = (TextBox)PSP.Controls[GV.partNameTextBoxName];
                        partNameList += TB.Text + ',';
                        if (minCB.Checked == true)
                        {
                            TB = (TextBox)PSP.Controls[minNameTB];
                            if (int.TryParse(TB.Text, out garbage) == true)
                            {
                                minValueList += TB.Text + ',';
                            }
                            else
                            {
                                minValueList += GV.defaultValue + ",";
                            }
                        }
                        else
                        {
                            minValueList += GV.defaultValue + ",";
                        }
                        if (maxCB.Checked == true)
                        {
                            TB = (TextBox)PSP.Controls[maxNameTB];
                            if (int.TryParse(TB.Text, out garbage) == true)
                            {
                                maxValueList += TB.Text + ',';
                            }
                            else
                            {
                                maxValueList += GV.defaultValue + ",";
                            }
                        }
                        else
                        {
                            maxValueList += GV.defaultValue + ",";
                        }
                    }
                    changeBoxColor(tempTB);
                }
            }

            //removes first and last commas from strings, as appropriate
            partNameList = partNameList.Remove(partNameList.Length - 1);
            if (partNameList.Length != 0) { partNameList = partNameList.Remove(0, 1); }
            minValueList = minValueList.Remove(minValueList.Length - 1);
            if (minValueList.Length != 0) { minValueList = minValueList.Remove(0, 1); }
            maxValueList = maxValueList.Remove(maxValueList.Length - 1);
            if (maxValueList.Length != 0) { maxValueList = maxValueList.Remove(0, 1); }
            mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];


            //updates mission values
            if (partNameList.Length == 0) { MF.missionPackage.editMissionGoal("partName", GV.defaultValue, "Part"); }
            else { MF.missionPackage.editMissionGoal("partName", partNameList, "Part"); }

            if (minValueList.Length == 0) { MF.missionPackage.editMissionGoal("partCount", GV.defaultValue, "Part"); }
            else { MF.missionPackage.editMissionGoal("partCount", minValueList, "Part"); }

            if (maxValueList.Length == 0) { MF.missionPackage.editMissionGoal("maxPartCount", GV.defaultValue, "Part"); }
            else { MF.missionPackage.editMissionGoal("maxPartCount", maxValueList, "Part"); }
        }
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox CB = (CheckBox)sender;
            Panel PIP = (Panel)CB.Parent.Parent;

            TextBox TB = (TextBox)PIP.Controls[CB.Name.Replace("CheckBox", "TextBox")];
            //Used to change the label color of radio / checkboxes. 
            //Accepts the checkbox/radioButton and changes its own labelcolor.
            if (CB != null)
            {
                if (CB.Checked == true)
                {
                    CB.Parent.Controls[CB.Name.Replace("CheckBox", "TextBox")].Enabled = true;
                    CB.ForeColor = Color.Lime;

                }
                else
                {
                    CB.Parent.Controls[CB.Name.Replace("CheckBox", "TextBox")].Text = "0";
                    CB.Parent.Controls[CB.Name.Replace("CheckBox", "TextBox")].Enabled = false;
                    CB.ForeColor = Color.Black;

                }
            }
        }
    }
    public class resourceSubPanel : Panel
    {
        public resourceSubPanel(int iteration)
        {
            Point resourceNameLabelLocation = new Point(8, 11);
            Point numberRequiredLabelLocation = new Point(8, 37);
            Point resourceNameTextBoxLocation = new Point(87, 7);
            Point numberRequiredTextBoxLocation = new Point(109, 33);
            Point maxRadioButtonLocation = new Point(190, 32);
            Size resourceNameTextBoxSize = new Size(105, 20);
            Size numberRequiredTextBoxSize = new Size(23, 20);

            this.BackColor = Color.DarkGray;
            this.Size = new Size(241, 56);
            this.Show();

            CheckBox minCheckBox = new CheckBox();
            minCheckBox.Text = "Min";
            minCheckBox.Font = GV.generalFont;
            minCheckBox.Location = new Point(75, 33);
            this.Controls.Add(minCheckBox);
            minCheckBox.AutoSize = true;
            minCheckBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
            minCheckBox.Name = GV.minResourceRequiredName + "CheckBox";   //"minNumberRequiredCheckBox";

            CheckBox maxCheckBox = new CheckBox();
            maxCheckBox.Text = "Max";
            maxCheckBox.Font = GV.generalFont;
            maxCheckBox.Location = new Point(158, 33);
            maxCheckBox.AutoSize = true;
            maxCheckBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
            this.Controls.Add(maxCheckBox);
            maxCheckBox.Name = GV.maxResourceRequiredName + "CheckBox";//"maxNumberRequiredCheckBox";

            TextBox minNumberRequiredTextBox = new TextBox();
            minNumberRequiredTextBox.Size = numberRequiredTextBoxSize;
            minNumberRequiredTextBox.Text = "0";
            minNumberRequiredTextBox.Enabled = false;
            minNumberRequiredTextBox.Location = new Point(125, 31);
            this.Controls.Add(minNumberRequiredTextBox);
            minNumberRequiredTextBox.BringToFront();
            minNumberRequiredTextBox.TextChanged += new EventHandler(resourceTextBox_TextChanged);
            minNumberRequiredTextBox.Name = GV.minResourceRequiredName + "TextBox"; //"minNumberRequiredTextBox";

            TextBox maxNumberRequiredTextBox = new TextBox();
            maxNumberRequiredTextBox.Size = numberRequiredTextBoxSize;
            maxNumberRequiredTextBox.Location = new Point(213, 31);
            maxNumberRequiredTextBox.Text = "0";
            this.Controls.Add(maxNumberRequiredTextBox);
            maxNumberRequiredTextBox.BringToFront();
            maxNumberRequiredTextBox.Enabled = false;
            maxNumberRequiredTextBox.TextChanged += new EventHandler(resourceTextBox_TextChanged);
            maxNumberRequiredTextBox.Name = GV.maxResourceRequiredName + "TextBox"; //"maxNumberRequiredTextBox";


            Label resourceNameLabel = new Label();
            resourceNameLabel.Text = "Resource name:";
            resourceNameLabel.Name = "resourceNameLabel";
            resourceNameLabel.Font = GV.generalFont;
            resourceNameLabel.Location = new Point(8, 9);
            resourceNameLabel.AutoSize = true;

            this.Controls.Add(resourceNameLabel);

            Label numberRequiredLabel = new Label();
            numberRequiredLabel.Text = " Number\nRequired";
            numberRequiredLabel.Name = "numberRequiredLabel";
            numberRequiredLabel.Font = GV.generalFont;
            numberRequiredLabel.Location = new Point(5, 25);
            numberRequiredLabel.AutoSize = true;
            this.Controls.Add(numberRequiredLabel);

            TextBox resourceNameTextBox = new TextBox();
            resourceNameTextBox.Size = resourceNameTextBoxSize;
            resourceNameTextBox.TextChanged += new EventHandler(resourceTextBox_TextChanged);
            resourceNameTextBox.Location = new Point(125, 7);
            this.Controls.Add(resourceNameTextBox);
            resourceNameTextBox.BringToFront();
            resourceNameTextBox.Name = GV.resourceNameTextBoxName;


        }
        private void changePartBoxColor(TextBox TB)
        {
            int garbage;
            //this segment edits the color of the checkbox text color
            if (int.TryParse(TB.Text, out garbage) == false)
            {
                if (TB.Name != GV.partNameTextBoxName) //this is here because was having problems with nameBox calling it
                {
                    TB.Parent.Controls[TB.Name.Replace("TextBox", "CheckBox")].ForeColor = Color.Red;
                }
            }
            else
            {
                if (TB.Name != GV.partNameTextBoxName) //this is here because was having problems with nameBox calling it
                {
                    TB.Parent.Controls[TB.Name.Replace("TextBox", "CheckBox")].ForeColor = Color.Lime;
                }
            }
        }
        private void changeResourceBoxColor(TextBox TB)
        {
            int garbage;
            //this segment edits the color of the checkbox text color
            if (int.TryParse(TB.Text, out garbage) == false)
            {
                if (TB.Name != GV.resourceNameTextBoxName) //this is here because was having problems with nameBox calling it
                {
                    TB.Parent.Controls[TB.Name.Replace("TextBox", "CheckBox")].ForeColor = Color.Red;
                }
            }
            else
            {
                if (TB.Name != GV.resourceNameTextBoxName) //this is here because was having problems with nameBox calling it
                {
                    TB.Parent.Controls[TB.Name.Replace("TextBox", "CheckBox")].ForeColor = Color.Lime;
                }
            }
        }
        private void resourceTextBox_TextChanged(object sender, EventArgs e)
        {
            int garbage;
            TextBox TB = (TextBox)sender;
            TextBox tempTB = TB;
            Panel PIP = (Panel)TB.Parent.Parent;


            string resourceNameList = ",";
            string minValueList = ",";
            string maxValueList = ",";

            //this segment collects the string values to be placed in the missionInfo
            foreach (Control control in PIP.Controls)
            {
                if (control is resourceSubPanel)
                {
                    resourceSubPanel PSP = (resourceSubPanel)control;
                    string minNameCB = GV.minResourceRequiredName + "CheckBox";
                    string maxNameCB = GV.maxResourceRequiredName + "CheckBox";
                    string minNameTB = GV.minResourceRequiredName + "TextBox";
                    string maxNameTB = GV.maxResourceRequiredName + "TextBox";
                    CheckBox minCB = (CheckBox)PSP.Controls[minNameCB];
                    CheckBox maxCB = (CheckBox)PSP.Controls[maxNameCB];


                    //This either adds name to partslist or doesn't
                    if (minCB.Checked == true || maxCB.Checked == true)
                    {
                        TB = (TextBox)PSP.Controls[GV.resourceNameTextBoxName];
                        resourceNameList += TB.Text + ',';
                        if (minCB.Checked == true)
                        {
                            TB = (TextBox)PSP.Controls[minNameTB];
                            if (int.TryParse(TB.Text, out garbage) == true)
                            {
                                minValueList += TB.Text + ',';
                            }
                            else
                            {
                                minValueList += GV.defaultValue + ",";
                            }
                        }
                        else
                        {
                            minValueList += GV.defaultValue + ",";
                        }
                        if (maxCB.Checked == true)
                        {
                            TB = (TextBox)PSP.Controls[maxNameTB];
                            if (int.TryParse(TB.Text, out garbage) == true)
                            {
                                maxValueList += TB.Text + ',';
                            }
                            else
                            {
                                maxValueList += GV.defaultValue + ",";
                            }
                        }
                        else
                        {
                            maxValueList += GV.defaultValue + ",";
                        }
                    }
                    changeResourceBoxColor(tempTB);
                }
            }

            //removes first and last commas from strings, as appropriate
            resourceNameList = resourceNameList.Remove(resourceNameList.Length - 1);
            if (resourceNameList.Length != 0) { resourceNameList = resourceNameList.Remove(0, 1); }
            minValueList = minValueList.Remove(minValueList.Length - 1);
            if (minValueList.Length != 0) { minValueList = minValueList.Remove(0, 1); }
            maxValueList = maxValueList.Remove(maxValueList.Length - 1);
            if (maxValueList.Length != 0) { maxValueList = maxValueList.Remove(0, 1); }
            mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];


            //updates mission values
            if (resourceNameList.Length == 0) { MF.missionPackage.editMissionGoal("name", GV.defaultValue, "Resource"); }
            else { MF.missionPackage.editMissionGoal("name", resourceNameList, "Resource"); }

            if (minValueList.Length == 0) { MF.missionPackage.editMissionGoal("minAmount", GV.defaultValue, "Resource"); }
            else { MF.missionPackage.editMissionGoal("minAmount", minValueList, "Resource"); }

            if (maxValueList.Length == 0) { MF.missionPackage.editMissionGoal("maxAmount", GV.defaultValue, "Resource"); }
            else { MF.missionPackage.editMissionGoal("maxAmount", maxValueList, "Resource"); }
        }
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox CB = (CheckBox)sender;
            Panel PIP = (Panel)CB.Parent.Parent;

            TextBox TB = (TextBox)PIP.Controls[CB.Name.Replace("CheckBox", "TextBox")];
            //Used to change the label color of radio / checkboxes. 
            //Accepts the checkbox/radioButton and changes its own labelcolor.
            if (CB != null)
            {
                if (CB.Checked == true)
                {
                    CB.Parent.Controls[CB.Name.Replace("CheckBox", "TextBox")].Enabled = true;
                    CB.ForeColor = Color.Lime;

                }
                else
                {
                    CB.Parent.Controls[CB.Name.Replace("CheckBox", "TextBox")].Text = "0";
                    CB.Parent.Controls[CB.Name.Replace("CheckBox", "TextBox")].Enabled = false;
                    CB.ForeColor = Color.Black;

                }
            }
        }
    }
}
