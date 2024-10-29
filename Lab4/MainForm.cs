using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public partial class MainForm : Form
    {
        private TextBox txtInput;
        private TextBox txtOutput;
        private ComboBox cmbMode;
        private Button btnEncrypt;
        private Button btnDecrypt;
        private Label lblInput;
        private Label lblOutput;
        private Label lblMode;
        private TextBox txtKey;
        private TextBox txtIV;
        private Label lblKey;
        private Label lblIV;
        private DESCryptoServiceProvider des;

        public MainForm()
        {
            InitializeComponent();
            cmbMode.DataSource = Enum.GetValues(typeof(CipherMode));
            cmbMode.SelectedItem = CipherMode.CBC;
            InitializeDES();

        }

        private void InitializeComponent()
        {
            txtInput = new TextBox();
            txtOutput = new TextBox();
            cmbMode = new ComboBox();
            btnEncrypt = new Button();
            btnDecrypt = new Button();
            lblInput = new Label();
            lblOutput = new Label();
            lblMode = new Label();
            txtKey = new TextBox();
            txtIV = new TextBox();
            lblKey = new Label();
            lblIV = new Label();
            SuspendLayout();
            // 
            // txtInput
            // 
            txtInput.Location = new Point(20, 40);
            txtInput.Multiline = true;
            txtInput.Name = "txtInput";
            txtInput.ScrollBars = ScrollBars.Vertical;
            txtInput.Size = new Size(540, 50);
            txtInput.TabIndex = 0;
            // 
            // txtOutput
            // 
            txtOutput.Font = new Font("Arial", 10F);
            txtOutput.Location = new Point(20, 120);
            txtOutput.Multiline = true;
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.ScrollBars = ScrollBars.Vertical;
            txtOutput.Size = new Size(540, 50);
            txtOutput.TabIndex = 1;
            // 
            // cmbMode
            // 
            cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMode.Location = new Point(20, 200);
            cmbMode.Name = "cmbMode";
            cmbMode.Size = new Size(200, 28);
            cmbMode.TabIndex = 2;
            // 
            // btnEncrypt
            // 
            btnEncrypt.Location = new Point(20, 300);
            btnEncrypt.Name = "btnEncrypt";
            btnEncrypt.Size = new Size(100, 30);
            btnEncrypt.TabIndex = 3;
            btnEncrypt.Text = "Encrypt";
            btnEncrypt.Click += btnEncrypt_Click;
            // 
            // btnDecrypt
            // 
            btnDecrypt.Location = new Point(140, 300);
            btnDecrypt.Name = "btnDecrypt";
            btnDecrypt.Size = new Size(100, 30);
            btnDecrypt.TabIndex = 4;
            btnDecrypt.Text = "Decrypt";
            btnDecrypt.Click += btnDecrypt_Click;
            // 
            // lblInput
            // 
            lblInput.Location = new Point(20, 20);
            lblInput.Name = "lblInput";
            lblInput.Size = new Size(100, 23);
            lblInput.TabIndex = 5;
            lblInput.Text = "Input Text:";
            // 
            // lblOutput
            // 
            lblOutput.Location = new Point(20, 100);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new Size(100, 23);
            lblOutput.TabIndex = 6;
            lblOutput.Text = "Output Text:";
            // 
            // lblMode
            // 
            lblMode.Location = new Point(20, 180);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(130, 23);
            lblMode.TabIndex = 7;
            lblMode.Text = "Encryption Mode:";
            // 
            // txtKey
            // 
            txtKey.Location = new Point(20, 250);
            txtKey.MaxLength = 8;
            txtKey.Name = "txtKey";
            txtKey.Size = new Size(200, 27);
            txtKey.TabIndex = 8;
            txtKey.Text = "ABCDEFGH";
            // 
            // txtIV
            // 
            txtIV.Location = new Point(250, 250);
            txtIV.MaxLength = 8;
            txtIV.Name = "txtIV";
            txtIV.Size = new Size(200, 27);
            txtIV.TabIndex = 9;
            txtIV.Text = "IJKLMNOP";
            // 
            // lblKey
            // 
            lblKey.Location = new Point(20, 230);
            lblKey.Name = "lblKey";
            lblKey.Size = new Size(143, 23);
            lblKey.TabIndex = 10;
            lblKey.Text = "Key (8 characters):";
            // 
            // lblIV
            // 
            lblIV.Location = new Point(250, 230);
            lblIV.Name = "lblIV";
            lblIV.Size = new Size(163, 23);
            lblIV.TabIndex = 11;
            lblIV.Text = "IV (8 characters):";
            // 
            // MainForm
            // 
            ClientSize = new Size(582, 353);
            Controls.Add(txtInput);
            Controls.Add(txtOutput);
            Controls.Add(cmbMode);
            Controls.Add(btnEncrypt);
            Controls.Add(btnDecrypt);
            Controls.Add(lblInput);
            Controls.Add(lblOutput);
            Controls.Add(lblMode);
            Controls.Add(txtKey);
            Controls.Add(txtIV);
            Controls.Add(lblKey);
            Controls.Add(lblIV);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DES Encryption Tool";
            ResumeLayout(false);
            PerformLayout();
        }

        private void InitializeDES()
        {
            des = new DESCryptoServiceProvider();
            UpdateDESParameters();
        }

        private void UpdateDESParameters()
        {
            try
            {
                if (txtKey.Text.Length != 8 || txtIV.Text.Length != 8)
                {
                    MessageBox.Show("Key and IV must be exactly 8 characters long!", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(txtKey.Text);
                des.IV = ASCIIEncoding.ASCII.GetBytes(txtIV.Text);
                des.Mode = (CipherMode)cmbMode.SelectedItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating parameters: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtInput.Text))
                {
                    MessageBox.Show("Please enter text to encrypt!", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdateDESParameters();
                string text = txtInput.Text;
                byte[] textBytes = ASCIIEncoding.ASCII.GetBytes(text);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(textBytes, 0, textBytes.Length);
                    cs.FlushFinalBlock();
                    txtOutput.Text = Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Encryption error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtInput.Text))
                {
                    MessageBox.Show("Please enter text to decrypt!", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdateDESParameters();
                byte[] encryptedBytes = Convert.FromBase64String(txtInput.Text);

                using (MemoryStream ms = new MemoryStream(encryptedBytes))
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(cs))
                {
                    txtOutput.Text = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Decryption error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
