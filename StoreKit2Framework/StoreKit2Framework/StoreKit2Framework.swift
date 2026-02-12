//
//  StoreKit2Framework.swift
//  StoreKit2Framework
//
//  Created by Yuting Li on 2025/5/28.
//

import Foundation
import StoreKit
import UIKit

// MARK: - Protocol

@available(iOS 15.0, *)
@objc public protocol PaymentManagerDelegate: AnyObject {
    @objc optional func paymentManagerDidFinishPurchase(_ productId: String, transaction: PaymentTransaction)
    @objc optional func paymentManagerDidFailPurchase(_ productId: String, error: String)
    @objc optional func paymentManagerDidUpdateProducts(_ products: [PaymentProduct])
    @objc optional func paymentManagerDidRestorePurchases(_ transactions: [PaymentTransaction])
    @objc optional func paymentManagerDidDetectStorefrontChange(_ storefront: StorefrontInfo)
    @objc optional func paymentManagerTransactionRevoked(_ productId: String, reason: String)
}

// MARK: - Subscription Period

@available(iOS 15.0, *)
@objc public class SubscriptionPeriodInfo: NSObject {
    @objc public let unit: String
    @objc public let value: Int
    
    public init(period: Product.SubscriptionPeriod) {
        switch period.unit {
        case .day:
            self.unit = "day"
        case .week:
            self.unit = "week"
        case .month:
            self.unit = "month"
        case .year:
            self.unit = "year"
        @unknown default:
            self.unit = "unknown"
        }
        self.value = period.value
        super.init()
    }
}

// MARK: - Subscription Offer

@available(iOS 15.0, *)
@objc public class SubscriptionOfferInfo: NSObject {
    @objc public let offerId: String?
    @objc public let offerType: String
    @objc public let price: NSDecimalNumber
    @objc public let displayPrice: String
    @objc public let period: SubscriptionPeriodInfo
    @objc public let periodCount: Int
    @objc public let paymentMode: String
    
    public init(offer: Product.SubscriptionOffer) {
        self.offerId = offer.id
        
        switch offer.type {
        case .introductory:
            self.offerType = "introductory"
        case .promotional:
            self.offerType = "promotional"
        default:
            self.offerType = "unknown"
        }
        
        self.price = NSDecimalNumber(decimal: offer.price)
        self.displayPrice = offer.displayPrice
        self.period = SubscriptionPeriodInfo(period: offer.period)
        self.periodCount = offer.periodCount
        
        switch offer.paymentMode {
        case .freeTrial:
            self.paymentMode = "freeTrial"
        case .payAsYouGo:
            self.paymentMode = "payAsYouGo"
        case .payUpFront:
            self.paymentMode = "payUpFront"
        default:
            self.paymentMode = "unknown"
        }
        
        super.init()
    }
}

// MARK: - Subscription Renewal Info

@available(iOS 15.0, *)
@objc public class SubscriptionRenewalInfo: NSObject {
    @objc public let originalTransactionId: String
    @objc public let currentProductId: String
    @objc public let willAutoRenew: Bool
    @objc public let autoRenewPreference: String?
    @objc public let expirationReason: String?
    @objc public let isInBillingRetry: Bool
    @objc public let gracePeriodExpirationDate: Date?
    @objc public let offerType: String?
    @objc public let offerId: String?
    @objc public let priceIncreaseStatus: String
    @objc public let environment: String?
    @objc public let renewalDate: Date?
    
    public init(renewalInfo: Product.SubscriptionInfo.RenewalInfo) {
        self.originalTransactionId = String(renewalInfo.originalTransactionID)
        self.currentProductId = renewalInfo.currentProductID
        self.willAutoRenew = renewalInfo.willAutoRenew
        self.autoRenewPreference = renewalInfo.autoRenewPreference
        self.isInBillingRetry = renewalInfo.isInBillingRetry
        self.gracePeriodExpirationDate = renewalInfo.gracePeriodExpirationDate
        self.offerId = renewalInfo.offerID
        
        if let offer = renewalInfo.offerType {
            switch offer {
            case .introductory:
                self.offerType = "introductory"
            case .promotional:
                self.offerType = "promotional"
            case .code:
                self.offerType = "code"
            default:
                self.offerType = "unknown"
            }
        } else {
            self.offerType = nil
        }
        
        if let reason = renewalInfo.expirationReason {
            switch reason {
            case .autoRenewDisabled:
                self.expirationReason = "autoRenewDisabled"
            case .billingError:
                self.expirationReason = "billingError"
            case .didNotConsentToPriceIncrease:
                self.expirationReason = "didNotConsentToPriceIncrease"
            case .productUnavailable:
                self.expirationReason = "productUnavailable"
            default:
                self.expirationReason = "unknown"
            }
        } else {
            self.expirationReason = nil
        }
        
        switch renewalInfo.priceIncreaseStatus {
        case .agreed:
            self.priceIncreaseStatus = "agreed"
        case .noIncreasePending:
            self.priceIncreaseStatus = "noIncreasePending"
        case .pending:
            self.priceIncreaseStatus = "pending"
        default:
            self.priceIncreaseStatus = "unknown"
        }
        
        if #available(iOS 16.0, *) {
            switch renewalInfo.environment {
            case .sandbox:
                self.environment = "sandbox"
            case .production:
                self.environment = "production"
            case .xcode:
                self.environment = "xcode"
            default:
                self.environment = "unknown"
            }
            self.renewalDate = renewalInfo.renewalDate
        } else {
            self.environment = nil
            self.renewalDate = nil
        }
        
        super.init()
    }
}

// MARK: - Subscription Status

@available(iOS 15.0, *)
@objc public class SubscriptionStatusInfo: NSObject {
    @objc public let state: String
    @objc public let renewalInfo: SubscriptionRenewalInfo?
    @objc public let transaction: PaymentTransaction?
    
    public init(status: Product.SubscriptionInfo.Status) {
        switch status.state {
        case .subscribed:
            self.state = "subscribed"
        case .expired:
            self.state = "expired"
        case .inBillingRetryPeriod:
            self.state = "inBillingRetryPeriod"
        case .inGracePeriod:
            self.state = "inGracePeriod"
        case .revoked:
            self.state = "revoked"
        default:
            self.state = "unknown"
        }
        
        switch status.renewalInfo {
        case .verified(let info):
            self.renewalInfo = SubscriptionRenewalInfo(renewalInfo: info)
        case .unverified(_, _):
            self.renewalInfo = nil
        }
        
        switch status.transaction {
        case .verified(let transaction):
            self.transaction = PaymentTransaction(transaction: transaction)
        case .unverified(_, _):
            self.transaction = nil
        }
        
        super.init()
    }
}

// MARK: - Storefront

@available(iOS 15.0, *)
@objc public class StorefrontInfo: NSObject {
    @objc public let storefrontId: String
    @objc public let countryCode: String
    
    public init(storefront: Storefront) {
        self.storefrontId = storefront.id
        self.countryCode = storefront.countryCode
        super.init()
    }
}

// MARK: - PaymentProduct

@available(iOS 15.0, *)
@objc public class PaymentProduct: NSObject {
    @objc public let productId: String
    @objc public let displayName: String
    @objc public let productDescription: String
    @objc public let price: NSDecimalNumber
    @objc public let displayPrice: String
    @objc public let productType: String
    @objc public let isFamilyShareable: Bool
    @objc public let subscriptionGroupID: String?
    @objc public let subscriptionPeriod: SubscriptionPeriodInfo?
    @objc public let introductoryOffer: SubscriptionOfferInfo?
    @objc public let promotionalOffers: [SubscriptionOfferInfo]
    @objc public let jsonRepresentation: String
    
    public init(product: Product) {
        self.productId = product.id
        self.displayName = product.displayName
        self.productDescription = product.description
        self.price = NSDecimalNumber(decimal: product.price)
        self.displayPrice = product.displayPrice
        
        switch product.type {
        case .consumable:
            self.productType = "consumable"
        case .nonConsumable:
            self.productType = "nonConsumable"
        case .autoRenewable:
            self.productType = "autoRenewable"
        case .nonRenewable:
            self.productType = "nonRenewable"
        default:
            self.productType = "unknown"
        }
        
        self.isFamilyShareable = product.isFamilyShareable
        self.subscriptionGroupID = product.subscription?.subscriptionGroupID
        
        if let period = product.subscription?.subscriptionPeriod {
            self.subscriptionPeriod = SubscriptionPeriodInfo(period: period)
        } else {
            self.subscriptionPeriod = nil
        }
        
        if let intro = product.subscription?.introductoryOffer {
            self.introductoryOffer = SubscriptionOfferInfo(offer: intro)
        } else {
            self.introductoryOffer = nil
        }
        
        self.promotionalOffers = product.subscription?.promotionalOffers.map {
            SubscriptionOfferInfo(offer: $0)
        } ?? []
        
        self.jsonRepresentation = String(data: product.jsonRepresentation, encoding: .utf8) ?? "{}"
        
        super.init()
    }
}

// MARK: - PaymentTransaction

@available(iOS 15.0, *)
@objc public class PaymentTransaction: NSObject {
    // Core properties
    @objc public let transactionId: String
    @objc public let originalTransactionId: String
    @objc public let productId: String
    @objc public let productType: String
    @objc public let appBundleID: String
    
    // Dates
    @objc public let purchaseDate: Date
    @objc public let originalPurchaseDate: Date
    @objc public let expirationDate: Date?
    @objc public let signedDate: Date
    
    // Purchase details
    @objc public let quantity: Int
    @objc public let ownershipType: String
    @objc public let offerType: String?
    @objc public let offerId: String?
    @objc public let storefrontCountryCode: String
    @objc public let subscriptionGroupID: String?
    
    // Status
    @objc public let isUpgraded: Bool
    @objc public let revocationDate: Date?
    @objc public let revocationReason: String?
    
    // iOS 16+ properties (nil on older versions)
    @objc public let environment: String?
    @objc public let reason: String?
    
    // Raw data
    @objc public let jsonRepresentation: String
    
    public init(transaction: Transaction) {
        self.transactionId = String(transaction.id)
        self.originalTransactionId = String(transaction.originalID)
        self.productId = transaction.productID
        self.appBundleID = transaction.appBundleID
        self.purchaseDate = transaction.purchaseDate
        self.originalPurchaseDate = transaction.originalPurchaseDate
        self.expirationDate = transaction.expirationDate
        self.signedDate = transaction.signedDate
        self.quantity = transaction.purchasedQuantity
        self.storefrontCountryCode = transaction.storefrontCountryCode
        self.subscriptionGroupID = transaction.subscriptionGroupID
        self.isUpgraded = transaction.isUpgraded
        self.revocationDate = transaction.revocationDate
        self.offerId = transaction.offerID
        
        // Product type
        switch transaction.productType {
        case .consumable:
            self.productType = "consumable"
        case .nonConsumable:
            self.productType = "nonConsumable"
        case .autoRenewable:
            self.productType = "autoRenewable"
        case .nonRenewable:
            self.productType = "nonRenewable"
        default:
            self.productType = "unknown"
        }
        
        // Revocation reason
        if let reason = transaction.revocationReason {
            switch reason {
            case .developerIssue:
                self.revocationReason = "developerIssue"
            case .other:
                self.revocationReason = "other"
            default:
                self.revocationReason = "unknown"
            }
        } else {
            self.revocationReason = nil
        }
        
        // Offer type
        if let offerType = transaction.offerType {
            switch offerType {
            case .introductory:
                self.offerType = "introductory"
            case .promotional:
                self.offerType = "promotional"
            case .code:
                self.offerType = "code"
            default:
                self.offerType = "unknown"
            }
        } else {
            self.offerType = nil
        }
        
        // Ownership type
        switch transaction.ownershipType {
        case .familyShared:
            self.ownershipType = "familyShared"
        case .purchased:
            self.ownershipType = "purchased"
        default:
            self.ownershipType = "unknown"
        }
        
        // iOS 16+ properties
        if #available(iOS 16.0, *) {
            switch transaction.environment {
            case .sandbox:
                self.environment = "sandbox"
            case .production:
                self.environment = "production"
            case .xcode:
                self.environment = "xcode"
            default:
                self.environment = "unknown"
            }
        } else {
            self.environment = nil
        }
        
        // iOS 17+ properties
        if #available(iOS 17.0, *) {
            switch transaction.reason {
            case .purchase:
                self.reason = "purchase"
            case .renewal:
                self.reason = "renewal"
            default:
                self.reason = "unknown"
            }
        } else {
            self.reason = nil
        }
        
        self.jsonRepresentation = String(data: transaction.jsonRepresentation, encoding: .utf8) ?? "{}"
        
        super.init()
    }
}

// MARK: - PaymentManager

@available(iOS 15.0, *)
@objc public class PaymentManager: NSObject {
    @objc public static let shared: PaymentManager = PaymentManager()
    @objc public weak var delegate: PaymentManagerDelegate?
    
    private var products: [String: Product] = [:]
    private var transactionListener: Task<Void, Error>?
    private var storefrontListener: Task<Void, Error>?
    
    private override init() {
        super.init()
        startTransactionListener()
        startStorefrontListener()
    }
    
    deinit {
        transactionListener?.cancel()
        storefrontListener?.cancel()
    }
    
    // MARK: - Product Fetching
    
    @objc public func requestProducts(productIds: [String], completion: @escaping (Bool, String?) -> Void) {
        Task {
            do {
                let storeProducts: [Product] = try await Product.products(for: Set(productIds))
                
                await MainActor.run {
                    for product: Product in storeProducts {
                        self.products[product.id] = product
                    }
                    
                    let paymentProducts: [PaymentProduct] = storeProducts.map { PaymentProduct(product: $0) }
                    self.delegate?.paymentManagerDidUpdateProducts?(paymentProducts)
                    completion(true, nil)
                }
            } catch {
                await MainActor.run {
                    completion(false, error.localizedDescription)
                }
            }
        }
    }
    
    @objc public func getProduct(productId: String) -> PaymentProduct? {
        guard let product = products[productId] else { return nil }
        return PaymentProduct(product: product)
    }
    
    @objc public func getAllProducts() -> [PaymentProduct] {
        return products.values.map { PaymentProduct(product: $0) }
    }
    
    // MARK: - Purchasing
    
    @objc public func purchaseProduct(productId: String, appAccountToken: UUID? = nil, completion: @escaping (Bool, String?) -> Void) {
        guard let product = products[productId] else {
            completion(false, "Product not found: \(productId)")
            return
        }
        
        Task {
            do {
                var options: Set<Product.PurchaseOption> = []
                if let token = appAccountToken {
                    options.insert(.appAccountToken(token))
                }
                let result: Product.PurchaseResult = try await product.purchase(options: options)
                
                switch result {
                case .success(let verification):
                    switch verification {
                    case .verified(let transaction):
                        let paymentTransaction = PaymentTransaction(transaction: transaction)
                        await MainActor.run {
                            self.delegate?.paymentManagerDidFinishPurchase?(productId, transaction: paymentTransaction)
                            completion(true, nil)
                        }
                        await transaction.finish()
                    case .unverified(_, let error):
                        await MainActor.run {
                            self.delegate?.paymentManagerDidFailPurchase?(productId, error: "Unverified transaction: \(error)")
                            completion(false, "Unverified transaction")
                        }
                    }
                case .userCancelled:
                    await MainActor.run {
                        self.delegate?.paymentManagerDidFailPurchase?(productId, error: "User cancelled")
                        completion(false, "User cancelled")
                    }
                case .pending:
                    await MainActor.run {
                        completion(false, "Purchase pending")
                    }
                @unknown default:
                    await MainActor.run {
                        completion(false, "Unknown result")
                    }
                }
            } catch {
                await MainActor.run {
                    self.delegate?.paymentManagerDidFailPurchase?(productId, error: error.localizedDescription)
                    completion(false, error.localizedDescription)
                }
            }
        }
    }
    
    @objc public func purchaseProductWithOptions(productId: String, appAccountToken: UUID?, quantity: Int, simulateAskToBuy: Bool, completion: @escaping (Bool, PaymentTransaction?, String?) -> Void) {
        guard let product = products[productId] else {
            completion(false, nil, "Product not found: \(productId)")
            return
        }
        
        Task {
            do {
                var options: Set<Product.PurchaseOption> = []
                if let token = appAccountToken {
                    options.insert(.appAccountToken(token))
                }
                if quantity > 1 {
                    options.insert(.quantity(quantity))
                }
                if simulateAskToBuy {
                    options.insert(.simulatesAskToBuyInSandbox(true))
                }
                
                let result: Product.PurchaseResult = try await product.purchase(options: options)
                
                switch result {
                case .success(let verification):
                    switch verification {
                    case .verified(let transaction):
                        let paymentTransaction = PaymentTransaction(transaction: transaction)
                        await MainActor.run {
                            self.delegate?.paymentManagerDidFinishPurchase?(productId, transaction: paymentTransaction)
                            completion(true, paymentTransaction, nil)
                        }
                        await transaction.finish()
                    case .unverified(_, let error):
                        await MainActor.run {
                            self.delegate?.paymentManagerDidFailPurchase?(productId, error: "Unverified transaction: \(error)")
                            completion(false, nil, "Unverified transaction")
                        }
                    }
                case .userCancelled:
                    await MainActor.run {
                        completion(false, nil, "User cancelled")
                    }
                case .pending:
                    await MainActor.run {
                        completion(false, nil, "Purchase pending")
                    }
                @unknown default:
                    await MainActor.run {
                        completion(false, nil, "Unknown result")
                    }
                }
            } catch {
                await MainActor.run {
                    self.delegate?.paymentManagerDidFailPurchase?(productId, error: error.localizedDescription)
                    completion(false, nil, error.localizedDescription)
                }
            }
        }
    }
    
    // MARK: - Restore Purchases
    
    @objc public func restorePurchases(completion: @escaping (Bool, String?) -> Void) {
        Task {
            do {
                try await AppStore.sync()
                
                let restoredTransactions = await collectCurrentEntitlements()
                
                await MainActor.run {
                    self.delegate?.paymentManagerDidRestorePurchases?(restoredTransactions)
                    completion(true, nil)
                }
            } catch {
                await MainActor.run {
                    completion(false, error.localizedDescription)
                }
            }
        }
    }
    
    // MARK: - Entitlements & Purchase Status
    
    @objc public func checkPurchaseStatus(productId: String, completion: @escaping (Bool, PaymentTransaction?) -> Void) {
        Task {
            let foundTransaction = await findCurrentEntitlement(for: productId)
            
            await MainActor.run {
                if let transaction = foundTransaction {
                    completion(true, transaction)
                } else {
                    completion(false, nil)
                }
            }
        }
    }
    
    @objc public func getCurrentEntitlements(completion: @escaping ([PaymentTransaction]) -> Void) {
        Task {
            let transactions = await collectCurrentEntitlements()
            await MainActor.run {
                completion(transactions)
            }
        }
    }
    
    // MARK: - Transaction History
    
    @objc public func getTransactionHistory(completion: @escaping ([PaymentTransaction]) -> Void) {
        Task {
            var transactions: [PaymentTransaction] = []
            for await result in Transaction.all {
                switch result {
                case .verified(let transaction):
                    transactions.append(PaymentTransaction(transaction: transaction))
                case .unverified(_, _):
                    continue
                }
            }
            await MainActor.run {
                completion(transactions)
            }
        }
    }
    
    @objc public func getLatestTransaction(productId: String, completion: @escaping (PaymentTransaction?) -> Void) {
        Task {
            guard let result = await Transaction.latest(for: productId) else {
                await MainActor.run {
                    completion(nil)
                }
                return
            }
            
            switch result {
            case .verified(let transaction):
                let paymentTransaction = PaymentTransaction(transaction: transaction)
                await MainActor.run {
                    completion(paymentTransaction)
                }
            case .unverified(_, _):
                await MainActor.run {
                    completion(nil)
                }
            }
        }
    }
    
    @objc public func getUnfinishedTransactions(completion: @escaping ([PaymentTransaction]) -> Void) {
        Task {
            var transactions: [PaymentTransaction] = []
            for await result in Transaction.unfinished {
                switch result {
                case .verified(let transaction):
                    transactions.append(PaymentTransaction(transaction: transaction))
                case .unverified(_, _):
                    continue
                }
            }
            await MainActor.run {
                completion(transactions)
            }
        }
    }
    
    @objc public func finishAllTransactions(completion: @escaping (Int) -> Void) {
        Task {
            var count = 0
            for await result in Transaction.unfinished {
                switch result {
                case .verified(let transaction):
                    await transaction.finish()
                    count += 1
                case .unverified(_, _):
                    continue
                }
            }
            await MainActor.run {
                completion(count)
            }
        }
    }
    
    // MARK: - Subscription Status
    
    @objc public func getSubscriptionStatus(productId: String, completion: @escaping (Bool, [SubscriptionStatusInfo]?, String?) -> Void) {
        guard let product = products[productId] else {
            completion(false, nil, "Product not found: \(productId)")
            return
        }
        
        Task {
            do {
                guard let subscription = product.subscription else {
                    await MainActor.run {
                        completion(false, nil, "Product is not a subscription: \(productId)")
                    }
                    return
                }
                
                let statuses = try await subscription.status
                let statusInfos = statuses.map { SubscriptionStatusInfo(status: $0) }
                
                await MainActor.run {
                    completion(true, statusInfos, nil)
                }
            } catch {
                await MainActor.run {
                    completion(false, nil, error.localizedDescription)
                }
            }
        }
    }
    
    @objc public func checkEligibleForIntroOffer(productId: String, completion: @escaping (Bool) -> Void) {
        guard let product = products[productId] else {
            completion(false)
            return
        }
        
        Task {
            guard let subscription = product.subscription else {
                await MainActor.run {
                    completion(false)
                }
                return
            }
            
            let isEligible = await subscription.isEligibleForIntroOffer
            await MainActor.run {
                completion(isEligible)
            }
        }
    }
    
    // MARK: - Manage Subscriptions UI
    
    @objc public func showManageSubscriptions(completion: @escaping (Bool, String?) -> Void) {
        Task {
            do {
                guard let windowScene = await MainActor.run(body: {
                    UIApplication.shared.connectedScenes
                        .compactMap { $0 as? UIWindowScene }
                        .first
                }) else {
                    await MainActor.run {
                        completion(false, "No window scene available")
                    }
                    return
                }
                
                try await AppStore.showManageSubscriptions(in: windowScene)
                await MainActor.run {
                    completion(true, nil)
                }
            } catch {
                await MainActor.run {
                    completion(false, error.localizedDescription)
                }
            }
        }
    }
    
    // MARK: - Refund Request
    
    @objc public func beginRefundRequest(transactionId: String, completion: @escaping (Bool, String?) -> Void) {
        guard let transactionIdUInt = UInt64(transactionId) else {
            completion(false, "Invalid transaction ID")
            return
        }
        
        Task {
            do {
                guard let windowScene = await MainActor.run(body: {
                    UIApplication.shared.connectedScenes
                        .compactMap { $0 as? UIWindowScene }
                        .first
                }) else {
                    await MainActor.run {
                        completion(false, "No window scene available")
                    }
                    return
                }
                
                let status = try await Transaction.beginRefundRequest(for: transactionIdUInt, in: windowScene)
                
                await MainActor.run {
                    switch status {
                    case .success:
                        completion(true, "success")
                    case .userCancelled:
                        completion(false, "userCancelled")
                    @unknown default:
                        completion(false, "unknown")
                    }
                }
            } catch {
                await MainActor.run {
                    completion(false, error.localizedDescription)
                }
            }
        }
    }
    
    // MARK: - Storefront
    
    @objc public func getCurrentStorefront(completion: @escaping (StorefrontInfo?) -> Void) {
        Task {
            guard let storefront = await Storefront.current else {
                await MainActor.run {
                    completion(nil)
                }
                return
            }
            
            let info = StorefrontInfo(storefront: storefront)
            await MainActor.run {
                completion(info)
            }
        }
    }
    
    // MARK: - Private Methods
    
    private func collectCurrentEntitlements() async -> [PaymentTransaction] {
        var transactions: [PaymentTransaction] = []
        
        for await result in Transaction.currentEntitlements {
            switch result {
            case .verified(let transaction):
                transactions.append(PaymentTransaction(transaction: transaction))
            case .unverified(_, _):
                continue
            }
        }
        
        return transactions
    }
    
    private func findCurrentEntitlement(for productId: String) async -> PaymentTransaction? {
        for await result in Transaction.currentEntitlements {
            switch result {
            case .verified(let transaction):
                if transaction.productID == productId {
                    return PaymentTransaction(transaction: transaction)
                }
            case .unverified(_, _):
                continue
            }
        }
        return nil
    }
    
    private func startTransactionListener() {
        transactionListener = Task(priority: .background) {
            for await result in Transaction.updates {
                switch result {
                case .verified(let transaction):
                    let paymentTransaction = PaymentTransaction(transaction: transaction)
                    
                    if transaction.revocationDate != nil {
                        let reason: String
                        if let revReason = transaction.revocationReason {
                            switch revReason {
                            case .developerIssue:
                                reason = "developerIssue"
                            case .other:
                                reason = "other"
                            default:
                                reason = "unknown"
                            }
                        } else {
                            reason = "unknown"
                        }
                        await MainActor.run {
                            self.delegate?.paymentManagerTransactionRevoked?(transaction.productID, reason: reason)
                        }
                    } else {
                        await MainActor.run {
                            self.delegate?.paymentManagerDidFinishPurchase?(transaction.productID, transaction: paymentTransaction)
                        }
                    }
                    
                    await transaction.finish()
                case .unverified(_, let error):
                    await MainActor.run {
                        self.delegate?.paymentManagerDidFailPurchase?("unknown", error: "Unverified transaction: \(error)")
                    }
                }
            }
        }
    }
    
    private func startStorefrontListener() {
        storefrontListener = Task(priority: .background) {
            for await storefront in Storefront.updates {
                let info = StorefrontInfo(storefront: storefront)
                await MainActor.run {
                    self.delegate?.paymentManagerDidDetectStorefrontChange?(info)
                }
            }
        }
    }
}
