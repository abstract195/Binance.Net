namespace Binance.Net.Objects.Models
{
    /// <summary>
    /// Query results
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record BinanceQueryRecordList<T>
    {
        /// <summary>
        /// The list records
        /// </summary>
        [JsonPropertyName("list")]
        public IEnumerable<T> Rows { get; set; } = Array.Empty<T>();
        /// <summary>
        /// The total count of the records
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
