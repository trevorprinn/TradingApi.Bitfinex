using System;
using System.Collections.Generic;
using TradingApi.ModelObjects;
using TradingApi.ModelObjects.Bitfinex.Json;

namespace TradingApi.Bitfinex
{
    public  partial class BitfinexApi
    {
        public class MessageEventArgs : EventArgs {
            public string Message { get; }
            public MessageEventArgs(string message) {
                Message = message;
            }
        }
        public event EventHandler<MessageEventArgs> ErrorMessage;

        public class OrderBookMsgEventArgs : EventArgs {
            public BitfinexOrderBookGet MarketData { get; }
            public OrderBookMsgEventArgs(BitfinexOrderBookGet marketData) {
                MarketData = marketData;
            }
        }
        public event EventHandler<OrderBookMsgEventArgs> OrderBookMsg;

        public class BalanceResponseMsgEventArgs : EventArgs {
            public IEnumerable<BitfinexBalanceResponse> BalanceResponse { get; }
            public BalanceResponseMsgEventArgs(IEnumerable<BitfinexBalanceResponse> balanceResponse) {
                BalanceResponse = balanceResponse;
            }
        }
        public event EventHandler<BalanceResponseMsgEventArgs> BalanceResponseMsg;

        public class MultipleOrderFeedMsgEventArgs : EventArgs {
            public BitfinexMultipleNewOrderResponse OrdersResponse { get; }
            public MultipleOrderFeedMsgEventArgs(BitfinexMultipleNewOrderResponse ordersResponse) {
                OrdersResponse = ordersResponse;
            }
        }
        public event EventHandler<MultipleOrderFeedMsgEventArgs> MultipleOrderFeedMsg;

        public class OrderFeedMsgEventArgs : EventArgs {
            public BitfinexNewOrderResponse OrderResponse { get; }
            public OrderFeedMsgEventArgs(BitfinexNewOrderResponse orderResponse) {
                OrderResponse = orderResponse;
            }
        }
        public event EventHandler<OrderFeedMsgEventArgs> OrderFeedMsg;

        public class CancelOrderMsgEventArgs : EventArgs {
            public BitfinexOrderStatusResponse CancelResponse { get; }
            public CancelOrderMsgEventArgs(BitfinexOrderStatusResponse cancelResponse) {
                CancelResponse = cancelResponse;
            }
        }
        public event EventHandler<CancelOrderMsgEventArgs> CancelOrderMsg;

        public class CancelReplaceFeedMsgEventArgs : EventArgs {
            public BitfinexCancelReplaceOrderResponse CancelReplaceReponse { get; }
            public CancelReplaceFeedMsgEventArgs(BitfinexCancelReplaceOrderResponse cancelReplaceReponse) {
                CancelReplaceReponse = cancelReplaceReponse;
            }
        }
        public event EventHandler<CancelReplaceFeedMsgEventArgs> CancelReplaceFeedMsg;
       
        public event EventHandler<MessageEventArgs> CancelMultipleOrdersMsg;

        public event EventHandler<MessageEventArgs> CancelAllActiveOrdersMsg;

        public class ActiveOrdersMsgEventArgs : EventArgs {
            public IEnumerable<BitfinexMarginPositionResponse> ActivePositionResponse { get; }
            public ActiveOrdersMsgEventArgs(IEnumerable<BitfinexMarginPositionResponse> activePositionResponse) {
                ActivePositionResponse = activePositionResponse;
            }
        }
       public event EventHandler<ActiveOrdersMsgEventArgs> ActiveOrdersMsg;

        public class HistoryMsgEventArgs : EventArgs {
            public IEnumerable<BitfinexHistoryResponse> HistoryResponse { get; }
            public HistoryMsgEventArgs(IEnumerable<BitfinexHistoryResponse> historyResponse) {
                HistoryResponse = historyResponse;
            }
        }
        public event EventHandler<HistoryMsgEventArgs> HistoryMsg;

        public class MyTradesMsgEventArgs : EventArgs {
            public IEnumerable<BitfinexMyTradesResponse> MyTradesResponse { get; }
            public MyTradesMsgEventArgs(IEnumerable<BitfinexMyTradesResponse> myTradesResponse) {
                MyTradesResponse = myTradesResponse;
            }
        }
        public event EventHandler<MyTradesMsgEventArgs> MyTradesMsg;

        public class MarginInformationMsgEventArgs : EventArgs {
            public BitfinexMarginInfoResponse MarginInfoResponse { get; }
            public MarginInformationMsgEventArgs(BitfinexMarginInfoResponse marginInfoResponse) {
                MarginInfoResponse = marginInfoResponse;
            }
        }
        public event EventHandler<MarginInformationMsgEventArgs> MarginInformationMsg;

        public class ActivePositionsMsgEventArgs : EventArgs {
            public IEnumerable<BitfinexMarginPositionResponse> ActivePositionResponse { get; }
            public ActivePositionsMsgEventArgs(IEnumerable<BitfinexMarginPositionResponse> activePositionResponse) {
                ActivePositionResponse = activePositionResponse;
            }
        }
        public event EventHandler<ActivePositionsMsgEventArgs> ActivePositionsMsg;

        public class OrderStatusEventArgs : EventArgs {
            public BitfinexOrderStatusResponse OrderStatusResponse { get; }
            public OrderStatusEventArgs(BitfinexOrderStatusResponse orderStatusResponse) {
                OrderStatusResponse = orderStatusResponse;
            }
        }
        public event EventHandler<OrderStatusEventArgs> OrderStatusMsg;

        public class OfferStatusEventArgs : EventArgs {
            public BitfinexOfferStatusResponse OfferStatusResponse { get; }
            public OfferStatusEventArgs(BitfinexOfferStatusResponse offerStatusResponse) {
                OfferStatusResponse = offerStatusResponse;
            }
        }
       public event EventHandler<OfferStatusEventArgs> NewOfferStatusMsg;

       public event EventHandler<OfferStatusEventArgs> CancelOfferMsg;

       public event EventHandler<OfferStatusEventArgs> OfferStatusMsg;

        public class OfferStatusesEventArgs : EventArgs {
            public IEnumerable<BitfinexOfferStatusResponse> ActiveOfferResponse { get; }
            public OfferStatusesEventArgs(IEnumerable<BitfinexOfferStatusResponse> activeOfferResponse) {
                ActiveOfferResponse = activeOfferResponse;
            }
        }
        public event EventHandler<OfferStatusesEventArgs> ActiveOffersMsg;

        public class ActiveCreditsMsgEventArgs : EventArgs {
            public IEnumerable<BitfinexActiveCreditsResponse> ActiveCreditsResponse { get; }
            public ActiveCreditsMsgEventArgs(IEnumerable<BitfinexActiveCreditsResponse> activeCreditsResponse) {
                ActiveCreditsResponse = activeCreditsResponse;
            }
        }
        public event EventHandler<ActiveCreditsMsgEventArgs> ActiveCreditsMsg;

        public class ActiveSwapsInMarginMsgEventArgs : EventArgs {
            public IEnumerable<BitfinexActiveSwapsInMarginResponse> ActiveSwapsInMarginResponse { get; }
            public ActiveSwapsInMarginMsgEventArgs(IEnumerable<BitfinexActiveSwapsInMarginResponse> activeSwapsInMarginResponse) {
                ActiveSwapsInMarginResponse = activeSwapsInMarginResponse;
            }
        }
        public event EventHandler<ActiveSwapsInMarginMsgEventArgs> ActiveSwapsUsedInMarginMsg;

        public class ActiveSwapInMarginMsgEventArgs : EventArgs {
            public BitfinexActiveSwapsInMarginResponse ActiveSwapInMarginResponse { get; }
            public ActiveSwapInMarginMsgEventArgs(BitfinexActiveSwapsInMarginResponse activeSwapInMarginResponse) {
                ActiveSwapInMarginResponse = activeSwapInMarginResponse;
            }
        }
        public event EventHandler<ActiveSwapInMarginMsgEventArgs> CloseSwapMsg;

        public class MarginPositionEventArgs : EventArgs {
            public BitfinexMarginPositionResponse PositionResponse { get; }
            public MarginPositionEventArgs(BitfinexMarginPositionResponse positionResponse) {
                PositionResponse = positionResponse;
            }
        }
        public event EventHandler<MarginPositionEventArgs> ClaimPositionMsg;

        public class LendbookEventArgs : EventArgs {
            public BitfinexLendbookResponse LendbookResponse { get; }
            public LendbookEventArgs(BitfinexLendbookResponse lendbookResponse) {
                LendbookResponse = lendbookResponse;
            }
        }
        public event EventHandler<LendbookEventArgs> LendbookResponseMsg;

        public class LendsEventArgs : EventArgs {
            public IEnumerable<BitfinexLendsResponse> LendsResponse { get; }
            public LendsEventArgs(IEnumerable<BitfinexLendsResponse> lendsResponse) {
                LendsResponse = lendsResponse;
            }
        }
        public event EventHandler<LendsEventArgs> LendsResponseMsg;

        protected virtual void OnLendsResponseMsg(IEnumerable<BitfinexLendsResponse> lendsResponse) {
            LendsResponseMsg?.Invoke(this, new LendsEventArgs(lendsResponse));
        }

        protected virtual void OnLendbookResponseMsg(BitfinexLendbookResponse lendbookResponse) {
            LendbookResponseMsg?.Invoke(this, new LendbookEventArgs(lendbookResponse));
        }

        protected virtual void OnClaimPositionMsg(BitfinexMarginPositionResponse claimPosResponse) {
            ClaimPositionMsg?.Invoke(this, new MarginPositionEventArgs(claimPosResponse));
        }

        protected virtual void OnCloseSwapMsg(BitfinexActiveSwapsInMarginResponse closeSwapResponse) {
            CloseSwapMsg?.Invoke(this, new ActiveSwapInMarginMsgEventArgs(closeSwapResponse));
        }

        protected virtual void OnActiveSwapsUsedInMarginMsg(IEnumerable<BitfinexActiveSwapsInMarginResponse> activeSwapsInMarginResponse) {
            ActiveSwapsUsedInMarginMsg?.Invoke(this, new ActiveSwapsInMarginMsgEventArgs(activeSwapsInMarginResponse));
        }

        protected virtual void OnActiveCreditsMsg(IEnumerable<BitfinexActiveCreditsResponse> activeCreditsResponse) {
            ActiveCreditsMsg?.Invoke(this, new ActiveCreditsMsgEventArgs(activeCreditsResponse));
        }

        protected virtual void OnOfferStatusMsg(BitfinexOfferStatusResponse offerStatusResponse) {
            OfferStatusMsg?.Invoke(this, new OfferStatusEventArgs(offerStatusResponse));
        }

        protected virtual void OnCancelOfferMsg(BitfinexOfferStatusResponse cancelOfferResponse) {
            CancelOfferMsg?.Invoke(this, new OfferStatusEventArgs(cancelOfferResponse));
        }

        protected virtual void OnNewOfferStatusMsg(BitfinexOfferStatusResponse newOfferResponse) {
            NewOfferStatusMsg?.Invoke(this, new OfferStatusEventArgs(newOfferResponse));
        }

        protected virtual void OnOrderStatusMsg(BitfinexOrderStatusResponse orderStatusResponse) {
            OrderStatusMsg?.Invoke(this, new OrderStatusEventArgs(orderStatusResponse));
        }

        protected virtual void OnActivePositionsMsg(IList<BitfinexMarginPositionResponse> activePositionResponse) {
            ActivePositionsMsg?.Invoke(this, new ActivePositionsMsgEventArgs(activePositionResponse));
        }

        protected virtual void OnMarginInformationMsg(BitfinexMarginInfoResponse marginInfoResponse) {
            MarginInformationMsg?.Invoke(this, new MarginInformationMsgEventArgs(marginInfoResponse));
        }

        protected virtual void OnMyTradesMsg(IList<BitfinexMyTradesResponse> myTradesResponse) {
            MyTradesMsg?.Invoke(this, new MyTradesMsgEventArgs(myTradesResponse));
        }

        protected virtual void OnActiveOrdersMsg(IEnumerable<BitfinexMarginPositionResponse> response) {
            ActiveOrdersMsg?.Invoke(this, new ActiveOrdersMsgEventArgs(response));
        }

        protected virtual void OnHistoryMsg(IList<BitfinexHistoryResponse> historyResponse) {
            HistoryMsg?.Invoke(this, new HistoryMsgEventArgs(historyResponse));
        }

        protected virtual void OnActiveOffersMsg(IEnumerable<BitfinexOfferStatusResponse> activeOfferResponse) {
            ActiveOffersMsg?.Invoke(this, new OfferStatusesEventArgs(activeOfferResponse));
        }

        protected virtual void OnCancelAllActiveOrdersMsg(string msg) {
            CancelAllActiveOrdersMsg?.Invoke(this, new MessageEventArgs(msg));
        }

        protected virtual void OnCancelMultipleOrdersMsg(string msg) {
            CancelMultipleOrdersMsg?.Invoke(this, new MessageEventArgs(msg));
        }

        protected virtual void OnCancelReplaceFeedMsg(BitfinexCancelReplaceOrderResponse cancelReplaceReponse) {
            CancelReplaceFeedMsg?.Invoke(this, new CancelReplaceFeedMsgEventArgs(cancelReplaceReponse));
        }

        protected virtual void OnCancelOrderMsg(BitfinexOrderStatusResponse cancelResponse) {
            CancelOrderMsg?.Invoke(this, new CancelOrderMsgEventArgs(cancelResponse));
        }

        protected virtual void OnOrderFeedMsg(BitfinexNewOrderResponse orderResponse) {
            OrderFeedMsg?.Invoke(this, new OrderFeedMsgEventArgs(orderResponse));
        }

        protected virtual void OnMultipleOrderFeedMsg(BitfinexMultipleNewOrderResponse ordersResponse) {
            MultipleOrderFeedMsg?.Invoke(this, new MultipleOrderFeedMsgEventArgs(ordersResponse));
        }

        protected virtual void OnBalanceResponseMsg(IList<BitfinexBalanceResponse> balanceResponse) {
            BalanceResponseMsg?.Invoke(this, new BalanceResponseMsgEventArgs(balanceResponse));
        }

        protected virtual void OnOrderBookMsg(BitfinexOrderBookGet marketData) {
            OrderBookMsg?.Invoke(this, new OrderBookMsgEventArgs(marketData));
        }

        protected virtual void OnErrorMessage(string errorMessage) {
            ErrorMessage?.Invoke(this, new MessageEventArgs(errorMessage));
        } 
    }
}
