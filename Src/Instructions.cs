namespace JuniorComputer
{
    #region InstructionType/Addressing

    // Type of instruction: (un)conditional jump/call/restart, halt or none
    public enum TYPE
    {
        NONE,
        CONDITIONALJUMP,
        UNCONDITIONALJUMP,
        UNCONDITIONALCALL,
        UNCONDITIONALRETURN,
        HALT
    }

    // Instruction addressing: immediate, absolute etc.
    public enum ADDRESSING
    {
        ABSOLUTE,
        ABSOLUTEX,
        ABSOLUTEY,
        IMMEDIATE,
        IMPLIED,
        INDIRECT,
        XINDIRECT,
        INDIRECTY,
        RELATIVE,
        ZEROPAGE,
        ZEROPAGEX,
        ZEROPAGEY
    }

    #endregion

    #region Instruction

    // Instruction structure
    public readonly struct Instruction
    {
        public Instruction(int opcode, string mnemonic, int size, TYPE type, ADDRESSING addressing, string description)
        {
            Opcode = opcode;
            Mnemonic = mnemonic;
            Size = size;
            Type = type;
            Addressing = addressing;
            Description = description;
        }

        public int Opcode { get; }

        public string Mnemonic { get; }

        public int Size { get; }

        public TYPE Type { get; }

        public ADDRESSING Addressing { get; }

        public string Description { get; }

        public override string ToString() => $"OPCODE:\t{Opcode}\r\nMNEMONIC:\t{Mnemonic}\r\nSIZE:\t\t{Size}\r\nTYPE:\t\t{Type}\r\nADDRESSING:\t{Addressing}\r\nDESCRIPTION:\t{Description}";
    }

    #endregion

    #region Instructions

    public class Instructions
    {
        public Instruction[] MainInstructions;

        /// <summary>
        /// Constructor
        /// </summary>
        public Instructions()
        {
            #region Instructions (Main)

            // 6502 Instructions 
            MainInstructions = new Instruction[]
            {
                new Instruction(0x00, "BRK", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Force break"),
                new Instruction(0x01, "ORA (n,X)", 2, TYPE.NONE, ADDRESSING.XINDIRECT, "Or with Accumulator"),
                new Instruction(0x02, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x03, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x04, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x05, "ORA n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Or with Accumulator"),
                new Instruction(0x06, "ASL n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Arithmetic shift left"),
                new Instruction(0x07, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x08, "PHP", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Push processor status on stack"),
                new Instruction(0x09, "ORA #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "Or with Accumulator"),
                new Instruction(0x0A, "ASL A", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Arithmetic shift left"),
                new Instruction(0x0B, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x0C, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x0D, "ORA nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Or with Accumulator"),
                new Instruction(0x0E, "ASL nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Arithmetic shift left"),
                new Instruction(0x0F, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x10, "BPL n", 2, TYPE.CONDITIONALJUMP, ADDRESSING.RELATIVE, "Branch on Positive"),
                new Instruction(0x11, "ORA (n),Y", 2, TYPE.NONE, ADDRESSING.INDIRECTY, "Or with Accumulator"),
                new Instruction(0x12, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x13, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x14, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x15, "ORA n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Or with Accumulator"),
                new Instruction(0x16, "ASL n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Arithmetic shift left"),
                new Instruction(0x17, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x18, "CLC", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Clear Carry"),
                new Instruction(0x19, "ORA nn,Y", 3, TYPE.NONE, ADDRESSING.ABSOLUTEY, "Or with Accumulator"),
                new Instruction(0x1A, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x1B, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x1C, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x1D, "ORA nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Or with Accumulator"),
                new Instruction(0x1E, "ASL nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Arithmetic shift left"),
                new Instruction(0x1F, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x20, "JSR nn", 3, TYPE.UNCONDITIONALCALL, ADDRESSING.ABSOLUTE, "Jump to subroutine"),
                new Instruction(0x21, "AND (n,X)", 2, TYPE.NONE, ADDRESSING.XINDIRECT, "And with Accumulator"),
                new Instruction(0x22, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x23, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x24, "BIT n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Test bits with Accumulator"),
                new Instruction(0x25, "AND n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "And with Accumulator"),
                new Instruction(0x26, "ROL n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Rotate left"),
                new Instruction(0x27, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x28, "PLP", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Pull processor status from stack"),
                new Instruction(0x29, "AND #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "And with Accumulator"),
                new Instruction(0x2A, "ROL", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Rotate left"),
                new Instruction(0x2B, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x2C, "BIT nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Test bits with Accumulator"),
                new Instruction(0x2D, "AND nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "And with Accumulator"),
                new Instruction(0x2E, "ROL nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Rotate left"),
                new Instruction(0x2F, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x30, "BMI n", 2, TYPE.CONDITIONALJUMP, ADDRESSING.RELATIVE, "Branch on Minus"),
                new Instruction(0x31, "AND (n),Y", 2, TYPE.NONE, ADDRESSING.INDIRECTY, "And with Accumulator"),
                new Instruction(0x32, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x33, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x34, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x35, "AND n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "And with Accumulator"),
                new Instruction(0x36, "ROL n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Rotate left"),
                new Instruction(0x37, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x38, "SEC", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Set Carry"),
                new Instruction(0x39, "AND nn,Y", 3, TYPE.NONE, ADDRESSING.ABSOLUTEY, "And with Accumulator"),
                new Instruction(0x3A, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x3B, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x3C, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x3D, "AND nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "And with Accumulator"),
                new Instruction(0x3E, "ROL nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Rotate left"),
                new Instruction(0x3F, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x40, "RTI", 1, TYPE.UNCONDITIONALRETURN, ADDRESSING.IMPLIED, "Return from interrupt"),
                new Instruction(0x41, "EOR (n,X)", 2, TYPE.NONE, ADDRESSING.XINDIRECT, "Exclusive-Or with Accumulator"),
                new Instruction(0x42, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x43, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x44, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x45, "EOR n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Exclusive-Or with Accumulator"),
                new Instruction(0x46, "LSR n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Logical shift right"),
                new Instruction(0x47, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x48, "PHA", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Push accumulator on stack"),
                new Instruction(0x49, "EOR #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "Exclusive-Or with Accumulator"),
                new Instruction(0x4A, "LSR A", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Logical shift right"),
                new Instruction(0x4B, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x4C, "JMP nn", 3, TYPE.UNCONDITIONALJUMP, ADDRESSING.ABSOLUTE, "Jump"),
                new Instruction(0x4D, "EOR nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Exclusive-Or with Accumulator"),
                new Instruction(0x4E, "LSR nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Logical shift right"),
                new Instruction(0x4F, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x50, "BVC n", 2, TYPE.CONDITIONALJUMP, ADDRESSING.RELATIVE, "Branch on Overflow clear"),
                new Instruction(0x51, "EOR (n),Y", 2, TYPE.NONE, ADDRESSING.INDIRECTY, "Exclusive-Or with Accumulator"),
                new Instruction(0x52, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x53, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x54, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x55, "EOR n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Exclusive-Or with Accumulator"),
                new Instruction(0x56, "LSR n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Logical shift right"),
                new Instruction(0x57, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x58, "CLI", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Clear Interrupt disable"),
                new Instruction(0x59, "EOR nn,Y", 3, TYPE.NONE, ADDRESSING.ABSOLUTEY, "Exclusive-Or with Accumulator"),
                new Instruction(0x5A, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x5B, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x5C, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x5D, "EOR nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Exclusive-Or with Accumulator"),
                new Instruction(0x5E, "LSR nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Logical shift right"),
                new Instruction(0x5F, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x60, "RTS", 1, TYPE.UNCONDITIONALRETURN, ADDRESSING.IMPLIED, "Return from subroutine"),
                new Instruction(0x61, "ADC (n,X)", 2, TYPE.NONE, ADDRESSING.XINDIRECT, "Add to Accumulator with Carry"),
                new Instruction(0x62, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x63, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x64, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x65, "ADC n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Add to Accumulator with Carry"),
                new Instruction(0x66, "ROR n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Rotate right"),
                new Instruction(0x67, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x68, "PLA", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Pull accumulator from stack"),
                new Instruction(0x69, "ADC #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "Add to Accumulator with Carry"),
                new Instruction(0x6A, "ROR", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Rotate right"),
                new Instruction(0x6B, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x6C, "JMP (nn)", 3, TYPE.UNCONDITIONALJUMP, ADDRESSING.INDIRECT, "Jump"),
                new Instruction(0x6D, "ADC nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Add to Accumulator with Carry"),
                new Instruction(0x6E, "ROR nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Rotate right"),
                new Instruction(0x6F, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x70, "BVS n", 2, TYPE.CONDITIONALJUMP, ADDRESSING.RELATIVE, "Branch on Overflow set"),
                new Instruction(0x71, "ADC (n),Y", 2, TYPE.NONE, ADDRESSING.INDIRECTY, "Add to Accumulator with Carry"),
                new Instruction(0x72, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x73, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x74, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x75, "ADC n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Add to Accumulator with Carry"),
                new Instruction(0x76, "ROR n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Rotate right"),
                new Instruction(0x77, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x78, "SEI", 1, TYPE.NONE, ADDRESSING.IMPLIED, "SET Interrupt disable"),
                new Instruction(0x79, "ADC nn,Y", 3, TYPE.NONE, ADDRESSING.ABSOLUTEY, "Add to Accumulator with Carry"),
                new Instruction(0x7A, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x7B, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x7C, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x7D, "ADC nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Add to Accumulator with Carry"),
                new Instruction(0x3E, "ROR nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Rotate right"),
                new Instruction(0x7F, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x80, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x81, "STA (n,X)", 2, TYPE.NONE, ADDRESSING.XINDIRECT, "Store Accumulator"),
                new Instruction(0x82, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x83, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x84, "STY n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Store register Y"),
                new Instruction(0x85, "STA n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Store Accumulator"),
                new Instruction(0x86, "STX n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Store register X"),
                new Instruction(0x87, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x88, "DEY", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Decrement register Y by one"),
                new Instruction(0x89, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x8A, "TXA", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Transfer register X to Accumulator"),
                new Instruction(0x8B, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x8C, "STY nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Store register Y"),
                new Instruction(0x8D, "STA nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Store Accumulator"),
                new Instruction(0x8E, "STX nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Store register X"),
                new Instruction(0x8F, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x90, "BCC n", 2, TYPE.CONDITIONALJUMP, ADDRESSING.RELATIVE, "Branch on Carry clear"),
                new Instruction(0x91, "STA (n),Y", 2, TYPE.NONE, ADDRESSING.INDIRECTY, "Store Accumulator"),
                new Instruction(0x92, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x93, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x94, "STY n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Store register Y"),
                new Instruction(0x95, "STA n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Store Accumulator"),
                new Instruction(0x96, "STX n,Y", 2, TYPE.NONE, ADDRESSING.ZEROPAGEY, "Store register X"),
                new Instruction(0x97, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x98, "TYA", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Transfer register Y to Accumulator"),
                new Instruction(0x99, "STA nn,Y", 3, TYPE.NONE, ADDRESSING.ABSOLUTEY, "Store Accumulator"),
                new Instruction(0x9A, "TXS", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Transfer register X to Stack Pointer"),
                new Instruction(0x9B, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x9C, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x9D, "STA nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Store Accumulator"),
                new Instruction(0x9E, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0x9F, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xA0, "LDY #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "Load register Y"),
                new Instruction(0xA1, "LDA (n,X)", 2, TYPE.NONE, ADDRESSING.XINDIRECT, "Load Accumulator"),
                new Instruction(0xA2, "LDX #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "Load register X"),
                new Instruction(0xA3, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xA4, "LDY n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Load register Y"),
                new Instruction(0xA5, "LDA n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Load Accumulator"),
                new Instruction(0xA6, "LDX n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Load register X"),
                new Instruction(0xA7, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xA8, "TAY", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Transfer Accumulator to register Y"),
                new Instruction(0xA9, "LDA #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "Load Accumulator"),
                new Instruction(0xAA, "TAX", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Transfer Accumulator to register X"),
                new Instruction(0xAB, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xAC, "LDY nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Load register Y"),
                new Instruction(0xAD, "LDA nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Load Accumulator"),
                new Instruction(0xAE, "LDX nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Load register X"),
                new Instruction(0xAF, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xB0, "BCS n", 2, TYPE.CONDITIONALJUMP, ADDRESSING.RELATIVE, "Branch on Carry set"),
                new Instruction(0xB1, "LDA (n),Y", 2, TYPE.NONE, ADDRESSING.INDIRECTY, "Load Accumulator"),
                new Instruction(0xB2, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xB3, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xB4, "LDY n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Load register Y"),
                new Instruction(0xB5, "LDA n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Load Accumulator"),
                new Instruction(0xB6, "LDA n,Y", 2, TYPE.NONE, ADDRESSING.ZEROPAGEY, "Load register X"),
                new Instruction(0xB7, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xB8, "CLV", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Clear Overflow"),
                new Instruction(0xB9, "LDA nn,Y", 3, TYPE.NONE, ADDRESSING.ABSOLUTEY, "Load Accumulator"),
                new Instruction(0xBA, "TSX", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Transfer Stack Pointer to register X"),
                new Instruction(0xBB, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xBC, "LDY nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Load register Y"),
                new Instruction(0xBD, "LDA nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Load Accumulator"),
                new Instruction(0xBE, "LDX nn,Y", 3, TYPE.NONE, ADDRESSING.ABSOLUTEY, "Load register X"),
                new Instruction(0xBF, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xC0, "CPY #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "Compare with register Y"),
                new Instruction(0xC1, "CMP (n,X)", 2, TYPE.NONE, ADDRESSING.XINDIRECT, "Compare with Accumulator"),
                new Instruction(0xC2, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xC3, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xC4, "CPY n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Compare with register Y"),
                new Instruction(0xC5, "CMP n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Compare with Accumulator"),
                new Instruction(0xC6, "DEC n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Decrement by one"),
                new Instruction(0xC7, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xC8, "INY", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Increment register Y by one"),
                new Instruction(0xC9, "CMP #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "Compare with Accumulator"),
                new Instruction(0xCA, "DEX", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Decrement register X by one"),
                new Instruction(0xCB, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xCC, "CPY nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Compare with register Y"),
                new Instruction(0xCD, "CMP nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Compare with Accumulator"),
                new Instruction(0xCE, "DEC nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Decrement by one"),
                new Instruction(0xCF, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xD0, "BNE n", 2, TYPE.CONDITIONALJUMP, ADDRESSING.RELATIVE, "Branch on not Zero"),
                new Instruction(0xD1, "CMP (n),Y", 2, TYPE.NONE, ADDRESSING.INDIRECTY, "Compare with Accumulator"),
                new Instruction(0xD2, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xD3, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xD4, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xD5, "CMP n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Compare with Accumulator"),
                new Instruction(0xD6, "DEC n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Decrement by one"),
                new Instruction(0xD7, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xD8, "CLD", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Clear Decimal Mode"),
                new Instruction(0xD9, "CMP nn,Y", 3, TYPE.NONE, ADDRESSING.ABSOLUTEY, "Compare with Accumulator"),
                new Instruction(0xDA, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xDB, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xDC, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xDD, "CMP nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Compare with Accumulator"),
                new Instruction(0xDE, "DEC nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Decrement by one"),
                new Instruction(0xDF, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xE0, "CPX #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "Compare with register X"),
                new Instruction(0xE1, "SBC (n,X)", 2, TYPE.NONE, ADDRESSING.XINDIRECT, "Subtract from Accumulator with Borrow"),
                new Instruction(0xE2, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xE3, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xE4, "CPX n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Compare with register X"),
                new Instruction(0xE5, "SBC n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Subtract from Accumulator with Borrow"),
                new Instruction(0xE6, "INC n", 2, TYPE.NONE, ADDRESSING.ZEROPAGE, "Increment by one"),
                new Instruction(0xE7, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xE8, "INX", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Increment register X by one"),
                new Instruction(0xE9, "SBC #n", 2, TYPE.NONE, ADDRESSING.IMMEDIATE, "Subtract from Accumulator with Borrow"),
                new Instruction(0xEA, "NOP", 1, TYPE.NONE, ADDRESSING.IMPLIED, "No operation"),
                new Instruction(0xEB, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xEC, "CPX nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Compare with register X"),
                new Instruction(0xED, "SBC nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Subtract from Accumulator with Borrow"),
                new Instruction(0xEE, "INC nn", 3, TYPE.NONE, ADDRESSING.ABSOLUTE, "Increment by one"),
                new Instruction(0xEF, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xF0, "BEQ n", 2, TYPE.CONDITIONALJUMP, ADDRESSING.RELATIVE, "Branch on Zero"),
                new Instruction(0xF1, "SBC (n),Y", 2, TYPE.NONE, ADDRESSING.INDIRECTY, "Subtract from Accumulator with Borrow"),
                new Instruction(0xF2, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xF3, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xF4, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xF5, "SBC n,X", 1, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Subtract from Accumulator with Borrow"),
                new Instruction(0xF6, "INC n,X", 2, TYPE.NONE, ADDRESSING.ZEROPAGEX, "Increment by one"),
                new Instruction(0xF7, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xF8, "SED", 1, TYPE.NONE, ADDRESSING.IMPLIED, "Set Decimal"),
                new Instruction(0xF9, "SBC nn,Y", 3, TYPE.NONE, ADDRESSING.ABSOLUTEY, "Subtract from Accumulator with Borrow"),
                new Instruction(0xFA, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xFB, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xFC, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, ""),
                new Instruction(0xFD, "SBC nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Subtract from Accumulator with Borrow"),
                new Instruction(0xFE, "INC nn,X", 3, TYPE.NONE, ADDRESSING.ABSOLUTEX, "Increment by one"),
                new Instruction(0xFF, "-", 1, TYPE.NONE, ADDRESSING.IMPLIED, "")
            };

            #endregion
        }
    }

    #endregion
}
