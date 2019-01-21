# MarketBillingService
MarketBilling Service serves to calculate the TotalPrice and returns Purchase Receipt to Client machine during basket checkout in Supermarket.It also serves to update special offer discounts for products and send information to client about the updated product catalogue whenever required.

Steps to run the Service:
1. Open the "MarketBillingService" solution via. Visual Studio ( For Development, I have used VS Community 2017 v15.9.5 ).
2. MANDATORY to "Clean and Rebuild" option in the solution 
3. Open MarketBillingSystem.asmx file from the solution and run it.
4. Your web service will be launched in your browser eg.http://localhost:58188/MarketBillingSystem.asmx 
5. You will see the list of services available to invoke in ( Details of the Service functionalities stated below ).

Requirement1: Client should consume this web service to calculate the total price for a number of items by summing their prices including applying any relevant discounts. 
Applicable Service: GetCostGrandTotal

Requirement2: Weekly offers for products change frequently so it is important to provide the ability to change them.
Applicable Service: UpdateProductDetails

Services that serves to prove good user interaction in CLI: GetReceipt,GetListValidItems,GetListInvalidItems,ListProducts

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
•	Return type: String
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

*****If input is incorrect, Code is handled in most cases to ignore it and lets the user know that their input is wrong *****So it can be tested also with Negative scenarios
