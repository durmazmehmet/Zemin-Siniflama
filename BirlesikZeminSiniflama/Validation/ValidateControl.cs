using System;
using System.Windows.Forms;
using com.mehmetdurmaz.SoilClassfication.Globals.Definations;

namespace com.mehmetdurmaz.SoilClassfication.Validation
{
    class ValidateControl
    {
        private readonly Validator m_validator;
        private readonly TextBox m_currentControl;
        private readonly TextBox m_previousControl;

        private double m_currentValue;
        private double m_previousValue;


        public ValidateControl(Validator validator, Control currentControl, bool isNext = false)
        {
            m_validator = validator;
            m_currentControl = (TextBox)currentControl;
            var previousControl = Form.ActiveForm?.GetNextControl(currentControl, isNext);
            m_previousControl = previousControl is not TextBox ? null : (TextBox)previousControl;
        }

        private bool CurrentIsNumber() => double.TryParse(m_currentControl.Text, out m_currentValue);
        private bool PreviousIsNumber() => double.TryParse(m_previousControl.Text, out m_previousValue);

        private bool ValidateInRange()
        {
            if (!CurrentIsNumber()) return m_validator.ShowError(m_currentControl, Errormessage.NOT_A_NUMBER);

            if (m_previousControl != null && PreviousIsNumber() && m_previousValue < m_currentValue)
                return m_validator.ShowError(m_currentControl, Errormessage.INVALID_SEQUENCE);

            return true;
        }

        public bool ValidateGradation()
        {
            if (ValidateInRange())
            {
                if (m_currentValue == 0 && Math.Abs(m_previousValue - 100) < 0.01)
                    m_currentControl.Text = @"100";

                if (m_currentValue > 100 || m_currentValue < 0)
                    m_currentControl.Text = @"0";

                return true;
            }

            return false;
        }

        public bool ValidateConsistency()
        {
            if (!CurrentIsNumber()) return m_validator.ShowError(m_currentControl, Errormessage.NOT_A_NUMBER);

            if (!PreviousIsNumber()) return m_validator.ShowError(m_previousControl, Errormessage.NOT_A_NUMBER);

            if (m_currentValue == 0 && m_previousValue == 0)
                return true;

            if (m_currentValue.Equals(m_previousValue))
                return m_validator.ShowError(m_currentControl, Errormessage.NONPLASTIC_MUST_BE_ZERO, true);

            if (m_currentValue > 0 && m_previousValue == 0)
                return m_validator.ShowError(m_currentControl, Errormessage.NONPLASTIC_MUST_BE_ZERO, true);

            if (m_currentValue == 0 && m_previousValue > 0)
                return m_validator.ShowError(m_currentControl, Errormessage.NONPLASTIC_MUST_BE_ZERO, true);

            if (m_currentValue < m_previousValue)
                return m_validator.ShowError(m_previousControl, Errormessage.NONPLASTIC_PI_MUST_BE_LESS_THAN_LL, true);

            if (m_currentValue > 100)
                return m_validator.ShowError(m_previousControl, Errormessage.INVALID_RANGE, true);

            return true;
        }
    }
}
