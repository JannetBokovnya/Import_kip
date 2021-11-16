using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ImportKIP.Business
{
    public class BoundedKip
    {
         public string KeyDB { get; private set; }
        public string KeyFile { get; private set; }
        public string CNAME { get; private set; }
        public string NKM { get; private set; }
        public string CType { get; private set; }
        public string Ckipnum { get; private set; }
        public string NKMF { get; private set; }
        public string Nu_tz { get; private set; }
        public string Nu_pol { get; private set; }
        public string Ccomment { get; private set; }

        public BoundedKip(string keyDB, string cNAME, string nKM, string cType, 
                          string keyFile, string ckipnum, string nKMF, string nu_tz, string nu_pol, string ccomment)
        {
            KeyDB = keyDB;
            CNAME = cNAME;
            NKM = nKM;
            CType = cType;
            Ckipnum = ckipnum;
            NKMF = nKMF;
            Nu_tz = nu_tz;
            KeyFile = keyFile;
            Nu_pol = nu_pol;
            Ccomment = ccomment;
           

        }

     
    }
}
