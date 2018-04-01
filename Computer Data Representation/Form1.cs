using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Numerics;

namespace Computer_Data_Representation {
    public partial class Form1 : Form {
        string saveMerk = null;
        string saveSubb = null;
        Button[] myBtn = new Button[32];
        static byte notation = 2;
        static byte type = 0;
        static byte sizeOfType = 1;
        static bool validInput = true;
        static BigInteger numberInDec = 0;
        static Decimal numberInDecReal = 0;
        static bool sign = false;
        static string binaryForm = "00000000";
        static int merkulovPosition = 0;
        static int merkulovAmount = 0;
        static bool merkulovSolveButtonF1 = false;
        static bool merkulovSolveButtonF2 = false;
        static int subbotinPosition = 0;
        static int subbotinAmount = 0;
        static bool subbotinGoodInput = false; 
        static ColorDialog MeDialog;
        static Color evencolor = Color.FromArgb (192, 255, 192);
        static Color oddcolor = Color.FromArgb (255, 128, 128);
        static Color chosencolor = Color.Yellow;

        public Form1 () {
            InitializeComponent ();
            this.KeyPreview = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler (this.Form1_KeyUp);
            MeDialog = new ColorDialog();
            button1.BackColor = chosencolor;
            button2.BackColor = evencolor;
            button4.BackColor = oddcolor;
            //CommonDialog.ShowDialog (colorDialog2);
            Test ();
        }

        private void ButtonClick (object sender, EventArgs e) {
            Button b = sender as Button;
            int floatE = 8;
            b.Text = b.Text[0] == '0' ? "1" : "0";
            string s = "";

            for (int i = 0;i < 32;i++) {
                s += myBtn[i].Text;
            }
            int E = Convert.ToInt32 (s.Substring (1, floatE), 2);
            E = E - 127;
            float F = 0;
            string m = "1" + s.Substring (floatE + 1);
            for (int i = 0;i < m.Length;i++)
                F += m[i] == '1' ? (float) Math.Pow (2, -i + E) : 0;
            if (s[0] == '1')
                F = -F;

            else
                label6.Text = F.ToString ();
        }
        private void Test () {

            for (int i = 0;i < 32;i++) {
                //this.SolveB.Click += new System.EventHandler(this.SolveB_Click);

                myBtn[i] = new Button ();
                myBtn[i].Text = "0";
                myBtn[i].TextAlign = ContentAlignment.MiddleCenter;
                myBtn[i].Size = new Size (25, 25);
                myBtn[i].Font = new Font ("Microsoft Sans Serif", 10); //Microsoft Sans Serif; 12pt"
                if (i < 1) {
                    myBtn[i].BackColor = System.Drawing.Color.FromArgb (128, 255, 255);
                    flowLayoutPanel3.Controls.Add (myBtn[i]);

                }
                else
                    if (i < 9) {
                    myBtn[i].BackColor = System.Drawing.Color.FromArgb (192, 255, 192);
                    flowLayoutPanel4.Controls.Add (myBtn[i]);
                }
                else {
                    myBtn[i].BackColor = System.Drawing.Color.FromArgb (255, 128, 128);
                    flowLayoutPanel5.Controls.Add (myBtn[i]);
                }
                myBtn[i].Click += new System.EventHandler (ButtonClick);
            }
        }
        private void Form1_KeyUp (object sender, KeyEventArgs e) {
            // if (e.KeyCode == Keys.Enter)
            //  SolveB.PerformClick ();
        }

        private string borders (byte type, int notation) {
            BigInteger lowerBorder;
            string s;
            BigInteger upperBorder = 0;
            for (int i = 0;i < sizeOfType * 8;i++)
                upperBorder += (BigInteger) (Math.Pow (2, i));

            if ((type > 6) || (type == 0)) {
                lowerBorder = 0;
                s = "от " + lowerBorder.ToString () + Environment.NewLine + "                                                               до " + FromDecToAny (upperBorder, notation);
            }
            else {
                if ((type == 4) || (type == 5)) {
                    s = null;
                }
                else {
                    lowerBorder = (upperBorder / 2 + 1);
                    upperBorder = upperBorder / 2;
                    s = "от " + '-' + FromDecToAny (lowerBorder, notation) + Environment.NewLine + "                                                               до " + FromDecToAny (upperBorder, notation);
                }
            }

            return s;
        }


        private string FromDecToAny (BigInteger input, int notation) {
            string output = "";
            do {

                if (notation <= 10) {
                    output = output + (input % notation).ToString ();
                    input = input / notation;
                }
                else {
                    if ((input % notation) >= 10) {
                        char symbol = (char) ((input % notation) + 55);

                        output = output + symbol;
                    }
                    else
                        output = output + (input % notation).ToString ();
                    input = input / notation;
                }
            }
            while ((input) >= notation);
            if (input < 10)
                output = output + input;
            else {
                char symbol = (char) ((input % notation) + 55);
                output = output + symbol;
            }

            return new string (output.ToCharArray ().Reverse ().ToArray ());

        }

        private void TypeCB_SelectedIndexChanged (object sender, EventArgs e) {
            string def = "Диапазон допустимых значений: ";
            type = (byte) TypeCB.SelectedIndex;
            switch (type) {
                case 0: {
                    sizeOfType = sizeof (byte);
                }
                break;
                case 1: {
                    sizeOfType = sizeof (short);
                }
                break;
                case 2: {
                    sizeOfType = sizeof (int);
                }
                break;
                case 3: {
                    sizeOfType = sizeof (long);
                }
                break;
                case 4: {
                    sizeOfType = sizeof (double);
                }
                break;
                case 5: {
                    sizeOfType = sizeof (float);
                }
                break;
                case 6: {
                    sizeOfType = sizeof (sbyte);
                }
                break;
                case 7: {
                    sizeOfType = sizeof (uint);
                }
                break;
                case 8: {
                    sizeOfType = sizeof (ushort);
                }
                break;
                case 9: {
                    sizeOfType = sizeof (ulong);
                }
                break;

            }
            buttonIntoDec.Enabled = false;
            buttonIntoDec2.Enabled = false;
            RangeL.Text = def + borders (type, notation);
            labelAboutType.Text = "О типе: длина " + sizeOfType + " байт(a)";
            CheckInput ();
        }

        private void numericUpDown1_ValueChanged (object sender, EventArgs e) {
            string def = "Алфавит: ";
            notation = (byte) NotationNUD.Value;
            string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring (0, notation);
            string a = "";
            for (int i = 0;i < notation;i++)
                a += (allowedChars[i] + " ");
            AlphabetL.Text = def + a;
            RangeL.Text = "Диапазон допустимых значений: " + borders (type, notation);
            CheckInput ();
        }

        private void NumberTB_TextChanged (object sender, EventArgs e) {
            CheckInput ();
            labelAboutNumber.Text = "О числе: длина введеной строки " + NumberTB.Text.Length + " символов";
        }

        private void CheckInput () {
            string input = NumberTB.Text;
            input = input.Replace (" ", string.Empty);
            NumberTB.Text = input;
            string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring (0, notation);
            byte dotCounter = 0;
            if (type >= 0 && type < 4 || type >= 6) //целые
                validInput = Regex.IsMatch (input, string.Format ("^-?[{0}]+$", allowedChars), RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (type == 4 || type == 5) //вещественные
                validInput = Regex.IsMatch (input, string.Format ("^-?[{0}]+(?:\\.[{0}]+|[{0}]*)$", allowedChars), RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!validInput) {
                if (input.Length > 0 && input[0] == '.') {
                    string error = "Первым символом не может быть точка. Программа удалит ее.";
                    MessageBox.Show (error, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    input = input.Remove (0, 1);
                    NumberTB.Text = input;
                    NumberTB.SelectionStart = NumberTB.Text.Length;
                }
                if (input.Length == 1 && input[0] == '-') {

                }
                else {
                    int errorChar = -1;
                    dotCounter = 0;
                    for (int i = 0;i < input.Length;i++) {
                        int j;
                        if (input[i] == '.')
                            dotCounter++;
                        for (j = 0;j < allowedChars.Length;j++)
                            if (input[i] == allowedChars[j])
                                break;
                        if (j == allowedChars.Length)
                            errorChar = i;
                    }
                    if (errorChar > -1) {
                        if (input[errorChar] == '.' && (type == 4 || type == 5)) {
                            if (dotCounter != 1) {
                                string error = "Введено 2 точки. Программа удалит последнюю из них.";
                                MessageBox.Show (error, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                input = input.Remove (errorChar, 1);
                                NumberTB.Text = input;
                                NumberTB.SelectionStart = NumberTB.Text.Length;
                            }
                        }
                        else {
                            if (input.Length > 0) {
                                string error = "Символ '" + input[errorChar] + "' не подходит для ввода. Программа удалит символ.";
                                MessageBox.Show (error, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                input = input.Remove (errorChar, 1);
                                NumberTB.Text = input;
                                NumberTB.SelectionStart = NumberTB.Text.Length;
                            }
                        }
                    }
                }
            }
            SolveB.Enabled = validInput;
        }

        private void SolveB_Click (object sender, EventArgs e) {
            numberInDec = 0;
            if (type >= 0 && type < 4 || type >= 6) //целые
            {
                IntegerToDecimal ();
                IntegerBinaryRepresentation ();
                groupBox1.Visible = false;
            }

            if (type == 4 || type == 5) //вещественные   
            {
                RealToDecimal ();
                RealBinaryRepresentation ();
                groupBox1.Visible = true;
            }
            //flowLayoutPanel5.Controls.Add (myBtn[i]);
            //Лр2 Меркулов
            string number = binaryForm.Replace (" ", "");
            merkulovAmount = 0;
            merkulovPosition = 0;
            textBox1.Text = "";
            textBox2.Text = "";
            buttonMerkulov.Enabled = true;
            RepresentBinaryFormForLab2Merkulov (number);
            buttonIntoDec.Enabled = false;
            //Лр2 Субботин
            subbotinAmount = 0;
            subbotinPosition = 0;
            textBoxSubbotinUpper.Text = "";
            textBoxSubbotinAmount.Text = "";
            subbotinGoodInput = false;
            buttonIntoDec2.Enabled = false;
            buttonSubbotin.Enabled = true;

            flowLayoutMerkulovAnswer.Controls.Clear ();
            flowLayoutSubbotinAnswer.Controls.Clear ();

        }
        Label[] bit = new Label[8];
        Label[] bit2 = new Label[8];
        private void RepresentBinaryFormForLab2Merkulov (string represent) {
            bool green = false;
            flowLayoutMerkulovBinaryForm.Controls.Clear ();
            flowLayoutSubbotinBinary.Controls.Clear ();

            bit = new Label[represent.Length];
            bit2 = new Label[represent.Length];
            for (int i = 0;i < represent.Length;i++) {
                if (i % 8 == 0 && i != 0 && i != 32)
                    green = green ? false : true;

                bit[i] = new Label ();
                bit2[i] = new Label ();
                bit[i].TextAlign = ContentAlignment.MiddleCenter;
                bit2[i].TextAlign = ContentAlignment.MiddleCenter;
                bit[i].Size = new Size (25, 40);
                bit2[i].Size = new Size (25, 40);
                //bit[i].Font = new Font ("Microsoft Sans Serif", 10); //Microsoft Sans Serif; 12pt"
                bit[i].Text = represent.Length - i - 1 + Environment.NewLine + represent[i];
                bit2[i].Text = represent.Length - i - 1 + Environment.NewLine + represent[i];

                flowLayoutMerkulovBinaryForm.Controls.Add (bit[i]);
                flowLayoutSubbotinBinary.Controls.Add (bit2[i]);
                if (green) {
                    bit[i].BackColor = evencolor;//System.Drawing.Color.FromArgb (190, 245, 116);
                    bit2[i].BackColor = evencolor;//System.Drawing.Color.FromArgb (190, 245, 116);
                }
                else {
                    bit[i].BackColor = oddcolor;//System.Drawing.Color.FromArgb (238, 220, 130);
                    bit2[i].BackColor = oddcolor;//System.Drawing.Color.FromArgb (238, 220, 130);
                }
            }

        }
        void RealToDecimal () {
            string input = NumberTB.Text;
            sign = NumberTB.Text[0] == '-' ? true : false;
            if (sign) {
                input = input.Remove (0, 1);
            }
            numberInDecReal = 0;

            bool overflowDecimal = false;
            int dotPosition = input.IndexOf ('.');
            if (dotPosition > -1)
                input = input.Remove (dotPosition, 1);

            for (int i = 0;i < input.Length;i++) {
                try {
                    numberInDecReal += (Decimal) Math.Pow (notation, input.Length - i - 1 - (dotPosition == -1 ? 0 : input.Length - dotPosition)) * (input[i] - '0' > 10 ? input[i] - 'A' + 10 : input[i] - '0');

                }
                catch {
                    overflowDecimal = true;

                }
            }
            numberInDecReal = sign ? -numberInDecReal : numberInDecReal;
            if (overflowDecimal)
                MessageBox.Show ("Переполнение типа", "Переполнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            textBoxDecimaForm.Text = numberInDecReal.ToString ();
        }

        void AboutRepresent (string s) {
            int floatE = 8, doubleE = 11;
            s = s.Replace (" ", string.Empty);
            switch (type) {

                case 4: {
                    labelSign.Text = "Знак" + Environment.NewLine + s[0];
                    labelExponent.Text = "Порядок" + Environment.NewLine + s.Substring (1, doubleE);
                    labelMantissa.Text = "Мантисса" + Environment.NewLine + s.Substring (doubleE + 1);

                    int E = Convert.ToInt32 (s.Substring (1, doubleE), 2);
                    E = E - 1023; //
                    double F = 0;
                    string m = "1" + s.Substring (doubleE + 1);
                    for (int i = 0;i < m.Length;i++)
                        F += m[i] == '1' ? Math.Pow (2, -i + E) : 0;
                    if (s[0] == '1')
                        F = -F;
                    else
                        textBoxToDecimal.Text = F.ToString ();


                }
                break;
                case 5: {
                    labelSign.Text = "Знак" + Environment.NewLine + s[0];
                    labelExponent.Text = "Порядок" + Environment.NewLine + s.Substring (1, floatE);
                    labelMantissa.Text = "Мантисса" + Environment.NewLine + s.Substring (floatE + 1);
                    int E = Convert.ToInt32 (s.Substring (1, floatE), 2);
                    E = E - 127;
                    float F = 0;
                    string m = "1" + s.Substring (floatE + 1);
                    for (int i = 0;i < m.Length;i++)
                        F += m[i] == '1' ? (float) Math.Pow (2, -i + E) : 0;
                    if (s[0] == '1')
                        F = -F;

                    else
                        textBoxToDecimal.Text = F.ToString ();
                }
                break;
            }
        }
        void RealBinaryRepresentation () {
            unsafe
            {
                if (type == 4) {
                    double a;
                    long* b;


                    a = (double) numberInDecReal;

                    b = (long*) &a; //присваиваем двоичный код переменной a переменной b
                    string s = "";
                    for (int i = sizeOfType * 8 - 1;i >= 0;i--) {
                        if (((i + 1) % 8 == 0) && (i != sizeOfType * 8 - 1))
                            s += ' ';
                        s += (((*b >> i) & 1));

                    }
                    binaryForm = s;
                    textBoxBinaryWType.Text = s;
                    AboutRepresent (s);
                }

                if (type == 5) {
                    float a;
                    int* b;

                    a = (float) numberInDecReal;

                    b = (int*) &a;
                    string s = "";
                    for (int i = sizeOfType * 8 - 1;i >= 0;i--) {
                        if (((i + 1) % 8 == 0) && (i != sizeOfType * 8 - 1))
                            s += ' ';
                        s += (((*b >> i) & 1));

                    }
                    binaryForm = s;
                    textBoxBinaryWType.Text = s;
                    AboutRepresent (s);
                }
            }
        }
        void IntegerToDecimal () {
            sign = NumberTB.Text[0] == '-' ? true : false;
            numberInDec = 0;
            string input = NumberTB.Text;
            bool overflowInt64 = false;
            if (sign) {
                input = input.Remove (0, 1);
            }
            if (notation == 10) {
                try {
                    numberInDec = Int64.Parse (input);
                }
                catch {
                    overflowInt64 = true;
                }
            }
            else {
                if (notation == 2 || notation == 8 || notation == 16) {
                    try {
                        numberInDec = Convert.ToInt64 (input, notation);
                    }
                    catch {
                        overflowInt64 = true;
                    }
                }
                else {

                    for (int i = 0;i < input.Length;i++)
                        numberInDec += (BigInteger) Math.Pow (notation, input.Length - i - 1) * (input[i] - '0' > 10 ? input[i] - 'A' + 10 : input[i] - '0');
                }
            }

            if (overflowInt64 || input.Length == 64) {
                numberInDec = 0;
                for (int i = 0;i < input.Length;i++)
                    numberInDec += (BigInteger) Math.Pow (notation, input.Length - i - 1) * (input[i] - '0' > 10 ? input[i] - 'A' + 10 : input[i] - '0');
            }
            numberInDec = sign ? -numberInDec : numberInDec;
            textBoxDecimaForm.Text = numberInDec.ToString ();
        }

        void IntegerBinaryRepresentation () {
            switch (type) {
                case 0: {
                    try {
                        byte result1 = (byte) numberInDec;
                    }
                    catch {
                        MessageBox.Show ("Переполнение типа", "Переполнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                break;
                case 1: {
                    try {
                        short result1 = (short) numberInDec;
                    }
                    catch {
                        MessageBox.Show ("Переполнение типа", "Переполнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                break;
                case 2: {
                    try {
                        int result1 = (int) numberInDec;
                    }
                    catch {
                        MessageBox.Show ("Переполнение типа", "Переполнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                break;
                case 3: {
                    try {
                        long result1 = (long) numberInDec;
                    }
                    catch {
                        MessageBox.Show ("Переполнение типа", "Переполнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                break;

                case 16: {
                    try {
                        char result1 = (char) numberInDec;
                    }
                    catch {
                        MessageBox.Show ("Переполнение типа", "Переполнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                break;
                case 6: {
                    try {
                        sbyte result1 = (sbyte) numberInDec;
                    }
                    catch {
                        MessageBox.Show ("Переполнение типа", "Переполнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                break;
                case 7: {
                    try {
                        uint result1 = (uint) numberInDec;
                    }
                    catch {
                        MessageBox.Show ("Переполнение типа", "Переполнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                break;

                case 8: {
                    try {
                        ushort result1 = (ushort) numberInDec;
                    }
                    catch {
                        MessageBox.Show ("Переполнение типа", "Переполнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                break;
                case 9: {
                    try {
                        ulong result1 = (ulong) numberInDec;
                    }
                    catch {
                        MessageBox.Show ("Переполнение типа", "Переполнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                break;

            }

            string s = "";

            for (int i = sizeOfType * 8 - 1;i >= 0;i--) {
                if (((i + 1) % 8 == 0) && (i != sizeOfType * 8 - 1))
                    s += ' ';
                s += (((numberInDec >> i) & 1));
            }
            string s2 = s.Replace (" ", string.Empty);
            switch (type) {
                case 0: {
                    byte result = Convert.ToByte (s2, 2);
                    textBoxToDecimal.Text = result.ToString ();
                }
                break;
                case 1: {
                    short result = Convert.ToInt16 (s2, 2);
                    textBoxToDecimal.Text = result.ToString ();
                }
                break;
                case 2: {
                    int result = Convert.ToInt32 (s2, 2);
                    textBoxToDecimal.Text = result.ToString ();
                }
                break;
                case 3: {
                    long result = Convert.ToInt64 (s2, 2);
                    textBoxToDecimal.Text = result.ToString ();
                }
                break;
                case 6: {
                    sbyte result = Convert.ToSByte (s2, 2);
                    textBoxToDecimal.Text = result.ToString ();
                }
                break;
                case 7: {
                    uint result = Convert.ToUInt32 (s2, 2);
                    textBoxToDecimal.Text = result.ToString ();
                }
                break;
                case 8: {
                    ushort result = Convert.ToUInt16 (s2, 2);
                    textBoxToDecimal.Text = result.ToString ();
                }
                break;
                case 9: {
                    ulong result = Convert.ToUInt64 (s2, 2);
                    textBoxToDecimal.Text = result.ToString ();
                }
                break;
            }
            binaryForm = s;
            textBoxBinaryWType.Text = s;

        }

        private void RangeL_Click (object sender, EventArgs e) {
            Label label = sender as Label;

            if (label != null) {
                Clipboard.SetText (label.Text, TextDataFormat.UnicodeText);
            }
        }

        private void label6_Click (object sender, EventArgs e) {

        }

        private void textBoxToDecimal_TextChanged (object sender, EventArgs e) {

        }

        private void tabPage2_Click (object sender, EventArgs e) {

        }

        private void label7_Click (object sender, EventArgs e) {

        }

        private void label9_Click (object sender, EventArgs e) {

        }
        private void textBox1_TextChanged (object sender, EventArgs e) {
            CheckAmountAndPositionMerkulov ();
        }
        private void textBox2_TextChanged (object sender, EventArgs e) {
            CheckAmountAndPositionMerkulov ();
        }
        private void Paint () {
            string number = binaryForm.Replace (" ", "");
            RepresentBinaryFormForLab2Merkulov (new string (binaryForm.Replace (" ", "").ToCharArray ()));
            for (int i = 0;i < merkulovAmount;i++)
                try {

                    bit[number.Length - 1 - merkulovPosition - i].BackColor = chosencolor;//System.Drawing.Color.FromArgb (128, 255, 255);
                }
                catch {
                    MessageBox.Show ("Выход за пределы бинарного представления числа");
                    break;
                }
        }

        private void CheckAmountAndPositionMerkulov () {
            string number = binaryForm.Replace (" ", "");
            int inputPosition = 0;
            int inputAmount = 0;
            try {
                inputPosition = Int32.Parse (textBox1.Text);
            }
            catch {
                if (textBox1.Text == "") {
                    inputPosition = 0;
                }
                else
                    MessageBox.Show ("Позиция - Вы должны ввести число в диапазоне от 0 до " + (number.Length - 1));
            }
            try {
                inputAmount = Int32.Parse (textBox2.Text);
            }
            catch {
                if (textBox2.Text == "") {
                    inputAmount = 0;
                }
                else
                    MessageBox.Show ("Количество - Вы должны ввести число в диапазоне от 0 до " + (number.Length - merkulovPosition));
            }

            if (inputPosition < number.Length && inputPosition >= 0) {
                merkulovSolveButtonF1 = true;
                merkulovPosition = inputPosition;
            }
            else {
                merkulovSolveButtonF1 = false;
                MessageBox.Show ("Позиция - Вы должны ввести число в диапазоне от 0 до " + (number.Length - 1));
            }
            if ((inputAmount + inputPosition <= number.Length) && inputAmount >= 0) {
                merkulovSolveButtonF2 = true;
                merkulovAmount = inputAmount;
            }
            else {
                merkulovSolveButtonF2 = false;
                MessageBox.Show ("Количество - Вы должны ввести число в диапазоне от 0 до " + (number.Length - merkulovPosition));

            }

            if (merkulovSolveButtonF1 && merkulovSolveButtonF2) {
                buttonMerkulov.Enabled = true;
                buttonIntoDec.Enabled = true;
                Paint ();
            }
            else
                buttonMerkulov.Enabled = false;
        }

        private string MerkulovAnswer () {
            string input = binaryForm.Replace (" ", "");
            int pos = merkulovPosition;
            int amo = merkulovAmount;
            pos = input.Length - pos;
            pos -= amo;

            string FirstHalf = input.Substring (0, pos);
            string SecondHalf = input.Substring (pos + amo, input.Length - pos - amo);
            char[] save = input.Substring (pos, amo).ToCharArray ();

            Array.Reverse (save);
            string Medium = new string (save);
            return (FirstHalf + Medium + SecondHalf);
        }



        private void buttonMerkulov_Click (object sender, EventArgs e) {
            textBox1_TextChanged (textBox1, e);
            textBox2_TextChanged (textBox2, e);
            string answer = MerkulovAnswer ();
            saveMerk = answer;

            bool green = false;
            Label[] bit = new Label[answer.Length];
            flowLayoutMerkulovAnswer.Controls.Clear ();
            bit = new Label[answer.Length];
            for (int i = 0;i < answer.Length;i++) {
                if (i % 8 == 0 && i != 0 && i != 32)
                    green = green ? false : true;

                bit[i] = new Label ();
                bit[i].TextAlign = ContentAlignment.MiddleCenter;
                bit[i].Size = new Size (25, 40);
                bit[i].Text = answer.Length - i - 1 + Environment.NewLine + answer[i];
                bit[i].TextAlign = ContentAlignment.MiddleCenter;
                flowLayoutMerkulovAnswer.Controls.Add (bit[i]);
                if (green)
                    bit[i].BackColor = evencolor;
                else
                    bit[i].BackColor = oddcolor;
            }

            for (int i = 0;i < merkulovAmount;i++)
                try {

                    bit[answer.Length - 1 - merkulovPosition - i].BackColor = chosencolor;
                }
                catch {
                    MessageBox.Show ("Выход за пределы бинарного представления числа");
                    break;
                }
        }

        private void textBoxSubbotinUpper_TextChanged (object sender, EventArgs e) {
            //CheckAmountAndPositionSubbotin ();
        }

        private void textBoxSubbotinAmount_TextChanged (object sender, EventArgs e) {
            //CheckAmountAndPositionSubbotin ();
        }



        private void buttonSubbotin_Click (object sender, EventArgs e) {
            int inputAmount = 0;
            int inputUpper = 0;
            string number = binaryForm.Replace (" ", "");
            try {
                inputUpper = Int32.Parse (textBoxSubbotinUpper.Text);
                if (inputUpper > (number.Length - 1) || inputUpper < 0) {
                    MessageBox.Show ("Номер старшего бита должен быть в пределах от 0 до " + (number.Length - 1));
                    subbotinGoodInput = false;
                }
                else {
                    try {
                        inputAmount = Int32.Parse (textBoxSubbotinAmount.Text);
                        if (inputAmount < 0 || inputAmount > inputUpper + 1) {
                            MessageBox.Show ("Количество бит должно быть в пределах от 0 до " + (inputUpper + 1));
                            subbotinGoodInput = false;
                        }
                        else {
                            subbotinAmount = inputAmount;
                            subbotinPosition = inputUpper;
                            subbotinGoodInput = true;
                        }
                    }
                    catch {
                        MessageBox.Show ("Количество бит должно быть в пределах от 0 до " + (inputUpper + 1));
                        subbotinGoodInput = false;
                    }
                }
            }
            catch {
                MessageBox.Show ("Номер старшего бита должен быть в пределах от 0 до " + (number.Length - 1));
                subbotinGoodInput = false;
            }

            if (subbotinGoodInput) {
                saveSubb = number;
                buttonIntoDec2.Enabled = true;
                RepresentBinaryFormForLab2Merkulov (number);
                for (int i = 0;i < subbotinAmount;i++)
                    try {

                        bit2[number.Length - subbotinPosition + i - 1].BackColor = chosencolor;
                    }
                    catch {
                        MessageBox.Show ("Выход за пределы бинарного представления числа");
                        break;
                    }

                //решение
                string EditablePart = number.Substring (number.Length - subbotinPosition - 1, subbotinAmount);
                char[] saved = EditablePart.ToCharArray ();
                Array.Sort (saved);
                
                EditablePart = new string (saved);

                string answer = number.Substring (0, number.Length - subbotinPosition - 1) + EditablePart + number.Substring (number.Length - subbotinPosition - 1 + subbotinAmount);
                saveSubb = answer;
                Label[] bit = new Label[answer.Length];
                flowLayoutSubbotinAnswer.Controls.Clear ();
                bool green = false;
                bit = new Label[answer.Length];
                for (int i = 0;i < answer.Length;i++) {
                    if (i % 8 == 0 && i != 0 && i != 32)
                        green = green ? false : true;

                    bit[i] = new Label ();
                    bit[i].TextAlign = ContentAlignment.MiddleCenter;
                    bit[i].Size = new Size (25, 40);
             
                    bit[i].Text = answer.Length - i - 1 + Environment.NewLine + answer[i];
                    bit[i].TextAlign = ContentAlignment.MiddleCenter;
                    flowLayoutSubbotinAnswer.Controls.Add (bit[i]);
                    if (green)
                        bit[i].BackColor = evencolor;
                    else
                        bit[i].BackColor = oddcolor;
                }

                for (int i = 0;i < subbotinAmount;i++)
                    try {

                        bit[number.Length - subbotinPosition + i - 1].BackColor = chosencolor;
                    }
                    catch {
                        MessageBox.Show ("Выход за пределы бинарного представления числа");
                        break;
                    }
            }
        }

        private void label13_Click (object sender, EventArgs e) {

        }

        private void buttonIntoDec_Click (object sender, EventArgs e) {
            lab2ToDec (saveMerk);
        }

        private void buttonIntoDec2_Click (object sender, EventArgs e) {
            lab2ToDec (saveSubb);
        }

        private void lab2ToDec (string save) {
            switch (type) {
                case 0: {

                    byte result1 = Convert.ToByte (save, 2);
                    MessageBox.Show (result1.ToString (), "Число в десятичной системе счисления");
                }
                break;
                case 1: {

                    short result1 = Convert.ToInt16 (save, 2);
                    MessageBox.Show (result1.ToString (), "Число в десятичной системе счисления");

                }
                break;
                case 2: {
                    int result1 = Convert.ToInt32 (save, 2);
                    MessageBox.Show (result1.ToString (), "Число в десятичной системе счисления");
                }
                break;
                case 3: {
                    long result1 = Convert.ToInt64 (save, 2);
                    MessageBox.Show (result1.ToString (), "Число в десятичной системе счисления");
                }
                break;
                case 4: {

                    if (save.Replace ("1", "").Length == 64) {
                        MessageBox.Show ("0", "Число в десятичной системе счисления");
                    }
                    else {
                        int E = Convert.ToInt32 (save.Substring (1, 11), 2);
                        E = E - 1023; //
                        double F = 0;
                        string m = "1" + save.Substring (12);
                        for (int i = 0;i < m.Length;i++)
                            F += m[i] == '1' ? Math.Pow (2, -i + E) : 0;
                        if (save[0] == '1')
                            F = -F;
                        MessageBox.Show (F.ToString (), "Число в десятичной системе счисления");
                    }
                }
                break;

                case 5: {

                    if (save.Replace ("1", "").Length == 32) {
                        MessageBox.Show ("0", "Число в десятичной системе счисления");
                    }
                    else {
                        int E = Convert.ToInt32 (save.Substring (1, 8), 2);
                        E = E - 127;
                        double F = 0;
                        string m = "1" + save.Substring (9);
                        for (int i = 0;i < m.Length;i++)
                            F += m[i] == '1' ? Math.Pow (2, -i + E) : 0;
                        if (save[0] == '1')
                            F = -F;
                        MessageBox.Show (F.ToString (), "Число в десятичной системе счисления");
                    }
                }
                break;
                case 6: {
                    sbyte result1 = Convert.ToSByte (save, 2);
                    MessageBox.Show (result1.ToString (), "Число в десятичной системе счисления");
                }
                break;
                case 7: {
                    uint result1 = Convert.ToUInt32 (save, 2);
                    MessageBox.Show (result1.ToString (), "Число в десятичной системе счисления");
                }
                break;

                case 8: {
                    ushort result1 = Convert.ToUInt16 (save, 2);
                    MessageBox.Show (result1.ToString (), "Число в десятичной системе счисления");
                }
                break;
                case 9: {
                    ulong result1 = Convert.ToUInt64 (save, 2);
                    MessageBox.Show (result1.ToString (), "Число в десятичной системе счисления");
                }
                break;
            }
        }

        private void Form1_Load (object sender, EventArgs e) {

        }

        private void ButtonChooseOddColor_Click (object sender, EventArgs e) {
            if (MeDialog.ShowDialog () == DialogResult.OK) {
                chosencolor = MeDialog.Color;
                button1.BackColor = chosencolor;
                //button1.ForeColor = color;
            }
                
        }

        private void button1_Click (object sender, EventArgs e) {            
        }

        private void ButtonChooseEvenColor_Click (object sender, EventArgs e) {
            if (MeDialog.ShowDialog () == DialogResult.OK) {
                evencolor = MeDialog.Color;
                button2.BackColor = evencolor;
                //button1.ForeColor = color;
            }
        }

        private void ButtonChooseChosenColor_Click (object sender, EventArgs e) {
            if (MeDialog.ShowDialog () == DialogResult.OK) {
                oddcolor = MeDialog.Color;
                button4.BackColor = oddcolor;
                //button1.ForeColor = color;
            }
        }
    }
}

