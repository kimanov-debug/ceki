using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace weight
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (!TryParseInput(txtWeight.Text, out double weight))
            {
                ShowError("Zəhmət olmasa düzgün çəki daxil edin!");
                return;
            }

            if (!TryParseInput(txtHeight.Text, out double height))
            {
                ShowError("Zəhmət olmasa düzgün boy daxil edin!");
                return;
            }

            if (height > 3.0)
            {
                height /= 100.0;
            }

            double bmi = weight / Math.Pow(height, 2);
            button4.Text = $"BKİ: {bmi:F2}";

            var (status, risk, imageName) = EvaluateBmi(bmi);

            lblStatus.Text = $"Çəki Statusu: {status}";
            lblRisk.Text = $"Sağlamlıq Riski: {risk}";

            LoadStatusImage(imageName);
        }

        private bool TryParseInput(string input, out double value)
        {
            return double.TryParse(input, out value) && value > 0;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private (string status, string risk, string imageName) EvaluateBmi(double bmi)
        {
            return bmi switch
            {
                < 18.5 => ("Çəki azlığı (Arıq)", "İmmunitet zəifliyi, qida çatışmazlığı riski", "Underweight.png"),
                <= 24.9 => ("Normal çəki", "Minimum risk (İdeal status)", "normal.png"),
                <= 32.0 => ("Artıq çəki", "Ürək-damar və şəkər xəstəliyi riski artır", "Overweight.png"),
                <= 39.0 => ("I dərəcəli piylənmə", "Yüksək tibbi risk", "obez.png"),
                _ => ("III dərəcəli (Morbid) piylənmə", "Həyati təhlükəli risk", "morbid.jpg")
            };
        }

        private void LoadStatusImage(string imageName)
        {
            try
            {
                string imagePath = Path.Combine(Application.StartupPath, "Images", imageName);

                if (File.Exists(imagePath))
                {
                    using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        pictureBoxStatus.Image?.Dispose();
                        pictureBoxStatus.Image = Image.FromStream(stream);
                    }
                    pictureBoxStatus.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    ResetPicture();
                }
            }
            catch
            {
                ResetPicture();
            }
        }

        private void ResetPicture()
        {
            pictureBoxStatus.Image?.Dispose();
            pictureBoxStatus.Image = null;
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }
    }
}
