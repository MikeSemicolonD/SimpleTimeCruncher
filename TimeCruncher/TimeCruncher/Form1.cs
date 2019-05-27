using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Something I made just to streamline the process of counting my hours for work.
/// </summary>

namespace TimeCruncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = "------";
        }

        private string originalText;

        /// <summary>
        /// Whenever text is added/deleted from the text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalTime();
        }

        /// <summary>
        /// If the 'Disregard Total Days' box is checked, recalculate to get the new total
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox1_Click(object sender, EventArgs e)
        {
            CalculateTotalTime();
        }

        /// <summary>
        /// Updates the UI on the window with results, using data inputted by the user
        /// </summary>
        private void CalculateTotalTime()
        {
            //recalculate if no original
            if (originalText != textBox1.Text)
            {
                originalText = textBox1.Text.ToLower();

                if (textBox1.Text.Length >= 2)
                {
                    string result;

                    try
                    {
                        result = CaculateTime(originalText);
                    }
                    catch (Exception)
                    {
                        result = "ERROR";
                    }

                    label1.Text = result;
                }
                else
                {
                    label1.Text = "------";
                }
            }
        }

        /// <summary>
        /// Creates a proper string format of the total amount of time spent.
        /// Ex: Input: "d4 h3 m2 s1" or "s356521"
        ///     Output: "4 Day(s) 3 Hour(s) 2 Minute(s) 1 Second(s)"
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string CaculateTime(string text)
        {
            int minutes = 0, hours = 0, days = 0, seconds = 0;

            FindCoupledVariables(ref seconds, ref minutes, ref hours, ref days);

            minutes += Multiple(seconds, 60);
            seconds %= 60;
            hours += Multiple(minutes, 60);
            minutes %= 60;

            if (!checkBox1.Checked)
            {
                days += Multiple(hours, 24);
                hours %= 24;
            }

            return ((days != 0) ? days.ToString() + " Day(s) " : "") +
                ((hours != 0) ? " " + hours.ToString() + " Hour(s) " : "") +
                ((minutes != 0) ? " " + minutes.ToString() + " Minute(s) " : "") +
                ((seconds != 0) ? " " + seconds.ToString() + " Second(s) " : "");
        }

        /// <summary>
        /// Returns a value representing the amount of times the dividend goes into the amount
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="dividend"></param>
        /// <returns></returns>
        private int Multiple(int amount, int dividend)
        {
            int count = 0;

            while (amount > 0)
            {
                amount -= dividend;

                if (amount >= 0)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Returns the values associated with the targetChar (direct right of the char)
        /// </summary>
        private void FindCoupledVariables(ref int seconds, ref int minutes, ref int hours, ref int days)
        {
            bool numbersFirst = false;
            List<Marker> charMap = new List<Marker>();

            //Parse for all of the chars needed ('h','m','s','d')
            for (int i = 0; i < originalText.Length; i++)
            {
                if (originalText[i] == 'h' || originalText[i] == 'm' || originalText[i] == 's' || originalText[i] == 'd')
                {
                    charMap.Add(new Marker(originalText[i], i));
                }
                else
                {
                    //While checking to see what order they're in
                    if (charMap.Count == 0 && originalText[i] != ' ')
                    {
                        numbersFirst = true;
                    }
                }
            }

            //Start parsing out the data depending on the order it was entered in
            if (numbersFirst)
            {
                //Substringing towards the left ("3h 2m")
                int leftIndex = 0;
                for (int i = 0; i < charMap.Count; i++)
                {
                    switch (charMap[i].Key)
                    {
                        case 's':
                            seconds += Convert.ToInt32(originalText.Substring(leftIndex, (charMap[i].MarkerIndex - leftIndex)));
                            leftIndex = charMap[i].MarkerIndex + 1;
                            break;
                        case 'm':
                            minutes += Convert.ToInt32(originalText.Substring(leftIndex, (charMap[i].MarkerIndex - leftIndex)));
                            leftIndex = charMap[i].MarkerIndex + 1;
                            break;
                        case 'h':
                            hours += Convert.ToInt32(originalText.Substring(leftIndex, (charMap[i].MarkerIndex - leftIndex)));
                            leftIndex = charMap[i].MarkerIndex + 1;
                            break;
                        case 'd':
                            days += Convert.ToInt32(originalText.Substring(leftIndex, (charMap[i].MarkerIndex - leftIndex)));
                            leftIndex = charMap[i].MarkerIndex + 1;
                            break;
                    }
                }
            }
            else
            {
                //Substringing towards the right ("h13 m3")
                for ( int i = 0; i < charMap.Count; i++)
                {
                    if ((charMap[i].MarkerIndex + 1) != originalText.Length)
                    {
                        int substringLength = ((i+1) == charMap.Count) ? (originalText.Length - (charMap[i].MarkerIndex+1)) : (charMap[i + 1].MarkerIndex - (charMap[i].MarkerIndex + 1));
                        switch (charMap[i].Key)
                        {
                            case 's':
                                seconds += Convert.ToInt32(originalText.Substring((charMap[i].MarkerIndex + 1), substringLength));
                                break;
                            case 'm':
                                minutes += Convert.ToInt32(originalText.Substring((charMap[i].MarkerIndex + 1), substringLength));
                                break;
                            case 'h':
                                hours += Convert.ToInt32(originalText.Substring((charMap[i].MarkerIndex + 1), substringLength));
                                break;
                            case 'd':
                                days += Convert.ToInt32(originalText.Substring((charMap[i].MarkerIndex + 1), substringLength));
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This associates a character with an index for parsing purposes 
        /// </summary>
        class Marker
        {
            public Marker(char newKey, int newMarkerIndex)
            {
                Key = newKey;
                MarkerIndex = newMarkerIndex;
            }

            public char Key { get; set; }
            public int MarkerIndex { get; set; }
        }
    }
}
