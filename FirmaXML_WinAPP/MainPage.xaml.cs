using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FirmaXML_WinAPP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            SignXML sign = new SignXML();
            sign.init();
            string xmlText = this.xmltxtbox.Text;
            if (resultXmlText.Text != null && resultXmlText.Text != string.Empty && resultXmlText.Text != "")
            {
                resultXmlText.Text = string.Empty;
            }

            resultXmlText.Text = sign.signDocument(xmlText);

        }

        private void listcertsbtn_Click(object sender, RoutedEventArgs e)
        {
            CertificateUtils utils = new CertificateUtils();
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }
            if (resultXmlText.Text != null && resultXmlText.Text != string.Empty && resultXmlText.Text != "")
            {
                resultXmlText.Text = string.Empty;
            }

            resultXmlText.Text = utils.listPersonalCertificates();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }
              
            opensslkeycsharp libssl = new opensslkeycsharp();

            string cadenaOriginal = "161200302222|Renovación de Certificado Digital|26-12-2016 12:59:57|MAHA7710074X8|ALBERTO AUGUSTO MANZANILLA HERNANDEZ | 00001000000404657715 / 00001000000203446715 | MIIBCgKCAQEAtsht8BjqegtDjgwdn38RtvZsGNyzZOd6mU / nj4W5F1JaDj + KCEdVkBVQ2PNhyQt8JbuOEPS0gHVHLG2QbBEEilYHYNSUeomPpm0dg2RLVr / 1NMsXH2rVYBW2SfMdo + pu70m5 +/ JO1naBnOksJXkjgfAeQe9rHyCy84vKSxengQ8ECmrr7ZT6azmfm";

            //string hardcodeFileRoute = @"D:\tempfolder\FIEL_MAHA7710074X8_20130412103407\Renovacion2017\Renovacion_FIEL_MAHA7710074X8_20161226130030\00001000000404657715.cer";
            string hardcodeFileRoute = @"C:\Users\alber\OneDrive\Documentos\ReunionesVarias.txt";
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Read, @txtCertRoute.Text);
            permission.Assert();

            string localFolder = ApplicationData.Current.LocalFolder.DisplayName;
            string roamingfolder = ApplicationData.Current.RoamingFolder.DisplayName;

            
            try
            {
                Stream s = File.OpenRead(txtCertRoute.Text);
            }
            catch (Exception ex)
            {
                resultXmlText.Text = ex.Message + " : " + ex.StackTrace;
                Console.WriteLine(ex.StackTrace);
                return;
            }

            if (File.Exists(hardcodeFileRoute)) {
                Console.Write("found file...");
            } else {
                resultXmlText.Text = "cannot open file, no acces? or non existen file";
                return;
            }
            

            if (txtCertRoute.Text != string.Empty && txtCertRoute.Text != "ruta al .cer" &&
                txtKeyRoute.Text != string.Empty && txtKeyRoute.Text != "ruta al .key")
            {
                if (File.Exists(txtCertRoute.Text)) {
                    Console.Write("File exists");
                }
                string signedString = libssl.SignString(@txtKeyRoute.Text, "..L3oyS4m..", cadenaOriginal);
                string Certificate = string.Empty;
                string CertificateNumber = string.Empty;
                libssl.CertificateData(@txtCertRoute.Text, out Certificate, out CertificateNumber);
                resultXmlText.Text = "Certificate data: " + Certificate + " | Certificate number: " + CertificateNumber;
                return;
            }

        }

        private async void pickCerbtn_ClickAsync(object sender, RoutedEventArgs e)
        {
                // Clear previous returned file name, if it exists, between iterations of this scenario
                txtCertRoute.Text = string.Empty;

                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".cer");
                openPicker.FileTypeFilter.Add(".txt");
                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    // Application now has read/write access to the picked file
                    txtCertRoute.Text = file.Path;
                }
                else
                {
                    txtCertRoute.Text = "Operation cancelled.";
                }
         }

        private async void pickKeyBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            txtKeyRoute.Text = string.Empty;

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".key");
            openPicker.FileTypeFilter.Add(".txt");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                txtKeyRoute.Text = file.Path;
            }
            else
            {
                txtKeyRoute.Text = "Operation cancelled.";
            }
        }
    }
}
