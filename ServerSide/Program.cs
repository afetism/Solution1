using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("user32.dll")]
    static extern int GetSystemMetrics(int nIndex);

    const int SM_CXSCREEN = 0;
    const int SM_CYSCREEN = 1;

    static void Main(string[] args)
    {
       
        string screenshotPath = TakeScreen();
        if (!string.IsNullOrEmpty(screenshotPath))
        {
            Console.WriteLine($"Screenshot saved at {screenshotPath}");
            SendImageOverUdp(screenshotPath);
        }
    }

    static string TakeScreen()
    {
        try
        {
            // Get the screen width and height
            int screenWidth = GetSystemMetrics(SM_CXSCREEN);
            int screenHeight = GetSystemMetrics(SM_CYSCREEN);

            // Create a bitmap with the same dimensions as the screen
            using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
            {
                // Use Graphics to capture the screen
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));
                }

                // Save the screenshot to a file with valid path formatting
                string filePath = $"screenshot_{DateTime.Now:yy-MM-dd_HH-mm-ss}.png";
                bmp.Save(filePath, ImageFormat.Png);
                return filePath;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    static void SendImageOverUdp(string filePath)
    {
        try
        {
            
            byte[] imageBytes = File.ReadAllBytes(filePath);

         
            using (UdpClient client = new UdpClient())
            {
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Loopback, 27001);
                client.Send(imageBytes, imageBytes.Length, remoteEp);
                
                Console.WriteLine("Screenshot sent over UDP.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while sending image: {ex.Message}");
        }
    }
}
