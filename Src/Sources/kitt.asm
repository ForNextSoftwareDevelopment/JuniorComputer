;----------------------------------------------------------------------------
; Test program for the Junior Computer Assembler/Simulator
; Just a dash going from one side of the display to another
; KITT (Night Rider)
; Dirk Prins, 2023
;----------------------------------------------------------------------------

;       MEMORY LOCATIONS IN THE 6532-IC
PAD:    .EQU $1A80      ; DATA REGISTER OF PORT A
PADD:   .EQU $1A81      ; DATA DIRECTION REGISTER OF PORT A
PBD:    .EQU $1A82      ; DATA REGISTER OF PORT B
PBDD:   .EQU $1A83      ; DATA DIRECTION REGISTER OF PORT B

        .ORG $0100
        LDA #$7F        ; PA0..PA6 is output
        STA PADD        
        LDA #$1E        ; PB1..PB4 is output
        STA PBDD       

KITT:   LDX #$08
        LDA #$BF
        JSR OUT
        LDX #$0A
        LDA #$BF
        JSR OUT
        LDX #$0C
        LDA #$BF
        JSR OUT
        LDX #$0E
        LDA #$BF
        JSR OUT
        LDX #$10
        LDA #$BF
        JSR OUT
        LDX #$12
        LDA #$BF
        JSR OUT

        LDX #$12
        LDA #$BF
        JSR OUT
        LDX #$10
        LDA #$BF
        JSR OUT
        LDX #$0E
        LDA #$BF
        JSR OUT
        LDX #$0C
        LDA #$BF
        JSR OUT
        LDX #$0A
        LDA #$BF
        JSR OUT
        LDX #$08
        LDA #$BF
        JSR OUT
        JMP KITT

;----------------------------------------------------------------------------
;
;       THE SUBROUTINE OUTPUTS A - TO THE DISPLAY
;       AT THE LOCATION INDICATED BY REGISTER X
;

OUT:
        STA     PAD         ; Output segment pattern
        STX     PBD         ; Output digit enable one display unit
        LDY     #$08
DELAY1: LDX     #$00        ; Delay
DELAY2: DEX     
        BPL     DELAY2
        DEY                 
        BPL     DELAY1
        STY     PAD         ; Turns segments off
        STY     PBD         
        RTS 

        .END
