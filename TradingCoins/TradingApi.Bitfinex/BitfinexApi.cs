using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using TradingApi.ModelObjects;
using TradingApi.ModelObjects.Bitfinex.Json;
using TradingApi.ModelObjects.Utility;
using System.Threading.Tasks;

namespace TradingApi.Bitfinex {
    public partial class BitfinexApi {
        private readonly string _apiSecret;
        private readonly string _apiKey;

        private const string ApiBfxKey = "X-BFX-APIKEY";
        private const string ApiBfxPayload = "X-BFX-PAYLOAD";
        private const string ApiBfxSig = "X-BFX-SIGNATURE";

        private const string SymbolDetailsRequestUrl = @"/v1/symbols_details";
        private const string BalanceRequestUrl = @"/v1/balances";
        private const string DepthOfBookRequestUrl = @"v1/book/";
        private const string NewOrderRequestUrl = @"/v1/order/new";
        private const string OrderStatusRequestUrl = @"/v1/order/status";
        private const string OrderCancelRequestUrl = @"/v1/order/cancel";
        private const string CancelAllRequestUrl = @"/all";
        private const string CancelReplaceRequestUrl = @"/replace";
        private const string MultipleRequestUrl = @"/multi";

        private const string ActiveOrdersRequestUrl = @"/v1/orders";
        private const string ActivePositionsRequestUrl = @"/v1/positions";
        private const string HistoryRequestUrl = @"/v1/history";
        private const string MyTradesRequestUrl = @"/v1/mytrades";

        private const string LendbookRequestUrl = @"/v1/lendbook/";
        private const string LendsRequestUrl = @"/v1/lends/";

        private const string DepositRequestUrl = @"/v1/deposit/new";
        private const string AccountInfoRequestUrl = "@/v1/account_infos";
        private const string MarginInfoRequstUrl = @"/v1/margin_infos";

        private const string NewOfferRequestUrl = @"/v1/offer/new";
        private const string CancelOfferRequestUrl = @"/v1/offer/cancel";
        private const string OfferStatusRequestUrl = @"/v1/offer/status";

        private const string ActiveOffersRequestUrl = @"/v1/offers";
        private const string ActiveCreditsRequestUrl = @"/v1/credits";

        private const string ActiveMarginSwapsRequestUrl = @"/v1/taken_swaps";
        private const string CloseSwapRequestUrl = @"/v1/swap/close";
        private const string ClaimPosRequestUrl = @"/v1/position/claim";

        private const string DefaulOrderExchangeType = "bitfinex";
        private const string DefaultLimitType = "exchange limit";
        private const string Buy = "buy";
        private const string Sell = "sell";

        public string BaseBitfinexUrl = @"https://api.bitfinex.com";

        public BitfinexApi(string apiSecret, string apiKey) {
            _apiSecret = apiSecret;
            _apiKey = apiKey;
            Logger.Log.InfoFormat("Connecting to Bitfinex Api with key: {0}", apiKey);
        }

        #region Unauthenticated Calls
        public BitfinexOrderBookGet GetOrderBook(BtcInfo.PairTypeEnum pairType) {
            try {
                var url = DepthOfBookRequestUrl + Enum.GetName(typeof(BtcInfo.PairTypeEnum), pairType);
                var response = GetBaseResponse(url);
                var orderBookResponseObj = JsonConvert.DeserializeObject<BitfinexOrderBookGet>(response.Content);
                OnOrderBookMsg(orderBookResponseObj);
                return orderBookResponseObj;
            } catch (Exception ex) {
                Logger.LogException(ex);
                return new BitfinexOrderBookGet();
            }
        }
        public async Task<BitfinexOrderBookGet> GetOrderBookAsync(BtcInfo.PairTypeEnum pairType) {
            try {
                var url = DepthOfBookRequestUrl + Enum.GetName(typeof(BtcInfo.PairTypeEnum), pairType);
                var response = await GetBaseResponseAsync(url);
                var orderBookResponseObj = JsonConvert.DeserializeObject<BitfinexOrderBookGet>(response.Content);
                OnOrderBookMsg(orderBookResponseObj);
                return orderBookResponseObj;
            } catch (Exception ex) {
                Logger.LogException(ex);
                return new BitfinexOrderBookGet();
            }
        }

        public IList<BitfinexSymbolDetailsResponse> GetSymbols() {
            var url = SymbolDetailsRequestUrl;
            var response = GetBaseResponse(url);
            var symbolsResponseObj = JsonConvert.DeserializeObject<IList<BitfinexSymbolDetailsResponse>>(response.Content);

            foreach (var bitfinexSymbolDetailsResponse in symbolsResponseObj)
                Logger.Log.InfoFormat("Symbol: {0}", bitfinexSymbolDetailsResponse);

            return symbolsResponseObj;
        }
        public async Task<IList<BitfinexSymbolDetailsResponse>> GetSymbolsAsync() {
            var url = SymbolDetailsRequestUrl;
            var response = await GetBaseResponseAsync(url);
            var symbolsResponseObj = JsonConvert.DeserializeObject<IList<BitfinexSymbolDetailsResponse>>(response.Content);

            foreach (var bitfinexSymbolDetailsResponse in symbolsResponseObj)
                Logger.Log.InfoFormat("Symbol: {0}", bitfinexSymbolDetailsResponse);

            return symbolsResponseObj;
        }

        public BitfinexPublicTickerGet GetPublicTicker(BtcInfo.PairTypeEnum pairType, BtcInfo.BitfinexUnauthenicatedCallsEnum callType) {
            var call = Enum.GetName(typeof(BtcInfo.BitfinexUnauthenicatedCallsEnum), callType);
            var symbol = Enum.GetName(typeof(BtcInfo.PairTypeEnum), pairType);
            var url = @"/v1/" + call.ToLower() + "/" + symbol.ToLower();
            var response = GetBaseResponse(url);

            var publicticketResponseObj = JsonConvert.DeserializeObject<BitfinexPublicTickerGet>(response.Content);
            Logger.Log.InfoFormat("Ticker: {0}", publicticketResponseObj);

            return publicticketResponseObj;
        }

        public async Task<BitfinexPublicTickerGet> GetPublicTickerAsync(BtcInfo.PairTypeEnum pairType, BtcInfo.BitfinexUnauthenicatedCallsEnum callType) {
            var call = Enum.GetName(typeof(BtcInfo.BitfinexUnauthenicatedCallsEnum), callType);
            var symbol = Enum.GetName(typeof(BtcInfo.PairTypeEnum), pairType);
            var url = @"/v1/" + call.ToLower() + "/" + symbol.ToLower();
            var response = await GetBaseResponseAsync(url);

            var publicticketResponseObj = JsonConvert.DeserializeObject<BitfinexPublicTickerGet>(response.Content);
            Logger.Log.InfoFormat("Ticker: {0}", publicticketResponseObj);

            return publicticketResponseObj;
        }

        public IList<BitfinexSymbolStatsResponse> GetPairStats(BtcInfo.PairTypeEnum pairType, BtcInfo.BitfinexUnauthenicatedCallsEnum callType) {
            var call = Enum.GetName(typeof(BtcInfo.BitfinexUnauthenicatedCallsEnum), callType);
            var symbol = Enum.GetName(typeof(BtcInfo.PairTypeEnum), pairType);
            var url = @"/v1/" + call.ToLower() + "/" + symbol.ToLower();
            var response = GetBaseResponse(url);

            var symbolStatsResponseObj = JsonConvert.DeserializeObject<IList<BitfinexSymbolStatsResponse>>(response.Content);

            foreach (var symbolStatsResponse in symbolStatsResponseObj)
                Logger.Log.InfoFormat("Pair Stats: {0}", symbolStatsResponse);

            return symbolStatsResponseObj;
        }

        public async Task<IList<BitfinexSymbolStatsResponse>> GetPairStatsAsync(BtcInfo.PairTypeEnum pairType, BtcInfo.BitfinexUnauthenicatedCallsEnum callType) {
            var call = Enum.GetName(typeof(BtcInfo.BitfinexUnauthenicatedCallsEnum), callType);
            var symbol = Enum.GetName(typeof(BtcInfo.PairTypeEnum), pairType);
            var url = @"/v1/" + call.ToLower() + "/" + symbol.ToLower();
            var response = await GetBaseResponseAsync(url);

            var symbolStatsResponseObj = JsonConvert.DeserializeObject<IList<BitfinexSymbolStatsResponse>>(response.Content);

            foreach (var symbolStatsResponse in symbolStatsResponseObj)
                Logger.Log.InfoFormat("Pair Stats: {0}", symbolStatsResponse);

            return symbolStatsResponseObj;
        }

        public IList<BitfinexTradesGet> GetPairTrades(BtcInfo.PairTypeEnum pairType, BtcInfo.BitfinexUnauthenicatedCallsEnum callType) {
            var call = Enum.GetName(typeof(BtcInfo.BitfinexUnauthenicatedCallsEnum), callType);
            var symbol = Enum.GetName(typeof(BtcInfo.PairTypeEnum), pairType);
            var url = @"/v1/" + call.ToLower() + "/" + symbol.ToLower();
            var response = GetBaseResponse(url);

            var pairTradesResponseObj = JsonConvert.DeserializeObject<IList<BitfinexTradesGet>>(response.Content);

            foreach (var pairTrade in pairTradesResponseObj)
                Logger.Log.InfoFormat("Pair Trade: {0}", pairTrade);

            return pairTradesResponseObj;
        }

        public async Task<IList<BitfinexTradesGet>> GetPairTradesAsync(BtcInfo.PairTypeEnum pairType, BtcInfo.BitfinexUnauthenicatedCallsEnum callType) {
            var call = Enum.GetName(typeof(BtcInfo.BitfinexUnauthenicatedCallsEnum), callType);
            var symbol = Enum.GetName(typeof(BtcInfo.PairTypeEnum), pairType);
            var url = @"/v1/" + call.ToLower() + "/" + symbol.ToLower();
            var response = await GetBaseResponseAsync(url);

            var pairTradesResponseObj = JsonConvert.DeserializeObject<IList<BitfinexTradesGet>>(response.Content);

            foreach (var pairTrade in pairTradesResponseObj)
                Logger.Log.InfoFormat("Pair Trade: {0}", pairTrade);

            return pairTradesResponseObj;
        }

        /// <summary>
        /// symbol = ExchangeSymbolEnum
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IList<BitfinexLendsResponse> GetLends(string symbol) {
            var url = LendsRequestUrl + symbol;
            var response = GetBaseResponse(url);

            var lendResponseObj = JsonConvert.DeserializeObject<IList<BitfinexLendsResponse>>(response.Content);
            OnLendsResponseMsg(lendResponseObj);
            return lendResponseObj;
        }

        public async Task<IList<BitfinexLendsResponse>> GetLendsAsync(string symbol) {
            var url = LendsRequestUrl + symbol;
            var response = await GetBaseResponseAsync(url);

            var lendResponseObj = JsonConvert.DeserializeObject<IList<BitfinexLendsResponse>>(response.Content);
            OnLendsResponseMsg(lendResponseObj);
            return lendResponseObj;
        }

        public BitfinexLendbookResponse GetLendbook(string symbol) {
            var url = LendbookRequestUrl + symbol;
            var response = GetBaseResponse(url);

            var lendBookResponseObj = JsonConvert.DeserializeObject<BitfinexLendbookResponse>(response.Content);
            OnLendbookResponseMsg(lendBookResponseObj);
            return lendBookResponseObj;
        }

        public async Task<BitfinexLendbookResponse> GetLendbookAsync(string symbol) {
            var url = LendbookRequestUrl + symbol;
            var response = await GetBaseResponseAsync(url);

            var lendBookResponseObj = JsonConvert.DeserializeObject<BitfinexLendbookResponse>(response.Content);
            OnLendbookResponseMsg(lendBookResponseObj);
            return lendBookResponseObj;
        }

        #endregion

        #region Sending Crypto Orders

        public BitfinexMultipleNewOrderResponse SendMultipleOrders(BitfinexNewOrderPost[] orders) {
            try {
                var multipleOrdersPost = new BitfinexMultipleNewOrdersPost {
                    Request = NewOrderRequestUrl + MultipleRequestUrl,
                    Nonce = Common.UnixTimeStampUtc().ToString(),
                    Orders = orders
                };

                var client = GetRestClient(multipleOrdersPost.Request);
                var response = GetRestResponse(client, multipleOrdersPost);

                var multipleOrderResponseObj = JsonConvert.DeserializeObject<BitfinexMultipleNewOrderResponse>(response.Content);
                OnMultipleOrderFeedMsg(multipleOrderResponseObj);

                Logger.Log.Info("Sending Multiple Orders:");
                foreach (var order in orders)
                    Logger.Log.Info(order.ToString());

                return multipleOrderResponseObj;

            } catch (Exception ex) {
                Logger.LogException(ex);
                return null;
            }
        }

        public async Task<BitfinexMultipleNewOrderResponse> SendMultipleOrdersAsync(BitfinexNewOrderPost[] orders) {
            try {
                var multipleOrdersPost = new BitfinexMultipleNewOrdersPost {
                    Request = NewOrderRequestUrl + MultipleRequestUrl,
                    Nonce = Common.UnixTimeStampUtc().ToString(),
                    Orders = orders
                };

                var client = GetRestClient(multipleOrdersPost.Request);
                var response = await GetRestResponseAsync(client, multipleOrdersPost);

                var multipleOrderResponseObj = JsonConvert.DeserializeObject<BitfinexMultipleNewOrderResponse>(response.Content);
                OnMultipleOrderFeedMsg(multipleOrderResponseObj);

                Logger.Log.Info("Sending Multiple Orders:");
                foreach (var order in orders)
                    Logger.Log.Info(order.ToString());

                return multipleOrderResponseObj;

            } catch (Exception ex) {
                Logger.LogException(ex);
                return null;
            }
        }

        public BitfinexNewOrderResponse SendOrder(BitfinexNewOrderPost newOrder) {
            try {
                newOrder.Request = NewOrderRequestUrl;
                newOrder.Nonce = Common.UnixTimeStampUtc().ToString();

                var client = GetRestClient(NewOrderRequestUrl);
                var response = GetRestResponse(client, newOrder);

                var newOrderResponseObj = JsonConvert.DeserializeObject<BitfinexNewOrderResponse>(response.Content);
                OnOrderFeedMsg(newOrderResponseObj);

                Logger.Log.InfoFormat("Sending New Order: {0}", newOrder.ToString());
                Logger.Log.InfoFormat("Response from Exchange: {0}", newOrderResponseObj);

                return newOrderResponseObj;
            } catch (Exception ex) {
                Logger.LogException(ex);
                return null;
            }
        }

        public async Task<BitfinexNewOrderResponse> SendOrderAsync(BitfinexNewOrderPost newOrder) {
            try {
                newOrder.Request = NewOrderRequestUrl;
                newOrder.Nonce = Common.UnixTimeStampUtc().ToString();

                var client = GetRestClient(NewOrderRequestUrl);
                var response = await GetRestResponseAsync(client, newOrder);

                var newOrderResponseObj = JsonConvert.DeserializeObject<BitfinexNewOrderResponse>(response.Content);
                OnOrderFeedMsg(newOrderResponseObj);

                Logger.Log.InfoFormat("Sending New Order: {0}", newOrder.ToString());
                Logger.Log.InfoFormat("Response from Exchange: {0}", newOrderResponseObj);

                return newOrderResponseObj;
            } catch (Exception ex) {
                Logger.LogException(ex);
                return null;
            }
        }

        public BitfinexNewOrderResponse SendOrder(string symbol, string amount, string price, string exchange, string side, string type, bool isHidden) {
            var newOrder = new BitfinexNewOrderPost() {
                Symbol = symbol,
                Amount = amount,
                Price = price,
                Exchange = exchange,
                Side = side,
                Type = type//,
                           //IsHidden = isHidden.ToString()
            };
            return SendOrder(newOrder);
        }

        public async Task<BitfinexNewOrderResponse> SendOrderAsync(string symbol, string amount, string price, string exchange, string side, string type, bool isHidden) {
            var newOrder = new BitfinexNewOrderPost() {
                Symbol = symbol,
                Amount = amount,
                Price = price,
                Exchange = exchange,
                Side = side,
                Type = type//,
                           //IsHidden = isHidden.ToString()
            };
            return await SendOrderAsync(newOrder);
        }

        public BitfinexNewOrderResponse SendSimpleLimit(string symbol, string amount, string price, string side, bool isHidden = false) {
            return SendOrder(symbol, amount, price, DefaulOrderExchangeType, side, DefaultLimitType, isHidden);
        }

        public async Task<BitfinexNewOrderResponse> SendSimpleLimitAsync(string symbol, string amount, string price, string side, bool isHidden = false) {
            return await SendOrderAsync(symbol, amount, price, DefaulOrderExchangeType, side, DefaultLimitType, isHidden);
        }

        public BitfinexNewOrderResponse SendSimpleLimitBuy(string symbol, string amount, string price, bool isHidden = false) {
            return SendOrder(symbol, amount, price, DefaulOrderExchangeType, Buy, DefaultLimitType, isHidden);
        }

        public async Task<BitfinexNewOrderResponse> SendSimpleLimitBuyAsync(string symbol, string amount, string price, bool isHidden = false) {
            return await SendOrderAsync(symbol, amount, price, DefaulOrderExchangeType, Buy, DefaultLimitType, isHidden);
        }

        public BitfinexNewOrderResponse SendSimpleLimitSell(string symbol, string amount, string price, bool isHidden = false) {
            return SendOrder(symbol, amount, price, DefaulOrderExchangeType, Sell, DefaultLimitType, isHidden);
        }

        public async Task<BitfinexNewOrderResponse> SendSimpleLimitSellAsync(string symbol, string amount, string price, bool isHidden = false) {
            return await SendOrderAsync(symbol, amount, price, DefaulOrderExchangeType, Sell, DefaultLimitType, isHidden);
        }

        #endregion

        #region Cancel Crypto Orders

        public BitfinexOrderStatusResponse CancelOrder(int orderId) {
            var cancelPost = new BitfinexOrderStatusPost {
                Request = OrderCancelRequestUrl,

                Nonce = Common.UnixTimeStampUtc().ToString(),
                OrderId = orderId
            };

            var client = GetRestClient(cancelPost.Request);
            var response = GetRestResponse(client, cancelPost);
            var orderCancelResponseObj = JsonConvert.DeserializeObject<BitfinexOrderStatusResponse>(response.Content);
            OnCancelOrderMsg(orderCancelResponseObj);

            Logger.Log.InfoFormat("Cancel OrderId: {0}, Response From Exchange: {1}", orderId, orderCancelResponseObj.ToString());

            return orderCancelResponseObj;
        }

        public async Task<BitfinexOrderStatusResponse> CancelOrderAsync(int orderId) {
            var cancelPost = new BitfinexOrderStatusPost {
                Request = OrderCancelRequestUrl,

                Nonce = Common.UnixTimeStampUtc().ToString(),
                OrderId = orderId
            };

            var client = GetRestClient(cancelPost.Request);
            var response = await GetRestResponseAsync(client, cancelPost);
            var orderCancelResponseObj = JsonConvert.DeserializeObject<BitfinexOrderStatusResponse>(response.Content);
            OnCancelOrderMsg(orderCancelResponseObj);

            Logger.Log.InfoFormat("Cancel OrderId: {0}, Response From Exchange: {1}", orderId, orderCancelResponseObj.ToString());

            return orderCancelResponseObj;
        }

        public BitfinexCancelReplaceOrderResponse CancelReplaceOrder(int cancelOrderId, BitfinexNewOrderPost newOrder) {
            var replaceOrder = new BitfinexCancelReplacePost() {
                Amount = newOrder.Amount,
                CancelOrderId = cancelOrderId,
                Exchange = newOrder.Exchange,
                Price = newOrder.Price,
                Side = newOrder.Side,
                Symbol = newOrder.Symbol,
                Type = newOrder.Type
            };
            return CancelReplaceOrder(replaceOrder);
        }

        public async Task<BitfinexCancelReplaceOrderResponse> CancelReplaceOrderAsync(int cancelOrderId, BitfinexNewOrderPost newOrder) {
            var replaceOrder = new BitfinexCancelReplacePost() {
                Amount = newOrder.Amount,
                CancelOrderId = cancelOrderId,
                Exchange = newOrder.Exchange,
                Price = newOrder.Price,
                Side = newOrder.Side,
                Symbol = newOrder.Symbol,
                Type = newOrder.Type
            };
            return await CancelReplaceOrderAsync(replaceOrder);
        }

        public BitfinexCancelReplaceOrderResponse CancelReplaceOrder(BitfinexCancelReplacePost replaceOrder) {
            replaceOrder.Request = OrderCancelRequestUrl + CancelReplaceRequestUrl;
            replaceOrder.Nonce = Common.UnixTimeStampUtc().ToString();

            var client = GetRestClient(replaceOrder.Request);
            var response = GetRestResponse(client, replaceOrder);

            var replaceOrderResponseObj = JsonConvert.DeserializeObject<BitfinexCancelReplaceOrderResponse>(response.Content);
            replaceOrderResponseObj.OriginalOrderId = replaceOrder.CancelOrderId;
            OnCancelReplaceFeedMsg(replaceOrderResponseObj);

            Logger.Log.InfoFormat("Cancel Replace: {0}");
            Logger.Log.InfoFormat("Response From Exchange: {0}", replaceOrderResponseObj.ToString());

            return replaceOrderResponseObj;
        }

        public async Task<BitfinexCancelReplaceOrderResponse> CancelReplaceOrderAsync(BitfinexCancelReplacePost replaceOrder) {
            replaceOrder.Request = OrderCancelRequestUrl + CancelReplaceRequestUrl;
            replaceOrder.Nonce = Common.UnixTimeStampUtc().ToString();

            var client = GetRestClient(replaceOrder.Request);
            var response = await GetRestResponseAsync(client, replaceOrder);

            var replaceOrderResponseObj = JsonConvert.DeserializeObject<BitfinexCancelReplaceOrderResponse>(response.Content);
            replaceOrderResponseObj.OriginalOrderId = replaceOrder.CancelOrderId;
            OnCancelReplaceFeedMsg(replaceOrderResponseObj);

            Logger.Log.InfoFormat("Cancel Replace: {0}");
            Logger.Log.InfoFormat("Response From Exchange: {0}", replaceOrderResponseObj.ToString());

            return replaceOrderResponseObj;
        }

        public string CancelMultipleOrders(int[] intArr) {
            var cancelMultiplePost = new BitfinexCancelMultipleOrderPost {
                Request = OrderCancelRequestUrl + MultipleRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                OrderIds = intArr
            };

            var client = GetRestClient(cancelMultiplePost.Request);
            var response = GetRestResponse(client, cancelMultiplePost);
            OnCancelMultipleOrdersMsg(response.Content);

            var str = new StringBuilder();

            foreach (var cancelOrderId in intArr)
                str.Append(cancelOrderId + ", ");

            Logger.Log.InfoFormat("Cancelling the following orders: {0}", str.ToString());
            Logger.Log.InfoFormat("Response From Exchange: {0}", response.Content);

            return response.Content;
        }

        public async Task<string> CancelMultipleOrdersAsync(int[] intArr) {
            var cancelMultiplePost = new BitfinexCancelMultipleOrderPost {
                Request = OrderCancelRequestUrl + MultipleRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                OrderIds = intArr
            };

            var client = GetRestClient(cancelMultiplePost.Request);
            var response = await GetRestResponseAsync(client, cancelMultiplePost);
            OnCancelMultipleOrdersMsg(response.Content);

            var str = new StringBuilder();

            foreach (var cancelOrderId in intArr)
                str.Append(cancelOrderId + ", ");

            Logger.Log.InfoFormat("Cancelling the following orders: {0}", str.ToString());
            Logger.Log.InfoFormat("Response From Exchange: {0}", response.Content);

            return response.Content;
        }

        public string CancellAllActiveOrders() {
            var url = OrderCancelRequestUrl + CancelAllRequestUrl;
            var cancelAllPost = new BitfinexPostBase {
                Request = url,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(url);
            var response = GetRestResponse(client, cancelAllPost);
            OnCancelAllActiveOrdersMsg(response.Content);
            return response.Content;
        }

        public async Task<string> CancellAllActiveOrdersAsync() {
            var url = OrderCancelRequestUrl + CancelAllRequestUrl;
            var cancelAllPost = new BitfinexPostBase {
                Request = url,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(url);
            var response = await GetRestResponseAsync(client, cancelAllPost);
            OnCancelAllActiveOrdersMsg(response.Content);
            return response.Content;
        }

        #endregion

        #region Trading Info
        public IList<BitfinexMarginPositionResponse> GetActiveOrders() {
            var activeOrdersPost = new BitfinexPostBase {
                Request = ActiveOrdersRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(activeOrdersPost.Request);
            var response = GetRestResponse(client, activeOrdersPost);
            var activeOrdersResponseObj = JsonConvert.DeserializeObject<IList<BitfinexMarginPositionResponse>>(response.Content);
            OnActiveOrdersMsg(activeOrdersResponseObj);

            Logger.Log.InfoFormat("Active Orders:");
            foreach (var activeOrder in activeOrdersResponseObj)
                Logger.Log.InfoFormat("Order: {0}", activeOrder.ToString());

            return activeOrdersResponseObj;
        }

        public async Task<IList<BitfinexMarginPositionResponse>> GetActiveOrdersAsync() {
            var activeOrdersPost = new BitfinexPostBase {
                Request = ActiveOrdersRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(activeOrdersPost.Request);
            var response = await GetRestResponseAsync(client, activeOrdersPost);
            var activeOrdersResponseObj = JsonConvert.DeserializeObject<IList<BitfinexMarginPositionResponse>>(response.Content);
            OnActiveOrdersMsg(activeOrdersResponseObj);

            Logger.Log.InfoFormat("Active Orders:");
            foreach (var activeOrder in activeOrdersResponseObj)
                Logger.Log.InfoFormat("Order: {0}", activeOrder.ToString());

            return activeOrdersResponseObj;
        }

        public IList<BitfinexHistoryResponse> GetHistory(string currency, string since, string until, int limit, string wallet) {
            var historyPost = new BitfinexHistoryPost {
                Request = HistoryRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                Currency = currency,
                Since = since,
                Until = until,
                Limit = limit,
                Wallet = wallet
            };

            var client = GetRestClient(historyPost.Request);
            var response = GetRestResponse(client, historyPost);
            var historyResponseObj = JsonConvert.DeserializeObject<IList<BitfinexHistoryResponse>>(response.Content);
            OnHistoryMsg(historyResponseObj);

            Logger.Log.InfoFormat("History:");
            foreach (var history in historyResponseObj)
                Logger.Log.InfoFormat("{0}", history);

            return historyResponseObj;
        }

        public async Task<IList<BitfinexHistoryResponse>> GetHistoryAsync(string currency, string since, string until, int limit, string wallet) {
            var historyPost = new BitfinexHistoryPost {
                Request = HistoryRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                Currency = currency,
                Since = since,
                Until = until,
                Limit = limit,
                Wallet = wallet
            };

            var client = GetRestClient(historyPost.Request);
            var response = await GetRestResponseAsync(client, historyPost);
            var historyResponseObj = JsonConvert.DeserializeObject<IList<BitfinexHistoryResponse>>(response.Content);
            OnHistoryMsg(historyResponseObj);

            Logger.Log.InfoFormat("History:");
            foreach (var history in historyResponseObj)
                Logger.Log.InfoFormat("{0}", history);

            return historyResponseObj;
        }

        public IList<BitfinexMyTradesResponse> GetMyTrades(string symbol, string timestamp, int limit) {
            var myTradesPost = new BitfinexMyTradesPost {
                Request = MyTradesRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                Symbol = symbol,
                Timestamp = timestamp,
                Limit = limit
            };

            var client = GetRestClient(myTradesPost.Request);
            var response = GetRestResponse(client, myTradesPost);

            var myTradesResponseObj = JsonConvert.DeserializeObject<IList<BitfinexMyTradesResponse>>(response.Content);
            OnMyTradesMsg(myTradesResponseObj);

            Logger.Log.InfoFormat("My Trades:");
            foreach (var myTrade in myTradesResponseObj)
                Logger.Log.InfoFormat("Trade: {0}", myTrade);

            return myTradesResponseObj;
        }

        public async Task<IList<BitfinexMyTradesResponse>> GetMyTradesAsync(string symbol, string timestamp, int limit) {
            var myTradesPost = new BitfinexMyTradesPost {
                Request = MyTradesRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                Symbol = symbol,
                Timestamp = timestamp,
                Limit = limit
            };

            var client = GetRestClient(myTradesPost.Request);
            var response = await GetRestResponseAsync(client, myTradesPost);

            var myTradesResponseObj = JsonConvert.DeserializeObject<IList<BitfinexMyTradesResponse>>(response.Content);
            OnMyTradesMsg(myTradesResponseObj);

            Logger.Log.InfoFormat("My Trades:");
            foreach (var myTrade in myTradesResponseObj)
                Logger.Log.InfoFormat("Trade: {0}", myTrade);

            return myTradesResponseObj;
        }

        public BitfinexOrderStatusResponse GetOrderStatus(int orderId) {
            var orderStatusPost = new BitfinexOrderStatusPost {
                Request = OrderStatusRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                OrderId = orderId
            };

            var client = GetRestClient(OrderStatusRequestUrl);
            var response = GetRestResponse(client, orderStatusPost);
            var orderStatusResponseObj = JsonConvert.DeserializeObject<BitfinexOrderStatusResponse>(response.Content);
            OnOrderStatusMsg(orderStatusResponseObj);

            Logger.Log.InfoFormat("OrderId: {0} Status: {1}", orderId, orderStatusResponseObj.ToString());

            return orderStatusResponseObj;
        }

        public async Task<BitfinexOrderStatusResponse> GetOrderStatusAsync(int orderId) {
            var orderStatusPost = new BitfinexOrderStatusPost {
                Request = OrderStatusRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                OrderId = orderId
            };

            var client = GetRestClient(OrderStatusRequestUrl);
            var response = await GetRestResponseAsync(client, orderStatusPost);
            var orderStatusResponseObj = JsonConvert.DeserializeObject<BitfinexOrderStatusResponse>(response.Content);
            OnOrderStatusMsg(orderStatusResponseObj);

            Logger.Log.InfoFormat("OrderId: {0} Status: {1}", orderId, orderStatusResponseObj.ToString());

            return orderStatusResponseObj;
        }

        #endregion

        #region Account Information

        public IList<BitfinexBalanceResponse> GetBalances() {
            try {
                var balancePost = new BitfinexPostBase {
                    Request = BalanceRequestUrl,
                    Nonce = Common.UnixTimeStampUtc().ToString()
                };

                var client = GetRestClient(BalanceRequestUrl);
                var response = GetRestResponse(client, balancePost);

                var balancesObj = JsonConvert.DeserializeObject<IList<BitfinexBalanceResponse>>(response.Content);
                OnBalanceResponseMsg(balancesObj);

                Logger.Log.InfoFormat("Balances:");
                foreach (var balance in balancesObj)
                    Logger.Log.Info(balance);

                return balancesObj;
            } catch (Exception ex) {
                Logger.LogException(ex);
                return null;
            }
        }

        public async Task<IList<BitfinexBalanceResponse>> GetBalancesAsync() {
            try {
                var balancePost = new BitfinexPostBase {
                    Request = BalanceRequestUrl,
                    Nonce = Common.UnixTimeStampUtc().ToString()
                };

                var client = GetRestClient(BalanceRequestUrl);
                var response = await GetRestResponseAsync(client, balancePost);

                var balancesObj = JsonConvert.DeserializeObject<IList<BitfinexBalanceResponse>>(response.Content);
                OnBalanceResponseMsg(balancesObj);

                Logger.Log.InfoFormat("Balances:");
                foreach (var balance in balancesObj)
                    Logger.Log.Info(balance);

                return balancesObj;
            } catch (Exception ex) {
                Logger.LogException(ex);
                return null;
            }
        }


        /// <summary>
        /// currency = upper case ExchangeSymbolEnum
        /// method = lower case ExchangeSymbolNameEnum
        /// wallet = BitfinexWalletEnum
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="method"></param>
        /// <param name="wallet"></param>
        /// <returns></returns>
        public BitfinexDepositResponse Deposit(string currency, string method, string wallet) {
            var depositPost = new BitfinexDepositPost {
                Request = DepositRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                Currency = currency,
                Method = method,
                WalletName = wallet
            };

            var client = GetRestClient(depositPost.Request);
            var response = GetRestResponse(client, depositPost);

            var depositResponseObj = JsonConvert.DeserializeObject<BitfinexDepositResponse>(response.Content);
            Logger.Log.InfoFormat("Attempting to deposit: {0} with method: {1} to wallet: {2}", currency, method, wallet);
            Logger.Log.InfoFormat("Response from exchange: {0}", depositResponseObj);
            return depositResponseObj;
        }

        public async Task<BitfinexDepositResponse> DepositAsync(string currency, string method, string wallet) {
            var depositPost = new BitfinexDepositPost {
                Request = DepositRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                Currency = currency,
                Method = method,
                WalletName = wallet
            };

            var client = GetRestClient(depositPost.Request);
            var response = await GetRestResponseAsync(client, depositPost);

            var depositResponseObj = JsonConvert.DeserializeObject<BitfinexDepositResponse>(response.Content);
            Logger.Log.InfoFormat("Attempting to deposit: {0} with method: {1} to wallet: {2}", currency, method, wallet);
            Logger.Log.InfoFormat("Response from exchange: {0}", depositResponseObj);
            return depositResponseObj;
        }

        /// <summary>
        /// This never worked for me...
        /// </summary>
        /// <returns></returns>
        public object GetAccountInformation() {
            var accountPost = new BitfinexPostBase {
                Request = AccountInfoRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(accountPost.Request);
            var response = GetRestResponse(client, accountPost);
            Logger.Log.InfoFormat("Account Information: {0}", response.Content);
            return response.Content;
        }
        public async Task<object> GetAccountInformationAsync() {
            var accountPost = new BitfinexPostBase {
                Request = AccountInfoRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(accountPost.Request);
            var response = await GetRestResponseAsync(client, accountPost);
            Logger.Log.InfoFormat("Account Information: {0}", response.Content);
            return response.Content;
        }

        public BitfinexMarginInfoResponse GetMarginInformation() {
            var marginPost = new BitfinexPostBase {
                Request = MarginInfoRequstUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(marginPost.Request);
            var response = GetRestResponse(client, marginPost);

            var jArr = JsonConvert.DeserializeObject(response.Content) as JArray;
            if (jArr == null || jArr.Count == 0)
                return null;

            var marginInfoStr = jArr[0].ToString();
            var marginInfoResponseObj = JsonConvert.DeserializeObject<BitfinexMarginInfoResponse>(marginInfoStr);
            OnMarginInformationMsg(marginInfoResponseObj);

            Logger.Log.InfoFormat("Margin Info: {0}", marginInfoResponseObj.ToString());

            return marginInfoResponseObj;
        }

        public async Task<BitfinexMarginInfoResponse> GetMarginInformationAsync() {
            var marginPost = new BitfinexPostBase {
                Request = MarginInfoRequstUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(marginPost.Request);
            var response = await GetRestResponseAsync(client, marginPost);

            var jArr = JsonConvert.DeserializeObject(response.Content) as JArray;
            if (jArr == null || jArr.Count == 0)
                return null;

            var marginInfoStr = jArr[0].ToString();
            var marginInfoResponseObj = JsonConvert.DeserializeObject<BitfinexMarginInfoResponse>(marginInfoStr);
            OnMarginInformationMsg(marginInfoResponseObj);

            Logger.Log.InfoFormat("Margin Info: {0}", marginInfoResponseObj.ToString());

            return marginInfoResponseObj;
        }

        public IList<BitfinexMarginPositionResponse> GetActivePositions() {
            var activePositionsPost = new BitfinexPostBase {
                Request = ActivePositionsRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(activePositionsPost.Request);
            var response = GetRestResponse(client, activePositionsPost);

            var activePositionsResponseObj = JsonConvert.DeserializeObject<IList<BitfinexMarginPositionResponse>>(response.Content);
            OnActivePositionsMsg(activePositionsResponseObj);

            Logger.Log.InfoFormat("Active Positions: ");
            foreach (var activePos in activePositionsResponseObj)
                Logger.Log.InfoFormat("Position: {0}", activePos);

            return activePositionsResponseObj;
        }

        public async Task<IList<BitfinexMarginPositionResponse>> GetActivePositionsAsync() {
            var activePositionsPost = new BitfinexPostBase {
                Request = ActivePositionsRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(activePositionsPost.Request);
            var response = await GetRestResponseAsync(client, activePositionsPost);

            var activePositionsResponseObj = JsonConvert.DeserializeObject<IList<BitfinexMarginPositionResponse>>(response.Content);
            OnActivePositionsMsg(activePositionsResponseObj);

            Logger.Log.InfoFormat("Active Positions: ");
            foreach (var activePos in activePositionsResponseObj)
                Logger.Log.InfoFormat("Position: {0}", activePos);

            return activePositionsResponseObj;
        }

        #endregion

        #region Lending and Borrowing Execution

        /// <summary>
        /// rate is the yearly rate. So if you want to borrow/lend at 10 basis points per day you would 
        /// pass in 36.5 as the rate (10 * 365). Also, lend = lend (aka offer swap), loan = borrow (aka receive swap)
        /// The newOffer's currency propery = ExchangeSymbolEnum uppercase.
        /// </summary>
        /// <param name="newOffer"></param>
        /// <returns></returns>
        public BitfinexOfferStatusResponse SendNewOffer(BitfinexNewOfferPost newOffer) {
            newOffer.Request = NewOfferRequestUrl;
            newOffer.Nonce = Common.UnixTimeStampUtc().ToString();

            var client = GetRestClient(NewOfferRequestUrl);
            var response = GetRestResponse(client, newOffer);

            var newOfferResponseObj = JsonConvert.DeserializeObject<BitfinexOfferStatusResponse>(response.Content);
            OnNewOfferStatusMsg(newOfferResponseObj);

            Logger.Log.InfoFormat("Sending New Offer: {0}", newOffer.ToString());
            Logger.Log.InfoFormat("Response From Exchange: {0}", newOfferResponseObj);
            return newOfferResponseObj;
        }

        public async Task<BitfinexOfferStatusResponse> SendNewOfferAsync(BitfinexNewOfferPost newOffer) {
            newOffer.Request = NewOfferRequestUrl;
            newOffer.Nonce = Common.UnixTimeStampUtc().ToString();

            var client = GetRestClient(NewOfferRequestUrl);
            var response = await GetRestResponseAsync(client, newOffer);

            var newOfferResponseObj = JsonConvert.DeserializeObject<BitfinexOfferStatusResponse>(response.Content);
            OnNewOfferStatusMsg(newOfferResponseObj);

            Logger.Log.InfoFormat("Sending New Offer: {0}", newOffer.ToString());
            Logger.Log.InfoFormat("Response From Exchange: {0}", newOfferResponseObj);
            return newOfferResponseObj;
        }

        /// <summary>
        /// rate is the yearly rate. So if you want to borrow/lend at 10 basis points per day you would 
        /// pass in 36.5 as the rate (10 * 365). Also, lend = lend (aka offer swap), loan = borrow (aka receive swap)
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <param name="rate"></param>
        /// <param name="period"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public BitfinexOfferStatusResponse SendNewOffer(string currency, string amount, string rate, int period, string direction) {
            var newOffer = new BitfinexNewOfferPost() {
                Amount = amount,
                Currency = currency,
                Rate = rate,
                Period = period,
                Direction = direction
            };
            return SendNewOffer(newOffer);
        }

        public async Task<BitfinexOfferStatusResponse> SendNewOfferAsync(string currency, string amount, string rate, int period, string direction) {
            var newOffer = new BitfinexNewOfferPost() {
                Amount = amount,
                Currency = currency,
                Rate = rate,
                Period = period,
                Direction = direction
            };
            return await SendNewOfferAsync(newOffer);
        }

        /// <summary>
        /// Note: bug with bitfinex Canceloffer - the object returned will still say offer is alive and not cancelled.
        /// If you execute a 'GetOfferStatus' after the cancel is alive will be true (aka the offer will show up as cancelled. 
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public BitfinexOfferStatusResponse CancelOffer(int offerId) {
            var cancelPost = new BitfinexOfferStatusPost {
                Request = CancelOfferRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),

                OfferId = offerId
            };

            var client = GetRestClient(cancelPost.Request);
            var response = GetRestResponse(client, cancelPost);
            var orderCancelResponseObj = JsonConvert.DeserializeObject<BitfinexOfferStatusResponse>(response.Content);
            OnCancelOfferMsg(orderCancelResponseObj);

            Logger.Log.InfoFormat("Cancelling offerId: {0}. Exchange response: {1}", offerId, orderCancelResponseObj.ToString());

            return orderCancelResponseObj;
        }

        public async Task<BitfinexOfferStatusResponse> CancelOfferAsync(int offerId) {
            var cancelPost = new BitfinexOfferStatusPost {
                Request = CancelOfferRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),

                OfferId = offerId
            };

            var client = GetRestClient(cancelPost.Request);
            var response = await GetRestResponseAsync(client, cancelPost);
            var orderCancelResponseObj = JsonConvert.DeserializeObject<BitfinexOfferStatusResponse>(response.Content);
            OnCancelOfferMsg(orderCancelResponseObj);

            Logger.Log.InfoFormat("Cancelling offerId: {0}. Exchange response: {1}", offerId, orderCancelResponseObj.ToString());

            return orderCancelResponseObj;
        }

        public BitfinexOfferStatusResponse GetOfferStatus(int offerId) {
            var statusPost = new BitfinexOfferStatusPost {
                Request = OfferStatusRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),

                OfferId = offerId
            };

            var client = GetRestClient(statusPost.Request);
            var response = GetRestResponse(client, statusPost);
            var offerStatuslResponseObj = JsonConvert.DeserializeObject<BitfinexOfferStatusResponse>(response.Content);
            OnOfferStatusMsg(offerStatuslResponseObj);

            Logger.Log.InfoFormat("Status of offerId: {0}. Exchange response: {1}", offerId, offerStatuslResponseObj.ToString());

            return offerStatuslResponseObj;
        }

        public async Task<BitfinexOfferStatusResponse> GetOfferStatusAsync(int offerId) {
            var statusPost = new BitfinexOfferStatusPost {
                Request = OfferStatusRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),

                OfferId = offerId
            };

            var client = GetRestClient(statusPost.Request);
            var response = await GetRestResponseAsync(client, statusPost);
            var offerStatuslResponseObj = JsonConvert.DeserializeObject<BitfinexOfferStatusResponse>(response.Content);
            OnOfferStatusMsg(offerStatuslResponseObj);

            Logger.Log.InfoFormat("Status of offerId: {0}. Exchange response: {1}", offerId, offerStatuslResponseObj.ToString());

            return offerStatuslResponseObj;
        }

        public IList<BitfinexOfferStatusResponse> GetActiveOffers() {
            var activeOffersPost = new BitfinexPostBase {
                Request = ActiveOffersRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(activeOffersPost.Request);
            var response = GetRestResponse(client, activeOffersPost);
            var activeOffersResponseObj = JsonConvert.DeserializeObject<IList<BitfinexOfferStatusResponse>>(response.Content);
            OnActiveOffersMsg(activeOffersResponseObj);

            Logger.Log.InfoFormat("Active Offers:");
            foreach (var activeOffer in activeOffersResponseObj)
                Logger.Log.InfoFormat("Offer: {0}", activeOffer.ToString());

            return activeOffersResponseObj;
        }

        public async Task<IList<BitfinexOfferStatusResponse>> GetActiveOffersAsync() {
            var activeOffersPost = new BitfinexPostBase {
                Request = ActiveOffersRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(activeOffersPost.Request);
            var response = await GetRestResponseAsync(client, activeOffersPost);
            var activeOffersResponseObj = JsonConvert.DeserializeObject<IList<BitfinexOfferStatusResponse>>(response.Content);
            OnActiveOffersMsg(activeOffersResponseObj);

            Logger.Log.InfoFormat("Active Offers:");
            foreach (var activeOffer in activeOffersResponseObj)
                Logger.Log.InfoFormat("Offer: {0}", activeOffer.ToString());

            return activeOffersResponseObj;
        }

        public IList<BitfinexActiveCreditsResponse> GetActiveCredits() {
            var activeCreditsPost = new BitfinexPostBase {
                Request = ActiveCreditsRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(activeCreditsPost.Request);
            var response = GetRestResponse(client, activeCreditsPost);
            var activeCreditsResponseObj = JsonConvert.DeserializeObject<IList<BitfinexActiveCreditsResponse>>(response.Content);
            OnActiveCreditsMsg(activeCreditsResponseObj);

            Logger.Log.InfoFormat("Active Credits:");
            foreach (var activeCredits in activeCreditsResponseObj)
                Logger.Log.InfoFormat("Credits: {0}", activeCredits.ToString());

            return activeCreditsResponseObj;
        }

        public async Task<IList<BitfinexActiveCreditsResponse>> GetActiveCreditsAsync() {
            var activeCreditsPost = new BitfinexPostBase {
                Request = ActiveCreditsRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(activeCreditsPost.Request);
            var response = await GetRestResponseAsync(client, activeCreditsPost);
            var activeCreditsResponseObj = JsonConvert.DeserializeObject<IList<BitfinexActiveCreditsResponse>>(response.Content);
            OnActiveCreditsMsg(activeCreditsResponseObj);

            Logger.Log.InfoFormat("Active Credits:");
            foreach (var activeCredits in activeCreditsResponseObj)
                Logger.Log.InfoFormat("Credits: {0}", activeCredits.ToString());

            return activeCreditsResponseObj;
        }

        /// <summary>
        /// In the Total Return Swaps page you will see a horizontal header "Swaps used in margin position"
        /// This function returns information about what you have borrowed. If you want to close the 
        /// swap you must pass the id returned here to the "CloseSwap" function. 
        /// If you want to 'cash out' and claim the position you must pass the position id to the "ClaimPosition" function. 
        /// </summary>
        /// <returns></returns>
        public IList<BitfinexActiveSwapsInMarginResponse> GetActiveSwapsUsedInMarginPosition() {
            var activeSwapsInMarginPost = new BitfinexPostBase {
                Request = ActiveMarginSwapsRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(activeSwapsInMarginPost.Request);
            var response = GetRestResponse(client, activeSwapsInMarginPost);
            var activeSwapsInMarginResponseObj = JsonConvert.DeserializeObject<IList<BitfinexActiveSwapsInMarginResponse>>(response.Content);
            OnActiveSwapsUsedInMarginMsg(activeSwapsInMarginResponseObj);

            Logger.Log.InfoFormat("Active Swaps In Margin Pos:");
            foreach (var activeSwaps in activeSwapsInMarginResponseObj)
                Logger.Log.InfoFormat("Swaps used in margin: {0}", activeSwaps.ToString());

            return activeSwapsInMarginResponseObj;
        }

        public async Task<IList<BitfinexActiveSwapsInMarginResponse>> GetActiveSwapsUsedInMarginPositionAsync() {
            var activeSwapsInMarginPost = new BitfinexPostBase {
                Request = ActiveMarginSwapsRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString()
            };

            var client = GetRestClient(activeSwapsInMarginPost.Request);
            var response = await GetRestResponseAsync(client, activeSwapsInMarginPost);
            var activeSwapsInMarginResponseObj = JsonConvert.DeserializeObject<IList<BitfinexActiveSwapsInMarginResponse>>(response.Content);
            OnActiveSwapsUsedInMarginMsg(activeSwapsInMarginResponseObj);

            Logger.Log.InfoFormat("Active Swaps In Margin Pos:");
            foreach (var activeSwaps in activeSwapsInMarginResponseObj)
                Logger.Log.InfoFormat("Swaps used in margin: {0}", activeSwaps.ToString());

            return activeSwapsInMarginResponseObj;
        }

        public BitfinexActiveSwapsInMarginResponse CloseSwap(int swapId) {
            var closeSwapPost = new BitfinexCloseSwapPost {
                Request = CloseSwapRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                SwapId = swapId
            };

            var client = GetRestClient(closeSwapPost.Request);
            var response = GetRestResponse(client, closeSwapPost);

            var closeSwapResponseObj = JsonConvert.DeserializeObject<BitfinexActiveSwapsInMarginResponse>(response.Content);
            OnCloseSwapMsg(closeSwapResponseObj);

            Logger.Log.InfoFormat("Close Swap Id: {0}, Response from Exchange: {1}", swapId, closeSwapResponseObj.ToString());

            return closeSwapResponseObj;
        }

        public async Task<BitfinexActiveSwapsInMarginResponse> CloseSwapAsync(int swapId) {
            var closeSwapPost = new BitfinexCloseSwapPost {
                Request = CloseSwapRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                SwapId = swapId
            };

            var client = GetRestClient(closeSwapPost.Request);
            var response = await GetRestResponseAsync(client, closeSwapPost);

            var closeSwapResponseObj = JsonConvert.DeserializeObject<BitfinexActiveSwapsInMarginResponse>(response.Content);
            OnCloseSwapMsg(closeSwapResponseObj);

            Logger.Log.InfoFormat("Close Swap Id: {0}, Response from Exchange: {1}", swapId, closeSwapResponseObj.ToString());

            return closeSwapResponseObj;
        }

        /// <summary>
        /// Ok... so from what I gather is:
        /// If you leverage usd for btc, and the price moved in your favor the trade
        /// you can physically claim the btc in your wallet as yours. You will notice the
        /// object return this function is the same as the GetActiveSwapUsedInMarginPosition
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public BitfinexMarginPositionResponse ClaimPosition(int positionId) {
            var claimPosPost = new BitfinexClaimPositionPost {
                Request = ClaimPosRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                PositionId = positionId
            };

            var client = GetRestClient(claimPosPost.Request);
            var response = GetRestResponse(client, claimPosPost);

            var claimPosResponseObj = JsonConvert.DeserializeObject<BitfinexMarginPositionResponse>(response.Content);
            OnClaimPositionMsg(claimPosResponseObj);

            Logger.Log.InfoFormat("Claim Position Id: {0}, Response from Exchange: {1}", positionId, claimPosResponseObj.ToString());

            return claimPosResponseObj;
        }

        public async Task<BitfinexMarginPositionResponse> ClaimPositionAsync(int positionId) {
            var claimPosPost = new BitfinexClaimPositionPost {
                Request = ClaimPosRequestUrl,
                Nonce = Common.UnixTimeStampUtc().ToString(),
                PositionId = positionId
            };

            var client = GetRestClient(claimPosPost.Request);
            var response = await GetRestResponseAsync(client, claimPosPost);

            var claimPosResponseObj = JsonConvert.DeserializeObject<BitfinexMarginPositionResponse>(response.Content);
            OnClaimPositionMsg(claimPosResponseObj);

            Logger.Log.InfoFormat("Claim Position Id: {0}, Response from Exchange: {1}", positionId, claimPosResponseObj.ToString());

            return claimPosResponseObj;
        }

        #endregion

        #region RestCalls

        private RestRequest GetRestRequest(object obj) {
            var jsonObj = JsonConvert.SerializeObject(obj);
            var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonObj));
            var request = new RestRequest {
                Method = Method.POST
            };
            request.AddHeader(ApiBfxKey, _apiKey);
            request.AddHeader(ApiBfxPayload, payload);
            request.AddHeader(ApiBfxSig, GetHexHashSignature(payload));
            return request;
        }

        private IRestResponse GetRestResponse(RestClient client, object obj) {
            var response = client.Execute(GetRestRequest(obj));
            CheckToLogError(response);
            return response;
        }

        private async Task<IRestResponse> GetRestResponseAsync(RestClient client, object obj) {
            var tcs = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(GetRestRequest(obj), (resp) => {
                tcs.SetResult(resp);
            });
            var response = await tcs.Task;
            CheckToLogError(response);
            return response;
        }

        private void CheckToLogError(IRestResponse response) {
            switch (response.StatusCode) {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.BadRequest:
                    var errorMsgObj = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                    OnErrorMessage(errorMsgObj.Message);
                    break;
                default:
                    OnErrorMessage(response.StatusCode + " - " + response.Content);
                    break;
            }
        }

        private RestClient GetRestClient(string requestUrl) {
            var client = new RestClient();
            var url = BaseBitfinexUrl + requestUrl;
            client.BaseUrl = url;
            return client;
        }

        private IRestResponse GetBaseResponse(string url) {
            try {
                var client = new RestClient {
                    BaseUrl = BaseBitfinexUrl
                };
                var request = new RestRequest {
                    Resource = url
                };
                IRestResponse response = client.Execute(request);

                CheckToLogError(response);
                return response;
            } catch (Exception ex) {
                Logger.LogException(ex);
                return null;
            }
        }

        private async Task<IRestResponse> GetBaseResponseAsync(string url) {
            try {
                var client = new RestClient {
                    BaseUrl = BaseBitfinexUrl
                };
                var request = new RestRequest {
                    Resource = url
                };
                var tcs = new TaskCompletionSource<IRestResponse>();
                client.ExecuteAsync(request, (resp) => {
                    tcs.SetResult(resp);
                });
                var response = await tcs.Task;
                CheckToLogError(response);
                return response;
            } catch (Exception ex) {
                Logger.LogException(ex);
                return null;
            }
        }

        #endregion

        private string GetHexHashSignature(string payload) {
            HMACSHA384 hmac = new HMACSHA384(Encoding.UTF8.GetBytes(_apiSecret));
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

    }
}
