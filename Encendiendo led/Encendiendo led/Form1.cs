using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Encendiendo_led
{
    public partial class Form1 : Form
    {
        private delegate void DelegadoAcceso(string accion);
        private string strBufferIn;
        public Form1()
        {
            InitializeComponent();
        }
        private void AccesoForm(string accion)
        {
            strBufferIn = accion;
            textBox1.Text = strBufferIn;
            if (textBox1.Text == "1")
            {
                pictureBox2.Visible = true;
                pictureBox1.Visible = false;
                //label3.Visible = true;
                //label3.Text = "ENCENDIDO";
            }
            else if (textBox1.Text == "0")
            {
                pictureBox2.Visible = false;
                pictureBox1.Visible = true;
                //label3.Visible = true;
                //label3.Text = "APAGADO";
            }
        }

        private void AccesoInterrupcion(string accion)
        {
            DelegadoAcceso Var_DelegadoAcceso;
            Var_DelegadoAcceso = new DelegadoAcceso(AccesoForm);
            object[] arg = { accion };
            base.Invoke(Var_DelegadoAcceso, arg);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            strBufferIn = "";
            BtnConectar.Enabled = false;
            pictureBox2.Visible = false;
            pictureBox1.Visible = false;
            label3.Visible = false; 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] PuertosDisponibles = SerialPort.GetPortNames();
            CboPuertos.Items.Clear();
            foreach (string puerto_simple in PuertosDisponibles)
            {
                CboPuertos.Items.Add(puerto_simple);
            }
            if (CboPuertos.Items.Count > 0)
            {
                CboPuertos.SelectedIndex = 0;
                MessageBox.Show("Selecciona el puerto de trabajo");
                BtnConectar.Enabled = true;
            }
            else
            {
                MessageBox.Show("Ningun puerto detectado");
                CboPuertos.Items.Clear();
                CboPuertos.Text = " ";
                strBufferIn = "";
                BtnConectar.Enabled = false;
            }
        }

        private void BtnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                if (BtnConectar.Text == "Conectar")
                {
                    /*Aquí se configura el puerto para la comunicación, se le da el valor 
                     del combo box cbobaudrate para seleccionar los baudios en los que se 
                     desea trabajar*/
                    serialPort1.BaudRate = Int32.Parse(CboBaudrate.Text);
                    serialPort1.DataBits = 8;
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Handshake = Handshake.None;
                    serialPort1.PortName = CboPuertos.Text;
                    try
                    {
                        /*Aquí se abre el puerto para la conexión con la interfaz
                         se desabilitan los combo box para evitar cambiar de puerto
                         y el boton conectar pasa a ser desconectar*/
                        serialPort1.Open();
                        BtnConectar.Text = "Desconectar";
                        CboPuertos.Enabled = false;
                        CboBaudrate.Enabled = false;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString());
                    }
                }
                else if (BtnConectar.Text == "Desconectar")
                {
                    /*Cuando se presiona el boton desconectar se cierra la conexión con el puerto
                     se habilitan los combo box para poder conectar a otro puerto y el boton desconectar
                     vuelve a ser conectar*/
                    serialPort1.Close();
                    BtnConectar.Text = "Conectar";
                    CboPuertos.Enabled = true;
                    CboBaudrate.Enabled = true;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
            }
        }

        private void DatoRecibido(object sender, SerialDataReceivedEventArgs e)
        {
            AccesoInterrupcion(serialPort1.ReadExisting());
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}