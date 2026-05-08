using DOMAIN.Entities;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Context
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InvoiceHistory> InvoiceHistories { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentRecord> PaymentRecords { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SaleOrder> SaleOrders { get; set; }
        public DbSet<SaleOrderDetails> SaleOrderDetails { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(u => u.Email);
                u.HasMany(u => u.Invoices)
                    .WithOne(i => i.User)
                    .HasForeignKey(i => i.User_creator_id);
                u.HasMany(u => u.Linked_companies)
                    .WithOne(lc => lc.User)
                    .HasForeignKey(lc => lc.User_id);
                u.HasMany(u => u.Orders)
                    .WithOne(s => s.User)
                    .HasForeignKey(s => s.User_email);
                u.Property(u => u.Created_at)
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Company>(c =>
            {
                c.HasKey(c => c.Company_id);
                c.HasMany(c => c.Sell_invoices)
                    .WithOne(i => i.Sell_company)
                    .HasForeignKey(i => i.Sell_company_id);
                c.HasMany(c => c.Charged_invoices)
                    .WithOne(i => i.Charged_company)
                    .HasForeignKey(i => i.Charged_company_id);
                c.HasMany(c => c.Company_users)
                    .WithOne(cu => cu.Company)
                    .HasForeignKey(cu => cu.Company_id);
                c.HasMany(c => c.Orders)
                    .WithOne(s => s.Company)
                    .HasForeignKey(s => s.Company_id);
            });

            modelBuilder.Entity<Supplier>(s =>
            {
                s.HasKey(s => s.Supplier_id);
                s.HasMany(s => s.Orders)
                .WithOne(o => o.Supplier)
                .HasForeignKey(o => o.Supplier_id);
                s.HasOne(s => s.Company)
                .WithMany(c => c.Suppliers)
                .HasForeignKey(s => s.Company_id);
            });

            modelBuilder.Entity<UserCompany>(uc =>
            {
                uc.HasKey(uc => new { uc.User_id, uc.Company_id });
                uc.HasOne(u => u.User)
                    .WithMany(u => u.Linked_companies)
                    .HasForeignKey(uc => uc.User_id);
                uc.HasOne(uc => uc.Company)
                    .WithMany(c => c.Company_users)
                    .HasForeignKey(uc => uc.Company_id);
            });

            modelBuilder.Entity<Claim>(c =>
            {
                c.HasKey(c => c.Claim_id);
                c.Property(c => c.Claim_amount)
                    .HasPrecision(15, 2);
                c.HasOne(c => c.Invoice)
                    .WithMany(i => i.Claims)
                    .HasForeignKey(c => c.Invoice_id);
                c.HasOne(c => c.User)
                    .WithMany(u => u.Claims)
                    .HasForeignKey(c => c.User_id);
            });

            modelBuilder.Entity<InvoiceHistory>(h =>
            {
                h.HasKey(h => h.Invoice_history_id);
                h.HasOne(h => h.Invoice)
                    .WithMany(i => i.Invoice_histories)
                    .HasForeignKey(h => h.Invoice_id);
                h.HasOne(h => h.PaymentRecord)
                    .WithMany(p => p.Invoice_histories)
                    .HasForeignKey(h => h.Payment_record_id);
                h.HasOne(h => h.Claim)
                    .WithMany(c => c.Invoice_histories)
                    .HasForeignKey(h => h.Claim_id);
                h.HasOne(h => h.User)
                    .WithMany(u => u.Invoice_histories)
                    .HasForeignKey(h => h.User_id);
                h.Property(h => h.Paid_amount).HasPrecision(15, 2);
                h.Property(h => h.Pending_balance).HasPrecision(15, 2);
            });

            modelBuilder.Entity<Invoice>(i =>
            {
                i.HasKey(i => i.Invoice_id);
                i.Property(i => i.Total_voucher)
                    .HasPrecision(15, 2);
                i.Property(i => i.Pending_balance)
                    .HasPrecision(15, 2);
                i.Property(i => i.Created_at)
                    .ValueGeneratedOnAdd();
                i.HasOne(i => i.User)
                    .WithMany(u => u.Invoices)
                    .HasForeignKey(i => i.User_creator_id);
                i.HasOne(i => i.Sell_company)
                    .WithMany(s => s.Sell_invoices)
                    .HasForeignKey(i => i.Sell_company_id);
                i.HasOne(i => i.Charged_company)
                    .WithMany(c => c.Charged_invoices)
                    .HasForeignKey(i => i.Charged_company_id);
                i.HasOne(i => i.Currency)
                    .WithMany(c => c.Invoices)
                    .HasForeignKey(i => i.Currency_id);
                i.HasMany(i => i.Claims)
                    .WithOne(c => c.Invoice)
                    .HasForeignKey(c => c.Invoice_id);
                i.HasMany(i => i.Invoice_histories)
                    .WithOne(h => h.Invoice)
                    .HasForeignKey(h => h.Invoice_id);
                i.HasMany(i => i.Payment_records)
                    .WithOne(r => r.Source)
                    .HasForeignKey(r => r.Source_id);
            });

            modelBuilder.Entity<PaymentRecord>(r =>
            {
                r.HasKey(r => r.Payment_record_id);
                r.Property(r => r.Paid_amount)
                    .HasPrecision(15, 2);
                r.Property(r => r.Paid_amount)
                    .HasPrecision(15, 2);
                r.HasOne(r => r.Source)
                    .WithMany(s => s.Payment_records)
                    .HasForeignKey(r => r.Source_id);
                r.HasOne(r => r.Currency)
                    .WithMany(c => c.Payment_records)
                    .HasForeignKey(r => r.Currency_id);
                r.HasOne(r => r.Payment_method)
                    .WithMany(m => m.Payment_records)
                    .HasForeignKey(r => r.Payment_method_id);
            });

            modelBuilder.Entity<Currency>(c =>
            {
                c.HasKey(c => c.Currency_id);
                c.HasMany(c => c.Payment_records)
                    .WithOne(r => r.Currency)
                    .HasForeignKey(r => r.Currency_id);
                c.HasMany(c => c.Invoices)
                    .WithOne(i => i.Currency)
                    .HasForeignKey(i => i.Currency_id);
            });

            modelBuilder.Entity<PaymentMethod>(p =>
            {
                p.HasKey(p => p.Payment_method_id);
                p.HasMany(p => p.Payment_records)
                    .WithOne(r => r.Payment_method)
                    .HasForeignKey(r => r.Payment_method_id);
            });

            modelBuilder.Entity<Product>(p =>
            {
                p.HasKey(p => p.Product_id);
                p.HasMany(p => p.Details)
                .WithOne(d => d.Product)
                .HasForeignKey(d => d.Product_id);

                p.Property(p => p.Unit_price).HasPrecision(15, 2);
            });

            modelBuilder.Entity<SaleOrder>(s =>
            {
                s.HasKey(s => s.Order_id);

                s.HasOne(s => s.Supplier)
                .WithMany(s => s.Orders)
                .HasForeignKey(s => s.Supplier_id);

                s.HasMany(s => s.Details)
                .WithOne(d => d.Order)
                .HasForeignKey(d => d.Order_id)
                .OnDelete(DeleteBehavior.Cascade);

                s.Property(s => s.Order_date)
                .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SaleOrderDetails>(d =>
            {
                d.HasKey(d => d.Detail_id);

                d.HasOne(d => d.Product)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.Product_id);

                d.HasOne(d => d.Order)
                .WithMany(s => s.Details)
                .HasForeignKey(d => d.Order_id);

                d.Property(d => d.Tax).HasPrecision(15, 2);
                d.Property(d => d.Tax_rate).HasPrecision(15, 2);
                d.Property(d => d.Unit_price).HasPrecision(15, 2);
                d.Property(d => d.Subtotal).HasPrecision(15, 2);
                d.Property(d => d.Total).HasPrecision(15, 2);
            });

            modelBuilder.Entity<Visit>(v =>
            {
                v.HasKey(v => v.Visit_id);
                v.Property(v => v.Visit_date).HasColumnType("date");
                v.Property(v => v.Visit_time).HasColumnType("time");
            });
        }
    }
}