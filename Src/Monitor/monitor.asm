
;----------------------------------------------------------------------------
;
; SOURCE LISTING OF ELEKTOR'S JUNIOR COMPUTER
;
; WRITTEN BY A. NACHTMANN
;
; DATE:  7 FEB. 1980
;
; THE FEATURES OF JUNIOR'S MONITOR ARE:
;
; HEX ADDRESS DATA DISPLAY (ENTRY VIA RST)
; HEX EDITOR (START ADDRESS $1CB5)
; HEX ASSEMBLER (START ADDRESS $1F51)
;
;----------------------------------------------------------------------------
; Original code restored and adapted for the 
; TASM (Telemark Assembler) by :
; A.J. Prosman
; October 26, 2019

; Checked and corrected and compared to original hex dump 
; Hans Otten, 2022

; Adjusted for the Junior Computer Assembler/Simulator
; Dirk Prins, 2023
;----------------------------------------------------------------------------
;
; EDITOR'S POINTERS AND TEMPS IN PAGE ZERO
;
        .ORG $00E1

KEY:    .DB $00
BEGADL: .DB $00            ; Begin Address Pointer
BEGADH: .DB $00
ENDADL: .DB $00            ; End Address Pointer
ENDADH: .DB $00
CURADL: .DB $00            ; Current Address Pointer
CURADH: .DB $00
CENDL:  .DB $00            ; Current Address Pointer
CENDH:  .DB $00
MOVADL: .DB $00
MOVADH: .DB $00
TABLEL: .DB $00
TABLEH: .DB $00
LABELS: .DB $00

        .ORG $00F6
BYTES:  .DB $00            ; Number of bytes to be displayed
COUNT:  .DB $00

;       MPU REGISTERS IN PAGE ZERO

        .ORG $00EF

PCL:    .DB $00
PCH:    .DB $00
PREG:   .DB $00            ; = Flags
SPUSER: .DB $00
ACC:    .DB $00
YREG:   .DB $00
XREG:   .DB $00

;       HEX DISPLAY BUFFERS IN PAGE ZERO

        .ORG $00F8

INL:    .DB $00
INH:    .DB $00
POINTL: .DB $00
POINTH: .DB $00

;       TEMPORARY DATA BUFFERS IN PAGE ZERO

TEMP:   .DB $00
TEMPX:  .DB $00
NIBBLE: .DB $00
MODE:   .DB $01            ; ( 0 = DA MODE , NOT 0 = AD MODE )

;       MEMORY LOCATIONS IN THE 6532-IC

PAD:    .EQU $1A80          ; DATA REGISTER OF PORT A
PADD:   .EQU $1A81          ; DATA DIRECTION REGISTER OF PORT A
PBD:    .EQU $1A82          ; DATA REGISTER OF PORT B
PBDD:   .EQU $1A83          ; DATA DIRECTION REGISTER OF PORT B

;       WRITE EDGE DETECT CONTROL

EDETA:  .EQU $1AE4          ; NEG EDET DISABLE PA7-IRQ
EDETB:  .EQU $1AE5          ; POS EDET DISABLE PA7-IRQ
WDETC:  .EQU $1AE6          ; NEG EDET ENABLE PA7-IRQ
EDETD:  .EQU $1AE7          ; POS EDET ENABLE PA7-IRQ

;       READ FLAG REGISTER AND CLEAR TIMER & IRQ FLAG

RDFLAG: .EQU $1AD5          ; BIT6=PA7-FLAG; BIT7=TIMER-FLAG

;       WRITE COUNT INTO TIMER, DISABLE TIMER-IRQ

CNTA:   .EQU $1AF4          ; CLK1T
CNTB:   .EQU $1AF5          ; CLK8T
CNTC:   .EQU $1AF6          ; CLK64T
CNTD:   .EQU $1AF7          ; CLK1024T

;       WRITE COUNT INTO TIMER, ENABLE TIMER-IRQ

CNTE:   .EQU $1AFC          ; CLK1T
CNTF:   .EQU $1AFD          ; CLK8T
CNTG:   .EQU $1AFE          ; CLK64T
CNTH:   .EQU $1AFF          ; CLK1024T

;       INTERRUPT VECTORS :  IRQ & NMI VECTORS SHOULD BE
;       LOADED IN THE FOLLOWING MEMORY LOCATIONS FOR
;       PROPER SYSTEM OPERATION.

NMIL:   .EQU $1A7A          ; NMI LOWER BYTE
NMIH:   .EQU $1A7B          ; NMI HIGHER BYTE
IRQL:   .EQU $1A7E          ; IRQ LOWER BYTE
IRQH:   .EQU $1A7F          ; IRQ HIGHER BYTE

;       BEGINNERS MAY LOAD INTO THESE LOCATIONS
;       $1C00 FOR STEP BY STEP MODUS AND BRK COMMAND

;----------------------------------------------------------------------------
;        JUNIOR'S MAIN ROUTINES
        .ORG $1C00 

SAVE:
        STA     ACC         ; Save ACCU
        PLA                 ; Get current P-Register
        STA     PREG        ; Save P-Register
SAVEA:
        PLA                 ; Get current PCL
        STA     PCL         ; Save current PCL
        STA     POINTL      ; PCL to display buffer
        PLA                 ; Get current PCH
        STA     PCH         ; Save current PCH
        STA     POINTH      ; PCH to display buffer
SAVEB:
        STY     YREG        ; Save current Y-Register
        STX     XREG        ; Save current X-Register 
        TSX                 ; Get current SP
        STX     SPUSER      ; Save current SP
        LDX     #$01        ; Set AD-Mode
        STX     MODE
        JMP     START

RESET:
        LDA     #$1E        ; PB1---PB4
        STA     PBDD        ; IS output
        LDA     #$04        ; Reset P-Register
        STA     PREG
        LDA     #$03
        STA     MODE        ; Set AD-Mode
        STA     BYTES       ; Display POINTH, POINTL, INH
        LDX     #$FF
        TXS 
        STX     SPUSER
        CLD 
        SEI 

START:
        JSR     SCAND       ; Display data specified by POINTH, POINTL
        BNE     START       ; Wait until key is released
STARA:
        JSR     SCAND       ; Display data specified by point
        BEQ     STARA       ; Any key pressed
        JSR     SCAND       ; Debounce key
        BEQ     STARA       ; Any key still pressed
        JSR     GETKEY      ; If Yes, decode key, return with key in ACC

GOEXEC:
        CMP     #$13        ; GO-Key ?
        BNE     ADMODE
        LDX     SPUSER      ; Get current SP
        TXS 
        LDA     POINTH      ; Start execution at POINTH, POINTL
        PHA 
        LDA     POINTL
        PHA 
        LDA     PREG        ; Restore current P register
        PHA 
        LDX     XREG
        LDY     YREG
        LDA     ACC
        RTI                 ; Execute program

ADMODE:
        CMP     #$10        ; AD-Key ?
        BNE     DAMODE
        LDA     #$03        ; Set AD-Mode
        STA     MODE
        BNE     STEPA       ; Always
        
DAMODE:
        CMP     #$11        ; DA-Key ?
        BNE     STEP
        LDA     #$00        ; Set DA-Mode
        STA     MODE
        BEQ     STEPA

STEP:
        CMP     #$12        ; PLUS-Key ?
        BNE     PCKEY
        INC     POINTL
        BNE     STEPA
        INC     POINTH

STEPA:
        JMP     START

PCKEY:   
        CMP     #$14        ; PC-Key
        BNE     ILLKEY
        LDA     PCL
        STA     POINTL      ; Last PC to display buffer
        LDA     PCH
        STA     POINTH
        JMP     STEPA
        
ILLKEY:
        CMP     #$15        ; Illegal key?
        BPL     STEPA       ; If Yes, ignore it

DATA:
        STA     KEY         ; Save key
        LDY     MODE        ; Y=0 Is data mode, else address mode
        BNE     ADDRESS
        LDA     (POINTL),Y  ; Get Data specified
        ASL     A           ; by point
        ASL     A           ; shift low order
        ASL     A           ; nibble into high order nibble
        ASL     A
        ORA     KEY         ; Data with key
        STA     (POINTL),Y  ; Restore data
        JMP     STEPA

ADDRESS:
        LDX     #$04        ; 4 Shifts
ADLOOP:
        ASL     POINTL      ; POINTH, POINTL 4 Positions to the left
        ROL     POINTH
        DEX 
        BNE     ADLOOP
        LDA     POINTL
        ORA     KEY         ; Restore address
        STA     POINTL
        JMP     STEPA


;----------------------------------------------------------------------------
;       JUNIOR'S HEX EDITOR
;
;       FOLLOWING COMMANDS ARE VALID:
;
;       "INSERT": INSERT A NEW LINE JUST BEFORE DISPLAYED LINE
;
;       "INPUT": INSERT A NEW LINE JUST BEHIND THE DISPLAYED LINE
;
;       "SEARCH": SEARCH IN WORKSPACE FOR A GIVEN 2BYTE PATTERN
;
;       "SKIP": SKIP TO NEXT INSTRUCTION
;
;       "DELETE": DELETE CURRENT DISPLAYED INSTRUCTION
;
;       AN ERROR IS INDICATED, IF THE INSTRUCTION POINTER
;       CURAD IS OUT OF RANGE

EDITOR:
        JSR     BEGIN       ; CURAD := BEGAD
        LDY     BEGADH
        LDX     BEGADL
        INX 
        BNE     EDIT
        INY 
EDIT:
        STX     CENDL       ; CEND := BEGAD + 1
        STY     CENDH
        LDA     #$77        ; Display "77"
        LDY     #$00
        STA     (CURADL),Y
CMND:
        JSR     SCAN        ; Display current instruction,
                            ; wait for a key
SEARCH:
        CMP     #$14        ; Search command ?
        BNE     INSERT
        JSR     GETBYT      ; Read 1st byte
        BPL     SEARCH      ; COM. Key ?
        STA     POINTH      ; Discard data
        JSR     GETBYT      ; Read 2nd byte
        BPL     SEARCH      ; COM. Key ?
        STA     POINTL      ; Discard data
        JSR     BEGIN       ; CURAD := BEGAD

SELOOP:
        LDY     #$00
        LDA     (CURADL),Y  ; Compare instruction
        CMP     POINTH      ; against data to be searched
        BNE     SEARA       ; Skip to the next instruction, if not equal
        INY 
        LDA     (CURADL),Y
        CMP     POINTL
        BEQ     CMND        ; Return if 2byte pattern is found

SEARA:
        JSR     OPLEN       ; Get length of the current instruction
        JSR     NEXT        ; Skip to the next instruction
        BMI     SELOOP      ; Search again, if CURAD is less than CEND
        BPL     ERRA

INSERT:
        CMP     #$10        ; Insert command ?
        BNE     INPUT
        JSR     RDINST      ; Read instruction and compute length
        BPL     SEARCH      ; COM. key?
        JSR     FILLWS      ; Move data in WS downward by the amount in bytes
        BEQ     CMND        ; Return to display the inserted instruction

INPUT:
        CMP     #$13        ; Input command ?
        BNE     SKIP
        JSR     RDINST      ; Read instruction and compute length
        BPL     SEARCH      ; COM. key ?
        JSR     OPLEN       ; Length of the current instruction
        JSR     NEXT        ; Return with N=1, if CURAD is less than CEND
        LDA     TEMPX       ; Length of instr. to be inserted
        STA     BYTES
        JSR     FILLWS      ; Move data in ws downward by the amount in bytes
        BEQ     CMND        ; Return to display the inserted data

SKIP:    
        CMP     #$12        ; Skip command ?
        BNE     DELETE
        JSR     NEXT        ; Skip to next instruction. CURAD less than CEND?
        BMI     CMND
        BPL     ERRA

DELETE:
        CMP     #$11        ; Delete command ?
        BNE     ERRA
        JSR     UP          ; Delete current instruction by moving up the WS
        JSR     RECEND      ; Adjust current end address
        JMP     CMND

ERRA:
        LDA     #$EE
        STA     POINTH
        STA     POINTL
        STA     INH
        LDA     #$03
        STA     BYTES

ERRB:
        JSR     SCANDS      ; Display "EEEEEE" until key is released
        BNE     ERRB
        JMP     CMND

;----------------------------------------------------------------------------
;
;       EDITOR'S SUBROUTINES
;
;       SCAN IS A SUBROUTINE, FILLING UP
;       THE DISPLAY BUFFER DETERMINED BY
;       CURAD.  THEN THE DISPLAY IS SCANNED
;       DEPENDING ON THE LENGTH OF THE INSTRUCTION
;       POINTED BY CURAD
;       IF A PRESSED KEY IS DETECTED
;
;----------------------------------------------------------------------------

;       SCAN RETURNS WITH VALUE IN ACCU

SCAN:
        LDX     #$02        ; Fill up the display buffer
        LDY     #$00

FILBUF:
        LDA     (CURADL),Y  ; Start filling at OPCode
        STA     INH,X
        INY 
        DEX 
        BPL     FILBUF
        JSR     OPLEN       ; Store instruction length in bytes
SCANA:
        JSR     SCANDS      ; Display current instruction
        BNE     SCANA       ; Key released ?
SCANB:
        JSR     SCANDS      ; Display current instruction
        BEQ     SCANB       ; Any key pressed
        JSR     SCANDS      ; Display current instruction
        BEQ     SCANB       ; Any key still pressed ?
        JSR     GETKEY      ; If yes, return with key in ACC
        RTS 


;----------------------------------------------------------------------------
;
;       GETBYT READS 2 HEXKEYS AND COMPOSES
;       THEIR VALUES IN THE A REGISTER. IF ONLY
;       HEXKEYS WERE PRESSED, IT RETURNS WITH
;       N=1. IF A COMMAND KEY WAS PRESSED, IT 
;       RETURNS WITH N=0.

GETBYT:
        JSR     SCANA       ; Read high order nibble
        CMP     #$10
        BPL     BYTEND      ; Command key ?
        ASL     A
        ASL     A           ; If not, save high order nibble
        ASL     A
        ASL     A
        STA     NIBBLE
        JSR     SCANA       ; Read low order nibble
        CMP     #$10
        BPL     BYTEND      ; Command key ?
        ORA     NIBBLE      ; If not, compose byte
        LDX     #$FF        ; Set N=1

BYTEND:
        RTS 


;----------------------------------------------------------------------------
;
;       SCAND IS A SUBROUTINE SHOWIND DATA SPECIFIED BY
;       POINT.
;       SCANDS IS A SUBROUTINE SHOWING THE CONTENTS OF THE 
;       DISPLAY BUFFER AS A FUNCTION OF BYTES.
;       THE FOLLOWING SUBROUTINE AK SCANS THE KEYBOARD.
;       IT RETURNS WITH A=0 IF NO KEY IS PRESSED AND
;       WITH A NOT 0 IF A KEY IS PRESSED.
;       WHEN SCAND OR SCANDS ARE LEFT, PA0..PA7 IS INPUT.
;

SCAND:
        LDY     #$00
        LDA     (POINTL),Y  ; Get data specified by point
        STA     INH

SCANDS:
        LDA     #$7F
        STA     PADD        ; PA0..PA6 is output
        LDX     #$08        ; Enable display
        LDY     BYTES       ; Fetch length from bytes
SCDSA:
        LDA     POINTH      ; Output 1st byte
        JSR     SHOW
        DEY 
        BEQ     SCDSB       ; More bytes ?
        LDA     POINTL
        JSR     SHOW        ; If yes, output 2nd byte
        DEY 
        BEQ     SCDSB       ; More bytes ?
        LDA     INH
        JSR     SHOW        ; If yes, output 3rd byte

SCDSB:
        LDA     #$00
        STA     PADD        ; PA0..PA7 is input
AK:
        LDY     #$03        ; Scan 3 rows
        LDX     #$00        ; Reset row counter

ONEKEY:
        LDA     #$FF
AKA:
        STX     PBD         ; Output row number
        INX                 ; Enable next row
        INX 
        AND     PAD         ; Input row pattern
        DEY                 ; All rows scanned ?
        BNE     AKA
        LDY     #$06        ; Turn display off
        STY     PBD
        ORA     #$80        ; Set BIT7=1
        EOR     #$FF        ; Invert key pattern
        RTS 


;----------------------------------------------------------------------------
;
;       THE SUBROUTINE SHOW TRANSPORTS THE
;       CONTENTS OF ANY DISPLAY BUFFER TO THE
;       DISPLAY. THE X REGISTER IS USED AS A
;       SCAN COUNTER. IT DETERMINES, IF POINTH,
;       POINTL OR INH IS TRANSPORTER TO THE
;       DISPLAY.
;

SHOW:
        PHA                 ; Save display 
        STY     TEMP        ; Save Y register
        LSR     A
        LSR     A           ; Get high order nibble
        LSR     A
        LSR     A
        JSR     CONVD       ; Output high order nibble
        PLA                 ; Get display again
        AND     #$0F        ; Mask off high order nibble
        JSR     CONVD       ; Output low order nibble
        LDY     TEMP
        RTS 


;----------------------------------------------------------------------------
;
;       THE SUBROUTINE CONVD CONTROLS THE DISPLAY SCAN.
;       IT CONVERTS THE CONTENTS OF THE DISPLAY BUFFER
;       TO BE DISPLAYED INTO A SEGMENT PATTERN.
;

CONVD:
        TAY                 ; Use nibble as index
        LDA     LOOK,Y      ; Fetch segment pattern
        STA     PAD         ; Output segment pattern
        STX     PBD         ; Output digit enable
        LDY     #$7F
DELAY:
        DEY                 ; Delay 500uS approx
        BPL     DELAY
        STY     PAD         ; Turns segments off
        LDY     #$06
        STY     PBD         ; Turn display off
        INX                 ; Enable next digit
        INX 
        RTS 


;----------------------------------------------------------------------------
;
;       GETKEY CONVERTS A PRESSED KEY INTO A
;       A HEX NUMBER.  IT RETURNS WITH THE KEY VALUE
;       IN ACCU.  IF AN INVALID KEY WAS PRESSED A=15.
;

GETKEY:
        LDX     #$21        ; Start at row 0
GETKEA:
        LDY     #$01        ; Get one row
        JSR     ONEKEY      ; A=0, No key pressed
        BNE     KEYIN
        CPX     #$27
        BNE     GETKEA      ; Each row scanned ?
        LDA     #$15        ; Return if invalid key
        RTS 
KEYIN:  
        LDY     #$FF
KEYINA:
        ASL     A            ; Shift left until Y=Key number
        BCS     KEYINB
        INY 
        BPL     KEYINA
KEYINB:
        TXA 
        AND     #$0F         ; Mask MSD
        LSR     A            ; Divide by 2
        TAX 
        TYA 
        BPL     KEYIND
KEYINC:
        CLC 
        ADC     #$07         ; Add row offset
KEYIND:
        DEX 
        BNE     KEYINC
        RTS 


;----------------------------------------------------------------------------
;
;       RDINST TRANSFERS AN INSTRUCTION FROM THE KEYBOARD
;       TO THE DISPLAY BUFFER.  IT RETURNS WITH N=0 IF
;       A COMMAND KEY WAS PRESSED.  ONCE THE ENTIRE
;       INSTRUCTION IS READ, RDINST RETURNS WITH N=1.
;


RDINST:
        JSR     GETBYT      ; Read OPCode
        BPL     RDB         ; Return if it is the command key
        STA     POINTH      ; Store OP cod in the display buffer
        JSR     LENACC      ; Calculate instruction length
        STY     COUNT
        STY     TEMPX
        DEC     COUNT
        BEQ     RDA         ; 1 Byte instruction ?
        JSR     GETBYT      ; If not, read first operand
        BPL     RDB         ; Return if it is the command key
        STA     POINTL      ; Store 1st operand in the display buffer
        DEC     COUNT
        BEQ     RDA         ; 2 Byte instruction ?
        JSR     GETBYT      ; If not, read second operand
        BPL     RDB         ; Return if it is the command key
        STA     INH         ; Store 2nd operand in the display buffer
RDA:
        LDX     #$FF        ; N=1
RDB:
        RTS 


;----------------------------------------------------------------------------
;
;       WILLWS TRANSFERS THE DATA FROM DISPLAY TO
;       WORKSPACE. IT's ALWAYS LEFT WITH Z=1.
;

FILLWS:
        JSR     DOWN        ; Move data down by the amount in bytes
        JSR     ADCEND      ; Adjust current end address
        LDX     #$02
        LDY     #$00
WS:
        LDA     INH,X       ; Fetch data from display buffer
        STA     (CURADL),Y  ; Insert data into the data field
        DEX 
        INY 
        CPY     BYTES       ; All inserted ?
        BNE     WS          ; If not, continue
        RTS 


;----------------------------------------------------------------------------
;
;       OPLEN CALCULATE THE LENGTH OFANY 6502 INSTRUCTION.
;       THE INSTRUCTION LENGTH IS SAVED IN BYTES.
;

OPLEN:
        LDY     #$00
        LDA     (CURADL),Y  ; Fetch OPCode from WS
LENACC:
        LDY     #$01        ; Length of the OPCode is 1 byte
        CMP     #$00
        BEQ     LENEND      ; BRK Instruction ?
        CMP     #$40
        BEQ     LENEND      ; TRI Instruction ?
        CMP     #$60
        BEQ     LENEND      ; RTS Instruction ?
        LDY     #$03
        CMP     #$20
        BEQ     LENEND      ; JSR Instruction ?
        AND     #$1F        ; Strip to 5 bits
        CMP     #$19
        BEQ     LENEND      ; Any ABS,Y instruction ?
        AND     #$0F        ; Strip to 4 bits
        TAX                 ; Use nibble as index
        LDY     LEN,X       ; Fetch length from LEN
LENEND:
        STY     BYTES       ; Discard length in bytes
        RTS 


;----------------------------------------------------------------------------
;
;       UP MOVES A DATA FIELD BETWEEN CURAD AND CEND
;       UPWARD BY THE AMOUNT IN BYTES
;

UP:      
        LDA     CURADL
        STA     MOVADL
        LDA     CURADH      ; MOVAD := CURAD
        STA     MOVADH
UPLOOP:
        LDY     BYTES
        LDA     (MOVADL),Y  ; Move upward by the number of bytes
        LDY     #$00
        STA     (MOVADL),Y
        INC     MOVADL
        BNE     UPA
        INC     MOVADH      ; MOVADH := MOVADH + 1
UPA:
        LDA     MOVADL
        CMP     CENDL
        BNE     UPLOOP      ; All data moved ?
        LDA     MOVADH      ; If not continue
        CMP     CENDH
        BNE     UPLOOP
        RTS 


;----------------------------------------------------------------------------
;
;       DOWN MOVES A DATA FIELD BETWEEN CURAD
;       AND ENDAD DOWNWARD BY TE AMOUNT IN BYTES
;

DOWN:
        LDA     CENDL
        STA     MOVADL      ; MOVAD := CEND
        LDA     CENDH
        STA     MOVADH
DNLOOP:
        LDY     #$00
        LDA     (MOVADL),Y  ; Move downward by the number of bytes
        LDY     BYTES
        STA     (MOVADL),Y
        LDA     MOVADL
        CMP     CURADL
        BNE     DNA         ; All data moved ?
        LDA     MOVADH      ; If not, continue
        CMP     CURADH
        BEQ     DNEND
DNA:
        SEC 
        LDA     MOVADL
        SBC     #$01
        STA     MOVADL
        LDA     MOVADH      ; MOVAD := MOVAD - 1
        SBC     #$00
        STA     MOVADH
        JMP     DNLOOP
DNEND:
        RTS 


;----------------------------------------------------------------------------
;
;       BEGIN SETS CURAD EQUAL TO BEGAD
;

BEGIN:
        LDA     BEGADL
        STA     CURADL
        LDA     BEGADH      ; CURAD := BEGAD
        STA     CURADH
        RTS 


;----------------------------------------------------------------------------
;
;       ADCEND ADVANCES CURRENT END ADDRESS
;       DOWNWARD BY THE NUMBER OF BYTES
;

ADCEND:
        CLC 
        LDA     CENDL
        ADC     BYTES       ; CEND := CEND + BYTES
        STA     CENDL
        LDA     CENDH
        ADC     #$00
        STA     CENDH
        RTS 


;----------------------------------------------------------------------------
;
;       RECEND REDUCES THE CURRENT END ADDRESS
;       BY THE NUMBER OF BYTES
;

RECEND:
        SEC 
        LDA     CENDL
        SBC     BYTES       ; CEND := CEND - BYTES
        STA     CENDL
        LDA     CENDH
        SBC     #$00
        STA     CENDH
        RTS 


;----------------------------------------------------------------------------
;
;       NEXT ADVANCES THE CURRENT DISPLAYED ADDRESS
;       DOWNWARD BY THE NUMBER OF BYTES
;

NEXT:
        CLC 
        LDA     CURADL
        ADC     BYTES       ; CURAD := CURAD + BYTES
        STA     CURADL
        LDA     CURADH
        ADC     #$00
        STA     CURADH
        SEC 
        LDA     CURADL
        SBC     CENDL
        LDA     CURADH
        SBC     CENDH
        RTS 


;----------------------------------------------------------------------------
;
;       THE LOOKUP TABLE "LOOK"" IS USED TO CONVERT
;       A HEX NUMBER INTO A 7-SEGMENT PATTERN.
;       THE LOOKUP TABLE "LEN" IS USED TO CONVERT AN
;       INSTRUCTION INTO AN INSTRUCTION LENGTH.
;

LOOK:
        .DB     $40         ; "0"
        .DB     $79         ; "1"
        .DB     $24         ; "2"
        .DB     $30         ; "3"
        .DB     $19         ; "4"
        .DB     $12         ; "5"
        .DB     $02         ; "6"
        .DB     $78         ; "7"
        .DB     $00         ; "8"
        .DB     $10         ; "9"
        .DB     $08         ; "A"
        .DB     $03         ; "B"
        .DB     $46         ; "C"
        .DB     $21         ; "D"
        .DB     $06         ; "E"
        .DB     $0E         ; "F"


LEN:
        .DB     $02
        .DB     $02
        .DB     $02
        .DB     $01
        .DB     $02
        .DB     $02
        .DB     $02
        .DB     $01
        .DB     $01
        .DB     $02
        .DB     $01
        .DB     $01
        .DB     $03
        .DB     $03
        .DB     $03
        .DB     $03


NMI:
        JMP     (NMIL)      ; Jump to a user selectable NMI vector
IRQ:
        JMP     (IRQL)      ; Jump to a user selectable IRQ vector


;----------------------------------------------------------------------------
;
;       GETLBL IS AN ASSEMBLER SUBROUTINE. IT SEARCHES FOR
;       LABELS ON THE SYMBOL PSEUDO STACK.  IF THIS STACK
;       CONTAINS A VALID LABEL, IT RETURNS WITH THE 
;       HIGH ORDER LABEL ADDRESS IN X AND THE LOW ORDER LABEL 
;       ADDRESS IN A.  IF NO VALID LABEL IS FOUND, IT RETURNS
;       WITH Z=1.
;


GETLBL:
        LDA     (CURADL),Y  ; Fetch current label number from WS
        LDY     #$FF        ; Reset pseudo stack
SYMA:      
        CPY     LABELS      ; Upper most symbol table address ?
        BEQ     SYMB        ; If yes, return, no label on pseudo stack
        CMP     (TABLEL),Y  ; Label Nr. in WS = Label Nr. on pseudo stack
        BNE     SYMNXT
        DEY                 ; If yes, get high order address
        LDA     (TABLEL),Y
        TAX                 ; Discard high order, add in X
        DEY 
        LDA     (TABLEL),Y  ; Get low order add
        LDY     #$01        ; Prepare Y register
SYMB:
        RTS 

SYMNXT:
        DEY                 ; *********   *********
        DEY                 ; * X=ADH *   * A=ADL *
        DEY                 ; *********   *********
        BNE     SYMA
        RTS 


;----------------------------------------------------------------------------
;
;       ASSEMBLER MAIN ROUTINE
;
;       THE FOLLOWING INSTRUCTIONS ARE ASSEMBLED:
;
;       JSR INSTRUCTION
;       JMP INSTRUCTION
;       BRANCH INSTRUCTIONS
;

ASSEMB:
        SEC 
        LDA     ENDADL
        SBC     #$FF
        STA     TABLEL      ; TABLE := ENDAD - $FF
        LDA     ENDADH
        SBC     #$00
        STA     TABLEH
        LDA     #$FF
        STA     LABELS
        JSR     BEGIN       ; CURAD := BEGAD

PASSA:
        JSR     OPLEN       ; Start pass one, get current instruction
        LDY     #$00
        LDA     (CURADL),Y  ; Fetch current instruction
        CMP     #$FF        ; Is the current instruction a label ? 
        BNE     NXTINS
        INY 
        LDA     (CURADL),Y  ; If yes, fetch label number
        LDY     LABELS
        STA     (TABLEL),Y  ; Push label number on symbol stack
        DEY 
        LDA     CURADH      ; Get high order address
        STA     (TABLEL),Y  ; Push on symbol stack
        DEY 
        LDA     CURADL      ; Get high order address
        STA     (TABLEL),Y  ; Push on symbol stack
        DEY 
        STY     LABELS      ; Adjust pseudo stack pointer
        JSR     UP          ; Delete current label in ws
        JSR     RECEND      ; Adjust current end address
        JMP     PASSA       ; Look for more labels

NXTINS:
        JSR     NEXT        ; If no label skip to the next instruction
        BMI     PASSA       ; All labels in WS collected ?
        JSR     BEGIN       ; Start pass 2
PASSB:
        JSR     OPLEN       ; Get length of the current instruction
        LDY     #$00
        LDA     (CURADL),Y  ; Fetch current instruction
        CMP     #$4C        ; JMP instruction ?
        BEQ     JUMPS
        CMP     #$20        ; JSR Instruction ?
        BEQ     JUMPS
        AND     #$1F        ; Strip to 5 bits
        CMP     #$10        ; Any branch instruction ?
        BEQ     BRINST
PB:
        JSR     NEXT        ; If not, return
        BMI     PASSB       ; All labels between CURAD and ENDAD assembled ?
        LDA     #$03        ; Enable 3 display buffers
        STA     BYTES
        JMP     START       ; Exit here

JUMPS:
        INY                 ; Set pointer to label number
        JSR     GETLBL      ; Get label address
        BEQ     PB          ; Return if not found
        STA     (CURADL),Y  ; Store low order address
        TXA 
        INY 
        STA     (CURADL),Y  ; Store high order address
        BNE     PB

BRINST:
        INY                 ; Set pointer to label number
        JSR     GETLBL      ; Get label address
        BEQ     PB          ; Return if label not found
        SEC 
        SBC     CURADL      ; Calculate branch offset
        SEC 
        SBC     #$02        ; DESTINATION - SOURCE - 2 = OFFSET
        STA     (CURADL),Y  ; Insert branch offset in WS
        JMP     PB


;----------------------------------------------------------------------------
;
;       THE SUBROUTINE BRANCH CALCULATES THE OFFSET OF BRANCH
;       INSTRUCTIONS.  THE 2 RIGHT HAND DISPLAYS SHOW THE 
;       CALCULATED OFFSET DEFINED BY THE 4 LEFT HAND DISPLAYS.
;       THE PROGRAM MUST BE STOPPED BY THE RESET KEY.
;

BRANCH:
        CLD 
        LDA     #$00        ; Reset display buffer
        STA     POINTH
        STA     POINTL
        STA     INH
BR:
        JSR     GETBYT      ; Read source
        BPL     BRANCH      ; Command key ?
        STA     POINTH      ; Save source in buffer
        JSR     GETBYT      ; Read destination
        BPL     BRANCH      ; Command key ?
        STA     POINTL      ; Save destination in buffer
        CLC 
        LDA     POINTL      ; Fetch destination
        SBC     POINTH      ; Substract source
        STA     INH
        DEC     INH         ; Equalize and save offset in buffer
        JMP     BR


;----------------------------------------------------------------------------
;
;       VECTORS AT THE END OF THE MEMORY
;
.DW     $FFFF               ; Fill

NMI_VECTOR:     .DW   NMI

RESET_VECTOR:   .DW   RESET

IRQ_BRK_VECTOR: .DW   IRQ

;----------------------------------------------------------------------------
;
;       END OF JUNIOR'S MONITOR

.END
