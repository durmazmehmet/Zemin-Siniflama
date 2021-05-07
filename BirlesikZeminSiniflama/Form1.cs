using com.mehmetdurmaz.SoilClassfication.Graph;
using com.mehmetdurmaz.SoilClassfication.SoilSpecs;
using com.mehmetdurmaz.SoilClassfication.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using com.mehmetdurmaz.SoilClassfication.Globals.Definations;
using com.mehmetdurmaz.SoilClassfication.Globals.Enums;
using com.mehmetdurmaz.SoilClassfication.Globals.Extensions;
using com.mehmetdurmaz.SoilClassfication.Graph.Builders;
using com.mehmetdurmaz.SoilClassfication.Graph.Layouts;
using com.mehmetdurmaz.SoilClassfication.SoilDescription;
using com.mehmetdurmaz.SoilClassfication.SoilIdentification;
using ZedGraph;
using static System.String;
using Label = System.Windows.Forms.Label;

namespace com.mehmetdurmaz.SoilClassfication
{
    public partial class UserUi : Form
    {
        private SoilSpec m_soilSpec;
        private List<Particle> m_currentSieveSet;

        private Classification m_currentSystem;
        private Sieveset m_sieveset;
        private Graphset m_graphset;

        private bool m_isFormClosing;
        private bool m_isCalculationInEffect;
        private bool m_isOrganic;

        private readonly Validator m_validator;
        private readonly List<Label> m_gradationLabels;
        private readonly List<TextBox> m_gradationTextBoxes;


        /// <summary>
        /// Gradasyon ve Kıvam girişlerinin yapıldığı textbox ve etiketleri bir listeye alır
        /// </summary>
        private void LoadControlsToList()
        {
            gradationPanel.Controls.OfType<TextBox>().ForEach(control => m_gradationTextBoxes.Add(control));
            gradationPanel.Controls.OfType<Label>().ForEach(control => m_gradationLabels.Add(control));

            m_gradationLabels.Sort((x, y) => Compare(x.Name, y.Name, StringComparison.Ordinal));
            m_gradationTextBoxes.Sort((x, y) => Compare(x.Name, y.Name, StringComparison.Ordinal));
        }


        /// <summary>
        /// Formu başlatır
        /// Gradasyon ve kıvam girişleir ve etiketlerini listeye alır
        /// Elek seti içeren CustomGradation sınıfını SieveSet referansına atar
        /// Validasyon kontrollerini aggregation olarak alan Validator sınıfını m_validator referansına atar
        /// m_gradation ve m_consistency üyelerine statick BLANKDATA sınıfından boş verinin referansını atar
        /// </summary>
        public UserUi()
        {
            InitializeComponent();

            m_gradationTextBoxes = new List<TextBox>();
            m_gradationLabels = new List<Label>();
            LoadControlsToList();
            m_validator = new Validator(errorProvider1, toolTip1);
            m_soilSpec = new SoilSpec();
        }

        /// <summary>
        /// Mevcut GradationSet içindeki bilgiye göre textboxların solundaki etiketleri yeniden düzenler         
        /// </summary>
        private void FillLabelsFromGradation()
        {
            for (int i = 0; i < m_currentSieveSet.Count; ++i)
                m_gradationLabels[i].Text = $@"{m_currentSieveSet[i].Size} {Desc.METRIC_MM}";
        }

        /// <summary>
        /// Verilen boş yada dolu gradasyon ve kıvam bilgilerine göre textboxları doldurur
        /// Dolu veri SelectedIndexChanged eventi ateşlendiğinde elek setinin değişmesinden dolayı gösterim değişliği ile gelebilir
        /// Boş veri Yeni düğmeisinin tıklanma eventi ateşlendiğinde olabilir
        /// </summary>
        /// <param name="soilSpec"></param>
        private void FillFromData(SoilSpec soilSpec)
        {
            for (int i = 0; i < m_currentSieveSet.Count; i++)
            {
                m_gradationTextBoxes[i].Text = Math.Round(soilSpec.Gradation.PassThroughFrom(m_currentSieveSet[i].Size), 2).ToString(CultureInfo.CurrentCulture);
            }
            eLL.Text = soilSpec.Consistency.LiquidLimit.ToString();
            ePI.Text = soilSpec.Consistency.PlasticityIdx.ToString();
        }

        /// <summary>
        /// Elek seti değiştirildiğinde verilen sete göre GradationSet'e SieveSet'deki listeyi yükler
        /// Grafiği sete göre tekrar ayarlar, 
        /// Gradasyon panelinde textboxları ve etiketlerini uygun sete göre yükler
        /// </summary>
        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            m_sieveset = comboStandartSelection.SelectedIndex == 0 ? Sieveset.Astm : Sieveset.Iso;
            m_currentSieveSet = new SieveSets(m_sieveset).Get();
            FillLabelsFromGradation();
            FillFromData(m_soilSpec);
            BuildGraph();
        }

        private Consistency CollectAndValidateConsistencyDataFromControls()
        {
            var vc = new ValidateControl(m_validator, eLL, true).ValidateConsistency();

            if (!vc)
                return new SoilSpec().Consistency;

            m_validator.Clear();

            var liquidLimit = Convert.ToInt32(Convert.ToDecimal(eLL.Text));
            var plasticityIndex = Convert.ToInt32(Convert.ToDecimal(ePI.Text));

            return new Consistency(liquidLimit, plasticityIndex, m_isOrganic);
        }

        private Gradation CollectAndServeGradationDataFromControls()
        {
            var result = new Gradation();

            for (var i = 0; i < m_currentSieveSet.Count; i++)
            {
                result.Add(m_currentSieveSet[i].Allias, m_currentSieveSet[i].Size, Convert.ToDouble(m_gradationTextBoxes[i].Text));
            }

            return result;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            zedGraphControl1.PointValueEvent += ZedGraphControl1_PointValueEvent;
            AppendEvents();
            comboStandartSelection.SelectedIndex = 0;
            NewCalculation();
        }

        /// <summary>
        /// Sonuç paneline "-" ile boş değer atanır
        /// gradasyon ve kıvam veri elemanlarına boş değer atanır
        /// boş değerler ile oluşturulan grafik boşaltılmış olur
        /// </summary>
        private void NewCalculation()
        {
            TXT_SEMBOL.Text = Desc.DASH;
            TXT_FINE.Text = Desc.DASH;
            TXT_DESC.Text = Desc.DASH;
            TXT_DETAILS.Text = Desc.DASH;
            FillFromData(new SoilSpec());
            BuildGraph();
        }

        /// <summary>
        /// Hesapkama esnasında çağrılan metodur.
        /// Hesaplama ateşlendikten sonra kullanıcının seçtiği elek setine göre girdiği verileri alır  
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns>
        /// userInput ile aldığı verilerinden Analitik interpolasyon ile (gradation sınıfının EstimateBySize metodu ile) 
        /// tüm bilinen elek büyüklüklerine göre 
        /// % geçen oranlarını içeren gradation ile döner
        /// </returns>
        private static Gradation CreateWholeSet(Gradation userInput)
        {
            var wholeParticles = new SieveSets(Sieveset.Whole).Get();
            wholeParticles.ForEach(p => p.Porpotion = p.Size <= 75 ? userInput.EstimateBySize(p.Size) : 100);
            return new Gradation(wholeParticles);
        }
        /// <summary>
        /// Düğmeler basılmak süreti ile seçilen zemin sınıflama sitemine göre 
        /// </summary>
        /// <returns>ISoilIdentificationBuilder döner</returns>
        private SoilDescriptionBuilder CreateDescription()
        {
            SoilIdentificationBuilder soilIdentificationBuilder;
            SoilDescriptionBuilder soilDescriptionBuilder;

            switch (m_currentSystem)
            {
                case Classification.Escs:
                    soilIdentificationBuilder = new IdentificateForEscs(m_soilSpec);
                    Identification.IdentificateFor(soilIdentificationBuilder);
                    soilDescriptionBuilder = new DescribeForEscs(soilIdentificationBuilder.DefinedSoil);
                    break;
                case Classification.Uscs:
                    soilIdentificationBuilder = new IdentificateForUscs(m_soilSpec);
                    Identification.IdentificateFor(soilIdentificationBuilder);
                    soilDescriptionBuilder = new DescribeForUscs(soilIdentificationBuilder.DefinedSoil);
                    break;
                case Classification.Aashto:
                    soilIdentificationBuilder = new IdentificateForAashto(m_soilSpec);
                    Identification.IdentificateFor(soilIdentificationBuilder);
                    soilDescriptionBuilder = new DescribeForAashto(soilIdentificationBuilder.DefinedSoil);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Description.DescribeFor(soilDescriptionBuilder);
            return soilDescriptionBuilder;
        }

        /// <summary>
        /// Builder metod ile ISoilIdentificationBuilder taban sınıfını miras almış sınıfları değerlerini 
        /// Identification ile set eder. (ilgili parametreleri hesaplar)
        /// Builder metod ile Identification'dan gelen veriler ile ISoilDescriptionBuilder taban sınıfını miras almış sınıfları değerlerini 
        /// Comment ile set eder. (hesaplanan parametrelere göre zemin sınıfına ilişkin açıklamaları ve sembollemeyi yapar)
        /// Sonuçları Panele yükler        
        /// </summary>
        private void FillPanel()
        {
            if (!m_isCalculationInEffect)
                return;

            SoilDescriptionBuilder result = CreateDescription();

            A_SCS.Text = result.DefinedSoil.SystemTitle;
            TXT_SEMBOL.Text = result.DefinedSoil.Symbol;
            TXT_FINE.Text = result.DefinedSoil.FineSymbol;
            TXT_DESC.Text = result.DefinedSoil.Comment.TrimStart();
            TXT_DETAILS.Text = result.ToString();
        }

        /// <summary>
        /// Gradasyon girişleri toplar, o girişlere göre bütün eleklerin kullanıldığı oranları hesaplatır
        /// kıvamı toplar ve hesaplatır.
        /// grafiği oluşturur
        /// paneli doldurur
        /// isCalculationInEffect ile hesaplanmanın yapıldığını teyit eder.
        /// </summary>
        private void Hesapla()
        {
            m_soilSpec = new SoilSpec(CreateWholeSet(CollectAndServeGradationDataFromControls()), CollectAndValidateConsistencyDataFromControls());
            BuildGraph();
            FillPanel();
            m_isCalculationInEffect = true;
        }

        /// <summary>
        ///  Form kapandığında validasyon ısrarını kapatır
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m) //Eğer form kapanacaksa validasyon tuzağını kapat
        {
            m_isFormClosing = m.Msg == 16;
            base.WndProc(ref m);
        }

        /// <summary>
        /// Validasyon için özel ayarlamaların olduğu eventleri textboxlara yükler
        /// </summary>
        private void AppendEvents()
        {
            void AppendCommonEventsTo(Control tb)
            {
                tb.Enter += TextBoxEnterEvent;
                tb.MouseDown += TextBoxEnterEvent;
                tb.KeyDown += TextBoxKeyEvents;
                tb.TextChanged += TextBoxChangedEvent;
            }
            void AppendToGradation(Control tb)
            {
                AppendCommonEventsTo(tb);
                tb.Validating += GradationValidation;
            }

            consistencyPanel.Controls.OfType<TextBox>().ForEach(AppendCommonEventsTo);
            gradationPanel.Controls.OfType<TextBox>().ForEach(AppendToGradation);
            comboStandartSelection.SelectedIndexChanged += SelectedIndexChanged;
        }

        /// <summary>
        /// Gradasyonda uygunsuz değer girilirse doğru değer girene kadar kullanıcıyı uyarır
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxKeyEvents(object sender, KeyEventArgs e)
        {
            void SupressEvent()
            {
                e.Handled = true; // tuşu basıldı say
                e.SuppressKeyPress = true;
            }
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Down:
                case Keys.Right:
                    if (ActiveControl != null)
                    {
                        SelectNextControl(ActiveControl, true, true, true, true);
                    }
                    SupressEvent();
                    break;
                case Keys.Up:
                case Keys.Left:
                    var onceki = GetNextControl(ActiveControl, false);
                    onceki?.Focus();
                    SupressEvent();
                    break;
            }
        }
        private static void TextBoxChangedEvent(object sender, EventArgs e) => ((TextBox)sender).Modified = true;
        private static void TextBoxEnterEvent(object sender, EventArgs e) => ((TextBox)sender).SelectAll();
        private string ZedGraphControl1_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt) 
            => pointGoster.Text = $@"( {Math.Round(curve[iPt].X, 2)} , {Math.Round(curve[iPt].Y, 2)} ) {curve.Label.Text}";
        private void Organik_CheckedChanged(object sender, EventArgs e) => m_isOrganic = organik.Checked;

        /// <summary>
        /// Gradasyonda uygunsuz değer girilirse validasyonu ateşler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GradationValidation(object sender, CancelEventArgs e)
        {
            if (m_isFormClosing)
                return;

            var currentControl = ((TextBox)sender);
            var validatedControl = new ValidateControl(m_validator, currentControl);

            if (!validatedControl.ValidateGradation())
            {
                e.Cancel = true;
                currentControl.SelectAll();
                return;
            }

            m_validator.Clear();
        }

        /// <summary>
        /// düğmelere tek bir event yüklenmiştir
        /// gelen düğme ismine göre yapılacak evente yönlendirir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonEvents(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case Buttonname.CALCULATE:
                    m_validator.Clear();
                    m_isCalculationInEffect = true;
                    Hesapla();
                    break;
                case Buttonname.ABOUTUS:
                    MessageBox.Show(Copyright.DISCLAIMER, Copyright.ABOUTUS, MessageBoxButtons.OK);
                    break;
                case Buttonname.NEW:
                    m_validator.Clear();
                    NewCalculation();
                    break;
                case Buttonname.SHOWGRADATIONGRAPH:
                    m_graphset = Graphset.Gradation;
                    BuildGraph();
                    break;
                case Buttonname.SHOWPIGRAPH:
                    m_graphset = Graphset.PlasticityIdx;
                    BuildGraph();
                    break;
                case Buttonname.USCS:
                    m_currentSystem = Classification.Uscs;
                    A_SCS.Text = Panelname.USCS_PANE_TITLE;
                    LABEL_SEMBOL.Text = Panelname.SCS_SEMBOL;
                    LABEL_FINE.Text = Panelname.SCS_FINE;
                    FillPanel();
                    BuildGraph();
                    break;
                case Buttonname.ESCS:
                    m_currentSystem = Classification.Escs;
                    A_SCS.Text = Panelname.ESCS_PANE_TITLE;
                    LABEL_SEMBOL.Text = Panelname.SCS_SEMBOL;
                    LABEL_FINE.Text = Panelname.SCS_FINE;
                    FillPanel();
                    BuildGraph();
                    break;
                case Buttonname.AASHTO:
                    m_currentSystem = Classification.Aashto;
                    A_SCS.Text = Panelname.AASHTO_PANE_TITLE;
                    LABEL_SEMBOL.Text = Panelname.AASHTO_SEMBOL;
                    LABEL_FINE.Text = Panelname.AASHTO_FINE;
                    FillPanel();
                    BuildGraph();
                    break;
            }
        }

        /// <summary>
        /// Grafik objesinin panelini GraphPane türünden myPane referansına atar
        /// Mevcut senaryoda o anda gradasyon textboxlardalarında hangi veri varsa onu toplar ve gradasyon verisi olarak alır
        /// Kıvam için buna gerek yoktur...
        /// BUILDER METOD İLE
        /// IGraphBuilder soyut sınıftan DrawGraph oluşturukur
        /// Eğer kullanıcı Gradasyon grafiğine bakmak istiyorsa ;
        /// ILayoutBuilder taban sınfından türetilmiş LayoutForGrad referansına ilgili standarda uygun myPane'i aggregation olarka alan layout nesnesi atanır
        /// DrawGraph referansına grafik objesinin referansı, oluşturulan LayoutForGrad ve seçilen sete göre gelen gradasyon verisini / kıvam limiti verisini parametre olarak alan 
        /// DrawBaseGradGraph sınıfı atanır.
        /// produceBase ile DrawGraph öğeleri set edilir.
        /// </summary>
        private void BuildGraph()
        {
            SoilSpec dataForGraph = new(CollectAndServeGradationDataFromControls(), m_soilSpec.Consistency);
            GraphBuilder drawGraph = new DrawBaseGraph(zedGraphControl1, dataForGraph);

            var isOrganic = m_soilSpec.Consistency.O;


            LayoutBuilder layoutGraph = m_currentSystem switch
            {
                Classification.Uscs => new LayoutForUscs(isOrganic) { MyPane = zedGraphControl1.GraphPane },
                Classification.Aashto => new LayoutForAashto() { MyPane = zedGraphControl1.GraphPane},
                _ => new LayoutForEscs(isOrganic) { MyPane = zedGraphControl1.GraphPane },
            };

            switch (m_graphset)
            {
                case Graphset.Gradation:
                    ProduceGraph.Grad(drawGraph, layoutGraph);
                    break;
                case Graphset.PlasticityIdx:
                    ProduceGraph.PlasticityIndex(drawGraph, layoutGraph);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

