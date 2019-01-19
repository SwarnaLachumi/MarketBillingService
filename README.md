# MarketBillingService

Type:  WebService |
Name: MarketBillingService |
Database: ProductsDB.mdf

Services:

1.GetReceipt
•	List the valid items added in checkout and creates Receipt
•	Return type: DataTable
•   Input1: List of items in Cart | Input Type: string
•	Valid Input Pattern:< SKUID1-UnitsPurchased>,<SKUID2-UnitsPurchased>
    o	Eg: A99-10,B15-2,C40-4

2.GetCostGrandTotal
•	Gives the Aggregate Price of all valid items added in Checkout 
•	Return type: String Array
•   Input1: List of items in Cart | Input Type: string
•	Valid Input Pattern:< SKUID1-UnitsPurchased>,<SKUID2-UnitsPurchased>
    o	Eg: A99-10,B15-2,C40-4

3.GetListValidItems
•	List the Valid inputs provided by the User
•	Return type: String Array
•   Input1: List of items in Cart | Input Type: string
•	Valid Input Pattern:< SKUID1-UnitsPurchased>,<SKUID2-UnitsPurchased>
    o	Eg: A99-10,B15-2,C40-4
    
4.GetListInvalidItems
•	List the Invalid inputs provided by the User
•	Return type: String Array
•   Input1: List of items in Cart | Input Type: string
•	Valid Input Pattern:< SKUID1-UnitsPurchased>,<SKUID2-UnitsPurchased>
    o	Eg: A99-10,B15-2,C40-4

5.UpdateProductDetails
•	Update Special Offer with respect to Product ID provided by User and returns the response of the update.
•	Return type: String
•	Input1: SKUID of Product for which offer to be updated | Input Type: string
•	Input2: Number of Units comes in Special offer | Input Type: string
•	Input3: Price for the offered Units | Input Type: string
    
6.ListProducts
•	List all details of all products in the Product Catalogue 
•	Return Type: DataTable
•	Input: NA
