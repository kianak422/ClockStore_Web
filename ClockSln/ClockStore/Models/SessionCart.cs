using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using ClockStore.Infrastructure;

namespace ClockStore.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
                .HttpContext?.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart")
                ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddItem(Clock clock, int quantity)
        {
            base.AddItem(clock, quantity);
            Session?.SetJson("Cart", this);
        }

        public override void RemoveItem(Clock clock, int quantity = 1)
        {
            base.RemoveItem(clock, quantity);
            Session?.SetJson("Cart", this);
        }

        public override void SetItemQuantity(Clock clock, int quantity)
        {
            base.SetItemQuantity(clock, quantity);
            Session?.SetJson("Cart", this);
        }

        public override void RemoveLine(Clock clock)
        {
            base.RemoveLine(clock);
            Session?.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Cart");
        }
    }
}