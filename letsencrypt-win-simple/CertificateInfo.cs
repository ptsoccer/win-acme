﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace LetsEncrypt.ACME.Simple
{
    public class CertificateInfo
    {
        public X509Certificate2 Certificate { get; set; }
        public X509Store Store { get; set; }
        public FileInfo PfxFile { get; set; }

        public string SubjectName
        {
            get
            {
                return Certificate.Subject.Replace("CN=", "").Trim();
            }
        }

        public List<string> HostNames
        {
            get
            {
                var ret = new List<string>();
                foreach (var x in Certificate.Extensions)
                {
                    if (x.Oid.Value.Equals("2.5.29.17"))
                    {
                        AsnEncodedData asndata = new AsnEncodedData(x.Oid, x.RawData);
                        ret.AddRange(asndata.Format(true).Trim().Split('\n').Select(s => s.Replace("DNS Name=", "").Trim()));
                    }
                }
                return ret;
            }
        }
    }
}
