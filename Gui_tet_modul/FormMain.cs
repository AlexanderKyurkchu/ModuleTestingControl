using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace Gui_test_modul
{

   
    public partial class Form1 : Form
    {
        const uint NumberOfPointHolReg = 32;        // Read 32 holding registers

        const uint NumberOfPointsStatus = 8;        // Read 8 input status 

        const uint NumberOfPointsWriteCoilReg = 1;  // Write coil

    
        const uint NumberOfPointMultWriteCoil = 8;  // Write miltiple coils


      static  float VDDA_DAC = 0.00f;
      static  float VDDA_TRANS = 0.00f;
      static  float VDDA_PIX = 0.00f;
      static  float VMMerge_DAC = 0.00f;
      static   float prm5 = 0.00f;
      static   float prm6 = 0.00f;
      
        // Declare Object ModBusRTUProtocol
        private ModbusRTUProtocol objModbusRTUProtocol = null;

        private Algorithm objAlgorithm = null;

        // Declare Object OutDataArray
       private static byte[] OutDataArray = new byte[64];
       private bool ModbusFlagOn = false; 

     

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                cBoxComPort.Items.AddRange(ports);

                objModbusRTUProtocol = new ModbusRTUProtocol(NumberOfPointHolReg, NumberOfPointsStatus, NumberOfPointsWriteCoilReg, NumberOfPointMultWriteCoil);

                OutDataArray = objModbusRTUProtocol.OutputDataArray;

                //DataBinding to DispalyControls

                displayControl1.Register = objModbusRTUProtocol.RegistersHolReg[0];    // VDDD   U
                displayControl2.Register = objModbusRTUProtocol.RegistersHolReg[1];    // VDDD   I
                displayControl3.Register = objModbusRTUProtocol.RegistersHolReg[2];    // VDDA   U
                displayControl4.Register = objModbusRTUProtocol.RegistersHolReg[3];    // VDDA   I
                displayControl5.Register = objModbusRTUProtocol.RegistersHolReg[4];    //2V5_VDD U

               // displayControl6.Register = objModbusRTUProtocol.RegistersHolReg[5];  //VMMerge_DAC_U
                displayControl7.Register = objModbusRTUProtocol.RegistersHolReg[6];    //VDDA_DAC U
               // displayControl8.Register = objModbusRTUProtocol.RegistersHolReg[7];  //VMMerge_DAC_I
                displayControl9.Register = objModbusRTUProtocol.RegistersHolReg[8];    //VDD_PIX U
                displayControl10.Register = objModbusRTUProtocol.RegistersHolReg[9];   //VDD_PIX I

                displayControl11.Register = objModbusRTUProtocol.RegistersHolReg[10];  // VDDA_TRANS U
                displayControl12.Register = objModbusRTUProtocol.RegistersHolReg[11];  // VDDA_TRANS I, not used
                displayControl13.Register = objModbusRTUProtocol.RegistersHolReg[12];  // VCDD U
                displayControl14.Register = objModbusRTUProtocol.RegistersHolReg[13];  // VCDD I, not used
                displayControl15.Register = objModbusRTUProtocol.RegistersHolReg[14];  // VDDU U

                displayControl43.Register = objModbusRTUProtocol.RegistersHolReg[5];    //VMMerge_DAC_U
                displayControl44.Register = objModbusRTUProtocol.RegistersHolReg[7];    //VMMerge_DAC_I



                displayControl33.Register = objModbusRTUProtocol.RegistersInputStatus[0];
                displayControl34.Register = objModbusRTUProtocol.RegistersInputStatus[1];
                displayControl35.Register = objModbusRTUProtocol.RegistersInputStatus[2];
                displayControl36.Register = objModbusRTUProtocol.RegistersInputStatus[3];
                displayControl37.Register = objModbusRTUProtocol.RegistersInputStatus[4];
                displayControl38.Register = objModbusRTUProtocol.RegistersInputStatus[5];
                displayControl39.Register = objModbusRTUProtocol.RegistersInputStatus[6];
                displayControl40.Register = objModbusRTUProtocol.RegistersInputStatus[7];
            }

            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            try
            {
                Application.DoEvents();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
               // после отклбчентя com port все значения на интерфейсе обнулить 
                objModbusRTUProtocol.RegistersHolReg[0].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[1].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[2].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[3].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[4].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[5].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[6].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[7].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[8].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[9].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[10].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[11].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[12].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[13].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[14].Value = 0.00f;
                objModbusRTUProtocol.RegistersHolReg[15].Value = 0.00f;


            }
        }

     

        private void powerEnable_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {

                    objModbusRTUProtocol.MultRegistersVal = 0x0003;

                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void powerDisable_Click(object sender, EventArgs e)
        {

            try
            {
                if (serialPort1.IsOpen)
                {

                   objModbusRTUProtocol.MultRegistersVal = 0x0000;

                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cBoxComPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDataBits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxStopBits.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxParityBits.Text);

                serialPort1.Open();
                
                if (serialPort1.IsOpen) 
                { 
                  progressBar1.Value = 100;
                    if (!ModbusFlagOn)
                    {
                        objModbusRTUProtocol.Start(serialPort1);
                        ModbusFlagOn = true;
                    }
                }

            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

      

     
       
     

     

     

     

        private void prm1ButIncrement_Click(object sender, EventArgs e)
        {

            VDDA_DAC += 0.1f;
            if (VDDA_DAC >= 3.3f)
            {
                VDDA_DAC = 3.3f;
            }
           // prm1Lbl.Text = prmUpitFirst.ToString();
            prm1Lbl.Text = String.Format("{0:f2}", VDDA_DAC);

            byte[] tempdata = BitConverter.GetBytes(VDDA_DAC);

            OutDataArray[0] = tempdata[1];
            OutDataArray[1] = tempdata[0];
            OutDataArray[2] = tempdata[3];
            OutDataArray[3] = tempdata[2];

        }

        private void prm1ButDecrement_Click(object sender, EventArgs e)
        {
            VDDA_DAC -= 0.1f;

            if (VDDA_DAC <= 0.0f)
            {
                VDDA_DAC = 0.0f;
            }
           // prm1Lbl.Text= prmUpitFirst.ToString("{0:f1}");
            prm1Lbl.Text = String.Format("{0:f2}", VDDA_DAC);
            byte[] tempdata = BitConverter.GetBytes(VDDA_DAC);

            OutDataArray[0] = tempdata[1];
            OutDataArray[1] = tempdata[0];
            OutDataArray[2] = tempdata[3];
            OutDataArray[3] = tempdata[2];

        }

        private void prm1ButReset_Click(object sender, EventArgs e)
        {
            VDDA_DAC = 0.00f;
            prm1Lbl.Text = String.Format("{0:f2}", VDDA_DAC);
            byte[] tempdata = BitConverter.GetBytes(VDDA_DAC);

            OutDataArray[0] = tempdata[1];
            OutDataArray[1] = tempdata[0];
            OutDataArray[2] = tempdata[3];
            OutDataArray[3] = tempdata[2];

        }

       private void prm2ButIncrement_Click(object sender, EventArgs e)
        {
            VDDA_TRANS += 0.1f;
            if (VDDA_TRANS >= 3.3f)
            {
                VDDA_TRANS = 3.3f;
            }
            prm2Lbl.Text = String.Format("{0:f2}", VDDA_TRANS);
            byte[] tempdata = BitConverter.GetBytes(VDDA_TRANS);

            OutDataArray[4] = tempdata[1];
            OutDataArray[5] = tempdata[0];
            OutDataArray[6] = tempdata[3];
            OutDataArray[7] = tempdata[2];

        }

        private void prm2ButDecrement_Click(object sender, EventArgs e)
        {

            VDDA_TRANS -= 0.1f;

            if (VDDA_TRANS <= 0.0f)
            {
                VDDA_TRANS = 0.0f;
            }
            prm2Lbl.Text = String.Format("{0:f2}", VDDA_TRANS);
            byte[] tempdata = BitConverter.GetBytes(VDDA_TRANS);

            OutDataArray[4] = tempdata[1];
            OutDataArray[5] = tempdata[0];
            OutDataArray[6] = tempdata[3];
            OutDataArray[7] = tempdata[2];

        }

        private void prm2ButReset_Click(object sender, EventArgs e)
        {
            VDDA_TRANS = 0.00f;
            prm2Lbl.Text = String.Format("{0:f2}", VDDA_TRANS);
            byte[] tempdata = BitConverter.GetBytes(VDDA_TRANS);

            OutDataArray[4] = tempdata[1];
            OutDataArray[5] = tempdata[0];
            OutDataArray[6] = tempdata[3];
            OutDataArray[7] = tempdata[2];

        }

        private void prm3ButDecrement_Click(object sender, EventArgs e)
        {

            VDDA_PIX -= 0.1f;

            if (VDDA_PIX <= 0.0f)
            {
                VDDA_PIX = 0.0f;
            }
            prm3Lbl.Text = String.Format("{0:f2}", VDDA_PIX);
            byte[] tempdata = BitConverter.GetBytes(VDDA_PIX);

            OutDataArray[8] = tempdata[1];
            OutDataArray[9] = tempdata[0];
            OutDataArray[10] = tempdata[3];
            OutDataArray[11] = tempdata[2];
        }

        private void prm3ButIncrement_Click(object sender, EventArgs e)
        {

            VDDA_PIX += 0.1f;
            if (VDDA_PIX >= 3.3f)
            {
                VDDA_PIX = 3.3f;
            }
            prm3Lbl.Text = String.Format("{0:f2}", VDDA_PIX);
            byte[] tempdata = BitConverter.GetBytes(VDDA_PIX);

            OutDataArray[8] = tempdata[1];
            OutDataArray[9] = tempdata[0];
            OutDataArray[10] = tempdata[3];
            OutDataArray[11] = tempdata[2];
        }

        private void prm3ButReset_Click(object sender, EventArgs e)
        {
            VDDA_PIX = 0.00f;
            prm3Lbl.Text = String.Format("{0:f2}", VDDA_PIX);
            byte[] tempdata = BitConverter.GetBytes(VDDA_PIX);

            OutDataArray[8] = tempdata[1];
            OutDataArray[9] = tempdata[0];
            OutDataArray[10] = tempdata[3];
            OutDataArray[11] = tempdata[2];
        }
       /****************** VVMerge_DAC  Decrement ******************************/
        private void prm4ButDecrement_Click(object sender, EventArgs e)
        {

            VMMerge_DAC -= 0.1f;

            if (VMMerge_DAC <= 0.0f)
            {
                VMMerge_DAC = 0.0f;
            }
            prm4Lbl.Text = String.Format("{0:f2}", VMMerge_DAC);
            byte[] tempdata = BitConverter.GetBytes(VMMerge_DAC);

            OutDataArray[12] = tempdata[1];
            OutDataArray[13] = tempdata[0];
            OutDataArray[14] = tempdata[3];
            OutDataArray[15] = tempdata[2];
        }
      /****************** VVMerge_DAC  Increment ******************************/
        private void prm4ButIncrement_Click(object sender, EventArgs e)
        {
            VMMerge_DAC += 0.1f;
            if (VMMerge_DAC >= 3.3f)
            {
                VMMerge_DAC = 3.3f;
            }
            prm4Lbl.Text = String.Format("{0:f2}", VMMerge_DAC);
            byte[] tempdata = BitConverter.GetBytes(VMMerge_DAC);

            OutDataArray[12] = tempdata[1];
            OutDataArray[13] = tempdata[0];
            OutDataArray[14] = tempdata[3];
            OutDataArray[15] = tempdata[2];
        }
       /****************** VVMerge_DAC  Reset ******************************/
        private void prm4ButReset_Click(object sender, EventArgs e)
        {
            VMMerge_DAC = 0.00f;
            prm4Lbl.Text = String.Format("{0:f2}", VMMerge_DAC);
            byte[] tempdata = BitConverter.GetBytes(VMMerge_DAC);

            OutDataArray[12] = tempdata[1];
            OutDataArray[13] = tempdata[0];
            OutDataArray[14] = tempdata[3];
            OutDataArray[15] = tempdata[2];

        }

        private void prm5ButDecrement_Click(object sender, EventArgs e)
        {

            //prm5 -= 0.1f;

            //if (prm5 <= 0.0f)
            //{
            //    prm5 = 0.0f;
            //}
            //prm5Lbl.Text = String.Format("{0:f2}", prm5);
            //byte[] tempdata = BitConverter.GetBytes(prm5);

            //OutDataArray[16] = tempdata[1];
            //OutDataArray[17] = tempdata[0];
            //OutDataArray[18] = tempdata[3];
            //OutDataArray[19] = tempdata[2];

        }

        private void prm5ButIncrement_Click(object sender, EventArgs e)
        {
            //prm5 += 0.1f;
            //if (prm5 >= 3.3f)
            //{
            //    prm5 = 3.3f;
            //}
            //prm5Lbl.Text = String.Format("{0:f2}", prm5);
            //byte[] tempdata = BitConverter.GetBytes(prm5);

            //OutDataArray[16] = tempdata[1];
            //OutDataArray[17] = tempdata[0];
            //OutDataArray[18] = tempdata[3];
            //OutDataArray[19] = tempdata[2];

        }

        private void prm5ButReset_Click(object sender, EventArgs e)
        {
        //    prm5 = 0.00f;
        //    prm5Lbl.Text = String.Format("{0:f2}", prm5);
        //    byte[] tempdata = BitConverter.GetBytes(prm5);

        //    OutDataArray[16] = tempdata[1];
        //    OutDataArray[17] = tempdata[0];
        //    OutDataArray[18] = tempdata[3];
        //    OutDataArray[19] = tempdata[2];
        //
        }

        private void prm6ButDecrement_Click(object sender, EventArgs e)
        {

            //prm6 -= 0.1f;

            //if (prm6 <= 0.0f)
            //{
            //    prm6 = 0.0f;
            //}
            //prm6Lbl.Text = String.Format("{0:f2}", prm6);
            //byte[] tempdata = BitConverter.GetBytes(prm6);

            //OutDataArray[20] = tempdata[1];
            //OutDataArray[21] = tempdata[0];
            //OutDataArray[22] = tempdata[3];
            //OutDataArray[23] = tempdata[2];
        }

        private void prm6ButIncrement_Click(object sender, EventArgs e)
        {
            //prm6 += 0.1f;
            //if (prm6 >= 3.3f)
            //{
            //    prm6 = 3.3f;
            //}
            //prm6Lbl.Text = String.Format("{0:f2}", prm6);
            //byte[] tempdata = BitConverter.GetBytes(prm6);

            //OutDataArray[20] = tempdata[1];
            //OutDataArray[21] = tempdata[0];
            //OutDataArray[22] = tempdata[3];
            //OutDataArray[23] = tempdata[2];

        }

        private void prm6ButReset_Click(object sender, EventArgs e)
        {
            //prm6 = 0.00f;
            //prm6Lbl.Text = String.Format("{0:f2}", prm6);
            //byte[] tempdata = BitConverter.GetBytes(prm6);

            //OutDataArray[20] = tempdata[1];
            //OutDataArray[21] = tempdata[0];
            //OutDataArray[22] = tempdata[3];
            //OutDataArray[23] = tempdata[2];

        }

        private void displayControl9_Click(object sender, EventArgs e)
        {
         

        }

        private void Start_algorithm1_Click(object sender, EventArgs e)
        {
           
             OutDataArray[60] = 0x00;
             OutDataArray[61] = (byte)Algorithm.STATE_ALGORITHM.ALG_1;
             OutDataArray[62] = 0x00;
             OutDataArray[63] = 0x00;
        }
         
        private void Start_algorithm2_Click(object sender, EventArgs e)
        {

            OutDataArray[60] = 0x00;
            OutDataArray[61] = (byte)Algorithm.STATE_ALGORITHM.ALG_2;

            OutDataArray[62] = 0x00;
            OutDataArray[63] = 0x00;

        }

        private void Start_algorithm3_Click(object sender, EventArgs e)
        {

            OutDataArray[60] = 0x00;
            OutDataArray[61] = (byte)Algorithm.STATE_ALGORITHM.ALG_3;
            OutDataArray[62] = 0x00;
            OutDataArray[63] = 0x00;

        }

       
    }    
    
}
