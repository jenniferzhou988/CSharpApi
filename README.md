# CSharpApi

Customer, CustomerAddressLink, AddressType


GDCTEntityBase


inheritance
Customer,

Order,

OrderStatus, seed, ordered, ReadyToShip,Shiped,Closed




Create Entity Order inheritance from GDCTEntityBase, reference Customer with Foreign key CustomerId, Reference BankCardInfo with foreign key BankCardId, Refence Address with ShippingAddressId, Reference Address with BillingAddressId, reference OrderStatus with Foreign key OrderStatusId. Create OrderDetail inhertiance from GDCTEntityBase , reference to Order with Foreign Key OrderId, reference Product with ProductId, include Price, Quantity, TotalPrice, Comment, Create OrderRepository and OrderController, to create new Order, with OrderStatus As Ordered, including adding New Order detail item





OrderDetail,

BillingMethod,  CreditCard, MasterCard, BankCard, Paypal
BillingBankCard,


ProductCategory-->Women, Men, Kids,Home, Garden, Kitchen, Backyard, Electornic, Makeup, Office, Bedroom, Family Room, Dinner Room


Order --> Customer 1 --> many BankCard
include CustomerId, BankCardId





ProductImportRecords,

ProductInventory,



ShoppingCart-ShoppingCartDetail




UserRole




ShippingTracking, TrackingNumber, ShippingServiceProvider

ShippingServiceProvider


User, Admin, ExternalClient

UserCustomerLink

Check AuthService





