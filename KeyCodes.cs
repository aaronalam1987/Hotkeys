using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotkeys
{
    public class KeyCodes
    {
        private static readonly Dictionary<string, byte> keyCodes = new Dictionary<string, byte>()
        {
            {"0", 48 },
            {"1", 49 },
            {"2", 50 },
            {"3", 51 },
            {"4", 52 },
            {"5", 53 },
            {"6", 54 },
            {"7", 55 },
            {"8", 56 },
            {"9", 57 },
            {"a", 65 },
            {"b", 66 },
            {"c", 67 },
            {"d", 68 },
            {"e", 69 },
            {"f", 70 },
            {"g", 71 },
            {"h", 72 },
            {"i", 73 },
            {"j", 74 },
            {"k", 75 },
            {"l", 76 },
            {"m", 77 },
            {"n", 78 },
            {"o", 79 },
            {"p", 80 },
            {"q", 81 },
            {"r", 82 },
            {"s", 83 },
            {"t", 84 },
            {"u", 85 },
            {"v", 86 },
            {"w", 87 },
            {"x", 88 },
            {"y", 89 },
            {"z", 90 },
            {"enter", 13 },
            {"ctrl", 17 },
            {"space", 32 },
            {"escape", 27 },
            {"alt", 18 },
            {"f1", 112 },
            {"f2", 113 },
            {"f3", 114 },
            {"f4", 115 },
            {"f5", 116 },
            {"f6", 117 },
            {"f7", 118 },
            {"f8", 119 },
            {"f9", 120 },
            {"f10", 121 },
            {"f11", 122 },
            {"f12", 123 }

        };

        public static byte GetKeyCode(string KeyIn)
        {
            byte keyByte = 0;
            if (keyCodes.ContainsKey(KeyIn)) { keyByte = keyCodes[KeyIn]; }
             
            return keyByte;

        }
    }
}
