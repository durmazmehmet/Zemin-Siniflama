using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using ZedGraph;

namespace BirlesikZeminSiniflama
{
    public partial class BZS : Form
    {
        GraphPane myPane;
        PointPairList gradPL = new PointPairList(); PointPairList PIChartpair = new PointPairList(); PointPairList D60pair = new PointPairList();
        PointPairList D30pair = new PointPairList(); PointPairList D10pair = new PointPairList();
        string birlesiksinif = "-"; string ince = "-"; string aciklama = "-";
        double d60 = double.NaN; double d30 = double.NaN; double d10 = double.NaN;
        double cu = double.NaN; double cc = double.NaN; double GI = double.NaN;
        string[] ctls = new string[] { "e75", "e50", "e37_5", "e25", "e19", "e9_5", "eNo4", "eNo10", "eNo40", "eNo200" };
        double[] xEksen = new double[] { 75, 50, 37.5, 25, 19, 9.5, 4.75, 2, 0.425, 0.075 };

        public BZS()
        {
            InitializeComponent();
        }

        private bool isFormClosing = false; //Formun kapanıyor olduğunu işaretleme
        private const int WM_CLOSE = 16; // Kapanma gizli mesajı
        protected override void WndProc(ref Message m) //Eğer form kapanacaksa validasyon tuzağını kapat
        {
            if (m.Msg == WM_CLOSE) isFormClosing = true;
            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            grad_grafik(gradPL);

            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Enter += new EventHandler(textBoxAll_Enter);
                tb.MouseDown += new MouseEventHandler(textBoxAll_Enter);
                tb.KeyDown += new KeyEventHandler(textBoxAll_pressEnter);
                tb.Validating += new CancelEventHandler(textBoxAll_validating);
                tb.TextChanged += new EventHandler(textBoxAll_Changed);
            }

            foreach (Button bt in this.Controls.OfType<Button>()) bt.Click += new System.EventHandler(this.button_Enter);

            zedGraphControl1.PointValueEvent += new ZedGraph.ZedGraphControl.PointValueHandler(zedGraphControl1_PointValueEvent);
        }

        private void textBoxAll_Changed(object sender, EventArgs e)
        {
            ((TextBox)sender).Modified = true;
        }

        private void textBoxAll_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void textBoxAll_validating(object sender, CancelEventArgs e)
        {
            if (!isFormClosing)
            {
                string errorMsg;

                if (((TextBox)sender).Text == "0" &&
                    !((TextBox)sender).Modified &&
                    ((TextBox)sender).Name != "eLL" &&
                    ((TextBox)sender).Name != "ePI" &&
                    rakamgiriskontrol(((TextBox)sender), out errorMsg))
                    ((TextBox)sender).Text = "100";

                if (!rakamgiriskontrol(((TextBox)sender), out errorMsg)) // Doğru değerigirene kadar inat et
                {
                    e.Cancel = true;
                    ((TextBox)sender).SelectAll();
                    errorProvider1.SetError(((TextBox)sender), errorMsg);
                    toolTip1.Show(errorMsg, ((TextBox)sender));
                }
                else
                {
                    if (((TextBox)sender).Text == "") ((TextBox)sender).Text = "100";
                    if (Convert.ToDouble(((TextBox)sender).Text) > 100) ((TextBox)sender).Text = "100";
                    if (Convert.ToDouble(((TextBox)sender).Text) < 0) ((TextBox)sender).Text = "0";
                    errorProvider1.Clear();
                    toolTip1.RemoveAll();
                }
            }
        }

        public bool rakamgiriskontrol(object sender, out string errorMessage)
        {
            double parsedValue;
            if (!double.TryParse(((TextBox)sender).Text, out parsedValue))
            {
                if (((TextBox)sender).Name != "eLL" && ((TextBox)sender).Name != "ePI")
                    errorMessage = "0 ile 100 arasında bir sayı giriniz!";
                else
                    errorMessage = "Non-Plastik Malzeme için 0 değeri giriniz!";
                return false;
            }
            else
            {
                var onceki = GetNextControl(((TextBox)sender), false);
                if (onceki != null)
                {
                    if (Convert.ToDouble(onceki.Text) < Convert.ToDouble(((TextBox)sender).Text) && ((TextBox)sender).Name != "eLL" && ((TextBox)sender).Name != "ePI")
                    {
                        errorMessage = xEksen[((TextBox)sender).TabIndex] + "mm elekten geçen % değeri\n" + xEksen[onceki.TabIndex] + "mm elekten geçen % değerinden\nbüyük olamaz!";
                        return false;
                    }
                }
                errorMessage = "";
                return true;
            }
        }

        private void button_Enter(object sender, EventArgs e)
        {

            if (((Button)sender).Name == "hesap") hesapla();
            if (((Button)sender).Name == "p1Clip")
            {
                Clipboard.SetText(txt_aashtosinif.Text);
                MessageBox.Show("' " + txt_aashtosinif.Text + "' \nMetni Panoya kopyalandı", "Uyarı");
            }
            if (((Button)sender).Name == "gClip") zedGraphControl1.Copy(true);
            if (((Button)sender).Name == "p2Clip")
            {
                Clipboard.SetText(txt_birlesiksinif.Text);
                MessageBox.Show("' " + txt_birlesiksinif.Text + "' \nMetni Panoya kopyalandı", "Uyarı");
            }
            if (((Button)sender).Name == "button5")
                MessageBox.Show("Birleşik Zemin Sınıflaması Programı" +
                    "\nKGM 1. Bölge AR-GE Başmühendisliğinde hazırlanmıştır." +
                    "\nYazılımın her hakkı saklıdır © Nisan 2015, İstanbul" +
                    "\n\nKodlama ve Dizayn : " + isimhakki() +
                    "\nİletişim: mehmet@mehmetdurmaz.com" +
                    "\n\nKaynakça:" +
                    "\n(1) AASHTO Soil Classification System (Hogentogler, C.A.;" +
                    " Terzaghi, K. (May 1929). Interrelationship of load, road and subgrade. Public Roads: pp.37–64.)" +
                    "\n(2) Classification of Soils for Engineering Purposes: Annual Book of ASTM Standards;" +
                    " D 2487-83, 04.08, American Society for Testing and Materials, 1985, pp. 395–408" +
                    " Evett, Jack and Cheng Liu (2007), Soils and Foundations (7 ed.), Prentice Hall, pp. 9–29, ISBN 0132221381" +
                    "\n\nKullanılan sınıf kütüphaneleri:" +
                    "\n(3) ZedGraph (görsel grafik); jchampion, kooseefoo, rjosulli", "Yazılım Hakkında", MessageBoxButtons.OK);
            if (((Button)sender).Name == "GRADgraf") grad_grafik(gradPL);
            if (((Button)sender).Name == "PIgraf") PI_grafik(PIChartpair);
            if (((Button)sender).Name == "cls") sifirla(true);
        }

        private void textBoxAll_temizle()
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>()) tb.Text = "0";
        }

        private void textBoxAll_pressEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
            {
                if (this.ActiveControl != null) this.SelectNextControl(this.ActiveControl, true, true, true, true);
                e.Handled = true; // tuşu basıldı say
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
            {
                var onceki = GetNextControl(ActiveControl, false);
                if (onceki != null) onceki.Focus();
                e.Handled = true; // tuşu basıldı say
                e.SuppressKeyPress = true;
            }
        }

        private void hesapla()
        {
            if (Convert.ToDouble(eLL.Text) > 0 && Convert.ToDouble(ePI.Text) == 0)
            {
                errorProvider1.SetError(eLL, "Malzeme Non-Plastik ise LL=PI=0 (NP) olmalı");
                toolTip1.Show("Malzeme Non-Plastik ise LL=PI=0 (NP) olmalı", eLL);
                ePI.Focus();
                return;
            }
            if (Convert.ToDouble(ePI.Text) > 0 && Convert.ToDouble(eLL.Text) == 0)
            {
                errorProvider1.SetError(ePI, "Malzeme Non-Plastik ise LL=PI=0 (NP) olmalı");
                toolTip1.Show("Malzeme Non-Plastik ise LL=PI=0 (NP) olmalı", ePI);
                eLL.Focus();
                return;
            }

            sifirla(false);
            double[] yEksen = new double[] 
            { 
                Convert.ToDouble(e75.Text), Convert.ToDouble(e50.Text), Convert.ToDouble(e37_5.Text), 
                Convert.ToDouble(e25.Text), Convert.ToDouble(e19.Text), Convert.ToDouble(e9_5.Text), 
                Convert.ToDouble(eNo4.Text), Convert.ToDouble(eNo10.Text), Convert.ToDouble(eNo40.Text), 
                Convert.ToDouble(eNo200.Text)
            };

            double[] xEksen = new double[] { 75, 50, 37.5, 25, 19, 9.5, 4.75, 2, 0.425, 0.075 };
            double LL = Convert.ToDouble(eLL.Text); double PI = Convert.ToDouble(ePI.Text);

            //D60 , D30 , D10 hesabı                 
            for (int i = 1; i < 10; i++)
            {
                if (yEksen[i - 1] >= 60 && yEksen[i] <= 60)
                    d60 = Math.Exp((Math.Log(xEksen[i - 1]) - Math.Log(xEksen[i])) * (60 - yEksen[i - 1]) / (yEksen[i - 1] - yEksen[i]) + Math.Log(xEksen[i - 1]));
                if (yEksen[i - 1] >= 30 && yEksen[i] <= 30)
                    d30 = Math.Exp((Math.Log(xEksen[i - 1]) - Math.Log(xEksen[i])) * (30 - yEksen[i - 1]) / (yEksen[i - 1] - yEksen[i]) + Math.Log(xEksen[i - 1]));
                if (yEksen[i - 1] >= 10 && yEksen[i] <= 10)
                    d10 = Math.Exp((Math.Log(xEksen[i - 1]) - Math.Log(xEksen[i])) * (10 - yEksen[i - 1]) / (yEksen[i - 1] - yEksen[i]) + Math.Log(xEksen[i - 1]));
            }

            //Cu, Cc hesabı
            cu = d60 / d10; 
            cc = (d30 * d30) / (d10 * d60);
            CuCc.Text = "Cu: " + Math.Round(cu,2).ToString() + "\n" + "Cc: " + Math.Round(cc,2).ToString();
            d603010.Text = "D60: " + Math.Round(d60, 2).ToString() + "\n" + "D30: " + Math.Round(d30, 2).ToString() + "\n" + "D10: " + Math.Round(d10, 2).ToString();

            // kum çakıl içeriği ayrımı
            double cakil = yEksen[0] - yEksen[6]; double kum = yEksen[6] - yEksen[9];

            // Grafiiği besleme
            for (int k = 0; k < 10; k++) gradPL.Add(xEksen[k], yEksen[k]);

            double[] PIy = new double[] { 0, PI }; double[] PIx = new double[] { LL, LL };
            double[] LLy = new double[] { PI, PI }; double[] LLx = new double[] { 0, LL };
            PIChartpair.Add(LLx, LLy); PIChartpair.Add(PIx, PIy);

            switch (myPane.Title.Text)
            {
                case "Gradasyon": grad_grafik(gradPL); break;
                case "PI Kart": PI_grafik(PIChartpair); break;
            }

            //
            // SINIFLANDIRMA               
            // AASHTO SINIFI
            string asinif = null;
            if (yEksen[7] <= 50 && yEksen[8] <= 30 && yEksen[9] <= 15 && PI <= 6 && asinif == null) asinif = "A - 1 - a";
            if (yEksen[8] <= 50 && yEksen[9] <= 25 && PI <= 6 && asinif == null) asinif = "A - 1 - b";
            if (yEksen[8] > 50 && yEksen[9] <= 10 && LL == 0 && asinif == null) asinif = "A - 3";
            if (yEksen[9] <= 35 && LL <= 40 && PI <= 10 && asinif == null) asinif = "A - 2 - 4";
            if (yEksen[9] <= 35 && LL > 40 && PI <= 10 && asinif == null) asinif = "A - 2 - 5";
            if (yEksen[9] <= 35 && LL <= 40 && PI > 10 && asinif == null) asinif = "A - 2 - 6";
            if (yEksen[9] <= 35 && LL > 40 && PI > 10 && asinif == null) asinif = "A - 2 - 7";
            if (yEksen[9] > 35 && LL <= 40 && PI <= 10 && asinif == null) asinif = "A - 4";
            if (yEksen[9] > 35 && LL > 40 && PI <= 10 && asinif == null) asinif = "A - 5";
            if (yEksen[9] > 35 && LL <= 40 && PI > 10 && asinif == null) asinif = "A - 6";
            if (yEksen[9] > 35 && LL > 40 && PI > 10 && asinif == null) asinif = (PI <= (LL - 30)) ? "A - 7 - 5" : "A - 7 - 6";
            //GI HESAPLAMA 
            //düzeltme 0.075 malzemenin limitleri için düzeltme 11.06.2015
            double fixNo200 = double.NaN;
            double fixLL = double.NaN;
            double fixPI = double.NaN;
            fixLL = LL < 40 ? 40 : LL;
            fixLL = LL > 60 ? 60 : LL;
            fixPI = PI < 10 ? 10 : PI;
            fixPI = PI > 30 ? 30 : PI;
            if (asinif == "A - 2 - 5" || asinif == "A - 2 - 6")
            {
                fixNo200 = yEksen[9] < 15.0 ? 15.0 : yEksen[9]; 
                fixNo200 = yEksen[9] > 55.0 ? 55.0 : yEksen[9];                
                GI = 0.01 * (fixNo200 - 15) * (fixPI - 10);
            }
            else
            {   
                fixNo200 = yEksen[9] < 35 ? 35 : yEksen[9];
                fixNo200 = yEksen[9] > 75 ? 75 : yEksen[9];                
                GI = (fixNo200 - 35) * (0.2 + 0.005 * (fixLL - 40)) + 0.01 * (fixNo200 - 15) * (fixPI - 10);
            }            
            GI = GI < 0 ? 0 : GI;
            //düzeltme bitiş
            txt_aashtosinif.Text = asinif + "  /  " + Convert.ToString(Math.Round(GI, 1));

            ince = inceK(LL, PI, yEksen[9]);
            if (100 - yEksen[9] > 50) // kaba malzeme
            {
                if (cakil > kum) // çakıllı kaba malzeme
                    kaba_Cakilli(cu, cc, ince, kum, yEksen[9]);
                else // kumlu kaba malzeme
                    kaba_Kumlu(cu, cc, ince, cakil, yEksen[9]);
            }
            else // ince malzeme
            {
                if (LL < 50) // düşük plastik
                    if (!organik.Checked) // LL < 50 ve organik değil (ince daneli, incesi inorganik ve düşük Plastiteli Malzeme)
                        ince_L(ince, kum, cakil, yEksen[9]);
                    else // LL < 50 ve organik (ince daneli, incesi organik ve düşük Plastiteli Malzeme)
                        ince_L_organik(PI, LL, kum, cakil, yEksen[9]);
                else // yüksek plastik
                    if (!organik.Checked) // LL >= 50 ve organik değil (ince daneli, incesi inorganik ve yüksek Plastiteli Malzeme)
                        ince_H(ince, kum, cakil, yEksen[9]);
                    else // LL >= 50 ve organik (ince daneli, incesi organik ve yüksek Plastiteli Malzeme)
                        ince_H_organik(PI, LL, kum, cakil, yEksen[9]);
            }

            txt_birlesiksinif.Text = birlesiksinif;
            txt_ince.Text = ince;
            txt_aciklama.Text = aciklama;
        }

        // KABA ÇAKILLI MALZEME
        private void kaba_Cakilli(double cu, double cc, string ince, double kum, double no200)
        {
            if (no200 > 12) // ince kısım > %12 --> GM, GC, GC-GM
            {
                if (ince == "ML" || ince == "MH") // "Silty Gravel" : "Silty Gravel with Sand"
                {
                    birlesiksinif = "GM";
                    aciklama = (kum < 15) ? "Siltli Çakıl" : "Siltli Çakıl ile birlikte Kum";
                }
                if (ince == "CL" || ince == "CH") // "Clayey Gravel" : "Clayey Gravel with Sand" 
                {
                    birlesiksinif = "GC";
                    aciklama = (kum < 15) ? "Killi Çakıl" : "Killi Çakıl ile birlikte Kum";
                }
                if (ince == "CL-ML") // "Silty Clayey Gravel" : "Silty Clayey Gravel with Sand" 
                {
                    birlesiksinif = "GC - GM";
                    aciklama = (kum < 15) ? "Siltli Killi Çakıl" : "Siltli Killi Çakıl ile birlikte Kum";
                }
            }

            if (no200 >= 5 && no200 <= 12) // %5 < ince kısım < %12  
            {
                if (cu >= 4 && cc >= 1 && cc <= 3) // --> GW-GM ve GW-GC
                {
                    if (ince == "ML" || ince == "MH") // "Well Graded Gravel with Silt" : "Well Graded Gravel with Silt and Sand"
                    {
                        birlesiksinif = "GW - GM";
                        aciklama = (kum < 15) ? "İyi Derecelenmiş Çakıl ile birlikte Silt" : "İyi Derecelenmiş Çakıl ile birlikte Silt ve Kum";
                    }
                    if (ince == "CL" || ince == "CH" || ince == "CL-ML") // "Well Graded Gravel with Clay" : "Well Graded Gravel with Clay and Sand"
                    {
                        birlesiksinif = "GW - GC";
                        aciklama = (kum < 15) ? "İyi Derecelenmiş Çakıl ile birlikte Kil" : "İyi Derecelenmiş Çakıl ile birlikte Kil ve Kum";
                    }
                }
                else // GP-GM, GP-GC ve siltli GP-GC 
                {
                    if (ince == "ML" || ince == "MH") // "Poor Graded Gravel with Silt" : "Poor Graded Gravel with Silt and Sand" 
                    {
                        birlesiksinif = "GP - GM";
                        aciklama = (kum < 15) ? "Kötü Derecelenmiş Çakıl ile birlikte Silt" : "Kötü Derecelenmiş Çakıl ile birlikte Silt ve Kum";
                    }
                    if (ince == "CL" || ince == "CH") // "Poor Graded Gravel with Clay" : "Poor Graded Gravel with Clay and Sand"
                    {
                        birlesiksinif = "GP - GC";
                        aciklama = (kum < 15) ? "Kötü Derecelenmiş Çakıl ile birlikte Kil" : "Kötü Derecelenmiş Çakıl ile birlikte Kil ve Kum";
                    }
                    if (ince == "CL-ML") // "Poor Graded Gravel with Silty Clay" : "Poor Graded Gravel with Silty Clay and Sand"
                    {
                        birlesiksinif = "GP - GC";
                        aciklama = (kum < 15) ? "Kötü Derecelenmiş Çakıl ile birlikte Siltli Kil" : "Kötü Derecelenmiş Çakıl ile birlikte Siltli Kil ve Kum";
                    }
                }
            }

            if (no200 < 5) // ince kısım < %5 --> GW ve GP
                if (cu >= 4 && cc >= 1 && cc <= 3) // "Well Graded Gravel" : "Well Graded Gravel with Sand"
                {
                    birlesiksinif = "GW";
                    ince = "-";
                    aciklama = (kum < 15) ? "İyi Derecelenmiş Çakıl" : "İyi Derecelenmiş Çakıl ile birlikte Kum";
                }
                else // "Poor Graded Gravel" : "Poor Graded Gravel with Sand"
                {
                    birlesiksinif = "GP";
                    ince = "-";
                    aciklama = (kum < 15) ? "Kötü Derecelenmiş Çakıl" : "Kötü Derecelenmiş Çakıl ile birlikte Kum";
                }
        }

        // KABA KUMLU MALZEME
        private void kaba_Kumlu(double cu, double cc, string ince, double cakil, double no200)
        {
            if (no200 > 12) // ince kısım > %12 --> SW, SC, SC-SM
            {
                if (ince == "ML" || ince == "MH") // "Silty Sand" : "Silty Sand with Gravel"
                {
                    birlesiksinif = "SM";
                    aciklama = (cakil < 15) ? "Siltli Kum" : "Siltli Kum ile birlikte Çakıl";
                }
                if (ince == "CL" || ince == "CH") // "Clayey Sand" : "Clayey Sand with Gravel"   
                {
                    birlesiksinif = "SC";
                    aciklama = (cakil < 15) ? "Killi Kum" : "Killi Kum ile birlikte Çakıl";
                }
                if (ince == "CL-ML") // "Silty Clayey Sand" : "Silty Clayey Sand with Gravel"   
                {
                    birlesiksinif = "SC - SM";
                    aciklama = (cakil < 15) ? "Siltli Killi Kum" : "Siltli Killi Kum ile birlikte Çakıl";
                }
            }

            if (no200 >= 5 && no200 <= 12) // %5 < ince kısım < %12 --> SW-SM ve SW-SC
            {
                if (cu >= 6 && cc >= 1 && cc <= 3)
                {
                    if (ince == "ML" || ince == "MH")
                    {
                        birlesiksinif = "SW - SM";
                        aciklama = (cakil < 15) ? "İyi Derecelenmiş Kum ile birlikte Silt" : "İyi Derecelenmiş Kum ile birlikte Silt ve Çakıl";
                        // "Well Graded Sand with Silt" : "Well Graded Sand with Silt and Gravel"    
                    }
                    if (ince == "CL" || ince == "CH" || ince == "CL-ML")
                    {
                        birlesiksinif = "SW - SC";
                        aciklama = (cakil < 15) ? "İyi Derecelenmiş Kum ile birlikte Kil" : "İyi Derecelenmiş Kum ile birlikte Kil ve Çakıl";
                        // "Well Graded Sand with Clay" : "Well Sand Gravel with Clay and Gravel"                            
                    }
                }
                else // SP-SM, SP-SC ve siltli SP-SC 
                {
                    if (ince == "ML" || ince == "MH") // "Poor Graded Sand with Silt" : "Poor Graded Sand with Silt and Gravel"
                    {
                        birlesiksinif = "SP - SM";
                        aciklama = (cakil < 15) ? "Kötü Derecelenmiş Kum ile birlikte Silt" : "Kötü Derecelenmiş Kum ile birlikte Silt ve Çakıl";
                    }
                    if (ince == "CL" || ince == "CH") // "Poor Graded Sand with Clay" : "Poor Graded Sand with Clay and Gravel"
                    {
                        birlesiksinif = "SP - SC";
                        aciklama = (cakil < 15) ? "Kötü Derecelenmiş Kum ile birlikte Kil" : "Kötü Derecelenmiş Kum ile birlikte Kil ve Çakıl";
                    }
                    if (ince == "CL-ML") // "Poor Graded Sand with Silty Clay" : "Poor Graded Sand with Silty Clay and Gravel"
                    {
                        birlesiksinif = "SP - SC";
                        aciklama = (cakil < 15) ? "Kötü Derecelenmiş Kum ile birlikte Siltli Kil" : "Kötü Derecelenmiş Kum ile birlikte Siltli Kil ve Çakıl";
                    }
                }
            }

            if (no200 < 5) // ince kısım < %5
            {
                if (cu >= 6 && cc >= 1 && cc <= 3) // SW ve SP "Well Graded Sand" : "Well Graded Sand with Gravel"
                {
                    birlesiksinif = "SW";
                    ince = "-";
                    aciklama = (cakil < 15) ? "İyi Derecelenmiş Kum" : "İyi Derecelenmiş Kum ile birlikte Çakıl";
                }
                else // "Poor Graded Sand" : "Poor Graded Sand with Gravel"
                {
                    birlesiksinif = "SP";
                    ince = "-";
                    aciklama = (cakil < 15) ? "Kötü Derecelenmiş Kum" : "Kötü Derecelenmiş Kum ile birlikte Çakıl";
                }
            }
        }

        // DÜŞÜK PLASTİSİTELİ İNORGANİK ZAYIF KİL - SİLTLİ KİL - SİLT
        private void ince_L(string ince, double kum, double cakil, double no200) // ince daneli, incesi inorganik ve düşük Plastiteli Malzeme
        {
            if (ince == "CL") // Lean Clay
            {
                if (100 - no200 >= 30)
                    if (kum >= cakil) // "Sandy Lean Clay" : "Sandy Lean Clay with Gravel"
                        aciklama = (cakil < 15) ? "Kumlu Zayıf Kil" : "Kumlu Zayıf Kil ile birlikte Çakıl";
                    else // "Gravelly Lean Clay" : "Gravelly Lean Clay with Sand"
                        aciklama = (kum < 15) ? "Çakıllı Zayıf Kil" : "Çakıllı Zayıf Kil ile birlikte Kum";

                if (100 - no200 < 30 && 100 - no200 >= 15) // "Lean Clay with Sand" : "Lean Clay with Gravel"
                    aciklama = (kum >= cakil) ? "Zayıf Kil ile birlikte Kum" : "Zayıf Kil ile birlikte Çakıl";

                aciklama = (100 - no200 < 15) ? "Zayıf Kil" : aciklama; // Lean Clay 
            }

            if (ince == "CL-ML") // Silty Clay
            {
                if (100 - no200 >= 30)
                    if (kum >= cakil)  // "Sandy Silty Clay" : "Sandy Silty Clay with Gravel"
                        aciklama = (cakil < 15) ? "Kumlu Siltli Kil" : "Kumlu Siltli Kil ile birlikte Çakıl";
                    else // "Gravelly Silty Clay" : "Gravelly Silty Clay with Sand"
                        aciklama = (kum < 15) ? "Çakıllı Siltli Kil" : "Çakıllı Siltli Kil ile birlikte Kum";

                if (100 - no200 < 30 && 100 - no200 >= 15) // "Silty Clay with Sand" : "Silty Clay with Gravel"
                    aciklama = (kum >= cakil) ? "Siltli Kil ile birlikte Kum" : "Siltli Kil ile birlikte Çakıl";

                aciklama = (100 - no200 < 15) ? "Siltli Kil" : aciklama; // Silty Clay 
            }

            if (ince == "ML") // Silt
            {
                if (100 - no200 >= 30) //isWithCoarseGrains
                    if (kum >= cakil) // "Sandy Silt" : "Sandy Clay with Gravel"
                        aciklama = (cakil < 15) ? "Kumlu Silt" : "Kumlu Silt ile birlikte Çakıl"; //SetExtendedExist()
                    else // "Gravelly Silt" : "Gravelly Silt with Sand"
                        aciklama = (kum < 15) ? "Çakıllı Silt" : "Çakıllı Silt ile birlikte Kum";

                if (100 - no200 < 30 && 100 - no200 >= 15) // "Silt with Sand" : "Silt with Gravel" //isExtendedExist()
                    aciklama = (kum >= cakil) ? "Silt ile birlikte Kum" : "Silt ile birlikte Çakıl";

                aciklama = (100 - no200 < 15) ? "Silt" : aciklama; // Silt               
            }

            birlesiksinif = ince;
            ince = "-";
        }

        // YÜKSEK PLASTİSİTELİ İNORGANİK YAĞLI KİL - ELASTİK SİLT
        private void ince_H(string ince, double kum, double cakil, double no200) // ince daneli, incesi inorganik ve Yüksek Plastiteli Malzeme
        {
            if (ince == "CH") // Fat Clay
            {
                if (100 - no200 >= 30)
                    if (kum >= cakil) // "Sandy Fat Clay" : "Sandy Fat Clay with Gravel"
                        aciklama = (cakil < 15) ? "Kumlu Yağlı Kil" : "Kumlu Yağlı Kil ile birlikte Çakıl";
                    else // "Gravelly Fat Clay" : "Gravelly Fat Clay with Sand"
                        aciklama = (kum < 15) ? "Çakıllı Yağlı Kil" : "Çakıllı Yağlı Kil ile birlikte Kum";

                if (100 - no200 < 30 && 100 - no200 >= 15) // "Fat Clay with Sand" : "Fat Clay with Gravel"
                    aciklama = (kum >= cakil) ? "Yağlı Kil ile birlikte Kum" : "Yağlı Kil ile birlikte Çakıl";

                aciklama = (100 - no200 < 15) ? "Yağlı Kil" : aciklama; // Lean Clay 
            }

            if (ince == "MH") // Elastic Silt
            {
                if (100 - no200 >= 30)
                    if (kum >= cakil) // "Sandy Elastic Silt" : "Sandy Elastic Silt with Gravel"
                        aciklama = (cakil < 15) ? "Kumlu Elastik Silt" : "Kumlu Elastik Silt ile birlikte Çakıl";
                    else // "Gravelly Elastic Silt" : "Gravelly Elastic Silt with Sand"
                        aciklama = (kum < 15) ? "Çakıllı Elastik Silt" : "Çakıllı Elastik Silt ile birlikte Kum";

                if (100 - no200 < 30 && 100 - no200 >= 15) // "Elastic Silt with Sand" : "Elastic Silt with Gravel"
                    aciklama = (kum >= cakil) ? "Elastik Silt ile birlikte Kum" : "Elastik Silt ile birlikte Çakıl";

                aciklama = (100 - no200 < 15) ? "Elastik Silt" : aciklama; // Elastic Silt          
            }

            birlesiksinif = ince;
            ince = "-";
        }

        // DÜŞÜK PLASTİSİTELİ ORGANİK KİL - SİLT
        private void ince_L_organik(double PI, double LL, double kum, double cakil, double no200) // ince daneli, incesi organik ve düşük Plastiteli Malzeme
        {
            if (PI >= 4 && PI >= 0.73 * (LL - 20))
            {
                if (100 - no200 >= 30)
                    if (kum >= cakil) // "Sandy Organic Clay" : "Sandy Organic Clay with Gravel"
                        aciklama = (cakil < 15) ? "Kumlu Organik Kil" : "Kumlu Organik Kil ile birlikte Çakıl";
                    else // "Gravelly Organic Clay" : "Gravelly Organic Clay with Sand"
                        aciklama = (kum < 15) ? "Çakıllı Organik Kil" : "Çakıllı Organik Kil ile birlikte Kum";

                if (100 - no200 < 30 && 100 - no200 >= 15) // "Organic Clay with Sand" : "Organic Clay with Gravel"
                    aciklama = (kum >= cakil) ? "Organik Kil ile birlikte Kum" : "Organik Kil ile birlikte Çakıl";

                aciklama = (100 - no200 < 15) ? "Organik Kil" : aciklama; //Organic Clay
            }
            else
            {
                if (100 - no200 >= 30)
                    if (kum >= cakil) // "Sandy Organic Silt" : "Sandy Organic Silt with Gravel"
                        aciklama = (cakil < 15) ? "Kumlu Organik Silt" : "Kumlu Organik Silt ile birlikte Çakıl";
                    else // "Gravelly Organic Silt" : "Gravelly Organic Silt with Sand"
                        aciklama = (kum < 15) ? "Çakıllı Organik Silt" : "Çakıllı Organik Silt ile birlikte Kum";

                if (100 - no200 < 30 && 100 - no200 >= 15) // "Organic Silt with Sand" : "Organic Silt with Gravel"
                    aciklama = (kum >= cakil) ? "Organik Silt ile birlikte Kum" : "Organik Silt ile birlikte Çakıl";

                aciklama = (100 - no200 < 15) ? "Organik Silt" : aciklama; //Organic Silt
            }

            birlesiksinif = "OL";
        }

        // YÜKSEK PLASTİSİTELİ ORGANİK YAĞLI KİL - ELASTİK SİLT
        private void ince_H_organik(double PI, double LL, double kum, double cakil, double no200) // ince daneli, incesi organik ve Yüksek Plastiteli Malzeme
        {
            if (PI >= 0.73 * (LL - 20))
            {
                if (100 - no200 >= 30)
                    if (kum >= cakil) // "Sandy Organic Clay" : "Sandy Organic Clay with Gravel"
                        aciklama = (cakil < 15) ? "Kumlu Organik Kil" : "Kumlu Organik Kil ile birlikte Çakıl";
                    else // "Gravelly Organic Clay" : "Gravelly Organic Clay with Sand"
                        aciklama = (kum < 15) ? "Çakıllı Organik Kil" : "Çakıllı Organik Kil ile birlikte Kum";

                if (100 - no200 < 30 && 100 - no200 >= 15) // "Organic Clay with Sand" : "Organic Clay with Gravel"
                    aciklama = (kum >= cakil) ? "Organik Kil ile birlikte Kum" : "Organik Kil ile birlikte Çakıl";

                aciklama = (100 - no200 < 15) ? "Organik Kil" : aciklama; //Organic Clay
            }
            else
            {
                if (100 - no200 >= 30)
                    if (kum >= cakil) // "Sandy Organic Silt" : "Sandy Organic Silt with Gravel"
                        aciklama = (cakil < 15) ? "Kumlu Organik Silt" : "Kumlu Organik Silt ile birlikte Çakıl";
                    else // "Gravelly Organic Silt" : "Gravelly Organic Silt with Sand"
                        aciklama = (kum < 15) ? "Çakıllı Organik Silt" : "Çakıllı Organik Silt ile birlikte Kum";

                if (100 - no200 < 30 && 100 - no200 >= 15) // "Organic Silt with Sand" : "Organic Silt with Gravel"
                    aciklama = (kum >= cakil) ? "Organik Silt ile birlikte Kum" : "Organik Silt ile birlikte Çakıl";

                aciklama = (100 - no200 < 15) ? "Organik Silt" : aciklama; //Organic Silt
            }

            txt_birlesiksinif.Text = "OH";
        }

        private string inceK(double LL, double PI, double no200)
        {
            if (100 - no200 > 50)
            { // Kaba Malzeme ise
                if (LL < 50)
                {
                    if (PI > 7 && PI >= 0.73 * (LL - 20))
                        return "CL";
                    else
                        return (PI >= 4 && PI >= 0.73 * (LL - 20)) ? "CL-ML" : "ML";
                }
                else
                    return (PI >= 0.73 * (LL - 20)) ? "CH" : "MH";
            }
            else
            { // İnce Malzeme ise
                if (LL < 50)
                {
                    if (PI > 7 && PI >= 0.73 * (LL - 20))
                        return "CL";
                    else
                        return (PI >= 4 && PI <= 7 && PI >= 0.73 * (LL - 20)) ? "CL-ML" : "ML";
                }
                else
                    return (PI >= 0.73 * (LL - 20)) ? "CH" : "MH";
            }
        }

        private void grad_grafik(PointPairList pl1)
        {
            myPane = zedGraphControl1.GraphPane;

            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();

            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.PointValueFormat = "0.00";

            myPane.XAxis.Type = AxisType.Log;
            myPane.XAxis.Scale.Max = 100;
            myPane.XAxis.Scale.Min = 0.01;
            myPane.XAxis.Scale.IsReverse = true;
            myPane.XAxis.Scale.Mag = 0;
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MinorGrid.IsVisible = true;
            myPane.XAxis.Title.Text = "Elekler";

            myPane.YAxis.Type = AxisType.Linear;
            myPane.YAxis.Scale.Max = 102;
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Title.Text = "Geçen %";

            LineItem myCurve = myPane.AddCurve("Gradasyon", pl1, Color.Red, SymbolType.Circle);
            myCurve.Symbol.Fill.Color = System.Drawing.Color.Black;
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.2F;
            myCurve.Line.Width = 3.0F;
            myCurve.Symbol.Fill.IsVisible = true;
            myCurve.Symbol.Border.IsVisible = false;
            myCurve.Symbol.Size = 8f;

            double[] y100 = new double[] { 0, 102 };
            double[] x75mm = new double[] { 75, 75 };
            double[] xno4 = new double[] { 4.75, 4.75 };
            double[] xno200 = new double[] { 0.075, 0.075 };
            PointPairList pl75mm = new PointPairList(x75mm, y100);
            PointPairList plno4 = new PointPairList(xno4, y100);
            PointPairList plno200 = new PointPairList(xno200, y100);
            LineItem mm75Curve = myPane.AddCurve("75mm", pl75mm, Color.Blue, SymbolType.Diamond);
            LineItem no4Curve = myPane.AddCurve("No:4", plno4, Color.Blue, SymbolType.Diamond);
            LineItem no200Curve = myPane.AddCurve("No:200", plno200, Color.Blue, SymbolType.Diamond);
            mm75Curve.Line.Width = 3.0F;
            no4Curve.Line.Width = 3.0F;
            no200Curve.Line.Width = 3.0F;

            double[] x50mm = new double[] { 50, 50 };
            double[] x38mm = new double[] { 37.5, 37.5 };
            double[] x25mm = new double[] { 25, 25 };
            double[] x19mm = new double[] { 19, 19 };
            double[] x10mm = new double[] { 9.5, 9.5 };
            double[] xno10mm = new double[] { 2, 2 };
            double[] xno40mm = new double[] { 0.425, 0.425 };
            PointPairList pl50 = new PointPairList(x50mm, y100);
            PointPairList pl38 = new PointPairList(x38mm, y100);
            PointPairList pl25 = new PointPairList(x25mm, y100);
            PointPairList pl19 = new PointPairList(x19mm, y100);
            PointPairList pl10 = new PointPairList(x10mm, y100);
            PointPairList plno10 = new PointPairList(xno10mm, y100);
            PointPairList plno40 = new PointPairList(xno40mm, y100);
            LineItem cmm50 = myPane.AddCurve("50mm", pl50, Color.Black, SymbolType.Diamond);
            LineItem cmm38 = myPane.AddCurve("37.5mm", pl38, Color.Black, SymbolType.Diamond);
            LineItem cmm25 = myPane.AddCurve("25mm", pl25, Color.Black, SymbolType.Diamond);
            LineItem cmm19 = myPane.AddCurve("19mm", pl19, Color.Black, SymbolType.Diamond);
            LineItem cmm10 = myPane.AddCurve("9.5mm", pl10, Color.Black, SymbolType.Diamond);
            LineItem cno10 = myPane.AddCurve("No:10", plno10, Color.Black, SymbolType.Diamond);
            LineItem cno40 = myPane.AddCurve("No:40", plno40, Color.Black, SymbolType.Diamond);
            cmm50.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            cmm38.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            cmm25.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            cmm19.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            cmm10.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            cno40.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            cno10.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;

            TextObj ce75 = new TextObj("75mm", 75, 110);
            ce75.FontSpec.Border.IsVisible = false;
            ce75.FontSpec.Angle = 90;
            ce75.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(ce75);

            TextObj ce50 = new TextObj("50mm", 50, 110);
            ce50.FontSpec.Border.IsVisible = false;
            ce50.FontSpec.Angle = 90;
            ce50.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(ce50);

            TextObj ce38 = new TextObj("37.5mm", 37.5, 110);
            ce38.FontSpec.Border.IsVisible = false;
            ce38.FontSpec.Angle = 90;
            ce38.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(ce38);

            TextObj ce25 = new TextObj("25mm", 25, 110);
            ce25.FontSpec.Border.IsVisible = false;
            ce25.FontSpec.Angle = 90;
            ce25.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(ce25);

            TextObj ce19 = new TextObj("19mm", 19, 110);
            ce19.FontSpec.Border.IsVisible = false;
            ce19.FontSpec.Angle = 90;
            ce19.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(ce19);

            TextObj ce10 = new TextObj("9.5mm", 9.5, 110);
            ce10.FontSpec.Border.IsVisible = false;
            ce10.FontSpec.Angle = 90;
            ce10.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(ce10);

            TextObj cen4 = new TextObj("No:4", 4.75, 110);
            cen4.FontSpec.Border.IsVisible = false;
            cen4.FontSpec.Angle = 90;
            cen4.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(cen4);

            TextObj cen10 = new TextObj("No:10", 2, 110);
            cen10.FontSpec.Border.IsVisible = false;
            cen10.FontSpec.Angle = 90;
            cen10.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(cen10);

            TextObj cen40 = new TextObj("No:40", 0.425, 110);
            cen40.FontSpec.Border.IsVisible = false;
            cen40.FontSpec.Angle = 90;
            cen40.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(cen40);

            TextObj cen200 = new TextObj("No:200", 0.075, 110);
            cen200.FontSpec.Border.IsVisible = false;
            cen200.FontSpec.Angle = 90;
            cen200.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(cen200);

            D60pair.Clear();
            double[] d60y = new double[] { 0, 60 };
            double[] d60x = new double[] { d60, d60 };
            double[] xd60y = new double[] { 60, 60 };
            double[] xd60x = new double[] { 90, d60 };
            D60pair.Add(d60x, d60y);
            D60pair.Add(xd60x, xd60y);
            LineItem pld60 = myPane.AddCurve("D60", D60pair, Color.Green, SymbolType.None);
            pld60.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;

            D30pair.Clear();
            double[] d30y = new double[] { 0, 30 };
            double[] d30x = new double[] { d30, d30 };
            double[] xd30y = new double[] { 30, 30 };
            double[] xd30x = new double[] { 90, d30 };
            D30pair.Add(d30x, d30y);
            D30pair.Add(xd30x, xd30y);
            LineItem pld30 = myPane.AddCurve("D30", D30pair, Color.Green, SymbolType.None);
            pld30.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;

            D10pair.Clear();
            double[] d10y = new double[] { 0, 10 };
            double[] d10x = new double[] { d10, d10 };
            double[] xd10y = new double[] { 10, 10 };
            double[] xd10x = new double[] { 90, d10 };
            D10pair.Add(d10x, d10y);
            D10pair.Add(xd10x, xd10y);
            LineItem pld10 = myPane.AddCurve("D10", D10pair, Color.Green, SymbolType.None);
            pld10.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;

            TextObj text2 = new TextObj("ÇAKIL", 19, 5, CoordType.AxisXYScale, AlignH.Center, AlignV.Center);
            TextObj text3 = new TextObj("KUM", .75, 5, CoordType.AxisXYScale, AlignH.Center, AlignV.Center);
            TextObj text4 = new TextObj("İNCE", 0.025, 5, CoordType.AxisXYScale, AlignH.Center, AlignV.Center);
            text2.ZOrder = ZOrder.A_InFront;
            text3.ZOrder = ZOrder.A_InFront;
            text4.ZOrder = ZOrder.A_InFront;
            text2.FontSpec.Border.IsVisible = false;
            text3.FontSpec.Border.IsVisible = false;
            text4.FontSpec.Border.IsVisible = false;
            zedGraphControl1.GraphPane.GraphObjList.Add(text2);
            zedGraphControl1.GraphPane.GraphObjList.Add(text3);
            zedGraphControl1.GraphPane.GraphObjList.Add(text4);

            myPane.Title.Text = "Gradasyon";
            myPane.Title.FontSpec.FontColor = System.Drawing.Color.White;
            myPane.Title.FontSpec.Size = 20f;
            myPane.Legend.IsVisible = false;

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl1.Refresh();
        }

        private void PI_grafik(PointPairList PIChartpair)
        {
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();

            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.PointValueFormat = "0.00";

            myPane.XAxis.Type = AxisType.Linear;
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 100;
            myPane.XAxis.Scale.IsReverse = false;
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MinorGrid.IsVisible = false;
            myPane.XAxis.Title.Text = "Likit Limit";

            myPane.YAxis.Type = AxisType.Linear;
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Scale.Max = 60;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.Title.Text = "Plastisite İndisi";

            TextObj tALine = new TextObj("Kil - Silt Ayrımı Hattı", 88, 52);
            tALine.FontSpec.Angle = 30;
            tALine.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(tALine);
            tALine.FontSpec.Border.IsVisible = false;
            tALine.FontSpec.Size = 15f;

            TextObj tULine = new TextObj("Plastisite Üst Limit Hattı", 58, 47);
            tULine.FontSpec.Angle = 36;
            tULine.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(tULine);
            tULine.FontSpec.Border.IsVisible = false;
            tULine.FontSpec.Size = 15f;

            TextObj tCH = new TextObj("CH", 60, 38);
            tCH.ZOrder = ZOrder.F_BehindGrid;
            tCH.FontSpec.Size = 15F;
            tCH.FontSpec.IsBold = true;
            zedGraphControl1.GraphPane.GraphObjList.Add(tCH);
            tCH.FontSpec.Border.IsVisible = false;

            TextObj tCL = new TextObj("CL", 40, 22);
            tCL.ZOrder = ZOrder.F_BehindGrid;
            tCL.FontSpec.Size = 15F;
            tCL.FontSpec.IsBold = true;
            zedGraphControl1.GraphPane.GraphObjList.Add(tCL);
            tCL.FontSpec.Border.IsVisible = false;

            TextObj tMLOL = new TextObj("ML / OL", 40, 5);
            tMLOL.ZOrder = ZOrder.F_BehindGrid;
            tMLOL.FontSpec.Size = 15F;
            tMLOL.FontSpec.IsBold = true;
            zedGraphControl1.GraphPane.GraphObjList.Add(tMLOL);
            tMLOL.FontSpec.Border.IsVisible = false;

            TextObj tMHOH = new TextObj("MH / OH", 60, 5);
            tMHOH.ZOrder = ZOrder.F_BehindGrid;
            tMHOH.FontSpec.Size = 15F;
            tMHOH.FontSpec.IsBold = true;
            zedGraphControl1.GraphPane.GraphObjList.Add(tMHOH);
            tMHOH.FontSpec.Border.IsVisible = false;

            TextObj tCLML = new TextObj("CL-ML", 20, 5.6);
            tCLML.ZOrder = ZOrder.F_BehindGrid;
            tCLML.FontSpec.Size = 12F;
            tCLML.FontSpec.IsBold = true;
            tCLML.FontSpec.Border.IsVisible = false;
            zedGraphControl1.GraphPane.GraphObjList.Add(tCLML);

            TextObj tML2 = new TextObj("ML", 17, 2);
            tML2.ZOrder = ZOrder.F_BehindGrid;
            tML2.FontSpec.Size = 12F;
            tML2.FontSpec.IsBold = true;
            tML2.FontSpec.Border.IsVisible = false;
            zedGraphControl1.GraphPane.GraphObjList.Add(tML2);

            TextObj tLL = new TextObj("Düşük(L)\nPlastisite", 40, 63);
            tLL.FontSpec.Border.IsVisible = false;
            tLL.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(tLL);
            TextObj tPI = new TextObj("Yüksek(H)\nPlastisite", 60, 63);
            tPI.FontSpec.Border.IsVisible = false;
            tPI.ZOrder = ZOrder.F_BehindGrid;
            zedGraphControl1.GraphPane.GraphObjList.Add(tPI);

            LineItem LLCurve = myPane.AddCurve("Likit Limit - Plastik Indisi", PIChartpair, Color.Blue, SymbolType.Circle);
            LLCurve.Line.Width = 2.0F;
            LLCurve.Symbol.Size = 10f;
            LLCurve.Symbol.Fill.Color = System.Drawing.Color.Blue;
            LLCurve.Symbol.Border.IsVisible = false;
            LLCurve.Symbol.Fill.IsVisible = true;

            double[] y60 = new double[] { 0, 37.8 };
            double[] x50 = new double[] { 50, 50 };
            PointPairList half = new PointPairList(x50, y60);
            LineItem halfc = myPane.AddCurve("Düşük - Yüksek Plastisite Ayrım Hattı", half, Color.Blue, SymbolType.None);
            halfc.Line.Width = 2.0F;
            halfc.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;

            //A -> PI = 0.73 * (LL - 20)
            double[] Ay = new double[] { 0, 58.4 }; // LL = 100 iken PI = 58.4
            double[] Ax = new double[] { 20, 100 }; // PI = 0 iken LL = 20
            PointPairList Apair = new PointPairList(Ax, Ay);
            LineItem ALine = myPane.AddCurve("Kil - Silt Ayrım Hattı", Apair, Color.Green, SymbolType.None);
            ALine.Line.Width = 2.0F;
            ALine.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;

            //U -> PI = 0.9 * (LL - 8)
            double[] Uy = new double[] { 0, 82.8 }; // LL = 100 iken PI = 82.8
            double[] Ux = new double[] { 8, 100 }; // PI = 0 iken LL = 8
            PointPairList Upair = new PointPairList(Ux, Uy);
            LineItem ULine = myPane.AddCurve("Plastisite Üst Limit Hattı", Upair, Color.Red, SymbolType.None);
            ULine.Line.Width = 2.0F;
            ULine.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;

            //A -> PI = 0.73 * (LL - 20)
            //U -> PI = 0.9 * (LL - 8)
            double[] AU4y = new double[] { 4, 4 };
            double[] AU4x = new double[] { 12.4, 25.5 };
            PointPairList AU4pair = new PointPairList(AU4x, AU4y);
            LineItem AU4Line = myPane.AddCurve("CL-ML ikili sembol", AU4pair, Color.Black, SymbolType.None);
            AU4Line.Line.Width = 2.0F;
            AU4Line.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;

            double[] AU7y = new double[] { 7, 7 };
            double[] AU7x = new double[] { 15.8, 30 };
            PointPairList AU7pair = new PointPairList(AU7x, AU7y);
            LineItem AU7Line = myPane.AddCurve("CL-ML ikili sembol", AU7pair, Color.Black, SymbolType.None);
            AU7Line.Line.Width = 2.0F;
            AU7Line.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;

            myPane.Title.Text = "PI Kart";
            myPane.Title.FontSpec.FontColor = System.Drawing.Color.White;
            myPane.Title.FontSpec.Size = 15f;
            myPane.Legend.IsVisible = false;

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl1.Refresh();
        }

        private void zedGraphControl1_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            foreach (ToolStripMenuItem item in menuStrip.Items)
                if ((string)item.Tag == "set_default")
                {
                    menuStrip.Items.Remove(item);
                    break;
                }
        }

        private string zedGraphControl1_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            pointGoster.Text = "( " + Math.Round(curve[iPt].X, 2).ToString() + " , " + Math.Round(curve[iPt].Y, 2).ToString() +" ) " + curve.Label.Text; 
            return "";
        }

        private string isimhakki()
        {
            char[] xCh = new char[13];
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            xCh[0] = '\u004D';
            xCh[1] = '\u0045';
            xCh[2] = '\u0048';
            xCh[3] = '\u004D';
            xCh[4] = '\u0045';
            xCh[5] = '\u0054';
            xCh[6] = ' '; //
            xCh[7] = '\u0044';
            xCh[8] = '\u0055';
            xCh[9] = '\u0052';
            xCh[10] = '\u004D';
            xCh[11] = '\u0041';
            xCh[12] = '\u005A';

            foreach (char c in xCh)
                sb.Append(c);
            return sb.ToString();
        }

        private void sifirla(bool text)
        {
            txt_aashtosinif.Text = "-";
            txt_birlesiksinif.Text = "-";
            txt_ince.Text = "-";
            txt_aciklama.Text = "-";
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();
            gradPL.Clear();
            PIChartpair.Clear();
            D60pair.Clear();
            D30pair.Clear();
            D10pair.Clear();
            d60 = 0;
            d30 = 0;
            d10 = 0;
            cu = 0;
            cc = 0;
            GI = 0;
            zedGraphControl1.Refresh();
            if (text)
            {
                textBoxAll_temizle();
                e75.Focus();
            }
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

    }
}
