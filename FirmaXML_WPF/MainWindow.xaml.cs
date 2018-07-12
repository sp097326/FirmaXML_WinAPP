using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Signing.library;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace FirmaXML_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void signBtn_Click(object sender, RoutedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            opensslkeycsharp libssl = new opensslkeycsharp();

            //string cadenaOriginal = "161200302222|Renovación de Certificado Digital|26-12-2016 12:59:57|MAHA7710074X8|ALBERTO AUGUSTO MANZANILLA HERNANDEZ | 00001000000404657715 / 00001000000203446715 | MIIBCgKCAQEAtsht8BjqegtDjgwdn38RtvZsGNyzZOd6mU / nj4W5F1JaDj + KCEdVkBVQ2PNhyQt8JbuOEPS0gHVHLG2QbBEEilYHYNSUeomPpm0dg2RLVr / 1NMsXH2rVYBW2SfMdo + pu70m5 +/ JO1naBnOksJXkjgfAeQe9rHyCy84vKSxengQ8ECmrr7ZT6azmfm";
            string cadenaOriginal = "<xlm><node1>Alberto Manzanilla</node1></xml>";


            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Read, txtCerPath.Text);
            permission.Assert();


            try
            {
                Stream s = File.OpenRead(txtCerPath.Text);
            }
            catch (Exception ex)
            {
                txtResultMessages.Text = ex.Message + " : " + ex.StackTrace;
                Console.WriteLine(ex.StackTrace);
                return;
            }

            if (txtCerPath.Text != string.Empty && txtCerPath.Text != "Ruta al .cer" &&
                txtKeyPath.Text != string.Empty && txtKeyPath.Text != "Ruta al .key")
            {
                if (File.Exists(txtCerPath.Text))
                {
                    Console.Write("File exists");
                }
                string Certificate = string.Empty;
                string CertificateNumber = string.Empty;
                libssl.CertificateData(@txtCerPath.Text, out Certificate, out CertificateNumber);

                try
                {
                    //get XML document and sign it
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.PreserveWhitespace = true;
                    xmldoc.Load(txtXMLFile.Text);
                    string signedString = libssl.SignString(@txtKeyPath.Text, "..L3oyS4m..", xmldoc.InnerXml);
                    txtResultMessages.Text = "Original String: " + xmldoc.InnerXml + " \nSigned string: " + signedString + " \nCertificate data: " + Certificate + " | Certificate number: " + CertificateNumber;
                }
                catch (XmlException xmle)
                {
                    txtResultMessages.Text = "Problem while loading xml file: " + xmle.Message;
                }
                //txtResultMessages.Text= "Signed String:" + libssl.SignString(txtKeyPath.Text, "..S4myL3o..", cadenaOriginal);

                return;
            }
        }

        private void pickCertBtn_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            txtCerPath.Text = string.Empty;
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Title = "Pick your .cer File";
            fileDialog.Filter = "Archivos de certificado (*.cer) | *.cer | Archivos de texto (*.txt) | *.txt | Todos los archivos (*.*) | *.*";
            var result = fileDialog.ShowDialog();
            switch(result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    txtCerPath.Text = fileDialog.FileName;
                    txtCerPath.ToolTip = fileDialog.FileName;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    txtCerPath.Text = "Operation Cancelled";
                    txtCerPath.ToolTip = "cancelled";
                    break;
                default:
                    txtCerPath.Text = String.Empty;
                    break;
            }

        }

        private void pickKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            txtKeyPath.Text = string.Empty;
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Title = "Pick your .key File";
            fileDialog.Filter = "Archivos de llave (*.key) | *.key | Archivos de texto (*.txt) | *.txt | Todos los archivos (*.*) | *.*";
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    txtKeyPath.Text = fileDialog.FileName;
                    txtKeyPath.ToolTip = fileDialog.FileName;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    txtKeyPath.Text = "Operation Cancelled";
                    txtKeyPath.ToolTip = "cancelled";
                    break;
                default:
                    txtKeyPath.Text = String.Empty;
                    break;
            }
        }

        //using code
        public void Test()
        {
            opensslkeycsharp libssl = new opensslkeycsharp();
            string SignedString = libssl.SignString(@"c:\fiel\aaaa121213123123aaa_t.key",
              "0123456789", "||3.2|test|cadena|original|");
            string Certificate = "";
            string CertificateNumber = "";
            libssl.CertificateData(@"c:\fiel\aaaa121213123123aaa_t.cer", out Certificate, out CertificateNumber);
        }

        private void pickXMLBtn_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            txtXMLFile.Text = string.Empty;
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Title = "Pick your .xml File";
            fileDialog.Filter = "Archivos XML (*.xml) | *.xml | Archivos de texto (*.txt) | *.txt | Todos los archivos (*.*) | *.*";
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    txtXMLFile.Text = fileDialog.FileName;
                    txtXMLFile.ToolTip = fileDialog.FileName;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    txtXMLFile.Text = "Operation Cancelled";
                    txtXMLFile.ToolTip = "cancelled";
                    break;
                default:
                    txtXMLFile.Text = String.Empty;
                    break;
            }
        }
    }
}
