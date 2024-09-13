using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.IO;
using System;

namespace ClientSide;

public class MainViewModel : INotifyPropertyChanged
{
    public RelayCommand StartCommand { get; set; }
    private ImageSource _imageOpenClose;
    public ImageSource ImageOpenClose
    {
        get
        {
            return _imageOpenClose;
        }
        set
        {
            _imageOpenClose = value;
            OnPropertyChanged(nameof(ImageOpenClose));
        }
    }



    public MainViewModel()
    {

        var server = new UdpClient(27001);
        var remoteEp = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            var btyes = server.Receive(ref remoteEp);

           Console.ReadKey();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }





}
