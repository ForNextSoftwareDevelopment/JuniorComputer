using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuniorComputer
{
    class DisAssembler
    {
        #region Members

        // All instructions
        Instructions instructions;

        // Binary file buffer
        private byte[] bytes;

        // Instruction has been decoded already
        private bool[] lineDecoded;

        // All start addresses to be decoded, will be filled along the way after conditional jumps
        private Dictionary<UInt16, bool> addresses;

        // Load address of the code
        private UInt16 loadAddress;

        // Use labels for jump/call adresses
        private bool useLabels;

        // Address Symbol Table
        private Dictionary<UInt16, string> addressSymbolTable = new Dictionary<UInt16, string>();

        // Datatable with addresses and instructions of resulting program 
        private DataTable dtProgram;

        // Resulting (lined) program text
        public string program = "";
        public string linedprogram = "";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="loadAddress"></param>
        /// <param name="startAddress"></param>
        public DisAssembler(byte[] bytes, UInt16 loadAddress, UInt16 startAddress, bool useLabels)
        {
            this.bytes = bytes;
            this.loadAddress = loadAddress;
            this.useLabels = useLabels;

            // All instructions
            instructions = new Instructions();
            
            lineDecoded = new bool[0xFFFF];
            addresses = new Dictionary<ushort, bool>();

            for (int i=0; i<lineDecoded.Length; i++)
            {
                lineDecoded[i] = false;
            }

            // Add first address to start disassemble from
            addresses.Add(startAddress, false);

            // Init program text
            dtProgram = new DataTable();
            dtProgram.Columns.Add("address", typeof(UInt16));
            dtProgram.Columns.Add("instruction", typeof(String));
            dtProgram.Columns.Add("size", typeof(UInt16));
            dtProgram.Columns.Add("type", typeof(int));

            dtProgram.Rows.Add(null, ".ORG $" + loadAddress.ToString("X4"), 0);
        }

        #endregion

        #region Methods

        public string Parse(UInt16 startAddress = 0xFFFF)
        {
            bool ready;

            // If startaddress has been supplied, use this 
            if (startAddress != 0xFFFF)
            {
                if (!addresses.ContainsKey(startAddress))
                {
                    addresses.Add(startAddress, false);
                }
            }

            do
            {
                ready = true;
                
                // Copy address table for the loop (otherwise we can't add to the original addresses dictionary)
                Dictionary<UInt16, bool> addresses_copy = new Dictionary<UInt16, bool>(addresses);
                
                // Check all adddresses in the dictionary for the program path to decode the instructions in this path
                foreach (KeyValuePair<UInt16, bool> keyValuePair in addresses_copy)
                {
                    UInt16 addressKeyValuePair = keyValuePair.Key;
                    UInt16 address = addressKeyValuePair;

                    // Done this program path allready ?
                    bool done = keyValuePair.Value;

                    while (((address - loadAddress) >= 0) && ((address - loadAddress) < bytes.Length) && (!lineDecoded[address]) && !done)
                    {
                        // Set address of the current instruction to be processed
                        UInt16 addressCurrentInstruction = address;

                        // Still some path's to go
                        ready = false;

                        string instruction = Decode(bytes[(UInt16)(address - loadAddress)], out int size, out TYPE type);
                        lineDecoded[address] = true;
                        address++;

                        // No operands
                        if (size == 1) 
                        {
                            // Check for end of the path (RETURN, RESTART)
                            if (type == TYPE.UNCONDITIONALRETURN)
                            {
                                done = true;
                            }
                        }

                        // 1 operand
                        if (size == 2)
                        {
                            if ((address - loadAddress) < bytes.Length)
                            {
                                byte operand = bytes[(UInt16)(address - loadAddress)];
                                string operand_hexascii = "$" + operand.ToString("X2");
                                lineDecoded[address] = true;
                                address++;

                                // Replace operand in mnemonic with actual value
                                string[] instructionSplit = instruction.Split(' ');

                                string arg = instructionSplit[1].Replace("n", operand_hexascii);
                                instruction = instructionSplit[0] + " " + arg;

                                // Check for 'fork' or change of the path (RELATIVE JUMP)
                                if ((type == TYPE.CONDITIONALJUMP) || (type == TYPE.UNCONDITIONALJUMP))
                                {
                                    UInt16 jmpAddress = 0;

                                    if (operand < 0x80)
                                    {
                                        // Positive
                                        jmpAddress = (UInt16)(address + operand);
                                    } else
                                    {
                                        // Negative
                                        jmpAddress = (UInt16)(address - (0x100 - operand));
                                    }

                                    if (useLabels)
                                    {
                                        if (!addressSymbolTable.ContainsKey(jmpAddress))
                                        {
                                            // Add to dictionary and insert
                                            addressSymbolTable.Add(jmpAddress, "lbl_jmp" + jmpAddress.ToString("X4"));
                                            instruction = instructionSplit[0] + " " + instructionSplit[1].Replace("n", "lbl_jmp" + jmpAddress.ToString("X4"));
                                        } else
                                        {
                                            // just insert    
                                            instruction = instructionSplit[0] + " " + instructionSplit[1].Replace("n", addressSymbolTable[jmpAddress]);
                                        }
                                    }

                                    if (type == TYPE.CONDITIONALJUMP)
                                    {
                                        if (!addresses.ContainsKey(jmpAddress)) addresses.Add(jmpAddress, false);
                                    }

                                    if (type == TYPE.UNCONDITIONALJUMP)
                                    {
                                        address = jmpAddress;
                                    }
                                }
                            }
                        }

                        // 2 operands
                        if (size == 3)
                        {
                            byte firstByte = 0x00;
                            byte secondByte = 0x00;

                            string first = "?";
                            string second = "?";

                            if ((address - loadAddress) < bytes.Length)
                            {
                                firstByte = bytes[(UInt16)(address - loadAddress)];
                                first = firstByte.ToString("X2");
                                lineDecoded[address] = true;
                                address++;

                                if ((address - loadAddress) < bytes.Length)
                                {
                                    secondByte = bytes[(UInt16)(address - loadAddress)];
                                    second = secondByte.ToString("X2");
                                    lineDecoded[address] = true;
                                    address++;
                                }
                            }

                            if (useLabels &&
                                (
                                 (type == TYPE.CONDITIONALJUMP) ||
                                 (type == TYPE.UNCONDITIONALJUMP) ||
                                 (type == TYPE.UNCONDITIONALCALL)
                                )
                               )
                            {
                                UInt16 jmpsubAddress = (UInt16)(secondByte * 0x100 + firstByte);

                                if (!addressSymbolTable.ContainsKey(jmpsubAddress))
                                {
                                    // Add to dictionary and insert
                                    if ((type == TYPE.CONDITIONALJUMP) || (type == TYPE.UNCONDITIONALJUMP))
                                    {
                                        addressSymbolTable.Add(jmpsubAddress, "lbl_jmp" + jmpsubAddress.ToString("X4"));

                                        string[] instructionSplit = instruction.Split(' ');
                                        instruction = instructionSplit[0] + " " + instructionSplit[1].Replace("nn", "lbl_jmp" + jmpsubAddress.ToString("X4"));
                                    }

                                    if (type == TYPE.UNCONDITIONALCALL)
                                    {
                                        addressSymbolTable.Add(jmpsubAddress, "lbl_sub" + jmpsubAddress.ToString("X4"));

                                        string[] instructionSplit = instruction.Split(' ');
                                        instruction = instructionSplit[0] + " " + instructionSplit[1].Replace("nn", "lbl_sub" + jmpsubAddress.ToString("X4"));
                                    }
                                } else
                                {
                                    // Just insert
                                    string[] instructionSplit = instruction.Split(' ');
                                    instruction = instructionSplit[0] + " " + instructionSplit[1].Replace("nn", addressSymbolTable[jmpsubAddress]);
                                }
                            } else
                            {
                                // Inverted (low byte first, high byte second)
                                string operand = "$" + second + first;
                                string[] instructionSplit = instruction.Split(' ');
                                instruction = instructionSplit[0] + " " + instructionSplit[1].Replace("nn", operand);
                            }

                            // Check for 'fork' of the path (JUMP ON PARITY, CALL ON ZERO etc.)
                            if ((type == TYPE.CONDITIONALJUMP) ||
                                (type == TYPE.UNCONDITIONALCALL))
                            {
                                UInt16 newAddress = (UInt16)(secondByte * 0x100 + firstByte);
                                if (!addresses.ContainsKey(newAddress)) addresses.Add(newAddress, false);
                            }

                            // Check for change of the path (JUMP)
                            if (type == TYPE.UNCONDITIONALJUMP)
                            {
                                address = (UInt16)(secondByte * 0x100 + firstByte);
                            }
                        }

                        // Add program line (address + instruction)
                        dtProgram.Rows.Add(addressCurrentInstruction, instruction, size, type);
                    }

                    // Set this key has been done
                    addresses[addressKeyValuePair] = true;
                }
            } while (!ready);

            // Order program lines to address
            dtProgram.DefaultView.Sort = "address";
            dtProgram = dtProgram.DefaultView.ToTable();

            // Copy addressSymbolTable to check for use
            Dictionary<UInt16, string> addressSymbolTableNotUsed = new Dictionary<UInt16, string>(addressSymbolTable);

            program = "";
            linedprogram = "";
            UInt16 lastAddress = loadAddress;
            UInt16 lastSize = 0x0000;
            foreach (DataRow row in dtProgram.Rows)
            {
                // If this instruction is at an address which has a gap with the previous one, then fill with data (DB)
                if (row["address"] != DBNull.Value)
                {
                    UInt16 currentAddress = Convert.ToUInt16(row["address"]);

                    UInt16 currentSize = 0;
                    if (row["size"] != DBNull.Value)
                    {
                        currentSize = Convert.ToUInt16(row["size"]);
                    }

                    // Check for gap, treat this as data
                    int dbSize = currentAddress - lastAddress - lastSize;
                    if (dbSize > 0)
                    {
                        string arg = " ";
                        string argASCII = "    ; ASCII: ";
                        for (int i = lastAddress + lastSize; i < currentAddress; i++)
                        {
                            if (i != lastAddress + lastSize) arg += ", ";
                            byte dataByte = bytes[(UInt16)(i - loadAddress)];
                            arg += "$" + dataByte.ToString("X2");
                            argASCII += (dataByte > 0x20) && (dataByte < 0x80) ? ((char)(bytes[(UInt16)(i - loadAddress)])).ToString() : ".";
                        }

                        if (useLabels) program += "                  ";
                        program += ".DB " + arg + argASCII + "\r\n";

                        linedprogram += (lastAddress + lastSize).ToString("X4") + ": ";
                        if (useLabels) linedprogram += "             ";
                        linedprogram += ".DB " + arg + argASCII + "\r\n";
                    }

                    if (currentAddress != 0) lastAddress = currentAddress;
                    if (currentSize != 0) lastSize = currentSize;
                }

                // If labels are being used, fill them into the program
                if (useLabels)
                {
                    if (row["address"] != DBNull.Value)
                    {
                        UInt16 currentAddress = Convert.ToUInt16(row["address"]);

                        if (addressSymbolTable.ContainsKey(currentAddress))
                        {
                            program += addressSymbolTable[currentAddress] + ": ";
                            program += row["instruction"] + "\r\n";

                            linedprogram += row["address"] != DBNull.Value ? Convert.ToUInt16(row["address"]).ToString("X4") + ": " : "";
                            linedprogram += addressSymbolTable[currentAddress] + ": ";
                            linedprogram += row["instruction"];

                            // Delete from (copied) table
                            if (addressSymbolTableNotUsed.ContainsKey(currentAddress))
                            {
                                addressSymbolTableNotUsed.Remove(currentAddress);
                            }
                        } else
                        {
                            program += "             ";
                            program += row["instruction"] + "\r\n";

                            linedprogram += row["address"] != DBNull.Value ? Convert.ToUInt16(row["address"]).ToString("X4") + ": " : "";
                            linedprogram += "             ";
                            linedprogram += row["instruction"];
                        }
                    } else
                    {
                        program += row["instruction"] + "\r\n";
                        linedprogram += row["address"] != DBNull.Value ? Convert.ToUInt16(row["address"]).ToString("X4") + ": " : "";
                        linedprogram += row["instruction"];
                    }
                } else
                {
                    program += row["instruction"] + "\r\n";
                    linedprogram += row["address"] != DBNull.Value ? Convert.ToUInt16(row["address"]).ToString("X4") + ": " : "";
                    linedprogram += row["instruction"];
                }

                linedprogram += "\r\n";
            }

            // If any data left after last line processed, treat this as byte data (DB)
            if (loadAddress + bytes.Length - lastAddress - lastSize > 0)
            {
                string arg = " ";
                string argASCII = "    ; ASCII: ";
                for (int i = lastAddress + lastSize; i < (loadAddress + bytes.Length); i++)
                {
                    if (i != lastAddress + lastSize) arg += ", ";
                    byte dataByte = bytes[(UInt16)(i - loadAddress)];
                    arg += "$" + dataByte.ToString("X2");
                    argASCII += (dataByte > 0x20) && (dataByte < 0x80) ? ((char)(bytes[(UInt16)(i - loadAddress)])).ToString() : ".";
                }

                if (useLabels) program += "             ";
                program += ".DB " + arg + argASCII + "\r\n";

                linedprogram += (lastAddress + lastSize).ToString("X4") + ": ";
                if (useLabels) linedprogram += "             ";
                linedprogram += ".DB " + arg + argASCII + "\r\n";
            }

            // Add not used address labels as EQU statements
            if (useLabels)
            {
                string labels = "";
                foreach (KeyValuePair<UInt16, string> kvp in addressSymbolTableNotUsed)
                {
                    labels += kvp.Value + " .EQU " + kvp.Key.ToString("X4") + "H\r\n";
                }

                if (labels != "")
                {
                    program = labels + "\r\n" + program;
                    linedprogram = labels + "\r\n" + linedprogram;
                }
            }

            return (program);
        }

        /// <summary>
        /// Decode a singe instruction and state the number of operand bytes
        /// </summary>
        /// <param name="code"></param>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string Decode(byte code, out int size, out TYPE type)
        {
            string mnemonic = "";
            size = 0;
            type = TYPE.NONE;

            // Check opcode to find the instruction in the list
            for (int index = 0; index < instructions.MainInstructions.Length; index++)
            {
                // Opcode matches
                if (code == instructions.MainInstructions[index].Opcode)
                {
                    mnemonic = instructions.MainInstructions[index].Mnemonic;
                    size = instructions.MainInstructions[index].Size;
                    type = instructions.MainInstructions[index].Type;

                    return (mnemonic);
                }
            }

            return ("");
        }

        #endregion
    }
}
