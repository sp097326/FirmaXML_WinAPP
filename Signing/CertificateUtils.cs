using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


namespace Signing.library
{
    class CertificateUtils
    {
        public string listPersonalCertificates()
        {
            string certificateList = String.Empty;
            try
            {
                X509Store objStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                //if (!objStore.IsOpen)
                //{
                    objStore.Open(OpenFlags.ReadOnly);

                    foreach (X509Certificate2 objCert in objStore.Certificates)
                    {
                        certificateList += objCert.SubjectName.Name + ":" + objCert.Thumbprint + "\n";
                    }

                    objStore.Close();
                //}
                //else throw new Exception("Store already open by something/one else");
                return certificateList;
            } catch(Exception e)
            {
                return e.StackTrace;
            }
        }
    }
}
