using System;
using Foundation;
using ObjCRuntime;

namespace StoreKit2
{
    // MARK: - PaymentManager

    /// <summary>
    /// PaymentManager is the main class for managing in-app purchases.
    /// It provides methods to request products, purchase products, restore purchases,
    /// manage subscriptions, check entitlements, and interact with the App Store.
    /// </summary>
    // @interface PaymentManager : NSObject
    [BaseType(typeof(NSObject), Name = "_TtC18StoreKit2Framework14PaymentManager")]
    [DisableDefaultCtor]
    interface PaymentManager
    {
        /// <summary>
        /// The shared instance of the PaymentManager.
        /// </summary>
        // @property (readonly, nonatomic, strong, class) PaymentManager * _Nonnull shared;
        [Static]
        [Export("shared", ArgumentSemantic.Strong)]
        PaymentManager Shared { get; }

        /// <summary>
        /// The delegate for handling purchase events.
        /// </summary>
        [Wrap("WeakDelegate")]
        [NullAllowed]
        PaymentManagerDelegate Delegate { get; set; }

        /// <summary>
        /// The delegate for handling purchase events.
        /// </summary>
        // @property (nonatomic, weak) id<PaymentManagerDelegate> _Nullable delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // MARK: - Product Fetching

        /// <summary>
        /// Requests products from the App Store.
        /// </summary>
        /// <param name="productIds">The product IDs to request.</param>
        /// <param name="completion">The completion handler to call when the request is complete.</param>
        // -(void)requestProductsWithProductIds:(NSArray<NSString *> * _Nonnull)productIds completion:(void (^ _Nonnull)(BOOL, NSString * _Nullable))completion;
        [Export("requestProductsWithProductIds:completion:")]
        void RequestProductsWithProductIds(string[] productIds, Action<bool, NSString> completion);

        /// <summary>
        /// Gets a product that was previously requested.
        /// </summary>
        /// <param name="productId">The product ID to get.</param>
        /// <returns>The product, or null if not found.</returns>
        // -(PaymentProduct * _Nullable)getProductWithProductId:(NSString * _Nonnull)productId;
        [Export("getProductWithProductId:")]
        [return: NullAllowed]
        PaymentProduct GetProductWithProductId(string productId);

        /// <summary>
        /// Gets all products that were previously requested.
        /// </summary>
        /// <returns>An array of all requested products.</returns>
        // -(NSArray<PaymentProduct *> * _Nonnull)getAllProducts;
        [Export("getAllProducts")]
        PaymentProduct[] AllProducts { get; }

        // MARK: - Purchasing

        /// <summary>
        /// Purchases a product from the App Store.
        /// </summary>
        /// <param name="productId">The product ID to purchase.</param>
        /// <param name="appAccountToken">Optional app account token for the purchase.</param>
        /// <param name="completion">The completion handler to call when the purchase is complete.</param>
        // -(void)purchaseProductWithProductId:(NSString * _Nonnull)productId appAccountToken:(NSUUID * _Nullable)appAccountToken completion:(void (^ _Nonnull)(BOOL, NSString * _Nullable))completion;
        [Export("purchaseProductWithProductId:appAccountToken:completion:")]
        void PurchaseProductWithProductId(string productId, [NullAllowed] NSUuid appAccountToken, Action<bool, NSString> completion);

        /// <summary>
        /// Purchases a product with additional options including quantity and ask-to-buy simulation.
        /// </summary>
        /// <param name="productId">The product ID to purchase.</param>
        /// <param name="appAccountToken">Optional app account token for the purchase.</param>
        /// <param name="quantity">The quantity to purchase (for consumables).</param>
        /// <param name="simulateAskToBuy">Whether to simulate Ask to Buy in sandbox.</param>
        /// <param name="completion">The completion handler with success flag, transaction, and error.</param>
        // -(void)purchaseProductWithOptionsWithProductId:(NSString * _Nonnull)productId appAccountToken:(NSUUID * _Nullable)appAccountToken quantity:(NSInteger)quantity simulateAskToBuy:(BOOL)simulateAskToBuy completion:(void (^ _Nonnull)(BOOL, PaymentTransaction * _Nullable, NSString * _Nullable))completion;
        [Export("purchaseProductWithOptionsWithProductId:appAccountToken:quantity:simulateAskToBuy:completion:")]
        void PurchaseProductWithOptions(string productId, [NullAllowed] NSUuid appAccountToken, nint quantity, bool simulateAskToBuy, Action<bool, PaymentTransaction, NSString> completion);

        // MARK: - Restore Purchases

        /// <summary>
        /// Restores purchases from the App Store by syncing with the server.
        /// </summary>
        /// <param name="completion">The completion handler to call when the restore is complete.</param>
        // -(void)restorePurchasesWithCompletion:(void (^ _Nonnull)(BOOL, NSString * _Nullable))completion;
        [Export("restorePurchasesWithCompletion:")]
        void RestorePurchasesWithCompletion(Action<bool, NSString> completion);

        // MARK: - Entitlements & Purchase Status

        /// <summary>
        /// Checks the purchase status of a specific product.
        /// </summary>
        /// <param name="productId">The product ID to check.</param>
        /// <param name="completion">The completion handler with entitled status and transaction.</param>
        // -(void)checkPurchaseStatusWithProductId:(NSString * _Nonnull)productId completion:(void (^ _Nonnull)(BOOL, PaymentTransaction * _Nullable))completion;
        [Export("checkPurchaseStatusWithProductId:completion:")]
        void CheckPurchaseStatusWithProductId(string productId, Action<bool, PaymentTransaction> completion);

        /// <summary>
        /// Gets all current entitlements (active purchases and subscriptions).
        /// </summary>
        /// <param name="completion">The completion handler with an array of active transactions.</param>
        // -(void)getCurrentEntitlementsWithCompletion:(void (^ _Nonnull)(NSArray<PaymentTransaction *> * _Nonnull))completion;
        [Export("getCurrentEntitlementsWithCompletion:")]
        void GetCurrentEntitlements(Action<NSArray> completion);

        // MARK: - Transaction History

        /// <summary>
        /// Gets the complete transaction history for the user.
        /// </summary>
        /// <param name="completion">The completion handler with an array of all transactions.</param>
        // -(void)getTransactionHistoryWithCompletion:(void (^ _Nonnull)(NSArray<PaymentTransaction *> * _Nonnull))completion;
        [Export("getTransactionHistoryWithCompletion:")]
        void GetTransactionHistory(Action<NSArray> completion);

        /// <summary>
        /// Gets the latest transaction for a specific product.
        /// </summary>
        /// <param name="productId">The product ID to get the latest transaction for.</param>
        /// <param name="completion">The completion handler with the latest transaction, or null.</param>
        // -(void)getLatestTransactionWithProductId:(NSString * _Nonnull)productId completion:(void (^ _Nonnull)(PaymentTransaction * _Nullable))completion;
        [Export("getLatestTransactionWithProductId:completion:")]
        void GetLatestTransaction(string productId, Action<PaymentTransaction> completion);

        /// <summary>
        /// Gets all unfinished transactions that need to be processed.
        /// </summary>
        /// <param name="completion">The completion handler with an array of unfinished transactions.</param>
        // -(void)getUnfinishedTransactionsWithCompletion:(void (^ _Nonnull)(NSArray<PaymentTransaction *> * _Nonnull))completion;
        [Export("getUnfinishedTransactionsWithCompletion:")]
        void GetUnfinishedTransactions(Action<NSArray> completion);

        /// <summary>
        /// Finishes all unfinished transactions.
        /// </summary>
        /// <param name="completion">The completion handler with the number of transactions finished.</param>
        // -(void)finishAllTransactionsWithCompletion:(void (^ _Nonnull)(NSInteger))completion;
        [Export("finishAllTransactionsWithCompletion:")]
        void FinishAllTransactions(Action<nint> completion);

        // MARK: - Subscription Status

        /// <summary>
        /// Gets the subscription status for a specific product.
        /// The product must be an auto-renewable subscription that was previously requested.
        /// </summary>
        /// <param name="productId">The product ID of the subscription.</param>
        /// <param name="completion">The completion handler with success flag, statuses array, and error.</param>
        // -(void)getSubscriptionStatusWithProductId:(NSString * _Nonnull)productId completion:(void (^ _Nonnull)(BOOL, NSArray<SubscriptionStatusInfo *> * _Nullable, NSString * _Nullable))completion;
        [Export("getSubscriptionStatusWithProductId:completion:")]
        void GetSubscriptionStatus(string productId, Action<bool, NSArray, NSString> completion);

        /// <summary>
        /// Checks whether the user is eligible for an introductory offer for a subscription product.
        /// </summary>
        /// <param name="productId">The product ID to check eligibility for.</param>
        /// <param name="completion">The completion handler with eligibility result.</param>
        // -(void)checkEligibleForIntroOfferWithProductId:(NSString * _Nonnull)productId completion:(void (^ _Nonnull)(BOOL))completion;
        [Export("checkEligibleForIntroOfferWithProductId:completion:")]
        void CheckEligibleForIntroOffer(string productId, Action<bool> completion);

        // MARK: - Manage Subscriptions UI

        /// <summary>
        /// Presents the App Store manage subscriptions sheet.
        /// </summary>
        /// <param name="completion">The completion handler with success flag and error.</param>
        // -(void)showManageSubscriptionsWithCompletion:(void (^ _Nonnull)(BOOL, NSString * _Nullable))completion;
        [Export("showManageSubscriptionsWithCompletion:")]
        void ShowManageSubscriptions(Action<bool, NSString> completion);

        // MARK: - Refund Request

        /// <summary>
        /// Begins a refund request for a specific transaction.
        /// Presents the App Store refund request sheet.
        /// </summary>
        /// <param name="transactionId">The transaction ID to request a refund for.</param>
        /// <param name="completion">The completion handler with success flag and status/error.</param>
        // -(void)beginRefundRequestWithTransactionId:(NSString * _Nonnull)transactionId completion:(void (^ _Nonnull)(BOOL, NSString * _Nullable))completion;
        [Export("beginRefundRequestWithTransactionId:completion:")]
        void BeginRefundRequest(string transactionId, Action<bool, NSString> completion);

        // MARK: - Storefront

        /// <summary>
        /// Gets the current App Store storefront information.
        /// </summary>
        /// <param name="completion">The completion handler with storefront info, or null.</param>
        // -(void)getCurrentStorefrontWithCompletion:(void (^ _Nonnull)(StorefrontInfo * _Nullable))completion;
        [Export("getCurrentStorefrontWithCompletion:")]
        void GetCurrentStorefront(Action<StorefrontInfo> completion);
    }

    // MARK: - PaymentManagerDelegate

    /// <summary>
    /// Delegate protocol for handling StoreKit 2 purchase events, storefront changes, and revocations.
    /// </summary>
    // @protocol PaymentManagerDelegate
    [Protocol(Name = "_TtP18StoreKit2Framework22PaymentManagerDelegate_"), Model]
    [BaseType(typeof(NSObject))]
    interface PaymentManagerDelegate
    {
        /// <summary>
        /// Called when a purchase is finished successfully.
        /// </summary>
        /// <param name="productId">The product ID of the purchased product.</param>
        /// <param name="transaction">The transaction details.</param>
        // @optional -(void)paymentManagerDidFinishPurchase:(NSString * _Nonnull)productId transaction:(PaymentTransaction * _Nonnull)transaction;
        [Export("paymentManagerDidFinishPurchase:transaction:")]
        void PaymentManagerDidFinishPurchase(string productId, PaymentTransaction transaction);

        /// <summary>
        /// Called when a purchase fails.
        /// </summary>
        /// <param name="productId">The product ID of the failed product.</param>
        /// <param name="error">The error message.</param>
        // @optional -(void)paymentManagerDidFailPurchase:(NSString * _Nonnull)productId error:(NSString * _Nonnull)error;
        [Export("paymentManagerDidFailPurchase:error:")]
        void PaymentManagerDidFailPurchase(string productId, string error);

        /// <summary>
        /// Called when the products are fetched and updated.
        /// </summary>
        /// <param name="products">The array of fetched products.</param>
        // @optional -(void)paymentManagerDidUpdateProducts:(NSArray<PaymentProduct *> * _Nonnull)products;
        [Export("paymentManagerDidUpdateProducts:")]
        void PaymentManagerDidUpdateProducts(PaymentProduct[] products);

        /// <summary>
        /// Called when purchases are restored.
        /// </summary>
        /// <param name="transactions">The array of restored transactions.</param>
        // @optional -(void)paymentManagerDidRestorePurchases:(NSArray<PaymentTransaction *> * _Nonnull)transactions;
        [Export("paymentManagerDidRestorePurchases:")]
        void PaymentManagerDidRestorePurchases(PaymentTransaction[] transactions);

        /// <summary>
        /// Called when the App Store storefront changes (e.g., user changes region).
        /// </summary>
        /// <param name="storefront">The new storefront information.</param>
        // @optional -(void)paymentManagerDidDetectStorefrontChange:(StorefrontInfo * _Nonnull)storefront;
        [Export("paymentManagerDidDetectStorefrontChange:")]
        void PaymentManagerDidDetectStorefrontChange(StorefrontInfo storefront);

        /// <summary>
        /// Called when a transaction is revoked (e.g., due to refund or family sharing revocation).
        /// </summary>
        /// <param name="productId">The product ID of the revoked transaction.</param>
        /// <param name="reason">The revocation reason (developerIssue, other, unknown).</param>
        // @optional -(void)paymentManagerTransactionRevoked:(NSString * _Nonnull)productId reason:(NSString * _Nonnull)reason;
        [Export("paymentManagerTransactionRevoked:reason:")]
        void PaymentManagerTransactionRevoked(string productId, string reason);
    }

    // MARK: - PaymentProduct

    /// <summary>
    /// Represents a product available for purchase from the App Store.
    /// Contains product details, pricing, and subscription information if applicable.
    /// </summary>
    // @interface PaymentProduct : NSObject
    [BaseType(typeof(NSObject), Name = "_TtC18StoreKit2Framework14PaymentProduct")]
    [DisableDefaultCtor]
    interface PaymentProduct
    {
        /// <summary>
        /// The product ID.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull productId;
        [Export("productId")]
        string ProductId { get; }

        /// <summary>
        /// The display name of the product.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull displayName;
        [Export("displayName")]
        string DisplayName { get; }

        /// <summary>
        /// The description of the product.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull productDescription;
        [Export("productDescription")]
        string ProductDescription { get; }

        /// <summary>
        /// The price of the product as a decimal number.
        /// </summary>
        // @property (readonly, nonatomic, strong) NSDecimalNumber * _Nonnull price;
        [Export("price", ArgumentSemantic.Strong)]
        NSDecimalNumber Price { get; }

        /// <summary>
        /// The localized display price string (e.g., "$4.99").
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull displayPrice;
        [Export("displayPrice")]
        string DisplayPrice { get; }

        /// <summary>
        /// The type of the product (consumable, nonConsumable, autoRenewable, nonRenewable).
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull productType;
        [Export("productType")]
        string ProductType { get; }

        /// <summary>
        /// Whether this product supports Family Sharing.
        /// </summary>
        // @property (readonly, nonatomic) BOOL isFamilyShareable;
        [Export("isFamilyShareable")]
        bool IsFamilyShareable { get; }

        /// <summary>
        /// The subscription group ID for auto-renewable subscriptions. Nil for non-subscription products.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable subscriptionGroupID;
        [NullAllowed, Export("subscriptionGroupID")]
        string SubscriptionGroupID { get; }

        /// <summary>
        /// The subscription period information. Nil for non-subscription products.
        /// </summary>
        // @property (readonly, nonatomic, strong) SubscriptionPeriodInfo * _Nullable subscriptionPeriod;
        [NullAllowed, Export("subscriptionPeriod", ArgumentSemantic.Strong)]
        SubscriptionPeriodInfo SubscriptionPeriod { get; }

        /// <summary>
        /// The introductory offer for this subscription. Nil if no introductory offer available.
        /// </summary>
        // @property (readonly, nonatomic, strong) SubscriptionOfferInfo * _Nullable introductoryOffer;
        [NullAllowed, Export("introductoryOffer", ArgumentSemantic.Strong)]
        SubscriptionOfferInfo IntroductoryOffer { get; }

        /// <summary>
        /// The list of promotional offers for this subscription.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSArray<SubscriptionOfferInfo *> * _Nonnull promotionalOffers;
        [Export("promotionalOffers", ArgumentSemantic.Copy)]
        SubscriptionOfferInfo[] PromotionalOffers { get; }

        /// <summary>
        /// The raw JSON representation of the product from the App Store.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull jsonRepresentation;
        [Export("jsonRepresentation")]
        string JsonRepresentation { get; }
    }

    // MARK: - PaymentTransaction

    /// <summary>
    /// Represents a completed or pending transaction from the App Store.
    /// Contains detailed information about the purchase including subscription details.
    /// </summary>
    // @interface PaymentTransaction : NSObject
    [BaseType(typeof(NSObject), Name = "_TtC18StoreKit2Framework18PaymentTransaction")]
    [DisableDefaultCtor]
    interface PaymentTransaction
    {
        /// <summary>
        /// The unique transaction ID assigned by the App Store.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull transactionId;
        [Export("transactionId")]
        string TransactionId { get; }

        /// <summary>
        /// The original transaction ID. For renewals, this is the ID of the first transaction in the subscription.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull originalTransactionId;
        [Export("originalTransactionId")]
        string OriginalTransactionId { get; }

        /// <summary>
        /// The product ID of the purchased product.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull productId;
        [Export("productId")]
        string ProductId { get; }

        /// <summary>
        /// The product type (consumable, nonConsumable, autoRenewable, nonRenewable).
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull productType;
        [Export("productType")]
        string ProductType { get; }

        /// <summary>
        /// The app bundle ID that the transaction belongs to.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull appBundleID;
        [Export("appBundleID")]
        string AppBundleID { get; }

        /// <summary>
        /// The date the product was purchased.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSDate * _Nonnull purchaseDate;
        [Export("purchaseDate", ArgumentSemantic.Copy)]
        NSDate PurchaseDate { get; }

        /// <summary>
        /// The date of the original purchase. For renewals, this is the date of the first purchase.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSDate * _Nonnull originalPurchaseDate;
        [Export("originalPurchaseDate", ArgumentSemantic.Copy)]
        NSDate OriginalPurchaseDate { get; }

        /// <summary>
        /// The expiration date for subscriptions. Nil for non-subscription products.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSDate * _Nullable expirationDate;
        [NullAllowed, Export("expirationDate", ArgumentSemantic.Copy)]
        NSDate ExpirationDate { get; }

        /// <summary>
        /// The date the JWS was signed by the App Store.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSDate * _Nonnull signedDate;
        [Export("signedDate", ArgumentSemantic.Copy)]
        NSDate SignedDate { get; }

        /// <summary>
        /// The quantity of items purchased.
        /// </summary>
        // @property (readonly, nonatomic) NSInteger quantity;
        [Export("quantity")]
        nint Quantity { get; }

        /// <summary>
        /// The ownership type (purchased, familyShared).
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull ownershipType;
        [Export("ownershipType")]
        string OwnershipType { get; }

        /// <summary>
        /// The type of offer applied (introductory, promotional, code). Nil if no offer was applied.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable offerType;
        [NullAllowed, Export("offerType")]
        string OfferType { get; }

        /// <summary>
        /// The offer identifier. Nil if no offer was applied.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable offerId;
        [NullAllowed, Export("offerId")]
        string OfferId { get; }

        /// <summary>
        /// The App Store storefront country code at time of purchase.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull storefrontCountryCode;
        [Export("storefrontCountryCode")]
        string StorefrontCountryCode { get; }

        /// <summary>
        /// The subscription group ID. Nil for non-subscription products.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable subscriptionGroupID;
        [NullAllowed, Export("subscriptionGroupID")]
        string SubscriptionGroupID { get; }

        /// <summary>
        /// Whether the subscription has been upgraded to a higher service level.
        /// </summary>
        // @property (readonly, nonatomic) BOOL isUpgraded;
        [Export("isUpgraded")]
        bool IsUpgraded { get; }

        /// <summary>
        /// The date the purchase was revoked (refunded). Nil if not revoked.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSDate * _Nullable revocationDate;
        [NullAllowed, Export("revocationDate", ArgumentSemantic.Copy)]
        NSDate RevocationDate { get; }

        /// <summary>
        /// The reason for revocation (developerIssue, other). Nil if not revoked.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable revocationReason;
        [NullAllowed, Export("revocationReason")]
        string RevocationReason { get; }

        /// <summary>
        /// The App Store environment (sandbox, production, xcode). Nil on iOS &lt; 16.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable environment;
        [NullAllowed, Export("environment")]
        string Environment { get; }

        /// <summary>
        /// The reason for the transaction (purchase, renewal). Nil on iOS &lt; 16.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable reason;
        [NullAllowed, Export("reason")]
        string Reason { get; }

        /// <summary>
        /// The raw JSON representation of the transaction.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull jsonRepresentation;
        [Export("jsonRepresentation")]
        string JsonRepresentation { get; }
    }

    // MARK: - SubscriptionPeriodInfo

    /// <summary>
    /// Represents the duration of a subscription period.
    /// </summary>
    // @interface SubscriptionPeriodInfo : NSObject
    [BaseType(typeof(NSObject), Name = "_TtC18StoreKit2Framework22SubscriptionPeriodInfo")]
    [DisableDefaultCtor]
    interface SubscriptionPeriodInfo
    {
        /// <summary>
        /// The unit of the subscription period (day, week, month, year).
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull unit;
        [Export("unit")]
        string Unit { get; }

        /// <summary>
        /// The number of units in the subscription period.
        /// </summary>
        // @property (readonly, nonatomic) NSInteger value;
        [Export("value")]
        nint Value { get; }
    }

    // MARK: - SubscriptionOfferInfo

    /// <summary>
    /// Represents a subscription offer (introductory or promotional).
    /// </summary>
    // @interface SubscriptionOfferInfo : NSObject
    [BaseType(typeof(NSObject), Name = "_TtC18StoreKit2Framework21SubscriptionOfferInfo")]
    [DisableDefaultCtor]
    interface SubscriptionOfferInfo
    {
        /// <summary>
        /// The offer identifier. Nil for introductory offers.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable offerId;
        [NullAllowed, Export("offerId")]
        string OfferId { get; }

        /// <summary>
        /// The type of offer (introductory, promotional).
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull offerType;
        [Export("offerType")]
        string OfferType { get; }

        /// <summary>
        /// The discounted price of the offer.
        /// </summary>
        // @property (readonly, nonatomic, strong) NSDecimalNumber * _Nonnull price;
        [Export("price", ArgumentSemantic.Strong)]
        NSDecimalNumber Price { get; }

        /// <summary>
        /// The localized display price of the offer.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull displayPrice;
        [Export("displayPrice")]
        string DisplayPrice { get; }

        /// <summary>
        /// The period information for this offer.
        /// </summary>
        // @property (readonly, nonatomic, strong) SubscriptionPeriodInfo * _Nonnull period;
        [Export("period", ArgumentSemantic.Strong)]
        SubscriptionPeriodInfo Period { get; }

        /// <summary>
        /// The number of periods in this offer.
        /// </summary>
        // @property (readonly, nonatomic) NSInteger periodCount;
        [Export("periodCount")]
        nint PeriodCount { get; }

        /// <summary>
        /// The payment mode (freeTrial, payAsYouGo, payUpFront).
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull paymentMode;
        [Export("paymentMode")]
        string PaymentMode { get; }
    }

    // MARK: - SubscriptionRenewalInfo

    /// <summary>
    /// Represents the renewal information for an auto-renewable subscription.
    /// </summary>
    // @interface SubscriptionRenewalInfo : NSObject
    [BaseType(typeof(NSObject), Name = "_TtC18StoreKit2Framework23SubscriptionRenewalInfo")]
    [DisableDefaultCtor]
    interface SubscriptionRenewalInfo
    {
        /// <summary>
        /// The original transaction ID for the subscription.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull originalTransactionId;
        [Export("originalTransactionId")]
        string OriginalTransactionId { get; }

        /// <summary>
        /// The product ID the subscription will renew to.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull currentProductId;
        [Export("currentProductId")]
        string CurrentProductId { get; }

        /// <summary>
        /// Whether the subscription will auto-renew.
        /// </summary>
        // @property (readonly, nonatomic) BOOL willAutoRenew;
        [Export("willAutoRenew")]
        bool WillAutoRenew { get; }

        /// <summary>
        /// The product ID the user has chosen to auto-renew to. Nil if not changing.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable autoRenewPreference;
        [NullAllowed, Export("autoRenewPreference")]
        string AutoRenewPreference { get; }

        /// <summary>
        /// The reason the subscription expired (autoRenewDisabled, billingError,
        /// didNotConsentToPriceIncrease, productUnavailable). Nil if not expired.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable expirationReason;
        [NullAllowed, Export("expirationReason")]
        string ExpirationReason { get; }

        /// <summary>
        /// Whether the subscription is currently in a billing retry period.
        /// </summary>
        // @property (readonly, nonatomic) BOOL isInBillingRetry;
        [Export("isInBillingRetry")]
        bool IsInBillingRetry { get; }

        /// <summary>
        /// The date the billing grace period expires. Nil if not in a grace period.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSDate * _Nullable gracePeriodExpirationDate;
        [NullAllowed, Export("gracePeriodExpirationDate", ArgumentSemantic.Copy)]
        NSDate GracePeriodExpirationDate { get; }

        /// <summary>
        /// The type of offer currently applied (introductory, promotional, code). Nil if none.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable offerType;
        [NullAllowed, Export("offerType")]
        string OfferType { get; }

        /// <summary>
        /// The identifier of the offer currently applied. Nil if none.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable offerId;
        [NullAllowed, Export("offerId")]
        string OfferId { get; }

        /// <summary>
        /// The price increase consent status (agreed, noIncreasePending, pending).
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull priceIncreaseStatus;
        [Export("priceIncreaseStatus")]
        string PriceIncreaseStatus { get; }

        /// <summary>
        /// The App Store environment (sandbox, production, xcode). Nil on iOS &lt; 16.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nullable environment;
        [NullAllowed, Export("environment")]
        string Environment { get; }

        /// <summary>
        /// The next renewal date. Nil on iOS &lt; 16.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSDate * _Nullable renewalDate;
        [NullAllowed, Export("renewalDate", ArgumentSemantic.Copy)]
        NSDate RenewalDate { get; }
    }

    // MARK: - SubscriptionStatusInfo

    /// <summary>
    /// Represents the current status of an auto-renewable subscription.
    /// </summary>
    // @interface SubscriptionStatusInfo : NSObject
    [BaseType(typeof(NSObject), Name = "_TtC18StoreKit2Framework22SubscriptionStatusInfo")]
    [DisableDefaultCtor]
    interface SubscriptionStatusInfo
    {
        /// <summary>
        /// The subscription state (subscribed, expired, inBillingRetryPeriod, inGracePeriod, revoked).
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull state;
        [Export("state")]
        string State { get; }

        /// <summary>
        /// The renewal information for this subscription. Nil if verification failed.
        /// </summary>
        // @property (readonly, nonatomic, strong) SubscriptionRenewalInfo * _Nullable renewalInfo;
        [NullAllowed, Export("renewalInfo", ArgumentSemantic.Strong)]
        SubscriptionRenewalInfo RenewalInfo { get; }

        /// <summary>
        /// The latest transaction for this subscription. Nil if verification failed.
        /// </summary>
        // @property (readonly, nonatomic, strong) PaymentTransaction * _Nullable transaction;
        [NullAllowed, Export("transaction", ArgumentSemantic.Strong)]
        PaymentTransaction Transaction { get; }
    }

    // MARK: - StorefrontInfo

    /// <summary>
    /// Represents the current App Store storefront (region/country).
    /// </summary>
    // @interface StorefrontInfo : NSObject
    [BaseType(typeof(NSObject), Name = "_TtC18StoreKit2Framework14StorefrontInfo")]
    [DisableDefaultCtor]
    interface StorefrontInfo
    {
        /// <summary>
        /// The App Store storefront identifier.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull storefrontId;
        [Export("storefrontId")]
        string StorefrontId { get; }

        /// <summary>
        /// The ISO 3166-1 alpha-3 country code for the storefront.
        /// </summary>
        // @property (readonly, copy, nonatomic) NSString * _Nonnull countryCode;
        [Export("countryCode")]
        string CountryCode { get; }
    }
}
