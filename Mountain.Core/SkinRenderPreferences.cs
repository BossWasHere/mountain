namespace Mountain.Core
{
    public struct SkinRenderPreferences
    {
        public bool CapeEnabled { get; set; }
        public bool JacketEnabled { get; set; }
        public bool LeftSleeveEnabled { get; set; }
        public bool RightSleeveEnabled { get; set; }
        public bool LeftPantsEnabled { get; set; }
        public bool RightPantsEnabled { get; set; }
        public bool HatEnabled { get; set; } // Unused ?

        public SkinRenderPreferences(byte fromByte)
        {
            var b = fromByte;
            CapeEnabled = (b & 0x01) == 0x01;
            JacketEnabled = (b & 0x02) == 0x02;
            LeftSleeveEnabled = (b & 0x04) == 0x04;
            RightSleeveEnabled = (b & 0x08) == 0x08;
            LeftPantsEnabled = (b & 0x10) == 0x10;
            RightPantsEnabled = (b & 0x20) == 0x20;
            HatEnabled = (b & 0x40) == 0x40;
        }

        public byte[] ToByteArray()
        {
            byte b = 0;
            if (CapeEnabled) b |= 0x01;
            if (JacketEnabled) b |= 0x02;
            if (LeftSleeveEnabled) b |= 0x04;
            if (RightSleeveEnabled) b |= 0x08;
            if (LeftPantsEnabled) b |= 0x10;
            if (RightPantsEnabled) b |= 0x20;
            if (HatEnabled) b |= 0x40;

            return new byte[] { b };
        }
    }
}
