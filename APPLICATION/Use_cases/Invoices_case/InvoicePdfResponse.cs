// DTOs/InvoicePdfDto.cs
namespace APPLICATION.Use_cases.Invoices_case;

public record InvoicePdfResponse(
    int InvoiceId,
    int Consecutive,
    decimal TotalVoucher,
    decimal? PendingBalance,
    string Status,
    DateTime CreatedAt,
    string UserCreatorName,
    string UserCreatorEmail,
    string SellCompanyName,
    string? SellCompanyAddress,
    string? SellCompanyPhone,
    string? SellCompanyEmail,
    string ChargedCompanyName,
    string? ChargedCompanyAddress,
    string CurrencyIso,
    string CurrencyCode,
    IReadOnlyList<PaymentRecordPdfDto> PaymentRecords
);

public record PaymentRecordPdfDto(
    decimal PaidAmount,
    DateTime PaymentDate,
    string PaymentDetail,
    string PaymentMethodName,
    string ThirdPartyTransactionId
);