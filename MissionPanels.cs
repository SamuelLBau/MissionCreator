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
    class MissionGoalPanel : Panel
    {
        public MissionGoalPanel()
        {
            Point checkBoxStartingLocation = new Point(3, 84);
            Point textBoxCurrentLocation = new Point(50, 84);
            Size textBoxSize = new Size(30, 20);
            //int checkBoxIncrement = 5;

            
            this.BackColor = Color.Red;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.MaximumSize = new Size(267, 200);
            this.MinimumSize = new Size(267, 0);

            Point checkBoxCurrentLocation = checkBoxStartingLocation;



            Label goalLabel = new Label();
            goalLabel.Name = "goalTitleLabel";
            goalLabel.Text = "Mission goal:";
            goalLabel.Font = GV.titleFont;
            goalLabel.Location = new Point(4, 4);
            goalLabel.AutoSize = true;
            this.Controls.Add(goalLabel);



        }
    }
    class MissionPackMissionPanel : Panel
    {
        public int panelID;

        public MissionPackagePanel parent;

        Point mouseDownLocation = new Point(0,0);
        public Panel firstIcon = new Panel();
        public Panel secondIcon = new Panel();
        public MissionPackMissionPanel(string missionName)
        {
            //These allows for the dragging of panels, most controls have it
            //this allows it dragged even if
            //this.MouseDown += new MouseEventHandler(missionPanel_MouseDown);
            //this.MouseMove += new MouseEventHandler(missionPanel_MouseMove);
            //this.MouseUp += new MouseEventHandler(missionPanel_MouseUp);



            this.BackColor = GV.activeMissionColor;
            this.Size = new Size(271, 91);
            this.Name = "missionOverviewPanel";
            this.MouseDown += new MouseEventHandler(missionPanel_MouseDown);
            this.MouseMove +=new MouseEventHandler(missionPanel_MouseMove);
            this.MouseUp += new MouseEventHandler(missionPanel_MouseUp);

            firstIcon.Location = new Point(120, 40);
            firstIcon.Size = new Size(45, 45);
            firstIcon.BackColor = Color.Transparent;
            firstIcon.BackgroundImageLayout = ImageLayout.Stretch;
            secondIcon.Location = new Point(175, 40);
            secondIcon.Size = new Size(45, 45);
            secondIcon.BackColor = Color.Transparent;
            secondIcon.BackgroundImageLayout = ImageLayout.Stretch;
            this.Controls.Add(firstIcon);
            this.Controls.Add(secondIcon);

            firstIcon.MouseDown += new MouseEventHandler(missionPanel_MouseDown);
            firstIcon.MouseMove += new MouseEventHandler(missionPanel_MouseMove);
            firstIcon.MouseUp += new MouseEventHandler(missionPanel_MouseUp);

            secondIcon.MouseDown += new MouseEventHandler(missionPanel_MouseDown);
            secondIcon.MouseMove += new MouseEventHandler(missionPanel_MouseMove);
            secondIcon.MouseUp += new MouseEventHandler(missionPanel_MouseUp);








            Button missionUpButton = new Button();
            missionUpButton.Name = "missionUpButton";
            missionUpButton.Size = new Size(25, 25);
            missionUpButton.Location = new Point(231, 35);
            missionUpButton.Click +=new EventHandler(missionChange_Click);
            missionUpButton.BackgroundImage = GV.upArrowIcon;
            missionUpButton.BackgroundImageLayout = ImageLayout.Stretch;
            missionUpButton.BackColor = Color.Transparent;
            this.Controls.Add(missionUpButton);

            Button missionDownButton = new Button();
            missionDownButton.Name = "missionDownButton";
            missionDownButton.Size = new Size(25, 25);
            missionDownButton.Location = new Point(231, 62);
            missionDownButton.Click += new EventHandler(missionChange_Click);
            missionDownButton.BackgroundImage = GV.downArrowIcon;
            missionDownButton.BackgroundImageLayout = ImageLayout.Stretch;
            missionDownButton.BackColor = Color.Transparent;
            this.Controls.Add(missionDownButton);

            Button removeMissionButton = new Button();
            removeMissionButton.Name = "removeMissionButton";
            removeMissionButton.Size = new Size(25, 25);
            removeMissionButton.Location = new Point(231, 7);
            removeMissionButton.BackColor = Color.Transparent;
            removeMissionButton.BackgroundImage = global::MissionCreator.Properties.Resources.removeIcon;
            removeMissionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            removeMissionButton.Click +=new EventHandler(removeMission_Click);
            this.Controls.Add(removeMissionButton);

            TextBox missionNameTB = new TextBox();
            missionNameTB.Name = "missionNameTextBox";
            missionNameTB.Text = missionName;
            missionNameTB.Size = new Size(182, 20);
            missionNameTB.Location = new Point(15, 8);
            missionNameTB.TextChanged +=new EventHandler(missionName_Changed);
            missionNameTB.MouseDown += new MouseEventHandler(missionPanel_MouseDown);
            missionNameTB.MouseMove += new MouseEventHandler(missionPanel_MouseMove);
            this.Controls.Add(missionNameTB);

            Label rewardLabel = new Label();
            rewardLabel.Text = "Reward";
            rewardLabel.Name = "rewardLabel";
            rewardLabel.Font = GV.generalFont;
            rewardLabel.Location = new Point(35, 40);
            rewardLabel.MouseDown += new MouseEventHandler(missionPanel_MouseDown);
            rewardLabel.MouseMove += new MouseEventHandler(missionPanel_MouseMove);
            this.Controls.Add(rewardLabel);

            Label rewardAmountLabel = new Label();
            rewardAmountLabel.Name = "rewardAmountLabel";
            rewardAmountLabel.AutoSize = true;
            rewardAmountLabel.Font = GV.generalFont;
            rewardAmountLabel.Location = new Point(79 - rewardAmountLabel.PreferredWidth/2, 59);
            rewardAmountLabel.TextChanged += new EventHandler(rewardAmount_Changed);
            rewardAmountLabel.MouseDown += new MouseEventHandler(missionPanel_MouseDown);
            rewardAmountLabel.MouseMove += new MouseEventHandler(missionPanel_MouseMove);
            this.Controls.Add(rewardAmountLabel);
            rewardAmountLabel.BringToFront();
            rewardAmountLabel.Text = "0 K";


        }
        //Event Handlers
        private void missionChange_Click(object sender, EventArgs e)
        {
            Button B = (Button)sender;
            MissionPackMissionPanel MP = (MissionPackMissionPanel)B.Parent;
            MissionPackagePanel parent = (MissionPackagePanel)MP.Parent;
            mainForm MF = (mainForm)this.Parent.Parent;



            int i = 0;
            if(B.Name == "missionUpButton") {i--;}
            else                            {i++;}

            /*foreach (Control control in parent.Controls)
            {
                if(control is  MissionPackMissionPanel)
                {
                    MissionPackMissionPanel MPMP = (MissionPackMissionPanel)control;
                    if (MPMP.panelID == MP.panelID + i)
                    {
                       MPMP.panelID = MPMP.panelID - i;
                       MP.panelID = MP.panelID + i;
                       break;
                    }
                }
            }*/

            //saves scroll position / prepares for proper panel placement
            Point autoScrollPosition = parent.AutoScrollPosition;
            int verticlePosition = parent.VerticalScroll.Value;
            parent.VerticalScroll.Value = 0;
            parent.AutoScrollPosition = new Point(0, 0);
            parent.PerformLayout();

            MP.Location = new Point(MP.Location.X, 89 + 95 * (panelID + i) + i);

            //returns panels to previous position
            if (verticlePosition > parent.VerticalScroll.Maximum)
            {
                parent.VerticalScroll.Value = parent.VerticalScroll.Maximum;
                parent.AutoScrollPosition = new Point(0, parent.VerticalScroll.Value * -1);
                parent.PerformLayout();
            }
            else
            {
                parent.AutoScrollPosition = autoScrollPosition;
                parent.VerticalScroll.Value = verticlePosition;
                parent.PerformLayout();
            }
            parent.editMissionboxOrder();
            MF.missionPackage.changeActiveMission(MP.panelID);
        }
        private void rewardAmount_Changed(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.Location = new Point(58 - TextRenderer.MeasureText(label.Text, label.Font).Width/2, 59);

        }
        private void missionName_Changed(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            mainForm mf = (mainForm)this.Parent.Parent;
            mf.missionPackage.editMission("name", TB.Text);
        }
        private void removeMission_Click(object sender, EventArgs e)
        {

            mainForm MF = (mainForm)Application.OpenForms[0];
            MF.missionPackage.removeMission(panelID);
        }
        private void missionPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.Capture = true;
            if (GV.debug == true)
            {
                Console.WriteLine(panelID);
            }



            mouseDownLocation = e.Location;
            TextBox TB = (TextBox)this.Controls["missionNameTextBox"];
            mainForm MF = (mainForm)this.Parent.Parent;
            MF.missionPackage.changeActiveMission(panelID);
            TB.Focus();
            TB.SelectionStart = 0;
            TB.SelectionLength = TB.Text.Length;

        }
        private void missionPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // I unno what these buttons are, but it didn't like them
            {
                this.Top += e.Y - mouseDownLocation.Y;
            }
        }
        private void missionPanel_MouseUp(object sender, MouseEventArgs e)
        {
            MissionPackMissionPanel panel = (MissionPackMissionPanel)sender;


            panel.BringToFront();
            MissionPackagePanel parent = (MissionPackagePanel)this.Parent;
            parent.editMissionboxOrder();
            this.Capture = false;

            mainForm MF = (mainForm)parent.Parent;
            MF.missionPackage.changeActiveMission(panelID);

        }

        //Supplemental methods
        public void rewardAmountChanged(string krones, string science)
        {
            Label rewardAmountLabel = (Label)this.Controls["rewardAmountLabel"];
            if (krones == GV.defaultValue) { krones = "0"; }
            rewardAmountLabel.Text = string.Format("{0:#,###0}", krones + " K");
            if (science != GV.defaultValue)
            {
                rewardAmountLabel.Text += "\n" + science + " sp";
                rewardAmountLabel.Location = new Point(79 - rewardAmountLabel.PreferredWidth / 2, 59);
            }
        }
    }
    class GoalPanel : Panel
    {
        Point mouseDownLocation = new Point();
        public int panelID;
        MissionInfoPanel parent;

        public GoalPanel(MissionInfoPanel P)
        {
            this.Size = new Size(260, 85);
            this.BackColor = GV.activeGoalColor;
            this.MouseDown += new MouseEventHandler(panel_mouseDown);
            this.MouseMove +=new MouseEventHandler(panel_mouseMove);
            this.MouseUp +=new MouseEventHandler(panel_mouseUp);
            this.AllowDrop = false;

            parent = P;
            this.Location = P.panelStartingLocation;

            Label goalTypeLabel = new Label();
            goalTypeLabel.Name = "goalTypeLabel";
            goalTypeLabel.Text = "Orbit";
            goalTypeLabel.BackColor = Color.Transparent;
            goalTypeLabel.AutoSize = true;
            goalTypeLabel.Location = new Point(9, 9);
            goalTypeLabel.Font = GV.generalFont;
            goalTypeLabel.MouseDown += new MouseEventHandler(panel_mouseDown);
            goalTypeLabel.MouseMove += new MouseEventHandler(panel_mouseMove);
            this.Controls.Add(goalTypeLabel);

            Label rewardLabel = new Label();
            rewardLabel.Name = "rewardLabel";
            rewardLabel.AutoSize = true;
            rewardLabel.Location = new Point(120 - rewardLabel.PreferredWidth / 2, 65);
            rewardLabel.TextAlign = (ContentAlignment)HorizontalAlignment.Center;
            rewardLabel.Font = GV.generalFont;
            rewardLabel.MouseDown += new MouseEventHandler(panel_mouseDown);
            rewardLabel.MouseMove += new MouseEventHandler(panel_mouseMove);
            rewardLabel.Text = "0 K";
            rewardLabel.MaximumSize = new Size(100, 50);
            this.Controls.Add(rewardLabel);
            rewardLabel.BringToFront();



            //need to give icon up arrow
            Button upButton = new Button();
            upButton.Name = "goalUpButton";
            upButton.Location = new Point(226, 30);
            upButton.Size = new Size(25, 25);
            upButton.Font = GV.generalFont;
            upButton.Click += new EventHandler(goalChange_Click);
            upButton.BackgroundImage = GV.upArrowIcon;
            upButton.BackgroundImageLayout = ImageLayout.Stretch;
            upButton.BackColor = Color.Transparent;
            this.Controls.Add(upButton);
            upButton.BringToFront();

            //need to give this down arrow Icon
            Button downButton = new Button();
            downButton.Name = "goalDownButton";
            downButton.Location = new Point(226, 57);
            downButton.Size = new Size(25, 25);
            downButton.Font = GV.generalFont;
            downButton.Click += new EventHandler(goalChange_Click);
            downButton.BackgroundImage = GV.downArrowIcon;
            downButton.BackgroundImageLayout = ImageLayout.Stretch;
            downButton.BackColor = Color.Transparent;
            this.Controls.Add(downButton);
            downButton.BringToFront();

            Button removeButton = new Button();
            removeButton.Name = "removeButton";
            removeButton.Location = new Point(226, 5);
            removeButton.Size = new Size(25, 25);
            removeButton.Font = GV.generalFont;
            removeButton.Click += new EventHandler(removeGoal_Click);
            removeButton.BackgroundImage = GV.removeIcon;
            removeButton.BackgroundImageLayout = ImageLayout.Stretch;
            removeButton.BackColor = Color.Transparent;
            this.Controls.Add(removeButton);
            removeButton.BringToFront();
        }

        public void rewardAmountChanged(string krones)
        {
            Label rewardAmountLabel = (Label)this.Controls["rewardLabel"];
            if (krones == GV.defaultValue) { krones = "0"; }
            rewardAmountLabel.Text = string.Format("{0:#,###0}", krones + " K");
            rewardAmountLabel.Location = new Point(120 - rewardAmountLabel.PreferredWidth / 2, 65);
        }
        public void goalTypeChanged(string goalTypes)
        {
            Label rewardAmountLabel = (Label)this.Controls["goalTypeLabel"];
            rewardAmountLabel.Text = "";
            string[] typeList = goalTypes.Replace(" ", "").Split(',');
            for (int i = 0; i < typeList.Length; i++)
            {
                rewardAmountLabel.Text += typeList[i] + "  ";
                if (i % 3 == 0) {rewardAmountLabel.Text += "\n";}
            }

        }
        private void removeGoal_Click(object sender, EventArgs e)
        {
            Button B = (Button)sender;
            GoalPanel GP = (GoalPanel)B.Parent;
            mainForm MF = (mainForm)parent.Parent;
            MF.missionPackage.removeGoal(GP.panelID);
            parent.editGoalPanelOrder();

        }
        private void panel_mouseDown(object sender, MouseEventArgs e)
        {
            this.Capture = true;
            //Console.WriteLine(panelID);
            mouseDownLocation = e.Location;
            parent.editGoalPanelOrder();
            mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];
            MF.missionPackage.changeActiveGoal(panelID);
        }
        private void panel_mouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Top += e.Y - mouseDownLocation.Y;
            }
        }
        private void panel_mouseUp(object sender, MouseEventArgs e)
        {
            GoalPanel panel = (GoalPanel)sender;


            panel.BringToFront();
            parent.editGoalPanelOrder();
            this.Capture = false;

         //   parent.changeGoalOrder();
        }
        private void goalChange_Click(object sender, EventArgs e)
        {
            Button B = (Button)sender;
            GoalPanel MP = (GoalPanel)B.Parent;
            MissionInfoPanel parent = (MissionInfoPanel)MP.Parent;
            mainForm MF = (mainForm)this.Parent.Parent;



            int i = 0;
            if (B.Name == "goalUpButton") { i--; }
            else { i++; }


            //saves scroll position / prepares for proper panel placement
            Point autoScrollPosition = parent.AutoScrollPosition;
            int verticlePosition = parent.VerticalScroll.Value;
            parent.VerticalScroll.Value = 0;
            parent.AutoScrollPosition = new Point(0, 0);
            parent.PerformLayout();

            MP.Location = new Point(MP.Location.X, parent.panelStartingLocation.Y + GV.goalPanelHeight * (panelID + i) + i);

            //returns panels to previous position
            if (verticlePosition > parent.VerticalScroll.Maximum)
            {
                parent.VerticalScroll.Value = parent.VerticalScroll.Maximum;
                parent.AutoScrollPosition = new Point(0, parent.VerticalScroll.Value * -1);
                parent.PerformLayout();
            }
            else
            {
                parent.AutoScrollPosition = autoScrollPosition;
                parent.VerticalScroll.Value = verticlePosition;
                parent.PerformLayout();
            }
            parent.editGoalPanelOrder();
        }
    }
}
