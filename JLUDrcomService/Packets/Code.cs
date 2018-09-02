using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLUDrcomService.Packets
{
    public enum Code
    {
        StartRequest = 0x01,
        StartResponse = 0x02,
        LoginAuth = 0x03,
        Success = 0x04,
        Failure = 0x05,
        LogoutAuth = 0x06,
        Misc = 0x07,
        Unknown = 0x08,
        NewPassword = 0x09,
        Message = 0x4d,
        Alive4Client = 0xfe,
        Alive = 0xff
    }
}
