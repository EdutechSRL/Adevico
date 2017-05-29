using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileTransfer.WinTest.ServiceReference1;
using System.IO;
using FileTransfer.Core;


namespace FileTransfer.WinTest
{
    public partial class FormMain : Form
    {
        
        public FormMain()
        {
            InitializeComponent();
        }        

        private void Form1_Load(object sender, EventArgs e)
        {
            if (txtPath.Text != "" && Directory.Exists(txtPath.Text))
            {
                listBox1.Items.Clear();
                DirectoryInfo d = new DirectoryInfo(txtPath.Text);
                FileInfo[] files = d.GetFiles("*.*", SearchOption.AllDirectories);

                listBox1.Items.AddRange(files);
            }

        }

        

        private void button1_Click(object sender, EventArgs e)
        {


            FileMQServiceClient servicetransfer = new FileMQServiceClient();

            servicetransfer.InnerChannel.Opening += InnerChannel_Opening;
            servicetransfer.InnerChannel.Opened += InnerChannel_Opened;
            servicetransfer.InnerChannel.Closing += InnerChannel_Closing;
            servicetransfer.InnerChannel.Closed += InnerChannel_Closed;
            servicetransfer.InnerChannel.Faulted += InnerChannel_Faulted;
            servicetransfer.InnerChannel.UnknownMessageReceived += InnerChannel_UnknownMessageReceived;

            servicetransfer.Endpoint.Address = new System.ServiceModel.EndpointAddress(txtServerAddress.Text);

            List<FileTransferBase> list = new List<FileTransferBase>();

        

            String path = txtPath.Text;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);

            System.IO.FileInfo[] files = di.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly);        

            foreach (var item in files)
            {

                String dirname = item.DirectoryName;
                String filename = item.Name;

                if (item.Length <= 104857600) { 
                    FileTransferBase ftb1 = new FileTransferBase()
                    {
                        Path = dirname,
                        Filename = filename
                    };

                    list.Add(ftb1);
                }
            }

            var n = list.Count;

            servicetransfer.TransferAllFilesDirect("TestTransfer", list.Take(n).ToArray());

       
        }

        void InnerChannel_UnknownMessageReceived(object sender, System.ServiceModel.UnknownMessageReceivedEventArgs e)
        {
            StatusLabel.Text = "Unknow message received";
        }

        void InnerChannel_Faulted(object sender, EventArgs e)
        {
            StatusLabel.Text = "Faulted";
        }

        void InnerChannel_Closed(object sender, EventArgs e)
        {
            StatusLabel.Text = "Closed";
        }

        void InnerChannel_Closing(object sender, EventArgs e)
        {
            StatusLabel.Text = "Closing";
        }

        void InnerChannel_Opened(object sender, EventArgs e)
        {
            StatusLabel.Text = "Opened";
        }

        void InnerChannel_Opening(object sender, EventArgs e)
        {
            StatusLabel.Text = "Opening";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtPath.Text != "")
            {
                folderBrowserDialog1.SelectedPath = txtPath.Text;
            }

            if(folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK){
                listBox1.Items.Clear();
                txtPath.Text = folderBrowserDialog1.SelectedPath;

                DirectoryInfo d = new DirectoryInfo(txtPath.Text);
                FileInfo[] files = d.GetFiles("*.*", SearchOption.AllDirectories);

                listBox1.Items.AddRange(files);

            }                   
        }
    




       


    }
}
