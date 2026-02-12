[![NuGet Version](https://img.shields.io/nuget/v/StoreKit2)](https://www.nuget.org/packages/StoreKit2)

# MAUI StoreKit2 IAP Module

A .NET MAUI binding library that provides seamless integration with iOS StoreKit2 In-App Purchase functionality.

## Overview

This library enables .NET MAUI applications to leverage Apple's modern StoreKit2 framework for handling in-app purchases on iOS. It provides a C# wrapper around the native StoreKit2 APIs, making it easy to integrate IAP functionality into your cross-platform MAUI applications.

## Features

- ✅ **Product Information Retrieval**: Fetch product details from the App Store
- ✅ **Purchase Processing**: Handle consumable, non-consumable, and subscription purchases
- ✅ **Purchase with Options**: Support for quantity, app account tokens, and Ask to Buy simulation
- ✅ **Purchase Restoration**: Restore previous purchases for users
- ✅ **Transaction Verification**: Built-in transaction verification using StoreKit2
- ✅ **Purchase Status Checking**: Check the current entitlement status of products
- ✅ **Current Entitlements**: Retrieve all active entitlements at once
- ✅ **Transaction History**: Access the complete transaction history
- ✅ **Latest Transaction**: Get the most recent transaction for any product
- ✅ **Unfinished Transactions**: Retrieve and finish pending transactions
- ✅ **Subscription Status**: Check detailed subscription status including renewal info
- ✅ **Subscription Offers**: Access introductory and promotional offer details
- ✅ **Intro Offer Eligibility**: Check if a user is eligible for an introductory offer
- ✅ **Manage Subscriptions UI**: Present the App Store subscription management sheet
- ✅ **Refund Requests**: Initiate refund requests through the App Store
- ✅ **Storefront Detection**: Get current storefront and listen for changes
- ✅ **Family Sharing**: Detect family-shared purchases
- ✅ **Revocation Detection**: Automatic detection and notification of revoked transactions
- ✅ **Rich Transaction Data**: Original transaction IDs, expiration dates, offer details, environment info
- ✅ **JSON Representations**: Access raw JSON data for products and transactions
- ✅ **Async/Await Support**: Modern async programming patterns
- ✅ **Delegate Pattern**: Event-driven callbacks for purchase and storefront events
- ✅ **iOS 15+ Support**: Takes advantage of StoreKit2, with iOS 16+ extras

## Requirements

- **iOS 15.0+** (StoreKit2 requirement)
- **.NET 8.0** or later
- **MAUI Project** targeting iOS

## Prerequisites

### App Store Connect Setup
1. Create your app in App Store Connect
2. Configure In-App Purchase products with the same product IDs used in your code
3. Create sandbox test users for testing

### iOS Project Requirements  
1. iOS 15.0+ deployment target
2. Valid Bundle ID matching App Store Connect
3. StoreKit capability enabled in your iOS project

## Installation

### NuGet Package

```bash
dotnet add package StoreKit2
```

Or add to your `.csproj` file:

```xml
<PackageReference Include="StoreKit2" Version="1.0.1" />
```

### Manual Installation

1. Clone this repository
2. Add the project reference to your MAUI project:
   ```xml
   <ProjectReference Include="path/to/MAUI.StoreKit2/MAUI.StoreKit2.csproj" />
   ```
3. Build and run

## Quick Start

### 1. Initialize the Payment Manager

```csharp
using StoreKit2;

// Get the shared instance
var paymentManager = PaymentManager.Shared;

// Set up delegate for callbacks
paymentManager.Delegate = new PaymentManagerDelegateImpl();
```

### 2. Create a Delegate Implementation

```csharp
public class PaymentManagerDelegateImpl : PaymentManagerDelegate
{
    public override void PaymentManagerDidFinishPurchase(string productId, PaymentTransaction transaction)
    {
        Console.WriteLine($"Purchase completed: {productId}");
        Console.WriteLine($"  Transaction ID: {transaction.TransactionId}");
        Console.WriteLine($"  Original Transaction ID: {transaction.OriginalTransactionId}");
        Console.WriteLine($"  Ownership: {transaction.OwnershipType}");
        // Handle successful purchase
    }

    public override void PaymentManagerDidFailPurchase(string productId, string error)
    {
        Console.WriteLine($"Purchase failed: {productId}, Error: {error}");
        // Handle purchase failure
    }

    public override void PaymentManagerDidUpdateProducts(PaymentProduct[] products)
    {
        foreach (var product in products)
        {
            Console.WriteLine($"Product: {product.DisplayName} - {product.DisplayPrice}");
            if (product.SubscriptionPeriod != null)
            {
                Console.WriteLine($"  Subscription: {product.SubscriptionPeriod.Value} {product.SubscriptionPeriod.Unit}");
            }
            if (product.IntroductoryOffer != null)
            {
                Console.WriteLine($"  Intro Offer: {product.IntroductoryOffer.DisplayPrice} ({product.IntroductoryOffer.PaymentMode})");
            }
        }
    }

    public override void PaymentManagerDidRestorePurchases(PaymentTransaction[] transactions)
    {
        Console.WriteLine($"Restored {transactions.Length} purchases");
        // Handle restored purchases
    }

    public override void PaymentManagerDidDetectStorefrontChange(StorefrontInfo storefront)
    {
        Console.WriteLine($"Storefront changed: {storefront.CountryCode}");
        // Re-fetch products if needed for new pricing
    }

    public override void PaymentManagerTransactionRevoked(string productId, string reason)
    {
        Console.WriteLine($"Transaction revoked: {productId}, reason: {reason}");
        // Revoke access to the product
    }
}
```

### 3. Load Products

```csharp
string[] productIds = { "com.yourapp.product1", "com.yourapp.product2" };

paymentManager.RequestProductsWithProductIds(productIds, (success, error) =>
{
    if (success)
    {
        Console.WriteLine("Products loaded successfully");
        var products = paymentManager.GetAllProducts;
        // Display products in your UI
    }
    else
    {
        Console.WriteLine($"Failed to load products: {error}");
    }
});
```

### 4. Make a Purchase (must Load Products first before Make a Purchase)

```csharp
paymentManager.PurchaseProductWithProductId("com.yourapp.product1", null, (success, error) =>
{
    if (success)
    {
        Console.WriteLine("Purchase initiated successfully");
    }
    else
    {
        Console.WriteLine($"Purchase failed: {error}");
    }
});
```

### 5. Restore Purchases

```csharp
paymentManager.RestorePurchasesWithCompletion((success, error) =>
{
    if (success)
    {
        Console.WriteLine("Purchases restored successfully");
    }
    else
    {
        Console.WriteLine($"Restore failed: {error}");
    }
});
```

### 6. Check Purchase Status

```csharp
paymentManager.CheckPurchaseStatusWithProductId("com.yourapp.product1", (hasPurchase, transaction) =>
{
    if (hasPurchase)
    {
        Console.WriteLine($"User owns this product. Transaction ID: {transaction.TransactionId}");
    }
    else
    {
        Console.WriteLine("User does not own this product");
    }
});
```

### 7. Get All Current Entitlements

```csharp
paymentManager.GetCurrentEntitlements((transactions) =>
{
    foreach (var t in transactions)
    {
        Console.WriteLine($"Entitled: {t.ProductId} (type: {t.ProductType}, ownership: {t.OwnershipType})");
    }
});
```

### 8. Check Subscription Status

```csharp
paymentManager.GetSubscriptionStatus("com.yourapp.subscription", (success, statuses, error) =>
{
    if (success && statuses != null)
    {
        foreach (var status in statuses)
        {
            Console.WriteLine($"Subscription state: {status.State}");
            if (status.RenewalInfo != null)
            {
                Console.WriteLine($"  Will auto-renew: {status.RenewalInfo.WillAutoRenew}");
                Console.WriteLine($"  Current product: {status.RenewalInfo.CurrentProductId}");
                Console.WriteLine($"  In billing retry: {status.RenewalInfo.IsInBillingRetry}");
            }
            if (status.Transaction != null)
            {
                Console.WriteLine($"  Expires: {status.Transaction.ExpirationDate}");
            }
        }
    }
});
```

### 9. Check Introductory Offer Eligibility

```csharp
paymentManager.CheckEligibleForIntroOffer("com.yourapp.subscription", (eligible) =>
{
    Console.WriteLine($"Eligible for intro offer: {eligible}");
});
```

### 10. Show Manage Subscriptions

```csharp
paymentManager.ShowManageSubscriptions((success, error) =>
{
    if (!success)
        Console.WriteLine($"Failed to show manage subscriptions: {error}");
});
```

### 11. Request a Refund

```csharp
paymentManager.BeginRefundRequest("123456789", (success, status) =>
{
    Console.WriteLine($"Refund request: {status}"); // "success" or "userCancelled"
});
```

### 12. Get Transaction History

```csharp
paymentManager.GetTransactionHistory((transactions) =>
{
    Console.WriteLine($"Total transactions: {transactions.Count}");
    foreach (var t in transactions)
    {
        Console.WriteLine($"  {t.ProductId}: {t.PurchaseDate} (env: {t.Environment ?? "unknown"})");
    }
});
```

### 13. Get Storefront Info

```csharp
paymentManager.GetCurrentStorefront((storefront) =>
{
    if (storefront != null)
    {
        Console.WriteLine($"Storefront: {storefront.CountryCode} (ID: {storefront.StorefrontId})");
    }
});
```

### 14. Purchase with Options (Consumable Quantity, Ask to Buy)

```csharp
paymentManager.PurchaseProductWithOptions("com.yourapp.coins100", null, 3, false, (success, transaction, error) =>
{
    if (success && transaction != null)
    {
        Console.WriteLine($"Purchased {transaction.Quantity}x {transaction.ProductId}");
    }
});
```

### 15. Handle Unfinished Transactions

```csharp
paymentManager.GetUnfinishedTransactions((transactions) =>
{
    Console.WriteLine($"Unfinished transactions: {transactions.Count}");
});

// Or finish them all at once
paymentManager.FinishAllTransactions((count) =>
{
    Console.WriteLine($"Finished {count} transactions");
});
```

## API Reference

### PaymentManager

The main class for handling in-app purchases. Access via `PaymentManager.Shared`.

#### Properties

- `Shared`: Static singleton instance
- `Delegate`: Delegate for receiving purchase and storefront events

#### Product Methods

- `RequestProductsWithProductIds(productIds, completion)`: Load product information from the App Store
- `GetProductWithProductId(productId)`: Get cached product information by ID
- `AllProducts`: Get all loaded products

#### Purchase Methods

- `PurchaseProductWithProductId(productId, appAccountToken, completion)`: Purchase a product
- `PurchaseProductWithOptions(productId, appAccountToken, quantity, simulateAskToBuy, completion)`: Purchase with advanced options (returns transaction)
- `RestorePurchasesWithCompletion(completion)`: Restore previous purchases via `AppStore.sync()`

#### Entitlement & Status Methods

- `CheckPurchaseStatusWithProductId(productId, completion)`: Check if user is entitled to a product
- `GetCurrentEntitlements(completion)`: Get all current active entitlements

#### Transaction Methods

- `GetTransactionHistory(completion)`: Get full transaction history
- `GetLatestTransaction(productId, completion)`: Get the latest transaction for a product
- `GetUnfinishedTransactions(completion)`: Get transactions that haven't been finished
- `FinishAllTransactions(completion)`: Finish all unfinished transactions

#### Subscription Methods

- `GetSubscriptionStatus(productId, completion)`: Get detailed subscription status and renewal info
- `CheckEligibleForIntroOffer(productId, completion)`: Check introductory offer eligibility
- `ShowManageSubscriptions(completion)`: Present the App Store manage subscriptions sheet

#### Other Methods

- `BeginRefundRequest(transactionId, completion)`: Initiate a refund request
- `GetCurrentStorefront(completion)`: Get current App Store storefront

### PaymentManagerDelegate

All delegate methods are optional.

- `PaymentManagerDidFinishPurchase(productId, transaction)`: Purchase succeeded
- `PaymentManagerDidFailPurchase(productId, error)`: Purchase failed
- `PaymentManagerDidUpdateProducts(products)`: Products fetched
- `PaymentManagerDidRestorePurchases(transactions)`: Purchases restored
- `PaymentManagerDidDetectStorefrontChange(storefront)`: Storefront changed
- `PaymentManagerTransactionRevoked(productId, reason)`: Transaction revoked (refund/family sharing)

### PaymentProduct

Represents a product available for purchase.

#### Properties

- `ProductId`: Unique product identifier
- `DisplayName`: Localized product name
- `ProductDescription`: Localized product description
- `Price`: Product price as NSDecimalNumber
- `DisplayPrice`: Formatted price string (e.g., "$4.99")
- `ProductType`: Product type (consumable, nonConsumable, autoRenewable, nonRenewable)
- `IsFamilyShareable`: Whether the product supports Family Sharing
- `SubscriptionGroupID`: Subscription group ID (nil for non-subscriptions)
- `SubscriptionPeriod`: Period info with `Unit` and `Value` (nil for non-subscriptions)
- `IntroductoryOffer`: Introductory offer details (nil if none)
- `PromotionalOffers`: Array of promotional offers
- `JsonRepresentation`: Raw JSON from the App Store

### PaymentTransaction

Represents a completed transaction with full details.

#### Properties

- `TransactionId`: Unique transaction identifier
- `OriginalTransactionId`: Original transaction ID (same for renewals)
- `ProductId`: Associated product identifier
- `ProductType`: Product type string
- `AppBundleID`: The app's bundle ID
- `PurchaseDate`: Date of this purchase
- `OriginalPurchaseDate`: Date of the original purchase
- `ExpirationDate`: Subscription expiration date (nil for non-subscriptions)
- `SignedDate`: Date the JWS was signed
- `Quantity`: Number of items purchased
- `OwnershipType`: "purchased" or "familyShared"
- `OfferType`: Applied offer type (introductory, promotional, code) or nil
- `OfferId`: Applied offer identifier or nil
- `StorefrontCountryCode`: Country code at time of purchase
- `SubscriptionGroupID`: Subscription group ID or nil
- `IsUpgraded`: Whether this subscription was upgraded
- `RevocationDate`: Date of revocation or nil
- `RevocationReason`: Revocation reason or nil
- `Environment`: App Store environment - iOS 16+ only (sandbox, production, xcode)
- `Reason`: Transaction reason - iOS 16+ only (purchase, renewal)
- `JsonRepresentation`: Raw JSON of the transaction

### SubscriptionStatusInfo

Represents the current state of an auto-renewable subscription.

#### Properties

- `State`: Current state (subscribed, expired, inBillingRetryPeriod, inGracePeriod, revoked)
- `RenewalInfo`: Detailed renewal information (`SubscriptionRenewalInfo`)
- `Transaction`: The latest transaction for this subscription

### SubscriptionRenewalInfo

Detailed renewal information for a subscription.

#### Properties

- `OriginalTransactionId`: Original transaction identifier
- `CurrentProductId`: Product the subscription renews to
- `WillAutoRenew`: Whether auto-renewal is enabled
- `AutoRenewPreference`: Product ID the user chose to renew to
- `ExpirationReason`: Why the subscription expired (if applicable)
- `IsInBillingRetry`: Whether billing is being retried
- `GracePeriodExpirationDate`: Grace period end date (if applicable)
- `OfferType`: Currently applied offer type
- `OfferId`: Currently applied offer identifier
- `PriceIncreaseStatus`: Price increase consent (agreed, noIncreasePending, pending)
- `Environment`: Store environment - iOS 16+ only
- `RenewalDate`: Next renewal date - iOS 16+ only

### SubscriptionPeriodInfo

- `Unit`: Period unit (day, week, month, year)
- `Value`: Number of units

### SubscriptionOfferInfo

- `OfferId`: Offer identifier (nil for introductory)
- `OfferType`: Offer type (introductory, promotional)
- `Price`: Offer price
- `DisplayPrice`: Formatted offer price
- `Period`: Period info (`SubscriptionPeriodInfo`)
- `PeriodCount`: Number of periods
- `PaymentMode`: Payment mode (freeTrial, payAsYouGo, payUpFront)

### StorefrontInfo

- `StorefrontId`: App Store storefront identifier
- `CountryCode`: ISO country code

### Constants

The `StructsAndEnums.cs` file provides string constants for easy comparison:

- `ProductType` — Consumable, NonConsumable, AutoRenewable, NonRenewable
- `SubscriptionState` — Subscribed, Expired, InBillingRetryPeriod, InGracePeriod, Revoked
- `SubscriptionPeriodUnit` — Day, Week, Month, Year
- `OfferType` — Introductory, Promotional, Code
- `PaymentMode` — FreeTrial, PayAsYouGo, PayUpFront
- `OwnershipType` — Purchased, FamilyShared
- `RevocationReason` — DeveloperIssue, Other
- `ExpirationReason` — AutoRenewDisabled, BillingError, DidNotConsentToPriceIncrease, ProductUnavailable
- `PriceIncreaseStatus` — Agreed, NoIncreasePending, Pending
- `StoreEnvironment` — Sandbox, Production, Xcode (iOS 16+)
- `TransactionReason` — Purchase, Renewal (iOS 16+)

## Product Types

The library supports all StoreKit2 product types:

- **Consumable**: Products that can be purchased multiple times
- **Non-Consumable**: Products that are purchased once
- **Auto-Renewable**: Subscriptions that renew automatically
- **Non-Renewable**: Subscriptions that don't renew automatically

## Error Handling

The library provides comprehensive error handling through:

- Completion callbacks with success/error parameters
- Delegate methods for handling purchase failures
- Detailed error messages for debugging

## Testing

### StoreKit Configuration File Setup

Before you can test in-app purchases during development, you need to set up a StoreKit Configuration file in Xcode. This allows you to test purchases locally without requiring App Store Connect configuration.

#### Creating a StoreKit Configuration File

1. **Open your iOS project in Xcode** (the native iOS project inside your MAUI solution's `Platforms/iOS` folder, or open via the `.xcodeproj`/`.xcworkspace` if building natively)

2. **Create a new StoreKit Configuration file:**
   - In Xcode, go to **File → New → File...**
   - Search for "StoreKit" and select **StoreKit Configuration File**
   - Name it (e.g., `Products.storekit`) and save it

3. **Add your products:**
   - Click the **+** button in the StoreKit Configuration editor
   - Choose the product type (Consumable, Non-Consumable, Auto-Renewable Subscription, or Non-Renewing Subscription)
   - Fill in the product details:
     - **Reference Name**: A descriptive name for your reference
     - **Product ID**: Must match the product IDs you use in your code (e.g., `com.yourapp.product1`)
     - **Price**: Set a test price
     - **Localization**: Add display name and description

4. **Enable the StoreKit Configuration in your scheme:**
   - Go to **Product → Scheme → Edit Scheme...**
   - Select **Run** on the left panel
   - Go to the **Options** tab
   - Under **StoreKit Configuration**, select your `.storekit` file

> **Important for MAUI developers:** The `.storekit` configuration file should remain in your Xcode project. It is used by Xcode's testing infrastructure and does not need to be moved to your MAUI project's iOS platform folder.

#### Testing Options

**Local Testing with StoreKit Configuration (Recommended for Development):**
- Works in the iOS Simulator (iOS 14+)
- No App Store Connect setup required
- Instant purchase confirmations
- Great for rapid development and debugging

**Sandbox Testing (Recommended for Pre-Release):**
1. Use Apple's sandbox environment
2. Create test user accounts in App Store Connect
3. Configure your products in App Store Connect
4. Test on physical devices for most accurate results

> **Note:** While StoreKit2 supports testing in the simulator with a StoreKit Configuration file, testing on physical devices with sandbox accounts is recommended before release to ensure the full purchase flow works correctly.

For detailed instructions, see Apple's official documentation: [Setting up StoreKit Testing in Xcode](https://developer.apple.com/documentation/xcode/setting-up-storekit-testing-in-xcode)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

If you encounter any issues or have questions:

1. Check the [Issues](https://github.com/9khub/MAUI.StoreKit2/issues) page
2. Create a new issue with detailed information
3. Provide sample code and error messages when applicable

## Author

**Yuting Li**  
Shanghai Jiuqianji Technology Co., Ltd.

## Acknowledgments

- Apple's StoreKit2 documentation and samples
- The .NET MAUI community for guidance and support

## Donation

If you’d like to support this project, you can buy us a coffee at [Buy me a coffee](https://buymeacoffee.com/9khub)
