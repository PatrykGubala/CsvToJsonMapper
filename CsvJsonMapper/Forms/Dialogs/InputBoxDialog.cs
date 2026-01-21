namespace CsvJsonMapper.Forms.Dialogs
{
    public partial class InputBoxDialog : Form
    {
        public string Value { get { return txtValue.Text; } }

        public InputBoxDialog(string title, string prompt, string defaultValue = "")
        {
            InitializeComponent();
            this.Text = title;
            lblPrompt.Text = prompt;
            txtValue.Text = defaultValue;
            txtValue.Select();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}