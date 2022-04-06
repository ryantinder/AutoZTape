using System;
using System.Windows.Forms;
using System.Xml;
using System.Data;
namespace SettingsForm
{
    public partial class SettingsForm : Form
    {
        XmlDocument xml = new XmlDocument();

        public SettingsForm()
        {
            InitializeComponent();
            xml.Load("./SettingsForm.dll.config");

            StoreId_panel.SendToBack();
            StoreId_textBox.BorderStyle = BorderStyle.None;
            StoreId_textBox.Text = ReadKey("StoreId");

            label4.Visible = false;

            string store = ReadKey("Store");
            store = store.Substring(1, store.Length - 1);
            Store_textBox.Text = store;


            string comboLoadText = ReadKey("pointedTo");
            if (comboLoadText == "testDB")
            {
                targetPicker.SelectedIndex = 0;
            }
            else if (comboLoadText == "publicDev") {
                targetPicker.SelectedIndex = 1;
            }
            else
            {
                targetPicker.SelectedIndex = 2;
            }

            disableProgram.Checked = ReadKey("disableProgram") == "true" ? true : false;
            disableLivePush.Checked = ReadKey("disableLivePush") == "true" ? true : false;
            checkBox2.Checked = ReadKey("consoleReadKey()") == "true" ? true : false;

            string dateOverride = ReadKey("dateOverride");
            if (dateOverride == "false")
            {
                checkBox3.Checked = false;
                dateTimePicker1.Enabled = false;
            }
            else
            {
                checkBox3.Checked = true;
                dateTimePicker1.Enabled = true;
                dateTimePicker1.Value = DateTime.ParseExact(dateOverride, "yyyy-MM-dd", null);
            }

            disableMobileAPI.Checked = ReadKey("disableMobileAPI") == "true" ? true : false;
            disableAutoUpdate.Checked = ReadKey("disableAutoUpdate") == "true" ? true : false;
            checkConnections.Checked = ReadKey("testConnections") == "true" ? true : false;
            StoreId_textBox.Focus();
            StoreId_textBox.SelectAll();
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

        private void StoreID_TextChanged(object sender, EventArgs e)
        {
            string newID = StoreId_textBox.Text;
            WriteKey("StoreId", newID);
        }

        private void Store_TextChanged(object sender, EventArgs e)
        {
            string newStore = "#" + Store_textBox.Text;
            WriteKey("Store", newStore);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newTarget;

            switch (targetPicker.SelectedIndex)
            {
                case 0:
                    newTarget = "testDB";
                    break;
                case 1:
                    newTarget = "publicDev";
                    break;
                case 2:
                    newTarget = "liveTacomayo";
                    break;
                default:
                    newTarget = "publicDev";
                    break;
            }

            WriteKey("pointedTo", newTarget);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            WriteKey("disableLivePush", disableLivePush.Checked ? "true" : "false");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            WriteKey("consoleReadKey()", checkBox2.Checked ? "true" : "false");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                dateTimePicker1.Enabled = true;
                WriteKey("dateOverride", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                dateTimePicker1.Enabled = false;
                WriteKey("dateOverride", "false");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            WriteKey("dateOverride", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            WriteKey("disableMobileAPI", disableMobileAPI.Checked ? "true" : "false");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Visible = true;
        }

        private void button1_Release(object sender, EventArgs e)
        {
            label4.Visible = false;
        }
        private void disableProgram_CheckedChanged(object sender, EventArgs e)
        {
            WriteKey("disableProgram", disableProgram.Checked ? "true" : "false");
        }

        private void disableAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            WriteKey("disableAutoUpdate", disableAutoUpdate.Checked ? "true" : "false");
            
        }

        private void checkConnections_CheckedChanged(object sender, EventArgs e)
        {
            WriteKey("testConnections", checkConnections.Checked ? "true" : "false");
        }
    }

}
