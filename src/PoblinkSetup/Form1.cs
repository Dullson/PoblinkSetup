using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Principal;
using Microsoft.Win32;

namespace PoblinkSetup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = $"Poblink Setup v{Assembly.GetEntryAssembly().GetName().Version}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var keyExists = UpdateSubKeyStatus();
            if (keyExists)
            {
                var command = Registry.ClassesRoot
                    .OpenSubKey("pob")
                    .OpenSubKey("shell")
                    .OpenSubKey("open")
                    .OpenSubKey("command")
                    .GetValue("") as string;
                textBoxExePath.Text = command.Split(' ').First();
            }

            if (textBoxExePath.Text == string.Empty)
            {
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var defaultExePath = $"{appDataFolder}\\Path of Building Community\\Path of Building.exe";
                if (File.Exists(defaultExePath))
                {
                    textBoxExePath.Text = defaultExePath;
                }
            }
        }

        private bool isElevated()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void restartElevated()
        {
            // Restart program and run as admin
                var exeName = Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Verb = "runas";
                Process.Start(startInfo);
                Application.Exit();
        }

        private bool CheckElevated()
        {
            if (!isElevated())
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Looks like you are running the application without required permissions. Click OK to restart as the Administrator.",
                    "Permission error",
                    MessageBoxButtons.OKCancel
                    );
                if (dialogResult == DialogResult.OK)
                {
                    restartElevated();
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        private bool UpdateSubKeyStatus()
        {
            RegistryKey rk = Registry.ClassesRoot;
            var keyExists = rk.GetSubKeyNames().Contains("pob");
            if (keyExists)
            {
                labelStatus.Text = "Registry found";
                labelStatus.ForeColor = Color.DarkGreen;
            }
            else
            {
                labelStatus.Text = "Registry not found";
                labelStatus.ForeColor = Color.DarkRed;
            }

            return keyExists;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckElevated()) 
                return;
            var execPath = textBoxExePath.Text.Trim();
            AddKeyGroup(execPath);
            UpdateSubKeyStatus();
        }

        private void AddKeyGroup(string executablePath)
        {
            RegistryKey rk = Registry.ClassesRoot;
            RegistryKey pobKey = rk.CreateSubKey("pob");
            pobKey.SetValue("URL Protocol", "");
            pobKey.SetValue("", "Poblink Protocol");
            var shellKey = pobKey.CreateSubKey("shell");
            var openKey = shellKey.CreateSubKey("open");
            var commandKey = openKey.CreateSubKey("command");
            commandKey.SetValue("", $"\"{executablePath.Trim('"')}\" \"%1\"");
        }

        private void removeKeyGroup()
        {
            Registry.ClassesRoot.DeleteSubKeyTree("pob");
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (!CheckElevated()) 
                return;
            removeKeyGroup();
            UpdateSubKeyStatus();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = "c:\\";
                dialog.Filter = @"Path of Building.exe (*.exe)|*.*";
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxExePath.Text = dialog.FileName;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Dullson/PoblinkSetup");
        }
    }
}