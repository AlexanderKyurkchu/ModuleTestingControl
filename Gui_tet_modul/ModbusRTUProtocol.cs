using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

namespace Gui_test_modul
{

  

    public class ModbusRTUProtocol
    {

        // Declares variables
        private byte slaveAddress = 0x05;


        // private byte functionReadCoils = 0x01;
        // private ushort startAddressReadCoils = 99;
        //  private uint _NumberOfPointReadCoils = 8;

        // private byte functionWriteCoil = 0x05;
        // private ushort startAddressWriteCoil = 99;
        //  private uint _NumberOfPointWriteCoil = 1;

        private byte functionWriteMultCoils = 0x0f;
        private ushort startAddressMultWriteCoil = 199;
        private uint _NumberOfPointMultWriteCoil = 8;

        private byte functionReadInputStatus = 0x02;
        private ushort startAddressInputstatus = 99;
        private uint _NumberOfPointsStatus = 8;


        private byte functionReadHolReg = 0x03;
        private ushort startAddressHolReg = 129;
        private uint _NumberOfPointsHolReg = 32;

        private byte functionWriteHolReg = 0x10;
        private ushort startAddrWriteHolReg = 129;
        //  private uint  _NumberOfOutputHolReg = 32;


        // private byte functionReadInputReg = 0x04;
        // private ushort startAddressInputReg = 1;


      
    
        private List<Register> _RegistersHolReg = new List<Register>();

        private List<Register> _RegistersInputStatus = new List<Register>();

        private List<Register> _RegistersWriteCoilReg = new List<Register>();

        private List<Register> _RegistersMultWriteCoilReg = new List<Register>();


        private byte[] _OutputDataArray = new byte[64];


        private ushort TempData;

        private ushort _MultRegistersVal;



        public ModbusRTUProtocol(uint pNumberOfPointsHolReg, uint pNumberOfPointsStatus, uint pNumberOfPointsWriteCoilReg, uint pNumberOfPointsMultWriteCoilReg)
        {
            this.NumberOfPointsHolReg = pNumberOfPointsHolReg;

            for (int i = 0; i < this.NumberOfPointsHolReg; i++)
            {
                _RegistersHolReg.Add(new Register() { Address = (ushort)(startAddressHolReg + i) }); // cast type to ushort.
            }

            this.NumberOfPointsStatus = pNumberOfPointsStatus;

            for (int i = 0; i < this.NumberOfPointsStatus; i++)
            {
                _RegistersInputStatus.Add(new Register() { Address = (ushort)(startAddressInputstatus + i) }); // cast type to ushort.
            }

            //   this.NumberOfPointWriteCoils = pNumberOfPointsWriteCoilReg;

            //   for (int i = 0; i < this.NumberOfPointWriteCoils; i++)
            //   {
            //       _RegistersWriteCoilReg.Add(new Register() { Address = (ushort)(startAddressWriteCoil + i) }); // cast type to ushort.
            //   }


            this.NumberOfPointMultWriteCoil = pNumberOfPointsMultWriteCoilReg;

            for (int i = 0; i < this.NumberOfPointMultWriteCoil; i++)
            {
                _RegistersMultWriteCoilReg.Add(new Register() { Address = (ushort)(startAddressMultWriteCoil + i) });

            }

        }


        /// <summary>
        /// Starts Modbus RTU Service.
        /// </summary>
        public void Start(SerialPort serialPort)
        {
            if (serialPort.IsOpen) 
            {
                serialPort.Close(); 
            }
            //Thread.Sleep(10); // Delay 100ms
            serialPort.Open();
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                while (true)
                {
                    //***********Read Holding Registers***************
                    if (serialPort.IsOpen)
                    {
                        byte[] frame = ReadHoldingRegistersMsg(slaveAddress, startAddressHolReg, functionReadHolReg, NumberOfPointsHolReg);
                        serialPort.Write(frame, 0, frame.Length);
                        Thread.Sleep(100); // Delay 100ms

                        if (serialPort.IsOpen)
                        {
                            if (serialPort.BytesToRead >= 5)
                            {
                                byte[] bufferReceiver = new byte[serialPort.BytesToRead];
                                serialPort.Read(bufferReceiver, 0, serialPort.BytesToRead);
                                serialPort.DiscardInBuffer();

                                // Process data.
                                byte[] data = new byte[bufferReceiver.Length - 5];
                                Array.Copy(bufferReceiver, 3, data, 0, data.Length);


                                Int16[] result = Word.IntByteToUInt16(data);

                                float VDDD_U = (float)Word.convert_uint_to_float2((uint)result[1], (uint)result[0]);
                                float VDDD_I = (float)Word.convert_uint_to_float2((uint)result[3], (uint)result[2]);
                                float VDDA_U = (float)Word.convert_uint_to_float2((uint)result[5], (uint)result[4]);
                                float VDDA_I = (float)Word.convert_uint_to_float2((uint)result[7], (uint)result[6]);
                                float V2_5VDD_U = (float)Word.convert_uint_to_float2((uint)result[9], (uint)result[8]);
                                float VMMerge_DAC_U = (float)Word.convert_uint_to_float2((uint)result[11], (uint)result[10]);
                                float VDDA_DAC_U = (float)Word.convert_uint_to_float2((uint)result[13], (uint)result[12]);
                                float VMMerge_DAC_I = (float)Word.convert_uint_to_float2((uint)result[15], (uint)result[14]);
                                float VDD_PIX_U = (float)Word.convert_uint_to_float2((uint)result[17], (uint)result[16]);
                                float VDD_PIX_I = (float)Word.convert_uint_to_float2((uint)result[19], (uint)result[18]);
                                float VDDA_TRANS_U = (float)Word.convert_uint_to_float2((uint)result[21], (uint)result[20]);
                                // float VDDA_TRANS_I = (float)Word.convert_uint_to_float2((uint)result[23], (uint)result[22]);
                                float VCCD_U = (float)Word.convert_uint_to_float2((uint)result[25], (uint)result[24]);
                                //  float VCCD_I      = (float)Word.convert_uint_to_float2((uint)result[27], (uint)result[26]);
                                float VDDU_U = (float)Word.convert_uint_to_float2((uint)result[29], (uint)result[28]);
                                //  float VDDU_I      = (float)Word.convert_uint_to_float2((uint)result[31], (uint)result[30]);

                                RegistersHolReg[0].Value = VDDD_U;
                                RegistersHolReg[1].Value = VDDD_I;
                                RegistersHolReg[2].Value = VDDA_U;
                                RegistersHolReg[3].Value = VDDA_I;
                                RegistersHolReg[4].Value = V2_5VDD_U;
                                RegistersHolReg[5].Value = VMMerge_DAC_U;
                                RegistersHolReg[6].Value = VDDA_DAC_U;
                                RegistersHolReg[7].Value = VMMerge_DAC_I;
                                RegistersHolReg[8].Value = VDD_PIX_U;
                                RegistersHolReg[9].Value = VDD_PIX_I;
                                RegistersHolReg[10].Value = VDDA_TRANS_U;
                                // RegistersHolReg[11].Value = VDDA_TRANS_I;
                                RegistersHolReg[12].Value = VCCD_U;
                                // RegistersHolReg[13].Value = VCCD_I;
                                RegistersHolReg[14].Value = VDDU_U;
                                //  RegistersHolReg[15].Value = VDDU_I;

                            }
                        }

                    }
                    Thread.Sleep(50); // Delay 20ms
                    //*********************************************************************
                    // Write holding registers

                    if (serialPort.IsOpen)
                    {

                        byte[] WriteFrame = WriteHoldingRegistersMsg(slaveAddress, startAddrWriteHolReg, functionWriteHolReg, 32, _OutputDataArray);
                        serialPort.Write(WriteFrame, 0, WriteFrame.Length);

                        Thread.Sleep(100); // Delay 100ms

                        if (serialPort.IsOpen)
                        {
                            if (serialPort.BytesToRead >= 3)
                            {
                                byte[] bufferReceiver = new byte[serialPort.BytesToRead];
                                serialPort.Read(bufferReceiver, 0, serialPort.BytesToRead);
                                serialPort.DiscardInBuffer();

                            }
                        }
                    }
                    Thread.Sleep(50); // Delay 20ms

                    //********************************************************************


                    // Read input registers 
                    if (serialPort.IsOpen)
                    {
                        byte[] frame2 = ReadInputRegistersMsg(slaveAddress, startAddressInputstatus, functionReadInputStatus, NumberOfPointsStatus);
                        serialPort.Write(frame2, 0, frame2.Length);
                        Thread.Sleep(100); // Delay 100ms

                        if (serialPort.IsOpen)
                        {
                            if (serialPort.BytesToRead >= 5)
                            {
                                byte[] bufferReceiver = new byte[serialPort.BytesToRead];
                                serialPort.Read(bufferReceiver, 0, serialPort.BytesToRead);
                                serialPort.DiscardInBuffer();

                                // Process data.
                                byte[] data = new byte[bufferReceiver.Length - 5];
                                Array.Copy(bufferReceiver, 3, data, 0, data.Length);
                                byte tempData = 0x00;

                                for (int i = 0; i < data.Length; i++)
                                {
                                    tempData = data[i];
                                    for (int k = 0; k < 8; k++)
                                    {
                                        RegistersInputStatus[k].Value = (short)((tempData >> k) & 0x01);
                                    }
                                }
                            }
                        }
                    }

                    Thread.Sleep(20); // Delay 20ms

                    //*********************************************************************
                    // Write  mult coils registers

                    if (serialPort.IsOpen)
                    {

                        byte[] frame4 = WriteMultCoilRegisterMsg(slaveAddress, startAddressMultWriteCoil, functionWriteMultCoils, _MultRegistersVal);
                        serialPort.Write(frame4, 0, frame4.Length);
                        Thread.Sleep(100); // Delay 100ms
                        if (serialPort.IsOpen)
                        {
                            if (serialPort.BytesToRead >= 5)
                            {
                                byte[] bufferReceiverCoils = new byte[serialPort.BytesToRead];
                                serialPort.Read(bufferReceiverCoils, 0, serialPort.BytesToRead);
                                serialPort.DiscardInBuffer();
                            }
                        }
                    }
                    Thread.Sleep(50); // Delay 20ms
                }

            }));

        }

        /// <summary>
        /// Function 03 (03hex) Read Holding Registers
        /// Read the binary contents of holding registers in the slave.
        /// </summary>
        /// <param name="slaveAddress">Slave Address</param>
        /// <param name="startAddress">Starting Address</param>
        /// <param name="function">Function</param>
        /// <param name="numberOfPoints">Quantity of inputs</param>
        /// <returns>Byte Array</returns>
        private byte[] ReadHoldingRegistersMsg(byte slaveAddress, ushort startAddress, byte function, uint numberOfPoints)
        {
            byte[] frame = new byte[8];
            frame[0] = slaveAddress;			    // Slave Address
            frame[1] = function;				    // Function             
            frame[2] = (byte)(startAddress >> 8);	// Starting Address High
            frame[3] = (byte)startAddress;		    // Starting Address Low            
            frame[4] = (byte)(numberOfPoints >> 8);	// Quantity of Registers High
            frame[5] = (byte)numberOfPoints;		// Quantity of Registers Low
            byte[] crc = this.CalculateCRC(frame);  // Calculate CRC.
            frame[frame.Length - 2] = crc[0];       // Error Check Low
            frame[frame.Length - 1] = crc[1];       // Error Check High
            return frame;
        }

        /// <summary>
        /// Function 16 (10hex) Write Holding Registers
        /// Read the binary contents of holding registers in the slave.
        /// </summary>
        /// <param name="slaveAddress">Slave Address</param>
        /// <param name="startAddress">Starting Address</param>
        /// <param name="function">Function</param>
        /// <param name="numberOfPoints">Quantity of inputs</param>
        /// <returns>Byte Array</returns>
        private byte[] WriteHoldingRegistersMsg(byte slaveAddress, ushort startAddress, byte function, uint numberOfPoints, byte[] dataArray)
        {

            byte[] frame = new byte[9 + numberOfPoints * 2];
            frame[0] = slaveAddress;			        // Slave Address
            frame[1] = function;				        // Function             
            frame[2] = (byte)(startAddress >> 8);	    // Starting Address High
            frame[3] = (byte)startAddress;		        // Starting Address Low            
            frame[4] = (byte)(numberOfPoints >> 8);	    // Quantity of Registers High
            frame[5] = (byte)numberOfPoints;		    // Quantity of Registers Low
            frame[6] = (byte)(numberOfPoints * 2);      // Quantity of bytes

            Array.Copy(dataArray, 0, frame, 7, 64);

            byte[] crc = this.CalculateCRC(frame);  // Calculate CRC.
            frame[frame.Length - 2] = crc[0];       // Error Check Low
            frame[frame.Length - 1] = crc[1];       // Error Check High

            return frame;
        }




        /// <summary>
        /// Function 04 (04hex) Read Input Registers
        /// Read the binary contents of Input registers in the slave.
        /// </summary>
        /// <param name="slaveAddress">Slave Address</param>
        /// <param name="startAddress">Starting Address</param>
        /// <param name="function">Function</param>
        /// <param name="numberOfPoints">Quantity of inputs</param>
        /// <returns>Byte Array</returns>

        private byte[] ReadInputRegistersMsg(byte slaveAddress, ushort startAddress, byte function, uint numberOfPoints)
        {
            byte[] frame = new byte[8];
            frame[0] = slaveAddress;			    // Slave Address
            frame[1] = function;				    // Function             
            frame[2] = (byte)(startAddress >> 8);	// Starting Address High
            frame[3] = (byte)startAddress;		    // Starting Address Low            
            frame[4] = (byte)(numberOfPoints >> 8);	// Quantity of Registers High
            frame[5] = (byte)numberOfPoints;		// Quantity of Registers Low
            byte[] crc = this.CalculateCRC(frame);  // Calculate CRC.
            frame[6] = crc[0];       // Error Check Low
            frame[7] = crc[1];       // Error Check High
            return frame;
        }



        /// <summary>
        /// Function 05 (05hex) Write coil Registers
        /// Read the binary contents of write coils registers in the slave.
        /// </summary>
        /// <param name="slaveAddress">Slave Address</param>
        /// <param name="startAddress">Starting Address</param>
        /// <param name="function">Function</param>
        /// <param name="numberOfPoints">Quantity of inputs</param>
        /// <returns>Byte Array</returns>

        private byte[] WriteCoilRegisterMsg(byte slaveAddress, ushort startAddress, byte function, ushort valueofRegister)
        {
            byte[] frame = new byte[8];
            frame[0] = slaveAddress;			    // Slave Address
            frame[1] = function;				    // Function             
            frame[2] = (byte)(startAddress >> 8);	// Starting Address High
            frame[3] = (byte)startAddress;		    // Starting Address Low            
            frame[4] = (byte)(valueofRegister >> 8);	// Value of Registers High
            frame[5] = (byte)valueofRegister;		// Value of Registers Low
            byte[] crc = this.CalculateCRC(frame);  // Calculate CRC.
            frame[6] = crc[0];       // Error Check Low
            frame[7] = crc[1];       // Error Check High
            return frame;
        }


        /// <summary>
        /// Function 0f (0fhex) Write  multiple coils Registers
        /// Read the binary contents of write coils registers in the slave.
        /// </summary>
        /// <param name="slaveAddress">Slave Address</param>
        /// <param name="startAddress">Starting Address</param>
        /// <param name="function">Function</param>
        /// <param name="numberOfPoints">Quantity of inputs</param>
        /// <returns>Byte Array</returns>

        private byte[] WriteMultCoilRegisterMsg(byte slaveAddress, ushort startAddress, byte function, ushort valueofRegister)
        {
            byte[] frame = new byte[10];
            frame[0] = slaveAddress;			    // Slave Address
            frame[1] = function;				    // Function             
            frame[2] = (byte)(startAddress >> 8);	// Starting Address High
            frame[3] = (byte)startAddress;          // Starting Address Low 
            frame[4] = 0x00;                        // Quantity of Registers High
            frame[5] = 0x08;                        // Quantity of Registers Low
            frame[6] = 0x01;                        // Quantity of Bytes
            frame[7] = (byte)valueofRegister;

            byte[] crc = this.CalculateCRC(frame);  // Calculate CRC.
            frame[8] = crc[0];                      // Error Check Low
            frame[9] = crc[1];                     // Error Check High
            return frame;
        }


        /// <summary>
        /// CRC Calculation 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] CalculateCRC(byte[] data)
        {
            ushort CRCFull = 0xFFFF; // Set the 16-bit register (CRC register) = FFFFH.
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;
            byte[] CRC = new byte[2];
            for (int i = 0; i < (data.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ data[i]); // 

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
            return CRC;
        }


        /// <summary>
        /// Properties: NumberOfPointsHolreg
        /// </summary>
        public uint NumberOfPointsHolReg
        {
            get
            {
                return _NumberOfPointsHolReg;
            }

            set
            {
                _NumberOfPointsHolReg = value;
            }
        }

        public uint NumberOfPointsStatus
        {
            get
            {
                return _NumberOfPointsStatus;
            }

            set
            {
                _NumberOfPointsStatus = value;
            }
        }

       
        public uint NumberOfPointMultWriteCoil
        {
            get
            {
                return _NumberOfPointMultWriteCoil;
            }

            set
            {
                _NumberOfPointMultWriteCoil = value;
            }

        }


        public ushort MultRegistersVal
        {
            get
            {
                return _MultRegistersVal;
            }

            set
            {
                _MultRegistersVal = value;
            }

        }
        public byte[] OutputDataArray

        {
            get
            {
                return _OutputDataArray;
            }

            set
            {
                _OutputDataArray = value;
            }

        }


        /// <summary>
        /// Properties: Holding Registers
        /// </summary>
        public List<Register> RegistersHolReg
        {
            get
            {
                return _RegistersHolReg;
            }

            set
            {
                _RegistersHolReg = value;
            }
        }

        /// <summary>
        /// Properties: Input Registers
        /// </summary>
        public List<Register> RegistersInputStatus
        {
            get
            {
                return _RegistersInputStatus;
            }

            set
            {
                _RegistersInputStatus = value;
            }
        }


        /// <summary>
        /// Properties: Write coils Registers
        /// </summary>
        public List<Register> RegistersWriteCoilReg
        {
            get
            {
                return _RegistersWriteCoilReg;
            }

            set
            {
                _RegistersWriteCoilReg = value;
            }
        }

    }
}
