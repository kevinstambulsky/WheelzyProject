2) What would you do if you had data that doesn’t change often but it’s used pretty much all
the time? Would it make a difference if you have more than one instance of your
application?

I would cache the data. If the application is simple / small one, I would use IMemoryCache.
If the application is bigger, and run in different instances, I would use something like Redis, to have a distributed cache.

3)I did the changes on "UpdateCustomerBalanceByInvoiceCommandHandler.cs".
	I did separate the logic to work better:
	I grouped Invoices by customers.
	Load customers.
	Update each customer.
	And saved all the changes at once.