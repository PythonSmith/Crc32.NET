/* This code is based on the original SafeProxy
 * It however uses the Sse42 CRC32C cpu instructions to boost the performance by a factor of at least 5 times
 * 
 * Benedikt Schlager
 */

using System;
using System.Runtime.InteropServices;
#if NETCOREAPP3_0
using System.Runtime.Intrinsics.X86;
#endif

namespace Force.Crc32
{
#if NETCOREAPP3_0
    internal class SafeProxyHardwareAccelerated
    {
        private bool _force32Bit;

        internal SafeProxyHardwareAccelerated(bool force32Bit)
        {
            _force32Bit = force32Bit;
        }

        public uint Append(uint crc, Span<byte> input)
        {
            if (!Sse42.IsSupported)
            {
                throw new InvalidOperationException("Not supported, CPU needs to support SSE 4.2 64bit");

            }

            if (!_force32Bit && Sse42.X64.IsSupported)
            {
                return (uint)AppendSse42_64(crc, input);
            }
            else
            {
                return AppendSse42_32(crc, input);
            }
        }

        public uint Append(uint crc, byte[] input, int offset, int length) => Append(crc, input.AsSpan(offset, length));


        private ulong AppendSse42_64(ulong crc, Span<byte> input)
        {
            crc ^= ulong.MaxValue;

            // Calculate Crc32 for 64bit values
            var full64BitValues = MemoryMarshal.Cast<byte, ulong>(input);
            for (int i = 0; i < full64BitValues.Length; i++)
            {
                crc = Sse42.X64.Crc32(crc, full64BitValues[i]);
            }

            uint result = (uint)crc;

            int lastBytesOffset = full64BitValues.Length * 8;
            // Calculate the last bytes one by one
            for (int i = lastBytesOffset; i < input.Length; i++)
            {
                result = Sse42.Crc32(result, input[i]);
            }

            return (uint)(result ^ ulong.MaxValue);
        }

        private uint AppendSse42_32(uint crc, Span<byte> input)
        {
            crc ^= uint.MaxValue;

            // Calculate Crc32 for 32bit values
            var full32BitValues = MemoryMarshal.Cast<byte, uint>(input);
            for (int i = 0; i < full32BitValues.Length; i++)
            {
                crc = Sse42.Crc32(crc, full32BitValues[i]);
            }

            uint result = crc;

            int lastBytesOffset = full32BitValues.Length * 4;
            // Calculate the last bytes one by one
            for (int i = lastBytesOffset; i < input.Length; i++)
            {
                result = Sse42.Crc32(result, input[i]);
            }

            return result ^ uint.MaxValue;
        }
    }
#endif
}
