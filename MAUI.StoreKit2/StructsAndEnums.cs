namespace StoreKit2 {

    /// <summary>
    /// Product types available in StoreKit 2.
    /// </summary>
    public static class ProductType
    {
        public const string Consumable = "consumable";
        public const string NonConsumable = "nonConsumable";
        public const string AutoRenewable = "autoRenewable";
        public const string NonRenewable = "nonRenewable";
    }

    /// <summary>
    /// Subscription states for auto-renewable subscriptions.
    /// </summary>
    public static class SubscriptionState
    {
        public const string Subscribed = "subscribed";
        public const string Expired = "expired";
        public const string InBillingRetryPeriod = "inBillingRetryPeriod";
        public const string InGracePeriod = "inGracePeriod";
        public const string Revoked = "revoked";
    }

    /// <summary>
    /// Subscription period units.
    /// </summary>
    public static class SubscriptionPeriodUnit
    {
        public const string Day = "day";
        public const string Week = "week";
        public const string Month = "month";
        public const string Year = "year";
    }

    /// <summary>
    /// Offer types that can be applied to purchases.
    /// </summary>
    public static class OfferType
    {
        public const string Introductory = "introductory";
        public const string Promotional = "promotional";
        public const string Code = "code";
    }

    /// <summary>
    /// Payment modes for subscription offers.
    /// </summary>
    public static class PaymentMode
    {
        public const string FreeTrial = "freeTrial";
        public const string PayAsYouGo = "payAsYouGo";
        public const string PayUpFront = "payUpFront";
    }

    /// <summary>
    /// Ownership types for transactions.
    /// </summary>
    public static class OwnershipType
    {
        public const string Purchased = "purchased";
        public const string FamilyShared = "familyShared";
    }

    /// <summary>
    /// Transaction revocation reasons.
    /// </summary>
    public static class RevocationReason
    {
        public const string DeveloperIssue = "developerIssue";
        public const string Other = "other";
    }

    /// <summary>
    /// Subscription expiration reasons.
    /// </summary>
    public static class ExpirationReason
    {
        public const string AutoRenewDisabled = "autoRenewDisabled";
        public const string BillingError = "billingError";
        public const string DidNotConsentToPriceIncrease = "didNotConsentToPriceIncrease";
        public const string ProductUnavailable = "productUnavailable";
    }

    /// <summary>
    /// Price increase consent statuses.
    /// </summary>
    public static class PriceIncreaseStatus
    {
        public const string Agreed = "agreed";
        public const string NoIncreasePending = "noIncreasePending";
        public const string Pending = "pending";
    }

    /// <summary>
    /// App Store environment identifiers (iOS 16+).
    /// </summary>
    public static class StoreEnvironment
    {
        public const string Sandbox = "sandbox";
        public const string Production = "production";
        public const string Xcode = "xcode";
    }

    /// <summary>
    /// Transaction reasons (iOS 16+).
    /// </summary>
    public static class TransactionReason
    {
        public const string Purchase = "purchase";
        public const string Renewal = "renewal";
    }
}
