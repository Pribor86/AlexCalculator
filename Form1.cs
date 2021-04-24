using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace AlexCalculator
{
    public partial class MainForm : Form
    {
        const int x_begin = 10, y_begin = 45;

        Label text; //!!!

        bool flag_text = false; // флаг новой цепочки вычисления!

        double Num = 0;
        char LastOperation = '+';

        char separ = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];

        public MainForm()
        {
            InitializeComponent();
            try
            {
                this.Icon = new Icon(this.GetType(), "calculator.ico");
            }
            catch { }
            this.Text = "AlexCalculator";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.ClientSize = new Size(100, 150);

            InitCalclabel();
            InitCalcButton();

            Scale(new SizeF(Font.Height / 4f, Font.Height / 4f));
        }

        void InitCalclabel()
        {

            text = new Label();
            text.Parent = this;
            text.TextAlign = ContentAlignment.TopRight;
            text.Font = new Font("Arial", 18, FontStyle.Bold);
            text.Location = new Point(x_begin, y_begin / 2);
            text.Size = new Size(ClientSize.Width - 2 * x_begin, Font.Height);

            text.Text = "0";

        }

        void InitCalcButton()
        {
            string[,] nameButton =
            {
                {"C", " ", "<=","/" },
                {"7", "8", "9", "*"},
                {"4","5","6","-" },
                {"1","2","3","+" },
                {"+/-", "0", separ.ToString(), "=" }
            };

            int w = 20, h = 20;
            int dx, dy;
            dx = w + 1 / 4 * w;
            dy = h + 1 / 4 * h;

            CalcButton btn;


            for (int row = 0, y = y_begin; row <= nameButton.GetUpperBound(0); row++, y += dy)
            {
                for (int col = 0, x = x_begin; col <= nameButton.GetUpperBound(1); col++, x += dx)
                {
                    //if (row == 0 && col == 1)
                    //{
                    //    continue;
                    //}
                    btn = new CalcButton(this, nameButton[row, col], x, y, w, h);
                    
                    if (col > 2)
                    {
                        btn.BackColor = SystemColors.ControlLightLight;
                    }
                    btn.Click += Btn_Click;
                }
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            CalcButton btn = (CalcButton)sender;

            if (btn.Text[0] >= '0' && btn.Text[0] <= '9')
            {
                if (flag_text == false)
                {
                    if (btn.Text != "0")
                    {
                        text.Text = "";
                        flag_text = true;
                    }
                    else
                    {
                        text.Text = "";
                    }
                }
                text.Text += btn.Text;
            }
            else if(btn.Text == "+/-")
            {
                if (flag_text)
                {
                    if (text.Text[0] != '-')
                    {
                        text.Text = text.Text.Insert(0, "-");
                    }
                    else
                    {
                        text.Text = text.Text.Remove(0, 1);
                    }
                }
            }
            else if (btn.Text == "C")
            {
                text.Text = "0";
                flag_text = false;
            }
            else if (btn.Text[0] == '+' || btn.Text[0] == '-' || btn.Text[0] == '*' || btn.Text[0] == '/')
            {
                Num = Double.Parse(text.Text);
                this.LastOperation = btn.Text[0];

                flag_text = false;
            }
            else if (btn.Text[0] == '=')
            {
                bool flag_error = false;
                switch (LastOperation)
                {
                    case '+':
                        Num += Double.Parse(text.Text);
                        break;
                    case '-':
                        Num -= Double.Parse(text.Text);
                        break;
                    case '*':
                        Num *= Double.Parse(text.Text);
                        break;
                    case '/':
                        try
                        {
                            if (Double.Parse(text.Text) != 0)
                            {
                                Num /= Double.Parse(text.Text);
                            }
                            else
                            {
                                flag_error = true;
                            }
                        }
                        catch { flag_error = true; }
                        break;
                }
                if (flag_error == true)
                {
                    text.Text = String.Format("Error {0}/0", Num);
                }
                else
                {
                    text.Text = Num.ToString();
                    flag_text = false;
                }
                //??:

                //Num = 0;
                //LastOperation = '+';
            }
            else
            {
                switch(btn.Text[0])
                {
                    case '.':
                        if(text.Text.IndexOf('.') == -1)
                        {
                            text.Text += btn.Text;
                            if(flag_text == false)
                            {
                                flag_text = true;
                            }
                        }

                        break;
                    case '<':
                        if(text.Text.Length > 0)
                        {
                            text.Text = text.Text.Remove(text.Text.Length - 1, 1);
                        }
                        if(text.Text.Length == 0)
                        {
                            text.Text = "0";
                            flag_text = false;
                        }
                        break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void aboutCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.Text + "\x00A9 2021 by Alex.", "About" + this.Text);
            //AboutDialog dlg = new AboutDialog();
            //dlg.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = this.BackColor;


            if (dlg.ShowDialog() == DialogResult.OK)
            {

                this.BackColor = dlg.Color;

            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            //this.Text += e.KeyChar.ToString();

            char chKey = Char.ToUpper(e.KeyChar);
            if(chKey == 0x000D)// Enter{
            {
                chKey = '=';
            }
            else if(chKey == 0x008)//Backspace <=
            {
                chKey = '<';
            }
            else if(chKey == 0x001B)//ESC
            {
                chKey = 'C';
            }
            else if(chKey == '.' || chKey == ',')
            {
                chKey = separ;
            }
            if((chKey >= '0' && chKey <= '9') || 
                   ( chKey == '+' || chKey == '-' || chKey == '*' || chKey == '/' ||
                    chKey == 'C' || chKey == '<' || chKey == '=' || chKey == separ))
                    {
                for (int i = 0; i < Controls.Count; i++)
                {
                    if(Controls[i] is CalcButton)
                    {
                        CalcButton btn = (CalcButton)Controls[i];
                        if(chKey == btn.Text[0])
                        {
                            InvokeOnClick(btn, EventArgs.Empty);
                            break;
                        }
                    }
                }
            }
        }
    }
}
