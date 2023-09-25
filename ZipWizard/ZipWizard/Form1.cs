using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace ZipWizard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string path;

        private void button1_Click(object sender, EventArgs e)
        {
            if (fileRad.Checked)
            {
                using (var openfileDialog = new OpenFileDialog())
                {
                    openfileDialog.Filter = "Tüm Dosyalar (*.*)|*.*";

                    if (openfileDialog.ShowDialog() == DialogResult.OK)
                    {
                        path = openfileDialog.FileName;
                        textBox1.Text = path;
                        textBox2.Text = path;
                    }
                }
            }
            else if (klasörRad.Checked)
            {
                using (var folderBrowserDialog = new FolderBrowserDialog())
                {
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        path = folderBrowserDialog.SelectedPath;
                        textBox1.Text = path;
                        textBox2.Text = path;
                    }
                }
            }         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Zip Dosyaları (*.zip)|*.zip";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog.FileName;
                    textBox3.Text = path;
                }
            }
        }

        private void CompressButton_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                string outputFile = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".zip");

                using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    archive.CreateEntry(path);
                }

                MessageBox.Show("Dosya başarıyla sıkıştırıldı.");
                path = "";
                textBox1.Text = "";
            }
        }

        private void ExtractButton_Click(object sender, EventArgs e)
        {
            if(textBox3.Text != "" && fileRad.Checked)
            {
                string outputPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));

                using (ZipArchive archive = ZipFile.OpenRead(path))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Path.Combine(outputPath, entry.FullName), true);
                    }
                }

                MessageBox.Show("Dosya başarıyla çıkarıldı.");
                textBox3.Text = "";
                path = "";
            }
            else if(textBox3.Text != "" && klasörRad.Checked)
            {              
                string hedefKlasor = Path.GetDirectoryName(path);

                ZipFile.ExtractToDirectory(path, hedefKlasor);

                MessageBox.Show("Klasör başarıyla çıkarıldı.");
            }
        }     
    }
}
