using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MoradzadeHelperUtilityLibrary;
using HtmlAgilityPack;

namespace UnHope
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void KeyPress_JustNumberAbs(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))) e.Handled = true;
        }
        private void KeyPress_JustDecimalNumber(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || (e.KeyChar == '.' && !textBox.Text.Contains('.')) || (e.KeyChar == '-' && !textBox.Text.Contains('-')))) e.Handled = true;
            else if (e.KeyChar == '.' && textBox.Text == "")
            {
                textBox.Text = "0.";
                textBox.SelectionStart = textBox.TextLength;
                e.Handled = true;
            }
        }
        private void KeyPress_JustDecimalNumberAbs(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || (e.KeyChar == '.' && !textBox.Text.Contains('.')))) e.Handled = true;
            else if (e.KeyChar == '.' && textBox.Text == "")
            {
                textBox.Text = "0.";
                textBox.SelectionStart = textBox.TextLength;
                e.Handled = true;
            }
        }

        #region Tab1
        private void result_TextChanged1(object sender, EventArgs e)
        {
            if (breakLoop) return;
            if (b_textBox1.Text != "")
            {
                breakLoop = true;
                length.Text = (!FastCode.IsNumber(length.Text)) ? "25" : length.Text;
                breakLoop = false;
                int a, b = int.Parse(b_textBox1.Text), r = ((sign_label1.Text == "+") ? +1 : -1) * ((r_textBox1.Text == "") ? 0 : int.Parse(r_textBox1.Text)), n = 0;
                result_textBox1.Clear();
                List<string> s = new List<string>();

                for (int i = 0; n <= int.Parse(length.Text) - 1; i++)
                {
                    if (!equal_label.Text.Contains("≠"))
                    {
                        a = b * i + r;
                        s.Add(a.ToString());
                        n++;
                    }
                    else
                    {
                        for (int j = 0; j <= b - 1 && n <= int.Parse(length.Text) - 1; j++)
                        {
                            if (j != r && j != (r + b) % b)
                            {
                                a = i * b + j;
                                s.Add(a.ToString());
                                n++;
                            }
                        }
                    }
                }
                result_textBox1.Text = string.Join(", ", s);
            }
            else result_textBox1.Clear();
        }
        private void equal_label_Click(object sender, EventArgs e)
        {
            equal_label.Text = (equal_label.Text == "a =") ? "a ≠" : "a =";
            result_TextChanged1(sender, e);
        }

        private void sign_label1_Click(object sender, EventArgs e)
        {
            sign_label1.Text = (sign_label1.Text == "+") ? "-" : "+";
            result_TextChanged1(sender, e);
        }
        #endregion

        #region Tab2
        private void result_TextChanged2(object sender, EventArgs e)
        {
            if (b_textBox2.Text != "" && b_textBox2.Text != "0" && min_textBox2.Text != "" && max_textBox2.Text != "" && double.Parse(min_textBox2.Text) < double.Parse(max_textBox2.Text))
            {
                int b = int.Parse(b_textBox2.Text), r = ((sign_label2.Text == "+") ? +1 : -1) * ((r_textBox2.Text == "") ? 0 : int.Parse(r_textBox2.Text));
                double min = (double.Parse(min_textBox2.Text) - r) / b, max = (double.Parse(max_textBox2.Text) - r) / b;
                max = (max == (int)max && max_label.Text == "<") ? max - 1 : Math.Floor(max);
                min = (min == (int)min && min_label.Text == "<") ? min + 1 : Math.Ceiling(min);
                label9.Text = $"K's values is from {min} to {max}";
                result_label.Text = (max - min + 1).ToString();
            }
        }
        private void sign_label2_Click(object sender, EventArgs e)
        {
            sign_label2.Text = (sign_label2.Text == "+") ? "-" : "+";
            result_TextChanged2(sender, e);
        }
        private void min_label_Click(object sender, EventArgs e)
        {
            min_label.Text = (min_label.Text == "<=") ? "<" : "<=";
            result_TextChanged2(sender, e);
        }
        private void max_label_Click(object sender, EventArgs e)
        {
            max_label.Text = (max_label.Text == "<=") ? "<" : "<=";
            result_TextChanged2(sender, e);
        }
        #endregion

        #region Tab3
        private void result_TextChanged3(object sender, EventArgs e)
        {
            try
            {
                float a = (ax2.Text == "") ? 0 : float.Parse(ax2.Text), b = (bx.Text == "") ? 0 : float.Parse(bx.Text), c = (this.c.Text == "") ? 0 : float.Parse(this.c.Text);
                label7.ResetText();
                label8.ResetText();
                if (a != 0)
                {
                    double d = Math.Pow(b, 2) - 4 * a * c;
                    label7.Text += (d < 0) ? "Quadratic equation doesn't have any roots!" : $"x = ({-b}+√{d})/{2 * a} = {Math.Round((-b + Math.Sqrt(d)) / (2 * a),3)}\n      ({-b}-√{d})/{2 * a} = {Math.Round((-b - Math.Sqrt(d)) / (2 * a),3)}";
                    label8.Location = new Point(label7.Location.X + label7.Size.Width + 6, 42);
                    label8.Text = (d < 0) ? $"Δ = √({d})" : $"Δ = √({Math.Pow(b, 2)}-({4 * a * c})) = {Math.Round(Math.Sqrt(d),3)}";
                }
                else if (a == 0 && b != 0)
                {
                    label7.Text += $"x = {-c}/{b} = {-c / b}";
                }
                else if (c == 0)
                {
                    label7.Text += "There is no x!(it's not a equation)";
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Tab4
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    textBox2.Text = FastCode.RomanNumeral(int.Parse(textBox1.Text));
                    textBox4.Text = string.Join(", ", FastCode.GetFibonacci(0, ushort.Parse(textBox1.Text)));
                    textBox3.Text = string.Join(", ", FastCode.GetPrime(0, ushort.Parse(textBox1.Text)));
                }
                else
                {
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                }
            }
            catch (OverflowException)
            {
                textBox3.Text = "Prime number must be ushort!\r\nOverFlowError";
            }
        }
        #endregion

        #region Tab5
        private void gram_TextChanged(object sender, EventArgs e)
        {
            if (mass.Text != "" && goldPrice.Text != "" && fee.Text != "")
            {
                try
                {
                    double mass = double.Parse(this.mass.Text), goldPrice = double.Parse(this.goldPrice.Text.Replace(",", "")), fee = double.Parse(this.fee.Text.Replace(",", "").Replace("%", "")) / 100, benefit = double.Parse(this.benefit.Text.Replace("%", "")) / 100, tax = double.Parse(this.tax.Text.Replace("%", "")) / 100, a, purePriceofGold = mass * goldPrice, percentage;
                    priceOfGold.Clear();

                    priceOfGold.Text += $"قیمت طلا: {FastCode.Split(purePriceofGold, 3, ",")}\r\n";

                    if (checkBox1.Checked)
                    {
                        if (this.fee.Text.Contains(',') || fee > 1) fee *= 100 / goldPrice;

                        a = Math.Round(mass * goldPrice * (fee + 1));
                        percentage = Math.Round((a - purePriceofGold) / purePriceofGold * 100, 2);
                        priceOfGold.Text += $"حالت بازاری اجرت %{percentage} بیشتر از قیمت طلا است: {FastCode.Split(a, 3, ",")}\r\n";

                        if (this.benefit.Text != "")
                        {
                            a = Math.Round(mass * goldPrice * (fee + 1) * (benefit + 1));
                            percentage = Math.Round((a - purePriceofGold) / purePriceofGold * 100, 2);
                            priceOfGold.Text += $"حالت بازاری اجرت × سود %{percentage} بیشتر از قیمت طلا است: {FastCode.Split(a, 3, ",")}\r\n";

                            if (this.tax.Text != "")
                            {
                                a = Math.Round(mass * goldPrice * (fee + 1) * (benefit + 1) * (tax + 1));
                                percentage = Math.Round((a - purePriceofGold) / purePriceofGold * 100, 2);
                                priceOfGold.Text += $"حالت بازاری اجرت × سود × مالیات %{percentage} بیشتر از قیمت طلا است: {FastCode.Split(a, 3, ",")}\r\n";

                                a = Math.Round(mass * goldPrice * (fee + 1) * (benefit + tax + 1));
                                percentage = Math.Round((a - purePriceofGold) / purePriceofGold * 100, 2);
                                priceOfGold.Text += $"حالت بازاری اجرت × (سود + مالیات) %{percentage} بیشتر از قیمت طلا است: {FastCode.Split(a, 3, ",")}\r\n";

                                a = Math.Round(mass * goldPrice * ((tax + 1) * ((benefit + 1) * (fee + 1) - 1) + 1));
                                percentage = Math.Round((a - purePriceofGold) / purePriceofGold * 100, 2);
                                priceOfGold.Text += $"حالت قانونی %{percentage} بیشتر از قیمت طلا است: {FastCode.Split(a, 3, ",")}";
                            }
                        }
                    }
                    else if (checkBox2.Checked)
                    {
                        a = Math.Round(mass * goldPrice * 0.99);
                        percentage = -Math.Round((a - purePriceofGold) / purePriceofGold * 100, 2);
                        priceOfGold.Text += $"حالت فروش حداقل %{percentage}- کمتر از قیمت طلا است: {FastCode.Split(a, 3, ",")}\r\n";
                       
                        a = Math.Round(mass * goldPrice * 0.98);
                        percentage = -Math.Round((a - purePriceofGold) / purePriceofGold * 100, 2);
                        priceOfGold.Text += $"حالت فروش حداکثر %{percentage}- کمتر از قیمت طلا است: {FastCode.Split(a, 3, ",")}\r\n";
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (priceOfGold.SelectedText.Length > 0)
            {
                Clipboard.SetText(priceOfGold.SelectedText);
            }
        }
        private void priceOfGold_DoubleClick(object sender, EventArgs e)
        {
            priceOfGold.SelectAll();
        }
        private void getGoldPrice_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Uri uri = new Uri("https://www.tgju.org/");
            HtmlWeb web = new HtmlWeb();
            var sourceHtmlCode = web.Load(uri, "GET");
            HtmlNode tmp = default;
            try
            {
                tmp = sourceHtmlCode.DocumentNode.SelectSingleNode("//tbody");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            HtmlNodeCollection childNodes = tmp.ChildNodes;
            goldPrice.Text = tmp.ChildNodes.Where(x => x.NodeType == HtmlNodeType.Element && x.InnerText.Contains("طلا")).First().InnerText.Split('\n')[2];
            goldPrice.Text = string.Concat(goldPrice.Text.Where(c => !char.IsWhiteSpace(c)));
            Cursor = Cursors.Default;
        }

        #region Condition
        bool breakLoop = false;
        private void mass_KeyUp(object sender, KeyEventArgs e)
        {
            int a;
            TextBox textBox = (TextBox)sender;
            a = textBox.SelectionStart;
            textBox.Text = FastCode.NumberFormatFixer(textBox.Text);
            textBox.SelectionStart = a + 1;
            if (!(textBox.Name == "fee" || e.KeyValue == 8 || (37 <= e.KeyValue && e.KeyValue <= 40)))
            {
                if (((textBox.TextLength >= 3 && textBox.Name == "mass") || (textBox.TextLength >= 2 && (textBox.Name == "benefit" || textBox.Name == "tax"))) && !textBox.Text.Contains('.'))
                {
                    a = textBox.SelectionStart;
                    textBox.Text += '.';
                    textBox.SelectionStart = a + 1;
                }
            }
        }
        private void goldPrice_TextChanged(object sender, EventArgs e)
        {
            if (breakLoop) return;
            if (FastCode.IsNumber(goldPrice.Text))
            {
                int a = goldPrice.SelectionStart;
                string s = goldPrice.Text.Replace(",", "");
                if (goldPrice.Text != FastCode.Split(s, 3, ","))
                {
                    breakLoop = true;
                    goldPrice.Text = FastCode.Split(s, 3, ",");
                    breakLoop = false;
                    goldPrice.SelectionStart = a + s.Length / 4;
                }
            }
            else if (goldPrice.Text != "")
            {
                breakLoop = true;
                goldPrice.Text = FastCode.NumberFormatFixer(goldPrice.Text);
                breakLoop = false;
                goldPrice.SelectionStart = goldPrice.Text.Length;
                goldPrice_TextChanged(sender, e);
            }
            gram_TextChanged(sender, e);
        }

        private void fee_TextChanged(object sender, EventArgs e)
        {
            if (breakLoop) return;
            else if (FastCode.IsNumber(fee.Text) && fee.Text.Replace(",", "").Replace(".", "").Length > 3)
            {
                int a = fee.SelectionStart;
                string s = fee.Text.Replace(",", "").Replace(".", "");
                if (fee.Text != FastCode.Split(s, 3, ","))
                {
                    breakLoop = true;
                    fee.Text = FastCode.Split(s, 3, ",");
                    breakLoop = false;
                    fee.SelectionStart = a + s.Length / 4;
                }
            }
            else if (fee.Text != "" && !FastCode.IsNumber(fee.Text))
            {
                breakLoop = true;
                fee.Text = FastCode.NumberFormatFixer(fee.Text);
                breakLoop = false;
                fee.SelectionStart = fee.Text.Length;
                fee_TextChanged(sender, e);
            }
            gram_TextChanged(sender, e);
        }
        private void fee_Validated(object sender, EventArgs e)
        {
            if (fee.Text == "")
            {
                breakLoop = true;
                fee.Text = "7";
                breakLoop = false;
            }
            if (FastCode.IsNumber(fee.Text) && !fee.Text.Contains(','))
            {
                breakLoop = true;
                fee.Text += '%';
                breakLoop = false;
            }
            else if (!FastCode.IsNumber(fee.Text))
            {
                breakLoop = true;
                fee.Text = FastCode.NumberFormatFixer(fee.Text);
                fee_Validated(sender, e);
                breakLoop = false;
            }
        }
        private void fee_Click(object sender, EventArgs e)
        {
            int a = fee.SelectionStart;
            breakLoop = true;
            fee.Text = fee.Text.Replace("%", "");
            fee.SelectionStart = a;
            breakLoop = false;
        }

        private void benefit_TextChanged(object sender, EventArgs e)
        {
            gram_TextChanged(sender, e);
        }
        private void benefit_Validated(object sender, EventArgs e)
        {
            if (benefit.Text == "") benefit.Text = "7";
            if (FastCode.IsNumber(benefit.Text)) benefit.Text += '%';
            else
            {
                benefit.Text = FastCode.NumberFormatFixer(benefit.Text);
                benefit_Validated(sender, e);
            }
        }
        private void benefit_Click(object sender, EventArgs e)
        {
            int a = benefit.SelectionStart;
            benefit.Text = benefit.Text.Replace("%", "");
            benefit.SelectionStart = a;
        }

        private void tax_TextChanged(object sender, EventArgs e)
        {
            gram_TextChanged(sender, e);
        }
        private void tax_Validated(object sender, EventArgs e)
        {
            if (tax.Text == "") tax.Text = "9";
            if (FastCode.IsNumber(tax.Text)) tax.Text += '%';
            else
            {
                tax.Text = FastCode.NumberFormatFixer(tax.Text);
                tax_Validated(sender, e);
            }
        }
        private void tax_Click(object sender, EventArgs e)
        {
            int a = tax.SelectionStart;
            tax.Text = tax.Text.Replace("%", "");
            tax.SelectionStart = a;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) checkBox2.Checked = false;
            else checkBox2.Checked = true;
            gram_TextChanged(sender, e);
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) checkBox1.Checked = false;
            else checkBox1.Checked = true;
            gram_TextChanged(sender, e);
        }

        #endregion

        #endregion

        
    }
}

