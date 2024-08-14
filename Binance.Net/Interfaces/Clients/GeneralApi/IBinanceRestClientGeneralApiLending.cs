using Binance.Net.Enums;
using Binance.Net.Objects.Models;
using Binance.Net.Objects.Models.Spot.Lending;
using Binance.Net.Objects.Models.Spot.Loans;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Net.Interfaces.Clients.GeneralApi
{
    public interface IBinanceRestClientGeneralApiLending
    {
        /// <summary>
        /// https://binance-docs.github.io/apidocs/spot/en/#query-subscription-transaction-history-user_data
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="targetAsset"></param>
        /// <param name="planType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="receiveWindow"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<BinanceQueryRecordList<BinanceAutoInvestHistoryItem>>> GetAutoInvestHistory(string? planId = null, string? targetAsset = null, string? planType = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default);
    }
}
