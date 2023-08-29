using dotnet_challenge;

namespace dotnet_challenge_test;

[TestClass]
public class OrderServiceTests
{
    private const decimal DefaultKitPrice = 98.99M;

    // Using https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices as a guide i have implemented unit tests
    [TestMethod]
    public void PlaceOrder_ValidInput_ReturnsNoDiscountOrder()
    {
        // Arrange 
        OrderService orderService = new OrderService();

        int customerId = 1;

        // valid order delivery date is in the future
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);

        // valid amount is from 1 to 999. order where amount is under 10 gets no discount
        int desiredAmount = 5;

        // valid kitId exists in the dictionary
        int kitId = 1;

        // expected total price calculation with no discounts 
        decimal expectedTotalPrice = desiredAmount * DefaultKitPrice;

        // Act
        Order order = orderService.PlaceOrder(customerId: customerId,
            expectedDeliveryDate: expectedDeliveryDate,
            desiredAmount: desiredAmount,
            kitId: kitId);

        // Assert that order has been created with all the passed in data
        Assert.IsNotNull(order);
        Assert.AreEqual(customerId, order.CustomerId);
        Assert.AreEqual(expectedDeliveryDate, order.ExpectedDeliveryDate);
        Assert.AreEqual(desiredAmount, order.Amount);
        Assert.AreEqual(kitId, order.KitId);
        Assert.AreEqual(expectedTotalPrice, order.TotalPrice);
    }

    [TestMethod]
    public void PlaceOrder_ValidInput_Returns5PercentDiscountOrder()
    {
        // Arrange 
        OrderService orderService = new OrderService();

        int customerId = 1;

        // valid order delivery date is in the future
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);

        // valid amount is from 1 to 999. order where amount is 10-49 gets 5% discount
        int desiredAmount = 10;

        // valid kitId exists in the dictionary
        int kitId = 1;

        // expected total price calculation with 5% discount 
        decimal expectedTotalPrice = desiredAmount * DefaultKitPrice * 0.95M;

        // Act
        Order order = orderService.PlaceOrder(customerId: customerId,
            expectedDeliveryDate: expectedDeliveryDate,
            desiredAmount: desiredAmount,
            kitId: kitId);

        // Assert that order has been created with all the passed in data
        Assert.IsNotNull(order);
        Assert.AreEqual(customerId, order.CustomerId);
        Assert.AreEqual(expectedDeliveryDate, order.ExpectedDeliveryDate);
        Assert.AreEqual(desiredAmount, order.Amount);
        Assert.AreEqual(kitId, order.KitId);
        Assert.AreEqual(expectedTotalPrice, order.TotalPrice);
    }

    [TestMethod]
    public void PlaceOrder_ValidInput_Returns15PercentDiscountOrder()
    {
        // Arrange 
        OrderService orderService = new OrderService();

        int customerId = 1;

        // valid order delivery date is in the future
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);

        // valid amount is from 1 to 999. order where amount is 50 or more gets 15% discount
        int desiredAmount = 50;

        // valid kitId exists in the dictionary
        int kitId = 1;

        // expected total price calculation with 15% discount 
        decimal expectedTotalPrice = desiredAmount * DefaultKitPrice * 0.85M;

        // Act
        Order order = orderService.PlaceOrder(customerId: customerId,
            expectedDeliveryDate: expectedDeliveryDate,
            desiredAmount: desiredAmount,
            kitId: kitId);

        // Assert that order has been created with all the passed in data
        Assert.IsNotNull(order);
        Assert.AreEqual(customerId, order.CustomerId);
        Assert.AreEqual(expectedDeliveryDate, order.ExpectedDeliveryDate);
        Assert.AreEqual(desiredAmount, order.Amount);
        Assert.AreEqual(kitId, order.KitId);
        Assert.AreEqual(expectedTotalPrice, order.TotalPrice);
    }

    [TestMethod]
    [DataRow("2010-01-01", 1, 1)] // expected delivery date is in the past
    [DataRow("2030-01-01", 1, 22)] // kit id does not exist in the list
    [DataRow("2030-01-01", 0, 1)] // desiredAmount is 0
    [DataRow("2030-01-01", 1000, 1)] // desiredAmount is over the 999 limit
    public void PlaceOrder_InvalidInput_ThrowsException(string expectedDeliveryDateString, int desiredAmount, int kitId)
    {
        // Arrange 
        OrderService orderService = new OrderService();
        int customerId = 1;

        DateTime expectedDeliveryDate = DateTime.Parse(expectedDeliveryDateString);

        // Act and Assert
        Assert.ThrowsException<ArgumentException>(() => orderService.PlaceOrder(customerId: customerId,
            expectedDeliveryDate: expectedDeliveryDate,
            desiredAmount: desiredAmount,
            kitId: kitId));
    }

    [TestMethod]
    public void GetCustomerOrders_HasOrders_ReturnsOrders()
    {
        // Arrange 
        OrderService orderService = new OrderService();

        int customerId = 1;

        // valid order delivery date is in the future
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);

        // valid amount is from 1 to 999. 
        int desiredAmount = 1;

        // valid kitId exists in the dictionary
        int kitId = 1;

        int expectedOrderCount = 3;

        // Act 
        for (int i = 0; i < expectedOrderCount; i++)
        {
            orderService.PlaceOrder(customerId: customerId,
                expectedDeliveryDate: expectedDeliveryDate,
                desiredAmount: desiredAmount,
                kitId: kitId);
        }

        var customerOrders = orderService.GetCustomerOrders(customerId);

        // Assert that customer has expected order count 
        Assert.AreEqual(expectedOrderCount, customerOrders.Count);
    }

    [TestMethod]
    public void Clear_DnaTestKitList_ClearsList()
    {
        // Arrange 
        OrderService orderService = new OrderService();

        // Act
        orderService.ClearDnaTestKitList();
        List<DnaTestingKit> testingKitList = new List<DnaTestingKit>(orderService.DnaTestingKits);

        // Assert that _dnaTestingKits list is empty 
        Assert.IsFalse(testingKitList.Any());
    }

    [TestMethod]
    public void Import_DnaTestKitList_ImportsList()
    {
        // Arrange 

        OrderService orderService = new OrderService();
        List<Tuple<int, string, decimal>> importList = new List<Tuple<int, string, decimal>>();
        int expectedDnaTestKitCount = 4;

        // Add 3 Tuples of DnaTestKit values to the import list 
        importList.Add(new Tuple<int, string, decimal>(2, "Kit 2", 20M));
        importList.Add(new Tuple<int, string, decimal>(3, "Kit 3", 30M));
        importList.Add(new Tuple<int, string, decimal>(4, "Kit 4", 40M));

        // Act

        orderService.ImportDnaTestKitList(importList);

        // Assert that _dnaTestingKits list is empty 
        Assert.AreEqual(expectedDnaTestKitCount, orderService.DnaTestingKits.Count);
    }
}