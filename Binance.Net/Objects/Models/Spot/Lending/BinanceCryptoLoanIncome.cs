using Binance.Net.Converters;
using Binance.Net.Enums;

namespace Binance.Net.Objects.Models.Spot.Lending
{
    /// <summary>
    /// Crypto loan income info
    /// </summary>
    public class BinanceAutoInvestHistoryItem
    {
        /// <summary>
        /// Unique identifier for the transaction.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The asset that is being targeted in the transaction (e.g., BTC).
        /// </summary>
        [JsonPropertyName("targetAsset")]
        public string TargetAsset { get; set; } = string.Empty;

        /// <summary>
        /// The type of the plan (e.g., SINGLE).
        /// </summary>
        [JsonPropertyName("planType")]
        public string PlanType { get; set; } = string.Empty;

        /// <summary>
        /// The name of the plan.
        /// </summary>
        [JsonPropertyName("planName")]
        public string PlanName { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier for the plan.
        /// </summary>
        [JsonPropertyName("planId")]
        public int PlanId { get; set; }

        /// <summary>
        /// The date and time of the transaction in Unix time (milliseconds).
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("transactionDateTime")]
        public DateTime TransactionDateTime { get; set; }

        /// <summary>
        /// The status of the transaction (e.g., SUCCESS).
        /// </summary>
        [JsonPropertyName("transactionStatus")]
        public string TransactionStatus { get; set; } = string.Empty;

        /// <summary>
        /// The reason for transaction failure, if any (e.g., INSUFFICIENT_BALANCE).
        /// </summary>
        [JsonPropertyName("failedType")]
        public string FailedType { get; set; } = string.Empty;

        /// <summary>
        /// The asset that is being used as the source in the transaction (e.g., BUSD).
        /// </summary>
        [JsonPropertyName("sourceAsset")]
        public string SourceAsset { get; set; } = string.Empty;

        /// <summary>
        /// The amount of the source asset being used in the transaction.
        /// </summary>
        [JsonPropertyName("sourceAssetAmount")]
        public decimal SourceAssetAmount { get; set; }

        /// <summary>
        /// The amount of the target asset being received in the transaction.
        /// </summary>
        [JsonPropertyName("targetAssetAmount")]
        public decimal TargetAssetAmount { get; set; }

        /// <summary>
        /// The wallet from which the source asset is taken (e.g., SPOT_WALLET).
        /// </summary>
        [JsonPropertyName("sourceWallet")]
        public string SourceWallet { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether flexible funds were used in the transaction.
        /// </summary>
        [JsonPropertyName("flexibleUsed")]
        public bool FlexibleUsed { get; set; }

        /// <summary>
        /// The fee charged for the transaction.
        /// </summary>
        [JsonPropertyName("transactionFee")]
        public decimal TransactionFee { get; set; }

        /// <summary>
        /// The unit of the transaction fee (e.g., BUSD).
        /// </summary>
        [JsonPropertyName("transactionFeeUnit")]
        public string TransactionFeeUnit { get; set; } = string.Empty;

        /// <summary>
        /// The execution price of the transaction.
        /// </summary>
        [JsonPropertyName("executionPrice")]
        public decimal ExecutionPrice { get; set; }

        /// <summary>
        /// The type of execution (e.g., RECURRING).
        /// </summary>
        [JsonPropertyName("executionType")]
        public string ExecutionType { get; set; } = string.Empty;

        /// <summary>
        /// The subscription cycle of the plan (e.g., WEEKLY).
        /// </summary>
        [JsonPropertyName("subscriptionCycle")]
        public string SubscriptionCycle { get; set; } = string.Empty;
    }

}
