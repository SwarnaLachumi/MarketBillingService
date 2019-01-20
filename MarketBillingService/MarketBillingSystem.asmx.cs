using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using MarketBillingService.DAL;


namespace MarketBillingService
{
    /// <summary>
    /// Summary description for MarketBillingSystem
    /// </summary>
    [WebService(Namespace = "http://billingsystem.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MarketBillingSystem : System.Web.Services.WebService
    {

        public AccessData AccessData = new AccessData();
       
        [WebMethod]
        public bool IsServiceAvailable()
        {
            return true;
        }
        [WebMethod]
        public DataTable ListProducts()
        {

            DataTable lstProductDetails = AccessData.GetProductdetails();
            return lstProductDetails;
        }   
        [WebMethod]
        public List<string> GetListInvalidItems(string strstrCartInput)
        {
            List<string> lstInvalidItems = AccessData.GetInvalidItemList(strstrCartInput);
            return lstInvalidItems;
        }
        [WebMethod]
        public List<string> GetListValidItems(string strstrCartInput)
        {
            List<string> lstValidItems = AccessData.GetValidItemList(strstrCartInput);
            return lstValidItems;

        }
        [WebMethod]
        public DataTable GetReceipt(string strstrCartInput)
        {
            DataTable lstProductDetails = AccessData.GetReceipt(strstrCartInput);
            return lstProductDetails;

        }
        [WebMethod]
        public string GetCostGrandTotal(string strstrCartInput)
        {
            string strGrandTotal = AccessData.GetCostGrandTotal(strstrCartInput);
            return strGrandTotal;

        }
        [WebMethod]
        public string UpdateProductDetails(string ItemId, string NumOfItemsOffer, string ItemsOfferPrice)
        {
            string Status = AccessData.UpdateProductDetails(ItemId, NumOfItemsOffer, ItemsOfferPrice);
            return Status;

        }
    }
}
