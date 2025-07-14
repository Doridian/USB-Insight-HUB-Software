using System;
using System.Windows.Forms;
using System.Management; // Required for USB device detection
using System.Collections.Generic;

class Program
{
    private static NotifyIcon trayIcon = null!;
    private static Icon greenIcon = null!;
    private static Icon redIcon = null!;
    private static System.Windows.Forms.Timer blinkTimer = null!;
    private static bool isGreen = true;

    static void Main()
    {
        Console.WriteLine("APP started");
        try
        {
            // Load icons
            greenIcon = new Icon("greenCircle.ico");
            redIcon = new Icon("redCircle.ico");
            Console.WriteLine("icones loaded");

            // Create tray icon
            trayIcon = new NotifyIcon
            {
                Icon = greenIcon,
                Text = "USB Insight Hub",
                Visible = true
            };

            // Update menu when clicked
            trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    UpdateUsbMenu();
                }
            };

            // Initial menu setup
            UpdateUsbMenu();

            // Set up timer
            blinkTimer = new System.Windows.Forms.Timer()
            {
                Interval = 9000
            };
            blinkTimer.Tick += BlinkTimer_Tick;
            blinkTimer.Start();

            Application.Run();
        }
        finally
        {
            Console.WriteLine("app finalized");
            blinkTimer?.Stop();
            trayIcon?.Dispose();
            greenIcon?.Dispose();
            redIcon?.Dispose();
        }
    }

    private static void UpdateUsbMenu()
    {
        var menu = new ContextMenuStrip();
        
        // Get USB devices
        List<string> usbDevices = GetConnectedUsbDevices();

        if (usbDevices.Count > 0)
        {
            menu.Items.Add("Connected USB Devices:", null);
            foreach (var device in usbDevices)
            {
                menu.Items.Add(device, null);
            }
            menu.Items.Add("-"); // Separator
        }
        else
        {
            menu.Items.Add("No USB devices connected", null);
            menu.Items.Add("-"); // Separator
        }

        // Add Exit button
        menu.Items.Add("Exit", null, (s, e) => Application.Exit());

        trayIcon.ContextMenuStrip = menu;
    }

    private static List<string> GetConnectedUsbDevices()
    {
        List<string> devices = new List<string>();
        
        try
        {
            using (var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%USB%'"))
            {
                foreach (ManagementObject device in searcher.Get())
                {
                    string description = device["Caption"]?.ToString() ?? "Unknown USB Device";
                    if (!description.Contains("Hub") && !description.Contains("Host Controller"))
                    {
                        devices.Add(description);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            devices.Add("Error detecting USB devices");
            Console.WriteLine(ex.Message);
        }

        return devices;
    }

    private static void BlinkTimer_Tick(object sender, EventArgs e)
    {
        if (isGreen)
        {
            trayIcon.Icon = redIcon;
            blinkTimer.Interval = 1000;
            isGreen = false;
        }
        else
        {
            trayIcon.Icon = greenIcon;
            blinkTimer.Interval = 9000;
            isGreen = true;
        }
    }
}