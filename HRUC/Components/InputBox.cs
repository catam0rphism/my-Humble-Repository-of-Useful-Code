using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRUC.Components
{
    public partial class InputBox : UserControl
    {
        public InputBox()
        {
            InitializeComponent();
            //Resize += (s, ea) => setTextBoxSizeAndLocation();
            //label.TextChanged += (s, ea) => setTextBoxSizeAndLocation();

            textBox.TextChanged += (s, ea) =>
                {
                    EventHandler temp = TextChanged;
                    if (temp != null)
                    {
                        temp(s, ea);
                    }
                };

        }

        private void setTextBoxSizeAndLocation()
        {
            const int padding = 3;
            textBox.Width = this.Width - label.Width - padding;
            textBox.Location = new Point(label.Width + padding, 0);
        }
        public int TextBoxWidth
        {
            get { return textBox.Width; }
            set
            {
                textBox.Width = value;
                textBox.Location = new Point(this.Width - value, 0);
            }
        }
        public string LabelText
        {
            get { return label.Text; }
            set
            {
                label.Text = value;
                setTextBoxSizeAndLocation();
            }
        }
        public string ValueString
        {
            get { return textBox.Text;}
            set { textBox.Text = value; }
        }
        public int? ValueInt
        {
            get
            {
                int i;
                if (int.TryParse(textBox.Text, out i))
                {
                    return i;
                }
                else { return null; }
            }
            set
            {
                if (value.HasValue)
                {
                    textBox.Text = value.Value.ToString();
                }
                else { textBox.Text = ""; }
            }
        }
        public double? ValueDouble
        {
            get
            {
                double i;
                if (double.TryParse(textBox.Text, out i))
                {
                    return i;
                }
                else { return null; }
            }
            set
            {
                textBox.Text = value.ToString();
            }
        }

        public T getValue<T>(Func<String,T> converter, Predicate<T> validator) where T: class {
            T foo = converter(textBox.Text);
            if (validator(foo))
                return foo;
            else return null;
        }

        public new event EventHandler TextChanged;
    }
}
