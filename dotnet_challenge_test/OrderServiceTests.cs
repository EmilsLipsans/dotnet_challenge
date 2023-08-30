using dotnet_challenge;

namespace dotnet_challenge_test;

[TestClass]
public class OrderServiceTests
{
    private const decimal DefaultKitPrice = 98.99M;

    // I have followed the guidelines from https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices to develop my unit tests.
    [TestMethod]
    public void PlaceOrder_ValidInput_ReturnsNoDiscountOrder()
    {
        // Arrange 
        OrderService orderService = new OrderService();
        int customerId = 1;
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);
        int desiredAmount = 5;
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
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);
        int desiredAmount = 10;
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
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);
        int desiredAmount = 50;
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
    [DataRow("2010-01-01", 1, 1)] // Expected delivery date is in the past
    [DataRow("2030-01-01", 1, 22)] // Kit id does not exist in the list
    [DataRow("2030-01-01", 0, 1)] // DesiredAmount is 0
    [DataRow("2030-01-01", 1000, 1)] // DesiredAmount is over the 999 limit
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
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);
        int desiredAmount = 1;
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
    public void ImportDnaTestKitList_ValidInput_ImportsList()
    {
        // Arrange 
        OrderService orderService = new OrderService();

        List<Tuple<int, string, decimal>> importList = new List<Tuple<int, string, decimal>>();

        int expectedDnaTestKitCount = 2; // Default kit and the imported kit

        int kitId = 2;
        string kitVariant = "Kit 2";
        decimal basePrice = 20M;

        // Add 3 Tuples of DnaTestKit values to the import list 
        importList.Add(new Tuple<int, string, decimal>(kitId, kitVariant, basePrice));

        // Act
        orderService.ImportDnaTestKitList(importList);

        // Assert that _dnaTestingKits list is empty 
        Assert.AreEqual(expectedDnaTestKitCount, orderService.DnaTestingKits.Count);
    }

    public static IEnumerable<object[]> InvalidTestData()
    {
        yield return new object[] { 1, "Kit 2", 20.0M }; // Kit id is already used
        yield return new object[] { 2, "Kit 2", -10.0M }; // Base price is not a positive number
    }

    [TestMethod]
    [DynamicData(nameof(InvalidTestData), DynamicDataSourceType.Method)]
    public void ImportDnaTestKitList_InvalidInput_ThrowsException(int kitId, string kitVariant, decimal basePrice)
    {
        // Arrange 
        OrderService orderService = new OrderService();
        List<Tuple<int, string, decimal>> importList = new List<Tuple<int, string, decimal>>();

        // Add Tuple of DnaTestKit values to the import list 
        importList.Add(new Tuple<int, string, decimal>(kitId, kitVariant, basePrice));

        // Act and Assert 
        Assert.ThrowsException<ArgumentException>(() => orderService.ImportDnaTestKitList(importList));
    }

    [TestMethod]
    public void ImportDnaTestKitList_EmptyInput_ThrowsException()
    {
        // Arrange 
        OrderService orderService = new OrderService();
        List<Tuple<int, string, decimal>> importList = new List<Tuple<int, string, decimal>>();

        // Act and Assert 
        Assert.ThrowsException<ArgumentNullException>(() => orderService.ImportDnaTestKitList(importList));
    }
}