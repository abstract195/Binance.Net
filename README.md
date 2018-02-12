# Binance.Net ![Icon](https://github.com/JKorf/Binance.Net/blob/master/Resources/binance-coin.png?raw=true)
![Nuget version](https://img.shields.io/nuget/v/binance.net.svg)

A .Net wrapper for the Binance API as described on [Binance](https://www.binance.com/restapipub.html), including all features.
## Installation
Binance.Net is available on [Nuget](https://www.nuget.org/packages/Binance.Net/).
```
pm> Install-Package Binance.Net
```

## Examples
Two examples have been provided, a console application providing the basis interaction with the API wrapper, and a WPF application showing some more advanced use casus. Both can be found in the Examples folder.

## Usage
Start using the API by including `using Binance.Net;` in your usings.
Binance.Net provides two clients to interact with the Binance API. The `BinanceClient` provides all rest API calls. The `BinanceSocketClient` provides functions to interact with the Websockets provided by the Binance API. Both clients are disposable and as such can be used in a `using` statement:
```C#
using(var client = new BinanceClient())
{
}

using(var client = new BinanceSocketClient())
{
}
```

For most API methods Binance.Net provides two versions, synchronized and async calls. 

### Setting API credentials
For private endpoints (trading, order history, account info etc) an API key and secret has to be provided. For this the `SetApiCredentials` method can be used in both clients, or the credentials can be provided as arguments:
```C#
using(var client = new BinanceClient("APIKEY", "APISECRET"))
{
	client.SetApiCredentials("APIKEY", "APISECRET");
}
```
Alternatively the credentials can be set as default in BinanceDefaults to provide them to all new clients.
```C#
BinanceDefaults.SetDefaultApiCredentials("APIKEY", "APISECRET");
```
API credentials can be managed at https://www.binance.com/userCenter/createApi.html. Make sure to enable the required permission for the right API calls.

### Error handling
All API requests will respond with an BinanceApiResult object. This object contains whether the call was successful, the data returned from the call and an error if the call wasn't successful. As such, one should always check the Success flag when processing a response.
For example:
```C#
using(var client = new BinanceClient())
{
	var allPrices = client.GetAllPrices();
	if (allPrices.Success)
	{
		foreach (var price in allPrices.Data)
			Console.WriteLine($"{price.Symbol}: {price.Price}");
	}
	else
		Console.WriteLine($"Error: {allPrices.Error.Message}");
}
```
If not successful the Error object will contain an error code and an error message as to what went wrong. Error codes are divided in 3 categories. Any errors outside these categories are generated by Binance self.

* 5000 - 5999: input errors. There was a problem when validating the input for a method. Check the input and try again.
* 6000 - 6999: returned errors. The server returned a service error. If this error is persistent check if Binance is online or has any status updates regarding connectivity. If this issue persists please open an Issue.
* 7000 - 7999: output errors. The server returned data, but we we're unable to successfully parse the response. If this error is persistent please open an Issue.

The `BinanceClient` provides an automatic retry for when the server returns error status codes, for example when a gateway timeout occurs or the service is temporarily unavailable. 
The amount of retries can be set in the client by setting the `MaxRetry` property. This can also be set to a default value by the `SetDefaultRetries` funtion in `BinanceDefaults.`:
```C#
// On a single client
var client = new BinanceClient();
client.MaxRetries = 3;

// On all new clients:
BinanceDefaults.SetDefaultRetries(3);
```
The default amount of retries is 2, setting it to 0 will disable the functionality.

### Requests
**Public requests:**
```C#
using(var client = new BinanceClient())
{
	// Pings the API to check the connection
	var ping = client.Ping();
	// Gets the server time
	var serverTime = client.GetServerTime();
	// Gets info about the exchange state, including rate limit and symbol rules
	var exchangeInfo = client.GetExchangeInfo();
	// Gets the order book for specified symbol
	var orderBook = client.GetOrderBook("BNBBTC", 10);
	// Gets a compresed view of trades for specified symbol
	var aggTrades = client.GetAggregatedTrades("BNBBTC", startTime: DateTime.UtcNow.AddMinutes(-2), endTime: DateTime.UtcNow, limit: 10);
	// Gets the recently completed trades for a symbol
	var recentTrades = client.GetRecentTrades("BNBBTC");	
	// Gets the trade history for a symbol
	var historicalTrades = client.GetHistoricalTrades("BNBBTC");
	// Gets klines data for the specified symbol
	var klines = client.GetKlines("BNBBTC", KlineInterval.OneHour, startTime: DateTime.UtcNow.AddHours(-10), endTime: DateTime.UtcNow, limit: 10);
	// Gets prices and changes in the last 24 hours for specified symbol
	var price24h = client.Get24HPrice("BNBBTC");
	// Gets prices and changes in the last 24 hours for all symbols
	var prices24h = client.Get24HPricesList();
	// Gets the latest price of a symbol
	var price = client.GetPrice("BNBBTC");
	// Gets all symbols and latest prices
	var allPrices = client.GetAllPrices();
	// Gets book prices (asks/bids) for a symbol
	var bookPrice = client.GetBookPrice("BNBBTC");
	// Gets book prices (asks/bids) for all symbols
	var allBookPrices = client.GetAllBookPrices();
	
}
```

**Private requests:**

Api credentials have to be provided to use these methods
```C#
using(var client = new BinanceClient())
{
	// Gets all open orders for specified symbol
	var openOrders = client.GetOpenOrders("BNBBTC");
	// Gets all orders for specified symbol
	var allOrders = client.GetAllOrders("BNBBTC");
	// Places a test order to test the API functionality. No order will actually be placed
	var testOrderResult = client.PlaceTestOrder("BNBBTC", OrderSide.Buy, OrderType.Limit, 1, price: 1, timeInForce: TimeInForce.GoodTillCancel);
	// Request information about an order
	var queryOrder = client.QueryOrder("BNBBTC", allOrders.Data[0].OrderId);
	// Places an order. `price` is optional parameter, if not set, order price will use market value
	var orderResult = client.PlaceOrder("BNBBTC", OrderSide.Sell, OrderType.Limit, 10, price: 0.0002m, timeInForce: TimeInForce.GoodTillCancel);
	// Cancels an existing order
	var cancelResult = client.CancelOrder("BNBBTC", orderResult.Data.OrderId);
	// Gets information about your account
	var accountInfo = client.GetAccountInfo();
	// Gets all trades for specified symbol
	var myTrades = client.GetMyTrades("BNBBTC");
	// Gets your deposit history
	var depositHistory = client.GetDepositHistory();
	// Gets your withdraw history
	var withdrawalHistory = client.GetWithdrawHistory();
	// Requests a withdraw
	var withdraw = client.Withdraw("TEST", "Address", 1, "TestWithdraw");
	// Retrieve the deposit address for an asset
	var depositAddress = client.GetDepositAddress("BTC");
	// Starts the user stream
	var listenKeyResult = client.StartUserStream(listenKey);
	// Keeps alive the user stream. User streams get automatically closed after 1 hour if not kept alive, and every 24 hours by default
	var keepAliveResult = client.KeepAliveUserStream(listenKey);
	// Stops the user stream
	var stopResult = client.StopUserStream(listenKey);
}
```

### Websockets
The Binance.Net socket client provides several socket endpoint to which can be subsribed.

**Public socket endpoints:**
```C#
using(var client = new BinanceSocketClient())
{
	var successDepth = client.SubscribeToDepthStream("bnbbtc", (data) =>
	{
		// handle data
	});
	var successTrades = client.SubscribeToTradesStream("bnbbtc", (data) =>
	{
		// handle data
	});
	var successKline = client.SubscribeToKlineStream("bnbbtc", KlineInterval.OneMinute, (data) =>
	{
		// handle data
	});
	var successSymbol = client.SubscribeToSymbolTicker("bnbbtc", (data) =>
	{
		// handle data
	});
	var successSymbols = client.SubscribeToAllSymbolTicker((data) =>
	{
		// handle data
	});
	var successOrderBook = client.SubscribeToPartialBookDepthStream("bnbbtc", 10, (data) =>
	{
		// handle data
	});
}
```

**Private socket endpoints:**

For the private endpoint a user stream has to be started on the Binance server. This can be done using the `StartUserStream()` method in the `BinanceClient`. This command will return a listen key which can then be provided to the private socket subscription:
```C#
using(var client = new BinanceSocketClient())
{
	var successOrderBook = client.SubscribeToUserStream(listenKey, 
	(accountInfoUpdate) =>
	{
		// handle account info update
	},
	(orderInfoUpdate) =>
	{
		// handle order info update
	});
}
```

**Handling socket events**

Subscribing to a socket stream returns a BinanceStreamSubscription object. This object can be used to be notified when a socket closes or an error occures:
````C#
var sub = client.SubscribeToAllSymbolTicker(data =>
{
	Console.WriteLine("Reveived list update");
});

sub.Data.Closed += () =>
{
	Console.WriteLine("Socket closed");
};

sub.Data.Error += (e) =>
{
	Console.WriteLine("Socket error " + e.Message);
};
````

**Unsubscribing from socket endpoints:**

Sockets streams can be unsubscribed by using the `client.UnsubscribeFromStream` method in combination with the stream subscription received from subscribing:
```C#
using(var client = new BinanceSocketClient())
{
	var successDepth = client.SubscribeToDepthStream("bnbbtc", (data) =>
	{
		// handle data
	});

	client.UnsubscribeFromStream(successDepth.Data);
}
```

Additionaly, all sockets can be closed with the `UnsubscribeAllStreams` method. Beware that when a client is disposed the sockets are automatically disposed. This means that if the code is no longer in the using statement the eventhandler won't fire anymore. To prevent this from happening make sure the code doesn't leave the using statement or don't use the socket client in a using statement:
```C#
// Doesn't leave the using block
using(var client = new BinanceSocketClient())
{
	var successDepth = client.SubscribeToDepthStream("bnbbtc", (data) =>
	{
		// handle data
	});

	Console.ReadLine();
}

// Without using block
var client = new BinanceSocketClient();
client.SubscribeToDepthStream("bnbbtc", (data) =>
{
	// handle data
});
```

When no longer listening to private endpoints the `client.StopUserStream` method in `BinanceClient` should be used to signal the Binance server the stream can be closed.

### AutoTimestamp
For some private calls a timestamp has to be send to the Binance server. This timestamp in combination with the recvWindow parameter in the request will determine how long the request will be valid. If more than the recvWindow in miliseconds has passed since the provided timestamp the request will be rejected.

While testing I found that my local computer time was offset to the Binance server time, which made it reject all my requests. I added a fix for this in the Binance.Net client which will automatically calibrate the timestamp to the Binance server time. This behaviour is turned off by default and can be turned on using the `client.AutoTimeStamp` property. 

### TradeRulesBehaviour
The Binance API provides some basic filters to which orders should comply. These include a min/max price and quantity and step sizes. Binance.Net can automatically check these rules when placing an order, before actually sending it to the server. This is managed with the `TradeRulesBehaviour` property on `BinanceClient` (also available in `BinanceDefaults`).
There are 3 options: 

 * `None`: Binance.Net will not validate the order and send it to the Binance server as is.
 * `ThrowError`: Binance.Net will check if the order complies with the trade rules. If it does not the order will not be placed and an error is returned.
 * `AutoComply`: Binance.Net will check if the order complies with the trade rules. If it does not the invalid parameters will be automatically adjusted to the nearest valid value.

Note that if the `TradeRulesBehaviour` is set to `ThrowError` or `AutoComply` Binance.Net will internally do a `GetExchangeInfo` request initialy to request the current rules. After that it will update these rules every 60 minutes with new data by doing a new `GetExchangeInfo` request.
The interval at which this happens can be controlled by the `TradeRulesUpdateInterval` property (also available in `BinanceDefaults`).
 
### Logging
Binance.Net will by default log warning and error messages. To change the verbosity `SetLogVerbosity` can be called on a client. The default log verbosity for all new clients can also be set using the `SetDefaultLogVerbosity` in `BinanceDefaults`.

Binance.Net logging will default to logging to the Trace (Trace.WriteLine). This can be changed with the `SetLogOutput` method on clients. Alternatively a default output can be set in the `BinanceDefaults` using the `SetDefaultLogOutput` method:
```C#
BinanceDefaults.SetDefaultLogOutput(Console.Out);
BinanceDefaults.SetDefaultLogVerbosity(LogVerbosity.Debug);
```


## Release notes
* Version 2.3.4 - 12 feb 2018
	* Fix for AutoComply trading rules sending too much trailing zero's

* Version 2.3.3 - 10 feb 2018
	* Fix for stream order parsing

* Version 2.3.2 - 09 feb 2018
	* Changed base address from https://www.binance.com to https://api.binance.com to fix connection errors

* Version 2.3.1 - 08 feb 2018
	* Updated models to latest version
	* Cleaned code and code docs

* Version 2.3.0 - 07 feb 2018
	* Added missing fields to 24h prices
	* Changed subscription results from an id to an object with closed/error events
	* Changed how to subscribe to the user stream
	* Updated/fixed unit test project
	* Updated readme

* Version 2.2.5 - 24 jan 2018
	* Added optional automated checking of trading rules when placing an order
	* Added `BinanceHelpers` static class containing some basic helper functions
	* Fix for default logger not writing on a new line
	* Simplified internal defaults

* Version 2.2.4 - 23 jan 2018
	* Fix for RateLimit type in GetExchangeInfo
	* Split the BinanceSymbolFilter in 3 classes

* Version 2.2.3 - 15 jan 2018
	* Fix for calls freezing when made from UI thread

* Version 2.2.2 - 15 jan 2018
	* Fix in PlaceOrder using InvariantCulture
	* Fix for FirstId property in 24h price
	* Added symbol property to 24h price

* Version 2.2.1 - 12 jan 2018
	* Fix for parse error in StreamOrderUpdate

* Version 2.2.0 - 08 jan 2018
	* Updated according to latest documentation, adding various endpoints

* Version 2.1.3 - 9 nov 2017
	* Added automatic configurable retry on server errors
	* Refactor on error returns
	* Renamed ApiResult to BinanceApiResult
	
* Version 2.1.2 - 31 okt 2017
	* Added alot of code documentation
	* Small cleanups and fix

* Version 2.1.1 - 30 okt 2017
	* Fix for socket closing

* Version 2.1.0 - 30 okt 2017
	* Small rename/refactor, BinanceSocketClient also use ApiResult now

* Version 2.0.1 - 30 okt 2017
	* Improved error messages/handling in BinanceClient
	* Extra unit tests for failing requests

* Version 2.0.0 - 25 okt 2017
	* Changed from static class to object orriented, added IDisposable interface to be able to use `using` statements
	* Split websocket and restapi functionality in BinanceClient and BinanceSocketClient
	* Added method to set log output writer
	* Added abitlity to set defaults for new clients
	* Fixed unit tests for new setup
	* Updated documentation

* Version 1.1.2 - 25 okt 2017 
	* Added `UnsubscribeAllStreams` method

* Version 1.1.1 - 20 okt 2017 
	* Fix for withdrawal/deposit filter

* Version 1.1.0 - 20 okt 2017 
	* Updated withdrawal/deposit functionality according to API changes
	* Cleaned up BinanceClient a bit

* Version 1.0.9 - 19 okt 2017 
	* Added withdrawal/deposit functionality