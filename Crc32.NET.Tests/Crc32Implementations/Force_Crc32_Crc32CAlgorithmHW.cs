using Force.Crc32;
using Force.Crc32.Tests.Crc32Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Crc32.Tests.Crc32Implementations
{
    class Force_Crc32_Crc32CAlgorithmHW : CrcCalculator
    {
        public Force_Crc32_Crc32CAlgorithmHW() : base("Force.Crc32.Crc32CAlgorithm_HW")
        {
        }

        public override uint Calculate(byte[] data)
        {
            return Crc32CAlgorithmHW.Compute(data);
        }
    }
}
