using System.Windows.Forms;

namespace com.mehmetdurmaz.SoilClassfication.Validation
{
    public class Validator
    {
        private readonly ErrorProvider m_errorProvider;
        private readonly ToolTip m_toolTip;

        public Validator(ErrorProvider errorProvider, ToolTip toolTip)
        {
            m_errorProvider = errorProvider;
            m_toolTip = toolTip;
        }

        public bool ShowError(Control control, string message, bool shouldIFocus = false)
        {
            m_errorProvider.SetError(control, message);
            m_toolTip.Show(message, control);

            if (shouldIFocus)
                control.Focus();

            return false;
        }

        public void Clear()
        {
            m_errorProvider.Clear();
            m_toolTip.RemoveAll();
        }
    }
}