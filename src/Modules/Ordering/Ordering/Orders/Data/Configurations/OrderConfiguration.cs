

namespace Ordering.Orders.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(e => e.CustomerId);
            builder.HasIndex(x => x.OrderName).IsUnique();
            builder.Property(x => x.OrderName).HasMaxLength(100).IsRequired();
            builder.HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey(si => si.OrderId);

            builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
            {
                addressBuilder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
                addressBuilder.Property(x => x.LastName).HasMaxLength(50).IsRequired();
                addressBuilder.Property(x => x.EmailAddress).HasMaxLength(50);
                addressBuilder.Property(x => x.AddressLine).HasMaxLength(180).IsRequired();
                addressBuilder.Property(x => x.Country).HasMaxLength(50);
                addressBuilder.Property(x => x.State).HasMaxLength(50);
                addressBuilder.Property(x => x.ZipCode).HasMaxLength(5).IsRequired();

            });
            builder.ComplexProperty(o => o.BillingAddress, addressBuilder =>
            {
                addressBuilder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
                addressBuilder.Property(x => x.LastName).HasMaxLength(50).IsRequired();
                addressBuilder.Property(x => x.EmailAddress).HasMaxLength(50);
                addressBuilder.Property(x => x.AddressLine).HasMaxLength(180).IsRequired();
                addressBuilder.Property(x => x.Country).HasMaxLength(50);
                addressBuilder.Property(x => x.State).HasMaxLength(50);
                addressBuilder.Property(x => x.ZipCode).HasMaxLength(5).IsRequired();

            });
            builder.ComplexProperty(o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(x => x.CardNumber).HasMaxLength(50).IsRequired();
                paymentBuilder.Property(x => x.CardName).HasMaxLength(50);
                paymentBuilder.Property(x => x.Expiration).HasMaxLength(10);
                paymentBuilder.Property(x => x.CVV).HasMaxLength(3);
                paymentBuilder.Property(x => x.PaymentMethod);

            });
        }
    }
}
