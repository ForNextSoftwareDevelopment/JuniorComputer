using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net;
using System.Reflection;
using System.Windows.Forms; 

namespace JuniorComputer
{
    class Assembler
    {
        #region Members

        // Operator types at arithmetic operations
        public enum OPERATOR
        {
            ADD =  1, // Add  
            SUB =  2, // Subtract  
            ADC =  3, // Add with carry
            SBC =  4, // Subtract with carry
            AND =  5, // Logical AND
            OR =   6, // Logical OR
            EOR =  7, // Logical Exclusive OR
            ASL =  8, // Arithmetic Shift Left  
            LSR =  9, // Logical Shift right  
            ROL = 10, // Rotate Left through carry
            ROR = 11  // Rotate Right through carry
        }

        // Segment types
        public enum SEGMENT
        {
            ASEG = 0,
            CSEG = 1,
            DSEG = 2
        }

        // Segment currently active
        SEGMENT segment = SEGMENT.ASEG;

        // Absolute program segment, Code program segment, Data program segment
        UInt16 ASEG = 0x0000;
        UInt16 CSEG = 0x0000;
        UInt16 DSEG = 0x0000;

        // 6502 Instructions 
        Instructions instructions = new Instructions();

        // Total RAM of 65536 bytes (0x0000 - 0xFFFF)
        public byte[] RAM = new byte[0x10000];

        // Linenumber for a given byte of program
        public int[] RAMprogramLine = new int[0x10000];

        // Address Symbol Table
        public Dictionary<string, int> addressSymbolTable = new Dictionary<string, int>();

        // Processed program for running second pass
        public string[] programRun;

        // Program listing
        public string[] programView;   
        
        // Current instruction to be processed
        private byte byteInstruction = 0x00;

        // Start location of the program
        public int startLocation;

        // Current location of the program (during firstpass and secondpass)
        public int locationCounter;

        // Register values
        public byte registerA = 0x00;
        public byte registerX = 0x00;
        public byte registerY = 0x00;

        public UInt16 registerPC = 0x0000;
        public byte registerSP = 0x00;

        // Flag values
        public bool flagN = false;
        public bool flagV = false;
        public bool flag1 = true;
        public bool flagB = false;
        public bool flagD = false;
        public bool flagI = false;
        public bool flagC = false;
        public bool flagZ  = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>        
        public Assembler(string[] program)
        {
            this.programRun = program;
            this.programView = new string[program.Length];

            startLocation = 0;
            registerSP = 0x0000;

            ClearRam();
        }

        #endregion

        #region Methods (Div)

        /// <summary>
        /// Calculate and adjust the flagN on screen
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="type"></param>
        private byte Calculate(byte arg1, byte arg2, OPERATOR type)
        {
            byte result = (byte)0x00;

            flag1 = true;

            switch (type)
            {
                case OPERATOR.ADD:
                    result = (byte)(arg1 + arg2);

                    // Carry flag
                    if (arg1 + arg2 > 0xFF)
                    {
                        flagC = true;
                    } else
                    {
                        flagC = false;
                    }

                    break;

                case OPERATOR.SUB:
                    result = (byte)(arg1 - arg2);

                    // Carry flag
                    if (arg1 - arg2 < 0x00)
                    {
                        flagC = true;
                    } else
                    {
                        flagC = false;
                    }

                    break;

                case OPERATOR.ADC:
                    result = (byte)(arg1 + arg2 + (flagC ? 1:0));

                    // Carry flag
                    if (arg1 + arg2 + (flagC ? 1 : 0) > 0xFF)
                    {
                        flagC = true;
                    } else 
                    {
                        flagC = false;
                    }

                    // Signed overflow flag
                    if ((arg1 >= 0x80) && (arg2 >= 0x80) && (result < 0x80)) flagV = true;
                    if ((arg1 >= 0x80) && (arg2 < 0x80)) flagV = false;
                    if ((arg1 < 0x80) && (arg2 >= 0x80)) flagV = false;
                    if ((arg1 < 0x80) && (arg2 < 0x80) && (result >= 0x80)) flagV = true;

                    break;

                case OPERATOR.SBC:
                    result = (byte)(arg1 - arg2 - (flagC ? 1 : 0));

                    // Carry flag
                    if (arg1 - arg2 - (flagC ? 1 : 0) < 0x00)
                    {
                        flagC = true;
                    } else
                    {
                        flagC = false;
                    }

                    // Signed overflow flag
                    if ((arg1 >= 0x80) && (arg2 >= 0x80)) flagV = false;
                    if ((arg1 >= 0x80) && (arg2 < 0x80) && (result < 0x80)) flagV = true;
                    if ((arg1 < 0x80) && (arg2 >= 0x80) && (result >= 0x80)) flagV = true;
                    if ((arg1 < 0x80) && (arg2 < 0x80)) flagV = false;

                    break;

                case OPERATOR.AND:
                    result = (byte)(arg1 & arg2);
                    break;

                case OPERATOR.OR:
                    result = (byte)(arg1 | arg2);
                    break;

                case OPERATOR.EOR:
                    result = (byte)(arg1 ^ arg2);
                    break;

                default:
                    throw new Exception("Unknown operator '" + Enum.GetName(typeof(OPERATOR), type) + "' in calculate");
            }

            string strResult = Convert.ToString(Convert.ToInt32(result.ToString("X2"), 16), 2).PadLeft(8, '0');

            // Negative flag
            if (strResult[0] == '1')
            {
                flagN = true;
            } else
            {
                flagN = false;
            }

            // Zero flag
            if (strResult == "00000000")
            {
                flagZ = true;
            } else
            {
                flagZ = false;
            }

            return (result);    
        }

        /// <summary>
        /// Rotate or Shift bitwise and adjust the flags on screen
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="type"></param>
        private byte RotateShift(byte arg, OPERATOR type)
        {
            bool lsb = false, msb = false;
            byte result = (byte)0x00;

            switch (type)
            {
                case OPERATOR.ROL:
                    msb = (arg & 0b10000000) == 0b10000000;
                    result = (byte)(arg << 1);
                    if (flagC) result = (byte)(result | 0b00000001);

                    // Carry flag
                    if (msb)
                    {
                        flagC = true;
                    } else
                    {
                        flagC = false;
                    }

                    break;

                case OPERATOR.ROR:
                    lsb = (arg & 0b00000001) == 0b00000001;
                    result = (byte)(arg >> 1);
                    if (flagC) result = (byte)(result | 0b10000000);

                    // Carry flag
                    if (lsb)
                    {
                        flagC = true;
                    } else
                    {
                        flagC = false;
                    }

                    break;

                case OPERATOR.ASL:
                    msb = (arg & 0b10000000) == 0b10000000;
                    result = (byte)(arg << 1);

                    // Carry flag
                    if (msb)
                    {
                        flagC = true;
                    } else
                    {
                        flagC = false;
                    }

                    break;

                case OPERATOR.LSR:
                    lsb = (arg & 0b00000001) == 0b00000001;
                    result = (byte)(arg >> 1);

                    // Carry flag
                    if (lsb)
                    {
                        flagC = true;
                    } else
                    {
                        flagC = false;
                    }

                    break;

                default:
                    throw new Exception("Unknown operator '" + Enum.GetName(typeof(OPERATOR), type) + "' in rotation/shift");
            }

            // Zero flag
            if (result == 0x00)
            {
                flagZ = true;
            } else
            {
                flagZ = false;
            }

            // Negative flag
            if (result >= 0x80)
            {
                flagN = true;
            } else
            {
                flagN = false;
            }

            return (result);
        }

        private byte GetByte(string arg, out string result)
        {
            /// Split arguments
            string[] args = arg.Split(new char[] { ' ', '(', ')', '+', '-', '*', '/' });

            // Sort by size, longest string first to avoid partial replacements
            Array.Sort(args, (x, y) => y.Length.CompareTo(x.Length));

            // Replace all symbols from symbol table
            foreach (string str in args)
            {
                foreach (KeyValuePair<string, int> keyValuePair in addressSymbolTable)
                {
                    if (str.ToUpper().Trim() == keyValuePair.Key.ToUpper().Trim())
                    {
                        arg = arg.Replace(keyValuePair.Key, keyValuePair.Value.ToString());
                    }
                }
            }

            // Process low order byte of argument
            if (arg.ToUpper().Contains("LOW("))
            {
                int start = arg.IndexOf('(') + 1;
                int end = arg.IndexOf(')', start);
            
                if (end - start < 2)
                {
                    result = "Illegal argument for LOW(arg)";
                    return (0);
                }

                string argLow = arg.Substring(start, end - start);
                argLow = Convert.ToInt32(argLow).ToString("X4").Substring(2, 2);

                arg = Convert.ToInt32(argLow, 16).ToString() + arg.Substring(end + 1, arg.Length - 1 - end).Trim();
            }

            // Process high order byte of argument
            if (arg.ToUpper().Contains("HIGH("))
            {
                int start = arg.IndexOf('(') + 1;
                int end = arg.IndexOf(')', start);

                if (end - start < 2)
                {
                    result = "Illegal argument for HIGH(arg)";
                    return (0);
                }

                string argHigh = arg.Substring(start, end - start);
                argHigh = Convert.ToInt32(argHigh).ToString("X4").Substring(0,2);

                arg = Convert.ToInt32(argHigh,16).ToString() + arg.Substring(end + 1, arg.Length - 1 - end).Trim();
            }

            // Replace AND with & as token
            arg = arg.Replace("AND", "&");

            // Replace OR with | as token
            arg = arg.Replace("OR", "|");

            // Calculate expression
            byte calc = Calculator.CalculateByte(arg, out string res);

            // result string of the expression ("OK" or error message)
            result = res;

            return(calc);
        }

        private UInt16 Get2Bytes(string arg, out string result)
        {
            // Replace $ with location counter
            arg = arg.Replace("$ ", locationCounter.ToString() + " ");
            arg = arg.Replace("$+", locationCounter.ToString() + "+");
            arg = arg.Replace("$-", locationCounter.ToString() + "-");

            /// Split arguments
            string[] args = arg.Split(new char[] { ' ', '(', ')', '+', '-', '*', '/' });

            // Sort by size, longest string first to avoid partial replacements
            Array.Sort(args, (x, y) => y.Length.CompareTo(x.Length));

            // Replace all symbols from symbol table
            foreach (string str in args)
            {
                foreach (KeyValuePair<string, int> keyValuePair in addressSymbolTable)
                {
                    if (str.ToUpper().Trim() == keyValuePair.Key.ToUpper().Trim())
                    {
                        arg = arg.Replace(keyValuePair.Key, keyValuePair.Value.ToString());
                    }
                }
            }

            // Replace AND with & as token
            arg = arg.Replace("AND", "&");

            // Replace OR with | as token
            arg = arg.Replace("OR", "|");

            // Calculate expression
            UInt16 calc = Calculator.Calculate2Bytes(arg, out string res);

            // result string of the expression ("OK" or error message)
            result = res;

            return (calc);
        }

        /// <summary>
        /// Convert integer to hexadecimal string representation
        /// </summary>
        /// <param name="n"></param>
        /// <param name="hi"></param>
        /// <param name="lo"></param>
        private void Get2ByteFromInt(int n, out string lo, out string hi)
        {
            string temp = n.ToString("X4");
            hi = temp.Substring(temp.Length - 4, 2);
            lo = temp.Substring(temp.Length - 2, 2);
        }

        /// <summary>
        /// Get a value according to the addressing method
        /// </summary>
        /// <param name="addressing"></param>
        /// <returns></returns>
        private byte GetValue(ADDRESSING addressing)
        {
            UInt16 address, pointer;
            byte val;

            registerPC++;
            switch (addressing)
            {
                case ADDRESSING.IMPLIED:
                    val = registerA;
                    break;
                case ADDRESSING.IMMEDIATE:
                    val = RAM[registerPC];
                    registerPC++;
                    break;
                case ADDRESSING.ZEROPAGE:
                    val = RAM[RAM[registerPC]];
                    registerPC++;
                    break;
                case ADDRESSING.ZEROPAGEX:
                    val = RAM[RAM[registerPC] + registerX];
                    registerPC++;
                    break;
                case ADDRESSING.ZEROPAGEY:
                    val = RAM[RAM[registerPC] + registerY];
                    registerPC++;
                    break;
                case ADDRESSING.ABSOLUTE:
                    val = RAM[RAM[registerPC] + RAM[registerPC +1] * 0x100];
                    registerPC++;
                    registerPC++;
                    break;
                case ADDRESSING.ABSOLUTEX:
                    val = RAM[RAM[registerPC] + RAM[registerPC + 1] * 0x100 + registerX];
                    registerPC++;
                    registerPC++;
                    break;
                case ADDRESSING.ABSOLUTEY:
                    val = RAM[RAM[registerPC] + RAM[registerPC + 1] * 0x100 + registerY];
                    registerPC++;
                    registerPC++;
                    break;
                case ADDRESSING.XINDIRECT:
                    pointer = (UInt16)(RAM[registerPC] + registerX);
                    address = (UInt16)(RAM[pointer]);
                    val = RAM[address];
                    registerPC++;
                    break;
                case ADDRESSING.INDIRECTY:
                    pointer = (UInt16)(RAM[registerPC]);
                    address = (UInt16)(RAM[pointer + registerY] + 0x100 * RAM[pointer + registerY + 1]);
                    val = RAM[address];
                    registerPC++;
                    break;
                default:
                    val = 0x00;
                    MessageBox.Show("Illegal Addressing in 'GetValue' method", "RUNINSTRUCTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    registerPC++;
                    break;
            }

            return val;
        }

        /// <summary>
        /// Set a value according to the addressing method
        /// </summary>
        /// <param name="addressing"></param>
        /// <param name="val"></param>
        private void SetValue(ADDRESSING addressing, byte val)
        {
            UInt16 address, pointer;

            registerPC++;
            switch (addressing)
            {
                case ADDRESSING.IMPLIED:
                    registerA = val;
                    break;
                case ADDRESSING.ZEROPAGE:
                    RAM[RAM[registerPC]] = val;
                    registerPC++;
                    break;
                case ADDRESSING.ZEROPAGEX:
                    RAM[RAM[registerPC] + registerX] = val;
                    registerPC++;
                    break;
                case ADDRESSING.ZEROPAGEY:
                    RAM[RAM[registerPC] + registerY] = val;
                    registerPC++;
                    break;
                case ADDRESSING.ABSOLUTE:
                    RAM[RAM[registerPC] + RAM[registerPC + 1] * 0x100] = val;
                    registerPC++;
                    registerPC++;
                    break;
                case ADDRESSING.ABSOLUTEX:
                    RAM[RAM[registerPC] + RAM[registerPC + 1] * 0x100 + registerX] = val;
                    registerPC++;
                    registerPC++;
                    break;
                case ADDRESSING.ABSOLUTEY:
                    RAM[RAM[registerPC] + RAM[registerPC + 1] * 0x100 + registerY] = val;
                    registerPC++;
                    registerPC++;
                    break;
                case ADDRESSING.XINDIRECT:
                    pointer = (UInt16)(RAM[registerPC] + registerX);
                    address = (UInt16)(RAM[pointer]);
                    RAM[address] = val;
                    registerPC++;
                    break;
                case ADDRESSING.INDIRECTY:
                    pointer = (UInt16)(RAM[registerPC]);
                    address = (UInt16)(RAM[pointer + registerY] + 0x100 * RAM[pointer + registerY + 1]);
                    RAM[address] = val;
                    registerPC++;
                    break;
                default:
                    MessageBox.Show("Illegal Addressing in 'SetValue' method", "RUNINSTRUCTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    registerPC++;
                    break;
            }
        }

        /// <summary>
        /// Clear the RAM
        /// </summary>
        public void ClearRam()
        {
            for (int i = 0; i < RAM.Length; i++)
            {
                RAM[i] = 0x00;
            }

            for (int i = 0; i < RAMprogramLine.Length; i++)
            {
                RAMprogramLine[i] = -1;
            }
        }

        #endregion

        #region Methods (FindInstruction)

        /// <summary>
        /// Find the instruction(s) that match the opcode + operands in the program line 
        /// </summary>
        /// <param name="instructions"></param>
        /// <param name="opcode"></param>
        /// <param name="operands"></param>
        /// <param name="matchOpcode"></param>
        /// <returns></returns>
        private List<Instruction> FindInstruction(Instruction[] instructions, string opcode, string[] operands, out bool matchOpcode)
        {
            matchOpcode = false;
            string result;

            List<Instruction> found = new List<Instruction>();

            if (opcode == "-") return (found);

            for (int indexInstructions = 0; indexInstructions < instructions.Length; indexInstructions++)
            {
                string[] splitInstruction = instructions[indexInstructions].Mnemonic.Split(' ');
                string opcodeInstruction = splitInstruction[0];

                string temp = "";
                for (int i = 1; i < splitInstruction.Length; i++)
                {
                    temp += splitInstruction[i];
                }
                string[] argumentsInstruction = new string[0];
                if (temp != "") argumentsInstruction = temp.Split(',');

                // Opcode matches
                if (opcode == opcodeInstruction)
                {
                    matchOpcode = true;
                    bool matchOperands = true;

                    // Check number of operands
                    if (operands.Length == argumentsInstruction.Length)
                    {
                        // Check operands
                        for (int indexOperands = 0; indexOperands < operands.Length; indexOperands++)
                        {
                            string arg = argumentsInstruction[indexOperands];
                            string opr = operands[indexOperands].ToUpper().Trim();
                            int value;

                            switch (arg)
                            {
                                case "A":
                                    if (opr.ToUpper() != "A") matchOperands = false;
                                    break;
                                case "#n":
                                    if (!opr.StartsWith("#")) matchOperands = false;
                                    break;
                                case "n":
                                    if ((opr.StartsWith("(") || opr.StartsWith("#")))
                                    {
                                        matchOperands = false;
                                    } else if ((opcode != "BNE") && 
                                               (opcode != "BEQ") &&
                                               (opcode != "BPL") &&
                                               (opcode != "BMI") &&
                                               (opcode != "BVC") && 
                                               (opcode != "BVS") && 
                                               (opcode != "BCC") && 
                                               (opcode != "BCS"))
                                    {
                                        GetByte(opr, out result);
                                        if (result != "OK") matchOperands = false;
                                    }
                                    break;
                                case "(n)":
                                    if (!opr.StartsWith("("))
                                    {
                                        matchOperands = false;
                                    } else
                                    {
                                        GetByte(opr, out result);
                                        if (result != "OK") matchOperands = false;
                                    }
                                    break;
                                case "(nn)":
                                    if (!opr.StartsWith("("))
                                    {
                                        matchOperands = false;
                                    }
                                    value = Get2Bytes(opr, out result);
                                    break;
                                case "(n":
                                    if (!opr.StartsWith("("))
                                    {
                                        matchOperands = false;
                                    } else
                                    {
                                        GetByte(opr.TrimStart('('), out result);
                                        if (result != "OK") matchOperands = false;
                                    }
                                    break;
                                case "X)":
                                    if (opr.ToUpper() != "X)") matchOperands = false;
                                    break;
                                case "X":
                                    if (opr.ToUpper() != "X") matchOperands = false;
                                    break;
                                case "Y":
                                    if (opr.ToUpper() != "Y") matchOperands = false;
                                    break;
                                case "nn":
                                    value = Get2Bytes(opr, out result);
                                    if ((result == "OK") && (value <= 0xFF)) matchOperands = false;
                                    if ((opcode == "STA") && (operands.Length == 2) && (operands[1] == "Y") && (result == "OK") && (value <= 0xFF)) matchOperands = true;
                                    if ((opcode == "LDX") && (operands.Length == 2) && (result == "OK") && (value <= 0xFF)) matchOperands = true;
                                    if (opr.StartsWith("(") || opr.StartsWith("#") || (opr == "A")) matchOperands = false;
                                    break;
                            }
                        }
                    } else
                    {
                        matchOperands = false;
                    }

                    if (matchOperands)
                    {
                        found.Add(instructions[indexInstructions]);
                    }
                }
            }

            return (found);
        }

        #endregion

        #region Methods (FirstPass)

        /// <summary>
        /// First pass through the code, remove labels, check etc.
        /// </summary>
        /// <returns></returns>
        public string FirstPass()
        {
            // StartLocation denotes the first RAM location to which we are assembling the program
            // locationCounter is a temporary variable to traverse program for first pass
            locationCounter = startLocation;

            // Opcode in the line
            string opcode;

            // Operand(s) for the opcode 
            string[] operands;

            char[] delimiters = new[] { ',' };

            // Process all lines
            for (int lineNumber = 0; lineNumber < programRun.Length; lineNumber++)
            {
                // Copy line of code to process and clear original line to rebuild
                string line = programRun[lineNumber];
                programRun[lineNumber] = "";
                programView[lineNumber] = "";

                opcode = null;
                operands = null;
                int InstructionStart = locationCounter;

                try
                {
                    // Replace all tabs with spaces
                    line = line.Replace('\t', ' ');

                    // Remove leading or trailing spaces
                    line = line.Trim();
                    if (line == "") continue;

                    // if a comment is found, remove
                    int start_of_search_pos = 0;
                    int start_of_comment_pos = line.IndexOf(';', start_of_search_pos);
                    while (start_of_comment_pos != -1)
                    {
                        // Check if really a comment (; could be in a string or char array)
                        int num_quotes = 0;
                        for (int i = 0; i < start_of_comment_pos; i++)
                        {
                            if ((line[i] == '\'') || (line[i] == '\"')) num_quotes++;
                        }

                        if ((num_quotes % 2) == 0)
                        {
                            line = line.Remove(line.IndexOf(';', start_of_search_pos)).Trim();
                        }

                        start_of_search_pos = start_of_comment_pos + 1;

                        if (start_of_search_pos < line.Length)
                        {
                            start_of_comment_pos = line.IndexOf(';', start_of_search_pos);
                        } else
                        {
                            start_of_comment_pos = -1;
                        }
                    }

                    // Replace single characters (in between single quotes) with HEX value
                    bool found;
                    do
                    {
                        found = false;
                        int startQuote = line.IndexOf('\'');
                        int endQuote = 0;
                        if (startQuote < line.Length - 2) endQuote = line.IndexOf('\'', startQuote + 1);
                        if ((startQuote != -1) && (endQuote == startQuote + 2))
                        {
                            found = true;
                            char ch = line[startQuote + 1];
                            line = line.Replace("'" + ch + "'", ((int)ch).ToString("X2") + "H");
                        }
                    } while (found);
                } catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "FirstPass:Quotes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                }

                // Check for equ directive 
                int equ_pos = line.ToUpper().IndexOf(".EQU");
                if (equ_pos > 0)
                {
                    string label = line.Substring(0, equ_pos - 1).Trim().TrimEnd(':');

                    if (addressSymbolTable.ContainsKey(label))
                    {
                        return ("Label already used at line " + (lineNumber + 1));
                    }

                    if (equ_pos < line.Length - 5)
                    {
                        string val = line.Substring(equ_pos + 4).Trim();

                        // Replace $ with location counter
                        if (val.Trim() == "$") val = val.Replace("$", locationCounter.ToString());

                        int calc = Get2Bytes(val, out string result);
                        if (result != "OK")
                        {
                            return ("Invalid operand for .EQU (" + result + ") at line " + (lineNumber + 1));
                        }

                        // ADD the label/value
                        if (label != "")
                        {
                            addressSymbolTable.Add(label, calc);
                        } else
                        {
                            return ("Syntax: [LABEL] .EQU [VALUE] at line " + (lineNumber + 1));
                        }

                        // Next line
                        continue;
                    } else
                    {
                        return ("Syntax: [LABEL] .EQU [VALUE] at line " + (lineNumber + 1));
                    }
                }

                // Check for/get a label
                if (line.IndexOf(':') != -1)
                {
                    try
                    {
                        int end_of_label_pos = line.IndexOf(':');

                        // Check if really a LABEL (: could be in a string or char array)
                        int num_quotes = 0;
                        for (int i = 0; i < end_of_label_pos; i++)
                        {
                            if ((line[i] == '\'') || (line[i] == '\"')) num_quotes++;
                        }

                        if ((num_quotes % 2) == 0)
                        {
                            string label = line.Substring(0, end_of_label_pos).Trim();

                            // Check for empty labels
                            if ((label == null) || (label == ""))
                            {
                                return ("Empty label at line " + (lineNumber + 1));
                            }

                            // Check for spaces in label
                            if (label.Contains(" "))
                            {
                                return ("label '" + label + "' contains spaces at line " + (lineNumber + 1));
                            }

                            if (addressSymbolTable.ContainsKey(label))
                            {
                                return ("Label already used at line " + (lineNumber + 1));
                            }

                            if (line.Length > end_of_label_pos + 1)
                            {
                                line = line.Substring(end_of_label_pos + 1, line.Length - end_of_label_pos - 1).Trim();

                                // ADD the label/value
                                addressSymbolTable.Add(label, locationCounter);
                            } else
                            {
                                line = "";

                                // ADD the label/value
                                addressSymbolTable.Add(label, locationCounter);

                                // Next line
                                continue;
                            }
                        }
                    } catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "FirstPass:Label", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                    }
                }

                try
                {
                    // Check for opcode (directive) DB, replace chars/strings with hex values
                    if (line.ToUpper().StartsWith(".DB"))
                    {
                        string lineDB = line;

                        // Create new line, copy opcode
                        opcode = lineDB.Substring(0, line.IndexOf(" "));
                        line = opcode;
                        for (int i = line.Length; i < 6; i++)
                        {
                            line += " ";
                        }

                        // Traverse the line from left to right
                        int index = opcode.Length;
                        bool stringProcessing = false;
                        while (index < lineDB.Length)
                        {
                            if ((lineDB[index] == '\"') || (lineDB[index] == '\''))
                            {
                                // Char or string found
                                stringProcessing = true;
                                char endChar = lineDB[index];

                                if (index < lineDB.Length - 1) index++;
                                char processChar = lineDB[index];

                                // Replace until end of string found
                                while ((index < lineDB.Length) && (processChar != endChar))
                                {
                                    processChar = lineDB[index];
                                    if ((processChar != endChar))
                                    {
                                        line += ((int)processChar).ToString("X2") + "H";
                                        line += ", ";
                                    }
                                    index++;
                                }

                                stringProcessing = false;
                            } else if ((lineDB[index] == ',') || (lineDB[index] == ' ') || (lineDB[index] == '\t') || (lineDB[index] == '\r') || (lineDB[index] == '\n'))
                            {
                                // Skip these chars
                                index++;
                            } else
                            {
                                // Just copy up to next comma or end of line
                                while ((index < lineDB.Length) && (lineDB[index] != ','))
                                {
                                    line += lineDB[index];
                                    index++;
                                }

                                line += ", ";
                            }
                        }

                        // Remove last comma and space
                        if (line.Length > 2)
                        {
                            line = line.Substring(0, line.Length - 2);
                        } else
                        {
                            return (opcode + " directive has an error at line " + (lineNumber + 1));
                        }

                        // Give warning if an unclosed string has been found
                        if (stringProcessing)
                        {
                            return (opcode + " directive has an unclosed string at line " + (lineNumber + 1));
                        }
                    }
                } catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "FirstPass:DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                }

                // Get the opcode and operands
                try
                {
                    int end_of_opcode_pos = line.IndexOf(' ');
                    if ((end_of_opcode_pos == -1) && (line.Length != 0)) end_of_opcode_pos = line.Length;

                    if (end_of_opcode_pos <= 0)
                    {
                        // Next line
                        continue;
                    }

                    opcode = line.Substring(0, end_of_opcode_pos).ToUpper().Trim();

                    // Split the line and store the strings formed in array
                    operands = line.Substring(end_of_opcode_pos).Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    // Rebuild the line
                    line = opcode;
                    while (line.Length < 6) line += " ";
                    for (int i = 0; i < operands.Length; i++)
                    {
                        operands[i] = operands[i].Trim();
                        if (i != 0) line += ",";
                        line += operands[i];
                    }
                } catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "FirstPass:Opcode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                }

                try
                {
                    // Check for opcode (directive) aseg
                    if (opcode.Equals(".ASEG"))
                    {
                        // Set current segment
                        segment = SEGMENT.ASEG;

                        // Set locationcounter
                        locationCounter = ASEG;

                        // Copy to program for second pass
                        programRun[lineNumber] = opcode;

                        // Copy to programView for examining
                        programView[lineNumber] = opcode;

                        // Next line
                        continue;
                    }
                } catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "FirstPass:ASEG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                }

                try
                {
                    // Check for opcode (directive) cseg
                    if (opcode.Equals(".CSEG"))
                    {
                        // Set current segment
                        segment = SEGMENT.CSEG;

                        // Set locationcounter
                        locationCounter = CSEG;

                        // Copy to program for second pass
                        programRun[lineNumber] = opcode;

                        // Copy to programView for examining
                        programView[lineNumber] = opcode;

                        // Next line
                        continue;
                    }
                } catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "FirstPass:CSEG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                }

                try
                {
                    // Check for opcode (directive) DSEG
                    if (opcode.Equals(".DSEG"))
                    {
                        // Set current segment
                        segment = SEGMENT.DSEG;

                        // Set locationcounter
                        locationCounter = DSEG;

                        // Copy to program for second pass
                        programRun[lineNumber] = opcode;

                        // Copy to programView for examining
                        programView[lineNumber] = opcode;

                        // Next line
                        continue;
                    }
                } catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "FirstPass:DSEG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                }

                try
                {
                    // Check for opcode (directive) .ORG
                    if (opcode.Equals(".ORG"))
                    {
                        // Line must have an argument after the opcode
                        if (operands.Length == 0)
                        {
                            return (".ORG directive must have an argument following at line " + (lineNumber + 1));
                        }

                        // If valid address then store in locationCounter
                        int calc = Get2Bytes(operands[0], out string result);
                        if (result == "OK")
                        {
                            locationCounter = calc;
                        } else
                        {
                            return ("Invalid operand for " + opcode + "(" + result + ") at line " + (lineNumber + 1));
                        }

                        // Copy to program for second pass
                        programRun[lineNumber] = opcode + " " + operands[0];

                        // Copy to programView for examining
                        programView[lineNumber] = opcode + " " + operands[0];

                        // Next line
                        continue;
                    }
                } catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "FirstPass:ORG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                }

                // Count the operand(s) for DB, 
                try
                {
                    if (opcode.Equals(".DB"))
                    {
                        if (operands.Length == 0)
                        {
                            return (opcode + " directive has too few operands at line " + (lineNumber + 1));
                        }

                        // Loop for traversing after DB
                        for (int pos = 0; pos < operands.Length; pos++)
                        {
                            // get to next location by skipping location for byte
                            locationCounter++;
                        }

                        // Copy to program for second pass
                        programRun[lineNumber] = line;

                        // Copy to programView for examining
                        programView[lineNumber] = InstructionStart.ToString("X4") + ": " + line;

                        // Next line
                        continue;
                    }

                    if (opcode.Equals(".DW"))
                    {
                        if (operands.Length == 0)
                        {
                            return ("DW directive has too few operands at line " + (lineNumber + 1));
                        }

                        for (int pos = 0; pos < operands.Length; pos++)
                        {
                            // Get to next location by skipping location for 2 bytes
                            locationCounter += 2;
                        }

                        // Copy to program for second pass
                        programRun[lineNumber] = line;

                        // Copy to programView for examining
                        programView[lineNumber] = InstructionStart.ToString("X4") + ": " + line;

                        // Next line
                        continue;
                    }

                    if (opcode.Equals(".DS"))
                    {
                        if (operands.Length == 0)
                        {
                            return ("DS directive has too few operands at line " + (lineNumber + 1));
                        }

                        // If valid address then store in locationCounter
                        int calc = Get2Bytes(operands[0], out string result);
                        if (result == "OK")
                        {
                            locationCounter += calc;
                        } else
                        {
                            return ("Invalid operand for " + opcode + "(" + result + ") at line " + (lineNumber + 1));
                        }

                        // Copy to program for second pass
                        programRun[lineNumber] = line;

                        // Copy to programView for examining
                        programView[lineNumber] = InstructionStart.ToString("X4") + ": " + line;

                        // Next line
                        continue;
                    }

                    // End of program
                    if (opcode == ".END")
                    {
                        programRun[lineNumber] = line;
                        programView[lineNumber] = line;
                        return "OK";
                    }

                    // List of matching instructions
                    List<Instruction> found;

                    // Check opcode/operands for 6502 instructions (Main)
                    bool matchOpcodeMain = false;
                    found = FindInstruction(instructions.MainInstructions, opcode, operands, out matchOpcodeMain);

                    string args = "";
                    foreach (string arg in operands)
                    {
                        args += arg + ", ";
                    }
                    args = args.Trim();
                    args = args.TrimEnd(',');

                    if (found.Count > 1)
                    {
                        // Just for debugging, should not happen
                        string message = "Multiple solutions for '" + opcode + " " + args + "':\r\n\r\n";
                        foreach (Instruction instruction in found)
                        {
                            message += instruction.Mnemonic + "\r\n";
                        }
                        message += "\r\nAt line " + (lineNumber + 1);

                        return (message);
                    }

                    // No match
                    if (found.Count == 0)
                    {
                        if (matchOpcodeMain)
                        {
                            return ("Error in arguments '" + args + "' at line " + (lineNumber + 1));
                        } else
                        {
                            return ("Unknown opcode '" + opcode + "' at line " + (lineNumber + 1));
                        }
                    }

                    // Update locationcounter
                    locationCounter += found[0].Size;
                } catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "FirstPass:OPCODE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                }

                // Update current segment
                if (segment == SEGMENT.ASEG) ASEG = (UInt16)locationCounter;
                if (segment == SEGMENT.CSEG) CSEG = (UInt16)locationCounter;
                if (segment == SEGMENT.DSEG) DSEG = (UInt16)locationCounter;

                //  Copy the edited program (without labels and equ) to new array of strings
                //  The new program array of strings will be used in second pass
                programRun[lineNumber] = line;

                // Copy to programView for examining
                programView[lineNumber] = InstructionStart.ToString("X4") + ": " + line;
            }

            return ("OK");
        }

        #endregion

        #region Methods (SecondPass)

        /// <summary>
        /// Second pass through the code, convert instructions etc.
        /// </summary>
        /// <returns></returns>
        public string SecondPass()
        {
            // StartLocation gives the location from which we have to start assembling
            // Using locationCounter to traverse the location of RAM during second pass
            locationCounter = startLocation; 

            // Opcode in the line
            string opcode;

            // Operand(s) for the opcode 
            string[] operands;

            // Split operands by these delimeter(s)
            char[] delimiters = new[] {','};

            // Reset segments
            ASEG = 0;
            CSEG = 0;
            DSEG = 0;

            // Temporary variables
            byte calcByte;
            UInt16 calcShort;
            int k;
            string str;

            for (int lineNumber = 0; lineNumber < programRun.Length; lineNumber ++)
            {
                int locationCounterInstructionStart = locationCounter;

                try
                {
                    // Empty line
                    if ((programRun[lineNumber] == null) || (programRun[lineNumber] == ""))
                    {
                        // If line is empty, there is no need to check
                        continue;
                    }

                    int end_of_opcode_pos = programRun[lineNumber].IndexOf(' ');
                    if ((end_of_opcode_pos == -1) && (programRun[lineNumber].Length != 0)) end_of_opcode_pos = programRun[lineNumber].Length;

                    if (end_of_opcode_pos <= 0)
                    {
                        // Next line
                        continue;
                    }

                    opcode = programRun[lineNumber].Substring(0, end_of_opcode_pos).Trim();

                    // Split the line and store the strings formed in array
                    operands = programRun[lineNumber].Substring(end_of_opcode_pos).Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    // Remove spaces and tabs from operands
                    for (int j = 0; j < operands.Length; j++)
                    {
                        operands[j] = operands[j].Trim();
                    }

                    // Check instruction
                    switch (opcode)
                    {
                        case ".ASEG":                                                                                   // .ASEG
                            segment = SEGMENT.ASEG;
                            locationCounter = ASEG;
                            continue;
                        case ".CSEG":                                                                                   // .CSEG
                            segment = SEGMENT.CSEG;
                            locationCounter = CSEG;
                            continue;
                        case ".DSEG":                                                                                   // .DSEG
                            segment = SEGMENT.DSEG;
                            locationCounter = DSEG;
                            continue;
                        // Set new location counter if .ORG
                        case ".ORG":
                            if (operands.Length == 0)
                            {
                                // Must have an operand
                                return ("Missing operand for " + opcode + " at line " + (lineNumber + 1));
                            } else
                            {
                                // If valid address then store in locationCounter
                                calcShort = Get2Bytes(operands[0], out string resultOrg);
                                if (resultOrg == "OK")
                                {
                                    locationCounter = calcShort;
                                } else
                                {
                                    return ("Invalid operand for " + opcode + ": " + resultOrg + " at line " + (lineNumber + 1));
                                }
                            }
                            continue;
                        case ".END":                                                                                   // .END
                            return ("OK");
                        case ".DB":                                                                                    // .DB
                            for (k = 0; k < operands.Length; k++)
                            {
                                // Extract all DB operands
                                calcByte = GetByte(operands[k], out string resultDB);
                                if (resultDB == "OK")
                                {
                                    RAMprogramLine[locationCounter] = lineNumber;
                                    RAM[locationCounter++] = calcByte;
                                } else
                                {
                                    return ("Invalid operand for " + opcode + ": " + resultDB + " at line " + (lineNumber + 1));
                                }
                            }
                            continue;
                        case ".DW":                                                                                    // .DW
                            for (k = 0; k < operands.Length; k++)
                            {
                                // Extract all DW operands
                                calcShort = Get2Bytes(operands[k], out string resultDW);
                                if (resultDW == "OK")
                                {
                                    str = calcShort.ToString("X4");
                                    RAMprogramLine[locationCounter] = lineNumber;
                                    RAM[locationCounter++] = Convert.ToByte(str.Substring(2, 2), 16);
                                    RAMprogramLine[locationCounter] = lineNumber;
                                    RAM[locationCounter++] = Convert.ToByte(str.Substring(0, 2), 16);
                                } else
                                {
                                    return ("Invalid operand for " + opcode + ": " + resultDW + " at line " + (lineNumber + 1));
                                }
                            }
                            continue;
                        case ".DS":                                                                                     // .DS
                            calcShort = Get2Bytes(operands[0], out string resultDS);
                            if (resultDS == "OK")
                            {
                                while (calcShort != 0)
                                {
                                    // We don't have to initialize operands for DS, just reserve space for them
                                    RAMprogramLine[locationCounter] = lineNumber;
                                    locationCounter++;
                                    calcShort--;
                                }
                            } else
                            {
                                return ("Invalid operand for " + opcode + ": " + resultDS + " at line " + (lineNumber + 1));
                            }
                            continue;
                    }

                    // Check if already code at this location 
                    if (RAMprogramLine[locationCounter] != -1)
                    {
                        return ("Already code at 0x" + locationCounter.ToString("X4") + " (from line " + (RAMprogramLine[locationCounter] +1).ToString() + ") for " + opcode + " at line " + (lineNumber + 1));
                    }
                } catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "SECONDPASS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                }

                // List of matching instructions
                List<Instruction> found;

                // Check opcode/operands for 6502 instructions (Main)
                bool matchOpcodeMain = false;
                found = FindInstruction(instructions.MainInstructions, opcode, operands, out matchOpcodeMain);

                string args = "";
                foreach (string arg in operands)
                {
                    args += arg + ", ";
                }
                args = args.Trim();
                args = args.TrimEnd(',');

                if (found.Count > 1)
                {
                    // Just for debugging, should not happen
                    string message = "Multiple solutions for '" + opcode + " " + args + "':\r\n\r\n";
                    foreach (Instruction instruction in found)
                    {
                        message += instruction.Mnemonic + "\r\n";
                    }
                    message += "\r\nAt line " + (lineNumber + 1);

                    return (message);
                }

                // No match
                if (found.Count == 0)
                {
                    if (matchOpcodeMain)
                    {
                        return ("Error in arguments '" + args + "' at line " + (lineNumber + 1));
                    } else
                    {
                        return ("Unknown opcode '" + opcode + "' at line " + (lineNumber + 1));
                    }
                }

                try
                {
                    // Just one instruction found that matched
                    Instruction instruction = found[0]; 

                    RAMprogramLine[locationCounter] = lineNumber;
                    RAM[locationCounter++] = (byte)instruction.Opcode;

                    // Process arguments
                    string result;
                    switch (instruction.Addressing)
                    {
                        case ADDRESSING.IMPLIED:
                            break;
                        case ADDRESSING.IMMEDIATE:
                            calcShort = GetByte(operands[0].TrimStart('#'), out result);
                            if (result == "OK") RAM[locationCounter++] = (byte)calcShort; else return ("Error in arguments '" + args + "':\r\n" + result + "\r\nAt line " + (lineNumber + 1));
                            break;
                        case ADDRESSING.RELATIVE:
                            if (addressSymbolTable.ContainsKey(operands[0]))
                            {
                                calcShort = Get2Bytes(operands[0], out result);
                                if (result == "OK")
                                {
                                    int offset = calcShort - locationCounter - 1;
                                    if (offset > 127) return ("Offset to large for " + opcode + ":\r\nOffset = " + offset + " (max 127)\r\nAt line " + (lineNumber + 1));
                                    if (offset < -128) return ("Offset to small for " + opcode + ":\r\nOffset = " + offset + " (min -128)\r\nAt line " + (lineNumber + 1));
                                    RAMprogramLine[locationCounter] = lineNumber;
                                    RAM[locationCounter++] = (byte)(offset);
                                } else
                                {
                                    return ("Error in arguments '" + args + "':\r\n" + result + "\r\nAt line " + (lineNumber + 1));
                                }
                            } else
                            {
                                int offset = GetByte(operands[0], out result);
                                if (result == "OK")
                                {
                                    RAMprogramLine[locationCounter] = lineNumber;
                                    RAM[locationCounter++] = (byte)(offset);
                                } else
                                {
                                    return ("Error in arguments '" + args + "':\r\n" + result + "\r\nAt line " + (lineNumber + 1));
                                }
                            }
                            break;
                        case ADDRESSING.ZEROPAGE:
                        case ADDRESSING.ZEROPAGEX:
                        case ADDRESSING.ZEROPAGEY:
                            calcShort = GetByte(operands[0], out result);
                            if (result == "OK") RAM[locationCounter++] = (byte)calcShort; else return ("Error in arguments '" + args + "':\r\n" + result + "\r\nAt line " + (lineNumber + 1));
                            break;
                        case ADDRESSING.INDIRECTY:
                            calcShort = GetByte(operands[0], out result);
                            if (result == "OK") RAM[locationCounter++] = (byte)calcShort; else return ("Error in arguments '" + args + "':\r\n" + result + "\r\nAt line " + (lineNumber + 1));
                            break;
                        case ADDRESSING.ABSOLUTE:
                        case ADDRESSING.ABSOLUTEX:
                        case ADDRESSING.ABSOLUTEY:
                        case ADDRESSING.INDIRECT:
                            calcShort = Get2Bytes(operands[0], out result);
                            if (result == "OK")
                            {
                                RAM[locationCounter++] = (byte)calcShort;
                                RAM[locationCounter++] = (byte)(calcShort >> 8);
                            } else
                            {
                                return ("Error in arguments '" + args + "':\r\n" + result + "\r\nAt line " + (lineNumber + 1));
                            }
                            break;
                        case ADDRESSING.XINDIRECT:
                            calcShort = GetByte(operands[0].TrimStart('('), out result);
                            if (result == "OK") RAM[locationCounter++] = (byte)calcShort; else return ("Error in arguments '" + args + "':\r\n" + result + "\r\nAt line " + (lineNumber + 1));
                            break;
                        default:
                            return ("Instruction argument selection error at line " + (lineNumber + 1));
                    }
 
                    while (programView[lineNumber].Length < 46)
                    {
                        programView[lineNumber] += " ";
                    }

                    for (int i = locationCounterInstructionStart; i < locationCounter; i++)
                    {
                        programView[lineNumber] += " " + RAM[i].ToString("X2");
                    }
                } catch (Exception exception)
                {
                    if (locationCounter > 0xFFFF)
                    {
                        return ("MEMORY OVERRUN AT LINE " + (lineNumber + 1));
                    }

                    MessageBox.Show(exception.Message, "SECONDPASS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("EXCEPTION ERROR AT LINE " + (lineNumber + 1));
                }
            }

            return ("OK");
        }

        #endregion

        #region Methods (RunInstruction)

        /// <summary>
        /// Run program from memory address
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="nextAddress"></param>
        /// <returns></returns>
        public string RunInstruction(UInt16 startAddress, ref UInt16 nextAddress)
        { 
            byte val;
            registerPC = startAddress;
            string lo, hi;

            byteInstruction = RAM[registerPC];
            Instruction instruction = instructions.MainInstructions[0];
            for (int i=0; i<instructions.MainInstructions.Length; i++)
            {
                if (instructions.MainInstructions[i].Opcode == byteInstruction)
                {
                    instruction = instructions.MainInstructions[i];
                }
            }

            // Check instruction mnemonic (could be '-')
            if (instruction.Mnemonic.Length < 3)
            {
                return ("No valid instruction at address: 0x" + startAddress.ToString("X4"));
            }

            string opcode = instruction.Mnemonic.Substring(0, 3);

            try
            {
                if (opcode == "ADC")                                                                                       // ADC
                {
                    val = GetValue(instruction.Addressing);
                    registerA = Calculate(registerA, val, OPERATOR.ADC);
                } else if (opcode == "AND")                                                                                // AND
                {
                    val = GetValue(instruction.Addressing);
                    registerA = Calculate(registerA, val, OPERATOR.AND);
                } else if (opcode == "ASL")                                                                                // ASL
                {
                    val = GetValue(instruction.Addressing);
                    val = RotateShift(val, OPERATOR.ASL);
                    registerPC = startAddress;
                    SetValue(instruction.Addressing, val);
                } else if (opcode == "BCC")                                                                                // BCC
                {
                    if (!flagC)
                    {
                        registerPC++;
                        byte offset = RAM[registerPC];
                        registerPC++;
                        UInt16 address = registerPC;
                        if (offset < 0x80) address += offset;
                        if (offset >= 0x80) address -= (UInt16)(0x100 - offset);
                        registerPC = address;
                    } else
                    {
                        registerPC++;
                        registerPC++;
                    }
                } else if (opcode == "BCS")                                                                                // BCS
                {
                    if (flagC)
                    {
                        registerPC++;
                        byte offset = RAM[registerPC];
                        registerPC++;
                        UInt16 address = registerPC;
                        if (offset < 0x80) address += offset;
                        if (offset >= 0x80) address -= (UInt16)(0x100 - offset);
                        registerPC = address;
                    } else
                    {
                        registerPC++;
                        registerPC++;
                    }
                } else if (opcode == "BEQ")                                                                                // BEQ
                {
                    if (flagZ)
                    {
                        registerPC++;
                        byte offset = RAM[registerPC];
                        registerPC++;
                        UInt16 address = registerPC;
                        if (offset < 0x80) address += offset;
                        if (offset >= 0x80) address -= (UInt16)(0x100 - offset);
                        registerPC = address;
                    } else
                    {
                        registerPC++;
                        registerPC++;
                    }
                } else if (opcode == "BIT")                                                                                // BIT
                {
                    val = GetValue(instruction.Addressing);
                    flagN = (val & 0b10000000) == 0b10000000;
                    flagV = (val & 0b01000000) == 0b01000000;
                    flagZ = (val & registerA)  == 0b00000000;
                } else if (opcode == "BMI")                                                                                // BMI
                {
                    if (flagN)
                    {
                        registerPC++;
                        byte offset = RAM[registerPC];
                        registerPC++;
                        UInt16 address = registerPC;
                        if (offset < 0x80) address += offset;
                        if (offset >= 0x80) address -= (UInt16)(0x100 - offset);
                        registerPC = address;
                    } else
                    {
                        registerPC++;
                        registerPC++;
                    }
                } else if (opcode == "BNE")                                                                                // BNE
                {
                    if (!flagZ)
                    {
                        registerPC++;
                        byte offset = RAM[registerPC];
                        registerPC++;
                        UInt16 address = registerPC;
                        if (offset < 0x80) address += offset;
                        if (offset >= 0x80) address -= (UInt16)(0x100 - offset);
                        registerPC = address;
                    } else
                    {
                        registerPC++;
                        registerPC++;
                    }
                } else if (opcode == "BPL")                                                                                // BPL
                {
                    if (!flagN)
                    {
                        registerPC++;
                        byte offset = RAM[registerPC];
                        registerPC++;
                        UInt16 address = registerPC;
                        if (offset < 0x80) address += offset;
                        if (offset >= 0x80) address -= (UInt16)(0x100 - offset);
                        registerPC = address;
                    } else
                    {
                        registerPC++;
                        registerPC++;
                    }
                } else if (opcode == "BRK")                                                                                // BRK
                {
                    return ("System Halted");
                } else if (opcode == "BVC")                                                                                // BVC
                {
                    if (!flagV)
                    {
                        registerPC++;
                        byte offset = RAM[registerPC];
                        registerPC++;
                        UInt16 address = registerPC;
                        if (offset < 0x80) address += offset;
                        if (offset >= 0x80) address -= (UInt16)(0x100 - offset);
                        registerPC = address;
                    } else
                    {
                        registerPC++;
                        registerPC++;
                    }
                } else if (opcode == "BVS")                                                                                // BVS
                {
                    if (flagV)
                    {
                        registerPC++;
                        byte offset = RAM[registerPC];
                        registerPC++;
                        UInt16 address = registerPC;
                        if (offset < 0x80) address += offset;
                        if (offset >= 0x80) address -= (UInt16)(0x100 - offset);
                        registerPC = address;
                    } else
                    {
                        registerPC++;
                        registerPC++;
                    }
                } else if (opcode == "CLC")                                                                                // CLC
                {
                    flagC = false;
                    registerPC++;
                } else if (opcode == "CLD")                                                                                // CLD
                {
                    flagD = false;
                    registerPC++;
                } else if (opcode == "CLI")                                                                                // CLI
                {
                    flagI = false;
                    registerPC++;
                } else if (opcode == "CLV")                                                                                // CLV
                {
                    flagV = false;
                    registerPC++;
                } else if (opcode == "CMP")                                                                                // CMP
                {
                    val = GetValue(instruction.Addressing);
                    Calculate(registerA, val, OPERATOR.SUB);
                } else if (opcode == "CPX")                                                                                // CPX
                {
                    val = GetValue(instruction.Addressing);
                    Calculate(registerX, val, OPERATOR.SUB);
                } else if (opcode == "CPY")                                                                                // CPY
                {
                    val = GetValue(instruction.Addressing);
                    Calculate(registerY, val, OPERATOR.SUB);
                } else if (opcode == "DEC")                                                                                // DEC
                {
                    val = GetValue(instruction.Addressing);
                    val--;
                    registerPC = startAddress;
                    SetValue(instruction.Addressing, val);
                    if ((val & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (val == 0x00) flagZ = true; else flagZ = false;
                } else if (opcode == "DEX")                                                                                // DEX
                {
                    registerX--;
                    if ((registerX & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerX == 0x00) flagZ = true; else flagZ = false;
                    registerPC++;
                } else if (opcode == "DEY")                                                                                // DEY
                {
                    registerY--;
                    if ((registerY & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerY == 0x00) flagZ = true; else flagZ = false;
                    registerPC++;
                } else if (opcode == "EOR")                                                                                // EOR
                {
                    val = GetValue(instruction.Addressing);
                    registerA = Calculate(registerA, val, OPERATOR.EOR);
                } else if (opcode == "INC")                                                                                // INC
                {
                    val = GetValue(instruction.Addressing);
                    val++;
                    registerPC = startAddress;
                    SetValue(instruction.Addressing, val);
                    if ((val & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (val == 0x00) flagZ = true; else flagZ = false;
                } else if (opcode == "INX")                                                                                // INX
                {
                    registerX++;
                    if ((registerX & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerX == 0x00) flagZ = true; else flagZ = false;
                    registerPC++;
                } else if (opcode == "INY")                                                                                // INY
                {
                    registerY++;
                    if ((registerY & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerY == 0x00) flagZ = true; else flagZ = false;
                    registerPC++;
                } else if (opcode == "JMP")                                                                                // JMP
                {
                    if (instruction.Addressing == ADDRESSING.ABSOLUTE)
                    {
                        UInt16 address = 0;
                        registerPC++;
                        address += RAM[registerPC];
                        registerPC++;
                        address += (UInt16)(0x0100 * RAM[registerPC]);
                        registerPC = address;
                    }
                    if (instruction.Addressing == ADDRESSING.INDIRECT)
                    {
                        UInt16 address = 0;
                        registerPC++;
                        address += RAM[registerPC];
                        registerPC++;
                        address += (UInt16)(0x0100 * RAM[registerPC]);
                        registerPC = RAM[address];
                        address++;
                        registerPC += (UInt16)(0x0100 * RAM[address]);
                    }
                } else if (opcode == "JSR")                                                                                // JSR
                {
                    UInt16 address = 0;
                    registerPC++;
                    address += RAM[registerPC];
                    registerPC++;
                    address += (UInt16)(0x0100 * RAM[registerPC]);
                    Get2ByteFromInt(registerPC, out lo, out hi);
                    registerSP--;
                    RAM[0x0100 + registerSP] = Convert.ToByte(hi, 16);
                    registerSP--;
                    RAM[0x0100 + registerSP] = Convert.ToByte(lo, 16);
                    registerPC = address;
                } else if (opcode == "LDA")                                                                                // LDA
                {
                    registerA = GetValue(instruction.Addressing);
                    if ((registerA & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerA == 0x00) flagZ = true; else flagZ = false;
                } else if (opcode == "LDX")                                                                                // LDX
                {
                    registerX = GetValue(instruction.Addressing);
                    if ((registerX & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerX == 0x00) flagZ = true; else flagZ = false;
                } else if (opcode == "LDY")                                                                                // LDY
                {
                    registerY = GetValue(instruction.Addressing);
                    if ((registerY & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerY == 0x00) flagZ = true; else flagZ = false;
                } else if (opcode == "LSR")                                                                                // LSR
                {
                    val = GetValue(instruction.Addressing);
                    val = RotateShift(val, OPERATOR.LSR);
                    registerPC = startAddress;
                    SetValue(instruction.Addressing, val);
                } else if (opcode == "NOP")                                                                                // NOP
                {
                    registerPC++;
                } else if (opcode == "ORA")                                                                                // ORA
                {
                    val = GetValue(instruction.Addressing);
                    registerA = Calculate(registerA, val, OPERATOR.OR);
                } else if (opcode == "PHA")                                                                                // PHA
                {
                    registerSP--;
                    RAM[0x0100 + registerSP] = registerA;
                    registerPC++;
                } else if (opcode == "PHP")                                                                                // PHP
                {
                    byte aflag = 00;
                    if (flagN) aflag += 0x80;
                    if (flagZ) aflag += 0x40;
                    aflag += 0x20;
                    if (flagC) aflag += 0x10;
                    aflag += 0x08;
                    if (flagI) aflag += 0x04;
                    if (flagD) aflag += 0x02;
                    if (flagV) aflag += 0x01;
                    registerSP--;
                    RAM[0x0100 + registerSP] = aflag;
                    registerPC++;
                } else if (opcode == "PLA")                                                                                // PLA
                {
                    registerA = RAM[0x0100 + registerSP];
                    registerSP++;
                    registerPC++;
                } else if (opcode == "PLP")                                                                                // PLP
                {
                    byte flags, b;
                    flags = RAM[0x0100 + registerSP];
                    registerSP++;
                    b = (byte)(flags & 0x01);
                    if (b != 0) flagV = true; else flagV = false;
                    b = (byte)(flags & 0x02);
                    if (b != 0) flagD = true; else flagD = false;
                    b = (byte)(flags & 0x04);
                    if (b != 0) flagI = true; else flagI = false;
                    b = (byte)(flags & 0x10);
                    if (b != 0) flagC = true; else flagC = false;
                    b = (byte)(flags & 0x40);
                    if (b != 0) flagZ = true; else flagZ = false;
                    b = (byte)(flags & 0x80);
                    if (b != 0) flagN = true; else flagN = false;
                    registerPC++;
                } else if (opcode == "ROL")                                                                                // ROL
                {
                    val = GetValue(instruction.Addressing);
                    val = RotateShift(val, OPERATOR.ROL);
                    registerPC = startAddress;
                    SetValue(instruction.Addressing, val);
                } else if (opcode == "ROR")                                                                                // ROR
                {
                    val = GetValue(instruction.Addressing);
                    val = RotateShift(val, OPERATOR.ROR);
                    registerPC = startAddress;
                    SetValue(instruction.Addressing, val);
                } else if (opcode == "RTI")                                                                                // RTI
                {
                    byte flags, b;
                    flags = RAM[0x0100 + registerSP];
                    registerSP++;
                    b = (byte)(flags & 0x01);
                    if (b != 0) flagV = true; else flagV = false;
                    b = (byte)(flags & 0x02);
                    if (b != 0) flagD = true; else flagD = false;
                    b = (byte)(flags & 0x04);
                    if (b != 0) flagI = true; else flagI = false;
                    b = (byte)(flags & 0x10);
                    if (b != 0) flagC = true; else flagC = false;
                    b = (byte)(flags & 0x40);
                    if (b != 0) flagZ = true; else flagZ = false;
                    b = (byte)(flags & 0x80);
                    if (b != 0) flagN = true; else flagN = false;
                    registerPC = (UInt16)(RAM[0x0100 + registerSP] + 0x100 * RAM[0x0100 + registerSP + 1]);
                    registerSP++;
                    registerSP++;
                } else if (opcode == "RTS")                                                                                // RTS
                {
                    registerPC = (UInt16)(RAM[0x0100 + registerSP] + 0x100 * RAM[0x0100 + registerSP + 1]);
                    registerSP++;
                    registerSP++;
                    registerPC++;
                } else if (opcode == "SBC")                                                                                // SBC
                {
                    val = GetValue(instruction.Addressing);
                    registerA = Calculate(registerA, val, OPERATOR.SBC);
                } else if (opcode == "SEC")                                                                                // SEC
                {
                    flagC = true;
                    registerPC++;
                } else if (opcode == "SED")                                                                                // SED
                {
                    flagD = true;
                    registerPC++;
                } else if (opcode == "SEI")                                                                                // SEI
                {
                    flagI = true;
                    registerPC++;
                } else if (opcode == "STA")                                                                                // STA
                {
                    SetValue(instruction.Addressing, registerA);
                } else if (opcode == "STX")                                                                                // STX
                {
                    SetValue(instruction.Addressing, registerX);
                } else if (opcode == "STY")                                                                                // STY
                {
                    SetValue(instruction.Addressing, registerY);
                } else if (opcode == "TAX")                                                                                // TAX
                {
                    registerX = registerA;
                    if ((registerX & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerX == 0x00) flagZ = true; else flagZ = false;
                    registerPC++;
                } else if (opcode == "TAY")                                                                                // TAY
                {
                    registerY = registerA;
                    if ((registerY & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerY == 0x00) flagZ = true; else flagZ = false;
                    registerPC++;
                } else if (opcode == "TSX")                                                                                // TSX
                {
                    registerX = registerSP;
                    if ((registerX & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerX == 0x00) flagZ = true; else flagZ = false;
                    registerPC++;
                } else if (opcode == "TXA")                                                                                // TXA
                {
                    registerA = registerX;
                    if ((registerA & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerA == 0x00) flagZ = true; else flagZ = false;
                    registerPC++;
                } else if (opcode == "TXS")                                                                                // TXS
                {
                    registerSP = registerX;
                    if ((registerSP & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerSP == 0x00) flagZ = true; else flagZ = false;
                    registerPC++;
                } else if (opcode == "TYA")                                                                                // TYA
                {
                    registerA = registerY;
                    if ((registerA & 0b10000000) == 0b10000000) flagN = true; else flagN = false;
                    if (registerA == 0x00) flagZ = true; else flagZ = false;
                    registerPC++;
                } else
                {
                    return ("Unknown instruction '" + byteInstruction.ToString("X2") + "'");
                }
            } catch (Exception exception)
            {
                return ("Exception at memory location: " + registerPC.ToString("X") + ":\r\n" + exception.Message);
            }

            if (RAMprogramLine[startAddress] == -1)
            {
                return("No valid instruction at address: 0x" + startAddress.ToString("X4"));
            }

            nextAddress = registerPC;
            return "";
        }

        #endregion
    }
}
