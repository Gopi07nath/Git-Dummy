using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Calibration
{
    class MemoryTest
    {

        #region I2c Interface -----------------------------------------------------------------
        // For I2C interface
        public byte I2C_IO_PORT_EXP_ADDRESS = 0x70; // I2C address of TI PCA9538 IO Port expander
        public byte I2C_IO_PORT_EXP_ADDRESS_READ = 0xE1; // I2C address of TI PCA9538 IO Port expander
        public byte I2C_IO_PORT_EXP_ADDRESS_WRITE = 0xE0; // I2C address of TI PCA9538 IO Port expander
        public byte I2C_IO_PORT_EXP_COMMAND_INPUT_READ = 0x00;
        public byte I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE = 0x01;
        public byte I2C_IO_PORT_EXP_COMMAND_POLARITY_INVERT = 0x02;
        public byte I2C_IO_PORT_EXP_COMMAND_CONFIGURATION = 0x03;

        // For I2C interface
        public byte I2C_MEMORY_ADDRESS = 0x53; // 0b0101 0011 - I2C address of Microchip I2C memory IOPort expander
        public byte I2C_MEMORY_ADDRESS_READ = 0xA7;
        public byte I2C_MEMORY_ADDRESS_WRITE = 0xA6;

        // For I2C Memory 2 interface
        public byte I2C_MEMORY2_ADDRESS = 0x57; // 0b0101 0111 - I2C address of Microchip I2C memory IO Port expander
        public byte I2C_MEMORY2_ADDRESS_READ = 0xAF;
        public byte I2C_MEMORY2_ADDRESS_WRITE = 0xAE;

        Tricam tricam;

        #endregion
        public MemoryTest()
        {
            //tricam = Tricam.crea
        }
      


    }
}
