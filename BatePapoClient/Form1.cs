using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace BatePapoClient
{
    public partial class Form1 : Form
    {
        private HubConnection _connection = null;
        public Form1()
        {
            InitializeComponent();
            InitializeSignalR();
        }

        private async void InitializeSignalR()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chathub")
                .Build();
            _connection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    this.Invoke((Action)(() =>
                        {
                            lstMessages.Items.Add($"{user}: {message}");
                        }));
                });
            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)

            {
                lstMessages.Items.Add($"Erro ao conectar: {ex.Message}");
            }
        }               
        

        private async void btnEnviar_Click(object sender, EventArgs e)

        {
            try
            {
                await _connection.InvokeAsync("SendMessage", txtUser.Text, txtMessage.Text);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                lstMessages.Items.Add("Erro ao enviar: {ex.Message}");
                
            }


        }
    }
}
