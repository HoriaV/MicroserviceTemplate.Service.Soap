using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroserviceTemplate.Service.Models.Request
{
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = true)]
    public partial class Envelope
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Header Header { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Body Body { get; set; }
    }

    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class Header
    {
        private string soapTestStringField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string TestData
        {
            get
            {
                return soapTestStringField;
            }
            set
            {
                soapTestStringField = value;
            }
        }

    }

    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class Body
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/")]
        public ExtractData ExtractData { get; set; }
    }

    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://tempuri.org/")]
    public partial class ExtractData
    {
        //private string soapTestStringField;
        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        //public string TestData
        //{
        //    get
        //    {
        //        return soapTestStringField;
        //    }
        //    set
        //    {
        //        soapTestStringField = value;
        //    }
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://schemas.datacontract.org/2004/07/WcfService1")]
        public SoapTestData SoapTestData { get; set; }
    }

}