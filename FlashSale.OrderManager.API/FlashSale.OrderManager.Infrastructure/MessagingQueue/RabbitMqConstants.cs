namespace FlashSale.OrderManager.Infrastructure.MessagingQueue
{
    public static class RabbitMqConstants
    {
        // =========================================
        // Exchanges
        // =========================================

        public const string EXCHANGE_ORDER_Topic =
            "order.exchange.topic";

        public const string EXCHANGE_ORDER_DLX =
            "order.dlx.topic";


        // =========================================
        // Queues
        // =========================================

        public const string QUEUE_ORDER_CREATED =
            "product.order-created";

        public const string QUEUE_ORDER_CANCELLED =
            "product.order-cancelled";

        public const string QUEUE_ORDER_FAILED =
            "product.order-failed";

        public const string QUEUE_ORDER_Approved =
            "product.order-approved";


        // =========================================
        // Dead Letter Queues
        // =========================================

        public const string QUEUE_ORDER_CREATED_DLQ =
            "product.order-created.dlq";

        public const string QUEUE_ORDER_CANCELLED_DLQ =
            "product.order-cancelled.dlq";

        public const string QUEUE_ORDER_FAILED_DLQ =
            "product.order-failed.dlq";

        public const string QUEUE_ORDER_APPROVED_DLQ =
            "product.order-approved.dlq";


        // =========================================
        // Routing Keys
        // =========================================

        public const string ROUTING_KEY_ORDER_CREATED =
            "order.created";

        public const string ROUTING_KEY_ORDER_CANCELLED =
            "order.cancelled";

        public const string ROUTING_KEY_ORDER_FAILED =
            "order.failed";

        public const string ROUTING_KEY_ORDER_APPROVED =
            "order.approved";


        // =========================================
        // Dead Letter Routing Keys
        // =========================================

        public const string ROUTING_KEY_ORDER_CREATED_DLQ =
            "order.created.dlq";

        public const string ROUTING_KEY_ORDER_CANCELLED_DLQ =
            "order.cancelled.dlq";

        public const string ROUTING_KEY_ORDER_FAILED_DLQ =
            "order.failed.dlq";

        public const string ROUTING_KEY_ORDER_APPROVED_DLQ =
            "order.approved.dlq";
    }
}