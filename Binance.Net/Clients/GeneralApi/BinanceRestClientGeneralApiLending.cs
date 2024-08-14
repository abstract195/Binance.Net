using Binance.Net.Clients.GeneralApi;
using Binance.Net.Enums;
using Binance.Net.Objects.Models;
using Binance.Net.Objects.Models.Spot;
using Binance.Net.Objects.Models.Spot.Lending;
using Binance.Net.Objects.Models.Spot.Loans;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Net.Interfaces.Clients.GeneralApi
{
    public class BinanceRestClientGeneralApiLending : IBinanceRestClientGeneralApiLending
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly BinanceRestClientGeneralApi _baseClient;

        internal BinanceRestClientGeneralApiLending(BinanceRestClientGeneralApi baseClient)
        {
            _baseClient = baseClient;
        }

        public async Task<WebCallResult<BinanceQueryRecordList<BinanceAutoInvestHistoryItem>>> GetAutoInvestHistory(string? planId, string? targetAsset, string? planType = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();


            parameters.AddOptionalParameter("planId", planId);
            parameters.AddOptionalParameter("targetAsset", targetAsset);
            parameters.AddOptionalParameter("planType", planType);

            parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));

            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/lending/auto-invest/history/list", BinanceExchange.RateLimiter.SpotRestUid, 6000, true);

            return await _baseClient.SendAsync<BinanceQueryRecordList<BinanceAutoInvestHistoryItem>>(request, parameters, ct).ConfigureAwait(false);
        }
    }
}
