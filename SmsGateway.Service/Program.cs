using System;
using System.IO.Ports;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;

class Program
{
    private static HttpClient _http = new HttpClient();
    private static bool _useHardware = false;
    private static string _comPort;
    private static int _baudRate;
    private static int _pollInterval;

    static async Task Main()
    {
        Console.Title = "SMS Gateway Service";
        Console.WriteLine("=== SMS Gateway Service ===");

        LoadConfig();

        _http.BaseAddress = new Uri(Config("WebApiBaseUrl"));

        SerialPort? port = null;

        if (_useHardware)
        {
            Console.WriteLine($"Connecting to GSM modem on {_comPort} ...");
            port = new SerialPort(_comPort, _baudRate);
            port.NewLine = "\r\n";
            port.Open();
            Console.WriteLine("✅ Modem connected.");
        }
        else
        {
            Console.WriteLine("🧪 Running in TEST MODE (no SMS will be sent).");
        }

        while (true)
        {
            try
            {
                var pending = await GetPendingMessages();
                foreach (var msg in pending)
                {
                    Console.WriteLine($"Processing SMS #{msg.Id} → {msg.PhoneNumber}");

                    bool sent = false;
                    string response = "";

                    if (_useHardware && port != null)
                    {
                        sent = SendSmsWithModem(port, msg.PhoneNumber, msg.Message, out response);
                    }
                    else
                    {
                        // Simulate fake sending in test mode
                        await Task.Delay(1000);
                        sent = true;
                        response = "SIM900A_TEST_MODE: Message accepted (simulated)";
                    }

                    await UpdateStatus(msg.Id, sent, response);
                    Console.WriteLine($"→ {(sent ? "✅ Sent" : "❌ Failed")}: {response}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error: {ex.Message}");
            }

            await Task.Delay(_pollInterval * 1000);
        }
    }

    static bool SendSmsWithModem(SerialPort port, string number, string message, out string response)
    {
        try
        {
            port.WriteLine("AT");
            Thread.Sleep(200);
            port.WriteLine("AT+CMGF=1");
            Thread.Sleep(200);
            port.WriteLine($"AT+CMGS=\"{number}\"");
            Thread.Sleep(500);
            port.Write(message + char.ConvertFromUtf32(26)); // Ctrl+Z
            Thread.Sleep(4000);
            response = port.ReadExisting();
            return response.Contains("OK");
        }
        catch (Exception ex)
        {
            response = ex.Message;
            return false;
        }
    }

    static async Task<List<SmsMessage>> GetPendingMessages()
    {
        var json = await _http.GetStringAsync("pending");
        return JsonConvert.DeserializeObject<List<SmsMessage>>(json);
    }

    static async Task UpdateStatus(int id, bool sent, string response)
    {
        var payload = new
        {
            Id = id,
            Status = sent ? "Sent" : "Failed",
            Response = response
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var result = await _http.PostAsync("update-status", content);
        Console.WriteLine(result.IsSuccessStatusCode 
            ? $"   ↳ Status updated on server." 
            : $"   ⚠️ Failed to update server status: {result.StatusCode}");
    }

    static string Config(string key)
    {
        var text = System.IO.File.ReadAllText("appsettings.json");
        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
        return data[key]?.ToString() ?? "";
    }

    static void LoadConfig()
    {
        var text = System.IO.File.ReadAllText("appsettings.json");
        var cfg = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);

        _useHardware = bool.Parse(cfg["UseHardware"].ToString());
        _comPort = cfg["ComPort"].ToString();
        _baudRate = int.Parse(cfg["BaudRate"].ToString());
        _pollInterval = int.Parse(cfg["PollingIntervalSeconds"].ToString());
    }

    public class SmsMessage
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
