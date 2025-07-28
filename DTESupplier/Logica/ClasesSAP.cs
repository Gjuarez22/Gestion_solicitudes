using System.Data;
using Microsoft.Data.SqlClient;
using SAPbobsCOM;
using GestionSolicitud.HelperClasses;
using GestionSolicitud.Models;

namespace GestionSolicitud.DTESupplier.Logica
{
    public class ClasesSAP
    {
        private static Company oCompany = new CompanyClass();

        private static string usuarioSAP;

        private static string claveSAP;

        public static string id;

        public static int docnum;

        public static DataTable items_global = new DataTable();

        public static int sucursal_defecto;

        public static string userDB;

        public static string passDB;

        public static string entryid;

        public static string ITEMCODE;

        public static string CARDCODE;

        public static string CARDNAME;

        public static string NIT;

        public static string REGISTRO;

        public static string GIRO;

        public static string bodega_sucursal;

        public static string sucursal;

        public static int salir;

        public static string codigo_sucursal;

        public static int peticion_clave;
        private DbFlujosTestContext context;
        private readonly MiServicio _miServicio;

        public ClasesSAP(MiServicio miServicio)
        {
            _miServicio = miServicio;
        }

        public ClasesSAP(DbFlujosTestContext context)
        {
            this.context = context;
        }

        public string Conexionsap()
        {
            usuarioSAP = "manager";
            claveSAP = "resorte82";
            oCompany.language = BoSuppLangs.ln_Spanish_La;
            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2005;
            string constr = _miServicio.GetConnectionSting("SAP");
            string conexion1 = constr;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conexion1);
            string server = builder.DataSource;
            oCompany.Server = server;
            string catalogo = builder.InitialCatalog;
            oCompany.CompanyDB = catalogo;
            string usuario = builder.UserID;
            oCompany.DbUserName = usuario;
            string clave = builder.Password;
            oCompany.DbPassword = clave;
            oCompany.UserName = usuarioSAP;
            oCompany.Password = claveSAP;
            int r = oCompany.Connect();
            string resultado;
            if (r != 0)
            {
                oCompany.GetLastError(out r, out var sErrMsg);
                resultado = r + "|" + sErrMsg;
                desconectarSAP();
            }
            return resultado = r + "|" + string.Empty;
        }

        public static void desconectarSAP()
        {
            oCompany.Disconnect();
        }
    }

}
