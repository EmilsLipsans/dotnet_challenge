namespace dotnet_challenge;

public class OrderService : IOrderService
{
    private List<Order> _orders = new List<Order>();
    private List<DnaTestingKit> _dnaTestingKits = new List<DnaTestingKit>();

    public Order PlaceOrder(int customerId, DateTime expectedDeliveryDate, int desiredAmount, int kitId)
    {
        if (expectedDeliveryDate <= DateTime.Now)
        {
            throw new ArgumentException("Expected delivery date must be in the future.");
        }

        switch (desiredAmount)
        {
            case <= 0:
                throw new ArgumentException("Desired amount must be greater than 0.");
            case > 999:
                throw new ArgumentException("Desired amount must be less than 1000.");
        }


        Order order = new Order
        {
            OrderId = GenerateOrderId(),
            CustomerId = customerId,
            ExpectedDeliveryDate = expectedDeliveryDate,
            Amount = desiredAmount,
            KitId = kitId,
            TotalPrice = CalculateTotalPrice(basePrice: GetKitBasePrice(kitId), desiredAmount: desiredAmount)
        };

        _orders.Add(order);
        return order;
    }

    private decimal CalculateTotalPrice(decimal basePrice, int desiredAmount)
    {
        if (basePrice <= 0)
        {
            throw new ArgumentException("Base price must be greater than zero.", nameof(basePrice));
        }

        if (desiredAmount <= 0)
        {
            throw new ArgumentException("Desired amount must be greater than zero.", nameof(desiredAmount));
        }

        decimal totalPrice = basePrice * desiredAmount;

        if (desiredAmount is >= 10 and < 50)
        {
            totalPrice *= 0.95m; // 5% discount
        }
        else if (desiredAmount >= 50)
        {
            totalPrice *= 0.85m; // 15% discount
        }

        return totalPrice;
    }

    private decimal GetKitBasePrice(int kitId)
    {
        return _dnaTestingKits.First(dnaTestingKit => dnaTestingKit.KitId == kitId).BasePrice;
    }

    private int GenerateOrderId()
    {
        if (_orders.Any())
        {
            // finds the largest index within _orders and returns index bigger by 1 
            return _orders.OrderByDescending(order => order.OrderId).First().OrderId + 1;
        }

        // If _orders is empty index order with 1
        return 1;
    }

    // Imports List of tuples containing int kitID, string kitVariant and decimal basePrice 
    public void ImportDnaTestKitList(List<Tuple<int, string, decimal>> sourceData)
    {
        if (sourceData == null)
        {
            throw new ArgumentNullException(nameof(sourceData), "Source data cannot be null.");
        }

        List<DnaTestingKit> copiedList = new List<DnaTestingKit>();

        foreach (var dataTuple in sourceData)
        {
            var kitId = dataTuple.Item1;
            var kitVariant = dataTuple.Item2;
            var basePrice = dataTuple.Item3;

            DnaTestingKit copiedKit = new DnaTestingKit
            {
                KitId = kitId,
                KitVariant = kitVariant,
                BasePrice = basePrice
            };

            copiedList.Add(copiedKit);
        }

        _dnaTestingKits = new List<DnaTestingKit>(copiedList);
    }

    public List<Order> GetCustomerOrders(int customerId)
    {
        return _orders.Where(order => order.CustomerId == customerId).ToList();
    }
}