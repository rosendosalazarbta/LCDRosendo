using System;

namespace ConsoleLCD
{
    public class Client{
        private string Rfc;
        private string ValidezObligaciones;
        private string EstatusCertificado;
        private string noCertificado;
        private string FechaFinal;
        private string FechaInicio;

        public Client(string rfc, string validezobl, string status, string ncertificado, string finicio, string ffinal){
            Rfc = rfc;
            ValidezObligaciones = validezobl;
            EstatusCertificado = status;
            noCertificado = ncertificado;
            FechaInicio = finicio;
            FechaFinal = ffinal;
        }
        public String getRfc() { return Rfc; }
        public String getValidezObligaciones() { return ValidezObligaciones; }
        public String getEstatusCertificado() { return EstatusCertificado; }
        public String getnoCertificado() { return noCertificado; }
        public String getFechaFinal() { return FechaInicio; }
        public String getFechaInicio() { return FechaInicio; }
    }
}
