using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TranslateApp
{
    public partial class Form1 : Form
    {
        // SQL bağlantı dizesi
        string connectionString = "Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;Trusted_Connection=True;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Dillerin ComboBox'lara eklenmesi
            cmbSourceLanguage.Items.AddRange(new string[] { "Türkçe", "İngilizce", "Almanca" });
            cmbTargetLanguage.Items.AddRange(new string[] { "Türkçe", "İngilizce", "Almanca" });

            // Verileri yükleme
            LoadTranslations();
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text) || cmbSourceLanguage.SelectedItem == null || cmbTargetLanguage.SelectedItem == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Çeviri işlemi (örnek çeviri: sadece ters çeviriyor)
            txtOutput.Text = ReverseText(txtInput.Text);

            // Çeviriyi SQL'e kaydet
            SaveTranslation(txtInput.Text, txtOutput.Text, cmbTargetLanguage.SelectedItem.ToString());

            // Verileri yeniden yükle
            LoadTranslations();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtInput.Clear();
            txtOutput.Clear();
            cmbSourceLanguage.SelectedIndex = -1;
            cmbTargetLanguage.SelectedIndex = -1;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadTranslations()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM Translations";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veriler yüklenirken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void SaveTranslation(string sourceText, string translatedText, string language)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Translations (SourceText, TranslatedText, Language) VALUES (@SourceText, @TranslatedText, @Language)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SourceText", sourceText);
                    cmd.Parameters.AddWithValue("@TranslatedText", translatedText);
                    cmd.Parameters.AddWithValue("@Language", language);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Çeviri kaydedilirken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private string ReverseText(string text)
        {
            // Basit bir örnek çeviri işlemi (metni ters çevirme)
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
