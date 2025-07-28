using System;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAPbobsCOM;
using GestionSolicitud.HelperClasses;

namespace GestionSolicitud.HelperClasses
{

    public class SAPService : IDisposable
    {
        private Company _sapCompany;

        private readonly IConfiguration _configuration;

        private readonly ILogger<SAPService> _logger;

        public SAPService(IConfiguration configuration, ILogger<SAPService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            ConnectToSAP();
        }

        private void ConnectToSAP()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_configuration["ConnectionStrings:SAP"]);
                _sapCompany = new CompanyClass
                {
                    language = BoSuppLangs.ln_Spanish_La,
                    DbServerType = (BoDataServerTypes)Enum.Parse(typeof(BoDataServerTypes), _configuration["SAP:DbServerType"]),
                    CompanyDB = _configuration["SAP:CompanyDB"],
                    Server = _configuration["SAP:Server"],
                    DbUserName = _configuration["SAP:DBUserName"],
                    DbPassword = _configuration["SAP:DBPassword"],
                    UserName = _configuration["SAP:UserName"],
                    Password = _configuration["SAP:Password"]
                };
                if (_sapCompany.Connect() != 0)
                {
                    string error = _sapCompany.GetLastErrorDescription();
                    _logger.LogError("Error connecting to SAP: " + error);
                    throw new Exception("SAP Connection Error: " + error);
                }
                _logger.LogInformation("Connected to SAP successfully");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error connecting to SAP");
                throw;
            }
        }

        public Company GetCompany()
        {
            if (_sapCompany == null || !_sapCompany.Connected)
            {
                ConnectToSAP();
            }
            return _sapCompany;
        }

        public void Dispose()
        {
            try
            {
                if (_sapCompany != null && _sapCompany.Connected)
                {
                    _sapCompany.Disconnect();
                    Marshal.ReleaseComObject(_sapCompany);
                    _sapCompany = null;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error disposing SAP connection");
            }
            GC.SuppressFinalize(this);
        }
    }
}
