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
    class PartsPanel : Panel
    {
        int initialY = 60;
        int startingX = 5;
        int maxNumberTypes = 10;
        public PartsPanel(string[,] partsFieldArray)
        {

            //int startingX = 5;
            //int startingY = 60;
            this.AutoSize = true;
            this.MinimumSize = new Size(311, 0);
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = Color.DarkSalmon;
            this.Location = new Point(56, 237);
            this.Name = "Part";
            this.Resize += new EventHandler(partsPanel_resize);



            Label numberPartTypesLabel = new System.Windows.Forms.Label();
            numberPartTypesLabel.AutoSize = true;
            numberPartTypesLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            numberPartTypesLabel.Location = new System.Drawing.Point(250, 36);
            numberPartTypesLabel.Name = "numberPartTypesLabel";
            numberPartTypesLabel.Size = new System.Drawing.Size(41, 12);
            numberPartTypesLabel.TabIndex = 3;
            numberPartTypesLabel.Text = "Types";                              //Note: If you change this, you must also change the change Label code in changeTypesLabel() below
            this.Controls.Add(numberPartTypesLabel);

            TextBox numberPartTypesTextBox = new System.Windows.Forms.TextBox();
            numberPartTypesTextBox.Location = new System.Drawing.Point(219, 32);
            numberPartTypesTextBox.Name = "numberPartTypesTextBox";
            numberPartTypesTextBox.Size = new System.Drawing.Size(23, 20);
            numberPartTypesTextBox.TabIndex = 2;
            numberPartTypesTextBox.Text = "0";
            numberPartTypesTextBox.TextChanged += new EventHandler(numberPartTypes_TextChanged);
            this.Controls.Add(numberPartTypesTextBox);


            Label numberPartTypesPromptLabel = new System.Windows.Forms.Label();
            numberPartTypesPromptLabel.AutoSize = true;
            numberPartTypesPromptLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            numberPartTypesPromptLabel.Location = new System.Drawing.Point(3, 28);
            numberPartTypesPromptLabel.Name = "numberPartTypesPromptLabel";
            numberPartTypesPromptLabel.Size = new System.Drawing.Size(210, 24);
            numberPartTypesPromptLabel.TabIndex = 1;
            numberPartTypesPromptLabel.Text = "How many different types of parts\r\nshould be necessary for this goal?";
            this.Controls.Add(numberPartTypesPromptLabel);


            Label partsParametersLabel = new System.Windows.Forms.Label();
            partsParametersLabel.AutoSize = true;
            partsParametersLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            partsParametersLabel.Location = new System.Drawing.Point(75, 7);
            partsParametersLabel.Name = "partsParametersLabel";
            partsParametersLabel.Size = new System.Drawing.Size(170, 16);
            partsParametersLabel.TabIndex = 0;
            partsParametersLabel.Text = "Parts goal parameters";
            this.Controls.Add(partsParametersLabel);
            this.VisibleChanged += new EventHandler(PartsPanel_VisibleChanged);
        }
        private void PartsPanel_VisibleChanged(object sender, EventArgs e)
        {

            mainForm parent = this.Parent as mainForm;
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                Control currentControl = this.Controls[i];
                TextBox tb = currentControl as TextBox;
                if (tb != null)
                {
                    if (tb.Name == "numberPartTypesTextBox")
                    {
                        tb.Text = "0";
                    }
                }
            }
            if (parent != null)
            {
                parent.changeGoalPanels(null);
            }
        }
        private void partsPanel_resize(object sender, EventArgs e)
        {
        }
        private void numberPartTypes_TextChanged(object sender, EventArgs e)
        {
            int currentY = initialY;
            int number;
            TextBox tb = sender as TextBox;
            intChangeTextBox(tb);
            if (tb != null)
            {
                for (int i = this.Controls.Count - 1; i >= 0; i--)   //This loop removes all panels, note it is independant of the integer check below
                {
                    Control currentControl = this.Controls[i];
                    Panel panel = currentControl as Panel;
                    if (panel != null)
                    {
                        this.Controls.Remove(panel);
                    }
                }
                if (int.TryParse(tb.Text, out number))                       //Checks if input is a valid integer
                {
                    if (number > maxNumberTypes) { number = maxNumberTypes; tb.Text = maxNumberTypes.ToString(); }   //If input number is to big, it is changed to the maximum number

                    for (int i = 0; i < number; i++)                       //Places the propor amount of panels
                    {
                        partsSubPanel subPanel = new partsSubPanel(i);
                        subPanel.Location = new Point(startingX, currentY);
                        this.Controls.Add(subPanel);
                        currentY += subPanel.Height + 5;
                    }
                }
                Panel goalDetailPanel = this.Parent as Panel;
                mainForm MAINFORM = goalDetailPanel.Parent as mainForm;               //Gets to mainform level
                if (MAINFORM != null)
                {
                    MAINFORM.changeGoalPanels(this);
                }
            }

        }
        private void intChangeTextBox(TextBox tb)
        {
            int igarbage;
            string name = getControlName(tb.Name, "TextBox", "Label");
            for (int i = tb.Parent.Controls.Count - 1; i >= 0; i--)
            {
                Control currentControl = tb.Parent.Controls[i];
                Label cL = currentControl as Label;
                if (cL != null)
                {
                    if (cL.Name == name)
                    {
                        if (int.TryParse(tb.Text, out igarbage))
                        {
                            cL.ForeColor = Color.Black;
                        }
                        else
                        {
                            cL.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }
        private string getControlName(string inputName, string fromControl, string toControl)
        {
            string name = inputName;
            int tnumber;
            if (int.TryParse(name.Substring(name.Length - 2), out tnumber))
            {
                name = name.Remove(name.Length - 2, 2);   //temporarily holds the number of the name
            }
            else if (int.TryParse(name.Substring(name.Length - 1), out tnumber))
            {
                name = name.Remove(name.Length - 1, 1);
            }
            name = name.Replace(fromControl, "");    //removes checkbox from the name
            name = name + toControl;
            return name;
        }
    }
    class ResourcePanel : Panel
    {
        int initialY = 60;
        int startingX = 5;
        int maxNumberTypes = 10;
        public ResourcePanel(string[,] resourceFieldArray)
        {
            this.AutoSize = true;
            this.MinimumSize = new Size(311, 0);
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = Color.DarkSalmon;
            this.Name = "Resource";
            this.Location = new Point(56, 237);



            Label numberResourceTypesLabel = new System.Windows.Forms.Label();
            numberResourceTypesLabel.AutoSize = true;
            numberResourceTypesLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            numberResourceTypesLabel.Location = new System.Drawing.Point(250, 36);
            numberResourceTypesLabel.Name = "numberResourceTypesLabel";
            numberResourceTypesLabel.Size = new System.Drawing.Size(41, 12);
            numberResourceTypesLabel.TabIndex = 3;
            numberResourceTypesLabel.Text = "Types";                              //Note: If you change this, you must also change the change Label code in changeTypesLabel() below
            this.Controls.Add(numberResourceTypesLabel);

            TextBox numberResourceTypesTextBox = new System.Windows.Forms.TextBox();
            numberResourceTypesTextBox.Location = new System.Drawing.Point(219, 32);
            numberResourceTypesTextBox.Name = "numberResourceTypesTextBox";
            numberResourceTypesTextBox.Size = new System.Drawing.Size(23, 20);
            numberResourceTypesTextBox.TabIndex = 2;
            numberResourceTypesTextBox.Text = "0";
            numberResourceTypesTextBox.TextChanged += new EventHandler(numberResourceTypes_TextChanged);
            this.Controls.Add(numberResourceTypesTextBox);


            Label numberResourceTypesPromptLabel = new System.Windows.Forms.Label();
            numberResourceTypesPromptLabel.AutoSize = true;
            numberResourceTypesPromptLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            numberResourceTypesPromptLabel.Location = new System.Drawing.Point(3, 28);
            numberResourceTypesPromptLabel.Name = "numberResourceTypesPromptLabel";
            numberResourceTypesPromptLabel.Size = new System.Drawing.Size(210, 24);
            numberResourceTypesPromptLabel.TabIndex = 1;
            numberResourceTypesPromptLabel.Text = "How many types of Resource\r\nshould be necessary for this goal?";
            this.Controls.Add(numberResourceTypesPromptLabel);


            Label resourceParametersLabel = new System.Windows.Forms.Label();
            resourceParametersLabel.AutoSize = true;
            resourceParametersLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            resourceParametersLabel.Location = new System.Drawing.Point(50, 7);
            resourceParametersLabel.Name = "resourceParametersLabel";
            resourceParametersLabel.Size = new System.Drawing.Size(170, 16);
            resourceParametersLabel.TabIndex = 0;
            resourceParametersLabel.Text = "Resource goal parameters";
            this.Controls.Add(resourceParametersLabel);
            this.VisibleChanged += new EventHandler(resourcePanel_VisibleChanged);
        }
        private void numberResourceTypes_TextChanged(object sender, EventArgs e)
        {
            int currentY = initialY;
            int number;
            TextBox tb = sender as TextBox;
            intChangeTextBox(tb);
            if (tb != null)
            {
                for (int i = this.Controls.Count - 1; i >= 0; i--)   //This loop removes all panels, note it is independant of the integer check below
                {
                    Control currentControl = this.Controls[i];
                    Panel panel = currentControl as Panel;
                    if (panel != null)
                    {
                        this.Controls.Remove(panel);
                    }
                }
                if (int.TryParse(tb.Text, out number))                       //Checks if input is a valid integer
                {
                    if (number > maxNumberTypes) { number = maxNumberTypes; tb.Text = maxNumberTypes.ToString(); }   //If input number is to big, it is changed to the maximum number

                    for (int i = 0; i < number; i++)                       //Places the propor amount of panels
                    {
                        resourceSubPanel subPanel = new resourceSubPanel(i);
                        subPanel.Location = new Point(startingX, currentY);
                        this.Controls.Add(subPanel);
                        currentY += subPanel.Height + 5;
                    }
                }
            }
            Panel goalDetailPanel = this.Parent as Panel;
            mainForm MAINFORM = goalDetailPanel.Parent as mainForm;               //Gets to mainform level
            if (MAINFORM != null)
            {
                MAINFORM.changeGoalPanels(this);
            }
        }
        private void resourcePanel_VisibleChanged(object sender, EventArgs e)
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                Control currentControl = this.Controls[i];
                TextBox tb = currentControl as TextBox;
                if (tb != null)
                {
                    if (tb.Name == "numberResourceTypesTextBox")
                    {
                        tb.Text = "0";
                    }
                }
            }
        }
        private void intChangeTextBox(TextBox tb)
        {
            int igarbage;
            string name = getControlName(tb.Name, "TextBox", "Label");
            for (int i = tb.Parent.Controls.Count - 1; i >= 0; i--)
            {
                Control currentControl = tb.Parent.Controls[i];
                Label cL = currentControl as Label;
                if (cL != null)
                {
                    if (cL.Name == name)
                    {
                        if (int.TryParse(tb.Text, out igarbage))
                        {
                            cL.ForeColor = Color.Black;
                        }
                        else
                        {
                            cL.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }
        private string getControlName(string inputName, string fromControl, string toControl)
        {
            string name = inputName;
            int tnumber;
            if (int.TryParse(name.Substring(name.Length - 2), out tnumber))
            {
                name = name.Remove(name.Length - 2, 2);   //temporarily holds the number of the name
            }
            else if (int.TryParse(name.Substring(name.Length - 1), out tnumber))
            {
                name = name.Remove(name.Length - 1, 1);
            }
            name = name.Replace(fromControl, "");    //removes checkbox from the name
            name = name + toControl;
            return name;
        }
    }
}

