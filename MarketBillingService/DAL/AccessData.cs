using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MarketBillingService.DAL
{

    public class AccessData
    {

        ProductsDBDataContext db = new ProductsDBDataContext();
        DataTable ProductDetails = new DataTable();

        private List<string> objValidItems = new List<string>();
        private List<int> objValidNumOfUnits = new List<int>();
        private List<string> objInvalidItems = new List<string>();
        private List<Decimal> CostTotalUnitsItem = new List<Decimal>();

        public AccessData()
            {
            FillData();
            }
        private void FillData()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["ProductsDBConnectionString"].ConnectionString;
            string query = "select * from Products_details";
            SqlConnection Connection = new SqlConnection(strConnectionString);
            SqlCommand cmd = new SqlCommand(query, Connection);
            Connection.Open();

            ProductDetails.TableName = "Product_details";
            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ProductDetails);
            Connection.Close();
            da.Dispose();
          
        }
        public DataTable GetProductdetails()
        {

            return ProductDetails;
        }
        public void CalculateCost(string strCartDetails)
        {
            string strItemId = null;
            string strNumOfUnits = null;

            int nNumOfUnits = 0;
            int nValidTotalItems = 0;
            int nInvalidTotalItems = 0;


            bool isItemUnitNumeric = false;
            bool isItemIdValid = false;

            string[] arrCartDetails = strCartDetails.Split(',');

            int itrCartItems = 0;

            FillData();

            for (itrCartItems = 0; itrCartItems < arrCartDetails.Length; itrCartItems++)
            {

                if (arrCartDetails[itrCartItems].Contains('-'))
                {
                    strItemId = arrCartDetails[itrCartItems].Split('-')[0].Trim();
                    strNumOfUnits = arrCartDetails[itrCartItems].Split('-')[1].Trim();

                    isItemIdValid = ProductDetails.AsEnumerable().Select(r => r.Field<string>("SKU_PRODUCT_ID")).Contains(strItemId);
                    if (strNumOfUnits != "0")
                    {
                        isItemUnitNumeric = int.TryParse(strNumOfUnits, out nNumOfUnits);
                    }
                    else
                    {
                        isItemIdValid = false;
                    }
                }
                else
                {
                    isItemIdValid = false;
                }

                // If block holds Valid Product and else block holds Invalid Product.
                if (isItemUnitNumeric & isItemIdValid)
                {
                    nValidTotalItems = nValidTotalItems + 1;
                    objValidNumOfUnits.Add(nNumOfUnits);
                    objValidItems.Add(strItemId);
                }
                else
                {
                    nInvalidTotalItems = nInvalidTotalItems + 1;
                    objInvalidItems.Add(arrCartDetails[itrCartItems]);
                }
            }


            //Total cost of each product added to cart * number of units 
            if (nValidTotalItems > 0)
            {
                int itrValidItems = 0;
                Decimal decTotalItemCost = 0.0m;

                string strItemUnitPrice = null;
                string strNumOfItemsOffer = null;
                string strNumOfItemsOfferPrice = null;

                int nRemainder = 0;
                int nQuotient = 0;

                Decimal decItemPriceOnOffer = 0.0m;
                Decimal decItemFixedPrice = 0.0m;

                for (itrValidItems = 0; itrValidItems < objValidItems.Count(); itrValidItems++)
                {
                    strItemId = objValidItems[itrValidItems];
                    //Fetch Required columns from DataTable using ItemID and assign it to string

                    strItemUnitPrice = ProductDetails.AsEnumerable()
                                   .Where(r => r.Field<string>("SKU_PRODUCT_ID") == strItemId)
                                   .Select(s => s.Field<Decimal>("PRODUCT_UNIT_PRICE")).First().ToString();

                    strNumOfItemsOffer = ProductDetails.AsEnumerable()
                                                      .Where(r => r.Field<string>("SKU_PRODUCT_ID") == strItemId)
                                                      .Select(s => s.Field<int>("PRODUCT_COUNT_OFFER")).First().ToString();

                    strNumOfItemsOfferPrice = ProductDetails.AsEnumerable()
                                                       .Where(r => r.Field<string>("SKU_PRODUCT_ID") == strItemId)
                                                       .Select(s => s.Field<Decimal>("PRODUCT_OFFER_PRICE")).First().ToString();


                    //Check if the product has offer and calculate total cost accordingly
                    if (strNumOfItemsOffer != "0")
                    {
                        nRemainder = objValidNumOfUnits[itrValidItems] % Convert.ToInt32(strNumOfItemsOffer);
                        nQuotient = objValidNumOfUnits[itrValidItems] / Convert.ToInt32(strNumOfItemsOffer);

                        decItemPriceOnOffer = (Decimal)(nQuotient * Convert.ToDouble(strNumOfItemsOfferPrice));
                        decItemFixedPrice = (Decimal)(nRemainder * Convert.ToDouble(strItemUnitPrice));

                        decTotalItemCost = decItemPriceOnOffer + decItemFixedPrice;
                    }
                    else
                    {
                        decTotalItemCost = (Decimal)(objValidNumOfUnits[itrValidItems] * Convert.ToDouble(strItemUnitPrice));
                    }

                    //Add the total cost of each product after calculation to array FinalCost
                    CostTotalUnitsItem.Add(decTotalItemCost);
                }

            }

        }
        public List<string> GetInvalidItemList(string strCartDetails)
        {
            CalculateCost(strCartDetails);
            return objInvalidItems;

        }
        public List<string> GetValidItemList(string strCartDetails)
        {
            CalculateCost(strCartDetails);
            return objValidItems;

        }
        public DataTable GetReceipt(string strCartDetails)
        {
            CalculateCost(strCartDetails);
            DataTable Receipt = new DataTable();

            Receipt.TableName = "Reciept";
            Receipt.Columns.Add("SlNo");
            Receipt.Columns.Add("ItemDescription");
            Receipt.Columns.Add("UnitPrice");
            Receipt.Columns.Add("SpecialOffer");
            Receipt.Columns.Add("UnitsPurchased");
            Receipt.Columns.Add("Price");

            if (objValidItems.Count() >= 1)
            {


                int nitem = 0;
                for (nitem = 0; nitem < objValidItems.Count(); nitem++)
                {
                    Receipt.Rows.Add();
                    Receipt.Rows[nitem]["SlNo"] = nitem + 1;
                    Receipt.Rows[nitem]["ItemDescription"] = ProductDetails.AsEnumerable()
                                                                    .Where(r => r.Field<string>("SKU_PRODUCT_ID") == objValidItems[nitem])
                                                                    .Select(s => s.Field<string>("PRODUCT_DESC")).First().ToString();
                    Receipt.Rows[nitem]["UnitPrice"] = ProductDetails.AsEnumerable()
                                                                    .Where(r => r.Field<string>("SKU_PRODUCT_ID") == objValidItems[nitem])
                                                                    .Select(s => s.Field<Decimal>("PRODUCT_UNIT_PRICE")).First().ToString();
                    Receipt.Rows[nitem]["SpecialOffer"] = ProductDetails.AsEnumerable()
                                                                    .Where(r => r.Field<string>("SKU_PRODUCT_ID") == objValidItems[nitem])
                                                                    .Select(s => s.Field<int>("PRODUCT_COUNT_OFFER")).First().ToString()
                                                                    + " for " +
                                                                ProductDetails.AsEnumerable()
                                                                    .Where(r => r.Field<string>("SKU_PRODUCT_ID") == objValidItems[nitem])
                                                                    .Select(s => s.Field<Decimal>("PRODUCT_OFFER_PRICE")).First().ToString();
                    Receipt.Rows[nitem]["UnitsPurchased"] = objValidNumOfUnits[nitem];
                    Receipt.Rows[nitem]["Price"] = String.Format("{0:0.00}", CostTotalUnitsItem[nitem]);
                }
                
            }
            return Receipt;
        }
        public string GetCostGrandTotal(string strCartDetails)
        {
            CalculateCost(strCartDetails);
            return String.Format("{0:0.00}", CostTotalUnitsItem.Sum());
        }
        public string UpdateProductDetails(string ItemId, string strNumOfItemsOffer, string strItemsOfferPrice)
        {
            bool isItemIdValid = false;
            bool isValidPrice = false;
            bool isValidNumOfItems = false;
            Decimal decItemsOfferPrice = 0.0m;
            int nNumOfItemsOffer = 0;
            string status;
            if (Int32.TryParse(strNumOfItemsOffer.Trim(),out nNumOfItemsOffer))
            {
                isValidNumOfItems = true;
            }
            else
            {
                isValidNumOfItems = false;
            }
            if (Decimal.TryParse(strItemsOfferPrice.Trim(), out decItemsOfferPrice))
            {
                isValidPrice = true;
            }
            else
            {
                isValidPrice = false;
            }
            ItemId.Trim();
            isItemIdValid = ProductDetails.AsEnumerable().Select(r => r.Field<string>("SKU_PRODUCT_ID")).Contains(ItemId);
            if (isItemIdValid & isValidPrice & isValidNumOfItems)
            {
              
                var QueryResultRow = db.Products_Details.Where(i => i.SKU_PRODUCT_ID == ItemId).FirstOrDefault();
                QueryResultRow.PRODUCT_COUNT_OFFER = nNumOfItemsOffer;
                QueryResultRow.PRODUCT_OFFER_PRICE = decItemsOfferPrice;

                db.SubmitChanges();
                FillData();
                status = "Message: Special offer updated successfully to the Product";
                return status;
            }
            else
            {
                status = "Error: Update Failed Please check the input";
                return status;
            }
           
        }
        
    }
}