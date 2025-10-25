using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarDealership.config;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CarDealership.page.admin;

public partial class SqlConsolePage : Page
{
    private readonly DealershipContext _context;
    private DataTable? _currentTable;
    private string? _currentTableName;

    public SqlConsolePage()
    {
        InitializeComponent();
        _context = new DealershipContext();
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        SqlInput.Clear();
        ResultGrid.ItemsSource = null;
        ResultText.Text = string.Empty;
    }

    private void SqlInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        {
            Execute_Click(sender, e);
            e.Handled = true;
        }
    }

    private void Execute_Click(object sender, RoutedEventArgs e)
    {
        var sql = SqlInput.Text?.Trim();
        ResultGrid.ItemsSource = null;
        ResultText.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(sql))
        {
            ResultText.Text = "Введіть SQL запит.";
            return;
        }

        try
        {
            // ВАЖЛИВО: не диспоузити з'єднання EF. Відкриваємо/закриваємо через Database.
            _context.Database.OpenConnection();
            var conn = _context.Database.GetDbConnection();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandTimeout = 60;

            // Якщо починається з SELECT або WITH — читаємо результат
            var lowered = sql.TrimStart().ToLowerInvariant();
            if (lowered.StartsWith("select") || lowered.StartsWith("with"))
            {
                using var reader = cmd.ExecuteReader();
                var table = new DataTable();
                table.Load(reader);
                ResultGrid.ItemsSource = table.DefaultView;
                ResultText.Text = $"OK. Рядків: {table.Rows.Count}";

                // зберігаємо для можливості редагування прямо в гріді
                _currentTable = table;
                _currentTableName = TryExtractBaseTableName(sql);
            }
            else
            {
                var affected = cmd.ExecuteNonQuery();
                ResultText.Text = $"OK. Змінено рядків: {affected}";
                _currentTable = null;
                _currentTableName = null;
            }
        }
        catch (Exception ex)
        {
            ResultText.Text = $"Помилка: {ex.Message}";
        }
        finally
        {
            try { _context.Database.CloseConnection(); } catch { /* ignore */ }
        }
    }

    private static string? TryExtractBaseTableName(string sql)
    {
        // Дуже проста евристика: select ... from <table>
        var m = Regex.Match(sql, @"(?is)select[\s\S]*?from\s+([a-zA-Z0-9_\.]+)");
        if (m.Success)
        {
            var name = m.Groups[1].Value.Trim();
            // відрізаємо аліас/крапки, беремо останню частину
            var lastDot = name.LastIndexOf('.');
            return lastDot >= 0 ? name[(lastDot + 1)..] : name;
        }
        return null;
    }

    private void ResultGrid_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        {
            SaveGridChanges();
            e.Handled = true;
        }
    }

    private void SaveGridChanges()
    {
        // Завершити поточне редагування осередку/рядка
        ResultGrid.CommitEdit(DataGridEditingUnit.Cell, true);
        ResultGrid.CommitEdit(DataGridEditingUnit.Row, true);

        if (_currentTable == null || string.IsNullOrWhiteSpace(_currentTableName))
        {
            ResultText.Text = "Редагування можливе лише для простого SELECT з однієї таблиці.";
            return;
        }

        var changes = _currentTable.GetChanges(DataRowState.Modified);
        if (changes == null || changes.Rows.Count == 0)
        {
            ResultText.Text = "Немає змін для збереження.";
            return;
        }

        // очікуємо наявність первинного ключа id
        if (!_currentTable.Columns.Contains("id"))
        {
            ResultText.Text = "Не знайдено колонки 'id'. Оновлення неможливе.";
            return;
        }

        try
        {
            _context.Database.OpenConnection();
            var conn = _context.Database.GetDbConnection();

            int total = 0;
            foreach (DataRow row in changes.Rows)
            {
                // формуємо UPDATE table SET col=@p,... WHERE id=@id
                var setParts = new List<string>();
                var cmd = conn.CreateCommand();

                foreach (DataColumn col in _currentTable.Columns)
                {
                    if (string.Equals(col.ColumnName, "id", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (row[col, DataRowVersion.Current]?.Equals(row[col, DataRowVersion.Original] ?? DBNull.Value) == true)
                        continue; // колонка не змінена

                    var p = cmd.CreateParameter();
                    p.ParameterName = "@p_" + col.ColumnName;
                    p.Value = row[col] ?? DBNull.Value;
                    cmd.Parameters.Add(p);
                    setParts.Add($"\"{col.ColumnName}\" = {p.ParameterName}");
                }

                if (setParts.Count == 0)
                    continue; // нічого оновлювати

                var idParam = cmd.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = row["id"];
                cmd.Parameters.Add(idParam);

                cmd.CommandText = $"update \"{_currentTableName}\" set {string.Join(", ", setParts)} where \"id\" = @id";
                var affected = cmd.ExecuteNonQuery();
                total += affected;
            }

            _currentTable.AcceptChanges();
            ResultText.Text = $"OK. Оновлено рядків: {total}";
        }
        catch (Exception ex)
        {
            ResultText.Text = $"Помилка збереження: {ex.Message}";
        }
        finally
        {
            try { _context.Database.CloseConnection(); } catch { }
        }
    }
}
