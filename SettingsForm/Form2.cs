using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SettingsForm
{
    public partial class Form2 : Form
    {
        XmlDocument xml = new XmlDocument();
        public Form2()
        {
            InitializeComponent();
            xml.Load("./SettingsForm.dll.config");

            paynumBox.Text = ReadKey("methodpaynums");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string newValue = paynumBox.Text;

            List<int> buffer = new List<int>();

            int i = (newValue.Length - newValue.Replace(",", "").Length);

            foreach (var str in newValue.Split(','))
            {
                if (int.TryParse(str.Trim(), out int o)) {
                    buffer.Add(o);
                }
                else
                {
                    MessageBox.Show("Invalid Pay Method Number");
                    return;
                }
            }

            string writeValue = string.Join(", ", buffer);

            WriteKey("methodpaynums", writeValue);
            

        }
        private void WriteKey(string key, string value)
        {
            XmlNodeList nodes = xml.SelectNodes("appSettings/add");
            foreach (XmlNode node in nodes)
            {
                XmlAttributeCollection nodeAtt = node.Attributes;
                if (nodeAtt["key"].Value.ToString() == key)
                {
                    XmlAttribute nValue = node.Attributes["value"];
                    nValue.Value = value;
                    xml.Save("./SettingsForm.dll.config");
                    return;
                }
            }
        }
        private string ReadKey(string key)
        {
            XmlNodeList nodes = xml.SelectNodes("appSettings/add");
            foreach (XmlNode node in nodes)
            {
                XmlAttributeCollection nodeAtt = node.Attributes;
                if (nodeAtt["key"].Value == key)
                {
                    return nodeAtt["value"].Value;
                }
            }
            return "Node not found";
        }

    }
}
