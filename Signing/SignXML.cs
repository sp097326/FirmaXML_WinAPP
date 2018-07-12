using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace Signing.library
{
    class SignXML
    {
        private CspParameters cspParams = null;
        RSACryptoServiceProvider rsakey = null;
        public void init()
        {
            try
            {
                //Create a new CspParameters object to specify key container
                cspParams = new CspParameters();
                cspParams.KeyContainerName = "XML_DSIG_RSA_KEY";

                //Create a new RSA signing key and save it
                rsakey = new RSACryptoServiceProvider(cspParams);


            } catch (Exception e)
            {
                Console.WriteLine("Cannot init class..." + e.GetBaseException().Message);
            }
        }

        public string signDocument(string xmlText)
        {
            try
            {
                //checking parameter
                if (xmlText == null || xmlText.Equals(""))
                {
                    throw new ArgumentNullException("xml text cannot be null or empty");
                }
                
                //Create a new XML document. this does not belong here...
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.LoadXml(xmlText);

                //Create a signedXML object
                SignedXml signedXml = new SignedXml(xmlDoc);

                //add the key
                signedXml.SigningKey = rsakey;

                //create a reference to be signed
                Reference reference = new Reference();
                reference.Uri = "";

                //Add an enveloped transformation to the reference
                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(env);

                //Add reference to the signed xml object
                signedXml.AddReference(reference);

                //compute the signature
                signedXml.ComputeSignature();

                //Get and XML representation of the signature and save to an XmlElemnt Object
                XmlElement xmlDigitalSignature = signedXml.GetXml();

                //Append to the XML Doc.
                xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

                return xmlDoc.InnerXml;

            } catch (XmlException xmle)
            {
                Console.WriteLine("Cannot write the XML file: " + xmle.Message);
                return "Cannot write the XML file: " + xmle.Message;
            } catch (Exception e)
            {
                Console.WriteLine("Something went wrong: " + e.GetBaseException().StackTrace);
                return "Something went wrong: " + e.GetBaseException().StackTrace;
            }
         }


    }
}
