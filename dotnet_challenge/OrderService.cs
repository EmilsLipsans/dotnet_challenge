namespace dotnet_challenge;

public class OrderService : IOrderService
{
    private List<Order> _orders = new List<Order>();
    private List<DnaTestingKit> _dnaTestingKits = new List<DnaTestingKit>();
    public List<DnaTestingKit> DnaTestingKits => _dnaTestingKits;

    // Include the standard DNA testing kit in the list during object instantiation
    public OrderService()
    {
        DnaTestingKit defaultKit = new DnaTestingKit
        {
            KitId = 1,
            KitVariant = "default Kit",
            BasePrice = 98.99M
        };
        _dnaTestingKits.Add(defaultKit);
    }

    public Order PlaceOrder(int customerId, DateTime expectedDeliveryDate, int desiredAmount, int kitId)
    {
        if (expectedDeliveryDate <= DateTime.Now)
        {
            throw new ArgumentException("Expected delivery date must be in the future.");
        }

        if (!IsKitId(kitId))
        {
            throw new ArgumentException(
                "Invalid kit ID. The provided kit ID is not found in the list of available DNA testing kits.");
        }

        switch (desiredAmount)
        {
            case <= 0:
                throw new ArgumentException("Desired amount must be greater than 0.");
            case > 999:
                throw new ArgumentException("Desired amount must be less than 1000.");
        }

        var order = new Order
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

    private bool IsKitId(int kitId)
    {
        return _dnaTestingKits.Any(dnaTestingKit => dnaTestingKit.KitId == kitId);
    }

    private decimal GetKitBasePrice(int kitId)
    {
        return _dnaTestingKits.First(dnaTestingKit => dnaTestingKit.KitId == kitId).BasePrice;
    }

    private int GenerateOrderId()
    {
        if (_orders.Any())
        {
            // Find the highest existing order ID within _orders and increment it by 1
            return _orders.OrderByDescending(order => order.OrderId).First().OrderId + 1;
        }

        // If _orders is empty, start with order ID 1
        return 1;
    }

    // Imports List of tuples containing int kit ID, string kit variant and decimal base price 
    public void ImportDnaTestKitList(List<Tuple<int, string, decimal>> sourceData)
    {
        if (!sourceData.Any())
        {
            throw new ArgumentNullException(nameof(sourceData), "Source data cannot be null.");
        }
        
        foreach (var dataTuple in sourceData)
        {
            var kitId = dataTuple.Item1;
            var kitVariant = dataTuple.Item2;
            var basePrice = dataTuple.Item3;

            if (IsKitId(kitId))
            {
                throw new ArgumentException("Kit id must be unique.", nameof(basePrice));
            }

            if (basePrice <= 0)
            {
                throw new ArgumentException("Base price must be greater than zero.", nameof(basePrice));
            }

            DnaTestingKit copiedKit = new DnaTestingKit
            {
                KitId = kitId,
                KitVariant = kitVariant,
                BasePrice = basePrice
            };

            _dnaTestingKits.Add(copiedKit);
        }
    }

    public void ClearDnaTestKitList()
    {
        _dnaTestingKits.Clear();
    }

    public List<Order> GetCustomerOrders(int customerId)
    {
        return _orders.Where(order => order.CustomerId == customerId).ToList();
    }
}