using System.Windows.Forms;

namespace Gui_test_modul
{
    public class DisplayControl : Label
    {
        private Register _Register = null;
        public DisplayControl()
        {
            this.AutoSize = false;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Lime;
            this.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Label.CheckForIllegalCrossThreadCalls = false;
        }

        protected override void InitLayout()
        {
            this.AutoSize = false;
            base.InitLayout();
        }

        public Register Register
        {
            get { return _Register; }
            set
            {
                _Register = value;
                if (_Register == null) return;
                if (this.DataBindings.Count > 0)
                    this.DataBindings.Clear();
                this.DataBindings.Add("Text", _Register, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }


    }
}
