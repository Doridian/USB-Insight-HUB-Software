/**
*   USB Insight Hub Enumeration Extraction Agent
*
*   Works in tandem with USB Insight Hub hardware
*   
*   https://github.com/Aeriosolutions/USB-Insight-HUB-Software
*
*   Copyright (C) 2024 - 2025 Aeriosolutions
*   Copyright (C) 2024 - 2025 estebanfex / JoDaSa

* MIT License. 
**/


using System;
using System.Windows.Forms;
using System.Management; // Required for USB device detection
using System.Collections.Generic;
using EnumerationExtractionAgent;

class Program
{
    private static NotifyIcon trayIcon = null!;
    private static Icon yesIcon = null!;
    private static Icon noIcon = null!;
    private static Icon pauseIcon = null!;
    private static System.Windows.Forms.Timer blinkTimer = null!;
    private static System.Windows.Forms.Timer usbTreeChangeTimer = null!;
   
    private static UIHAgent Agent = new UIHAgent();
    private static bool runState = true;

    static void Main()
    {
        Console.WriteLine("APP started");
        try
        {
            // Load icons
            yesIcon = new Icon("yesCon.ico");
            noIcon = new Icon("noCon.ico");
            pauseIcon = new Icon("pauseCon.ico");
            Console.WriteLine("icones loaded");

            // Create tray icon
            trayIcon = new NotifyIcon
            {
                Icon = yesIcon,
                Text = "USB Insight Hub",
                Visible = true
            };

            // Update menu when clicked
            trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    UpdateUsbMenu();
                }
            };

            // Initial menu setup
            //UpdateUsbMenu();

            Agent.Start();


            // Set timer to check changes in device tree
            usbTreeChangeTimer = new System.Windows.Forms.Timer()
            {
                Interval = 500
            };
            usbTreeChangeTimer.Tick += UsbTreeChangeTimer_Tick;
            usbTreeChangeTimer.Start();

            Application.Run();
        }
        finally
        {
            Console.WriteLine("app finalized");
            Agent.Stop();
            usbTreeChangeTimer?.Stop();
            trayIcon?.Dispose();
            yesIcon?.Dispose();
            noIcon?.Dispose();
            pauseIcon?.Dispose();
        }
    }

    private static void UsbTreeChangeTimer_Tick(object? sender, EventArgs e)
    {
        if(Agent.getUpdatedFlag() && runState)
        {
            //Run logic of changes from hub

            var numHubs = Agent.getConnectedHubs();

            if (numHubs > 0)
            {
                trayIcon.Icon = yesIcon;
                trayIcon.Text = "USB Insight Hub - Connected(" + numHubs +")";
            }
            else
            {
                trayIcon.Icon = noIcon;
                trayIcon.Text = "USB Insight Hub - Disconnected";
            }

            Agent.setUpdatedFlag(false);
        }

        if (!runState)
        {
            trayIcon.Icon = pauseIcon;
            trayIcon.Text = "USB Insight Hub - Paused";
        }


    }

    private static void UpdateUsbMenu()
    {
        var menu = new ContextMenuStrip();
        
        if (runState)
        {
            // Add Pause item
            menu.Items.Add("Pause", null, (s, e) => { 
                Agent.Stop();
                runState = false;
                UpdateUsbMenu();
            });
        }
        else
        {
            // Add Pause item
            menu.Items.Add("Run", null, (s, e) => {
                Agent.Start();
                runState = true;
                UpdateUsbMenu();
            });
        }

        // Add Exit button
        menu.Items.Add("Exit", null, (s, e) =>
        {
            Agent.Stop();
            Application.Exit();
        });

        trayIcon.ContextMenuStrip = menu;
    }
    

}