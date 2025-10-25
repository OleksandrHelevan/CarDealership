using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.repo.impl;
using Microsoft.Win32;
using System.Windows.Navigation;

namespace CarDealership.page.authorized;

public partial class ReceiptViewerPage : Page
{
    private readonly int _receiptId;
    private readonly DealershipContext _context;
    private readonly PaymentHistoryRepositoryImpl _repo;
    private string? _tempFile;

    public ReceiptViewerPage(int receiptId)
    {
        InitializeComponent();
        _receiptId = receiptId;
        _context = new DealershipContext();
        _repo = new PaymentHistoryRepositoryImpl(_context);
        // Clean temp file when page is unloaded (navigation away)
        this.Unloaded += (_, __) => CleanupTempFile();
        LoadPdf();
    }

    private void LoadPdf()
    {
        var ph = _repo.GetById(_receiptId);
        if (ph == null || ph.ReceiptPdf == null || ph.ReceiptPdf.Length == 0)
        {
            MessageBox.Show("Квитанцію не знайдено.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        // Use a unique temp file to avoid file lock conflicts with WebBrowser
        var unique = System.Guid.NewGuid().ToString("N");
        _tempFile = Path.Combine(Path.GetTempPath(), $"receipt-{ph.Id}-{unique}.pdf");
        File.WriteAllBytes(_tempFile, ph.ReceiptPdf);
        try
        {
            var psi = new ProcessStartInfo(_tempFile) { UseShellExecute = true };
            Process.Start(psi);
        }
        catch
        {
            // If no handler, at least leave the file on disk so user can Save As
        }
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.GoBack();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var ph = _repo.GetById(_receiptId);
        if (ph == null || ph.ReceiptPdf == null || ph.ReceiptPdf.Length == 0)
        {
            MessageBox.Show("Квитанцію не знайдено.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        var dlg = new SaveFileDialog
        {
            FileName = $"receipt-{ph.Id}.pdf",
            Filter = "PDF (*.pdf)|*.pdf"
        };
        if (dlg.ShowDialog() == true)
        {
            File.WriteAllBytes(dlg.FileName, ph.ReceiptPdf);
            MessageBox.Show("Збережено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void CleanupTempFile()
    {
        try
        {
            if (!string.IsNullOrEmpty(_tempFile) && File.Exists(_tempFile))
            {
                File.Delete(_tempFile);
            }
        }
        catch { /* ignore cleanup errors */ }
    }
}
