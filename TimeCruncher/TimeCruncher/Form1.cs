using System;

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
            if (originalText != textBox1.Text)
            {
                originalText = textBox1.Text.ToLower();

                //recalculate
                if (textBox1.Text.Length != 0)
                {
                    label1.Text = CaculateTime(originalText);
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
            int minutes = 0, hours = 0, days = 0, seconds;

            seconds = FindCoupledVariables('s');
            minutes = FindCoupledVariables('m') + Multiple(seconds,60);
            seconds %= 60;
            hours = FindCoupledVariables('h') + Multiple(minutes,60);
            minutes %= 60;
            days = FindCoupledVariables('d') + Multiple(hours, 24);
            hours %= 24;

            return ((days != 0) ? days.ToString()+" Day(s) " : "")+
                ((hours != 0) ? " "+hours.ToString()+" Hour(s) " : "")+
                ((minutes != 0) ? " "+minutes.ToString()+" Minute(s) " : "")+
                ((seconds != 0) ? " "+seconds.ToString()+" Second(s) " : "");
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

                if (amount > 0)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Returns the values associated with the targetChar (direct right of the char)
        /// </summary>
        /// <param name="targetChar"></param>
        /// <returns></returns>
        private int FindCoupledVariables(char targetChar)
        {
            int total = 0;
            int startIndex = -1, currentIndex = 0;

            while(currentIndex < originalText.Length)
            {
                if (startIndex == -1)
                {
                    //if we hit the target char
                    if (originalText[currentIndex] == targetChar)
                    {
                        //Remeber where it was
                        startIndex = currentIndex;
                    }
                }
                else
                {
                    //If we've found a letter
                    if ((currentIndex == originalText.Length-1 && ((int)originalText[currentIndex] >= 97 && (int)originalText[currentIndex] <= 122) || (int)originalText[currentIndex] == 32) || (((int)originalText[currentIndex] >= 97 && (int)originalText[currentIndex] <= 122) || (int)originalText[currentIndex] == 3))
                    {
                        total += Convert.ToInt32(originalText.Substring(startIndex+1,(currentIndex-1-startIndex)));
                        startIndex = -1;
                    }
                }

                currentIndex++;
            }

            return total;
        }
    }
}
