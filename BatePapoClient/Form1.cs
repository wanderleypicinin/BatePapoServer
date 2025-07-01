using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace BatePapoClient
{
    public partial class Form1 : Form
    {
        private HubConnection connection = null;
        public Form1()
        {
            InitializeComponent();
            InitializeSignalR();
        }

        private void InitializeSignalR()
        {
            throw new NotImplementedException();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {

        }
    }
}
