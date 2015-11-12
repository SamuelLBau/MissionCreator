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
    class InputTypes
    {
        public InputTypes(object sender, string[,] fieldArray, int i, ref Point checkBoxCurrentLocation, ref Point textBoxCurrentLocation, Size textBoxSize, int checkBoxIncrement)
        {
            Panel panel = sender as Panel;
            if (fieldArray[1, i] == "int")                   { drawInt(panel, fieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement); }
            else if (fieldArray[1, i] == "selection")        { drawSelection(panel, fieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement); }
            else if (fieldArray[1, i] == "TIME()")           { drawTIME(panel, fieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement); }
            else if (fieldArray[1, i] == "double")           { drawDouble(panel, fieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement); }
            else if (fieldArray[1, i] == "string")           { drawString(panel, fieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement); }
            else if (fieldArray[1, i] == "checkedListbox")   { drawCheckedListbox(panel, fieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement); }
            else if (fieldArray[1, i] == "partSubPanel")     { drawPartSubPanel(panel, fieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement); }
            else if (fieldArray[1, i] == "resourceSubPanel") { drawResourceSubPanel(panel, fieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement); }
            else if (fieldArray[1, i] == "N/A")              { return; }
            else if (fieldArray[1, i] == "bool")             { drawBool(panel, fieldArray, i, ref checkBoxCurrentLocation, ref textBoxCurrentLocation, textBoxSize, checkBoxIncrement); }
        }
        //----------------------BEGIN OBJECT CREATION-----------------------------
        private void drawSelection(       Panel sender, string[,] fieldArray, int i, ref Point checkBoxCurrentLocation, ref Point textBoxCurrentLocation, Size textBoxSize, int checkBoxIncrement)
        {
            {
                CheckBox cb = new CheckBox();
                cb.BackColor = Color.Transparent;
                cb.ForeColor = GV.inactiveButtonColor;
                cb.Text = fieldArray[3, i];
                cb.Location = checkBoxCurrentLocation;
                cb.Name = fieldArray[0, i] + "CheckBox";
                cb.Font = GV.generalFont;
                cb.AutoSize = true;
                sender.Controls.Add(cb);
                cb.CheckedChanged += new EventHandler(cbSelection_CheckedChanged);

                ComboBox CB = new ComboBox();
                CB.Location = textBoxCurrentLocation;
                CB.Name = fieldArray[0, i];
                CB.Size = textBoxSize;
                CB.Enabled = false;
                CB.Name = fieldArray[0, i];
                CB.AutoCompleteSource = AutoCompleteSource.ListItems;
                CB.AutoCompleteMode = AutoCompleteMode.Suggest;
                CB.SelectedIndexChanged +=new EventHandler(CB_SelectionChangeCommitted);
                sender.Controls.Add(CB);




                checkBoxCurrentLocation = new Point(checkBoxCurrentLocation.X, checkBoxCurrentLocation.Y + checkBoxIncrement);
                textBoxCurrentLocation = new Point(textBoxCurrentLocation.X, textBoxCurrentLocation.Y + checkBoxIncrement);
            }
        }
        private void drawTIME(            Panel sender, string[,] fieldArray, int i, ref Point checkBoxCurrentLocation, ref Point textBoxCurrentLocation, Size textBoxSize, int checkBoxIncrement)
        {
            {
                //-------------------------------------------BEGIN CHECKBOX CREATION---------------------------------------------
                CheckBox cb = new CheckBox();
                cb.BackColor = Color.Transparent;
                cb.ForeColor = GV.inactiveButtonColor;
                cb.Text = fieldArray[3, i];
                cb.Location = checkBoxCurrentLocation;
                cb.CheckedChanged += new EventHandler(cbTIME_CheckedChanged);
                cb.Name = fieldArray[0, i] + "TIMECheckBox";
                cb.Font = GV.generalFont;
                cb.AutoSize = true;
                sender.Controls.Add(cb);
                //--------------------------------------------END CHECKBOX CREATION------------------------------------------


                //-----------------------------------------------BEGIN LABELS------------------------------------------------

                Label yearsLabel = new Label();
                yearsLabel.Text = "Years";
                yearsLabel.ForeColor = GV.inactiveButtonColor;
                yearsLabel.AutoSize = true;
                yearsLabel.Font = GV.generalFont;
                yearsLabel.Location = new Point(checkBoxCurrentLocation.X + 18, checkBoxCurrentLocation.Y + 26);
                yearsLabel.Name = fieldArray[0, i] + "TIMEYearsTypeLabel";
                sender.Controls.Add(yearsLabel);

                Label daysLabel = new Label();
                daysLabel.Text = "Days";
                daysLabel.ForeColor = GV.inactiveButtonColor;
                daysLabel.AutoSize = true;
                daysLabel.Font = GV.generalFont;
                daysLabel.Location = new Point(checkBoxCurrentLocation.X + 87, checkBoxCurrentLocation.Y + 26);
                daysLabel.Name = fieldArray[0, i] + "TIMEDaysTypeLabel";
                sender.Controls.Add(daysLabel);

                Label hoursLabel = new Label();
                hoursLabel.Text = "Hours";
                hoursLabel.ForeColor = GV.inactiveButtonColor;
                hoursLabel.AutoSize = true;
                hoursLabel.Font = GV.generalFont;
                hoursLabel.Location = new Point(checkBoxCurrentLocation.X + 143, checkBoxCurrentLocation.Y + 26);
                hoursLabel.Name = fieldArray[0, i] + "TIMEHoursTypeLabel";
                sender.Controls.Add(hoursLabel);

                Label minutesLabel = new Label();
                minutesLabel.ForeColor = GV.inactiveButtonColor;
                minutesLabel.Text = "Min";
                minutesLabel.AutoSize = true;
                minutesLabel.Font = GV.generalFont;
                minutesLabel.Location = new Point(checkBoxCurrentLocation.X + 206, checkBoxCurrentLocation.Y + 26);
                minutesLabel.Name = fieldArray[0, i] + "TIMEMinutesTypeLabel";
                sender.Controls.Add(minutesLabel);

                Label secondsLabel = new Label();
                secondsLabel.Text = "Sec";
                secondsLabel.ForeColor = GV.inactiveButtonColor;
                secondsLabel.AutoSize = true;
                secondsLabel.Font = GV.generalFont;
                secondsLabel.Location = new Point(checkBoxCurrentLocation.X + 257, checkBoxCurrentLocation.Y + 26);
                secondsLabel.Name = fieldArray[0, i] + "TIMESecondsTypeLabel";
                sender.Controls.Add(secondsLabel);

                //-----------------------------------------------------END LABELS--------------------------------------------------

                //----------------------------------------------------BEGIN TEXTBOXES---------------------------------------------

                TextBox yearsTextBox = new TextBox();
                yearsTextBox.Size = new Size(18, 20);
                yearsTextBox.Text = "0";
                yearsTextBox.Location = new Point(checkBoxCurrentLocation.X, checkBoxCurrentLocation.Y + 23);
                yearsTextBox.Name = fieldArray[0, i] + "TIMEYears";
                yearsTextBox.Enabled = false;
                yearsTextBox.BackColor = Color.DarkGray;
                sender.Controls.Add(yearsTextBox);
                yearsTextBox.TextChanged += new EventHandler(tbTIME_TextChanged);

                TextBox daysTextBox = new TextBox();
                daysTextBox.Size = new Size(25, 20);
                daysTextBox.Text = "0";
                daysTextBox.Location = new Point(checkBoxCurrentLocation.X + 62, checkBoxCurrentLocation.Y + 23);
                daysTextBox.Name = fieldArray[0, i] + "TIMEDays";
                daysTextBox.Enabled = false;
                daysTextBox.BackColor = Color.DarkGray;
                sender.Controls.Add(daysTextBox);
                daysTextBox.TextChanged += new EventHandler(tbTIME_TextChanged);

                TextBox hoursTextBox = new TextBox();
                hoursTextBox.Size = new Size(18, 20);
                hoursTextBox.Text = "0";
                hoursTextBox.Location = new Point(checkBoxCurrentLocation.X + 125, checkBoxCurrentLocation.Y + 23);
                hoursTextBox.Name = fieldArray[0, i] + "TIMEHours";
                hoursTextBox.Enabled = false;
                hoursTextBox.BackColor = Color.DarkGray;
                sender.Controls.Add(hoursTextBox);
                hoursTextBox.TextChanged += new EventHandler(tbTIME_TextChanged);

                TextBox minutesTextBox = new TextBox();
                minutesTextBox.Size = new Size(18, 20);
                minutesTextBox.Text = "0";
                minutesTextBox.Location = new Point(checkBoxCurrentLocation.X + 188, checkBoxCurrentLocation.Y + 23);
                minutesTextBox.Name = fieldArray[0, i] + "TIMEMinutes";
                minutesTextBox.Enabled = false;
                minutesTextBox.BackColor = Color.DarkGray;
                sender.Controls.Add(minutesTextBox);
                minutesTextBox.TextChanged += new EventHandler(tbTIME_TextChanged);

                TextBox secondsTextBox = new TextBox();
                secondsTextBox.Size = new Size(18, 20);
                secondsTextBox.Text = "0";
                secondsTextBox.Location = new Point(checkBoxCurrentLocation.X + 239, checkBoxCurrentLocation.Y + 23);
                secondsTextBox.Name = fieldArray[0, i] + "TIMESeconds";
                secondsTextBox.Enabled = false;
                secondsTextBox.BackColor = Color.DarkGray;
                sender.Controls.Add(secondsTextBox);
                secondsTextBox.TextChanged += new EventHandler(tbTIME_TextChanged);


                checkBoxCurrentLocation = new Point(checkBoxCurrentLocation.X, checkBoxCurrentLocation.Y + 51);
                textBoxCurrentLocation = new Point(textBoxCurrentLocation.X, textBoxCurrentLocation.Y + 51);


            }
        }
        private void drawDouble(          Panel sender, string[,] fieldArray, int i, ref Point checkBoxCurrentLocation, ref Point textBoxCurrentLocation, Size textBoxSize, int checkBoxIncrement)
        {
            {
                //--------------------------------------- START CHECKBOX CREATION---------------------------------------------
                CheckBox cb = new CheckBox();
                cb.BackColor = Color.Transparent;
                cb.ForeColor = GV.inactiveButtonColor;
                cb.Text = fieldArray[3, i];
                cb.Location = checkBoxCurrentLocation;
                cb.CheckedChanged += new EventHandler(cbDouble_CheckedChanged);
                cb.Name = fieldArray[0, i] + "CheckBox";
                cb.Font = GV.generalFont;
                cb.AutoSize = true;
                sender.Controls.Add(cb);
                // ----------------------------------------END CHECKBOX CREATION--------------------------------------


                //----------------------------------------- START TEXTBOX CREATION--------------------------------------
                TextBox tb = new TextBox();
                tb.BackColor = Color.DarkGray;
                tb.Text = "0";
                tb.ForeColor = Color.Black;
                tb.Location = textBoxCurrentLocation;
                tb.Size = textBoxSize;
                tb.Enabled = false;
                tb.Name = fieldArray[0, i];
                tb.TextAlign = HorizontalAlignment.Right;
                sender.Controls.Add(tb);

                tb.TextChanged += new EventHandler(tbDouble_TextChanged);
                //-------------------------------------------END TEXTBOX CREATION----------------------------------------------

                //------------------------------------------BEGIN LABEL CREATION--------------------------------------
                Label lb = new Label();
                lb.Font = GV.generalFont;
                lb.Text = fieldArray[2, i];
                lb.Location = new Point(textBoxCurrentLocation.X + tb.Size.Width +5, checkBoxCurrentLocation.Y);
                lb.AutoSize = true;
                lb.Name = fieldArray[0, i] + "TypeLabel";
                sender.Controls.Add(lb);
                lb.ForeColor = GV.inactiveButtonColor;
                //--------------------------------------------END LABEL CREATION------------------------------------------
                checkBoxCurrentLocation = new Point(checkBoxCurrentLocation.X, checkBoxCurrentLocation.Y + checkBoxIncrement);
                textBoxCurrentLocation = new Point(textBoxCurrentLocation.X, textBoxCurrentLocation.Y + checkBoxIncrement);


            } //Ends drawDouble();
        }
        private void drawInt(             Panel sender, string[,] fieldArray, int i, ref Point checkBoxCurrentLocation, ref Point textBoxCurrentLocation, Size textBoxSize, int checkBoxIncrement)
        {
            {
                //--------------------------------------- START CHECKBOX CREATION---------------------------------------------
                CheckBox cb = new CheckBox();
                cb.BackColor = Color.Transparent;
                cb.ForeColor = GV.inactiveButtonColor;
                cb.Text = fieldArray[3, i];
                cb.Location = checkBoxCurrentLocation;
                cb.CheckedChanged += new EventHandler(cbDouble_CheckedChanged);
                cb.Name = fieldArray[0, i] + "CheckBox";
                cb.Font = GV.generalFont;
                cb.AutoSize = true;
                sender.Controls.Add(cb);
                // ----------------------------------------END CHECKBOX CREATION--------------------------------------


                //----------------------------------------- START TEXTBOX CREATION--------------------------------------
                TextBox tb = new TextBox();
                tb.BackColor = Color.DarkGray;
                tb.Text = "0";
                tb.ForeColor = Color.Black;
                tb.Location = new Point(textBoxCurrentLocation.X - 20, textBoxCurrentLocation.Y) ;
                tb.Size = new Size(textBoxSize.Width + 20, textBoxSize.Height);
                tb.Enabled = false;
                tb.Name = fieldArray[0, i];
                tb.TextAlign = HorizontalAlignment.Right;
                sender.Controls.Add(tb);

                tb.TextChanged += new EventHandler(tbInt_TextChanged);
                //-------------------------------------------END TEXTBOX CREATION----------------------------------------------

                //------------------------------------------BEGIN LABEL CREATION--------------------------------------
                Label lb = new Label();
                lb.Font = GV.generalFont;
                lb.Text = fieldArray[2, i];
                lb.Location = new Point(textBoxCurrentLocation.X + tb.Size.Width -20, checkBoxCurrentLocation.Y);
                lb.AutoSize = true;
                lb.ForeColor = GV.inactiveButtonColor;
                lb.Name = fieldArray[0, i] + "TypeLabel";
                sender.Controls.Add(lb);
                //--------------------------------------------END LABEL CREATION------------------------------------------
                checkBoxCurrentLocation = new Point(checkBoxCurrentLocation.X, checkBoxCurrentLocation.Y + checkBoxIncrement);
                textBoxCurrentLocation = new Point(textBoxCurrentLocation.X, textBoxCurrentLocation.Y + checkBoxIncrement);


            }
        }
        private void drawString(          Panel sender, string[,] fieldArray, int i, ref Point checkBoxCurrentLocation, ref Point textBoxCurrentLocation, Size textBoxSize, int checkBoxIncrement)
        {
            {
                //--------------------------------------- START CHECKBOX CREATION---------------------------------------------
                CheckBox cb = new CheckBox();
                cb.BackColor = Color.Transparent;
                cb.ForeColor = GV.inactiveButtonColor;
                cb.Text = fieldArray[3, i];
                cb.Location = checkBoxCurrentLocation;
                cb.CheckedChanged += new EventHandler(cbString_CheckedChanged);
                cb.Name = fieldArray[0, i] + "CheckBox";
                cb.Font = GV.generalFont;
                cb.AutoSize = true;
                sender.Controls.Add(cb);
                // ----------------------------------------END CHECKBOX CREATION--------------------------------------


                //----------------------------------------- START TEXTBOX CREATION--------------------------------------
                TextBox tb = new TextBox();
                tb.BackColor = Color.DarkGray;
                tb.Text = "";
                tb.ForeColor = Color.Black;
                tb.Location = textBoxCurrentLocation;
                tb.Size = textBoxSize;
                tb.Enabled = false;
                tb.Name = fieldArray[0, i];
                tb.TextAlign = HorizontalAlignment.Left;
                sender.Controls.Add(tb);

                tb.TextChanged += new EventHandler(tbString_TextChanged);
                //-------------------------------------------END TEXTBOX CREATION----------------------------------------------

                //------------------------------------------BEGIN LABEL CREATION--------------------------------------
                Label lb = new Label();
                lb.Font = GV.generalFont;
                lb.Text = fieldArray[2, i];
                lb.Location = new Point(textBoxCurrentLocation.X + tb.Size.Width + 5, checkBoxCurrentLocation.Y);
                lb.AutoSize = true;
                lb.ForeColor = GV.inactiveButtonColor;
                lb.Name = fieldArray[0, i] + "TypeLabel";
                sender.Controls.Add(lb);
                //--------------------------------------------END LABEL CREATION------------------------------------------
                checkBoxCurrentLocation = new Point(checkBoxCurrentLocation.X, checkBoxCurrentLocation.Y + checkBoxIncrement);
                textBoxCurrentLocation = new Point(textBoxCurrentLocation.X, textBoxCurrentLocation.Y + checkBoxIncrement);


            }
        }
        private void drawCheckedListbox ( Panel sender, string[,] fieldArray, int i, ref Point checkBoxCurrentLocation, ref Point textBoxCurrentLocation, Size textBoxSize, int checkBoxIncrement)
        {
            CheckBox cb = new CheckBox();
            cb.BackColor = Color.Transparent;
            cb.ForeColor = GV.inactiveButtonColor;
            cb.Text = fieldArray[3, i];
            cb.Location = checkBoxCurrentLocation;
            cb.Name = fieldArray[0, i] + "CheckBox";
            cb.Font = GV.generalFont;
            cb.AutoSize = true;
            sender.Controls.Add(cb);
            cb.CheckedChanged += new EventHandler(cbCheckedListBox_CheckedChanged);

            CheckedListBox clb = new CheckedListBox();
            clb.Location = new Point(textBoxCurrentLocation.X - 30, textBoxCurrentLocation.Y);
            clb.Size = new Size(113, 20);
            clb.Enabled = false;
            clb.Name = fieldArray[0, i];
            //clb.Items.AddRange(supplementList);           
            sender.Controls.Add(clb);
            clb.ItemCheck += new ItemCheckEventHandler(cbCheckedListbox_ItemCheck);
            clb.CheckOnClick = true;
            clb.MouseHover += new EventHandler(clb_MouseHover);
            clb.MouseLeave += new EventHandler(clb_MouseLeave);
            //clb.Anchor = AnchorStyles.Right;


            checkBoxCurrentLocation = new Point(checkBoxCurrentLocation.X, checkBoxCurrentLocation.Y + checkBoxIncrement);
            textBoxCurrentLocation = new Point(textBoxCurrentLocation.X, textBoxCurrentLocation.Y + checkBoxIncrement);


        }
        private void drawBool(            Panel sender, string[,] fieldArray, int i, ref Point checkBoxCurrentLocation, ref Point textBoxCurrentLocation, Size textBoxSize, int checkBoxIncrement)
        {
            CheckBox cb = new CheckBox();
            cb.BackColor = Color.Transparent;
            cb.ForeColor = GV.inactiveButtonColor;
            cb.Text = fieldArray[3, i];
            cb.Location = checkBoxCurrentLocation;
            cb.CheckedChanged += new EventHandler(cbBool_CheckedChanged);
            cb.Name = fieldArray[0, i] + "CheckBox";
            cb.Font = GV.generalFont;
            cb.AutoSize = true;
            sender.Controls.Add(cb);

            checkBoxCurrentLocation = new Point(checkBoxCurrentLocation.X, checkBoxCurrentLocation.Y + checkBoxIncrement);
            textBoxCurrentLocation = new Point(textBoxCurrentLocation.X, textBoxCurrentLocation.Y + checkBoxIncrement);
        }
        private void drawPartSubPanel(    Panel sender, string[,] fieldArray, int i, ref Point checkBoxCurrentLocation, ref Point textBoxCurrentLocation, Size textBoxSize, int checkBoxIncrement)
        {
            Label partTypesLabel = new System.Windows.Forms.Label();
            partTypesLabel.AutoSize = true;
            partTypesLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            partTypesLabel.Location = new System.Drawing.Point(3, 28);
            partTypesLabel.Name = "numberPartTypesPromptLabel";
            partTypesLabel.Size = new System.Drawing.Size(210, 24);
            partTypesLabel.TabIndex = 1;
            partTypesLabel.Text = "How many different types of parts\r\nshould be necessary for this goal?";
            sender.Controls.Add(partTypesLabel);

            TextBox partTypesTextBox = new System.Windows.Forms.TextBox();
            partTypesTextBox.Location = new System.Drawing.Point(219, 32);
            partTypesTextBox.Name = GV.numberPartTypesTBName;
            partTypesTextBox.Size = new System.Drawing.Size(23, 20);
            partTypesTextBox.TabIndex = 2;
            partTypesTextBox.Text = "0";
            partTypesTextBox.TextChanged += new EventHandler(numberPartTypes_TextChanged);
            sender.Controls.Add(partTypesTextBox);

            Label numberPartTypesLabel = new System.Windows.Forms.Label();
            numberPartTypesLabel.AutoSize = true;
            numberPartTypesLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            numberPartTypesLabel.Location = new System.Drawing.Point(250, 36);
            numberPartTypesLabel.Name = "numberPartTypesTypeLabel";
            numberPartTypesLabel.Size = new System.Drawing.Size(41, 12);
            numberPartTypesLabel.TabIndex = 3;
            numberPartTypesLabel.Text = "Types";                              //Note: If you change this, you must also change the change Label code in changeTypesLabel() below
            sender.Controls.Add(numberPartTypesLabel);
        }
        private void drawResourceSubPanel(Panel sender, string[,] fieldArray, int i, ref Point checkBoxCurrentLocation, ref Point textBoxCurrentLocation, Size textBoxSize, int checkBoxIncrement)
        {
            Label resourceTypesLabel = new System.Windows.Forms.Label();
            resourceTypesLabel.AutoSize = true;
            resourceTypesLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            resourceTypesLabel.Location = new System.Drawing.Point(3, 28);
            resourceTypesLabel.Name = "numberResourceTypesPromptLabel";
            resourceTypesLabel.Size = new System.Drawing.Size(210, 24);
            resourceTypesLabel.TabIndex = 1;
            resourceTypesLabel.Text = "How many different types of resources\r\nshould be necessary for this goal?";
            sender.Controls.Add(resourceTypesLabel);

            TextBox resourceTypesTextBox = new System.Windows.Forms.TextBox();
            resourceTypesTextBox.Location = new System.Drawing.Point(240, 32);
            resourceTypesTextBox.Name = GV.numberResourceTypesTBName;
            resourceTypesTextBox.Size = new System.Drawing.Size(23, 20);
            resourceTypesTextBox.TabIndex = 2;
            resourceTypesTextBox.Text = "0";
            resourceTypesTextBox.TextChanged += new EventHandler(numberResourceTypes_TextChanged);
            sender.Controls.Add(resourceTypesTextBox);

            Label numberResourceTypesLabel = new System.Windows.Forms.Label();
            numberResourceTypesLabel.AutoSize = true;
            numberResourceTypesLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 6.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            numberResourceTypesLabel.Location = new System.Drawing.Point(265, 36);
            numberResourceTypesLabel.Name = "numberResourceTypesTypeLabel";
            numberResourceTypesLabel.Size = new System.Drawing.Size(41, 12);
            numberResourceTypesLabel.TabIndex = 3;
            numberResourceTypesLabel.Text = "Types";                              //Note: If you change this, you must also change the change Label code in changeTypesLabel() below
            sender.Controls.Add(numberResourceTypesLabel);
        }


        //-----------------------BEGIN CHECKBOX CALLS---------------------------------------------
        private void cbBool_CheckedChanged(object sender, EventArgs e)
        {
            mainForm MF = (mainForm)Application.OpenForms[0];
            CheckBox cb = sender as CheckBox;
            changeBoxColor(cb);
            if (cb.Checked == true)
            {
                editParameter(sender, cb.Name.Replace("CheckBox", ""), "true");
            }
            else
            {
                editParameter(sender, cb.Name.Replace("CheckBox", ""), GV.defaultValue);
            }
        }
        private void cbDouble_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            changeBoxColor(cb);


            string name = getControlName(cb.Name, "CheckBox", "");  //returns the name of the matching TextBox control
            TextBox tb = (TextBox)cb.Parent.Controls[name];

            if (cb.Checked == true)
            {
                tb.BackColor = Color.White;
                tb.Enabled = true;
            }
            else
            {
                tb.BackColor = Color.DarkGray;
                tb.Enabled = false;
                tb.Text = "0";
                editParameter(sender, name, GV.defaultValue);
            }

        }   //changes box color/changes matching textbox (used for double and Int)
        private void cbTIME_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            changeBoxColor(cb);
            string timeLengthName = "";

            for (int timeCount = 0; timeCount < 5; timeCount++)
            {
                switch (timeCount)
                {
                    case 0:
                        timeLengthName = "Years";
                        break;
                    case 1:
                        timeLengthName = "Days";
                        break;
                    case 2:
                        timeLengthName = "Hours";
                        break;
                    case 3:
                        timeLengthName = "Minutes";
                        break;
                    case 4:
                        timeLengthName = "Seconds";
                        break;
                    default:
                        timeLengthName = "";
                        break;
                }
                string name = getControlName(cb.Name, "CheckBox", timeLengthName);  //returns the name of the matching TextBox control
                TextBox tb = (TextBox)cb.Parent.Controls[name];
                if (cb.Checked == true)
                {
                    tb.BackColor = Color.White;
                    tb.Enabled = true;
                }
                else
                {
                    tb.BackColor = Color.DarkGray;
                    tb.Enabled = false;
                    tb.Text = "0";
                    editParameter(sender, cb.Name.Replace("TIMECheckBox", ""), GV.defaultValue);
                }
            }

        }    //changes box color and changes all matching textBoxes
        private void cbString_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            changeBoxColor(cb);


            string name = getControlName(cb.Name, "CheckBox", "");  //returns the name of the matching TextBox control
            TextBox tb = (TextBox)cb.Parent.Controls[name];

            if (cb.Checked == true)
            {
                tb.BackColor = Color.White;
                tb.Enabled = true;
            }
            else
            {
                tb.BackColor = Color.DarkGray;
                tb.Enabled = false;
                tb.Text = "0";
                editParameter(sender, name, GV.defaultValue);
            }
        }//changes box color/changes matching textbox (used for double, int and string)
        private void cbSelection_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            changeBoxColor(cb);

            string name = getControlName(cb.Name, "CheckBox", "");  //returns the name of the matching TextBox control
            ComboBox CB = (ComboBox)cb.Parent.Controls[name];
            if (cb.Checked == true)
            {
                CB.Enabled = true;
                editParameter(sender, CB.Name, CB.SelectedItem.ToString());
            }
            else
            {
                CB.SelectedItem = "Kerbin";
                CB.Enabled = false;
                editParameter(sender, CB.Name, GV.defaultValue);
            }

        }
        private void cbCheckedListBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            changeBoxColor(cb);


            string name = getControlName(cb.Name, "CheckBox", "");  //returns the name of the matching Control
            CheckedListBox CB = (CheckedListBox)cb.Parent.Controls[name];

            if (cb.Checked == true)
            {
                CB.Enabled = true;
            }
            else
            {
                foreach (int i in CB.CheckedIndices)
                {
                    CB.SetItemCheckState(i, CheckState.Unchecked);
                }
                CB.Enabled = false;
                editParameter(sender, CB.Name, GV.defaultValue);
            }

        }

        //-----------------------------BEGIN VALUE CHANGES------------------------------------
        private void tbInt_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            int igarbage;
            string name =  tb.Name.Replace("TextBox","") + "TypeLabel";
            Label cL = (Label)tb.Parent.Controls[name];
            if (int.TryParse(tb.Text, out igarbage))
            {
                cL.ForeColor = GV.inactiveButtonColor;
                editParameter(sender, tb.Name, tb.Text);
            }
            else
            {
                cL.ForeColor = GV.invalidButtonColor;
                editParameter(sender, tb.Name, GV.defaultValue);
            }
        }
        private void tbDouble_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            double dgarbage;
            string name = tb.Name + "TypeLabel";
            Label cL = (Label)tb.Parent.Controls[name];
            if (double.TryParse(tb.Text, out dgarbage))
            {
                cL.ForeColor = GV.inactiveButtonColor;
                editParameter(sender, tb.Name, tb.Text);
            }
            else
            {
                cL.ForeColor = GV.invalidButtonColor;
                editParameter(sender, tb.Name, GV.defaultValue);
            }

        }
        private void tbTIME_TextChanged(object sender, EventArgs e)
        {
            mainForm MF = (mainForm)Application.OpenForms[0];
            TextBox tb = sender as TextBox;
            int igarbage;
            string name = tb.Name + "TypeLabel";
            string type = tb.Name;

            if (type.EndsWith("Years")) { type = type.Remove(type.Length - 5); }
            else if (type.EndsWith("Days")) { type = type.Remove(type.Length - 4); }
            else if (type.EndsWith("Hours")) { type = type.Remove(type.Length - 5); }
            else if (type.EndsWith("Minutes")) { type = type.Remove(type.Length - 7); }
            else if (type.EndsWith("Seconds")) { type = type.Remove(type.Length - 7); }

            Label cL = (Label)tb.Parent.Controls[name];


            //Used to check that a textbox is an integer, turns designated label red if it is not, black if it is a double or Red if it is not
            if (int.TryParse(tb.Text, out igarbage))
            {
                cL.ForeColor = GV.inactiveButtonColor; ;

                editParameter(sender, type.Replace("TIME", ""), TIMEstring(type, (Panel)tb.Parent));
            }
            else
            {
                cL.ForeColor = GV.invalidButtonColor;
                editParameter(sender,type.Replace("TIME", ""), GV.defaultValue);
            }

        }
        private void tbString_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            editParameter(sender, tb.Name, tb.Text);
        }
        private void CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox CB = (ComboBox)sender;
            if (Application.OpenForms.Count != 0)
            {
                mainForm MF = (mainForm)Application.OpenForms[0];
                editParameter(sender, CB.Name, CB.SelectedItem.ToString());
            }
        }
        private void cbCheckedListbox_ItemCheck(     object sender, ItemCheckEventArgs e)
        {
            string newValue = "";
            CheckedListBox CLB = (CheckedListBox)sender;

            foreach (string item in CLB.CheckedItems) { newValue += item + ", "; }
            if (e.NewValue == CheckState.Checked)
            {
                newValue += CLB.Items[e.Index].ToString() + ", ";
            }
            else
            {
                newValue = newValue.Replace(CLB.Items[e.Index].ToString(), "");
                newValue = newValue.Replace(", ,", ",");
            }
            if (newValue.Length > 0) { newValue = newValue.Remove(newValue.Length-2); }
            if (newValue == "") { newValue = GV.defaultValue; }
            editParameter(CLB, CLB.Name, newValue);

        }
        private void numberPartTypes_TextChanged(object sender, EventArgs e)
        {
            mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];              //Gets to mainform level

            int initialY = 60;
            int startingX = 5;
            int maxNumberTypes = GV.maxNumberSubPanels;

            int currentY = initialY;
            int number;
            TextBox TB = sender as TextBox;
            Panel partsPanel = (Panel)TB.Parent;
            tbInt_TextChanged(sender, e);
            if (TB != null)
            {
                if (int.TryParse(TB.Text, out number))                       //Checks if input is a valid integer
                {
                    for (int i = partsPanel.Controls.Count - 1; i >= 0; i--)   //This loop removes all panels, note it is independant of the integer check below
                    {
                        Control currentControl = partsPanel.Controls[i];
                        partsSubPanel panel = currentControl as partsSubPanel;
                        if (panel != null)
                        {
                            partsPanel.Controls.Remove(panel);
                        }
                    }
                    if (number > maxNumberTypes) { number = maxNumberTypes; TB.Text = maxNumberTypes.ToString(); }   //If input number is to big, it is changed to the maximum number
                    if (number == 0) 
                    { 
                        MF.missionPackage.editMissionGoal("partName", GV.defaultValue, "Part");
                        MF.missionPackage.editMissionGoal("partCount", GV.defaultValue, "Part");
                        MF.missionPackage.editMissionGoal("maxPartCount", GV.defaultValue, "Part");
                    }
                    for (int i = 0; i < number; i++)                       //Places the proper amount of panels
                    {
                        partsSubPanel subPanel = new partsSubPanel(i);
                        subPanel.Location = new Point(startingX, currentY);
                        partsPanel.Controls.Add(subPanel);
                        currentY += subPanel.Height + 5;
                    }
                    Panel goalDetailPanel = partsPanel.Parent as Panel;
                    if (MF != null)
                    {
                        MF.changeGoalPanels(this);
                    }
                    MF.missionPackage.updateValue(partsPanel, "partSubPanel", "", "", number); 

                }
            }

        }
        private void numberResourceTypes_TextChanged(object sender, EventArgs e)
        {
            mainForm MF = (mainForm)Application.OpenForms[GV.mainFormNumber];              //Gets to mainform level

            int initialY = 60;
            int startingX = 5;
            int maxNumberTypes = GV.maxNumberSubPanels;

            int currentY = initialY;
            int number;
            TextBox TB = sender as TextBox;
            Panel resourcePanel = (Panel)TB.Parent;
            tbInt_TextChanged(sender, e);
            if (TB != null)
            {
                if (int.TryParse(TB.Text, out number))                       //Checks if input is a valid integer
                {
                    for (int i = resourcePanel.Controls.Count - 1; i >= 0; i--)   //This loop removes all panels, note it is independant of the integer check below
                    {
                        Control currentControl = resourcePanel.Controls[i];
                        resourceSubPanel panel = currentControl as resourceSubPanel;
                        if (panel != null)
                        {
                            resourcePanel.Controls.Remove(panel);
                        }
                    }
                    if (number > maxNumberTypes) { number = maxNumberTypes; TB.Text = maxNumberTypes.ToString(); }   //If input number is to big, it is changed to the maximum number
                    if (number == 0)
                    {
                        MF.missionPackage.editMissionGoal("name", GV.defaultValue, "Resource");
                        MF.missionPackage.editMissionGoal("minAmount", GV.defaultValue, "Resource");
                        MF.missionPackage.editMissionGoal("maxAmount", GV.defaultValue, "Resource");
                    }
                    for (int i = 0; i < number; i++)                       //Places the proper amount of panels
                    {
                        resourceSubPanel subPanel = new resourceSubPanel(i);
                        subPanel.Location = new Point(startingX, currentY);
                        resourcePanel.Controls.Add(subPanel);
                        currentY += subPanel.Height + 5;
                    }
                    Panel goalDetailPanel = resourcePanel.Parent as Panel;
                    if (MF != null)
                    {
                        MF.changeGoalPanels(this);
                    }
                    MF.missionPackage.updateValue(resourcePanel, "resourceSubPanel", "", "", number);

                }
            }
        }

        //-------------------------------SUPPLEMENTARY METHODS--------------------------------------------
        private void clb_MouseHover(                 object sender, EventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            if (clb != null && clb.Enabled == true)
            {
                clb.Size = new Size(clb.Size.Width, GV.CLBItemHeight * clb.Items.Count+1);
            }
        }
        private void clb_MouseLeave(                 object sender, EventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            if (clb != null)
            {
                clb.Size = new Size(clb.Size.Width, 20);
            }
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
                    if (button.Checked == true) { button.ForeColor = GV.activeButtonColor; }
                    else { button.ForeColor = GV.inactiveButtonColor; }
                }
            }
            else if (box != null)
            {
                if (box.Checked == true) { box.ForeColor = GV.activeButtonColor; }
                else { box.ForeColor = GV.inactiveButtonColor; }
            }
        }
        private string getControlName(string inputName, string fromControl, string toControl)
        {
            string name = inputName;
            name = name.Replace(fromControl, "");    //removes checkbox from the name
            name = name + toControl;
            return name;
        }
        private string TIMEstring(string type, Panel parent)
        {

            //allows appropriate TIME boxes to be found
            string returnString = "TIME(";

            //This segment only does the removal of the end timeValue

            //Creates TIME string
            returnString += parent.Controls[type + "Years"].Text + "y ";
            returnString += parent.Controls[type + "Days"].Text + "d ";
            returnString += parent.Controls[type + "Hours"].Text + "h ";
            returnString += parent.Controls[type + "Minutes"].Text + "m ";
            returnString += parent.Controls[type + "Seconds"].Text + "s ";
            returnString += ')';

            return returnString;
        }
        private void editParameter(object sender, string key, string newValue)
        {
            mainForm MF = (mainForm)Application.OpenForms[0];
            Control control = (Control)sender;
            if (control.Parent.Name == GV.missionInfoPanelName)     { MF.missionPackage.editMission(key, newValue); }
            else if (control.Parent.Name == GV.goalDetailPanelName) { MF.missionPackage.editMissionAllGoal(key, newValue); }
            else                                               
            {
                string name = control.Parent.Name.Replace("Panel", "");
                MF.missionPackage.editMissionGoal(key, newValue, name); 
            }
        }
    }
}
