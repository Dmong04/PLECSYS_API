using APPLICATION.Use_cases.Invoices_case;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace APPLICATION.Utils.PDFs
{
    public sealed class InvoicePdfService(IInvoiceRepository repo)
    {
        public async Task<byte[]> GenerateAsync(int invoiceId, CancellationToken ct = default)
        {
            var invoice = await repo.GetInvoiceForPdfAsync(invoiceId, ct)
                ?? throw new Exception($"Factura {invoiceId} no encontrada.");

            return Render(MapToDto(invoice));
        }

        private static InvoicePdfResponse MapToDto(Invoice inv) => new(
            inv.Invoice_id,
            inv.Consecutive,
            inv.Total_voucher,
            inv.Pending_balance,
            inv.Status,
            inv.Created_at,
            $"{inv.User?.Name} {inv.User?.First_lastname} {inv.User?.Second_lastname}",
            inv.User.Email,
            inv.Sell_company.Company_name,
            inv.Sell_company?.Address,
            inv.Sell_company?.Phone,
            inv.Sell_company?.Email,
            inv.Charged_company?.Company_name,
            inv.Charged_company?.Address,
            inv.Currency?.Currency_ISO,
            inv.Currency.Currency_code,
            inv.Payment_records.Select(pr => new PaymentRecordPdfDto(
                pr.Paid_amount,
                pr.Payment_date,
                pr.Payment_detail,
                pr.Payment_method.Payment_method_name,
                pr.Third_party_transaction_id
            )).ToList()
        );

        private static byte[] Render(InvoicePdfResponse inv)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(h => BuildHeader(h, inv));
                    page.Content().Element(c => BuildContent(c, inv));
                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Página ").FontSize(8).FontColor(Colors.Grey.Medium);
                        t.CurrentPageNumber().FontSize(8);
                        t.Span(" de ").FontSize(8).FontColor(Colors.Grey.Medium);
                        t.TotalPages().FontSize(8);
                    });
                });
            }).GeneratePdf();
        }

        private static void BuildHeader(IContainer h, InvoicePdfResponse inv) =>
            h.Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("PLECSYS").FontSize(22).Bold().FontColor("#1a56db");
                    c.Item().Text("Sistema de Gestión de Facturas").FontSize(9).FontColor(Colors.Grey.Medium);
                });
                row.ConstantItem(170).Column(c =>
                {
                    c.Item().Background("#1a56db").Padding(8)
                        .Text($"FACTURA #{inv.Consecutive:D6}")
                        .FontColor(Colors.White).Bold().FontSize(13).AlignRight();
                    c.Item().PaddingTop(4).Text($"Estado: {inv.Status}").AlignRight().Bold();
                    c.Item().Text($"Fecha: {inv.CreatedAt:dd/MM/yyyy}").AlignRight();
                    c.Item().Text($"Moneda: {inv.CurrencyIso} ({inv.CurrencyCode})").AlignRight();
                });
            });

        private static void BuildContent(IContainer ct, InvoicePdfResponse inv) =>
            ct.PaddingTop(20).Column(col =>
            {
                col.Item().Row(row =>
                {
                    CompanyBox(row.RelativeItem(), "EMPRESA VENDEDORA",
                        inv.SellCompanyName, inv.SellCompanyAddress,
                        inv.SellCompanyPhone, inv.SellCompanyEmail);
                    row.ConstantItem(20);
                    CompanyBox(row.RelativeItem(), "FACTURADO A",
                        inv.ChargedCompanyName, inv.ChargedCompanyAddress);
                });

                col.Item().PaddingTop(20).Row(row =>
                {
                    row.RelativeItem();
                    row.ConstantItem(260).Border(1).BorderColor(Colors.Grey.Lighten2).Column(c =>
                    {
                        SummaryRow(c, "Total factura", $"{inv.CurrencyCode} {inv.TotalVoucher:N2}", false);
                        SummaryRow(c, "Pagado", $"{inv.CurrencyCode} {inv.TotalVoucher - inv.PendingBalance:N2}", false);
                        SummaryRow(c, "Saldo pendiente", $"{inv.CurrencyCode} {inv.PendingBalance:N2}", true);
                    });
                });

                if (inv.PaymentRecords.Count > 0)
                {
                    col.Item().PaddingTop(24).Text("HISTORIAL DE PAGOS").Bold().FontSize(11);
                    col.Item().PaddingTop(6).Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2); c.RelativeColumn(2);
                            c.RelativeColumn(2); c.RelativeColumn(3);
                            c.RelativeColumn(2);
                        });
                        table.Header(h =>
                        {
                            foreach (var label in new[] { "Fecha", "Método", "# Transacción", "Detalle", "Monto" })
                                h.Cell().Background("#1a56db").Padding(6)
                                    .Text(label).FontColor(Colors.White).Bold().FontSize(9);
                        });
                        foreach (var (p, i) in inv.PaymentRecords.Select((p, i) => (p, i)))
                        {
                            var bg = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;
                            table.Cell().Background(bg).Padding(6).Text(p.PaymentDate.ToString("dd/MM/yyyy")).FontSize(9);
                            table.Cell().Background(bg).Padding(6).Text(p.PaymentMethodName).FontSize(9);
                            table.Cell().Background(bg).Padding(6).Text(p.ThirdPartyTransactionId).FontSize(9);
                            table.Cell().Background(bg).Padding(6).Text(p.PaymentDetail).FontSize(9);
                            table.Cell().Background(bg).Padding(6)
                                .Text($"{inv.CurrencyCode} {p.PaidAmount:N2}").FontSize(9).AlignRight();
                        }
                    });
                }

                col.Item().PaddingTop(30).Row(row =>
                {
                    row.RelativeItem()
                        .Text($"Generado por: {inv.UserCreatorName} ({inv.UserCreatorEmail})")
                        .FontSize(8).FontColor(Colors.Grey.Medium);
                    row.RelativeItem().AlignRight()
                        .Text($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(8).FontColor(Colors.Grey.Medium);
                });
            });

        private static void CompanyBox(IContainer c, string label, string name,
            string? address = null, string? phone = null, string? email = null) =>
            c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(col =>
            {
                col.Item().Text(label).FontSize(8).FontColor(Colors.Grey.Medium).Bold();
                col.Item().PaddingTop(4).Text(name).Bold().FontSize(11);
                if (address is not null) col.Item().Text(address).FontSize(9);
                if (phone is not null) col.Item().Text($"Tel: {phone}").FontSize(9);
                if (email is not null) col.Item().Text(email).FontSize(9);
            });

        private static void SummaryRow(ColumnDescriptor col, string label, string value, bool highlight) =>
            col.Item()
                .Background(highlight ? "#1a56db" : Colors.Transparent)
                .Padding(8).Row(r =>
                {
                    var labelText = r.RelativeItem().Text(label)
                    .FontColor(highlight ? Colors.White : Colors.Black);
                    if (highlight) labelText.Bold();

                    var valueText = r.RelativeItem().AlignRight().Text(value)
                    .FontColor(highlight ? Colors.White : Colors.Black);
                    if (highlight) valueText.Bold();
                });
    }
}